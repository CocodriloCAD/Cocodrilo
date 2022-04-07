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

        public Dictionary<int, List<KeyValuePair<int, List<double>>>> mBrepId_NodeId_Coordinates;
        public Dictionary<int, List<KeyValuePair<int, List<double>>>> mEvaluationPointList;
        // Current Results need to be stored within this object.
        public RESULT_INFO mCurrentResultInfo;
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

        public void ComputeMinMax(Rhino.DocObjects.RhinoObject obj, ref Interval MinMax, int SelectedResultDirection)
        {
            var brep = obj.Geometry as Brep;
            if (brep == null)
                return;

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
                        result_surfaces.Add(result_brep_id, CreateResultNurbsPatch(
                            brep_face.ToNurbsSurface(), result_brep_id, SelectedResultDirection, mBrepId_NodeId_Coordinates, mCurrentResultInfo));
                    }
                    result_surfaces[result_brep_id].Evaluate(s, t, 0, out result_uv, out _);
                    var value = result_uv.X;
                    MinMax[0] = Math.Min(MinMax[0], value);
                    MinMax[1] = Math.Max(MinMax[1], value);
                }
            }
        }
        public static void CreateRenderMesh(BrepFace ThisBrepFace,
            Dictionary<int, List<KeyValuePair<int, List<double>>>> EvaluationPointList,
            double MaximumEdgeLength)
        {
            List<Point3d> list_evaluation_points = new List<Point3d>();

            /// Eventually additional brep loop requrired.

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

            // Get closed edges around face.
            List<List<Point3d>> closed_edges = new List<List<Point3d>>();
            foreach (var brep_edge_index in ThisBrepFace.AdjacentEdges())
            {
                var edge = ThisBrepFace.Brep.Edges[brep_edge_index];

                double length = edge.GetLength();
                int number_of_segments = (int)(length / MaximumEdgeLength);

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

            // Create Tesselation in Paramter space.
            Plane plane = new Plane();
            Plane.FitPlaneToPoints(list_evaluation_points, out plane);
            var new_mesh = Mesh.CreateFromTessellation(list_evaluation_points, closed_edges, plane, false);

            // Remove all faces, which have only vertices on the edges. These are faces within the trimmed domain.
            List<int> faces_to_remove = new List<int>();
            int counter = 0;
            foreach (var face in new_mesh.Faces)
            {
                int reference = list_evaluation_points.Count;
                if (face.IsTriangle)
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
            for (int i = faces_to_remove.Count - 1; i >= 0; --i)
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

            ThisBrepFace.SetMesh(MeshType.Render, new_mesh);
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
                foreach (var evaluation_point in mEvaluationPointList[brep_id])
                {

                    var point_face_relation = brep_face.IsPointOnFace(evaluation_point.Value[0], evaluation_point.Value[1]);
                    if (point_face_relation == Rhino.Geometry.PointFaceRelation.Exterior )
                    {
                        points_to_remove.Add(evaluation_point);
                    }
                }
                //var list_brep_evauation_points = mEvaluationPointList[brep_id];
                //foreach ( var point in points_to_remove)
                //{
                //    list_brep_evauation_points.Remove(point);
                //}
            }

            foreach (var brep_face in brep.Faces)
            {
                int brep_id = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(brep_face).BrepId;

                var nurbs_surface = brep_face.ToNurbsSurface();
                var tmp_evaluation_point = new Point3d();
                foreach (var evaluation_point in mEvaluationPointList[brep_id])
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
                } else {
                    mNumberEvaluationPoints[brep_id] = list_evaluation_points.Count;
                }
                // Get closed edges around face.
                List<List<Point3d>> closed_edges = new List<List<Point3d>>();
                foreach (var brep_edge_index in brep_face.AdjacentEdges())
                {
                    var adjacent_edges = brep_face.AdjacentEdges();
                    var edge = brep_face.Brep.Edges[brep_edge_index];

   
                     double length = edge.GetLength();
                    int number_of_segments = (int) (length / MaximumEdgeLength);
                var curve2d = brep_face.Brep.Trims[brep_face.Brep.Edges[brep_edge_index].TrimIndices()[0]];
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
                
                if (closed_edges[0][0].DistanceTo(closed_edges.Last()[1]) > RhinoDoc.ActiveDoc.ModelAbsoluteTolerance)
                {
                    closed_edges.Add(new List<Point3d> {
                            closed_edges.Last()[1],
                            closed_edges[0][0] });
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
                if (brep_id == -1)
                    continue;
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

                    if (mCurrentResultInfo.NodeOrGauss == "OnNodes")
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
                                result_surfaces.Add(result_brep_id, CreateResultNurbsPatch(
                                    brep_face.ToNurbsSurface(), result_brep_id, post_processing_index, mBrepId_NodeId_Coordinates, mCurrentResultInfo));
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
                                double x = mEvaluationPointList[brep_id][i].Value[0];
                                double y = mEvaluationPointList[brep_id][i].Value[1];
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
            var current_results = mCurrentResultInfo.Results;
            var evaluation_point_id = mEvaluationPointList[PatchId][point_index].Key;
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
            var current_results = mCurrentResultInfo.Results;
            var result_info = mCurrentResultInfo;
            double min_dist = 1000000000;
            foreach (var point in mEvaluationPointList[PatchId])
            {
                // Eventually only check the quadratic distance. Check if this is significant faster.
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

        public static NurbsSurface CreateResultNurbsPatch(
            NurbsSurface ThisNurbsSurface,
            int patch_id,
            int ResultIndex,
            Dictionary<int, List<KeyValuePair<int, List<double>>>> BrepId_NodeId_Coordinates,
            RESULT_INFO CurrentResultInfo)
        {
            var nodes_per_patch_list = BrepId_NodeId_Coordinates[patch_id];
            var result_surface = new NurbsSurface(ThisNurbsSurface);

            int num_points_u = result_surface.Points.CountU;
            int num_points_v = result_surface.Points.CountV;

            var current_results = CurrentResultInfo.Results;

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
