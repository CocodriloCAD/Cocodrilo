using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Cocodrilo.ElementProperties;
using Cocodrilo.UserData;

namespace Cocodrilo.Commands
{
    public class CommandDeleteElementFormulation : Command
    {
        static CommandDeleteElementFormulation _instance;
        public CommandDeleteElementFormulation()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandDeleteElementFormulation command.</summary>
        public static CommandDeleteElementFormulation Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_DeleteElementFormulation"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            string ElementType = Panels.UserControlCocodriloPanel.Instance.getSelectedElementType();

            if (ElementType == "Membrane")
            {
                if(CommandUtilities.TryGetUserDataSurface(
                    out List<UserDataSurface> UserDataSurfaceList))
                {
                    foreach (var user_data_surface in UserDataSurfaceList)
                    {
                        user_data_surface.DeleteGeometryElementSurfaceOfPropertyType(
                            typeof(PropertyMembrane));
                    }
                }
            }
            else if (ElementType == "Shell")
            {
                if (CommandUtilities.TryGetUserDataSurface(
                    out List<UserDataSurface> UserDataSurfaceList))
                {
                    foreach (var user_data_surface in UserDataSurfaceList)
                    {
                        user_data_surface.DeleteGeometryElementSurfaceOfPropertyType(
                            typeof(PropertyShell));
                    }
                }
            }
            else if (ElementType == "Cable")
            {
                var topology_type = Panels.UserControlCocodriloPanel.Instance.getCableTopologyType();
                if (topology_type == GeometryType.SurfaceEdge)
                {
                    if (CommandUtilities.TryGetUserDataEdge(
                        out List<UserDataEdge> UserDataEdgeList))
                    {
                        foreach (var user_data_edge in UserDataEdgeList)
                        {
                            user_data_edge.DeleteBrepElementEdgePropertyType(
                                typeof(PropertyCable));
                        }
                    }
                }
                if (topology_type == GeometryType.GeometryCurve)
                {
                    if (CommandUtilities.TryGetUserDataCurve(
                        out List<UserDataCurve> UserDataCurveList))
                    {
                        foreach (var user_data_curve in UserDataCurveList)
                        {
                            user_data_curve.DeleteGeometryElementCurveOfPropertyType(
                                typeof(PropertyCable));
                        }
                    }
                }
            }
            else if (ElementType == "Beam")
            {
                var topology_type = Panels.UserControlCocodriloPanel.Instance.getCableTopologyType();
                if (topology_type == GeometryType.SurfaceEdge)
                {
                    if (CommandUtilities.TryGetUserDataEdge(
                        out List<UserDataEdge> UserDataEdgeList))
                    {
                        foreach (var user_data_edge in UserDataEdgeList)
                        {
                            user_data_edge.DeleteBrepElementEdgePropertyType(
                                typeof(PropertyBeam));
                        }
                    }
                }
                if (topology_type == GeometryType.GeometryCurve)
                {
                    if (CommandUtilities.TryGetUserDataCurve(
                        out List<UserDataCurve> UserDataCurveList))
                    {
                        foreach (var user_data_curve in UserDataCurveList)
                        {
                            user_data_curve.DeleteGeometryElementCurveOfPropertyType(
                                typeof(PropertyBeam));
                        }
                    }
                }
            }

            return Result.Success;
        }
    }
}