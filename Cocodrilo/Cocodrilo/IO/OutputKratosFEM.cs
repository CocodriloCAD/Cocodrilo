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
                PropertyIdDict property_id_dictionary_grid = new PropertyIdDict();
                //PropertyIdDict PropertyIDDictionary_Edges = new PropertyIdDict();
                PropertyIdDict rPropertyIdsBrepsIds = new PropertyIdDict();
                         
                ///Different output files for FEM and MPM
                
                if (analysis is Cocodrilo.Analyses.AnalysisMpm_new)
                {
                    // downcast to access Bodymesh
                    Cocodrilo.Analyses.AnalysisMpm_new outputCopy = (Cocodrilo.Analyses.AnalysisMpm_new) analysis;

                    // meshing quantities: get physical values via referencing in WriteBodyMesh, think of more elegant way later!
                    // WriteBodyMdpaFile obtains reference to bodyMesh as nodes of BodyMesh are required in background grid, too.

                    Mesh bodyMesh = null;
                    
                    System.IO.File.WriteAllLines(project_path + "/"+ outputCopy.Name + "_Body.mdpa", new List<string> {
                        WriteBodyMdpaFile(outputCopy.mBodyMesh, outputCopy.mCurveList, ref property_id_dictionary, ref bodyMesh, property_id_dictionary) });
                    

                    //call of WriteProjectParameters with downcasted analysis to access class members

                    System.IO.File.WriteAllLines(project_path + "/" + "ProjectParameters.json", new List<string> { WriteProjectParameters(project_path, ref outputCopy, outputCopy.mCurveList) });
                    
                    System.IO.File.WriteAllLines(project_path + "/" + "ParticleMaterials.json", new List<string> { GetMaterials(property_id_dictionary) });

                    System.IO.File.WriteAllLines(project_path + "/" + outputCopy.Name + "_Grid.mdpa",
                        new List<string> { GetMPM_MdpaFile(MeshList, outputCopy.mCurveList, ref property_id_dictionary, outputCopy.mBodyMesh) }); //, ref PropertyIDDictionary_Edges) });
                                        
                }
                else
                {
                    System.IO.File.WriteAllLines(project_path + "/" + "Grid.mdpa",
                       new List<string> { GetFemMdpaFile(MeshList, new List<Curve>(), ref property_id_dictionary) }); //  , ref PropertyIDDictionary_Edges) });
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
            ref PropertyIdDict PropertyIdDictionary,
            ref Mesh bodyMesh,
            PropertyIdDict ElementConditionDictionary
            )

        {
            int brep_ids = 1;

            //value of brep_ids should be changed after calling GeomteryUtilities.AssignBrepIds
            GeometryUtilities.AssignBrepIdToMesh(MeshList, ref brep_ids);

            string bodyMdpaFile;

            bodyMdpaFile = "Begin ModelPartData\n"
                        + "//  VARIABLE_NAME value\n"
                        + "End ModelPartData\n\n";

            bodyMdpaFile += "Begin Properties " + 0.ToString() + "\n End Properties \n";
            
            /// variables to ensure continouos numbering of nodes, elements and submodelparts

            int id_node_counter = 1;
            int id_element_counter = 1;
            int sub_model_part_counter = 0;

            /// strings to collect nodes, elements and submodelparts

            string node_string = "Begin Nodes\n";
            string sub_model_part_string = "";
            string element_string = "";

            foreach (var mesh in MeshList)
            {
                var user_data_mesh = mesh.UserData.Find(typeof(UserDataMesh)) as UserDataMesh;

                /// check somehow that mesh belongs to a solid-element and not to something else 
                /// Unterscheiden: get userData -> schaue bei Schale; Solid Element oder Mesh fragen
                /// updated Langrangian aus UserData     
                /// find propertyID of corresponding BrepId and get Krats Model Part

                user_data_mesh.TryGetKratosPropertyIdsBrepIds(
                    ref PropertyIdDictionary);

                var property_id = PropertyIdDictionary.ElementAt(sub_model_part_counter).Key;

                var this_property = CocodriloPlugIn.Instance.GetProperty(property_id, out bool success);
                                 
                /// Get type of mesh elements: quads or triangles
                var face_test = mesh.Faces[0];
                if (face_test.IsQuad)
                    element_string += "Begin Elements UpdatedLagrangian2D4N//"; 
                else if (face_test.IsTriangle)
                    element_string += "Begin Elements UpdatedLagrangian2D3N//";

                /// assign correct model part name to model: still hardcoded - remove in later version!
                element_string += " GUI group identifier: Solid Auto" + property_id + " \n";
                                
                sub_model_part_string += "Begin SubModelPart Parts_"+ this_property.GetKratosModelPart() + " // Group Solid Auto"+ property_id + " // Subtree Parts_Solid\n";
                    
                sub_model_part_string += "   Begin SubModelPartNodes\n";

                    for (int i = 0; i < mesh.Vertices.Count; i++)
                    {
                        node_string += "      " + (id_node_counter + i).ToString() + " " + mesh.Vertices[i].X + " " + mesh.Vertices[i].Y + " " + mesh.Vertices[i].Z + "\n";
                        sub_model_part_string += "      " + (id_node_counter + i).ToString() + "\n";
                    }

                    sub_model_part_string += "  End SubModelPartNodes\n";
                    sub_model_part_string += "  Begin SubModelPartElements\n";

                /// write correct number of nodes of each element into mdpa file. So far, only working for triangles and quads. Modify below to print also different element types.
                if (face_test.IsQuad)
                {
                    foreach (var face in mesh.Faces)
                    {
                        element_string += "      " + id_element_counter + "  " + (0).ToString() + "  " + (face.A + id_node_counter) + "  " + (face.B + id_node_counter) + "  " + (face.C + id_node_counter) + "   " + (face.D + id_node_counter) + "\n";
                        sub_model_part_string += "      " + id_element_counter.ToString() + "\n";
                        id_element_counter++;
                    }
                }
                else if (face_test.IsTriangle)
                {
                    foreach (var face in mesh.Faces)
                    {
                        element_string += "      " + id_element_counter + "  " + (0).ToString() + "  " + (face.A + id_node_counter) + "  " + (face.B + id_node_counter) + "  " + (face.C + id_node_counter) + "\n";
                        sub_model_part_string += "      " + id_element_counter.ToString() + "\n";
                        id_element_counter++;
                    }
                }
                else
                {
                    RhinoApp.WriteLine("Element type of unknown format (not triangles or quads) is used! MDPA generation for such meshes is not yet implemented!");
                }

                 sub_model_part_string += "  End SubModelPartElements\n";
                 sub_model_part_string += "  Begin SubModelPartConditions\n";
                 sub_model_part_string += "  End SubModelPartConditions\n";
                 sub_model_part_string += "End SubModelPart\n\n";

                 id_node_counter += mesh.Vertices.Count;
                 sub_model_part_counter++;

                 
                 element_string += "End Elements\n\n";

                 
                   
            }

            node_string += "End Nodes\n\n";
            bodyMdpaFile += node_string;
            bodyMdpaFile += element_string;
            bodyMdpaFile += sub_model_part_string;
            return bodyMdpaFile;
        }

        private string GetFemMdpaFile(List<Mesh> MeshList, List<Curve> CurveList, ref PropertyIdDict PropertyIdDictionary, int number_of_body_mesh_nodes = 1, int number_of_body_mesh_elements = 1) // ref PropertyIdDict PropertyIDDictionary_Edges,
        {
            int brep_ids = 1;

            //Creation of new (empty) lists of Breps and Points to use already existing function AssignBrepIds
            GeometryUtilities.AssignBrepIds(new List<Brep>(), CurveList, new List<Point>(), ref brep_ids);

            //value of brep_ids should be changed after calling GeomteryUtilities.AssignBrepIds
            GeometryUtilities.AssignBrepIdToMesh(MeshList, ref brep_ids);

            //Rhino.Geometry.Collections.MeshTopologyEdgeList mesh_edges = brep.Meshes;
            
            /// Variables for grid-conforming bcs (curves)
            IDictionary<string, Point3d> nodesCurves = new Dictionary<string, Point3d>();
            int numConfBC = 0; // number of grid-conforming bcs that should be considered
            List<Point3d> startEndPointsCurve = new List<Point3d>();
            List<int> num_of_line_segments_non_grid_conforming_bcs = new List<int>();

            List<IDictionary<string, Point3d>> Dict_list_conf_bc = new List<IDictionary<string, Point3d>>();
            List<List<int>> List_of_nodes = new List<List<int>>();

            /// number of non-grid conforming boundary conditions which should be considered
            int numNonConfBC = 0;
            

            // couple this somehow with meshlength
            double max_parameter_segment_length = 0.1; 

            foreach (var curve in CurveList)
            {
                /// Grid non-conforming boundary conditions are of type "UserDataEdge"
                /// Grid conforming boundary conditions (=boundary conditions on the background grid) are represented by 
                /// the type "UserDataCurve"
                
                var user_data_edge = curve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;

                    if (user_data_edge != null)
                    {
                        // count how many non-conforming bcs are present
                        numNonConfBC++;
                    }

                var user_data_curve = curve.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;

                    if (user_data_curve != null)
                    {
                        /// count how many non-conforming bcs are present
                        numConfBC++;
                        List<int> new_List = new List<int>();
                        List_of_nodes.Add(new_List);

                        foreach (var mesh in MeshList)
                        {
                            /// Gets a polyline approximation of the input curve and then moves its control points
                            /// to the closest point on the mesh. Then it "connects the points" over edges so
                            /// that a polyline on the mesh is formed.
                            var polyline = mesh.PullCurve(curve, 0.05);

                            /// get PropertyIds and BrepIds used by Kratos of the element user_data_edge
                            user_data_curve.TryGetKratosPropertyIdsBrepIds(
                                ref PropertyIdDictionary);

                            /// Returns underlying polyline or points
                            var poly_curve = polyline.PullToMesh(mesh, 0.05);

                            /// Makes a polyline approximation of the curve and gets the closest point on the
                            /// mesh for each point on the curve. Then it "connects the points" so that you have
                            /// a polyline on the mesh.
                            Polyline polyline_grid_conforming_bc = poly_curve.ToPolyline();

                            startEndPointsCurve.Add(polyline_grid_conforming_bc.First());
                            startEndPointsCurve.Add(polyline_grid_conforming_bc.Last());

                            IDictionary<string, Point3d> nodes_curves_temp = new Dictionary<string, Point3d>();

                            foreach (var point in polyline_grid_conforming_bc)
                            {
                                var closest_point = mesh.ClosestPoint(point);

                                /// check if point's key is already part of nodes_curves_temp before adding

                            if (!nodes_curves_temp.ContainsKey(closest_point.ToString()))
                                {
                                    nodes_curves_temp.Add(closest_point.ToString(), closest_point);
                                }
                                else
                                {
                                    RhinoApp.WriteLine("Key already present in curve dictionary!");
                                    RhinoApp.WriteLine("coordinates: "+ closest_point.ToString());
                                }

                            }

                        Dict_list_conf_bc.Add(nodes_curves_temp);

                        /// add number of line segments of grid-conforming bc

                        num_of_line_segments_non_grid_conforming_bcs.Add(polyline_grid_conforming_bc.Count());

                        }
                    }
            }

            /// Creation of overall string for grid mdpa-file

            string mdpa_file;

            mdpa_file = "Begin ModelPartData\n"
                        + "//  VARIABLE_NAME value\n"
                        + "End ModelPartData\n\n";

                     
            mdpa_file += "Begin Properties " + 0.ToString() + "\n End Properties \n";
            
            /// Introduction of variables to ensure continous counting of nodes, elements and submodelparts
            
            int id_node_counter;
            int id_element_counter;
            int sub_model_part_counter = 0;

            /// check if elements from body mesh (from MPM) exist
           
            if (number_of_body_mesh_nodes != 1)
                id_node_counter = number_of_body_mesh_nodes + 1;
            else
                id_node_counter = 1;

            if (number_of_body_mesh_elements != 1)
                id_element_counter = number_of_body_mesh_elements + 1;
            else
                id_element_counter = 1;
                                

            string node_string = "Begin Nodes\n";
            string element_string = null;
            string group_identifier = null; // belongs to element_string

            string sub_model_part_string = null;
            string sub_model_part_displacement_boundary = null;

            /// overall strings for all slip submodel parts
            string sub_model_part_slip = null;
            string sub_model_slip_conditions = null;

            /// overall helper variables
            int totalNumNodes = 0;

            /// iterate over all meshes in MeshList

            foreach (var mesh in MeshList)
            {
                var user_data_mesh = mesh.UserData.Find(typeof(UserDataMesh)) as UserDataMesh;

                /// check somehow that mesh belongs to a solid-element and not to something else 
                /// Unterscheiden: get userData -> schaue bei Schale; Solid Element oder Mesh fragen
                /// updated Langrangian aus UserData     
                /// find propertyID of corresponding BrepId and get Krats Model Part

                user_data_mesh.TryGetKratosPropertyIdsBrepIds(
                    ref PropertyIdDictionary);

                var property_id = PropertyIdDictionary.ElementAt(sub_model_part_counter).Key;

                var this_property = CocodriloPlugIn.Instance.GetProperty(property_id, out bool success);

                /// Get type of mesh elements: quads or triangles
                
                var face_test = mesh.Faces[0];
                if (face_test.IsQuad)
                    element_string += "Begin Elements UpdatedLagrangian2D4N//";
                else if (face_test.IsTriangle)
                    element_string += "Begin Elements UpdatedLagrangian2D3N//";
                                
                /// so far only ONE background mesh can be created. To be able to use more background meshes change function in the next lines.

                sub_model_part_string += "Begin SubModelPart Parts_Grid_Grid_Auto1 // Group Grid Auto1 // Subtree Parts_Grid\n" +
                                         "  Begin SubModelPartNodes\n";

                element_string += " GUI group identifier: Grid Auto1 \n";

                //make modelpartname, group and subtree automatic


                totalNumNodes = id_node_counter - 1;

                for (int i = 0; i < mesh.Vertices.Count; i++)
                {
                    node_string += "     " + (id_node_counter + i).ToString() + " " + mesh.Vertices[i].X + " " + mesh.Vertices[i].Y + " " + mesh.Vertices[i].Z + "\n";
                    sub_model_part_string += "     " + (id_node_counter + i).ToString() + "\n";

                    totalNumNodes++;                                       

                    /// GO OVER ALL CURVES HERE

                    // Get node from mesh that is closest to 1st and last point of curve

                    for (int current_conf_bc = 0; current_conf_bc < numConfBC; current_conf_bc++)
                    {
                        // bool to decide whether to check if current vertex corresponds to an inner point of a 
                        // boundary condition

                        bool checkForInnerPointOfBoundaryCondition = true;

                        //string group_part_name_disp = null;
                        //string model_part_name_disp = null;
                        //string group_identifier_disp = null;

                        //group_part_name_disp = "Group Displacement Auto" + (current_conf_bc + 1).ToString() + " //";
                        //model_part_name_disp = "DISPLACEMENT_Displacement_Auto" + (current_conf_bc + 1).ToString() + " // ";
                        //group_identifier_disp = "GUI group identifier: Slip Auto" + (current_conf_bc+1).ToString() + "\n";

                        //string sub_model_part_displacement_boundary_temp = null;

                        //sub_model_part_displacement_boundary_temp = "Begin SubModelPart " + model_part_name_disp + group_part_name_disp +" Subtree DISPLACEMENT\n" +
                        //                 "  Begin SubModelPartNodes\n";

                        /// get current start and endpoint
                        List<Point3d> current_startEndPointsCurve = new List<Point3d>();

                        current_startEndPointsCurve.Add(startEndPointsCurve[2 * current_conf_bc]);
                        current_startEndPointsCurve.Add(startEndPointsCurve[2 * current_conf_bc +1]);

                        foreach (Point3d startEndPoint in current_startEndPointsCurve)
                        {
                            /// make this 0.001 value dependend on mesh-size
                            if (startEndPoint.DistanceToSquared(mesh.Vertices[i]) < 0.001)
                            {
                                //sub_model_part_displacement_boundary_temp += "     " + (id_node_counter + i).ToString() + "\n";
                                List_of_nodes[current_conf_bc].Add(id_node_counter + i);
                                checkForInnerPointOfBoundaryCondition = false;
                            }

                        }

                        // create testpoint to see if current mesh-vertex is included in boundary conditions. This seems to be necessary as 
                        // vertices and points seem to have different hash-codes

                        if (checkForInnerPointOfBoundaryCondition)
                        {
                            Point3d testpoint = mesh.Vertices[i];

                            if (Dict_list_conf_bc[current_conf_bc].ContainsKey(testpoint.ToString()))
                            {
                                var variable_test = Dict_list_conf_bc[current_conf_bc][testpoint.ToString()];
                                Point3d node_from_curve = (Point3d)Dict_list_conf_bc[current_conf_bc][testpoint.ToString()];

                                if (node_from_curve == null)
                                    continue;

                                //make threshold 0.001 depended on mesh size

                                if (node_from_curve.DistanceToSquared(mesh.Vertices[i]) < 0.001)
                                {//sub_model_part_displacement_boundary_temp += "     " + (id_node_counter + i).ToString() + " \n";
                                    List_of_nodes[current_conf_bc].Add(id_node_counter + i);
                                }
                            }

                        }

                        //sub_model_part_displacement_boundary_temp += "End SubModelPartNodes\n";
                        //sub_model_part_displacement_boundary += sub_model_part_displacement_boundary_temp;
                    }
                }

                sub_model_part_string += "End SubModelPartNodes\n";
                sub_model_part_string += "Begin SubModelPartElements\n";
                


                /// Check if faces are quads or triangles: write three or four nodes in the mdpa - file
                if (face_test.IsQuad)
                {
                    foreach (var face in mesh.Faces)
                    {
                        element_string += "    " + id_element_counter + "  " + (0).ToString() + "  " + (face.A + id_node_counter) + "  " + (face.B + id_node_counter) + "  " + (face.C + id_node_counter) + "   " + (face.D + id_node_counter) + "\n";
                        sub_model_part_string += "     " + id_element_counter.ToString() + "\n";
                        id_element_counter++;
                    }
                }
                else if (face_test.IsTriangle)
                {
                    foreach (var face in mesh.Faces)
                    {
                        element_string += "    " + id_element_counter + "  " + (0).ToString() + "  " + (face.A + id_node_counter) + "  " + (face.B + id_node_counter) + "  " + (face.C + id_node_counter) + "\n";
                        sub_model_part_string += "     " + id_element_counter.ToString() + "\n";
                        id_element_counter++;
                    }
                }
                else
                {
                    RhinoApp.WriteLine("Element type of unknown format (not triangles or quads) is used! MDPA generation for such meshes is not yet implemented!");
                }

                sub_model_part_string += "    End SubModelPartElements\n";
                sub_model_part_string += "    Begin SubModelPartConditions\n";
                sub_model_part_string += "    End SubModelPartConditions\n";
                sub_model_part_string += "End SubModelPart\n\n";


                //sub_model_part_displacement_boundary += "    End SubModelPartElements\n";
                //sub_model_part_displacement_boundary += "    Begin SubModelPartConditions\n";
                //sub_model_part_displacement_boundary += "    End SubModelPartConditions\n";
                //sub_model_part_displacement_boundary += "End SubModelPart\n\n";

                // Add displacement boundary conditions resp. grid-conforming boundary conditions HERE!

                id_node_counter += mesh.Vertices.Count;
                sub_model_part_counter++;
            }
            ///////////////////////////////////////////////////////////////////////////////////////////////
            /// Go through all curves and add nodes from grid conforming boundary conditions to background_mdpa
            ///////////////////////////////////////////////////////////////////////////////////////////////

            for (int current_conf_bc = 0; current_conf_bc < numConfBC; current_conf_bc++)
            {                                            
                string group_part_name_disp = null;
                string model_part_name_disp = null;
                string group_identifier_disp = null;

                group_part_name_disp = "Group Displacement Auto" + (current_conf_bc + 1).ToString() + " //";
                model_part_name_disp = "DISPLACEMENT_Displacement_Auto" + (current_conf_bc + 1).ToString() + " // ";
                group_identifier_disp = "GUI group identifier: Slip Auto" + (current_conf_bc + 1).ToString() + "\n";

                string sub_model_part_displacement_boundary_temp = null;

                sub_model_part_displacement_boundary_temp = "Begin SubModelPart " + model_part_name_disp + group_part_name_disp + " Subtree DISPLACEMENT\n" +
                                 "  Begin SubModelPartNodes\n";

                foreach (int node_id in List_of_nodes[current_conf_bc])
                {
                    sub_model_part_displacement_boundary_temp += "     " + (node_id).ToString() + "\n";
                }

                sub_model_part_displacement_boundary_temp += "    End SubModelPartNodes\n";
                sub_model_part_displacement_boundary_temp += "    Begin SubModelPartElements\n" + "    End SubModelPartElements\n";
                       
                sub_model_part_displacement_boundary_temp += "    Begin SubModelPartConditions\n" + "    End SubModelPartConditions\n";
                sub_model_part_displacement_boundary_temp += "End SubModelPart\n\n";

                sub_model_part_displacement_boundary += sub_model_part_displacement_boundary_temp;

            }
            


            ////////////////////////////////////////////////////////////////////////////////////////////////
            /// Here the NON-CONFORMING boundary conditions are added to the background_mdpa resp. grid_mdpa
            ////////////////////////////////////////////////////////////////////////////////////////////////

            int num_Of_non_conf_BCs = 0;
            string model_part_name_slip;
            string group_identifier_slip;
            string group_part_name_slip;
            string temp = "";
            string sub_model_part_slip_temp;
            string sub_model_slip_conditions_temp;

            int numOfLineSegments_slip = 1;
            int counter_for_nodes = 1;

            foreach (var curve in CurveList)
            {
                 var user_data_edge = curve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;

                 if (user_data_edge != null)
                 {
                     num_Of_non_conf_BCs++;

                     group_part_name_slip = "Group Slip Auto" + (num_Of_non_conf_BCs+numConfBC).ToString() + " //";
                     model_part_name_slip = "Slip2D_Slip_Auto" + (num_Of_non_conf_BCs + numConfBC).ToString() + " // "; 
                     group_identifier_slip = "GUI group identifier: Slip Auto" + (num_Of_non_conf_BCs + numConfBC).ToString() + "\n";

                    // add headers to general string

                    sub_model_part_slip_temp = null;
                     sub_model_part_slip_temp = "Begin SubModelPart " + model_part_name_slip + group_part_name_slip + " Subtree Slip2D\n"+
                                 "    Begin SubModelPartNodes\n";

                     sub_model_slip_conditions_temp = "Begin Conditions LineCondition2D2N// " + group_identifier_slip;

                     temp = "    Begin SubModelPartElements \n    End SubModelPartElements \n" + "    Begin SubModelPartConditions \n";
                    if (curve is PolylineCurve)
                     {
                         PolylineCurve poyline_curve = curve.ToPolyline(-1, 1, 0.1, 0.1, 0.1, RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, 0, max_parameter_segment_length, true);
                         Polyline polyline_edge;
                         poyline_curve.TryGetPolyline(out polyline_edge);

                         int help_node_counter = 0;
                         foreach (var point in polyline_edge)
                         {
                             // Add node number to Nodelist

                            int index = totalNumNodes + counter_for_nodes;

                            node_string += "     " + index.ToString() + " " + point.X + " " + point.Y + " " + point.Z + "\n";

                            sub_model_part_slip_temp += "     " + index.ToString() + "\n";
                                               
                            counter_for_nodes++;

                            if (help_node_counter != 0)
                            {
                                 int startNode = index - 1;
                                 int endNode = index;
                                
                                 sub_model_slip_conditions_temp += "         " + numOfLineSegments_slip.ToString() + " " + (0).ToString() + " " + startNode.ToString() + " " + endNode.ToString() + "\n";

                                    /// Add number of segment to list

                                 temp += "           " + numOfLineSegments_slip.ToString() + "\n";


                                    numOfLineSegments_slip++;
                                };

                            help_node_counter++;

                            };

                        sub_model_part_slip_temp += "    End SubModelPartNodes \n";
                        temp += "   End SubModelPartConditions \n";

                        sub_model_part_slip_temp += temp;
                            
                        sub_model_slip_conditions_temp += "End Conditions\n";
                        sub_model_part_slip_temp += "End SubModelPart\n \n";



                        sub_model_part_slip += sub_model_part_slip_temp;
                        sub_model_slip_conditions += sub_model_slip_conditions_temp;
                            
                        }
                        else
                        {
                            RhinoApp.WriteLine("Edge has wrong datatype. Choose a curve for imposing non-grid conforming bcs.");
                        }
                    }
                }

            node_string += "End Nodes\n\n";
            element_string += "End Elements\n\n";

            mdpa_file += node_string;
            mdpa_file += element_string;
            mdpa_file += sub_model_slip_conditions;
            mdpa_file += sub_model_part_string;
            mdpa_file += sub_model_part_displacement_boundary;
            mdpa_file += sub_model_part_slip;

            return mdpa_file;
            ////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            /// Implementierung unter Verwendung von PropertyIds und GetKratosModelPartName()

            // loop über curves bzw. edges 
            // hole user data edges
            // von userdataedge hole getModelPartName.. 
            // generisch einbauen; Listen sparen

            /// helper variable
            /// 
            /// user_data_mesh.TryGetKratosPropertyIdsBrepIds(
            //ref PropertyIdDictionary);

            //var property_id = PropertyIdDictionary.ElementAt(sub_model_part_counter).Key;

            //var this_property = CocodriloPlugIn.Instance.GetProperty(property_id, out bool success);
            /// 

            //int counter_for_nodes = 1; /// to gives nodes from non-conforming (weak boundary conditions) right numbering


            //PropertyIdDict property_id_dictionary2 = new PropertyIdDict();
            //foreach (var curve in CurveList)
            //{
            //    var user_data_edge = curve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;

            //    if (user_data_edge != null)
            //    {
            //        user_data_edge.TryGetKratosPropertyIdsBrepIds(ref PropertyIdDictionary);

            //        var property_id_edge = PropertyIdDictionary.ElementAt(sub_model_part_counter).Key;

            //        // -> welches element der liste soll ich auslesen??
            //    }
            //}                       


        }
                 
        private string GetMPM_MdpaFile(List<Mesh> MeshList, List<Curve> CurveList, ref PropertyIdDict PropertyIdDictionary, List<Mesh> BodyMeshList) //, ref PropertyIdDict PropertyIDDictionary_Edges)
        {
            int number_nodes_body_mesh = 0;

            int number_of_elements_body_mesh = 0;

            foreach (var mesh in BodyMeshList)
            {
                number_nodes_body_mesh += mesh.Vertices.Count;

                foreach (var face in mesh.Faces)
                {
                    number_of_elements_body_mesh++;
                }
            }

            string basicGridMdpa = GetFemMdpaFile(MeshList, CurveList, ref PropertyIdDictionary, number_nodes_body_mesh, number_of_elements_body_mesh); //ref PropertyIDDictionary_Edges,
                        
            return basicGridMdpa;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ElementConditionDictionary"></param>
        /// <returns></returns>
        /// 
        public string GetMaterials(PropertyIdDict ElementConditionDictionary )
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
                int material_id = this_property.GetMaterialId();

                var property_dict = new Dict
                    {
                        {"model_part_name", "Initial_MPM_Material.Parts_" + this_property.GetKratosModelPart() },
                        {"properties_id", this_property.mPropertyId},

                         
                    };

                var material_dict = new Dict { };
                                
                if (material_id >= 0)
                {
                    var material = CocodriloPlugIn.Instance.GetMaterial(material_id);
                    var material_variables = material.GetKratosVariables();
                    foreach (var material_variable in material_variables)
                        variables.Add(material_variable.Key, material_variable.Value);

                    // cast of material to material non-linear to access material parameters
                    Cocodrilo.Materials.MaterialNonLinear downcast_material_for_particles = (Cocodrilo.Materials.MaterialNonLinear)material;
                    variables.Add("PARTICLES_PER_ELEMENT", downcast_material_for_particles.particlesPerElement);
                                        
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
        public string WriteProjectParameters(string ProjectPath, ref Cocodrilo.Analyses.AnalysisMpm_new analysis, List<Curve> CurveList)
        {

            string model_part_name = "MPM_Material";
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

                time_stepping.Add("time_step", copyForAccess.mStepSize);

                solver_settings.Add("time_stepping", time_stepping);
                solver_settings.Add("solver_type", "Dynamic");
                solver_settings.Add("model_part_name", model_part_name);
                solver_settings.Add("domain_size", 2);
                solver_settings.Add("echo_level", 1);
                solver_settings.Add("analysis_type", "non_linear");
                solver_settings.Add("time_integration_method", copyForAccess.TimeInteg);

                solver_settings.Add("scheme_type", copyForAccess.Scheme);

                
                               
            }

            else // if (analysis.mAnalysisType_static_dynamic_quasi_static is Analyses.AnalysisLinear)
            {
                /// Fixed end-time for linear Analysis
                problem_data.Add("end_time", 1.0);

                /// Fixed timestep for linear Analysis since so far no the class Linear analysis does not contain an attribute 'timestep'
                time_stepping.Add("time_step", 1.0);
                solver_settings.Add("time_stepping", time_stepping);

                solver_settings.Add("solver_type", "Static");
                solver_settings.Add("model_part_name", model_part_name);
                solver_settings.Add("domain_size", 2);
                solver_settings.Add("echo_level", 1);
                solver_settings.Add("analysis_type", "non_linear");

               
               
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
            solver_settings.Add("convergence_criterion", "residual_criterion");
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

            int counterCurve = 0;
            List<int> countCurve = new List<int>();

            foreach (var curve in CurveList)
            {
                var user_data_edge = curve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;

                if (user_data_edge != null)
                {
                    countCurve.Add(counterCurve);
                    counterCurve++;
                }

            }
            
            if (counterCurve != 0)
            {
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
            }
                    

            //gravity
            var gravity = new DictList();

            var gravity_direction =
                new System.Collections.ArrayList()
                {
                                   0.0,-1.0, 0.0
                };

            var gravity_parameters = new Dict
                        {
                            { "model_part_name", model_part_name },
                            { "variable_name", "MP_VOLUME_ACCELERATION" },
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
                                       "MP_VELOCITY", "MP_DISPLACEMENT"
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
                            {"model_part_name", model_part_name },
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
                            {"analysis_stage", "KratosMultiphysics.ParticleMechanicsApplication.particle_mechanics_analysis"}
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
