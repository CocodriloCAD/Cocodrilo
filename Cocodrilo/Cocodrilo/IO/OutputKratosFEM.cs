using Cocodrilo.UserData;
using Rhino;
using Rhino.Geometry;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Cocodrilo.Analyses;
using System.IO;
using Cocodrilo.ElementProperties;


namespace Cocodrilo.IO
{
    using Dict = Dictionary<string, object>;
    using DictList = List<Dictionary<string, object>>;
    using IdDict = Dictionary<int, List<int>>;
    using PropertyIdDict = Dictionary<int, List<BrepToParameterLocations>>;
    public class OutputKratosFEM : Output
    {
        public OutputKratosFEM() : base(new Analyses.Analysis("FEM"))
        {
        }

        public OutputKratosFEM(Analyses.Analysis analysis):base(analysis)
        {
            this.analysis = analysis;
        }

        
        // Wofür war diese Funktion? Scheint nicht mehr benötigt zu werden.
        //public void StartAnalysis(List<Mesh> MeshList, ref Cocodrilo.Analyses.Analysis analysis)
        //{
        //    string project_path = UserDataUtilities.GetProjectPath("DEM");

        //    StartAnalysis(project_path, MeshList, ref Cocodrilo.Analyses.Analysis analysis);
        //}

        ///overloading StartAnalysis function for MPM Analysis
        ///

        public void StartAnalysis(List<Mesh> MeshList, ref Cocodrilo.Analyses.Analysis analysis)
        {
            string project_path = UserDataUtilities.GetProjectPath(analysis.Name);

            StartAnalysis(project_path, MeshList, ref analysis);
        }

        public void StartAnalysis(string project_path, List<Mesh> MeshList, ref Cocodrilo.Analyses.Analysis analysis)
        {
            try
            {
                PropertyIdDict property_id_dictionary = new PropertyIdDict();
                PropertyIdDict rPropertyIdsBrepsIds = new PropertyIdDict();
                         
                ///Different output files for FEM and MPM
                
                if (analysis is Cocodrilo.Analyses.AnalysisMpm_new)
                {
                    // downcast to access Bodymesh
                    Cocodrilo.Analyses.AnalysisMpm_new outputCopy = (Cocodrilo.Analyses.AnalysisMpm_new) analysis;

                    // meshing quantities: get physical values via referencing in WriteBodyMesh, think of more elegant way later!
                    // WriteBodyMdpaFile obtains reference to bodyMesh as nodes of BodyMesh are required in background grid, too.

                    Mesh bodyMesh = null;
                    
                    System.IO.File.WriteAllLines(project_path + "/" + "Body.mdpa", new List<string> {
                        WriteBodyMdpaFile(outputCopy.mBodyMesh, outputCopy.mCurveList, ref property_id_dictionary, ref bodyMesh) });
                    

                    //call of WriteProjectParameters with downcasted analysis to access class members

                    System.IO.File.WriteAllLines(project_path + "/" + "ProjectParamaters.json", new List<string> { WriteProjectParameters(project_path, ref outputCopy) });
                    
                    System.IO.File.WriteAllLines(project_path + "/" + "Materials.json", new List<string> { GetMaterials(property_id_dictionary) });
                              
                    System.IO.File.WriteAllLines(project_path + "/" + "Grid.mdpa",
                        new List<string> { GetFemMdpaFile(MeshList, outputCopy.mCurveList, ref property_id_dictionary) });
                                        
                }
                else
                {
                    System.IO.File.WriteAllLines(project_path + "/" + "Grid.mdpa",
                       new List<string> { GetFemMdpaFile(MeshList, new List<Curve>(), ref property_id_dictionary) });
                    System.IO.File.WriteAllLines(project_path + "/" + "Materials.json",
                        new List<string> { "" });
                    //Overload writeProjectParameters to work also with objects of type Analysis
                    //System.IO.File.WriteAllLines(project_path + "/" + "ProjectParamaters.json",
                    //new List<string> { WriteProjectParameters(project_path,ref analysis) });
                    // Cocodrilo Warnung ausgeben!!
                }

            }
            catch (Exception ex)
            {
                RhinoApp.WriteLine("Json output not possible.");
                RhinoApp.WriteLine(ex.ToString());
            }
        }

        ///new WriteGeometryJson-Function, similiar to the one of OutputKratosIGA:

        private string WriteBodyMdpaFile(
            List<Mesh> MeshList,
            List<Curve> CurveList,
            ref PropertyIdDict rPropertyIdsBrepsIds,
            ref Mesh bodyMesh)

        {
            int brep_ids = 1;

            //value of brep_ids should be changed after calling GeomteryUtilities.AssignBrepIds
            GeometryUtilities.AssignBrepIdToMesh(MeshList, ref brep_ids);

            string mdpa_file;

            mdpa_file = "Begin ModelPartData\n"
                        + "//  VARIABLE_NAME value\n"
                        + "End ModelPartData\n\n";

            
            foreach (var mesh in MeshList)
            {
                var user_data_mesh = mesh.UserData.Find(typeof(UserDataMesh)) as UserDataMesh;

                user_data_mesh.TryGetKratosPropertyIdsBrepIds(
                    ref rPropertyIdsBrepsIds);
                        
            }

            for (int i = 0; i < MeshList.Count; i++)
            {
                mdpa_file += "Begin Properties " + i.ToString() + "\n End Properties \n";
            }
            mdpa_file += "\n\n";


            string node_string = "Begin Nodes\n";


            string element_string = "Begin Elements UpdatedLagrangian2D4N// ";

            //assign correct model part name to model: still hardcoded - remove in later version!
            element_string += "GUI group identifier: Solid Auto1 \n";

            int id_node_counter = 1;
            int id_element_counter = 1;

            int sub_model_part_counter = 1;

            string sub_model_part_string = "";

            for (int m = 0; m < MeshList.Count; m++)
            {

                sub_model_part_string += "Begin SubModelPart Parts_Solid_Solid_Auto1 // Group Solid Auto1 // Subtree Parts_Solid\n";
                                
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
                    element_string += "    " + id_element_counter + "  " + (0).ToString() + "  " + (face.A + id_node_counter) + "  " + (face.B + id_node_counter) + "  " + (face.C + id_node_counter) + "   " + (face.D + id_node_counter) + "\n";
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


        private string GetFemMdpaFile(List<Mesh> MeshList, List<Curve> CurveList, ref PropertyIdDict PropertyIdDictionary)
        {
            int brep_ids = 1;

            //Creation of new (empty) lists of Breps and Points to use already existing function AssignBrepIds
            GeometryUtilities.AssignBrepIds(new List<Brep>(), CurveList, new List<Point>(), ref brep_ids);

            //value of brep_ids should be changed after calling GeomteryUtilities.AssignBrepIds
            GeometryUtilities.AssignBrepIdToMesh(MeshList, ref brep_ids);

            //Rhino.Geometry.Collections.MeshTopologyEdgeList mesh_edges = brep.Meshes;
            // Iteration über richtige Netze? 

            // Tables for points belonging to edges and curves

            Hashtable nodes_of_edges = new Hashtable();
            Hashtable nodes_of_curves = new Hashtable();

            List<Point3d> startEndPointsCurve = new List<Point3d>();

            foreach (var curve in CurveList)
            {
                var user_data_edge = curve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;

                if (user_data_edge != null)
                {
                    foreach (var mesh in MeshList)
                    {
                        // Gets a polyline approximation of the input curve and then moves its control points
                        // to the closest point on the mesh. Then it "connects the points" over edges so
                        // that a polyline on the mesh is formed.

                        var polyline = mesh.PullCurve(curve, 0.01);

                        // get PropertyIds and BrepIds used by Kratos of the element user_data_edge
                        user_data_edge.TryGetKratosPropertyIdsBrepIds(
                            ref PropertyIdDictionary);

                        // Makes a polyline approximation of the curve and gets the closest point on the
                        // mesh for each point on the curve. Then it "connects the points" so that you have
                        // a polyline on the mesh.

                        // Returns undelying polyline or points
                        var polyLineCurve = polyline.PullToMesh(mesh, 0.01);


                        // Give meaningful names!!
                        Polyline pol = polyLineCurve.ToPolyline();

                        
                        foreach (var point in pol)
                        {
                            var closest_point = mesh.ClosestMeshPoint(point, 0.01);
                            closest_point.GetHashCode();
                            nodes_of_edges.Add(closest_point.GetHashCode(), closest_point);
                                           
                            
                        }

                    }
                }
                                          
                var user_data_curve = curve.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;

                if (user_data_curve != null)
                {
                    foreach (var mesh in MeshList)
                    {
                        var polyline = mesh.PullCurve(curve, 0.05);

                        user_data_curve.TryGetKratosPropertyIdsBrepIds(
                            ref PropertyIdDictionary);

                        var poly_curve = polyline.PullToMesh(mesh, 0.05);

                        Polyline pol = poly_curve.ToPolyline();

                        startEndPointsCurve.Add(pol.First());
                        startEndPointsCurve.Add(pol.Last());
                        
                        foreach (var point in pol)
                        {
                            var closest_point = mesh.ClosestPoint(point);
                            closest_point.GetHashCode();
                            nodes_of_curves.Add(closest_point.GetHashCode(), closest_point);
                                   
                        }
                        //for (int i = 1; i < polyline.PointCount - 1; i++)
                        //{
                        //    //round depending on size of mesh
                        //    //double tempRoundX = Math.Round(pol[i].X, 2);
                        //    //double tempRoundY = Math.Round(pol[i].Y, 2);
                        //    //double tempRoundZ = Math.Round(pol[i].Z, 2);

                        //    //var point = new Point3d(tempRoundZ, tempRoundY, tempRoundZ);

                        //    //relate maximum distance to size of mesh
                        //    var closest_point = mesh.ClosestPoint(pol[i]);
                        //    closest_point.GetHashCode();
                        //    nodes_of_curves.Add(closest_point.GetHashCode(), closest_point);

                        //    nodesCurves.Add(closest_point);


                        //}
                    }
                }
            }

            foreach (var mesh in MeshList)
            {
                var user_data_mesh = mesh.UserData.Find(typeof(UserDataMesh)) as UserDataMesh;

                user_data_mesh.TryGetKratosPropertyIdsBrepIds(
                    ref PropertyIdDictionary);
            }

            string mdpa_file;

            mdpa_file = "Begin ModelPartData\n"
                        + "//  VARIABLE_NAME value\n"
                        + "End ModelPartData\n\n";

            //Why does for-loop start at two here? In GiD -.mdpa file, starts at properties 0

            for (int i = 0; i < MeshList.Count; i++)
            {
                mdpa_file += "Begin Properties " + i.ToString() + "\n End Properties \n";
            }
            mdpa_file += "\n\n";

            string node_string = "Begin Nodes\n";
            string element_string = "Begin Conditions Element2D4N// GUI group identifier: Grid Auto1 \n";
            int id_node_counter = 1;
            int id_element_counter = 1;

            int sub_model_part_counter = 1;
            //How to get total number of submodelparts?

            List<string> submodelparts = new List<string>();

            foreach (var property_id in PropertyIdDictionary.Keys)
            {
                var this_property = CocodriloPlugIn.Instance.GetProperty(property_id, out bool success);

                submodelparts.Add(this_property.GetKratosModelPart());
            };


            
            string sub_model_part_string = "";
            string sub_model_part_displacement_boundary = "";

            for (int m = 0; m < MeshList.Count; m++)
            {

                // Group Name and Submodelpart Name must become dynamic parameters
                // use here "Begin"+CocodriloPlugIn.Instance.GetProperty(property_id, out bool success)+"...

                sub_model_part_string += "Begin SubModelPart Parts_Solid_Solid_Auto1 // Group Grid Auto1 // Subtree Parts_Grid\n" +
                                         "  Begin SubModelPartNodes\n";

                //make modelpartname, group and subtree automatic
                sub_model_part_displacement_boundary += "Begin SubModelPart DISPLACEMENT_Displacement_Auto1 // Group Displacement Auto1 // Subtree DISPLACEMENT\n" +
                                         "  Begin SubModelPartNodes\n";

                var mesh = MeshList[m];

                for (int i = 0; i < mesh.Vertices.Count; i++)
                {
                    node_string += "    " + (id_node_counter + i).ToString() + " " + mesh.Vertices[i].X + " " + mesh.Vertices[i].Y + " " + mesh.Vertices[i].Z + "\n";
                    sub_model_part_string += "     " + (id_node_counter + i).ToString() + "\n";

                    // if loop to iterate over different submodelparts 

                    //if(==) hashcode is in the hashtable of a submodelpart, then do check for index of the respective node in overall node numbering
                    //    sub_model_list(indexer ==).Add(sub_model_part_string += "     " + (id_node_counter + i).ToString() + "\n")


                    // bool to decide whether to check if current vertex corresponds to an inner point of a 
                    // boundary condition

                    bool checkForInnerPointOfBoundaryCondition = true;

                    //Get node fo mesh that is closest to 1st and last point of curve

                    foreach (Point3d startEndPoint in startEndPointsCurve)
                    {
                        if (startEndPoint.DistanceToSquared(mesh.Vertices[i]) < 0.01)
                        {
                            sub_model_part_displacement_boundary += "     " + (id_node_counter + i).ToString() + " Start/End-Point \n";
                            checkForInnerPointOfBoundaryCondition = false;
                        }

                    }

                    //create testpoint to see if current mesh-vertex is included in boundary conditions. This seems to be necessary as 
                    //vertices and points seem to have different hash-codes


                    if (checkForInnerPointOfBoundaryCondition)
                    {
                        Point3d testpoint = mesh.Vertices[i];

                        // Add correct submodelpart for nodes from edges
                        if (nodes_of_edges.ContainsKey(testpoint.GetHashCode()))
                        {
                            var variable_test = nodes_of_edges[testpoint.GetHashCode()];
                            Point3d node_from_edge = (Point3d)nodes_of_edges[testpoint.GetHashCode()];

                            if (node_from_edge == null)
                                continue;

                            //maybe try make threshold depended on mesh size

                            if (node_from_edge.DistanceToSquared(mesh.Vertices[i]) < 0.01)
                                sub_model_part_displacement_boundary += "     " + (id_node_counter + i).ToString() + "testhash \n";

                        }
                        else if (nodes_of_curves.ContainsKey(testpoint.GetHashCode()))
                        {
                            var variable_test = nodes_of_curves[testpoint.GetHashCode()];
                            Point3d node_from_curve = (Point3d)nodes_of_curves[testpoint.GetHashCode()];

                            if (node_from_curve == null)
                                continue;

                            //maybe try make threshold depended on mesh size

                            if (node_from_curve.DistanceToSquared(mesh.Vertices[i]) < 0.01)
                                sub_model_part_displacement_boundary += "     " + (id_node_counter + i).ToString() + "inner Point \n";


                        }

                    }

                }

                sub_model_part_string += "End SubModelPartNodes\n";
                sub_model_part_displacement_boundary += "End SubModelPartNodes\n";

                sub_model_part_string += "Begin SubModelPartElements\n";
                sub_model_part_displacement_boundary += "Begin SubModelPartElements\n";

                foreach (var face in mesh.Faces)
                {
                    element_string += "    " + id_element_counter + "  " + (0).ToString() + "  " + (face.A + id_node_counter) + "  " + (face.B + id_node_counter) + "  " + (face.C + id_node_counter) + "   " + (face.D + id_node_counter) + "\n";
                    sub_model_part_string += "     " + id_element_counter.ToString() + "\n";
                    id_element_counter++;
                }

                sub_model_part_string += "End SubModelPartElements\n";
                sub_model_part_string += "Begin SubModelPartConditions\n";
                sub_model_part_string += "End SubModelPartConditions\n";
                sub_model_part_string += "End SubModelPart\n\n";


                sub_model_part_displacement_boundary += "End SubModelPartElements\n";
                sub_model_part_displacement_boundary += "Begin SubModelPartConditions\n";
                sub_model_part_displacement_boundary += "End SubModelPartConditions\n";
                sub_model_part_displacement_boundary += "End SubModelPart\n\n";




                // Add displacement boundary conditions resp. grid-conforming boundary conditions HERE!

                id_node_counter += mesh.Vertices.Count;
                sub_model_part_counter++;

               

            }
            node_string += "End Nodes\n\n";
            element_string += "End Elements\n\n";

            mdpa_file += node_string;
            mdpa_file += element_string;
            mdpa_file += sub_model_part_string;
            mdpa_file += sub_model_part_displacement_boundary;

            return mdpa_file;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ElementConditionDictionary"></param>
        /// <returns></returns>
        /// 
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
        //public string GetMaterials(PropertyIdDict ElementConditionDictionary)
        //{
        //    var property_dict_list = new DictList();
        //    foreach (var property_id in ElementConditionDictionary.Keys)
        //    {
        //        var this_property = CocodriloPlugIn.Instance.GetProperty(property_id, out bool success);
        //        if (!success)
        //        {
        //            RhinoApp.WriteLine("InputJSON::GetMaterials: Property with Id: " + property_id + " does not exist.");
        //            continue;
        //        }

        //        //some properties do not need materials and properties
        //        if (!this_property.HasKratosMaterial())
        //            continue;

        //        var variables = this_property.GetKratosVariables();

        //        var property_dict = new Dict
        //            {
        //                {"model_part_name", "IgaModelPart." + this_property.GetKratosModelPart()},
        //                {"properties_id", this_property.mPropertyId},
        //            };

        //        var material_dict = new Dict { };

        //        int material_id = this_property.GetMaterialId();
        //        if (material_id >= 0)
        //        {
        //            var material = CocodriloPlugIn.Instance.GetMaterial(material_id);
        //            var material_variables = material.GetKratosVariables();
        //            foreach (var material_variable in material_variables)
        //                variables.Add(material_variable.Key, material_variable.Value);

        //            material_dict.Add("name", material.Name);
        //            material_dict.Add("material_id", material.Id);
        //            material_dict.Add("constitutive_law", material.GetKratosConstitutiveLaw());

        //            if (material.HasKratosSubProperties())
        //            {
        //                property_dict.Add("sub_properties", material.GetKratosSubProperties());
        //            }
        //        }

        //        material_dict.Add("Variables", variables);
        //        material_dict.Add("Tables", new Dict { });

        //        property_dict.Add("Material", material_dict);

        //        property_dict_list.Add(property_dict);
        //    }

        //    var dict = new Dict
        //    {
        //        {"properties", property_dict_list }
        //    };
        //    var serializer = new JavaScriptSerializer();
        //    string json = serializer.Serialize((object)dict);

        //    return json;
        //}

        #region CO SIMULATION
        public override Dictionary<string, object> GetCouplingSequence(
            List<Analyses.Analysis> InputAnalyses,
            List<Analyses.Analysis> OutputAnalyses)
        {
            var input_data_list = new List<Dictionary<string, object>> { };
            foreach (var input_analysis in InputAnalyses)
                input_data_list.Add(new Dictionary<string, object> {
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
        // /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        // /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////    
        public string WriteProjectParameters(string ProjectPath, ref Cocodrilo.Analyses.AnalysisMpm_new analysis)
        {
            var problem_data = new Dict
                    {
                        { "problem_name", analysis.Name},
                        { "parallel_type", "OpenMP"},
                        { "echo_level", 1 },
                        { "start_time", 0.0},
                    };


            // solver setting block

            var solver_settings = new Dict();
            var time_stepping = new Dict();

            if (analysis.mAnalysisType_static_dynamic_quasi_static is Analyses.AnalysisTransient)
            {
                /// Downcast to access attributes of Analysis Transient

                Cocodrilo.Analyses.AnalysisTransient copyForAccess = (Cocodrilo.Analyses.AnalysisTransient)analysis.mAnalysisType_static_dynamic_quasi_static;

                problem_data.Add("end_time", copyForAccess.Time);

                solver_settings.Add("solver_type", "Dynamic");
                solver_settings.Add("model_part_name", "MPM_Material");

                solver_settings.Add("domain_size", 2);
                solver_settings.Add("echo_level", 1);
                solver_settings.Add("analysis_type", "non_linear");
                solver_settings.Add("time_integration_method", copyForAccess.TimeInteg);

                solver_settings.Add("scheme_type", copyForAccess.Scheme);

                time_stepping.Add("time_step", copyForAccess.mStepSize);
                               
            }

            else // if (analysis.mAnalysisType_static_dynamic_quasi_static is Analyses.AnalysisLinear)
            {
                /// Fixed end-time for linear Analysis
                problem_data.Add("end_time", 1.0);

                solver_settings.Add("solver_type", "Static");
                solver_settings.Add("model_part_name", "MPM_Material");
                solver_settings.Add("domain_size", 2);
                solver_settings.Add("echo_level", 1);
                solver_settings.Add("analysis_type", "linear");

                /// Fixed timestep for linear Analysis since so far no the class Linear analysis does not contain an attribute 'timestep'
                time_stepping.Add("time_step", 1.0);
                //RhinoApp.WriteLine("")
                // Cocodrilo Warnung
            }

            /// Add further cases/exceptions for analysis-types that cannot be handled

            var model_import_settings =
                new Dict { { "input_type", "mdpa" },
                                   { "input_filename", analysis.Name+"_Body" } };

            var material_import_settings =
                new Dict { { "materials_filename", "ParticleMaterials.json" } };

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


            /// until now LinearSolversApplication is hardcoded; discuss if other options should be available -> No 22.06
            var linear_solver_settings =
                new Dict {
                            { "solver_type", "LinearSolversApplication.sparse_lu" },
                            { "max_iteration", 500 },
                            { "tolerance", 1e-9 },
                            { "scaling", false },
                            { "verbosity", 1 } 
                };
                      
            solver_settings.Add("model_import_settings", model_import_settings);
            solver_settings.Add("material_import_settings", material_import_settings);
            solver_settings.Add("time_stepping", time_stepping);
            solver_settings.Add("convergence_criterion", "residual criterion");
            solver_settings.Add("displacement_relative_tolerance", 0.0001);
            solver_settings.Add("displacement_absolute_tolerance", 1e-9);
            solver_settings.Add("residual_relative_tolerance", 0.0001);
            solver_settings.Add("residual_absolute_tolerance", 1e-9);
            solver_settings.Add("max_iteration", 10);
            solver_settings.Add("grid_model_import_settings", grid_model_import_settings);
            solver_settings.Add("pressure_dofs", false);
            solver_settings.Add("linear_solver_settings", linear_solver_settings);
            solver_settings.Add("auxiliary_variables_list", auxiliary_variables_list);
                    
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

            //System.IO.File.WriteAllLines(ProjectPath + "/" + "ProjectParameters.json",
            //new List<string> { project_parameters_json });

            return project_parameters_json;
                        
        }
    }
}


//using Cocodrilo.UserData;
//using Rhino;
//using Rhino.Geometry;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using System.Web.Script.Serialization;

//namespace Cocodrilo.IO
//{
//    using Dict = Dictionary<string, object>;
//    using DictList = List<Dictionary<string, object>>;
//    using IdDict = Dictionary<int, List<int>>;
//    using PropertyIdDict = Dictionary<int, List<BrepToParameterLocations>>;
//    public class OutputKratosFEM : Output
//    {
//        //public OutputKratosFEM() : base(new Analyses.Analysis())
//        //{
//        //}
//        public OutputKratosFEM(Analyses.Analysis analysis) : base(analysis)
//        {
//            this.analysis = analysis;
//        }
//        public void StartAnalysis(List<Mesh> MeshList)
//        {
//            // Namen aus Analyse generieren
//            //this.analysis = analysis;
//            string project_path = UserDataUtilities.GetProjectPath(analysis.Name);

//            StartAnalysis(project_path, MeshList);
//        }

//        public void StartAnalysis(string project_path, List<Mesh> MeshList)
//        {
//            try
//            {
//                PropertyIdDict property_id_dictionary = new PropertyIdDict();

//                System.IO.File.WriteAllLines(project_path + "/" + "Grid.mdpa",
//                    new List<string> { GetFemMdpaFile(MeshList, ref property_id_dictionary) });
//                System.IO.File.WriteAllLines(project_path + "/" + "Body.mdpa",
//                    new List<string> { GetFemMdpaFile(MeshList, ref property_id_dictionary) });
//                System.IO.File.WriteAllLines(project_path + "/" + "ParticleMaterials.json",
//                    new List<string> { GetMaterials(property_id_dictionary) });
//                //System.IO.File.WriteAllLines(project_path + "/" + "ProjectParamaters.json",
//                //    new List<string> {WriteProjectParameters(project_path)});
//                //new List<string> { WriteProjectParameters(property_id_dictionary, NodalVariables, project_path) });
//                WriteProjectParameters(project_path);
//            }
//            catch (Exception ex)
//            {
//                RhinoApp.WriteLine("Json output not possible.");
//                RhinoApp.WriteLine(ex.ToString());
//            }
//        }

//        private string GetFemMdpaFile(List<Mesh> MeshList, ref PropertyIdDict PropertyIdDictionary)
//        {
//            string mdpa_file;

//            mdpa_file = "Begin ModelPartData\n"
//                        + "//  VARIABLE_NAME value\n"
//                        + "End ModelPartData\n\n";

//            for (int i = 2; i < MeshList.Count + 2; i++)
//            {
//                mdpa_file += "Begin Properties " + i.ToString() + "\n End Properties \n";
//            }
//            mdpa_file += "\n\n";

//            string node_string = "Begin Nodes\n";
//            string element_string = "Begin Conditions RigidFace3D3N\n";
//            int id_node_counter = 1;
//            int id_element_counter = 1;

//            int sub_model_part_counter = 1;

//            string sub_model_part_string = "";

//            for (int m = 0; m < MeshList.Count; m++)
//            {

//                sub_model_part_string += "Begin SubModelPart " + sub_model_part_counter + " // GUI DEM-FEM-Wall - DEM-FEM-Wall - group identifier: Parts_membran_oben\n"
//                    + "  Begin SubModelPartData // DEM-FEM-Wall. Group name: Parts_membran_oben\n"
//                    + "    IS_GHOST false\n"
//                    + "    IDENTIFIER Parts_membran_oben\n"
//                    + "    FORCE_INTEGRATION_GROUP 0\n"
//                    + "  End SubModelPartData\n"
//                    + "  Begin SubModelPartNodes\n";

//                var mesh = MeshList[m];

//                for (int i = 0; i < mesh.Vertices.Count; i++)
//                {
//                    node_string += "    " + (id_node_counter + i).ToString() + " " + mesh.Vertices[i].X + " " + mesh.Vertices[i].Y + " " + mesh.Vertices[i].Z + "\n";
//                    sub_model_part_string += "     " + (id_node_counter + i).ToString() + "\n";
//                }
//                sub_model_part_string += "End SubModelPartNodes\n";
//                sub_model_part_string += "Begin SubModelPartConditions\n";
//                foreach (var face in mesh.Faces)
//                {
//                    element_string += "    " + id_element_counter + "  " + (m + 2).ToString() + "  " + (face.A + id_node_counter) + "  " + (face.B + id_node_counter) + "  " + (face.C + id_node_counter) + "\n";
//                    sub_model_part_string += "     " + id_element_counter.ToString() + "\n";
//                    id_element_counter++;
//                }

//                sub_model_part_string += "End SubModelPartConditions\n";
//                sub_model_part_string += "End SubModelPart\n\n";

//                id_node_counter += mesh.Vertices.Count;
//                sub_model_part_counter++;
//            }
//            node_string += "End Nodes\n\n";
//            element_string += "End Elements\n\n";

//            mdpa_file += node_string;
//            mdpa_file += element_string;
//            mdpa_file += sub_model_part_string;

//            return mdpa_file;
//        }

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="ElementConditionDictionary"></param>
//        /// <returns></returns>
//        public string GetMaterials(PropertyIdDict ElementConditionDictionary)
//        {
//            var property_dict_list = new DictList();
//            foreach (var property_id in ElementConditionDictionary.Keys)
//            {
//                var this_property = CocodriloPlugIn.Instance.GetProperty(property_id, out bool success);
//                if (!success)
//                {
//                    RhinoApp.WriteLine("InputJSON::GetMaterials: Property with Id: " + property_id + " does not exist.");
//                    continue;
//                }

//                //some properties do not need materials and properties
//                if (!this_property.HasKratosMaterial())
//                    continue;

//                var variables = this_property.GetKratosVariables();

//                var property_dict = new Dict
//                    {
//                        {"model_part_name", "IgaModelPart." + this_property.GetKratosModelPart()},
//                        {"properties_id", this_property.mPropertyId},
//                    };

//                var material_dict = new Dict { };

//                int material_id = this_property.GetMaterialId();
//                if (material_id >= 0)
//                {
//                    var material = CocodriloPlugIn.Instance.GetMaterial(material_id);
//                    var material_variables = material.GetKratosVariables();
//                    foreach (var material_variable in material_variables)
//                        variables.Add(material_variable.Key, material_variable.Value);

//                    material_dict.Add("name", material.Name);
//                    material_dict.Add("material_id", material.Id);
//                    material_dict.Add("constitutive_law", material.GetKratosConstitutiveLaw());

//                    if (material.HasKratosSubProperties())
//                    {
//                        property_dict.Add("sub_properties", material.GetKratosSubProperties());
//                    }
//                }

//                material_dict.Add("Variables", variables);
//                material_dict.Add("Tables", new Dict { });

//                property_dict.Add("Material", material_dict);

//                property_dict_list.Add(property_dict);
//            }

//            var dict = new Dict
//            {
//                {"properties", property_dict_list }
//            };
//            var serializer = new JavaScriptSerializer();
//            string json = serializer.Serialize((object)dict);

//            return json;
//        }
//        public void WriteProjectParameters(
//           //PropertyIdDict ElementConditionDictionary,
//           //List<string> NodalVariables,
//           string ProjectPath
//           )
//        {
//            string model_part_name = "MPM_Material";
//            string analysis_type = "linear";
//            string solver_type_analysis = "static";
//            double step_size = 1.0;
//            int max_iteration = 1;
//            double displacement_absolute_tolerance = 1.0e-9;
//            double residual_absolute_tolerance = 1.0e-9;
//            double end_time = 0.1;
//            bool compute_reactions = true;
//            bool rotation_dofs = false;

//            //case differentiation: static, dynamic, quasi-static necessary
//            //if (this.analysis.GetType() == typeof(AnalysisLinear))
//            //{
//            //    analysis_type = "linear";
//            //}
//            // 1. problem data block
//            var problem_data = new Dict
//            {
//                { "problem_name", analysis.Name},
//                { "parallel_type", "OpenMP"},
//                { "echo_level", 1 },
//                { "start_time", 0.0},
//                { "end_time", end_time}
//            };

//            // solver setting block

//            var model_import_settings =
//                new Dict { { "input_type", "mdpa" },
//                           { "input_filename", analysis.Name+"_Body" } };

//            var material_import_settings =
//                new Dict { { "materials_filename", "ParticleMaterials.json" } };

//            var time_stepping = new Dict
//                { { "time_step", step_size} };

//            var grid_model_import_settings = new Dict
//            {
//                { "input_type", "mdpa" },
//                { "input_filename", analysis.Name+"_Grid" }
//            };

//            var auxiliary_variables_list =
//                new System.Collections.ArrayList()
//                {
//                    "NORMAL", "IS_STRUCTURE"
//                };


//            var linear_solver_settings =
//                new Dict {
//                    { "solver_type", "LinearSolversApplication.sparse_lu" },
//                    { "max_iteration", 500 },
//                    { "tolerance", 1e-9 },
//                    { "scaling", false },
//                    { "verbosity", 1 } };

//            var solver_settings = new Dict
//            {
//                { "solver_type", "Dynamic" },
//                { "model_part_name", "MPM_Material" },
//                { "domain_size", 2 },
//                { "echo_level", 1 },
//                { "analysis_type", "non_linear"},
//                { "time_integration_method", "implicit" },
//                { "scheme_type", "newmark" },
//                { "model_import_settings", model_import_settings },
//                { "material_import_settings", material_import_settings },
//                { "time_stepping", time_stepping },
//                { "convergence_criterion", "residual criterion" },
//                { "displacement_relative_tolerance", 0.0001  },
//                { "displacement_absolute_tolerance", 1e-9 },
//                { "residual_relative_tolerance", 0.0001 },
//                { "residual_absolute_tolerance", 1e-9 },
//                { "max_iteration", 10 },
//                { "grid_model_import_settings", grid_model_import_settings },
//                { "pressure_dofs", false },
//                { "auxiliary_variables_list", auxiliary_variables_list }
//            };

//            //Processes block

//            //constraint_process_list
//            var constraints_process_list = new DictList();

//            var constraint_process_list_interval =
//                new System.Collections.ArrayList()
//                {
//                            0.0 , "End"
//                };

//            var constraint_process_list_constrained =
//                new System.Collections.ArrayList()
//                {
//                            true, true, true
//                };

//            var constraint_process_list_value =
//                new System.Collections.ArrayList()
//                {
//                           0.0, 0.0, 0.0
//                };


//            var constraint_process_list_parameters = new Dict
//                {
//                    { "model_part_name", "Background_Grid.DISPLACEMENT_Displacement_Auto1" },
//                    { "variable_name", "DISPLACEMENT" },
//                    { "interval", constraint_process_list_interval },
//                    { "constrained", constraint_process_list_constrained },
//                    { "value", constraint_process_list_value }
//                };

//            constraints_process_list.Add(new Dict
//            {
//                {"python_module", "assign_vector_variable_process" },
//                {"kratos_module", "KratosMultiphysics" },
//                {"process_name", "AssignVectorVariableProcess" },
//                { "Parameters", constraint_process_list_parameters }

//            });


//            //load_process_list
//            var load_process_list = new DictList();
//            // up to now empty; has to be adopted

//            //list_other_processes
//            var list_other_processes = new DictList();

//            var list_other_processes_parameters = new Dict
//                {
//                    { "model_part_name", "Background_Grid.Slip2D_Slip_Auto1" },
//                    { "particles_per_condition", 3 },
//                    { "penalty_factor", 1e10 },
//                    { "constrained", "fixed" }
//                };

//            list_other_processes.Add(new Dict
//            {
//                {"python_module", "apply_mpm_particle_dirichlet_condition_process" },
//                {"kratos_module", "KratosMultiphysics.ParticleMechanicsApplication" },
//                { "Parameters", list_other_processes_parameters }

//            });

//            //gravity
//            var gravity = new DictList();

//            var gravity_direction =
//                new System.Collections.ArrayList()
//                {
//                           0.0,-1.0, 0.0
//                };

//            var gravity_parameters = new Dict
//                {
//                    { "model_part_name", "MPM_Material" },
//                    { "variable_name", "MPM_VOLUME_ACCELERATION" },
//                    { "modulus", 9.81 },
//                    { "direction", gravity_direction }
//                };

//            gravity.Add(new Dict
//            {
//                {"python_module", "assign_gravity_to_particle_process" },
//                {"kratos_module", "KratosMultiphysics.ParticleMechanicsApplication" },
//                {"process_name", "AssignGravityToParticleProcess" },
//                { "Parameters", gravity_parameters }

//            });

//            var processes = new Dict()
//            {
//                { "constraints_process_list", constraints_process_list },
//                { "load_process_list", load_process_list },
//                { "list_other_processes", list_other_processes },
//                { "gravity", gravity }
//            };

//            // End processes block

//            // Begin body output

//            var body_output_process = new DictList();

//            var gidpost_flags = new Dict()
//                {
//                    {"GiDPostMode", "GiD_PostBinary" },
//                    {"WriteDeformedMeshFlag", "WriteDeformed" },
//                    {"WriteConditionsFlag", "WriteConditions" },
//                    {"MultiFileFlag", "SingleFile" }
//                };

//            var plane_output =
//                new System.Collections.ArrayList()
//                {
//                        //empty by default
//                    };

//            var gauss_points_results =
//                new System.Collections.ArrayList()
//                {
//                               "MP_Velocity","MP_Displacement"
//                };

//            var nodal_historical_results =
//                new System.Collections.ArrayList()
//                {
//                        //empty by default
//                    };

//            var point_data_configuration =
//                    new System.Collections.ArrayList()
//                    {
//                            //empty by default
//                        };

//            var result_file_configuration = new Dict()
//                {
//                    {"gidpost_flags", gidpost_flags },
//                    {"file_label", "step" },
//                    {"output_control_type", "time" },
//                    {"output_interval", 0.1 },
//                    {"body_output", true },
//                    {"node_output", false },
//                    {"skin_output", false },
//                    {"plane_output", plane_output },
//                    {"gauss_point_results", gauss_points_results },
//                    {"nodal_nonhistorical_results", nodal_historical_results }
//                };

//            var postprocess_parameters = new Dict()
//                {
//                    {"result_file_configuration", result_file_configuration },
//                    {"point_data_configuration", point_data_configuration }
//                };

//            var body_output_process_parameters = new Dict()
//                {
//                    {"model_part_name", "MPM_Material" },
//                    {"output_name", analysis.Name },
//                    {"postprocess_parameters", postprocess_parameters }
//                };

//            body_output_process.Add(new Dict
//                {
//                    {"python_module", "particle_gid_output_process" },
//                    {"kratos_module", "KratosMultiphysics.ParticleMechanicsApplication" },
//                    {"process_name", "ParticleMPMGiDOutputProcess" },
//                    {"help", "This process writes postprocessing files for GiD" },
//                    { "Parameters", body_output_process_parameters }

//                });

//            // End body output

//            // Begin grid output

//            var grid_output_process = new DictList();

//            var grid_gidpost_flags = new Dict()
//                {
//                    {"GiDPostMode", "GiD_PostBinary" },
//                    {"WriteDeformedMeshFlag", "WriteDeformed" },
//                    {"WriteConditionsFlag", "WriteConditions" },
//                    {"MultiFileFlag", "SingleFile" }
//                };

//            var grid_plane_output =
//                new System.Collections.ArrayList()
//                {
//                    //empty by default
//                };

//            var nodal_results =
//                new System.Collections.ArrayList()
//                {
//                    "DISPLACEMENT","REACTION"
//                };

//            var grid_nodal_historical_results =
//                new System.Collections.ArrayList()
//                {
//                    //empty by default
//                };

//            var grid_point_data_configuration =
//                    new System.Collections.ArrayList()
//                    {
//                        //empty by default
//                    };

//            var grid_result_file_configuration = new Dict()
//                {
//                    {"gidpost_flags", gidpost_flags },
//                    {"file_label", "step" },
//                    {"output_control_type", "time" },
//                    {"output_interval", 0.1 },
//                    {"body_output", true },
//                    {"node_output", false },
//                    {"skin_output", false },
//                    {"plane_output", plane_output },
//                    {"nodal_results", nodal_results },
//                    {"nodal_nonhistorical_results", grid_nodal_historical_results }
//                };

//            var grid_postprocess_parameters = new Dict()
//                {
//                    {"result_file_configuration", grid_result_file_configuration },
//                    {"point_data_configuration", grid_point_data_configuration }
//                };

//            var grid_output_process_parameters = new Dict()
//                {
//                    {"model_part_name", "Background_Grid" },
//                    {"output_name", analysis.Name },
//                    {"postprocess_parameters", grid_postprocess_parameters }
//                };

//            grid_output_process.Add(new Dict
//                {
//                    {"python_module", "gid_output_process" },
//                    {"kratos_module", "KratosMultiphysics" },
//                    {"process_name", "GiDOutputProcess" },
//                    {"help", "This process writes postprocessing files for GiD" },
//                    {"Parameters", grid_output_process_parameters }

//                });


//            var output_processes = new Dict()
//            {
//                { "body_output_process", body_output_process },
//                { "grid_output_process", grid_output_process }

//            };

//            //end grid output

//            var dict = new Dict
//                {
//                    {"problem_data", problem_data},
//                    {"solver_settings", solver_settings},
//                    {"processes", processes },
//                    {"output_processes", output_processes},

//                };

//            var serializer = new JavaScriptSerializer();
//            string project_parameters_json = serializer.Serialize((object)dict);

//            System.IO.File.WriteAllLines(ProjectPath + "/" + "ProjectParameters.json",
//            new List<string> { project_parameters_json });
//        }
//        #region CO SIMULATION
//        public override Dictionary<string, object> GetCouplingSequence(
//            List<Analyses.Analysis> InputAnalyses,
//            List<Analyses.Analysis> OutputAnalyses)
//        {
//            var input_data_list = new List<Dictionary<string, object>> { };
//            foreach (var input_analysis in InputAnalyses)
//                input_data_list.Add(new Dictionary<string, object> {
//                        {"data"                   , "load" },
//                        {"from_solver"            , input_analysis.Name},
//                        {"from_solver_data"       , "contact_force"},
//                        {"data_transfer_operator" , "mapper_1"}});
//            var output_data_list = new List<Dictionary<string, object>> { };
//            foreach (var input_analysis in InputAnalyses)
//                input_data_list.Add(new Dictionary<string, object> {
//                        { "data"                  , "disp" },
//                        {"from_solver"            , input_analysis.Name},
//                        {"from_solver_data"       , "disp"},
//                        {"data_transfer_operator" , "mapper_1"}});

//            return new Dictionary<string, object> {
//                    { "name", analysis.Name },
//                    { "input_data_list", input_data_list },
//                    { "output_data_list", input_data_list } };
//        }

//        public override Dictionary<string, object> GetCouplingSolver()
//        {
//            var solver_wrapper_settings = new Dictionary<string, object> {
//                { "input_file", "ProjectParametersFEM" }
//            };

//            var disp = new Dictionary<string, object> {
//                { "model_part_name", "Structure.struct_sub" },
//                { "variable_name", "DISPLACEMENT" },
//                { "dimension", 3 }
//            };
//            var load = new Dictionary<string, object> {
//                { "model_part_name", "Structure.struct_sub" },
//                { "variable_name", "POINT_LOAD" },
//                { "dimension", 3 }
//            };
//            var velocity = new Dictionary<string, object> {
//                { "model_part_name", "Structure.struct_sub" },
//                { "variable_name", "VELOCITY" },
//                { "dimension", 3 }
//            };

//            var data = new Dictionary<string, object> {
//                { "disp", disp },
//                { "load", load },
//                { "velocity", velocity }
//            };

//            return new Dictionary<string, object> {
//                    { analysis.Name, new Dictionary<string, object> {
//                        { "type", "solver_wrappers.kratos.dem_wrapper"},
//                        { "solver_wrapper_settings", solver_wrapper_settings},
//                        { "data", data}
//                    } } };
//        }
//        #endregion
//    }

//}
