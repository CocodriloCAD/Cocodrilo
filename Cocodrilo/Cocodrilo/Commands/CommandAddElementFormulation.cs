using System.Collections.Generic;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Cocodrilo.ElementProperties;
using Cocodrilo.UserData;

namespace Cocodrilo.Commands
{
    public class CommandAddElementFormulation : Command
    {
        static CommandAddElementFormulation _instance;
        public CommandAddElementFormulation()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandAddElementFormulation command.</summary>
        public static CommandAddElementFormulation Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_AddElementFormulation"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var panel = Panels.UserControlCocodriloPanel.Instance;
            int material_id = panel.getMaterialIdElement();
            string ElementType = panel.getSelectedElementType();



            if (ElementType == "Shell")
            {
                if(CommandUtilities.TryGetUserDataSurface(out List<UserDataSurface> UserDataSurfaceList))
                {
                    foreach (var user_data_surface in UserDataSurfaceList)
                    {
                        var shell_properties = panel.GetShellProperties();
                        var shell_property = new PropertyShell(material_id, shell_properties);
                        user_data_surface.AddGeometryElementSurface(
                            shell_property);
                    }
                }
            }

            if (ElementType == "Membrane")
            {
                var rcSurface = RhinoGet.GetMultipleObjects(
                    "Select Surfaces...",
                    false,
                    ObjectType.Surface,
                    out var objrefSurface);

                if (rcSurface != Result.Success)
                    return Result.Success;

                // See if user data of my custom type is attached to the geomtry
                foreach (var surface in objrefSurface)
                {
                    var user_data_surface = surface.Brep()
                        .Surfaces[surface.Face().FaceIndex]
                        .UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                    if (user_data_surface == null)
                    {
                        user_data_surface = new UserData.UserDataSurface();
                        surface.Brep().Surfaces[surface.Face().FaceIndex].UserData.Add(user_data_surface);
                        RhinoApp.WriteLine("New surface user data added.");
                    }

                    var membrane_properties = panel.getMembraneProperties(surface);
                    var is_formfinding = panel.getIsFormFindingElement();
                    var membrane_property = new PropertyMembrane(
                        material_id,
                        is_formfinding,
                        membrane_properties);
                    user_data_surface.AddGeometryElementSurface(
                        membrane_property);
                }
            }

            else if (ElementType == "Cable")
            {
                var cable_properties = panel.GetCableProperties();
                var geometry_type = panel.getCableTopologyType();
                var is_formfinding = panel.getIsCableFormFinding();
                if (geometry_type == GeometryType.SurfaceEdge)
                {
                    if (CommandUtilities.TryGetUserDataEdge(out List<UserDataEdge> UserDataEdgeList))
                    {
                        var property_cable = new PropertyCable(
                            geometry_type,
                            material_id,
                            cable_properties,
                            is_formfinding);
                        foreach (var user_data_edge in UserDataEdgeList)
                        {
                            user_data_edge.AddBrepElementEdge(
                                property_cable);
                        }
                    }
                }
                else if (geometry_type == GeometryType.GeometryCurve)
                {
                    if (CommandUtilities.TryGetUserDataCurve(out List<UserDataCurve> UserDataCurveList))
                    {
                        var property_cable = new PropertyCable(
                            geometry_type,
                            material_id,
                            cable_properties,
                            is_formfinding);
                        foreach (var user_data_curve in UserDataCurveList)
                        {
                            user_data_curve.AddGeometryElementCurve(property_cable);
                        }
                    }
                }
            }
            else if (ElementType == "Beam")
            {
                RhinoApp.WriteLine("Adding of a Beam formulation is not provided yet.");
            }

            return Result.Success;
        }
    }
}