using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using Cocodrilo.UserData;

namespace Cocodrilo.IO
{
    public class OutputKratosDEM
    {
        public OutputKratosDEM()
        {
        }


        public void StartAnalysis(List<Point> PointList)
        {
            string project_path = UserDataUtilities.GetProjectPath("DEM");

            StartAnalysis(project_path, PointList);
        }

        public void StartAnalysis(string project_path, List<Point> PointList)
        {
            //GET GEOMETRY JSON INPUT TEXT
            try
            {
                System.IO.File.WriteAllLines(project_path + "/" + "KlausDEM.mdpa",
                    new List<string> { GetMdpaFile(PointList) });
                System.IO.File.WriteAllLines(project_path + "/" + "KlausDEM_Clusters.mdpa",
                    new List<string> { "" });
                System.IO.File.WriteAllLines(project_path + "/" + "KlausDEM_FEM_boundary.mdpa",
                    new List<string> { "" });
                System.IO.File.WriteAllLines(project_path + "/" + "KlausDEM_Inlet.mdpa",
                    new List<string> { "" });
                System.IO.File.WriteAllLines(project_path + "/" + "MaterialsDEM.json",
                    new List<string> { GetMaterials() });
                System.IO.File.WriteAllLines(project_path + "/" + "ProjectParamatersDEM.json",
                    new List<string> { GetProjectParameters() });
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine("Json output not possible.");
                RhinoApp.WriteLine(ex.ToString());
            }
        }

        private string GetMdpaFile(List<Point> PointList)
        {
            string mdpa_file;

            mdpa_file = @"Begin ModelPartData
//  VARIABLE_NAME value
End ModelPartData

Begin Properties 0
End Properties

";

            string node_string = "Begin Nodes\n";
            string element_string = "Begin Elements SphericParticle3D\n";
            string nodal_data_string = "Begin NodalData RADIUS\n";
            int id_node_counter = 1;
            int id_element_counter = 1;
            foreach (var point in PointList)
            {
                node_string += "    " + id_node_counter + " " + point.Location[0] + " " + point.Location[1] + " " + point.Location[2] + "\n";
                element_string += "    " + id_element_counter + "  0  " + id_node_counter + "\n";
                if (!point.UserDictionary.TryGetDouble("RADIUS", out double radius))
                    radius = 1.0;
                nodal_data_string += "    " + id_node_counter + "  0  " + radius + "\n";
                id_node_counter++;
                id_element_counter++;
            }
            node_string += "End Nodes\n\n";
            element_string += "End Elements\n\n";
            nodal_data_string += "End NodalData\n\n";


            mdpa_file += node_string;
            mdpa_file += element_string;
            mdpa_file += nodal_data_string;
            return mdpa_file;
        }

        public string GetMaterials()
        {
            var materials_list_dict = new List<Dictionary<string, object>> { };
            var materials_relations_list_dict = new List<Dictionary<string, object>> { };
            var material_assignation_table_list_dict = new List<string[]> { };
            foreach (var property in CocodriloPlugIn.Instance.Properties)
            {
                if (property is Cocodrilo.ElementProperties.PropertyDem)
                {
                    var material = CocodriloPlugIn.Instance.GetMaterial(property.mMaterialId);

                    if (material is Cocodrilo.Materials.MaterialDem)
                    {
                        var dem_material = material as Cocodrilo.Materials.MaterialDem;

                        var material_dict = new Dictionary<string, object> { };

                        material_dict.Add("name", dem_material.Name);
                        material_dict.Add("material_id", dem_material.Id);

                        var nodal_variables_dict = dem_material.GetKratosVariables();
                        material_dict.Add("Variables", nodal_variables_dict);

                        materials_list_dict.Add(material_dict);
                        materials_relations_list_dict.AddRange(dem_material.GetKratosMaterialRelations());

                        material_assignation_table_list_dict.Add(new string[] { property.GetKratosModelPart(), material.Name});
                    }
                }
            }

            var materials_dict = new Dictionary<string, object> {
                { "materials", materials_list_dict },
                { "material_relations", materials_relations_list_dict },
                { "material_assignation_table", material_assignation_table_list_dict },
            };

            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize((object)materials_dict);

            return json;
        }

        public string GetProjectParameters()
        {
            var solver_settings_dict = new Dictionary<string, object> {
                    { "RemoveBallsInitiallyTouchingWalls" , false},
                    { "strategy"                          , "sphere_strategy"},
                    { "material_import_settings"          , new Dictionary<string, object>{
                        { "materials_filename" , "MaterialsDEM.json" }
                    }
                    }
                };

            var project_parameters_dict = new Dictionary<string, object> {
                { "Dimension", 3 },
                { "PeriodicDomainOption"          , false},
                {"BoundingBoxOption"              , false},
                {"AutomaticBoundingBoxOption"     , false},
                {"BoundingBoxEnlargementFactor"   , 1.1},
                {"BoundingBoxStartTime"           , 0.0},
                {"BoundingBoxStopTime"            , 1000.0},
                {"BoundingBoxMaxX"                , 10},
                {"BoundingBoxMaxY"                , 10},
                {"BoundingBoxMaxZ"                , 10},
                {"BoundingBoxMinX"                , -10},
                {"BoundingBoxMinY"                , -10},
                {"BoundingBoxMinZ"                , -10},
                {"dem_inlet_option"               , false},
                {"GravityX"                       , 0.0},
                {"GravityY"                       , 0.0},
                {"GravityZ"                       , -9.81},
                {"RotationOption"                 , true},
                {"CleanIndentationsOption"        , false},
                {"solver_setting"                 , solver_settings_dict},
                {"VirtualMassCoefficient"         , 1.0},
                {"RollingFrictionOption"          , false},
                {"GlobalDamping"                  , 0.0},
                {"ContactMeshOption"              , false},
                {"OutputFileType"                 , "Binary"},
                {"Multifile"                      , "multiple_files"},
                {"ElementType"                    , "SphericPartDEMElement3D"},
                {"TranslationalIntegrationScheme" , "Symplectic_Euler"},
                {"RotationalIntegrationScheme"    , "Direct_Integration"},
                {"MaxTimeStep"                    , 1e-6},
                {"FinalTime"                      , 1.0},
                {"NeighbourSearchFrequency"       , 50},
                {"SearchTolerance"                , 0.001},
                {"GraphExportFreq"                , 0.001},
                {"VelTrapGraphExportFreq"         , 0.001},
                {"OutputTimeStep"                 , 0.01},
                {"PostBoundingBox"                , false},
                {"PostLocalContactForce"          , false},
                {"PostDisplacement"               , true},
                {"PostRadius"                     , true},
                {"PostVelocity"                   , true},
                {"PostAngularVelocity"            , false},
                {"PostElasticForces"              , false},
                {"PostContactForces"              , false},
                {"PostRigidElementForces"         , false},
                {"PostStressStrainOption"         , false},
                {"PostTangentialElasticForces"    , false},
                {"PostTotalForces"                , false},
                {"PostPressure"                   , false},
                {"PostShearStress"                , false},
                {"PostSkinSphere"                 , false},
                {"PostNonDimensionalVolumeWear"   , false},
                {"PostParticleMoment"             , false},
                {"PostEulerAngles"                , false},
                {"PostRollingResistanceMoment"    , false},
                {"problem_name"                   , "Klaus"},
                {"post_vtk_option"                , true}
            };

            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize((object)project_parameters_dict);

            return json;
        }
    }
}
