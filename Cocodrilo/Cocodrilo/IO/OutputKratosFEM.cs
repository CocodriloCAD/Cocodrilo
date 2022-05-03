using Cocodrilo.UserData;
using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Cocodrilo.IO
{
    using Dict = Dictionary<string, object>;
    using DictList = List<Dictionary<string, object>>;
    using IdDict = Dictionary<int, List<int>>;
    using PropertyIdDict = Dictionary<int, List<BrepToParameterLocations>>;
    public class OutputKratosFEM : Output
    {
        //public OutputKratosFEM() : base(new Analyses.Analysis())
        //{
        //}
        public OutputKratosFEM(Analyses.Analysis analysis) : base(analysis)
        {
           this.analysis = analysis;
        }
        public void StartAnalysis(List<Mesh> MeshList)
        {
            // Namen aus Analyse generieren
            //this.analysis = analysis;
            string project_path = UserDataUtilities.GetProjectPath(analysis.Name);

            StartAnalysis(project_path, MeshList);
        }

        public void StartAnalysis(string project_path, List<Mesh> MeshList)
        {
            try
            {
                PropertyIdDict property_id_dictionary = new PropertyIdDict();
                
                System.IO.File.WriteAllLines(project_path + "/" + "Grid.mdpa",
                    new List<string> { GetFemMdpaFile(MeshList, ref property_id_dictionary) });
                System.IO.File.WriteAllLines(project_path + "/" + "Body.mdpa",
                    new List<string> { GetFemMdpaFile(MeshList, ref property_id_dictionary) });
                System.IO.File.WriteAllLines(project_path + "/" + "ParticleMaterials.json",
                    new List<string> {GetMaterials(property_id_dictionary)});
                //System.IO.File.WriteAllLines(project_path + "/" + "ProjectParamaters.json",
                //    new List<string> {WriteProjectParameters(project_path)});
                //new List<string> { WriteProjectParameters(property_id_dictionary, NodalVariables, project_path) });
                WriteProjectParameters(project_path);
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine("Json output not possible.");
                RhinoApp.WriteLine(ex.ToString());
            }
        }

        private string GetFemMdpaFile(List<Mesh> MeshList, ref PropertyIdDict PropertyIdDictionary)
        {
            string mdpa_file;

            mdpa_file = "Begin ModelPartData\n"
                        + "//  VARIABLE_NAME value\n"
                        + "End ModelPartData\n\n";

            for (int i = 2; i < MeshList.Count + 2; i++)
            {
                mdpa_file += "Begin Properties " + i.ToString() + "\n End Properties \n";
            }
            mdpa_file += "\n\n";

            string node_string = "Begin Nodes\n";
            string element_string = "Begin Conditions RigidFace3D3N\n";
            int id_node_counter = 1;
            int id_element_counter = 1;

            int sub_model_part_counter = 1;

            string sub_model_part_string = "";

            for (int m = 0; m < MeshList.Count; m++)
            {

                sub_model_part_string += "Begin SubModelPart " + sub_model_part_counter + " // GUI DEM-FEM-Wall - DEM-FEM-Wall - group identifier: Parts_membran_oben\n"
                    + "  Begin SubModelPartData // DEM-FEM-Wall. Group name: Parts_membran_oben\n"
                    + "    IS_GHOST false\n"
                    + "    IDENTIFIER Parts_membran_oben\n"
                    + "    FORCE_INTEGRATION_GROUP 0\n"
                    + "  End SubModelPartData\n"
                    + "  Begin SubModelPartNodes\n";

                var mesh = MeshList[m];

                for (int i = 0; i < mesh.Vertices.Count; i++)
                {
                    node_string += "    " + (id_node_counter + i).ToString() + " " + mesh.Vertices[i].X + " " + mesh.Vertices[i].Y + " " + mesh.Vertices[i].Z + "\n";
                    sub_model_part_string += "     " + (id_node_counter + i).ToString() + "\n";
                }
                sub_model_part_string += "End SubModelPartNodes\n";
                sub_model_part_string += "Begin SubModelPartConditions\n";
                foreach (var face in mesh.Faces)
                {
                    element_string += "    " + id_element_counter + "  " + (m + 2).ToString() + "  " + (face.A + id_node_counter) + "  " + (face.B + id_node_counter) + "  " + (face.C + id_node_counter) + "\n";
                    sub_model_part_string += "     " + id_element_counter.ToString() + "\n";
                    id_element_counter++;
                }

                sub_model_part_string += "End SubModelPartConditions\n";
                sub_model_part_string += "End SubModelPart\n\n";

                id_node_counter += mesh.Vertices.Count;
                sub_model_part_counter++;
            }
            node_string += "End Nodes\n\n";
            element_string += "End Elements\n\n";

            mdpa_file += node_string;
            mdpa_file += element_string;
            mdpa_file += sub_model_part_string;

            return mdpa_file;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ElementConditionDictionary"></param>
        /// <returns></returns>
        public string GetMaterials(PropertyIdDict ElementConditionDictionary)
        {
            var property_dict_list = new DictList();
            foreach (var property_id in ElementConditionDictionary.Keys)
            {
                var this_property = CocodriloPlugIn.Instance.GetProperty(property_id, out bool success);
                if (!success)
                {
                    RhinoApp.WriteLine("InputJSON::GetMaterials: Property with Id: " + property_id + " does not exist.");
                    continue;
                }

                //some properties do not need materials and properties
                if (!this_property.HasKratosMaterial())
                    continue;

                var variables = this_property.GetKratosVariables();

                var property_dict = new Dict
                    {
                        {"model_part_name", "IgaModelPart." + this_property.GetKratosModelPart()},
                        {"properties_id", this_property.mPropertyId},
                    };

                var material_dict = new Dict { };

                int material_id = this_property.GetMaterialId();
                if (material_id >= 0)
                {
                    var material = CocodriloPlugIn.Instance.GetMaterial(material_id);
                    var material_variables = material.GetKratosVariables();
                    foreach (var material_variable in material_variables)
                        variables.Add(material_variable.Key, material_variable.Value);

                    material_dict.Add("name", material.Name);
                    material_dict.Add("material_id", material.Id);
                    material_dict.Add("constitutive_law", material.GetKratosConstitutiveLaw());

                    if (material.HasKratosSubProperties())
                    {
                        property_dict.Add("sub_properties", material.GetKratosSubProperties());
                    }
                }

                material_dict.Add("Variables", variables);
                material_dict.Add("Tables", new Dict { });

                property_dict.Add("Material", material_dict);

                property_dict_list.Add(property_dict);
            }

            var dict = new Dict
            {
                {"properties", property_dict_list }
            };
            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize((object)dict);

            return json;
        }
        public void WriteProjectParameters(
           //PropertyIdDict ElementConditionDictionary,
           //List<string> NodalVariables,
           string ProjectPath
           )
        {
            string model_part_name = "MPM_Material";
            string analysis_type = "linear";
            string solver_type_analysis = "static";
            double step_size = 1.0;
            int max_iteration = 1;
            double displacement_absolute_tolerance = 1.0e-9;
            double residual_absolute_tolerance = 1.0e-9;
            double end_time = 0.1;
            bool compute_reactions = true;
            bool rotation_dofs = false;

            //case differentiation: static, dynamic, quasi-static necessary
            //if (this.analysis.GetType() == typeof(AnalysisLinear))
            //{
            //    analysis_type = "linear";
            //}
            // 1. problem data block
            var problem_data = new Dict
            {
                { "problem_name", analysis.Name},
                { "parallel_type", "OpenMP"},
                { "echo_level", 1 },
                { "start_time", 0.0},
                { "end_time", end_time}
            };

            // solver setting block

            var model_import_settings =
                new Dict { { "input_type", "mdpa" },
                           { "input_filename", analysis.Name+"_Body" } };

            var material_import_settings =
                new Dict { { "materials_filename", "ParticleMaterials.json" } };

            var time_stepping = new Dict
                { { "time_step", step_size} };

            var grid_model_import_settings = new Dict
            {
                { "input_type", "mdpa" },
                { "input_filename", analysis.Name+"_Grid" }
            };

            var auxiliary_variables_list =
                new System.Collections.ArrayList()
                {
                    "NORMAL", "IS_STRUCTURE"
                };


            var linear_solver_settings =
                new Dict {
                    { "solver_type", "LinearSolversApplication.sparse_lu" },
                    { "max_iteration", 500 },
                    { "tolerance", 1e-9 },
                    { "scaling", false },
                    { "verbosity", 1 } };

            var solver_settings = new Dict
            {
                { "solver_type", "Dynamic" },
                { "model_part_name", "MPM_Material" },
                { "domain_size", 2 },
                { "echo_level", 1 },
                { "analysis_type", "non_linear"},
                { "time_integration_method", "implicit" },
                { "scheme_type", "newmark" },
                { "model_import_settings", model_import_settings },
                { "material_import_settings", material_import_settings },
                { "time_stepping", time_stepping },
                { "convergence_criterion", "residual criterion" },
                { "displacement_relative_tolerance", 0.0001  },
                { "displacement_absolute_tolerance", 1e-9 },
                { "residual_relative_tolerance", 0.0001 },
                { "residual_absolute_tolerance", 1e-9 },
                { "max_iteration", 10 },
                { "grid_model_import_settings", grid_model_import_settings },
                { "pressure_dofs", false },
                { "auxiliary_variables_list", auxiliary_variables_list }
            };

            //Processes block
            
            //constraint_process_list
            var constraints_process_list = new DictList();

                var constraint_process_list_interval =
                    new System.Collections.ArrayList()
                    {
                            0.0 , "End"
                    };

                var constraint_process_list_constrained =
                    new System.Collections.ArrayList()
                    {
                            true, true, true
                    };

                var constraint_process_list_value =
                    new System.Collections.ArrayList()
                    {
                           0.0, 0.0, 0.0
                    };


                var constraint_process_list_parameters = new Dict
                {
                    { "model_part_name", "Background_Grid.DISPLACEMENT_Displacement_Auto1" },
                    { "variable_name", "DISPLACEMENT" },
                    { "interval", constraint_process_list_interval },
                    { "constrained", constraint_process_list_constrained },
                    { "value", constraint_process_list_value }
                };

            constraints_process_list.Add(new Dict
            {
                {"python_module", "assign_vector_variable_process" },
                {"kratos_module", "KratosMultiphysics" },
                {"process_name", "AssignVectorVariableProcess" },
                { "Parameters", constraint_process_list_parameters }

            });


            //load_process_list
            var load_process_list = new DictList();
            // up to now empty; has to be adopted

            //list_other_processes
            var list_other_processes = new DictList();

                var list_other_processes_parameters = new Dict
                {
                    { "model_part_name", "Background_Grid.Slip2D_Slip_Auto1" },
                    { "particles_per_condition", 3 },
                    { "penalty_factor", 1e10 },
                    { "constrained", "fixed" }
                };

            list_other_processes.Add(new Dict
            {
                {"python_module", "apply_mpm_particle_dirichlet_condition_process" },
                {"kratos_module", "KratosMultiphysics.ParticleMechanicsApplication" },
                { "Parameters", list_other_processes_parameters }

            });

            //gravity
            var gravity = new DictList();

                var gravity_direction =
                    new System.Collections.ArrayList()
                    {
                           0.0,-1.0, 0.0
                    };

                var gravity_parameters = new Dict
                {
                    { "model_part_name", "MPM_Material" },
                    { "variable_name", "MPM_VOLUME_ACCELERATION" },
                    { "modulus", 9.81 },
                    { "direction", gravity_direction }
                };

            gravity.Add(new Dict
            {
                {"python_module", "assign_gravity_to_particle_process" },
                {"kratos_module", "KratosMultiphysics.ParticleMechanicsApplication" },
                {"process_name", "AssignGravityToParticleProcess" },
                { "Parameters", gravity_parameters }

            });

            var processes = new Dict()
            {
                { "constraints_process_list", constraints_process_list },
                { "load_process_list", load_process_list },
                { "list_other_processes", list_other_processes },
                { "gravity", gravity }
            };

            // End processes block

            // Begin body output

            var body_output_process = new DictList();

                var gidpost_flags = new Dict()
                {
                    {"GiDPostMode", "GiD_PostBinary" },
                    {"WriteDeformedMeshFlag", "WriteDeformed" },
                    {"WriteConditionsFlag", "WriteConditions" },
                    {"MultiFileFlag", "SingleFile" }
                };

                var plane_output =
                    new System.Collections.ArrayList()
                    {
                           //empty by default
                    };

                var gauss_points_results =
                    new System.Collections.ArrayList()
                    {
                               "MP_Velocity","MP_Displacement"
                    };

                var nodal_historical_results =
                    new System.Collections.ArrayList()
                    {
                               //empty by default
                    };

                var point_data_configuration =
                        new System.Collections.ArrayList()
                        {
                               //empty by default
                        };

            var result_file_configuration = new Dict()
                {
                    {"gidpost_flags", gidpost_flags },
                    {"file_label", "step" },
                    {"output_control_type", "time" },
                    {"output_interval", 0.1 },
                    {"body_output", true },
                    {"node_output", false },
                    {"skin_output", false },
                    {"plane_output", plane_output },
                    {"gauss_point_results", gauss_points_results },
                    {"nodal_nonhistorical_results", nodal_historical_results }
                };

                var postprocess_parameters = new Dict()
                {
                    {"result_file_configuration", result_file_configuration },
                    {"point_data_configuration", point_data_configuration }
                };

                var body_output_process_parameters = new Dict()
                {
                    {"model_part_name", "MPM_Material" },
                    {"output_name", analysis.Name },
                    {"postprocess_parameters", postprocess_parameters }
                };

                body_output_process.Add(new Dict
                {
                    {"python_module", "particle_gid_output_process" },
                    {"kratos_module", "KratosMultiphysics.ParticleMechanicsApplication" },
                    {"process_name", "ParticleMPMGiDOutputProcess" },
                    {"help", "This process writes postprocessing files for GiD" },
                    { "Parameters", body_output_process_parameters }

                });

            // End body output

            // Begin grid output

            var grid_output_process = new DictList();

            var grid_gidpost_flags = new Dict()
                {
                    {"GiDPostMode", "GiD_PostBinary" },
                    {"WriteDeformedMeshFlag", "WriteDeformed" },
                    {"WriteConditionsFlag", "WriteConditions" },
                    {"MultiFileFlag", "SingleFile" }
                };

            var grid_plane_output =
                new System.Collections.ArrayList()
                {
                        //empty by default
                    };

            var nodal_results =
                new System.Collections.ArrayList()
                {
                    "DISPLACEMENT","REACTION"
                };

            var grid_nodal_historical_results =
                new System.Collections.ArrayList()
                {
                        //empty by default
                    };

            var grid_point_data_configuration =
                    new System.Collections.ArrayList()
                    {
                            //empty by default
                        };

            var grid_result_file_configuration = new Dict()
                {
                    {"gidpost_flags", gidpost_flags },
                    {"file_label", "step" },
                    {"output_control_type", "time" },
                    {"output_interval", 0.1 },
                    {"body_output", true },
                    {"node_output", false },
                    {"skin_output", false },
                    {"plane_output", plane_output },
                    {"nodal_results", nodal_results },
                    {"nodal_nonhistorical_results", grid_nodal_historical_results }
                };

            var grid_postprocess_parameters = new Dict()
                {
                    {"result_file_configuration", grid_result_file_configuration },
                    {"point_data_configuration", grid_point_data_configuration }
                };

            var grid_output_process_parameters = new Dict()
                {
                    {"model_part_name", "Background_Grid" },
                    {"output_name", analysis.Name },
                    {"postprocess_parameters", grid_postprocess_parameters }
                };

            grid_output_process.Add(new Dict
                {
                    {"python_module", "gid_output_process" },
                    {"kratos_module", "KratosMultiphysics" },
                    {"process_name", "GiDOutputProcess" },
                    {"help", "This process writes postprocessing files for GiD" },
                    {"Parameters", grid_output_process_parameters }

                });


            var output_processes = new Dict()
            {
                { "body_output_process", body_output_process },
                { "grid_output_process", grid_output_process }                

            };

            //end grid output

            var dict = new Dict
                {
                    {"problem_data", problem_data},
                    {"solver_settings", solver_settings},
                    {"processes", processes },
                    {"output_processes", output_processes},
                    
                };

                var serializer = new JavaScriptSerializer();
                string project_parameters_json = serializer.Serialize((object)dict);

                System.IO.File.WriteAllLines(ProjectPath + "/" + "ProjectParameters.json",
                new List<string> { project_parameters_json });
            }
            #region CO SIMULATION
        public override Dictionary<string, object> GetCouplingSequence(
            List<Analyses.Analysis> InputAnalyses,
            List<Analyses.Analysis> OutputAnalyses)
        {
            var input_data_list = new List<Dictionary<string, object>> { };
            foreach (var input_analysis in InputAnalyses)
                input_data_list.Add( new Dictionary<string, object> {
                        {"data"                   , "load" },
                        {"from_solver"            , input_analysis.Name},
                        {"from_solver_data"       , "contact_force"},
                        {"data_transfer_operator" , "mapper_1"}});
            var output_data_list = new List<Dictionary<string, object>> { };
            foreach (var input_analysis in InputAnalyses)
                input_data_list.Add(new Dictionary<string, object> {
                        { "data"                  , "disp" },
                        {"from_solver"            , input_analysis.Name},
                        {"from_solver_data"       , "disp"},
                        {"data_transfer_operator" , "mapper_1"}});

            return new Dictionary<string, object> {
                    { "name", analysis.Name },
                    { "input_data_list", input_data_list },
                    { "output_data_list", input_data_list } };
        }

        public override Dictionary<string, object> GetCouplingSolver()
        {
            var solver_wrapper_settings = new Dictionary<string, object> {
                { "input_file", "ProjectParametersFEM" }
            };

            var disp = new Dictionary<string, object> {
                { "model_part_name", "Structure.struct_sub" },
                { "variable_name", "DISPLACEMENT" },
                { "dimension", 3 }
            };
            var load = new Dictionary<string, object> {
                { "model_part_name", "Structure.struct_sub" },
                { "variable_name", "POINT_LOAD" },
                { "dimension", 3 }
            };
            var velocity = new Dictionary<string, object> {
                { "model_part_name", "Structure.struct_sub" },
                { "variable_name", "VELOCITY" },
                { "dimension", 3 }
            };

            var data = new Dictionary<string, object> {
                { "disp", disp },
                { "load", load },
                { "velocity", velocity }
            };

            return new Dictionary<string, object> {
                    { analysis.Name, new Dictionary<string, object> {
                        { "type", "solver_wrappers.kratos.dem_wrapper"},
                        { "solver_wrapper_settings", solver_wrapper_settings},
                        { "data", data}
                    } } };
        }
        #endregion
    }

}
