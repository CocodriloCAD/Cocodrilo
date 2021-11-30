using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Cocodrilo.ElementProperties;
using Cocodrilo.UserData;

namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("d430e9a2-b7a2-44a5-8601-eb6fd46b0a9a")]
    public class CommandDeleteSupports : Command
    {
        static CommandDeleteSupports _instance;
        public CommandDeleteSupports()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandDeleteSupports command.</summary>
        public static CommandDeleteSupports Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_DeleteSupports"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var support = Panels.UserControlCocodriloPanel.Instance.getSupport();
            var time_interval = Panels.UserControlCocodriloPanel.Instance.GetTimeInterval();
            bool is_support_strong = Panels.UserControlCocodriloPanel.Instance.getIsSupportStrong();

            GeometryType GeometryTypeSelected = Panels.UserControlCocodriloPanel.Instance.getGeometryTypeSupport();
            switch (GeometryTypeSelected)
            {
                case GeometryType.GeometrySurface:
                    if (CommandUtilities.TryGetUserDataSurface(out List<UserDataSurface> UserDataSurfaceList))
                    {
                        var property_support = new PropertySupport(
                            GeometryTypeSelected,
                            support,
                            time_interval,
                            -1, -1);
                        foreach (var user_data_surface in UserDataSurfaceList)
                        {
                            var parameter_location = new Elements.ParameterLocationSurface(-1, -1, -1, -1);
                            user_data_surface.DeleteGeometryElementSurface(
                                property_support,
                                parameter_location);
                        }
                    }
                    break;
                case GeometryType.SurfaceEdge:
                    if (!is_support_strong)
                    {
                        if (CommandUtilities.TryGetUserDataEdge(out List<UserDataEdge> UserDataEdgeList))
                        {
                            var property_support = new PropertySupport(
                                GeometryTypeSelected, support, time_interval);

                            foreach (var user_data_edge in UserDataEdgeList)
                                user_data_edge.DeleteBrepElementEdge(
                                    property_support);
                        }
                    }
                    else
                    {
                        if (CommandUtilities.TryGetUserDataAndParameterEdgeOnSurface(
                            out List<Tuple<UserDataSurface, Elements.ParameterLocationSurface>>
                                UserDataSurfaceParameterLocationList))
                        {
                            foreach (var user_data in UserDataSurfaceParameterLocationList)
                            {
                                var property_support = new PropertySupport(
                                    GeometryTypeSelected,
                                    support,
                                    time_interval,
                                    (int) user_data.Item2.mU_Normalized,
                                    (int) user_data.Item2.mV_Normalized);

                                user_data.Item1.DeleteGeometryElementSurface(
                                    property_support,
                                    user_data.Item2);
                            }
                        }
                    }
                    break;
                case GeometryType.SurfacePoint:
                    if (CommandUtilities.TryGetUserDataAndParmeterVertexOnSurface(
                        out List<Tuple<UserDataSurface, Elements.ParameterLocationSurface>>
                            UserDataSurfaceParameterLocationDictionary))
                    {
                        foreach (var user_data in UserDataSurfaceParameterLocationDictionary)
                        {
                            if (user_data.Item2.IsPoint())
                            {
                                if (is_support_strong && user_data.Item2.IsOnNodes())
                                {
                                    var property_support = new PropertySupport(
                                        GeometryTypeSelected, support, time_interval,
                                        (int) user_data.Item2.mU_Normalized,
                                        (int) user_data.Item2.mV_Normalized);

                                    user_data.Item1.DeleteGeometryElementSurface(
                                        property_support,
                                        user_data.Item2);
                                }
                                else
                                {
                                    var property_support = new PropertySupport(
                                        GeometryTypeSelected,
                                        support,
                                        time_interval);
                                    user_data.Item1.DeleteBrepElementSurfaceVertex(
                                        property_support,
                                        user_data.Item2);
                                }
                            }
                        }
                    }
                    break;
                case GeometryType.GeometryCurve:
                    if (CommandUtilities.TryGetUserDataCurve(out List<UserDataCurve> UserDataCurveList))
                    {
                        var property_support = new PropertySupport(
                            GeometryType.GeometryCurve,
                            support,
                            time_interval);

                        var parameter_location_curve = new Elements.ParameterLocationCurve(-1, -1);
                        foreach (var user_data_curve in UserDataCurveList)
                            user_data_curve.DeleteGeometryElementCurve(
                                property_support,
                                parameter_location_curve);
                    }
                    break;
                case GeometryType.CurvePoint:
                    if (CommandUtilities.TryGetUserDataAndParameterVertexOnCurve(
                        out List<Tuple<UserDataCurve, Elements.ParameterLocationCurve>>
                            UserDataCurveParameterLocationList))
                    {
                        foreach (var user_data in UserDataCurveParameterLocationList)
                        {
                            if (user_data.Item2.IsPoint())
                            {
                                if (is_support_strong && user_data.Item2.IsOnNodes())
                                {
                                    var property_support = new PropertySupport(
                                        GeometryType.CurvePoint,
                                        support,
                                        time_interval,
                                        (int) user_data.Item2.mU_Normalized);

                                    user_data.Item1.DeleteGeometryElementCurve(
                                        property_support,
                                        user_data.Item2);
                                }
                                else
                                {
                                    var property_support = new PropertySupport(
                                        GeometryType.CurvePoint,
                                        support,
                                        time_interval);

                                    user_data.Item1.DeleteBrepElementCurveVertex(
                                        property_support,
                                        user_data.Item2);
                                }
                            }
                        }
                    }
                    break;
            }

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}
