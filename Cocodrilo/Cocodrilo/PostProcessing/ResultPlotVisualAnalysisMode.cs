﻿using System;
using Rhino;
using Rhino.Geometry;

using System.Collections.Generic;
using System.Linq;
using Cocodrilo.UserData;

namespace Cocodrilo.PostProcessing
{
    /// <summary>
    /// This class provides a false color based on the evaluation point results.
    /// For details, see the implementation of the FalseColor() function.
    /// </summary>
    public class ResultPlotVisualAnalysisMode : Rhino.Display.VisualAnalysisMode
    {
        //Interval m_res_range = CocodriloPlugIn.Instance.PostProcessingCocodrilo.MinMax;
        Interval m_hue_range = new Interval(0, 4 * Math.PI / 3);

        public override string Name { get { return "ResultPlot"; } }
        public override Rhino.Display.VisualAnalysisMode.AnalysisStyle Style { get { return AnalysisStyle.FalseColor; } }

        public Dictionary<int, int> mNumberEvaluationPoints;
        public Dictionary<int, Mesh> mVizualizationMeshes;

        public override bool ObjectSupportsAnalysisMode(Rhino.DocObjects.RhinoObject obj)
        {
            if (obj is Rhino.DocObjects.MeshObject || obj is Rhino.DocObjects.BrepObject)
                return true;
            return false;
        }

        public void ShowMeshBoundaryPoints(Rhino.DocObjects.RhinoObject obj, ref List<Guid> PointGuids)
        {
            var brep = obj.Geometry as Brep;

            if (brep == null)
                return;

            foreach (var brep_face in brep.Faces)
            {
                int brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(brep_face).BrepId;
                var vertices = mVizualizationMeshes[brep_id].Vertices;

                int num = mNumberEvaluationPoints[brep_id];

                for( int i = num; i < vertices.Count; i++)
                {
                    PointGuids.Add(RhinoDoc.ActiveDoc.Objects.AddPoint(vertices[i]));
                }
            }
        }

        public void RealMinMax(Rhino.DocObjects.RhinoObject obj, ref Interval tmp_min_max)
        {
            var brep = obj.Geometry as Brep;
            if (brep == null)
                return;

            var result_index = PostProcessing.s_SelectedCurrentResultDirectionIndex;
            foreach (var brep_face in brep.Faces)
            {
                int brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(brep_face).BrepId;
                var mesh = mVizualizationMeshes[brep_id];

                Dictionary<int, NurbsSurface> result_surfaces = new Dictionary<int, NurbsSurface>();
                Point3d result_uv;
                for (int i = 0; i < mesh.Vertices.Count; i++)
                {
                    double s, t, dist = 0;
                    ComponentIndex ci;
                    brep.ClosestPoint(mesh.Vertices[i], out _, out ci, out s, out t, dist, out _);

                    BrepFace result_brep_face;
                    int result_brep_id;
                    if (ci.ComponentIndexType == ComponentIndexType.BrepEdge)
                    {
                        var brep_edge = brep.Edges[ci.Index];
                        result_brep_face = brep.Trims[brep_edge.TrimIndices()[0]].Face;
                        result_brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(result_brep_face).BrepId;
                        var local_coordinates = brep.Trims[brep_edge.TrimIndices()[0]].PointAt(s);
                        s = local_coordinates[0];
                        t = local_coordinates[1];
                    }
                    else
                    {
                        result_brep_face = brep.Faces[ci.Index];
                        result_brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(result_brep_face).BrepId;
                    }
                    if (result_brep_id == -1) continue;
                    if (!result_surfaces.ContainsKey(result_brep_id))
                    {
                        result_surfaces.Add(result_brep_id, CreateResultNurbsPatch(brep_face.ToNurbsSurface(), result_brep_id, result_index));
                    }
                    result_surfaces[result_brep_id].Evaluate(s, t, 0, out result_uv, out _);
                    var value = result_uv.X;
                    if (value < tmp_min_max[0])
                        tmp_min_max[0] = value;
                    if (value > tmp_min_max[1])
                        tmp_min_max[1] = value;
                }
            }
        }

        public void UpdateVizualizationMesh(Rhino.DocObjects.RhinoObject obj, double MaximumEdgeLength)
        {
            var brep = obj.Geometry as Brep;
            if (brep == null)
                return;

            List<Point3d> list_evaluation_points = new List<Point3d>();
            // TODO: Remove this, when Kratos only provides trimmed ips
            foreach (var brep_face in brep.Faces)
            {
                var ud = brep_face.UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                if (ud == null)
                    continue;
                int brep_id = ud.BrepId;
                var nurbs_surface = brep_face.ToNurbsSurface();
                List<KeyValuePair<int,List<double>>> points_to_remove = new List<KeyValuePair<int, List<double>>>();
                foreach (var evaluation_point in PostProcessing.s_EvaluationPointList[brep_id])
                {

                    var point_face_relation = brep_face.IsPointOnFace(evaluation_point.Value[0], evaluation_point.Value[1]);
                    if (point_face_relation == Rhino.Geometry.PointFaceRelation.Exterior )
                    {
                        points_to_remove.Add(evaluation_point);
                    }
                }
                var list_brep_evauation_points = PostProcessing.s_EvaluationPointList[brep_id];
                foreach ( var point in points_to_remove)
                {
                    list_brep_evauation_points.Remove(point);
                }
            }

            foreach (var brep_face in brep.Faces)
            {
                int brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(brep_face).BrepId;

                var nurbs_surface = brep_face.ToNurbsSurface();
                var tmp_evaluation_point = new Point3d();
                foreach (var evaluation_point in PostProcessing.s_EvaluationPointList[brep_id])
                {
                    tmp_evaluation_point.X = evaluation_point.Value[0];
                    tmp_evaluation_point.Y = evaluation_point.Value[1];
                    tmp_evaluation_point.Z = 0.0;
                    list_evaluation_points.Add(tmp_evaluation_point);
                }

                if( mNumberEvaluationPoints == null)
                {
                    mNumberEvaluationPoints = new Dictionary<int, int>();
                }
                if (!mNumberEvaluationPoints.ContainsKey(brep_id))
                {
                    mNumberEvaluationPoints.Add(brep_id, list_evaluation_points.Count);
                } else
                {
                    mNumberEvaluationPoints[brep_id] = list_evaluation_points.Count;
                }
                // Get closed edges around face.
                List<List<Point3d>> closed_edges = new List<List<Point3d>>();
                foreach (var brep_edge_index in brep_face.AdjacentEdges())
                {
                    var edge = brep_face.Brep.Edges[brep_edge_index];

                    double length = edge.GetLength();
                    double segment_length = length / MaximumEdgeLength;

                    var curve2d = brep_face.Brep.Trims[brep_face.Brep.Edges[brep_edge_index].TrimIndices()[0]];
                    //List<int> visited_indices = new List<int>();
                    //foreach (var trim_index in tmp_brep.Edges[brep_edge_index].TrimIndices())
                    //{
                    //    var trim_curve = tmp_brep.Trims[trim_index];
                    //    int edge_index = trim_curve.Edge.EdgeIndex;
                    //    var check = trim_curve.Edge.IsPolyline();
                    //    var curve = trim_curve.Edge.EdgeCurve;
                    PolylineCurve poyline_curve = null;
                    Polyline polyline1;
                    try
                        {
                            if (!curve2d.IsValidWithLog(out string log))
                            {
                                RhinoApp.WriteLine(log);
                            }
                            //TODO: Maybe, it is useful to have some more parameters as user input.
                            var check = curve2d.TryGetPolyline(out polyline1);
                        RhinoApp.WriteLine(check.ToString());
                        //if(check)
                            poyline_curve = curve2d.ToPolyline(-1,1,0.1,0.1, 0.1, 0.01, 0,MaximumEdgeLength, true);
                        }
                        catch(Exception e) {
                            RhinoApp.WriteLine(e.ToString());
                        }
                        Polyline polyline;
                    var check3 = curve2d.TryGetPolyline(out polyline);
                        poyline_curve.TryGetPolyline(out polyline);
                    foreach (var line in polyline.GetSegments())
                        {
                            List<Point3d> line_segment = new List<Point3d>();
                            Point3d point1 = new Point3d(line.FromX, line.FromY, line.FromZ);
                            Point3d point2 = new Point3d(line.ToX, line.ToY, line.ToZ);
                            line_segment.Add(point1);
                            line_segment.Add(point2);
                            closed_edges.Add(line_segment);
                        }
                        //List<Point3d> line_segment_last = new List<Point3d>();
                        //var line_first = polyline.GetSegments().First();
                        //var line_last = polyline.GetSegments().Last();
                        //Point3d point1_last = new Point3d(line_last.ToX, line_last.ToY, line_last.ToZ);
                        //Point3d point2_last = new Point3d(line_first.FromX, line_first.FromY, line_first.FromZ);
                        //line_segment_last.Add(point1_last);
                        //line_segment_last.Add(point2_last);
                        //closed_edges.Add(line_segment_last);
                    //}
                }

                // Create Tesselation in Paramter space.
                Plane plane = new Plane();
                Plane.FitPlaneToPoints(list_evaluation_points, out plane);
                var new_mesh = Mesh.CreateFromTessellation(list_evaluation_points, closed_edges, plane, false);

                // Remove all faces, which have only vertices on the edges. These are faces within the trimmed domain.
                List<int> faces_to_remove = new List<int>();
                int counter = 0;
                foreach ( var face in new_mesh.Faces)
                {
                    int reference = mNumberEvaluationPoints[brep_id];
                    if ( face.IsTriangle)
                    {
                        if (face.A >= reference && face.B >= reference && face.C >= reference)
                        {
                            faces_to_remove.Add(counter);
                        }
                    }
                    if (face.IsQuad)
                    {
                        if (face.A >= reference && face.B >= reference && face.C >= reference && face.D >= reference)
                        {
                            faces_to_remove.Add(counter);
                        }
                    }
                    counter++;
                }
                // This loop must be in revers order.
                for( int i = faces_to_remove.Count - 1; i >= 0; --i)
                {
                    new_mesh.Faces.RemoveAt(faces_to_remove[i]);
                }

                // Map mesh into global space
                var global_coord = new Point3f();
                for (int i = 0; i < new_mesh.Vertices.Count; i++)
                {
                    var tmp_point = new_mesh.Vertices[i];
                    var result_point = new Point3d();
                    nurbs_surface.Evaluate(tmp_point.X, tmp_point.Y, 0, out result_point, out _);
                    global_coord.X = Convert.ToSingle(result_point.X);
                    global_coord.Y = Convert.ToSingle(result_point.Y);
                    global_coord.Z = Convert.ToSingle(result_point.Z);
                    new_mesh.Vertices[i] = global_coord;
                }

                // This makes the mesh smoother, but destroys vertex ordering.
                // Create mesh by ignoring dublicated vertices.
                //Mesh[] tmp_mesh = new Mesh[1];
                //tmp_mesh[0] = new_mesh;
/*                var new_mesh_list = Mesh.CreateFromIterativeCleanup(tmp_mesh, 1e-6);
                if (new_mesh_list[0] != null)
                {
                    new_mesh = new_mesh_list[0];
                }*/
                //new_mesh.RebuildNormals();
                //new_mesh.Weld(3);

                if (mVizualizationMeshes == null)
                {
                    mVizualizationMeshes = new Dictionary<int, Mesh>();
                }
                if (!mVizualizationMeshes.ContainsKey(brep_id))
                {
                    mVizualizationMeshes.Add(brep_id, new_mesh);
                }
                else
                {
                    mVizualizationMeshes[brep_id] = new_mesh;
                }
            }
        }

        protected override void UpdateVertexColors(Rhino.DocObjects.RhinoObject obj, Mesh[] meshes)
        {

            // A "mapping tag" is used to determine if the colors need to be set
            Rhino.Render.MappingTag mt = GetMappingTag(obj.RuntimeSerialNumber);

            var brep = obj.Geometry as Brep;
            if (brep == null)
                return;

            // Return if active meshes are empty
            if (meshes.Length == 0)
                return;

            foreach (var brep_face in brep.Faces)
            {
                int brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(brep_face).BrepId;
                Mesh custom_mesh = mVizualizationMeshes[brep_id];

                // Replace active mesh
                var mesh = meshes[0];
                mesh.Vertices.Clear();
                mesh.Vertices.AddVertices(custom_mesh.Vertices);
                mesh.Faces.Clear();
                mesh.Faces.AddFaces(custom_mesh.Faces);

                if (mesh.VertexColors.Tag.Id != this.Id || PostProcessing.s_UpdateResultPlotInAnalysisMode)
                {
                    int post_processing_index = PostProcessing.s_SelectedCurrentResultDirectionIndex;

                    // reset result range to current range.
                    //m_res_range = new Interval(
                    //    CocodriloPlugIn.Instance.PostProcessingCocodrilo.MinMax[0],
                    //    CocodriloPlugIn.Instance.PostProcessingCocodrilo.MinMax[1]);

                    // The mesh's mapping tag is different from ours. Either the mesh has
                    // no false colors, has false colors set by another analysis mode, has
                    // false colors set using different m_z_range[]/m_hue_range[] values, or
                    // the mesh has been moved.  In any case, we need to set the false
                    // colors to the ones we want.
                    System.Drawing.Color[] colors = new System.Drawing.Color[mesh.Vertices.Count];

                    double[] res = new double[mesh.Vertices.Count];

                    if (PostProcessing.s_CurrentResultInfo.NodeOrGauss == "OnNodes")
                    {
                        Dictionary<int, NurbsSurface> result_surfaces = new Dictionary<int, NurbsSurface>();
                        Point3d result_uv;
                        for (int i = 0; i < mesh.Vertices.Count; i++)
                        {
                            double s, t, dist = 0;
                            ComponentIndex ci;
                            brep.ClosestPoint(mesh.Vertices[i], out _, out ci, out s, out t, dist, out _);
                            BrepFace result_brep_face;
                            int result_brep_id;
                            if (ci.ComponentIndexType == ComponentIndexType.BrepEdge)
                            {
                                var brep_edge = brep.Edges[ci.Index];
                                result_brep_face = brep.Trims[brep_edge.TrimIndices()[0]].Face;
                                result_brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(result_brep_face).BrepId;
                                var local_coordinates = brep.Trims[brep_edge.TrimIndices()[0]].PointAt(s);
                                s = local_coordinates[0];
                                t = local_coordinates[1];
                            }
                            else
                            {
                                result_brep_face = brep.Faces[ci.Index];
                                result_brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(result_brep_face).BrepId;
                            }
                            if (result_brep_id == -1) continue;
                            if (!result_surfaces.ContainsKey(result_brep_id))
                            {
                                result_surfaces.Add(result_brep_id, CreateResultNurbsPatch(brep_face.ToNurbsSurface(), result_brep_id, post_processing_index));
                            }
                            result_surfaces[result_brep_id].Evaluate(s, t, 0, out result_uv, out _);
                            res[i] = result_uv.X;
                        }
                        //PostProcessing.s_MinMax.T0 = res.Min();
                        //PostProcessing.s_MinMax.T1 = res.Max();

                        for (int i = 0; i < colors.Count(); i++)
                            colors[i] = PostProcessingUtilities.FalseColor(res[i], PostProcessing.s_MinMax);
                    }
                    else {

                        for (int i = 0; i < mesh.Vertices.Count; i++)
                        {
                            if (i < mNumberEvaluationPoints[brep_id])
                            {
                                 double x = PostProcessing.s_EvaluationPointList[brep_id][i].Value[0];
                                double y = PostProcessing.s_EvaluationPointList[brep_id][i].Value[1];
                                Point3d test_point;
                                brep_face.Evaluate(x, y, 0, out test_point, out _);

                                res[i] = GetResultGP(brep_id, i, post_processing_index);
                                colors[i] = PostProcessingUtilities.FalseColor(res[i], PostProcessing.s_MinMax);
                            }
                            else {
                                double s, t, dist = 0;
                                ComponentIndex ci;
                                brep.ClosestPoint(mesh.Vertices[i], out _, out ci, out s, out t, dist, out _);
                                BrepFace result_brep_face;
                                int result_brep_id;
                                if (ci.ComponentIndexType == ComponentIndexType.BrepEdge)
                                {
                                    var brep_edge = brep.Edges[ci.Index];
                                    result_brep_face = brep.Trims[brep_edge.TrimIndices()[0]].Face;
                                    result_brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(result_brep_face).BrepId;
                                    var local_coordinates = brep.Trims[brep_edge.TrimIndices()[0]].PointAt(s);
                                    s = local_coordinates[0];
                                    t = local_coordinates[1];
                                }
                                else
                                {
                                    result_brep_face = brep.Faces[ci.Index];
                                    result_brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(result_brep_face).BrepId;
                                }
                                if (result_brep_id == -1) continue;
                                res[i] = FindClosestResultGP( result_brep_id, s, t, post_processing_index);

                                colors[i] = PostProcessingUtilities.FalseColor(res[i], PostProcessing.s_MinMax);
                            }
                        }
                    }
                    // Mesh must be added to ActiveDoc again.
                    RhinoDoc.ActiveDoc.Objects.AddMesh(mesh);

                    // set the mesh's color tag
                    mesh.VertexColors.SetColors(colors);
                    mesh.VertexColors.Tag = mt;
                }
            }
        }

/*        public void AddPointsToMesh(ref Mesh mesh, Point3d point)
        {

        }*/
        public override bool ShowIsoCurves
        {
            get
            {
                // Most shaded analysis modes that work on breps have the option of
                // showing or hiding isocurves.  Run the built-in Rhino ZebraAnalysis
                // to see how Rhino handles the user interface.  If controlling
                // iso-curve visability is a feature you want to support, then provide
                // user interface to set this member variable.
                return PostProcessing.s_ShowKnotSpanIsoCurves;
            }
        }

        /// <summary>
        /// Returns a mapping tag that is used to detect when a mesh's colors need to
        /// be set.
        /// </summary>
        /// <returns></returns>
        Rhino.Render.MappingTag GetMappingTag(uint serialNumber)
        {
            Rhino.Render.MappingTag mt = new Rhino.Render.MappingTag();
            mt.Id = this.Id;

            // Since the false colors that are shown will change if the mesh is
            // transformed, we have to initialize the transformation.
            mt.MeshTransform = Transform.Identity;

            // This is a 32 bit CRC or the information used to set the false colors.
            // For this example, the m_z_range and m_hue_range intervals control the
            // colors, so we calculate their crc.
            uint crc = RhinoMath.CRC32(serialNumber, PostProcessing.s_MinMax.T0);
            crc = RhinoMath.CRC32(crc, PostProcessing.s_MinMax.T1);
            crc = RhinoMath.CRC32(crc, m_hue_range.T0);
            crc = RhinoMath.CRC32(crc, m_hue_range.T1);
            mt.MappingCRC = crc;
            return mt;
        }

        private double GetResultGP(int PatchId, int point_index, int ResultIndex)
        {
            var current_results = PostProcessing.s_CurrentResultInfo.Results;
            var evaluation_point_id = PostProcessing.s_EvaluationPointList[PatchId][point_index].Key;
            //if (current_results == null) { return 0.0; }
            var current_result = current_results[evaluation_point_id];
            return (ResultIndex >= current_result.Length)
                    ? PostProcessingUtilities.GetVonMises(current_result)
                    : current_result[ResultIndex];
        }

        private double FindClosestResultGP(int PatchId, double u, double v, int ResultIndex)
        {
            int evaluation_point_id = 0;
            var point_local_coordinates = new Point3d(u, v, 0);
            var current_results = PostProcessing.s_CurrentResultInfo.Results;
            var result_info = PostProcessing.s_CurrentResultInfo;
            double min_dist = 1000000000;
            foreach (var point in PostProcessing.s_EvaluationPointList[PatchId])
            {
                var dist_tmp = point_local_coordinates.DistanceTo(new Point3d(point.Value[0], point.Value[1], 0.0));
                if (dist_tmp < min_dist)
                {
                    min_dist = dist_tmp;
                    evaluation_point_id = point.Key;
                }
            }
            //if (current_results == null) { return 0.0; }
            var closest_result = current_results[evaluation_point_id];
            return (ResultIndex >= closest_result.Length)
                ? PostProcessingUtilities.GetVonMises(closest_result)
                : closest_result[ResultIndex];
        }

        private NurbsSurface CreateResultNurbsPatch(NurbsSurface ThisNurbsSurface, int patch_id, int ResultIndex)
        {
            var nodes_per_patch_list = PostProcessing.s_BrepId_NodeId_Coordinates[patch_id];
            var result_surface = new NurbsSurface(ThisNurbsSurface);

            int num_points_u = result_surface.Points.CountU;
            int num_points_v = result_surface.Points.CountV;

            var current_results = Cocodrilo.PostProcessing.PostProcessing.s_CurrentResultInfo.Results;

            for (int point_u = 0; point_u < num_points_u; point_u++)
            {
                for (int point_v = 0; point_v < num_points_v; point_v++)
                {
                    var control_point_id = point_v * num_points_u + point_u;
                    var node_id = nodes_per_patch_list[control_point_id].Key;

                    if (current_results.ContainsKey(node_id))
                    {
                        var nodal_result = current_results[nodes_per_patch_list[control_point_id].Key];
                        double result = (ResultIndex >= nodal_result.Length)
                            ? PostProcessingUtilities.GetArrayLength(nodal_result)
                            : nodal_result[ResultIndex];
                        var point_result = new Point3d(result, 0, 0);
                        result_surface.Points.SetPoint(point_u, point_v, point_result);
                    }
                    else
                    {
                        var point_result = new Point3d(0, 0, 0);
                        result_surface.Points.SetPoint(point_u, point_v, point_result);
                    }
                }
            }
            return result_surface;
        }
        private double EvalNurbsPatchAt(NurbsSurface ResultSurface, double u, double v)
        {
            Point3d result_uv;
            ResultSurface.Evaluate(u, v, 0, out result_uv, out _);

            return result_uv.X;
        }
    }

}
