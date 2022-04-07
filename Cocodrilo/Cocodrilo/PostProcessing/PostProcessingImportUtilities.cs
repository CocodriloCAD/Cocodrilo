using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Script.Serialization;

namespace Cocodrilo.PostProcessing
{
    //Struct for result file
    public struct RESULT_INFO : IEquatable<RESULT_INFO>
    {
        public string ResultType { get; set; }
        public string LoadCaseType { get; set; }
        public int LoadCaseNumber { get; set; }
        public string VectorOrScalar { get; set; }
        public string NodeOrGauss { get; set; }
        public Dictionary<int, double[]> Results { get; set; }

        public bool Equals(RESULT_INFO result_info)
        {
            return (result_info.ResultType == ResultType) &&
                (result_info.LoadCaseType == LoadCaseType) &&
                (result_info.LoadCaseNumber == LoadCaseNumber) &&
                (result_info.VectorOrScalar == VectorOrScalar);
        }
    }

    public static class PostProcessingImportUtilities
    {
        public static List<RESULT_INFO> LoadResults(string path)
        {
            using (System.IO.StreamReader reader = System.IO.File.OpenText(path))
            {

                var result_info_list = new List<RESULT_INFO>() { };
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    string[] items = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (items[0] != "Result")
                        continue;
                    else
                    {
                        RESULT_INFO result_info = new RESULT_INFO();
                        result_info.ResultType = items[1];
                        if (items[2] == "\"Load")
                        {
                            result_info.LoadCaseType = "Load Case";
                        }
                        result_info.LoadCaseNumber = Convert.ToInt32(items[4]);
                        result_info.VectorOrScalar = items[5];
                        result_info.NodeOrGauss = items[6];
                        result_info.Results = new Dictionary<int, double[]>();

                        line = reader.ReadLine();
                        items = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        if (items[0] != "Values")
                            Rhino.RhinoApp.WriteLine("Result Block should start with: \"Values\".");

                        line = reader.ReadLine();
                        items = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        while (items[0] != "End")
                        {
                            /// Probably this check is not required as anyways the vector length is being checked.
                            if (result_info.VectorOrScalar == "Scalar")
                            {
                                double[] this_result = (items[1] != "nan")
                                    ? new double[] { Convert.ToDouble(items[1]) }
                                    : new double[] { 0.0 };
                                result_info.Results.Add(Convert.ToInt32(items[0]), this_result);
                            }
                            else if (result_info.VectorOrScalar == "Vector")
                            {
                                double[] this_result;
                                if (items.Count() == 4)
                                {
                                    if (!items.Contains("nan"))
                                    {
                                        this_result = new double[] { Convert.ToDouble(items[1]), Convert.ToDouble(items[2]), Convert.ToDouble(items[3]) };
                                    }
                                    else
                                    {
                                        this_result = new double[] { 0, 0, 0 };
                                    }
                                }
                                else if (items.Count() == 3)
                                {
                                    if (!items.Contains("nan"))
                                    {
                                        this_result = new double[] { Convert.ToDouble(items[1]), Convert.ToDouble(items[2]) };
                                    }
                                    else
                                    {
                                        this_result = new double[] { 0, 0 };
                                    }
                                }
                                else if (items.Count() == 7)
                                {
                                    this_result = new double[] {
                                        Convert.ToDouble(items[1]), Convert.ToDouble(items[2]), Convert.ToDouble(items[3]),
                                        Convert.ToDouble(items[4]), Convert.ToDouble(items[5]), Convert.ToDouble(items[6]) };
                                }
                                else
                                {
                                    List<double> result_list = new List<double>();
                                    for (int i = 1; i < items.Count(); i++)
                                    {
                                        result_list.Add(Convert.ToDouble(items[i]));
                                    }
                                    this_result = result_list.ToArray();
                                }
                                result_info.Results.Add(Convert.ToInt32(items[0]), this_result);
                            }
                            else
                            {
                                Rhino.RhinoApp.WriteLine(result_info.VectorOrScalar, " is no valid input type.");
                            }
                            line = reader.ReadLine();
                            items = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                        }
                        result_info_list.Add(result_info);
                    }
                }
                return result_info_list;
            }
        }

        public static void LoadEvaluationPoints(string path,
            ref Dictionary<int, List<KeyValuePair<int, List<double>>>> rEvaluationPointList,
            ref Dictionary<int[], List<KeyValuePair<int, List<double>>>> rCouplingEvaluationPointList)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            serializer.MaxJsonLength = 100000000;
            Dictionary<string, object> dict;
            using (System.IO.StreamReader reader = System.IO.File.OpenText(path))
            {
                string StringJsonProperties = reader.ReadToEnd();
                dict = serializer.Deserialize<Dictionary<string, object>>(StringJsonProperties);
            }
            foreach (System.Collections.ArrayList evaluation_point in (dict["geometry_integration_points"] as System.Collections.ArrayList))
            {
                var local_coords = new List<double> {
                    Convert.ToDouble((evaluation_point[2] as System.Collections.ArrayList)[0]),
                    Convert.ToDouble((evaluation_point[2] as System.Collections.ArrayList)[1]) };
                var evaluation_point_dict = new KeyValuePair<int, List<double>>(Convert.ToInt32(evaluation_point[0]), local_coords);
                if (rEvaluationPointList.ContainsKey(Convert.ToInt32(evaluation_point[1])))
                {
                    rEvaluationPointList[Convert.ToInt32(evaluation_point[1])].Add(evaluation_point_dict);
                }
                else
                {
                    rEvaluationPointList.Add(
                        Convert.ToInt32(evaluation_point[1]),
                        new List<KeyValuePair<int, List<double>>>() { evaluation_point_dict });
                }
            }
            if (dict.ContainsKey("geometry_coupling_integration_points")){
                foreach (System.Collections.ArrayList coupling_evaluation_point in (dict["geometry_coupling_integration_points"] as System.Collections.ArrayList))
                {
                    var local_coords = new List<double> {
                    Convert.ToDouble((coupling_evaluation_point[2] as System.Collections.ArrayList)[0]),
                    Convert.ToDouble((coupling_evaluation_point[2] as System.Collections.ArrayList)[1]),
                    Convert.ToDouble((coupling_evaluation_point[4] as System.Collections.ArrayList)[0]),
                    Convert.ToDouble((coupling_evaluation_point[4] as System.Collections.ArrayList)[1]) };
                    var evaluation_point_dict = new KeyValuePair<int, List<double>>(Convert.ToInt32(coupling_evaluation_point[0]), local_coords);

                    var connecting_brep_ids = new int[] { Convert.ToInt32(coupling_evaluation_point[1]), Convert.ToInt32(coupling_evaluation_point[3]) };
                    if (rCouplingEvaluationPointList.ContainsKey(connecting_brep_ids))
                    {
                        rCouplingEvaluationPointList[connecting_brep_ids].Add(evaluation_point_dict);
                    }
                    else
                    {
                        rCouplingEvaluationPointList.Add(
                            connecting_brep_ids,
                            new List<KeyValuePair<int, List<double>>>() { evaluation_point_dict });
                    }
                }
            }
        }

        public static void LoadRhinoGeometries(string path,
            ref Dictionary<int, Rhino.Geometry.Brep> rBrepList,
            ref Dictionary<int, List<KeyValuePair<int, List<double>>>> rBrepId_NodeId_Coordinates)
        {
            var serializer = new JavaScriptSerializer(new SimpleTypeResolver());
            serializer.MaxJsonLength = 2147483643;
            System.IO.File.Exists(path);
            string StringJsonProperties;
            using (System.IO.StreamReader reader = System.IO.File.OpenText(path))
            {
                StringJsonProperties = reader.ReadToEnd();
            }
            Dictionary<string, object> geometry = serializer.Deserialize<Dictionary<string, object>>(StringJsonProperties);

            int brep_counter = 0;

            foreach (var breps_dict in geometry["breps"] as System.Collections.ArrayList)
            {
                var this_brep = new Rhino.Geometry.Brep();
                int brep_id_entire_brep = (breps_dict as Dictionary<string, object>).ContainsKey("brep_id")
                    ? (int)(breps_dict as Dictionary<string, object>)["brep_id"]
                    : brep_counter;
                brep_counter++;

                var faces_array_dict = (breps_dict as Dictionary<string, object>)["faces"];

                foreach (Dictionary<string, object> face_dict in (System.Collections.ArrayList)faces_array_dict)
                {
                    var brep_id = (int)face_dict["brep_id"];

                    var surface_dict = (Dictionary<string, object>)face_dict["surface"];
                    var degrees = surface_dict["degrees"];
                    var degreeList = (from object u in (System.Collections.ArrayList)degrees select Convert.ToInt32(u)).ToList();
                    List<List<double>> knot_vectors = new List<List<double>>();
                    foreach (var knot_vector_dict in (System.Collections.ArrayList)surface_dict["knot_vectors"])
                    {
                        var knots_vector_i = (from object u in (System.Collections.ArrayList)knot_vector_dict select Convert.ToDouble(u)).ToList();
                        knot_vectors.Add(knots_vector_i);
                    }
                    List<KeyValuePair<int, List<double>>> control_points = new List<KeyValuePair<int, List<double>>>() { };
                    int size = ((System.Collections.ArrayList)surface_dict["control_points"]).Count;
                    foreach (var cp in (System.Collections.ArrayList)surface_dict["control_points"])
                    {
                        var one_cp = cp as System.Collections.ArrayList;
                        int id = Convert.ToInt32(one_cp[0]);
                        var locations = (System.Collections.ArrayList)one_cp[1];
                        control_points.Add(new KeyValuePair<int, List<double>>(
                            id,
                            new List<double>(){ Convert.ToDouble(locations[0]),
                            Convert.ToDouble(locations[1]),
                            Convert.ToDouble(locations[2]),
                            Convert.ToDouble(locations[3]) }));
                    }

                    rBrepId_NodeId_Coordinates.Add(brep_id, control_points);

                    int degree_u = degreeList[0];
                    int degree_v = degreeList[1];
                    int control_points_u = knot_vectors[0].Count - degreeList[0] - 1;
                    int control_points_v = knot_vectors[1].Count - degreeList[1] - 1;
                    var nurbSurface = Rhino.Geometry.NurbsSurface.Create(3, true, degree_u + 1,
                        degree_v + 1, control_points_u, control_points_v);

                    var degreeu = nurbSurface.Degree(0);
                    var degreev = nurbSurface.Degree(1);

                    if (nurbSurface != null)
                    {
                        for (int u = 0; u < nurbSurface.KnotsU.Count; u++)
                        {
                            nurbSurface.KnotsU[u] = knot_vectors[0][u + 1];
                        }
                        for (int v = 0; v < nurbSurface.KnotsV.Count; v++)
                        {
                            nurbSurface.KnotsV[v] = knot_vectors[1][v + 1];
                        }
                        for (int u = 0; u < nurbSurface.Points.CountU; u++)
                        {
                            for (int v = 0; v < nurbSurface.Points.CountV; v++)
                            {
                                var ControlPointID = v * nurbSurface.Points.CountU + u;
                                nurbSurface.Points.SetPoint(u, v,
                                    new Rhino.Geometry.Point3d(control_points[ControlPointID].Value[0], control_points[ControlPointID].Value[1], control_points[ControlPointID].Value[2]),
                                    control_points[ControlPointID].Value[3]);
                            }
                        }

                        int surfaceIndex = this_brep.AddSurface(nurbSurface);
                        Rhino.Geometry.BrepFace face = this_brep.Faces.Add(surfaceIndex);
                        var user_data_surface = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(face);
                        var boundary_loops_dict = (System.Collections.ArrayList)face_dict["boundary_loops"];
                        user_data_surface.BrepId = brep_id;

                        foreach (Dictionary<string, object> boundary_loop_dict in boundary_loops_dict)
                        {
                            Rhino.Geometry.BrepLoopType loop_type = (Convert.ToString(boundary_loop_dict["loop_type"]) == "inner")
                                ? Rhino.Geometry.BrepLoopType.Inner
                                : Rhino.Geometry.BrepLoopType.Outer;

                            List<Rhino.Geometry.NurbsCurve> nurbs_curves = new List<Rhino.Geometry.NurbsCurve>() { };

                            var loop = this_brep.Loops.Add(loop_type, face);
                            foreach (Dictionary<string, object> trimming_curve_dict in (System.Collections.ArrayList)boundary_loop_dict["trimming_curves"])
                            {
                                Dictionary<string, object> one_parameter_curve_dict = trimming_curve_dict["parameter_curve"] as Dictionary<string, object>;
                                int degree = Convert.ToInt32(one_parameter_curve_dict["degree"]);

                                var parameter_knot_vector = (from object u in (System.Collections.ArrayList)one_parameter_curve_dict["knot_vector"] select Convert.ToDouble(u)).ToList();
                                var active_range = (from object i in (System.Collections.ArrayList)one_parameter_curve_dict["active_range"] select Convert.ToDouble(i)).ToList();

                                List<List<double>> parameter_control_points = new List<List<double>>();
                                foreach (var cp in (System.Collections.ArrayList)one_parameter_curve_dict["control_points"])
                                {
                                    var one_cp = cp as System.Collections.ArrayList;
                                    var location = (from object u in (System.Collections.ArrayList)one_cp[1] select Convert.ToDouble(u)).ToList();
                                    parameter_control_points.Add(location);
                                }

                                Rhino.Geometry.NurbsCurve nurbs_curve_2d = new Rhino.Geometry.NurbsCurve(2, true, degree + 1,
                                    parameter_control_points.Count);
                                for (int i = 0; i < nurbs_curve_2d.Knots.Count; i++)
                                {
                                    nurbs_curve_2d.Knots[i] = parameter_knot_vector[i + 1];
                                }
                                for (int i = 0; i < parameter_control_points.Count; i++)
                                {
                                    nurbs_curve_2d.Points.SetPoint(i, parameter_control_points[i][0], parameter_control_points[i][1],
                                        parameter_control_points[i][2], parameter_control_points[i][3]);
                                }

                                nurbs_curve_2d.IsValidWithLog(out string agaga);

                                nurbs_curves.Add(nurbs_curve_2d);
                            }
                            //var joined_curves = Rhino.Geometry.Curve.JoinCurves(nurbs_curves);
                            //var curve = joined_curves[0].ToNurbsCurve();
                            //curve.ChangeDimension(2);
                            foreach (var curve in nurbs_curves)
                            {
                                List<int> edge_indices = new List<int>();

                                Rhino.Geometry.Curve curve_3d = nurbSurface.Pushup(curve, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);
                                int curve_3d_index = this_brep.Curves3D.Add(curve_3d);
                                edge_indices.Add(this_brep.Edges.Count);
                                this_brep.Edges.Add(curve_3d_index);

                                int c2index = this_brep.Curves2D.Add(curve);
                                var trim = this_brep.Trims.Add(this_brep.Edges[edge_indices[0]], false, loop, c2index);
                                trim.IsoStatus = Rhino.Geometry.IsoStatus.None;
                                trim.TrimType = Rhino.Geometry.BrepTrimType.Boundary;
                                trim.SetTolerances(Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);

                                face.OrientationIsReversed = false;
                                this_brep.SetVertices();
                            }
                        }
                    }

                }
                this_brep.SetTrimIsoFlags();
                this_brep.Compact();

                this_brep.IsValidWithLog(out string texty);
                this_brep.Repair(Rhino.RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);

                rBrepList.Add(brep_id_entire_brep, this_brep);
            }
        }
    }
}
