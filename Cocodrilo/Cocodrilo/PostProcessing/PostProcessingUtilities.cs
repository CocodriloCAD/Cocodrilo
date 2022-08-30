using Rhino;
using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.PostProcessing
{
    public static class PostProcessingUtilities
    {
        public static Mesh CreatePostProcessingMesh(BrepFace ThisBrepFace,
            Dictionary<int, List<KeyValuePair<int, List<double>>>> EvaluationPointList,
            bool ConsiderEdges, double MaxEdgeLength)
        {
            List<Point3d> list_evaluation_points = new List<Point3d>();

            int brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(ThisBrepFace).BrepId;

            var nurbs_surface = ThisBrepFace.ToNurbsSurface();
            var tmp_evaluation_point = new Point3d();
            foreach (var evaluation_point in EvaluationPointList[brep_id])
            {
                tmp_evaluation_point.X = evaluation_point.Value[0];
                tmp_evaluation_point.Y = evaluation_point.Value[1];
                tmp_evaluation_point.Z = 0.0;
                list_evaluation_points.Add(tmp_evaluation_point);
            }

            List<List<Point3d>> closed_edges = new List<List<Point3d>>();
            if (ConsiderEdges)
            {
                foreach (var brep_edge_index in ThisBrepFace.AdjacentEdges())
                {
                    var adjacent_edges = ThisBrepFace.AdjacentEdges();
                    var edge = ThisBrepFace.Brep.Edges[brep_edge_index];


                    double length = edge.GetLength();
                    int number_of_segments = (int)(length / MaxEdgeLength);
                    var curve2d = ThisBrepFace.Brep.Trims[ThisBrepFace.Brep.Edges[brep_edge_index].TrimIndices()[0]];
                    double parameter_length = curve2d.GetLength();
                    double max_parameter_segment_length = parameter_length / number_of_segments;
                    PolylineCurve poyline_curve = curve2d.ToPolyline(
                        -1, 1, 0.1, 0.1, 0.1, RhinoDoc.ActiveDoc.ModelAbsoluteTolerance,
                        0, max_parameter_segment_length, true);
                    Polyline polyline;
                    poyline_curve.TryGetPolyline(out polyline);
                    foreach (var line in polyline.GetSegments())
                    {
                        closed_edges.Add(new List<Point3d> {
                                new Point3d(line.FromX, line.FromY, line.FromZ),
                                new Point3d(line.ToX, line.ToY, line.ToZ) });
                    }
                }

                if (closed_edges[0][0].DistanceTo(closed_edges.Last()[1]) > RhinoDoc.ActiveDoc.ModelAbsoluteTolerance)
                {
                    closed_edges.Add(new List<Point3d> {
                                closed_edges.Last()[1],
                                closed_edges[0][0] });
                }
            }

            // Create Tesselation in Paramter space.
            Plane plane = new Plane();
            Plane.FitPlaneToPoints(list_evaluation_points, out plane);
            var new_mesh = Mesh.CreateFromTessellation(list_evaluation_points, closed_edges, plane, false);

            for (int i = 0; i < new_mesh.Vertices.Count; i++)
            {
                var mesh_point = new_mesh.Vertices[i];
                nurbs_surface.Evaluate(mesh_point.X, mesh_point.Y, 0, out Point3d result_point, out _);
                var global_mesh_vertex_coordinates = new Point3f(
                    Convert.ToSingle(result_point.X),
                    Convert.ToSingle(result_point.Y),
                    Convert.ToSingle(result_point.Z));
                new_mesh.Vertices[i] = global_mesh_vertex_coordinates;
            }

            return new_mesh;
        }

        /// <summary>
        /// Computes the length of an array.
        /// Ideal for the computation of lengths.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double GetArrayLength(double[] array)
        {
            double square = array.Sum(i => i * i);
            return Math.Sqrt(square);
        }
        /// <summary>
        /// Computes the von Mises stress from a 3 dimensional array.
        /// </summary>
        /// <param name="array"></param>
        /// <returns></returns>
        public static double GetVonMises(double[] array)
        {
            if (array.GetLength(0) == 3)
            {
                return Math.Sqrt(array[0] * array[0] - array[0] * array[1] + array[1] * array[1] + 3 * array[2] * array[2]);
            } else
            {
                double von_mises = Math.Sqrt( 1.0/2.0 * (Math.Pow(array[0] - array[1], 2) + Math.Pow(array[1] - array[2], 2) + Math.Pow(array[0] - array[2], 2)
                        + 6 * (array[3] * array[3] + array[4] * array[4] + array[5] * array[5])) );
                return von_mises;
            }
         }

        public static Rhino.DocObjects.ObjectAttributes GetStressPatternObjectAttributes(double Stress, Rhino.Geometry.Interval StressMinMax)
        {
            var attributes = RhinoDoc.ActiveDoc.CreateDefaultAttributes();
            //if (Math.Abs(Stress) > 0.01)
            //{
            //    attributes.ObjectDecoration = (Stress > 0)
            //            ? Rhino.DocObjects.ObjectDecoration.EndArrowhead
            //            : Rhino.DocObjects.ObjectDecoration.StartArrowhead;
            //}
            attributes.ObjectColor = PostProcessingUtilities.FalseColor(Stress, StressMinMax);
            attributes.ColorSource = Rhino.DocObjects.ObjectColorSource.ColorFromObject;
            return attributes;
        }

        public static List<string> GetResultIndices(RESULT_INFO ResultInfo)
        {
            List<string> result_indices = new List<string>();

            if (ResultInfo.NodeOrGauss == "OnNodes" || ResultInfo.NodeOrGauss == "\"OnNodes\"")
            {
                result_indices.Add("X");
                result_indices.Add("Y");
                result_indices.Add("Z");
                result_indices.Add("Length");
            }
            else
            {
                var result = ResultInfo.Results.First();
                int result_length = (ResultInfo.Results.Count > 0)
                    ? result.Value.Length
                    : 0;

                if (ResultInfo.ResultType == "\"DAMAGE_TENSION_VECTOR\"" || ResultInfo.ResultType == "\"DAMAGE_COMPRESSION_VECTOR\"")
                {
                    for (int i = 0; i < result_length; i++)
                    {
                        result_indices.Add(i.ToString());
                    }
                }
                else if (result_length == 3)
                {
                    result_indices.Add("11");
                    result_indices.Add("22");
                    result_indices.Add("12");
                    result_indices.Add("von Mises");
                }
                else
                {
                    result_indices.Add("11");
                    result_indices.Add("22");
                    result_indices.Add("33");
                    result_indices.Add("12");
                    result_indices.Add("13");
                    result_indices.Add("23");
                    result_indices.Add("von Mises");
                }
            }

            return result_indices;
        }

        public static System.Drawing.Color FalseColor(double z, Rhino.Geometry.Interval MinMax)
        {
            // Simple example of one way to change a number into a color.
            double s = MinMax.NormalizedParameterAt(z);
            s = Rhino.RhinoMath.Clamp((1 - s), 0, 1);
            double r_val = 0, g_val = 0, b_val = 0;
            double hue = s * 240;
            double c = 1.0;
            double x = (1 - Math.Abs((hue / 60) % 2 - 1));
            double m = 0.0;

            if (hue < 60)
            {
                r_val = c;
                g_val = x;
                b_val = 0;
            }
            else if (hue < 120)
            {
                r_val = x;
                g_val = c;
                b_val = 0;
            }
            else if (hue < 180)
            {
                r_val = 0;
                g_val = c;
                b_val = x;
            }
            else if (hue <= 240)
            {
                r_val = 0;
                g_val = x;
                b_val = c;
            }
            else
            {
                r_val = 0;
                g_val = 0;
                b_val = 0;
            }
            r_val += m;
            g_val += m;
            b_val += m;
            //if (s > 0.5)
            //{
            //    r_val = (int)((s-0.5) * 2 * 255);
            //    g_val = (int)((1 - s) * 2 * 255);
            //    b_val = 0;
            //}
            //else
            //{
            //    r_val = 0;
            //    g_val = (int)(s * 2 * 255);
            //    b_val = (int)((0.5-s) * 2 * 255);
            //}


            return System.Drawing.Color.FromArgb((int)(r_val * 255), (int)(g_val * 255), (int)(b_val * 255));
        }
    }
}
