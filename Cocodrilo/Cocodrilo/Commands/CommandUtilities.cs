using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using System;
using System.Collections.Generic;
using System.Linq;
//using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using Cocodrilo.ElementProperties;
using Cocodrilo.UserData;

namespace Cocodrilo.Commands
{
    /// <summary>
    /// This class provides utilities for the selection of specific geometry items
    /// for an optimized and unified creation of commands.
    /// </summary>
    public static class CommandUtilities
    {
        public static bool TryGetUserDataSurface(out List<UserDataSurface> UserDataSurfaceList)
        {
            UserDataSurfaceList = new List<UserDataSurface>();

            var rcSurface = RhinoGet.GetMultipleObjects(
                "Select Surfaces...",
                false,
                ObjectType.Surface,
                out var objrefSurface);

            if (rcSurface != Result.Success)
                return false;

            // See if user data of my custom type is attached to the geomtry
            foreach (var surface in objrefSurface)
            {
                var ud = UserDataUtilities.GetOrCreateUserDataSurface(
                    surface.Brep().Surfaces[surface.Face().FaceIndex]);

                UserDataSurfaceList.Add(ud);
            }

            return true;
        }

        public static bool TryGetUserDataEdge(out List<UserDataEdge> UserDataEdgeList)
        {
            UserDataEdgeList = new List<UserDataEdge>();

            var rc = RhinoGet.GetMultipleObjects(
                "Select Edges...",
                false,
                ObjectType.Curve,
                out var objref);

            if (rc != Result.Success || objref == null)
                return false;



            foreach (var curve in objref)
            {
                var edge = curve.Edge();
                var trimIndex = edge.TrimIndices()[0];
                var trim = edge.Brep.Trims[trimIndex];

                UserDataEdgeList.Add(
                    UserDataUtilities.GetOrCreateUserDataEdge(
                        curve.Brep().Curves2D[trim.TrimCurveIndex]));
            }

            return true;
        }

        public static bool TryGetUserDataAndParameterEdgeOnSurface(
            out List<Tuple<UserDataSurface, Elements.ParameterLocationSurface>> UserDataSurfaceParameterLocationList)
        {
            UserDataSurfaceParameterLocationList = new List<Tuple<UserDataSurface, Elements.ParameterLocationSurface>>();

            var filter = ObjectType.Curve;
            ObjRef[] objref = null;
            var rc = RhinoGet.GetMultipleObjects("Select Edges...", false, filter, out objref);

            if (rc != Result.Success || objref == null)
                return false;

            foreach (var curve in objref)
            {
                var brep = curve.Brep();
                var edge = curve.Edge();
                var trim_index = edge.TrimIndices()[0];
                var trim_curve = brep.Trims[trim_index];
                var nurbs_surface = brep.Surfaces[trim_curve.Face.FaceIndex].ToNurbsSurface();

                if (IO.GeometryUtilities.TryGetParamaterLocationSurface(trim_curve, nurbs_surface, out Elements.ParameterLocationSurface parameter_location))
                {
                    var user_data_surface = UserDataUtilities.GetOrCreateUserDataSurface(brep.Surfaces[trim_curve.Face.FaceIndex]);

                    UserDataSurfaceParameterLocationList.Add(
                        new Tuple<UserDataSurface, Elements.ParameterLocationSurface>(user_data_surface, parameter_location));
                }
                else
                {
                    RhinoApp.WriteLine("Selected edge is not suitable for strong boundary conditions. Parameters are as following: U_Norm: "
                        + parameter_location.mU_Normalized + "V_Norm: " + parameter_location.mV_Normalized);
                }
            }

            return true;
        }

        public static bool TryGetUserDataCurve(out List<UserDataCurve> UserDataCurveList)
        {
            UserDataCurveList = new List<UserDataCurve>();

            ObjRef[] objrefCurve = null;
            var rcCurve = RhinoGet.GetMultipleObjects("Select Curves...", false, ObjectType.Curve, out objrefCurve);
            if (rcCurve != Result.Success)
                return false;

            // See if user data of my custom type is attached to the geomtry
            foreach (var curve in objrefCurve)
            {
                var user_data_curve = curve.Curve().UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                if (user_data_curve == null)
                {
                    user_data_curve = new UserData.UserDataCurve();
                    curve.Curve().UserData.Add(user_data_curve);
                    RhinoApp.WriteLine("New curve user data added.");
                }
                UserDataCurveList.Add(user_data_curve);
            }
            return true;
        }

        public static bool TryGetUserDataAndParmeterVertexOnSurface(
            out List<Tuple<UserDataSurface, Elements.ParameterLocationSurface>> UserDataSurfaceParameterLocationDictionary)
        {
            UserDataSurfaceParameterLocationDictionary = new List<Tuple<UserDataSurface, Elements.ParameterLocationSurface>>();

            var rc_surface = RhinoGet.GetOneObject("Select Surface...", 
                false, 
                ObjectType.Surface, 
                out var objref_surface);

            if (rc_surface != Result.Success)
                return false;

            var user_data_surface = UserDataUtilities.GetOrCreateUserDataSurface(
                objref_surface.Brep().Surfaces[objref_surface.Face().FaceIndex]);
            //if (user_data_surface == null)
            //{
            //    // No user data found; create one and add it
            //    if (rc_surface != Result.Success)
            //        return false;

            //    user_data_surface = new UserData.UserDataSurface();
            //    objref_surface.Brep().Surfaces[objref_surface.Face().FaceIndex].UserData.Add(user_data_surface);
            //    RhinoApp.WriteLine("New surface user data added.");
            //}

            //// get the selected face
            var face = objref_surface.Face();
            if (face == null)
                return false;

            // pick a point on the surface.  Constrain
            // picking to the face.
            while (true)
            {
                var gp = new Rhino.Input.Custom.GetPoint();
                gp.SetCommandPrompt("Select points on surface...");
                gp.Constrain(face, false);
                gp.Get();
                if (gp.CommandResult() != Result.Success)
                    break;

                double u, v = 0;
                face.ClosestPoint(gp.Point(), out u, out v);

                var parameter_location = new Elements.ParameterLocationSurface(u, v, -1, -1);

                var nurbs_surface = face.ToNurbsSurface();

                if (Math.Abs(nurbs_surface.KnotsU[0] - parameter_location.mU) < 1e-5)
                    parameter_location.mU_Normalized = 0;
                else if (Math.Abs(nurbs_surface.KnotsU[nurbs_surface.KnotsU.Count - 1] - parameter_location.mU) < 1e-5)
                    parameter_location.mU_Normalized = 1;
                else if (nurbs_surface.KnotsU[0] < parameter_location.mU
                    && nurbs_surface.KnotsU[nurbs_surface.KnotsU.Count - 1] > parameter_location.mU)
                {
                    parameter_location.mU_Normalized = nurbs_surface.KnotsU[0] + parameter_location.mU
                        / Math.Abs(nurbs_surface.KnotsU[0] - nurbs_surface.KnotsU[nurbs_surface.KnotsU.Count - 1]);
                }
                else if (nurbs_surface.KnotsU[0] > parameter_location.mU
                    && nurbs_surface.KnotsU[nurbs_surface.KnotsU.Count - 1] < parameter_location.mU)
                {
                    parameter_location.mU_Normalized = nurbs_surface.KnotsU[nurbs_surface.KnotsU.Count - 1] + parameter_location.mU
                        / Math.Abs(nurbs_surface.KnotsU[0] - nurbs_surface.KnotsU[nurbs_surface.KnotsU.Count - 1]);
                }

                if (Math.Abs(nurbs_surface.KnotsV[0] - parameter_location.mV) < 1e-5)
                    parameter_location.mV_Normalized = 0;
                else if (Math.Abs(nurbs_surface.KnotsV[nurbs_surface.KnotsV.Count - 1] - parameter_location.mV) < 1e-5)
                    parameter_location.mV_Normalized = 1;
                else if (nurbs_surface.KnotsV[0] < parameter_location.mV
                    && nurbs_surface.KnotsV[nurbs_surface.KnotsV.Count - 1] > parameter_location.mV)
                {
                    parameter_location.mV_Normalized = nurbs_surface.KnotsV[0] + parameter_location.mV
                        / Math.Abs(nurbs_surface.KnotsV[0] - nurbs_surface.KnotsV[nurbs_surface.KnotsV.Count - 1]);
                }
                else if (nurbs_surface.KnotsV[0] > parameter_location.mV
                    && nurbs_surface.KnotsV[nurbs_surface.KnotsV.Count - 1] < parameter_location.mV)
                {
                    parameter_location.mV_Normalized = nurbs_surface.KnotsV[nurbs_surface.KnotsV.Count - 1] + parameter_location.mV
                        / Math.Abs(nurbs_surface.KnotsV[0] - nurbs_surface.KnotsV[nurbs_surface.KnotsV.Count - 1]);
                }


                UserDataSurfaceParameterLocationDictionary.Add(
                    new Tuple<UserDataSurface, Elements.ParameterLocationSurface>(user_data_surface, parameter_location));
            }
            return true;
        }

        public static bool TryGetUserDataAndParameterVertexOnCurve(
            out List<Tuple<UserDataCurve, Elements.ParameterLocationCurve>> UserDataSurfaceParameterLocationList)
        {
            UserDataSurfaceParameterLocationList = new List<Tuple<UserDataCurve, Elements.ParameterLocationCurve>>();
            
            // select a curve
            var gs = new Rhino.Input.Custom.GetObject();
            gs.SetCommandPrompt("Select curve...");
            gs.GeometryFilter = ObjectType.Curve;
            gs.DisablePreSelect();
            gs.SubObjectSelect = true;
            gs.Get();
            if (gs.CommandResult() != Result.Success)
                return false;
            // get the selected face
            var crv = gs.Object(0).Curve();
            if (crv == null)
                return false;

            var curve = gs.Object(0);
            //face.ToBrep().UserData
            var user_data_curve =
                curve.Curve().UserData.Find(typeof(UserDataCurve)) as
                    UserDataCurve;
            if (user_data_curve == null)
            {
                user_data_curve = new UserDataCurve();
                curve.Curve().UserData.Add(user_data_curve);
                RhinoApp.WriteLine("New curve user data added");
                //new Exception();
            }

            // pick a point on the surface.  Constrain
            // picking to the face.
            while (true)
            {
                var gp = new Rhino.Input.Custom.GetPoint();
                gp.SetCommandPrompt("Select point on curve...");
                gp.Constrain(crv, false);
                gp.Get();
                if (gp.CommandResult() != Result.Success)
                    break;
                double u;
                crv.ClosestPoint(gp.Point(), out u);
                crv.NormalizedLengthParameter(u, out double u_normalized);

                var parameter_location = new Elements.ParameterLocationCurve(u, u_normalized);

                var nurbs_crv = crv.ToNurbsCurve();

                if (Math.Abs(nurbs_crv.Knots[0] - u) < 1e-5)
                    parameter_location.mU_Normalized = 0;
                else if (Math.Abs(nurbs_crv.Knots[nurbs_crv.Knots.Count - 1] - u) < 1e-5)
                    parameter_location.mU_Normalized = 1;

                UserDataSurfaceParameterLocationList.Add(
                    new Tuple<UserDataCurve, Elements.ParameterLocationCurve>(user_data_curve, parameter_location));
            }
            return true;
        }
    }
}
