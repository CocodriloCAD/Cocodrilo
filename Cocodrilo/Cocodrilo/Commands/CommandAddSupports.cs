using System;
using Rhino;
using Rhino.Commands;
using Cocodrilo.UserData;
using System.Collections.Generic;
using Cocodrilo.ElementProperties;

namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("ee5334ed-c05e-473b-9bfa-0f7a87daf34b")]
    public class CommandAddSupports : Command
    {
        static CommandAddSupports _instance;
        public CommandAddSupports()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandAddSupports command.</summary>
        public static CommandAddSupports Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_AddSupports"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var support = Panels.UserControlCocodriloPanel.Instance.getSupport();
            var time_interval = Panels.UserControlCocodriloPanel.Instance.GetTimeInterval();
            var overwrite_support = Panels.UserControlCocodriloPanel.Instance.getOverwriteSupport();
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
                            time_interval);
                        foreach (var user_data_surface in UserDataSurfaceList)
                        {
                            user_data_surface.AddNumericalElement(
                                property_support,
                                overwrite_support);
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
                                user_data_edge.AddNumericalElement(
                                    property_support,
                                    overwrite_support);
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
                                    time_interval);

                                user_data.Item1.AddNumericalElement(
                                    property_support,
                                    user_data.Item2,
                                    overwrite_support);
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
                                        GeometryTypeSelected, support, time_interval);

                                    user_data.Item1.AddNumericalElement(
                                        property_support,
                                        user_data.Item2,
                                        overwrite_support);
                                }
                                else
                                {
                                    var property_support = new PropertySupport(
                                        GeometryTypeSelected,
                                        support,
                                        time_interval);
                                    user_data.Item1.AddNumericalElement(
                                        property_support,
                                        user_data.Item2,
                                        overwrite_support);
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
                        foreach (var user_data_curve in UserDataCurveList)
                            user_data_curve.AddNumericalElement(
                                property_support,
                                overwrite_support);
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
                                        time_interval);

                                    user_data.Item1.AddNumericalElement(
                                        property_support,
                                        user_data.Item2,
                                        overwrite_support);
                                }
                                else
                                {
                                    var property_support = new PropertySupport(
                                        GeometryType.CurvePoint,
                                        support,
                                        time_interval);

                                    user_data.Item1.AddNumericalElement(
                                        property_support,
                                        user_data.Item2,
                                        overwrite_support);
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
