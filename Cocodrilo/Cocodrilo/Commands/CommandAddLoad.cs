using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Cocodrilo.UserData;
using Cocodrilo.ElementProperties;

namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("1dcef267-cb9b-4a33-bcae-b44144df05b8")]
    public class CommandAddLoad : Command
    {
        static CommandAddLoad _instance;
        public CommandAddLoad()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandAddLoad command.</summary>
        public static CommandAddLoad Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_AddLoad"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var time_interval = Panels.UserControlCocodriloPanel.Instance.GetTimeInterval();
            bool overwrite = Panels.UserControlCocodriloPanel.Instance.getLoadBoolOverwrite();

            var GeometyTypeSelected = Panels.UserControlCocodriloPanel.Instance.getGeometryTypeLoad();

            switch (GeometyTypeSelected)
            {
                case GeometryType.GeometrySurface:
                    if (CommandUtilities.TryGetUserDataSurface(out List<UserDataSurface> UserDataSurfaceList))
                    {
                        var load = Panels.UserControlCocodriloPanel.Instance.getLoad();
                        var parameter_location = new Elements.ParameterLocationSurface(GeometryType.GeometrySurface, - 1, -1, -1, -1);
                        var property_load = new PropertyLoad(GeometryType.GeometrySurface, load, time_interval);
                        foreach (var user_data_surface in UserDataSurfaceList)
                        {
                            user_data_surface.AddNumericalElement(
                                property_load,
                                parameter_location,
                                overwrite);
                        }
                    }
                    break;
                case GeometryType.SurfaceEdge:
                    if (CommandUtilities.TryGetUserDataEdge(out List<UserDataEdge> UserDataEdgeList))
                    {
                        var load = Panels.UserControlCocodriloPanel.Instance.getLoad();
                        var property_load = new PropertyLoad(GeometryType.SurfaceEdge, load, time_interval);

                        foreach (var user_data_edge in UserDataEdgeList)
                            user_data_edge.AddNumericalElement(
                                property_load,
                                overwrite);
                    }
                    break;
                case GeometryType.SurfacePoint:
                    if (CommandUtilities.TryGetUserDataAndParmeterVertexOnSurface(
                        out List<Tuple<UserDataSurface, Elements.ParameterLocationSurface>>
                            UserDataSurfaceParameterLocationDictionary))
                    {
                        foreach (var user_data_tuple in UserDataSurfaceParameterLocationDictionary)
                        {
                            if (user_data_tuple.Item2.IsPoint())
                            {
                                var load = Panels.UserControlCocodriloPanel.Instance.getLoad();
                                var property_load = new PropertyLoad(
                                    GeometryType.SurfacePoint,
                                    load,
                                    time_interval);
                                user_data_tuple.Item1.AddNumericalElement(
                                    property_load,
                                    user_data_tuple.Item2,
                                    overwrite);
                            }
                        }
                    }
                    break;
                case GeometryType.GeometryCurve:
                    if (CommandUtilities.TryGetUserDataCurve(out List<UserDataCurve> UserDataCurveList))
                    {
                        var load = Panels.UserControlCocodriloPanel.Instance.getLoad();
                        var parameter_location = new Elements.ParameterLocationCurve(GeometryType.GeometryCurve, - 1, -1);
                        var property_load = new PropertyLoad(GeometryType.GeometryCurve, load, time_interval);
                        foreach (var user_data_curve in UserDataCurveList)
                        {
                            user_data_curve.AddNumericalElement(
                                property_load,
                                parameter_location,
                                overwrite);
                        }
                    }
                    break;
                case GeometryType.CurvePoint:
                    if (CommandUtilities.TryGetUserDataAndParameterVertexOnCurve(
                        out List<Tuple<UserDataCurve, Elements.ParameterLocationCurve>>
                            UserDataCurveParameterLocationList))
                    {
                        var load = Panels.UserControlCocodriloPanel.Instance.getLoad();
                        var property_load = new PropertyLoad(GeometryType.CurvePoint, load, time_interval);
                        foreach (var user_data in UserDataCurveParameterLocationList)
                        {
                            if (user_data.Item2.IsPoint())
                            {
                                user_data.Item1.AddNumericalElement(
                                    property_load,
                                    user_data.Item2,
                                    overwrite);
                            }
                        }
                    }
                    break;
            }

            return Result.Success;
        }
    }
}
