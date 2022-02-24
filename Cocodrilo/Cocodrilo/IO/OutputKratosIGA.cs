using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Rhino;
using Rhino.Geometry;
using System.IO;
using System.Web.Script.Serialization;
using Cocodrilo.ElementProperties;
using Cocodrilo.UserData;
using Cocodrilo.Analyses;

namespace Cocodrilo.IO
{

    using Dict = Dictionary<string, object>;
    using DictList = List<Dictionary<string, object>>;
    using IdDict = Dictionary<int, List<int>>;
    using PropertyIdDict = Dictionary<int, List<BrepToParameterLocations>>;

    public struct BrepToParameterLocations
    {
        public int BrepId;
        public List<Elements.ParameterLocation> ParameterLocations;

        public BrepToParameterLocations(int ThisBrepId)
        {
            BrepId = ThisBrepId;
            ParameterLocations = new List<Elements.ParameterLocation>();
        }
        public BrepToParameterLocations(int ThisBrepId, Elements.ParameterLocation ThisParameterLocation)
        {
            BrepId = ThisBrepId;
            ParameterLocations = new List<Elements.ParameterLocation> { ThisParameterLocation };
        }
        public void AddParameterLocation(Elements.ParameterLocation ThisParameterLocation)
        {
            if (!ParameterLocations.Contains(ThisParameterLocation))
            {
                ParameterLocations.Add(ThisParameterLocation);
            }
        }
    }

    public class OutputKratosIGA : Output
    {
        public OutputKratosIGA(Analyses.Analysis analysis) : base(analysis)
        {
            this.analysis = analysis;
        }

        public override void StartAnalysis()
        {
            string project_path = UserDataUtilities.GetProjectPath(analysis.Name);

            var brep_list = GeometryUtilities.GetBrepList();
            var curve_list = GeometryUtilities.GetCurveList();

            StartAnalysis(project_path, brep_list, curve_list, new List<Point>());
        }
        public override void StartAnalysis(
            List<Brep> BrepList, List<Curve> CurveList, List<Point> PointList)
        {
            string project_path = UserDataUtilities.GetProjectPath(analysis.Name);

            StartAnalysis(project_path, BrepList, CurveList, PointList);
        }

        public void StartAnalysis(string ProjectPath)
        {
            var brep_list = GeometryUtilities.GetBrepList();
            var curve_list = GeometryUtilities.GetCurveList();

            StartAnalysis(ProjectPath, brep_list, curve_list, new List<Point>());
        }
        public void StartAnalysis(string project_path, List<Brep> BrepList, List<Curve> CurveList, List<Point> PointList)
        {
            //GET GEOMETRY JSON INPUT TEXT
            try
            {
                var propert_ids_brep_ids = new PropertyIdDict();

                WriteGeometryJson(BrepList, CurveList, PointList,
                    project_path, ref propert_ids_brep_ids);

                List<string> nodal_variables = new List<string> { };
                List<string> integration_point_variables = new List<string> { };
                System.IO.File.WriteAllLines(project_path + "/" + "materials.json",
                    new List<string> { GetMaterials(propert_ids_brep_ids, ref nodal_variables) });

                System.IO.File.WriteAllLines(project_path + "/" + "physics.iga.json",
                    new List<string> { GetPhysics(propert_ids_brep_ids) });

                WriteProjectParameters(propert_ids_brep_ids, nodal_variables, project_path);

                CocodriloPlugIn.Instance.ClearUnusedProperties(propert_ids_brep_ids.Keys.ToList());
            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine("Json output not possible.");
                RhinoApp.WriteLine(ex.ToString());
            }

            //GET PYTHON FILES FOR KRATOS
            try
            {
                string source_katos_scripts = UserDataUtilities.GetPluginPath() + "KRATOS_TEMPLATES";
                string[] pythonFiles = Directory.GetFiles(source_katos_scripts, "*.py");

                // Copy python files.
                foreach (string f in pythonFiles)
                {
                    // Remove path from the file name.
                    string fName = f.Substring(source_katos_scripts.Length + 1);

                    // Use the Path.Combine method to safely append the file name to the path.
                    // Will overwrite if the destination file already exists.
                    File.Copy(
                        Path.Combine(source_katos_scripts, fName),
                        Path.Combine(project_path, fName),
                        true);
                }
            }
            catch
            {
                RhinoApp.WriteLine("No Kratos scripts found. Needs python (.py) files in the folder: KRATOS_TEMPLATES.");
            }
        }

        public void WriteGeometryJson(
            List<Brep> Breps,
            List<Curve> Curves,
            List<Point> PointList,
            string ProjectPath,
            ref PropertyIdDict rPropertyIdsBrepsIds)
        {
            int brep_ids = 1;

            GeometryUtilities.AssignBrepIds(Breps, Curves, PointList, ref brep_ids);

            List<Curve> IntersectionCurveList = new List<Curve>();
            List<Point> IntersectionPointList = new List<Point>();

            GeometryUtilities.GetIntersection(
                Breps,
                Curves,
                PointList,
                ref IntersectionCurveList,
                ref IntersectionPointList,
                IntersectionCurveList,
                IntersectionPointList);

            foreach (var intersection_curve in IntersectionCurveList)
            {
                var user_data_edge = intersection_curve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;
                if (user_data_edge != null)
                {
                    user_data_edge.BrepId = brep_ids;
                    brep_ids++;
                }
                var user_data_curve = intersection_curve.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                if (user_data_curve != null)
                {
                    user_data_curve.BrepId = brep_ids;
                    brep_ids++;
                }
            }
            foreach (var intersection_point in IntersectionPointList)
            {
                var user_data_point = intersection_point.UserData.Find(typeof(UserDataPoint)) as UserDataPoint;
                user_data_point.BrepId = brep_ids;
                brep_ids++;
            }

            CocodriloPlugIn.Instance.IntersectionCurveList = IntersectionCurveList;
            CocodriloPlugIn.Instance.IntersectionPointList = IntersectionPointList;

            var breps = new DictList();

            var cp_id = 0;

            var vertices_dict = new DictList();
            var all_refinements_dict = new DictList();

            foreach (var brep in Breps)
            {
                var user_data_brep = UserDataUtilities.GetOrCreateUserDataBrep(brep);
                if (user_data_brep.BrepId == -1)
                {
                    user_data_brep.BrepId = brep_ids;
                    brep_ids++;
                }
                var brep_id_model = user_data_brep.BrepId;

                var faces_dict = new DictList();
                var edges_dict = new DictList();
                var this_vertices_dict = new DictList();

                GetNurbsGeometriesJSON(
                    brep,
                    ref cp_id,
                    ref rPropertyIdsBrepsIds,
                    ref faces_dict,
                    ref this_vertices_dict,
                    ref all_refinements_dict,
                    IntersectionCurveList,
                    IntersectionPointList);

                GetNurbsGeometriesEdgesJSON(
                    brep,
                    ref cp_id,
                    ref rPropertyIdsBrepsIds,
                    ref edges_dict);

                breps.Add(new Dict
                {
                    {"brep_id", brep_id_model},
                    {"faces", faces_dict},
                    {"edges", edges_dict},
                    {"vertices", this_vertices_dict}
                });
            }

            var curves_dict = new DictList();

            var curve_list = GeometryUtilities.GetCurveList();
            foreach (var curve in curve_list)
            {
                GetNurbsGeometriesCurvesJSON(
                    curve,
                    ref cp_id,
                    ref curves_dict,
                    ref vertices_dict,
                    IntersectionCurveList,
                    IntersectionPointList,
                    ref rPropertyIdsBrepsIds);
            }

            GetIntersectionPointsJSON(
                IntersectionPointList,
                ref cp_id,
                ref vertices_dict,
                ref rPropertyIdsBrepsIds);

            GetIntersectionCurvesJSON(
                IntersectionCurveList,
                ref cp_id,
                ref curves_dict,
                ref rPropertyIdsBrepsIds);

            if (curves_dict.Count > 0 || vertices_dict.Count > 0)
            {
                var user_data_brep_rest_geometries = new UserDataBrep();
                if (user_data_brep_rest_geometries.BrepId == -1)
                {
                    user_data_brep_rest_geometries.BrepId = brep_ids;
                    brep_ids++;
                }
                breps.Add(new Dict()
                {
                    {"brep_id", user_data_brep_rest_geometries.BrepId},
                    {"faces", new ArrayList()},
                    {"edges", curves_dict},
                    {"vertices", vertices_dict}
                });
            }

            var tolerances = new Dict
            {
                {"model_tolerance", RhinoDoc.ActiveDoc.ModelAbsoluteTolerance }
            };

            var dict = new Dict {
                { "tolerances", tolerances },
                { "version_number", 1.0 },
                { "breps", breps } };

            var serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = 2147483643;
            var geometry_string = serializer.Serialize(dict);
            System.IO.File.WriteAllLines(ProjectPath + "/" + "geometry.cad.json",
                new List<string> { geometry_string });

            var refinement_dict = new Dict { { "refinements", all_refinements_dict } };
            var refinement_string = serializer.Serialize(refinement_dict);
            System.IO.File.WriteAllLines(ProjectPath + "/" + "refinements.iga.json",
                new List<string> { refinement_string });
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ElementConditionDictionary"></param>
        /// <returns></returns>
        public string GetMaterials(PropertyIdDict ElementConditionDictionary, ref List<string> NodalVariables)
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
                NodalVariables.AddRange(
                    this_property.GetKratosOutputValuesNodes(CocodriloPlugIn.Instance.OutputOptions));

                //some properties do not need materials and properties
                if (!this_property.HasKratosMaterial())
                    continue;

                var variables = this_property.GetKratosVariables();

                int material_id = this_property.GetMaterialId();
                var property_dict = new Dict
                    {
                        {"model_part_name", "IgaModelPart." + this_property.GetKratosModelPart()},
                        {"properties_id", material_id},
                    };

                var material_dict = new Dict{};
                if(material_id >= 0)
                {
                    var material = CocodriloPlugIn.Instance.GetMaterial(material_id);
                    var material_variables = material.GetKratosVariables();
                    foreach (var material_variable in material_variables)
                        variables.Add(material_variable.Key, material_variable.Value);

                    material_dict.Add("name", material.Name);
                    material_dict.Add("constitutive_law", material.GetKratosConstitutiveLaw());

                    if (material.HasKratosSubProperties())
                    {
                        property_dict.Add("sub_properties", material.GetKratosSubProperties());
                    }

                    NodalVariables.AddRange(
                        material.GetKratosOutputValuesNodes(CocodriloPlugIn.Instance.OutputOptions));
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ElementConditionDictionary"></param>
        /// <returns></returns>
        public string GetPhysics(PropertyIdDict ElementConditionDictionary)
        {
            var element_condition_list = new DictList();

            foreach (var dict_entry in ElementConditionDictionary)
            {
                var this_property = CocodriloPlugIn.Instance.GetProperty(dict_entry.Key, out bool success);
                if (success)
                {
                    element_condition_list.AddRange(this_property.GetKratosPhysic(dict_entry.Value));
                }
            }

            var dict = new Dict
            {{ "element_condition_list", element_condition_list }};

            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize((object)dict);

            return json;
        }

        /// <summary>
        /// Returns the Project Parameters file as a string for KRATOS Multiphysics.
        /// </summary>
        /// <param name="ElementConditionDictionary"></param>
        /// <returns></returns>
        public void WriteProjectParameters(
            PropertyIdDict ElementConditionDictionary,
            List<string> NodalVariables,
            string ProjectPath)
        {
            string model_part_name = "IgaModelPart";
            string analysis_type = "linear";
            string solver_type_analysis = "static";
            double step_size = 1.0;
            int max_iteration = 1;
            double displacement_absolute_tolerance = 1.0e-9;
            double residual_absolute_tolerance = 1.0e-9;
            double end_time = 0.1;
            bool compute_reactions = true;
            bool rotation_dofs = false;
            if (this.analysis.GetType() == typeof(AnalysisLinear))
            {
                analysis_type = "linear";
            }
            if (this.analysis.GetType() == typeof(AnalysisNonLinear))
            {
                var non_linear_analysis = analysis as AnalysisNonLinear;
                analysis_type = "non_linear";
                step_size = non_linear_analysis.mStepSize;
                max_iteration = non_linear_analysis.mMaxSolverIteration;
                displacement_absolute_tolerance = non_linear_analysis.mSolverTolerance;
                residual_absolute_tolerance = non_linear_analysis.mSolverTolerance;
                end_time = non_linear_analysis.mNumSimulationSteps * step_size;
            }
            else if (this.analysis.GetType() == typeof(AnalysisTransient))
            {
                analysis_type = "non_linear";
                var transient_analysis = analysis as AnalysisTransient;
                step_size = transient_analysis.mStepSize;
                max_iteration = transient_analysis.MaxIter;
                displacement_absolute_tolerance = transient_analysis.tolerance;
                residual_absolute_tolerance = transient_analysis.tolerance;
                end_time = transient_analysis.NumStep * step_size - step_size * 9 / 10;
                solver_type_analysis = "dynamic";
            }
            else if (this.analysis.GetType() == typeof(AnalysisEigenvalue))
            {
                analysis_type = "linear";
                solver_type_analysis = "eigen_value";
            }
            else if (this.analysis.GetType() == typeof(AnalysisFormfinding))
            {
                var formfinding_analysis = analysis as AnalysisFormfinding;
                analysis_type = "non_linear";
                solver_type_analysis = "formfinding";
                max_iteration = formfinding_analysis.maxIterations;
                displacement_absolute_tolerance = formfinding_analysis.tolerance;
                residual_absolute_tolerance = formfinding_analysis.tolerance;
                end_time = formfinding_analysis.maxSteps * step_size - step_size * 9 / 10;
                compute_reactions = false;
            }
            else if (this.analysis.GetType() == typeof(AnalysisShapeOptimization))
            {
                analysis_type = "linear";
                max_iteration = 1;
            }

            // 1. problem data block
            var problem_data = new Dict
            {
                { "problem_name", analysis.Name + "_kratos"},
                { "echo_level", 0},
                { "parallel_type", "OpenMP"},
                { "start_time", 0},
                { "end_time", end_time}
            };

            // solver setting block
            var model_import_settings = 
                new Dict { {"input_type", "use_input_model_part"} };

            var time_stepping = new Dict
                { { "time_step", step_size} };

            var material_import_settings = 
                new Dict { { "materials_filename", "materials.json"} };

            var linear_solver_settings =
                new Dict {
                    { "solver_type", "LinearSolversApplication.sparse_lu" },
                    { "max_iteration", 500 },
                    { "tolerance", 1e-9 },
                    { "scaling", false },
                    { "verbosity", 1 } };

            // Modeler block
            var modelers = new DictList();

            var cad_io_modeler_parameters = new Dict
            {
                { "echo_level", 0 },
                { "cad_model_part_name", model_part_name },
                { "geometry_file_name", "geometry.cad.json" },
                { "output_geometry_file_name", analysis.Name + "_kratos_0.georhino.json" }
            };
            modelers.Add(new Dict
            {
                { "modeler_name", "CadIoModeler"},
                { "Parameters", cad_io_modeler_parameters}
            });
            var refinement_modeler_parameters = new Dict
            {
                { "echo_level", 0 },
                { "physics_file_name", "refinements.iga.json" }
            };
            modelers.Add(new Dict
            {
                { "modeler_name", "RefinementModeler"},
                { "Parameters", refinement_modeler_parameters}
            });
            var iga_modeler_parameters = new Dict
            {
                { "echo_level", 0 },
                { "cad_model_part_name", model_part_name },
                { "analysis_model_part_name", model_part_name },
                { "physics_file_name", "physics.iga.json"}
            };
            modelers.Add(new Dict
            {
                { "modeler_name", "IgaModeler"},
                { "Parameters", iga_modeler_parameters}
            });

            if (this.analysis.GetType() == typeof(AnalysisShapeOptimization))
            {
                (this.analysis as AnalysisShapeOptimization).WriteOptimizationParameters(modelers, model_part_name, ProjectPath);
                modelers.Clear();
            }

            // 3. process block
            var dirichlet_process_list = new DictList();
            var neumann_process_list = new DictList();
            var additional_processes = new DictList();
            var additional_variables = new List<string>();
            var additional_dofs = new List<string>();
            var additional_reactions = new List<string>();
            var output_process_list = new DictList();
            foreach (var dict_entry in ElementConditionDictionary)
            {
                var property = CocodriloPlugIn.Instance.GetProperty(dict_entry.Key, out bool get_property_success);
                if (!get_property_success)
                    continue;

                if (property.RotationDofs() == true) {
                    rotation_dofs = true;
                }
                if(property.GetAdditionalDofs() != "") {
                    additional_variables.Add(property.GetAdditionalDofs());
                    additional_dofs.Add(property.GetAdditionalDofs());
                }
                if (property.GetAdditionalDofReactions() != "") {
                    additional_variables.Add(property.GetAdditionalDofReactions());
                    additional_reactions.Add(property.GetAdditionalDofReactions());
                }

                if (property.GetType() == typeof(PropertyCoupling)
                    || property.GetType() == typeof(PropertySupport))
                {
                    dirichlet_process_list.AddRange(property.GetKratosProcesses());
                }
                else if (property.GetType() == typeof(PropertyLoad))
                {
                    neumann_process_list.AddRange(property.GetKratosProcesses());
                }
                else if(property.GetType() == typeof(PropertyCheck))
                {
                    output_process_list.AddRange(property.GetKratosProcesses());
                }
                else if (property.GetType() == typeof(PropertyShell))
                {
                    additional_processes.AddRange(property.GetKratosProcesses(dict_entry.Value.Select(item => item.BrepId).ToList()));
                }

                var output_integration_domain_process_dict = property.GetKratosOutputIntegrationDomainProcess(
                    CocodriloPlugIn.Instance.OutputOptions, analysis.Name, model_part_name);
                if (output_integration_domain_process_dict.Count > 0)
                    additional_processes.Add(output_integration_domain_process_dict);

                // output processes
                var output_process_dict = property.GetKratosOutputProcess(
                    CocodriloPlugIn.Instance.OutputOptions, analysis, model_part_name);

                if (output_process_dict.Count > 0)
                    output_process_list.Add(output_process_dict);
            }

            if (this.analysis.GetType() == typeof(AnalysisTransient))
            {
                var transient_analysis = analysis as AnalysisTransient;
                if(transient_analysis.AutomaticRayleigh == true)
                {
                    var eigen_settings_parameters = new Dict
                    {
                        { "solver_type", "eigen_eigensystem" },
                        { "max_iteration", 100 },
                        { "tolerance", 0.001 },
                        { "number_of_eigenvalues", transient_analysis.NumEigen },
                        { "echo_level", 0 }
                    };

                    var automatic_rayleigh_parameters = new Dict
                    {
                        { "echo_level", 0 },
                        { "write_on_properties", false },
                        { "model_part_name", model_part_name },
                        { "damping_ratio_0", transient_analysis.DampingRatio0 },
                        { "damping_ratio_1", transient_analysis.DampingRatio1 },
                        { "eigen_system_settings", eigen_settings_parameters }
                    };

                    additional_processes.Add(new Dict
                    {
                        { "kratos_module", "KratosMultiphysics.StructuralMechanicsApplication"},
                        { "python_module", "automatic_rayleigh_parameters_computation_process"},
                        { "process_name", "AutomaticRayleighComputationProcess"},
                        { "Parameters", automatic_rayleigh_parameters}
                    });
                }
            }


            if (this.analysis.GetType() == typeof(AnalysisFormfinding))
            {
                foreach(var output_process in output_process_list)
                {
                    output_process.Add("form_finding", true);
                }
            }

            var output_processes = new Dict { { "output_process_list", output_process_list } };

            if (this.analysis.GetType() == typeof(AnalysisEigenvalue))
            {
                output_processes["output_process_list"] =  new ArrayList();

                additional_processes.Add(new Dict {
                    { "kratos_module", "IgaApplication" },
                    { "python_module", "output_eigen_values_process" },
                    { "Parameters", new Dict { 
                        { "output_file_name", analysis.Name + "_kratos_eigen_values.post.res" },
                        { "model_part_name", model_part_name }} },
                });
            }

            var processes = new Dict
            {
                { "additional_processes", additional_processes},
                { "dirichlet_process_list", dirichlet_process_list},
                { "neumann_process_list",   neumann_process_list}
            };

            var solver_settings = new Dict
            {
                { "model_part_name", model_part_name},
                { "domain_size", 1},
                { "echo_level", 1},
                { "buffer_size", 2},
                { "analysis_type", analysis_type},
                { "model_import_settings", model_import_settings},
                { "material_import_settings", material_import_settings},
                { "time_stepping", time_stepping},
                { "rotation_dofs", rotation_dofs},
                { "reform_dofs_at_each_step", false },
                { "line_search", false},
                { "compute_reactions", compute_reactions},
                { "block_builder", true },
                { "clear_storage", false },
                { "move_mesh_flag", true },
                { "convergence_criterion", "residual_criterion" },
                { "displacement_relative_tolerance", 1.0e-4 },
                { "displacement_absolute_tolerance", displacement_absolute_tolerance },
                { "residual_relative_tolerance", 1.0e-4 },
                { "residual_absolute_tolerance", residual_absolute_tolerance },
                { "max_iteration", max_iteration },
                { "solver_type", solver_type_analysis },
                { "linear_solver_settings", linear_solver_settings }
            };

            // Additional solver settings for formfinding
            if (this.analysis.GetType() == typeof(AnalysisFormfinding))
            {
                var projection_settings = new Dictionary<string, object>
                {
                    { "model_part_name", "IgaModelPart"},
                    { "echo_level", 1},
                    { "projection_type", "planar"},
                    { "global_direction", new object[] { 1.0, 0.0, 0.0 }},
                    { "variable_name", "LOCAL_PRESTRESS_AXIS_1"},
                    { "method_specific_settings", new object{}},
                    { "check_local_space_dimension", false}
                };
                solver_settings["reform_dofs_at_each_step"] = true;
                solver_settings.Add("projection_settings", projection_settings);
                solver_settings.Add("printing_format", "gid");
                solver_settings.Add("write_formfound_geometry_file", true);
            }

            // Additional integration method and scheme in solver settings for transient analysis
            if (this.analysis.GetType() == typeof(AnalysisTransient))
            {
                var transient_analysis = analysis as AnalysisTransient;
                solver_settings.Add("time_integration_method", transient_analysis.TimeInteg);
                solver_settings.Add("scheme_type", transient_analysis.Scheme);
                solver_settings.Add("rayleigh_alpha", transient_analysis.RayleighAlpha);
                solver_settings.Add("rayleigh_beta", transient_analysis.RayleighBeta);
            }

            // Additional settings and scheme in solver settings for eigenvalue analysis
            if (this.analysis.GetType() == typeof(AnalysisEigenvalue))
            {
                var eigenvalue_analysis = analysis as AnalysisEigenvalue;
                var eigensolver_settings = new Dict
                {
                    {"solver_type", eigenvalue_analysis.mSolverType },
                    {"max_iteration", eigenvalue_analysis.mMaximumIterations },
                    {"number_of_eigenvalues", eigenvalue_analysis.mNumEigenvalues },
                    {"echo_level", 4 }
                };
                solver_settings.Remove("linear_solver_settings");
                solver_settings.Add("eigensolver_settings", eigensolver_settings);
            }

            // Additional dofs in solver setings if required, as e.g. for the Lagrange Multiplier method.
            if (additional_dofs.Count == 0) {
                solver_settings.Add("auxiliary_variables_list", new ArrayList());
                solver_settings.Add("auxiliary_dofs_list", new ArrayList());
                solver_settings.Add("auxiliary_reaction_list", new ArrayList());
            }
            else
            {
                solver_settings.Add("auxiliary_variables_list", additional_variables.Distinct().ToList());
                solver_settings.Add("auxiliary_dofs_list", additional_dofs.Distinct().ToList());
                solver_settings.Add("auxiliary_reaction_list", additional_reactions.Distinct().ToList());
            }

            var dict = new Dict
            {
                {"problem_data", problem_data},
                {"solver_settings", solver_settings},
                {"modelers", modelers},
                {"processes", processes},
                {"output_processes", output_processes}
            };

            var serializer = new JavaScriptSerializer();
            string project_parameters_json = serializer.Serialize((object)dict);

            System.IO.File.WriteAllLines(ProjectPath + "/" + "ProjectParameters.json",
                new List<string> { project_parameters_json });
        }

        /// <summary>
        /// Creates a dictionary for JSON files for surface geometries.
        /// The format is defined in https://amses-journal.springeropen.com/articles/10.1186/s40323-018-0109-4
        /// </summary>
        /// <param name="brep">Input Brep for which the surface geometries are obtained</param>
        /// <param name="rCpId">Counter for the Control Points in 3D space (Degrees of Freedom)</param>
        /// <param name="rPropertyElements">List to relate property ids to the respective brep_ids</param>
        /// <param name="rFacesDictionary"></param>
        /// <param name="rVerticesDictionary"></param>
        /// <param name="rRefinementsDictionary"></param>
        /// <param name="IntersectionCurveList"></param>
        /// <param name="IntersectionPointList"></param>
        /// <returns></returns>
        public void GetNurbsGeometriesJSON(
            Brep brep,
            ref int rCpId,
            ref PropertyIdDict rPropertyElements,
            ref DictList rFacesDictionary,
            ref DictList rVerticesDictionary,
            ref DictList rRefinementsDictionary,
            List<Curve> IntersectionCurveList,
            List<Point> IntersectionPointList)
        {
            Rhino.Geometry.Collections.BrepSurfaceList surfaces = brep.Surfaces;

            foreach (var rhino_surface in surfaces)
            {
                var user_data_surface = UserDataUtilities.GetOrCreateUserDataSurface(rhino_surface);

                var nurbs_surface = rhino_surface.ToNurbsSurface();
                //to keep the id for this iteration (r_brep_id can be increased with adding new entities)
                int this_brep_surface_id = user_data_surface.BrepId;
                //trim index for additional trim entities as vertices or embedded loops
                int this_trim_index_iterator = brep.Trims.Count + 1;

                var refinements = user_data_surface.GetRefinement();
                rRefinementsDictionary.Add(refinements?.GetKratosRefinement(this_brep_surface_id));

                user_data_surface.TryGetKratosPropertyIdsBrepIds(
                    ref rPropertyElements);

                var embedded_points = new DictList();
                var embedded_edges = new DictList();

                var embedded_points_surface = user_data_surface.GetNumericalElements().Where(item => item.HasBrepId()).ToArray();

                var embedded_points_surfac2e = user_data_surface.GetNumericalElements();
                foreach (var embedded_point in embedded_points_surface)
                {
                    this_trim_index_iterator++;

                    var parameter_location = embedded_point.mParameterLocation.GetPoint2d();

                    rVerticesDictionary.Add(OutputUtilitiesJSON.CreateVertexDictionary(
                        embedded_point.GetBrepId(),
                        new Coupling(this_brep_surface_id, this_trim_index_iterator, 1),
                        nurbs_surface.PointAt(
                            parameter_location.X,
                            parameter_location.Y),
                        parameter_location));
                }

                foreach (var coupling_point in IntersectionPointList)
                {
                    var user_data_point = coupling_point.UserData.Find(typeof(UserDataPoint)) as UserDataPoint;
                    if (user_data_point == null)
                        continue;
                    if (user_data_point.IsCoupledWith(this_brep_surface_id))
                    {
                        nurbs_surface.ClosestPoint(
                            coupling_point.Location, 
                            out double u, out double v);

                        user_data_point.TryAddLocalCoordinates(
                            this_brep_surface_id,
                            u, v, 0.0);
                    }
                }

                foreach (var coupling_edge in IntersectionCurveList)
                {
                    var user_data_edge = coupling_edge.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;
                    if (user_data_edge == null)
                        continue;
                    if (user_data_edge.IsCoupledWith(this_brep_surface_id))
                    {
                        this_trim_index_iterator++;

                        user_data_edge.TryAddTrimIndex(
                            this_brep_surface_id,
                            this_trim_index_iterator);

                        var curve = nurbs_surface.Pullback(
                            coupling_edge, 
                            RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);

                        if (curve == null) {
                            RhinoApp.WriteLine("Cannot find coupling/ embedded edge");
                            continue;
                        }

                        embedded_edges.Add(OutputUtilitiesJSON.CreateEmbeddedCurveDictionary(
                            this_trim_index_iterator,
                            ref rCpId,
                            curve));
                    }
                }

                var surface_knot_vector = new List<List<double>>();
                surface_knot_vector.Add(OutputUtilitiesJSON.CreateKnotVector(nurbs_surface.KnotsU, nurbs_surface.IsPeriodic(0)));
                surface_knot_vector.Add(OutputUtilitiesJSON.CreateKnotVector(nurbs_surface.KnotsV, nurbs_surface.IsPeriodic(0)));

                var control_points = OutputUtilitiesJSON.CreateSurfaceControlPointList(nurbs_surface, ref rCpId);

                var surfaceListData = new Dict
                {
                    {"is_trimmed", true},
                    {"is_rational", nurbs_surface.IsRational},
                    {"degrees", new double[] { nurbs_surface.Degree(0), nurbs_surface.Degree(1)}},
                    {"knot_vectors", surface_knot_vector},
                    {"control_points", control_points}
                };

                DictList boundary_loops = new DictList();
                bool is_surface_normal_swapped = false;

                foreach (var loop in brep.Loops)
                {
                    //Skips the loops which are not depending to this surface
                    if (rhino_surface != surfaces[loop.Face.SurfaceIndex])
                        continue;
                    is_surface_normal_swapped = loop.Face.OrientationIsReversed;

                    var trimming_curves = new DictList();

                    var trims = loop.Trims;

                    for (int t = 0; t < trims.Count(); t++)
                    {
                        BrepTrim trim = trims[t];
                        var curve = brep.Curves2D[trim.TrimCurveIndex];

                        if (trim.Edge == null)
                        {
                            continue;
                        }

                        trimming_curves.Add(
                            OutputUtilitiesJSON.CreateEmbeddedCurveDictionary(
                                trim.TrimIndex, ref rCpId, curve, false));
                    }
                    string LoopTypeText = OutputUtilitiesJSON.CreateLoopType(loop);

                    boundary_loops.Add(new Dict
                    {
                        {"loop_type", LoopTypeText},
                        {"trimming_curves", trimming_curves}
                    });
                }

                var embedded_loops = new DictList();
                var Face = new Dict
                {
                    {"brep_id", this_brep_surface_id},
                    {"swapped_surface_normal", is_surface_normal_swapped},
                    {"surface", surfaceListData},
                    {"boundary_loops", boundary_loops},
                    {"embedded_loops", embedded_loops},
                    {"embedded_edges", embedded_edges},
                    {"embedded_points", embedded_points}
                };
                rFacesDictionary.Add(Face);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="brep"></param>
        /// <param name="rCpId"></param>
        /// <param name="rPropertyElements"></param>
        /// <param name="rEdgesDictionary"></param>
        private void GetNurbsGeometriesEdgesJSON(
            Brep brep,
            ref int rCpId,
            ref PropertyIdDict rPropertyElements,
            ref DictList rEdgesDictionary)
        {
            Rhino.Geometry.Collections.BrepEdgeList edges = brep.Edges;

            foreach (var edge in edges)
            {
                var trims = new DictList();

                if (edge.TrimIndices().Count() < 1)
                    continue;

                var trimIndex = edge.TrimIndices()[0];

                var trim = edge.Brep.Trims[trimIndex];

                var user_data_surface = (UserDataSurface)edge.Brep.Surfaces[trim.Face.FaceIndex].UserData
                    .Find(typeof(UserDataSurface));
                if (user_data_surface == null)
                {
                    RhinoApp.WriteLine("WARNING: User data of surface not found!");
                    continue;
                }
                int brep_id_face = user_data_surface.BrepId;

                if (trim.TrimCurveIndex == -1)
                {
                    RhinoApp.WriteLine("ERROR: Trim without curve found. Surface: " + user_data_surface.BrepId);
                    continue;
                }

                var curve = edge.Brep.Curves2D[trim.TrimCurveIndex];
                var user_data_edge = curve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;

                int brep_id_edge = user_data_edge.BrepId;

                if (edge.TrimIndices().Count() > 1)
                {
                    bool rotational_continuity = user_data_surface.CheckRotationalContinuity();
                    var this_support = new Support(
                        true, true, true,
                        "0.0", "0.0", "0.0",
                        rotational_continuity, false);
                    var property_coupling = new PropertyCoupling(
                        GeometryType.SurfaceEdgeSurfaceEdge,
                        this_support);

                    user_data_edge.AddNumericalElement(property_coupling);
                }

                user_data_edge.TryGetKratosPropertyIdsBrepIds(
                    ref rPropertyElements);

                var trimDict = new Dict
                {
                    {"brep_id", brep_id_face},
                    {"trim_index", trim.TrimIndex},
                    {"relative_direction", !trim.IsReversed()}
                };

                trims.Add(trimDict);

                if (edge.TrimIndices().Count() > 1)
                {
                    for (var index = 1; index < edge.TrimIndices().Length; index++)
                    {
                        trimIndex = edge.TrimIndices()[index];

                        trim = edge.Brep.Trims[trimIndex];

                        var user_data_surface_2 = UserDataUtilities.GetOrCreateUserDataSurface(edge.Brep.Surfaces[trim.Face.FaceIndex]);

                        var trim_dict = new Dict
                        {
                            {"brep_id", user_data_surface_2.BrepId},
                            {"trim_index", trim.TrimIndex},
                            {"relative_direction", !trim.IsReversed()}
                        };

                        trims.Add(trim_dict);
                    }
                }

                Dict curve_dict;

                if (edge.EdgeIndex >= edge.Brep.Curves3D.Count)
                {
                    curve_dict = OutputUtilitiesJSON.CreateEdgeDictionary(
                    brep_id_edge,
                    new Coupling(0),
                    ref rCpId);
                }
                else
                {
                    curve_dict = OutputUtilitiesJSON.CreateCurveDictionary(
                        brep_id_edge,
                        new Coupling(0),
                        ref rCpId,
                        edge.Brep.Curves3D[edge.EdgeIndex]);
                }

                curve_dict["topology"] = trims;

                rEdgesDictionary.Add(curve_dict);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ThisCurve"></param>
        /// <param name="rCpId"></param>
        /// <param name="rCurvesDictionary"></param>
        /// <param name="rVerticesDictionary"></param>
        /// <param name="IntersectionCurveList"></param>
        /// <param name="IntersectionPointList"></param>
        /// <param name="rPropertyElements"></param>
        private void GetNurbsGeometriesCurvesJSON(
            Curve ThisCurve,
            ref int rCpId,
            ref DictList rCurvesDictionary,
            ref DictList rVerticesDictionary,
            List<Curve> IntersectionCurveList,
            List<Point> IntersectionPointList, 
            ref PropertyIdDict rPropertyElements)
        {
            var user_data_curve = ThisCurve.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
            if (user_data_curve == null) return;

            user_data_curve.TryGetKratosPropertyIdsBrepIds(ref rPropertyElements);

            var this_coupling = new Coupling(0);

            var curve_dict = OutputUtilitiesJSON.CreateCurveDictionary(
                    user_data_curve.BrepId,
                    this_coupling,
                    ref rCpId, 
                    ThisCurve);

            int this_trim_index_iterator = 0;
            int this_curve_id = user_data_curve.BrepId;

            var embedded_points = new DictList();
            var embedded_edges = new DictList();

            var embedded_points_curve = user_data_curve.GetNumericalElements();
            foreach (var embedded_point in embedded_points_curve)
            {
                this_trim_index_iterator++;

                var parameter_location = embedded_point.mParameterLocation.GetParameters();

                rVerticesDictionary.Add(OutputUtilitiesJSON.CreateVertexDictionary(
                    embedded_point.GetBrepId(),
                    new Coupling(this_curve_id, this_trim_index_iterator, 1),
                    parameter_location[0]));
            }

            foreach (var coupling_point in IntersectionPointList)
            {
                var user_data_point = coupling_point.UserData.Find(typeof(UserDataPoint)) as UserDataPoint;
                if (user_data_point == null)
                    continue;
                if (user_data_point.IsCoupledWith(this_curve_id))
                {
                    ThisCurve.ClosestPoint(
                        coupling_point.Location,
                        out double u);

                    user_data_point.TryAddLocalCoordinates(
                        this_curve_id,
                        u, 0.0, 0.0);
                }
            }

            foreach (var coupling_edge in IntersectionCurveList)
            {
                var user_data_edge = coupling_edge.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;
                if (user_data_edge == null)
                    continue;
                if (user_data_edge.IsCoupledWith(this_curve_id))
                {
                    this_trim_index_iterator++;

                    user_data_edge.TryAddTrimIndex(
                        this_curve_id,
                        this_trim_index_iterator);

                    var active_range = new double[] {0.0, 0.0};
                    if (ThisCurve.ClosestPoint(coupling_edge.PointAtStart, out double t1))
                        active_range[0] = t1;
                    if (ThisCurve.ClosestPoint(coupling_edge.PointAtEnd, out double t2))
                        active_range[1] = t2;

                    var embedded_range = new Dict()
                    {
                        {"trim_index", this_trim_index_iterator},
                        {"active_range", active_range}
                    };

                    embedded_edges.Add(embedded_range);
                }
            }
            curve_dict.Add("embedded_points", embedded_points);
            curve_dict.Add("embedded_edges", embedded_edges);

            rCurvesDictionary.Add(curve_dict);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntersectionPoints"></param>
        /// <param name="rCpId"></param>
        /// <param name="rVerticesDictionary"></param>
        /// <param name="rPropertyElements"></param>
        private void GetIntersectionPointsJSON(
            List<Point> IntersectionPoints,
            ref int rCpId,
            ref DictList rVerticesDictionary,
            ref PropertyIdDict rPropertyElements)
        {
            foreach (var intersection_point in IntersectionPoints)
            {
                rCpId++;
                var user_data_point = intersection_point.UserData.Find(typeof(UserDataPoint)) as UserDataPoint;
                if (user_data_point == null)
                    continue;

                user_data_point.TryGetKratosPropertyIdsBrepIds(ref rPropertyElements);

                rVerticesDictionary.Add(
                    OutputUtilitiesJSON.CreateVertexDictionary(
                        user_data_point.BrepId, 
                        user_data_point.GetCoupling(), 
                        ref rCpId, 
                        intersection_point.Location));
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="IntersectionCurves"></param>
        /// <param name="rCpId"></param>
        /// <param name="rCurveDictionary"></param>
        /// <param name="rPropertyElements"></param>
        private void GetIntersectionCurvesJSON(
            List<Curve> IntersectionCurves,
            ref int rCpId,
            ref DictList rCurveDictionary,
            ref PropertyIdDict rPropertyElements)
        {
            foreach (var intersection_curve in IntersectionCurves)
            {
                rCpId++;
                var user_data_edge = intersection_curve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;
                if (user_data_edge == null)
                    continue;

                user_data_edge.TryGetKratosPropertyIdsBrepIds(ref rPropertyElements);

                rCurveDictionary.Add(
                    OutputUtilitiesJSON.CreateCurveDictionary(
                        user_data_edge.BrepId,
                        user_data_edge.GetCoupling(),
                        ref rCpId,
                        intersection_curve));
            }
        }
    }
}
