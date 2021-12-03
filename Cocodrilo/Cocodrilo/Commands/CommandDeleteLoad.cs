using System;
using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Cocodrilo.UserData;
using Cocodrilo.ElementProperties;


namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("3FF0F7B4-BA8E-416B-9043-7876BFFBA9E4")]
    public class CommandDeleteLoad : Command
    {
        static CommandDeleteLoad _instance;
        public CommandDeleteLoad()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandDeleteLoad command.</summary>
        public static CommandDeleteLoad Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_DeleteLoad"; }
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
                            user_data_surface.DeleteNumericalElement(
                                property_load,
                                parameter_location);
                        }
                    }
                    break;
                case GeometryType.SurfaceEdge:
                    if (CommandUtilities.TryGetUserDataEdge(out List<UserDataEdge> UserDataEdgeList))
                    {
                        var load = Panels.UserControlCocodriloPanel.Instance.getLoad();
                        var property_load = new PropertyLoad(GeometryType.SurfaceEdge, load, time_interval);

                        foreach (var user_data_edge in UserDataEdgeList)
                            user_data_edge.DeleteNumericalElement(
                                property_load);
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
                                user_data_tuple.Item1.DeleteNumericalElement(
                                    property_load,
                                    user_data_tuple.Item2);
                            }
                        }
                    }
                    break;
                case GeometryType.GeometryCurve:
                    if (CommandUtilities.TryGetUserDataCurve(out List<UserDataCurve> UserDataCurveList))
                    {
                        var load = Panels.UserControlCocodriloPanel.Instance.getLoad();
                        var parameter_location = new Elements.ParameterLocationCurve(GeometryType.GeometrySurface, - 1, -1);
                        var property_load = new PropertyLoad(GeometryType.GeometrySurface, load, time_interval);
                        foreach (var user_data_curve in UserDataCurveList)
                        {
                            user_data_curve.DeleteNumericalElement(
                                property_load,
                                parameter_location);
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
                                user_data.Item1.DeleteNumericalElement(
                                    property_load,
                                    user_data.Item2);
                            }
                        }
                    }
                    break;
            }

  
            return Result.Success;
        }
    }
}
