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
                    new List<string> { "" });
                //System.IO.File.WriteAllLines(project_path + "/" + "ProjectParamaters.json",
                //    new List<string> { WriteProjectParameters( ElementConditionDictionary, NodalVariables, project_path)});

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
                { "Parameters", list_other_processes_parameters }

            });

            var processes = new Dict()
            {
                { "constraints_process_list", constraints_process_list },
                { "load_process_list", load_process_list },
                { "list_other_processes", list_other_processes },
                { "gravity", gravity }
            };

            // End processes block

            //Output block 1

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



            // End Output block 1

            // Output block 2

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

            var grid_gauss_points_results =
                new System.Collections.ArrayList()
                {
                               "MP_Velocity","MP_Displacement"
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
                    {"gauss_point_results", gauss_points_results },
                    {"nodal_nonhistorical_results", nodal_historical_results }
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

            body_output_process.Add(new Dict
                {
                    {"python_module", "gid_output_process" },
                    {"kratos_module", "KratosMultiphysics" },
                    {"process_name", "GiDOutputProcess" },
                    {"help", "This process writes postprocessing files for GiD" },
                    { "Parameters", grid_output_process_parameters }

                });






































            //    // Modeler block
            //    var modelers = new DictList();

            //    var cad_io_modeler_parameters = new Dict
            //    {
            //        { "echo_level", 0 },
            //        { "cad_model_part_name", model_part_name },
            //        { "geometry_file_name", "geometry.cad.json" },
            //        { "output_geometry_file_name", analysis.Name + "_kratos_0.georhino.json" }
            //    };
            //    modelers.Add(new Dict
            //    {
            //        { "modeler_name", "CadIoModeler"},
            //        { "Parameters", cad_io_modeler_parameters}
            //    });
            //    var refinement_modeler_parameters = new Dict
            //    {
            //        { "echo_level", 0 },
            //        { "physics_file_name", "refinements.iga.json" }
            //    };
            //    modelers.Add(new Dict
            //    {
            //        { "modeler_name", "RefinementModeler"},
            //        { "Parameters", refinement_modeler_parameters}
            //    });
            //    var iga_modeler_parameters = new Dict
            //    {
            //        { "echo_level", 0 },
            //        { "cad_model_part_name", model_part_name },
            //        { "analysis_model_part_name", model_part_name },
            //        { "physics_file_name", "physics.iga.json"}
            //    };
            //    modelers.Add(new Dict
            //    {
            //        { "modeler_name", "IgaModeler"},
            //        { "Parameters", iga_modeler_parameters}
            //    });

            //    if (this.analysis.GetType() == typeof(AnalysisShapeOptimization))
            //    {
            //        (this.analysis as AnalysisShapeOptimization).WriteOptimizationParameters(modelers, model_part_name, ProjectPath);
            //        modelers.Clear();
            //    }

            //    // 3. process block
            //    var dirichlet_process_list = new DictList();
            //    var neumann_process_list = new DictList();
            //    var additional_processes = new DictList();
            //    var additional_variables = new List<string>();
            //    var additional_dofs = new List<string>();
            //    var additional_reactions = new List<string>();
            //    var output_process_list = new DictList();
            //    foreach (var dict_entry in ElementConditionDictionary)
            //    {
            //        var property = CocodriloPlugIn.Instance.GetProperty(dict_entry.Key, out bool get_property_success);
            //        if (!get_property_success)
            //            continue;

            //        if (property.RotationDofs() == true)
            //        {
            //            rotation_dofs = true;
            //        }
            //        if (property.GetAdditionalDofs() != "")
            //        {
            //            additional_variables.Add(property.GetAdditionalDofs());
            //            additional_dofs.Add(property.GetAdditionalDofs());
            //        }
            //        if (property.GetAdditionalDofReactions() != "")
            //        {
            //            additional_variables.Add(property.GetAdditionalDofReactions());
            //            additional_reactions.Add(property.GetAdditionalDofReactions());
            //        }

            //        if (property.GetType() == typeof(PropertyCoupling)
            //            || property.GetType() == typeof(PropertySupport))
            //        {
            //            dirichlet_process_list.AddRange(property.GetKratosProcesses());
            //        }
            //        else if (property.GetType() == typeof(PropertyLoad))
            //        {
            //            neumann_process_list.AddRange(property.GetKratosProcesses());
            //        }
            //        else if (property.GetType() == typeof(PropertyCheck))
            //        {
            //            output_process_list.AddRange(property.GetKratosProcesses());
            //        }
            //        else if (property.GetType() == typeof(PropertyShell))
            //        {
            //            additional_processes.AddRange(property.GetKratosProcesses(dict_entry.Value.Select(item => item.BrepId).ToList()));
            //        }

            //        var output_integration_domain_process_dict = property.GetKratosOutputIntegrationDomainProcess(
            //            CocodriloPlugIn.Instance.OutputOptions, analysis.Name, model_part_name);
            //        if (output_integration_domain_process_dict.Count > 0)
            //            additional_processes.Add(output_integration_domain_process_dict);

            //        // output processes
            //        var output_process_dict = property.GetKratosOutputProcess(
            //            CocodriloPlugIn.Instance.OutputOptions, analysis, model_part_name);

            //        if (output_process_dict.Count > 0)
            //            output_process_list.Add(output_process_dict);
            //    }

            //    if (this.analysis.GetType() == typeof(AnalysisTransient))
            //    {
            //        var transient_analysis = analysis as AnalysisTransient;
            //        if (transient_analysis.AutomaticRayleigh == true)
            //        {
            //            var eigen_settings_parameters = new Dict
            //            {
            //                { "solver_type", "eigen_eigensystem" },
            //                { "max_iteration", 100 },
            //                { "tolerance", 0.001 },
            //                { "number_of_eigenvalues", transient_analysis.NumEigen },
            //                { "echo_level", 0 }
            //            };

            //            var automatic_rayleigh_parameters = new Dict
            //            {
            //                { "echo_level", 0 },
            //                { "write_on_properties", false },
            //                { "model_part_name", model_part_name },
            //                { "damping_ratio_0", transient_analysis.DampingRatio0 },
            //                { "damping_ratio_1", transient_analysis.DampingRatio1 },
            //                { "eigen_system_settings", eigen_settings_parameters }
            //            };

            //            additional_processes.Add(new Dict
            //            {
            //                { "kratos_module", "KratosMultiphysics.StructuralMechanicsApplication"},
            //                { "python_module", "automatic_rayleigh_parameters_computation_process"},
            //                { "process_name", "AutomaticRayleighComputationProcess"},
            //                { "Parameters", automatic_rayleigh_parameters}
            //            });
            //        }
            //    }


            //    if (this.analysis.GetType() == typeof(AnalysisFormfinding))
            //    {
            //        foreach (var output_process in output_process_list)
            //        {
            //            output_process.Add("form_finding", true);
            //        }
            //    }

            //    var output_processes = new Dict { { "output_process_list", output_process_list } };

            //    if (this.analysis.GetType() == typeof(AnalysisEigenvalue))
            //    {
            //        output_processes["output_process_list"] = new ArrayList();

            //        additional_processes.Add(new Dict {
            //            { "kratos_module", "IgaApplication" },
            //            { "python_module", "output_eigen_values_process" },
            //            { "Parameters", new Dict {
            //                { "output_file_name", analysis.Name + "_kratos_eigen_values.post.res" },
            //                { "model_part_name", model_part_name }} },
            //        });
            //    }

            //    var processes = new Dict
            //    {
            //        { "additional_processes", additional_processes},
            //        { "dirichlet_process_list", dirichlet_process_list},
            //        { "neumann_process_list",   neumann_process_list}
            //    };

            //    var solver_settings = new Dict
            //    {
            //        { "model_part_name", model_part_name},
            //        { "domain_size", 1},
            //        { "echo_level", 1},
            //        { "buffer_size", 2},
            //        { "analysis_type", analysis_type},
            //        { "model_import_settings", model_import_settings},
            //        { "material_import_settings", material_import_settings},
            //        { "time_stepping", time_stepping},
            //        { "rotation_dofs", rotation_dofs},
            //        { "reform_dofs_at_each_step", false },
            //        { "line_search", false},
            //        { "compute_reactions", compute_reactions},
            //        { "block_builder", true },
            //        { "clear_storage", false },
            //        { "move_mesh_flag", true },
            //        { "convergence_criterion", "residual_criterion" },
            //        { "displacement_relative_tolerance", 1.0e-4 },
            //        { "displacement_absolute_tolerance", displacement_absolute_tolerance },
            //        { "residual_relative_tolerance", 1.0e-4 },
            //        { "residual_absolute_tolerance", residual_absolute_tolerance },
            //        { "max_iteration", max_iteration },
            //        { "solver_type", solver_type_analysis },
            //        { "linear_solver_settings", linear_solver_settings }
            //    };

            //    // Additional solver settings for formfinding
            //    if (this.analysis.GetType() == typeof(AnalysisFormfinding))
            //    {
            //        var projection_settings = new Dictionary<string, object>
            //        {
            //            { "model_part_name", "IgaModelPart"},
            //            { "echo_level", 1},
            //            { "projection_type", "planar"},
            //            { "global_direction", new object[] { 1.0, 0.0, 0.0 }},
            //            { "variable_name", "LOCAL_PRESTRESS_AXIS_1"},
            //            { "method_specific_settings", new object{}},
            //            { "check_local_space_dimension", false}
            //        };
            //        solver_settings["reform_dofs_at_each_step"] = true;
            //        solver_settings.Add("projection_settings", projection_settings);
            //        solver_settings.Add("printing_format", "gid");
            //        solver_settings.Add("write_formfound_geometry_file", true);
            //    }

            //    // Additional integration method and scheme in solver settings for transient analysis
            //    if (this.analysis.GetType() == typeof(AnalysisTransient))
            //    {
            //        var transient_analysis = analysis as AnalysisTransient;
            //        solver_settings.Add("time_integration_method", transient_analysis.TimeInteg);
            //        solver_settings.Add("scheme_type", transient_analysis.Scheme);
            //        solver_settings.Add("rayleigh_alpha", transient_analysis.RayleighAlpha);
            //        solver_settings.Add("rayleigh_beta", transient_analysis.RayleighBeta);
            //    }

            //    // Additional settings and scheme in solver settings for eigenvalue analysis
            //    if (this.analysis.GetType() == typeof(AnalysisEigenvalue))
            //    {
            //        var eigenvalue_analysis = analysis as AnalysisEigenvalue;
            //        var eigensolver_settings = new Dict
            //        {
            //            {"solver_type", eigenvalue_analysis.mSolverType },
            //            {"max_iteration", eigenvalue_analysis.mMaximumIterations },
            //            {"number_of_eigenvalues", eigenvalue_analysis.mNumEigenvalues },
            //            {"echo_level", 4 }
            //        };
            //        solver_settings.Remove("linear_solver_settings");
            //        solver_settings.Add("eigensolver_settings", eigensolver_settings);
            //    }

            //    // Additional dofs in solver setings if required, as e.g. for the Lagrange Multiplier method.
            //    if (additional_dofs.Count == 0)
            //    {
            //        solver_settings.Add("auxiliary_variables_list", new ArrayList());
            //        solver_settings.Add("auxiliary_dofs_list", new ArrayList());
            //        solver_settings.Add("auxiliary_reaction_list", new ArrayList());
            //    }
            //    else
            //    {
            //        solver_settings.Add("auxiliary_variables_list", additional_variables.Distinct().ToList());
            //        solver_settings.Add("auxiliary_dofs_list", additional_dofs.Distinct().ToList());
            //        solver_settings.Add("auxiliary_reaction_list", additional_reactions.Distinct().ToList());
            //    }

            var dict = new Dict
                {
                    {"problem_data", problem_data},
                    {"solver_settings", solver_settings},
                    {"processes", processes }
                    //{"modelers", modelers},
                    //{"processes", processes},
                    //{"output_processes", output_processes}
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
                        { "data"                  , "load" },
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
