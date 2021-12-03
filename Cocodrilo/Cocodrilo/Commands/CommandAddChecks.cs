using System;
using Rhino;
using Rhino.Commands;
using Cocodrilo.UserData;
using System.Collections.Generic;
using Cocodrilo.ElementProperties;

namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("72E1DD4B-4D71-4CB1-979C-41B0C3A0CE2D")]
    public class CommandAddChecks : Command
    {
        static CommandAddChecks _instance;
        public CommandAddChecks()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandAddChecks command.</summary>
        public static CommandAddChecks Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_AddChecks"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var check_properties = Panels.UserControlCocodriloPanel.Instance.getCheckProperties();
            var time_interval = Panels.UserControlCocodriloPanel.Instance.GetTimeInterval();
            var overwrite_check = false;// Panels.UserControlCocodriloPanel.Instance.getOverwriteCheck();

            GeometryType GeometryTypeSelected = Panels.UserControlCocodriloPanel.Instance.getGeometryTypeCheck();
            switch (GeometryTypeSelected)
            {
                case GeometryType.GeometrySurface:
                    if (CommandUtilities.TryGetUserDataSurface(out List<UserDataSurface> UserDataSurfaceList))
                    {
                        var property_check = new PropertyCheck(
                            GeometryTypeSelected,
                            check_properties,
                            false,
                            time_interval);
                        foreach (var user_data_surface in UserDataSurfaceList)
                        {
                            user_data_surface.AddNumericalElement(
                                property_check,
                                overwrite_check);
                        }
                    }
                    break;
                //case GeometryType.SurfaceEdge:
                    // Not Supportet yet
                    //if (!is_support_strong)
                    //{
                    //    if (CommandUtilities.TryGetUserDataEdge(out List<UserDataEdge> UserDataEdgeList))
                    //    {
                    //        var property_support = new PropertySupport(
                    //            GeometryTypeSelected, support, time_interval);

                    //        foreach (var user_data_edge in UserDataEdgeList)
                    //            user_data_edge.AddBrepElementEdge(
                    //                property_support,
                    //                overwrite_support);
                    //    }
                    //}
                    //else
                    //{
                    //    if (CommandUtilities.TryGetUserDataAndParameterEdgeOnSurface(
                    //        out List<Tuple<UserDataSurface, ParameterLocationSurface>>
                    //            UserDataSurfaceParameterLocationList))
                    //    {
                    //        foreach (var user_data in UserDataSurfaceParameterLocationList)
                    //        {
                    //            var property_support = new PropertySupport(
                    //                GeometryTypeSelected,
                    //                support,
                    //                time_interval,
                    //                (int) user_data.Item2.mU_Normalized,
                    //                (int) user_data.Item2.mV_Normalized);

                    //            user_data.Item1.AddGeometryElementSurface(
                    //                property_support,
                    //                user_data.Item2,
                    //                overwrite_support);
                    //        }
                    //    }
                    //}
                    //break;
                case GeometryType.SurfacePoint:
                    if (CommandUtilities.TryGetUserDataAndParmeterVertexOnSurface(
                        out List<Tuple<UserDataSurface, Elements.ParameterLocationSurface>>
                            UserDataSurfaceParameterLocationDictionary))
                    {
                        foreach (var user_data in UserDataSurfaceParameterLocationDictionary)
                        {
                            if (user_data.Item2.IsPoint())
                            {
                                if (user_data.Item2.IsOnNodes())
                                {
                                    var property_check = new PropertyCheck(
                                        GeometryTypeSelected,
                                        check_properties,
                                        true,
                                        time_interval);

                                    user_data.Item1.AddNumericalElement(
                                        property_check,
                                        user_data.Item2,
                                        overwrite_check);
                                }
                                else
                                {
                                    var property_check = new PropertyCheck(
                                        GeometryTypeSelected,
                                        check_properties,
                                        false,
                                        time_interval);

                                    user_data.Item1.AddNumericalElement(
                                        property_check,
                                        user_data.Item2,
                                        overwrite_check);
                                }
                            }
                        }
                    }
                    break;
                //case GeometryType.GeometryCurve:
                //    if (CommandUtilities.TryGetUserDataCurve(out List<UserDataCurve> UserDataCurveList))
                //    {
                //        var property_support = new PropertySupport(
                //            GeometryType.GeometryCurve,
                //            support,
                //            time_interval);
                //        foreach (var user_data_curve in UserDataCurveList)
                //            user_data_curve.AddGeometryElementCurve(
                //                property_support,
                //                overwrite_support);
                //    }
                //    break;
                //case GeometryType.CurveVertex:
                //    if (CommandUtilities.TryGetUserDataAndParameterVertexOnCurve(
                //        out List<Tuple<UserDataCurve, ParameterLocationCurve>>
                //            UserDataCurveParameterLocationList))
                //    {
                //        foreach (var user_data in UserDataCurveParameterLocationList)
                //        {
                //            if (user_data.Item2.IsPoint())
                //            {
                //                if (is_support_strong && user_data.Item2.IsOnNodes())
                //                {
                //                    var property_support = new PropertySupport(
                //                        GeometryType.CurveVertex,
                //                        support,
                //                        time_interval,
                //                        (int) user_data.Item2.mU_Normalized);

                //                    user_data.Item1.AddGeometryElementCurve(
                //                        property_support,
                //                        user_data.Item2,
                //                        overwrite_support);
                //                }
                //                else
                //                {
                //                    var property_support = new PropertySupport(
                //                        GeometryType.CurveVertex,
                //                        support,
                //                        time_interval);

                //                    user_data.Item1.AddBrepElementCurveVertex(
                //                        property_support,
                //                        user_data.Item2,
                //                        overwrite_support);
                //                }
                //            }
                //        }
                //    }
                //    break;
            }

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}
