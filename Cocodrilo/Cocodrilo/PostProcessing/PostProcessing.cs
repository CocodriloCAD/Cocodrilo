using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using Rhino;
using Rhino.Geometry;
using System.Windows;
using Cocodrilo.UserData;
using System.Threading.Tasks;

namespace Cocodrilo.PostProcessing
{
    public class PostProcessing
    {
        public List<RESULT_INFO> ResultList = new List<RESULT_INFO>();

        public Dictionary<int, List<KeyValuePair<int, List<double>>>> mEvaluationPointList;
        public Dictionary<int[], List<KeyValuePair<int, List<double>>>> mCouplingEvaluationPointList;

        //public Dictionary<int, NurbsSurface> BrepId_NurbsSurface = new Dictionary<int, NurbsSurface>();

        public Dictionary<int, Brep> BrepId_Breps;
        private Dictionary<int, Brep> BrepId_Breps_deformed;

        public Dictionary<int, List<KeyValuePair<int, List<double>>>> mBrepId_NodeId_Coordinates = new Dictionary<int, List<KeyValuePair<int, List<double>>>>();

        /// Variables to keep track through the
        public List<Guid> mPostprocessingObjects { get; set; }
        public List<Guid> mPostprocessingInitialObjects { get; set; }
        public List<Guid> PostprocessingObjectsGaussPoints { get; set; }
        public List<Guid> PostprocessingObjectsCouplingPoints { get; set; }
        public List<Guid> PostprocessingObjects1DResults { get; set; }
        public List<Guid> PostprocessingObjectsForcePatterns { get; set; }

        public List<Guid> mPostprocessingMesh { get; set; }

        /// Variables to detect the selected object
        public Dictionary<int, double[]> CurrentDisplacements()
            => ResultList.Find(r => r.LoadCaseNumber == mCurrentResultInfo.LoadCaseNumber &&
                                    r.ResultType == "\"DISPLACEMENT\"").Results;
        public List<string> CurrentDistinctResultTypes { get; set; }
        public List<string> DistinctLoadCaseTypes { get; set; }
        public List<int> DistinctLoadCaseNumbers { get; set; }
        public RESULT_INFO mCurrentResultInfo { get; private set; }
        public RESULT_INFO ResultInfo(string LoadCaseType, int LoadCaseNumber, string ResultType)
            => ResultList.Find(p => p.LoadCaseType == LoadCaseType &&
                                    p.LoadCaseNumber == LoadCaseNumber &&
                                    p.ResultType == ResultType);

        public static int s_SelectedCurrentResultDirectionIndex = 0;

        public double[] mCurrentMinMax = new double[] { 0, 1 };
        public static Interval s_MinMax = new Interval(0, 1);
        public static double s_MaxDistanceVertexOnMeshEdge = 1.0;

        public Rhino.Display.VisualAnalysisMode mVisualizationMode;

        /// <summary>
        /// Controls the Update of the Geometry if a different shape is expected.
        /// </summary>
        public bool mUpdateGeometry = false;
        public bool mUpdateGaussPoints = false;
        public bool mUpdateCouplingPoints = false;
        public bool mUpdateResultPlot = false;
        public bool mUpdateStressPatterns = true;
        public bool mUpdateVisualizationMesh = true;

        /// <summary>
        /// Controls if the color plot is being updated during the ResultPlotVisualAnalysisMode.
        /// Through the additional flag it is controlled that this happens only once the show
        /// process is restarted.
        /// </summary>
        public static bool s_UpdateResultPlotInAnalysisMode = false;

        public static bool s_ShowKnotSpanIsoCurves = false;

        public PostProcessing()
        {
        }
        public PostProcessing(string path)
        {
            var layer_post_processing = RhinoDoc.ActiveDoc.Layers.FindName("PostProcessing");
            if (layer_post_processing == null)
            {
                RhinoDoc.ActiveDoc.Layers.Add("PostProcessing", System.Drawing.Color.Black);
            }

            ReadData(path);
            UpdateInitialGeometry(true);

            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public void ReadData(string path)
        {
            mPostprocessingObjects = new List<Guid>();
            mPostprocessingInitialObjects = new List<Guid>();
            PostprocessingObjectsGaussPoints = new List<Guid>();
            PostprocessingObjectsCouplingPoints = new List<Guid>();
            PostprocessingObjects1DResults = new List<Guid>();
            PostprocessingObjectsForcePatterns = new List<Guid>();

            if (path != "dummy")
            {
                try
                {
                    BrepId_Breps = new Dictionary<int, Brep>();
                    BrepId_Breps_deformed = new Dictionary<int, Brep>();

                    mBrepId_NodeId_Coordinates = new Dictionary<int, List<KeyValuePair<int, List<double>>>>();
                    if (System.IO.File.Exists(path))
                    {
                        PostProcessingImportUtilities.LoadRhinoGeometries(
                            path, ref BrepId_Breps, ref mBrepId_NodeId_Coordinates);
                    }
                    else
                    {
                        RhinoApp.WriteLine("File does not exist: " + path);
                    }
                }
                catch
                {
                    RhinoApp.WriteLine("ERROR: could not read geometry file.");
                }

                var analysis_folder = Path.GetDirectoryName(path);
                string analysis_geometry_name = Path.GetFileName(path);
                int pos_georhino_in_analysis_geometry_name = analysis_geometry_name.IndexOf(".georhino.json");
                string analysis_name_with_kratos_0 = analysis_geometry_name.Substring(0, pos_georhino_in_analysis_geometry_name);
                int pos_kratos_0_in_analysis_name = analysis_geometry_name.IndexOf("_kratos_0");
                string analysis_name = analysis_geometry_name.Substring(0, pos_kratos_0_in_analysis_name);

                string[] result_files = System.IO.Directory.GetFiles(analysis_folder, analysis_name + "*.post.res");
                foreach (var file in result_files)
                {
                    try
                    {
                        if (File.Exists(file))
                            ResultList.AddRange(PostProcessingImportUtilities.LoadResults(file));
                    }
                    catch
                    {
                        RhinoApp.WriteLine("ERROR: could not read results from file: " + file);
                    }
                }


                string[] evaluation_point_files = System.IO.Directory.GetFiles(analysis_folder, analysis_name + "*_integrationdomain.json");
                mEvaluationPointList = new Dictionary<int, List<KeyValuePair<int, List<double>>>>();
                mCouplingEvaluationPointList = new Dictionary<int[], List<KeyValuePair<int, List<double>>>>();

                foreach (var file in evaluation_point_files)
                {
                    if (File.Exists(file))
                    {
                        try
                        {
                            PostProcessingImportUtilities.LoadEvaluationPoints(
                                file, ref mEvaluationPointList, ref mCouplingEvaluationPointList);
                        }
                        catch(Exception e)
                        {
                            RhinoApp.WriteLine("ERROR: could not read evaluation points from file: " + file + "\n" + e.ToString());
                        }
                    }
                }
            }

            if (mVisualizationMode != null)
            {
                (mVisualizationMode as ResultPlotVisualAnalysisMode).mBrepId_NodeId_Coordinates = mBrepId_NodeId_Coordinates;
                (mVisualizationMode as ResultPlotVisualAnalysisMode).mEvaluationPointList = mEvaluationPointList;
            }
            if (!ResultList.Exists(p => p.LoadCaseNumber == 0))
            {
                var Results = new Dictionary<int, double[]>();
                Results.Add(0, new double[] { 0, 0, 0 });
                var default_result_info = new RESULT_INFO() { LoadCaseType = "Load Case", LoadCaseNumber = 0, ResultType = "\"DISPLACEMENT\"", NodeOrGauss = "\"OnNodes\"", VectorOrScalar = "Vector", Results = Results };
                ResultList.Add(default_result_info);

                /// Set initial values.
                mCurrentResultInfo = default_result_info;

                if (mVisualizationMode != null)
                {
                    (mVisualizationMode as ResultPlotVisualAnalysisMode).mCurrentResultInfo = default_result_info;
                }
            }
            var load_cases_per_time_step = ResultList.Where(p =>
                    p.LoadCaseNumber == mCurrentResultInfo.LoadCaseNumber
                && (p.NodeOrGauss == "\"OnNodes\"" || p.NodeOrGauss == "\"OnGaussPoints\"")).ToList();
            CurrentDistinctResultTypes = load_cases_per_time_step.Select(p => p.ResultType).Distinct().ToList();
            DistinctLoadCaseTypes = load_cases_per_time_step.Select(p => p.LoadCaseType).Distinct().ToList();
            DistinctLoadCaseNumbers = ResultList.Select(p => p.LoadCaseNumber).Distinct().ToList();
            DistinctLoadCaseNumbers.Sort();

            RhinoDoc.ActiveDoc.Views.Redraw();
        }
        public void DrawGeometries(Dictionary<int, Brep> Breps,
            List<Guid> ObjectsList,
            System.Drawing.Color LayerColor,
            string LayerName = "Geometries")
        {
            foreach (var brep in Breps)
            {
                var attr = new Rhino.DocObjects.ObjectAttributes();
                attr.Name = "Brep_" + brep.Key;
                attr.LayerIndex = GetLayerIndex(LayerName, LayerColor);

                ObjectsList.Add(RhinoDoc.ActiveDoc.Objects.AddBrep(brep.Value, attr));
            }
        }

        public void UpdateCurrentResults(
            string LoadCaseType, int LoadCaseNumber, string ResultType)
        {
            bool update_load_case_numbers = (mCurrentResultInfo.LoadCaseNumber != LoadCaseNumber);

            if (ResultType == "")
            {
                ResultType = mCurrentResultInfo.ResultType;
            }

            LoadCaseNumber = Math.Max(LoadCaseNumber, 1);

            mCurrentResultInfo = ResultList.Find(p => p.LoadCaseType == LoadCaseType &&
                                                       p.LoadCaseNumber == LoadCaseNumber &&
                                                       p.ResultType == ResultType);

            if (mVisualizationMode != null)
            {
                (mVisualizationMode as ResultPlotVisualAnalysisMode).mCurrentResultInfo = mCurrentResultInfo;
            }

            if (update_load_case_numbers)
            {
                var load_cases_per_time_step = ResultList.Where(p =>
                        p.LoadCaseNumber == mCurrentResultInfo.LoadCaseNumber
                    && (p.NodeOrGauss == "OnNodes" || p.NodeOrGauss == "OnGaussPoints")).ToList();
                CurrentDistinctResultTypes = load_cases_per_time_step.Select(p => p.ResultType).Distinct().ToList();
                // Stress pattern updates once a new load case is considered.
                mUpdateStressPatterns = true;
            }

            UpdateCurrentMinMax();

            if (mCurrentResultInfo.ResultType == "\"DISPLACEMENT\"")
            {
                mUpdateGeometry = true;
                mUpdateGaussPoints = true;
                mUpdateCouplingPoints = true;
            }
            mUpdateResultPlot = true;
        }

        public void UpdateCurrentMinMax()
        {
            mCurrentMinMax = GetMinMax(mCurrentResultInfo, s_SelectedCurrentResultDirectionIndex);
        }
        public double[] GetMinMax(RESULT_INFO ResultInfo, int SelectedResultDirection)
        {
            double[] current_min_max = new double[] { 0, 0 };
            if (ResultInfo.Results == null || ResultInfo.Results.Count == 0)
            {
                return current_min_max;
            }
            else
            {
                if (ResultInfo.Results.First().Value.Length <= SelectedResultDirection)
                {
                    if (ResultInfo.ResultType == "\"DISPLACEMENT\"")
                    {
                        current_min_max[0] = ResultInfo.Results.Min(p => PostProcessingUtilities.GetArrayLength(p.Value));
                        current_min_max[1] = ResultInfo.Results.Max(p => PostProcessingUtilities.GetArrayLength(p.Value));
                    }
                    else
                    {
                        current_min_max[0] = ResultInfo.Results.Min(p => PostProcessingUtilities.GetVonMises(p.Value));
                        current_min_max[1] = ResultInfo.Results.Max(p => PostProcessingUtilities.GetVonMises(p.Value));
                    }
                }
                else
                {
                    current_min_max[0] = ResultInfo.Results.Min(p => p.Value[SelectedResultDirection]);
                    current_min_max[1] = ResultInfo.Results.Max(p => p.Value[SelectedResultDirection]);
                }

                return current_min_max;
            }
        }

        public void UpdateComputeMinMax()
        {
            s_MinMax = GetComputeMinMax(mCurrentResultInfo, s_SelectedCurrentResultDirectionIndex);
        }
        public Interval GetComputeMinMax(RESULT_INFO ResultInfo, int SelectedResultDirection)
        {
            Interval tmp_min_max = new Interval(1e10, -1e10);

            if (mCurrentResultInfo.NodeOrGauss.Contains("OnNodes"))
            {
                foreach (var brep_key_pair in BrepId_Breps)
                {
                    var brep = brep_key_pair.Value;
                    foreach (var brep_face in brep.Faces)
                    {
                        var mesh = brep_face.GetMesh(MeshType.Render);
                        if (mesh== null)
                        {
                            ResultPlotVisualAnalysisMode.CreateRenderMesh(brep_face, mEvaluationPointList, 0.1);
                            mesh = brep_face.GetMesh(MeshType.Render);
                        }

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
                                result_surfaces.Add(result_brep_id, ResultPlotVisualAnalysisMode.CreateResultNurbsPatch(
                                    brep_face.ToNurbsSurface(), result_brep_id, SelectedResultDirection, mBrepId_NodeId_Coordinates, ResultInfo));
                            }
                            result_surfaces[result_brep_id].Evaluate(s, t, 0, out result_uv, out _);
                            var value = result_uv.X;
                            tmp_min_max[0] = Math.Min(tmp_min_max[0], value);
                            tmp_min_max[1] = Math.Max(tmp_min_max[1], value);
                        }
                    }
                }
                RegisterVisualAnalysisMode();

                // Nodal values
                for (int i = 0; i < mPostprocessingObjects.Count; i++)
                {
                    var objc = RhinoDoc.ActiveDoc.Objects.Find(mPostprocessingObjects[i]);

                    if (mVisualizationMode != null)
                    {
                        (mVisualizationMode as ResultPlotVisualAnalysisMode).ComputeMinMax(objc, ref tmp_min_max, SelectedResultDirection);
                    }
                }
            }
            else
            {
                var min_max = GetMinMax(mCurrentResultInfo, SelectedResultDirection);
                tmp_min_max[0] = min_max[0];
                tmp_min_max[1] = min_max[1];
            }
            return tmp_min_max;
        }

        public void ShowMeshBoundaryPoints(ref List<Guid> ids)
        {
            double input = s_MaxDistanceVertexOnMeshEdge;

            Rhino.UI.Dialogs.ShowNumberBox("Vizualization Mesh Refinement", "Maximum Distance of Vertices on Mesh Edges", ref input);

            foreach (var id in ids)
            {
                RhinoDoc.ActiveDoc.Objects.Delete(id, false);
            }
            RhinoDoc.ActiveDoc.Views.Redraw();

            if ( (Math.Abs(input - s_MaxDistanceVertexOnMeshEdge) < 1e-5) && !mUpdateVisualizationMesh)
                return;

            s_MaxDistanceVertexOnMeshEdge = input;

            for (int i = 0; i < mPostprocessingObjects.Count; i++)
            {
                var objc = RhinoDoc.ActiveDoc.Objects.Find(mPostprocessingObjects[i]);
                if (objc == null)
                    continue;

                (mVisualizationMode as ResultPlotVisualAnalysisMode).UpdateVizualizationMesh(objc, s_MaxDistanceVertexOnMeshEdge);

                (mVisualizationMode as ResultPlotVisualAnalysisMode).ShowMeshBoundaryPoints(objc, ref ids);
            }

            PostProcessing.s_UpdateResultPlotInAnalysisMode = true;
            RhinoDoc.ActiveDoc.Views.Redraw();
            PostProcessing.s_UpdateResultPlotInAnalysisMode = false;
            mUpdateVisualizationMesh = false;
            ShowMeshBoundaryPoints(ref ids);
        }

        public void ShowPostProcessing(
            double scalingFactor,
            double flyingNodeLimit,
            double ResultScaling,
            bool ShowResultColorPlot,
            bool ShowEvaluationPoints,
            bool ShowCouplingEvaluationPoints,
            bool ShowCauchyStresses,
            bool ShowPK2Stresses,
            bool ShowPrincipalStresses,
            bool ShowUndeformed)
        {
            UpdateInitialGeometry(ShowUndeformed);

            VisualizeGeometry(scalingFactor, flyingNodeLimit, ShowResultColorPlot);
            UpdateEvaluationPoints(ShowEvaluationPoints, true);
            UpdateCouplingPoints(ShowCouplingEvaluationPoints, true);

            if (ShowCauchyStresses)
            {
                if (CurrentDistinctResultTypes.Contains("\"CAUCHY_STRESS\""))
                    VisualizeStressPatterns(true, ResultScaling, "\"CAUCHY_STRESS\"", ShowPrincipalStresses, true);
                else if (CurrentDistinctResultTypes.Contains("\"CAUCHY_STRESS_VECTOR\""))
                    VisualizeStressPatterns(true, ResultScaling, "\"CAUCHY_STRESS_VECTOR\"", ShowPrincipalStresses, true);
            }
            if (ShowPK2Stresses) {
                if (CurrentDistinctResultTypes.Contains("\"PK2_STRESS\""))
                    VisualizeStressPatterns(true, ResultScaling, "\"PK2_STRESS\"", ShowPrincipalStresses, true);
                else if (CurrentDistinctResultTypes.Contains("\"PK2_STRESS_VECTOR\""))
                    VisualizeStressPatterns(true, ResultScaling, "\"PK2_STRESS_VECTOR\"", ShowPrincipalStresses, true);
            }

            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public void ClearPostProcessing()
        {
            ResultList.Clear();
            var Results = new Dictionary<int, double[]>();
            Results.Add(0, new double[] { 0, 0, 0 });
            var default_result_info = new RESULT_INFO() { ResultType = "", LoadCaseNumber = 0, LoadCaseType = "", NodeOrGauss = "", VectorOrScalar = "", Results = Results };
            ResultList.Add(default_result_info);
            mCurrentResultInfo = default_result_info;
            DistinctLoadCaseTypes = ResultList.Select(p => p.LoadCaseType).Distinct().ToList();
            var load_cases_per_time_step = ResultList.Where(p => p.LoadCaseNumber == mCurrentResultInfo.LoadCaseNumber).ToList();
            CurrentDistinctResultTypes = load_cases_per_time_step.Select(p => p.ResultType).Distinct().ToList();
            DistinctLoadCaseNumbers = ResultList.Select(p => p.LoadCaseNumber).Distinct().ToList();
            mCurrentMinMax = new double[] { 0, 0 };

            mEvaluationPointList?.Clear();
            mCouplingEvaluationPointList?.Clear();

            mPostprocessingObjects = DeleteGeometries(mPostprocessingObjects);
            mPostprocessingInitialObjects = DeleteGeometries(mPostprocessingInitialObjects);
            PostprocessingObjectsGaussPoints = DeleteGeometries(PostprocessingObjectsGaussPoints);
            PostprocessingObjectsCouplingPoints = DeleteGeometries(PostprocessingObjectsCouplingPoints);
            PostprocessingObjects1DResults = DeleteGeometries(PostprocessingObjects1DResults);
            PostprocessingObjectsForcePatterns = DeleteGeometries(PostprocessingObjectsForcePatterns);
            mPostprocessingMesh = DeleteGeometries(mPostprocessingMesh);

            var geometry_layer = RhinoDoc.ActiveDoc.Layers.FindName("PostProcessing");
            if (geometry_layer != null)
            {
                RhinoDoc.ActiveDoc.Layers.Delete(geometry_layer);
            }

            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public void FreezePostProcessing()
        {
            ResultList.Clear();
            mEvaluationPointList.Clear();
            mCouplingEvaluationPointList.Clear();
            CurrentDistinctResultTypes.Clear();

            mPostprocessingObjects.Clear();
            mPostprocessingInitialObjects.Clear();
            PostprocessingObjectsGaussPoints.Clear();
            PostprocessingObjectsCouplingPoints.Clear();
            PostprocessingObjects1DResults.Clear();
            PostprocessingObjectsForcePatterns.Clear();

            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public int GetLayerIndex(string LayerName, System.Drawing.Color LayerColor)
        {
            // Get respective layer
            var layer = RhinoDoc.ActiveDoc.Layers.FindName(LayerName);
            if (layer == null)
            {
                var new_layer = new Rhino.DocObjects.Layer();
                new_layer.Name = LayerName;
                new_layer.ParentLayerId = RhinoDoc.ActiveDoc.Layers.FindName("PostProcessing").Id;
                new_layer.Color = LayerColor;
                return RhinoDoc.ActiveDoc.Layers.Add(new_layer);
            }
            return layer.Index;
        }

        public bool CheckMissingGeometry(List<Guid> GeometryGuids)
        {
            for (int i = 0; i < GeometryGuids.Count; i++)
            {
                var objc = RhinoDoc.ActiveDoc.Objects.Find(GeometryGuids[i]);
                if (objc == null)
                {
                    return true;
                }
            }
            return false;
        }

        public List<Guid> DeleteGeometries(List<Guid> GeometryGuids)
        {
            if (GeometryGuids != null)
            {
                GeometryGuids.ForEach(item => RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(item), false, true));
                GeometryGuids.Clear();
                RhinoDoc.ActiveDoc.Views.Redraw();
            }
            else
            {
                GeometryGuids = new List<Guid>();
            }
            return GeometryGuids;
        }

        private void RegisterVisualAnalysisMode()
        {
            if (mVisualizationMode == null)
            {
                // Colorplot
                mVisualizationMode = Rhino.Display.VisualAnalysisMode.Register(typeof(ResultPlotVisualAnalysisMode));
                (mVisualizationMode as ResultPlotVisualAnalysisMode).mBrepId_NodeId_Coordinates = mBrepId_NodeId_Coordinates;
                (mVisualizationMode as ResultPlotVisualAnalysisMode).mEvaluationPointList = mEvaluationPointList;
                (mVisualizationMode as ResultPlotVisualAnalysisMode).mCurrentResultInfo = mCurrentResultInfo;
            }
        }

        public void UpdatePostProcessingMeshes(bool show)
        {
            mPostprocessingMesh = DeleteGeometries(mPostprocessingMesh);

            if (!show)
                return;

            for (int i = 0; i < mPostprocessingObjects.Count; i++)
            {
                var objc = RhinoDoc.ActiveDoc.Objects.Find(mPostprocessingObjects[i]);
                if (objc == null)
                    continue;

                foreach (var mesh in (mVisualizationMode as ResultPlotVisualAnalysisMode).mVizualizationMeshes)
                {
                    var attr = new Rhino.DocObjects.ObjectAttributes();
                    attr.Name = "Brep_" + mesh.Key + "_mesh";
                    attr.LayerIndex = GetLayerIndex("PostProcessingMeshes", System.Drawing.Color.FromArgb(162, 173, 0));

                    mPostprocessingMesh.Add(RhinoDoc.ActiveDoc.Objects.AddMesh(mesh.Value));
                }
            }
            RhinoDoc.ActiveDoc.Views.Redraw();
        }


        public void UpdateInitialGeometry(bool Show)
        {
            mPostprocessingInitialObjects = DeleteGeometries(mPostprocessingInitialObjects);
            if (Show)
            {
                DrawGeometries(BrepId_Breps, mPostprocessingInitialObjects, System.Drawing.Color.FromArgb(150, 150, 150), "GeometriesInitial");
            }
        }

        public Dictionary<int, Brep> GetDeformedGeometries(
            double ScalingFactor,
            double FlyingNodeLimit,
            Dictionary<int, double[]> DisplacementResults)
        {
            var deformed_breps = new Dictionary<int, Brep>();
            foreach (var brep in BrepId_Breps)
            {
                int index = 0;
                foreach (var brep_face in brep.Value.Faces)
                {
                    Brep new_brep = brep.Value.DuplicateSubBrep(new List<int>() { index });
                    index++;
                    var new_brep_face = new_brep.Faces[0];
                    var ud_old = UserData.UserDataUtilities.GetOrCreateUserDataSurface(brep_face);

                    var ud = new_brep_face.UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                    if (ud == null)
                    {
                        //ud = ud_old;
                        //new_brep_face.UserData.Add(ud.BrepId = ud_old.BrepId);
                    }
                    if (ud.BrepId == -1)
                    {
                        //new_brep_face.UserData.Remove(ud_old);
                        ud.BrepId = ud_old.BrepId;
                        //new_brep_face.UserData.Add(ud);
                    }

                    if (mBrepId_NodeId_Coordinates.ContainsKey(ud.BrepId))
                    {
                        var surface = new_brep_face.UnderlyingSurface();
                        var nurbs_surface = surface.ToNurbsSurface();

                        int u = 0;
                        int v = 0;
                        Brep this_updated_face_brep = new Brep();
                        foreach (var control_point in mBrepId_NodeId_Coordinates[ud.BrepId])
                        {
                            var ControlPointID = v * nurbs_surface.Points.CountU + u;

                            var nodal_displacements = DisplacementResults.ContainsKey(control_point.Key)
                                ? DisplacementResults[control_point.Key]
                                : new double[] { 0, 0, 0 };

                            /// avoid flying nodes
                            double disp_x = Math.Min(FlyingNodeLimit, nodal_displacements[0]);
                            double disp_y = Math.Min(FlyingNodeLimit, nodal_displacements[1]);
                            double disp_z = Math.Min(FlyingNodeLimit, nodal_displacements[2]);

                            nurbs_surface.Points.SetPoint(u, v,
                                                            new Rhino.Geometry.Point3d(
                                                                control_point.Value[0] + disp_x * ScalingFactor,
                                                                control_point.Value[1] + disp_y * ScalingFactor,
                                                                control_point.Value[2] + disp_z * ScalingFactor),
                                                            control_point.Value[3]);

                            u++;
                            if (u >= nurbs_surface.Points.CountU)
                            {
                                u = 0;
                                v++;
                            }
                            if (v >= nurbs_surface.Points.CountV)
                                continue;
                        }
                        int surface_index = new_brep.AddSurface(nurbs_surface);
                        new_brep_face.ChangeSurface(surface_index);
                        new_brep_face.RebuildEdges(0.00001, false, true);
                        new_brep.CullUnusedSurfaces();
                    }
                    deformed_breps.Add(ud.BrepId, new_brep);
                }
            }
            return deformed_breps;
        }

        public void VisualizeGeometry(
            double ScalingFactor,
            double FlyingNodeLimit,
            bool ShowResultColorPlot)
        {
            bool check_missing_geometries = CheckMissingGeometry(mPostprocessingObjects);
            if (mUpdateGeometry || check_missing_geometries)
            {
                mPostprocessingObjects = DeleteGeometries(mPostprocessingObjects);

                BrepId_Breps_deformed = GetDeformedGeometries(ScalingFactor, FlyingNodeLimit, CurrentDisplacements());

                DrawGeometries(BrepId_Breps_deformed, mPostprocessingObjects, System.Drawing.Color.FromArgb(90, 90, 90), "Geometries");
            }
/*            if (mUpdateGeometry || check_geometries_missing)
            {
                for (int i = 0; i < mPostprocessingObjects.Count; i++)
                {
                    var check = RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(mPostprocessingObjects[i]), false, true);
                }
                mPostprocessingObjects.Clear();

                Dictionary<int, Brep> deformed_breps = new Dictionary<int, Brep>();
                foreach (var brep in BrepId_Breps)
                {
                    int index = 0;
                    foreach (var brep_face in brep.Value.Faces)
                    {
                        Brep new_brep = brep.Value.DuplicateSubBrep(new List<int>() { index });
                        index++;
                        var new_brep_face = new_brep.Faces[0];
                        var ud_old = UserDataUtilities.GetOrCreateUserDataSurface(brep_face);

                        var ud = new_brep_face.UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                        if (ud == null)
                        {
                            ud = ud_old;
                        }
                        deformed_breps.Add(ud.BrepId, new_brep);
                    }
                }
                DrawGeometries(deformed_breps, mPostprocessingObjects, System.Drawing.Color.FromArgb(90, 90, 90), "Geometries");

                int order_x = 2;
                int order_y = 2;
                int order_z = 2;
                int cp_x = 13;
                int cp_y = 13;
                int cp_z = 9;

                string directory = "Z:/tmp/Internal_pressure/test.post.res";
                CppWrapper.CageEdit(mPostprocessingObjects, directory, order_x, order_y, order_z, cp_x, cp_y, cp_z, ScalingFactor);
            }*/
            if (mUpdateResultPlot || check_missing_geometries || mUpdateVisualizationMesh)
            {
                // Enables that color plot is being updated.
                s_UpdateResultPlotInAnalysisMode = true;

                RegisterVisualAnalysisMode();
                int count = 0;
                for (int i = 0; i < mPostprocessingObjects.Count; i++)
                {
                    var objc = RhinoDoc.ActiveDoc.Objects.Find(mPostprocessingObjects[i]);
                    if (objc == null)
                        continue;

                    if (ShowResultColorPlot)
                    {
                        // see if this object is alreay in Z analysis mode
                        if (objc.InVisualAnalysisMode(mVisualizationMode))
                            continue;

                        if (objc.EnableVisualAnalysisMode(mVisualizationMode, true))
                            count++;
                    }
                    else
                    {
                        if (objc.EnableVisualAnalysisMode(mVisualizationMode, false))
                            count++;
                    }

                    if (mUpdateVisualizationMesh || mUpdateGeometry)
                    {
                        (mVisualizationMode as ResultPlotVisualAnalysisMode).UpdateVizualizationMesh(objc, s_MaxDistanceVertexOnMeshEdge);
                    }
                }

                RhinoDoc.ActiveDoc.Views.Redraw();
                // var visual_analysis_mode_2 = Rhino.Display.VisualAnalysisMode.Register(typeof(ResultPlotVisualAnalysisMode));
                // Rhino.Display.VisualAnalysisMode.AdjustAnalysisMeshes(RhinoDoc.ActiveDoc, visual_analysis_mode.Id);
                // Disables that color plot is being updated.
                s_UpdateResultPlotInAnalysisMode = false;
            }

            mUpdateResultPlot = false;
            mUpdateGeometry = false;
            mUpdateVisualizationMesh = false;

            //////    //1D results
            //////    //delete old 1d results
            //////    for (int i = 0; i < PostprocessingObjects1DResults.Count; i++)
            //////    {
            //////        RhinoDoc.ActiveDoc.Objects.Delete(PostprocessingObjects1DResults[i], false);
            //////    }
            //////    PostprocessingObjects1DResults.Clear();
            //////    //generate new 1d results if required
            //////    //check if result is gp_result
            //////    if (CurrentResults[0].ContainsKey("PatchId"))
            //////    {
            //////        for (int i = 0; i < PostprocessingObjects.Count; i++)
            //////        {

            //////            var objc = RhinoDoc.ActiveDoc.Objects.Find(PostprocessingObjects[i]);
            //////            if (objc == null)
            //////                continue;

            //////            if (Flag_ShowResult)
            //////            {
            //////                var attr = new Rhino.DocObjects.ObjectAttributes();
            //////                var layer_gp = RhinoDoc.ActiveDoc.Layers.FindName("1DResults");
            //////                int layerIndex = -1;
            //////                if (layer_gp == null)
            //////                {
            //////                    var layer_geo_new = new Rhino.DocObjects.Layer();
            //////                    layer_geo_new.Name = "1DResults";
            //////                    layer_geo_new.ParentLayerId = RhinoDoc.ActiveDoc.Layers.FindName("Postprocessing").Id;
            //////                    layer_geo_new.Color = System.Drawing.Color.FromArgb(50, 50, 50);
            //////                    layerIndex = RhinoDoc.ActiveDoc.Layers.Add(layer_geo_new);
            //////                }
            //////                else
            //////                    layerIndex = layer_gp.Index;
            //////                attr.LayerIndex = layerIndex;

            //////                var crv = objc.Geometry as Curve;
            //////                //if (crv != null)
            //////                //{
            //////                //    //get result list
            //////                //    int patch_id = Convert.ToInt32(objc.Attributes.Name);
            //////                //    var patch_res = carat_postprocessing.Postprocessing.CurrentResults.Where(p => (int)p["PatchId"] == patch_id).ToList();
            //////                //    int gp_count = patch_res.Count;
            //////                //    attr.Name = "1DResult_" + patch_id.ToString();

            //////                //    if (gp_count > 0)
            //////                //    {
            //////                //        NurbsCurve nrbs = crv.ToNurbsCurve();
            //////                //        List<Guid> ln_guid_list = new List<Guid>();

            //////                //        for (int i_gp = 0; i_gp < gp_count; i_gp++)
            //////                //        {
            //////                //            double u = ((Point3d)patch_res[i_gp]["ParLoc"]).X;      //gp coordinate in parameter space
            //////                //            double res = (double)patch_res[i_gp]["Result"];         //result
            //////                //            Point3d pt = nrbs.PointAt(u);                           //gp coordinate in geometry space
            //////                //            Vector3d dir = nrbs.CurvatureAt(u);                     //direction of result
            //////                //            if (dir.Length < 1e-13)                                 //if curve is straight
            //////                //            {
            //////                //                Vector3d tan = nrbs.TangentAt(u);
            //////                //                tan.Unitize();
            //////                //                Vector3d nrm = new Vector3d(0, 0, 1);
            //////                //                if (Math.Abs(tan.Z) > 0.99999)
            //////                //                {
            //////                //                    nrm.Z = 0.0;
            //////                //                    nrm.Y = 1.0;
            //////                //                }
            //////                //                dir = Vector3d.CrossProduct(tan, nrm);
            //////                //            }
            //////                //            Line ln = new Line(pt, dir, res * result_scaling);
            //////                //            if (ln != null && ln.IsValid)
            //////                //            {
            //////                //                //var guid_srf = RhinoDoc.ActiveDoc.Objects.AddSurface(geo, attr);
            //////                //                var guid_ln = RhinoDoc.ActiveDoc.Objects.AddLine(ln, attr);
            //////                //                if (guid_ln != Guid.Empty)
            //////                //                {
            //////                //                    PostprocessingObjects1DResults.Add(guid_ln);
            //////                //                    ln_guid_list.Add(guid_ln);
            //////                //                    //rc = Rhino.Commands.Result.Success;
            //////                //                }
            //////                //            }
            //////                //        }
            //////                //        string group_name = "1DRes_" + patch_id;
            //////                //        int index = RhinoDoc.ActiveDoc.Groups.Add(group_name, ln_guid_list);
            //////                //        //RhinoDoc.ActiveDoc.Views.Redraw();
            //////                //    }
            //////                //}
            //////                //else
            //////                if (crv == null)
            //////                {
            //////                    if (CurrentResultType == "\"Normal Force\"" || CurrentResultType == "\"Bending Moment 1\"" || CurrentResultType == "\"Bending Moment 2\"" || CurrentResultType == "\"Torsional Moment\"")    //HC only known result types are represented by 1d curves
            //////                    {
            //////                        var brp = objc.Geometry as Brep;
            //////                        //if (brp != null)
            //////                        //{
            //////                        //    //get result list
            //////                        //    int patch_id_master = Convert.ToInt32(objc.Attributes.Name);
            //////                        //    var patch_res = carat_postprocessing.Postprocessing.CurrentResults.Where(p => (int)p["PatchId"] == patch_id_master).ToList();
            //////                        //    int gp_count = patch_res.Count;
            //////                        //    attr.Name = "1DResult_" + patch_id_master.ToString();
            //////                        //    //disable colorplot for 2d patches with 1dresults
            //////                        //    objc.EnableVisualAnalysisMode(rmode, false);


            //////                        //    if (gp_count > 0)
            //////                        //    {
            //////                        //        NurbsSurface nrbs = brp.Surfaces[0].ToNurbsSurface();
            //////                        //        List<Guid> ln_guid_list = new List<Guid>();

            //////                        //        for (int i_gp = 0; i_gp < gp_count; i_gp++)
            //////                        //        {
            //////                        //            double u = ((Point3d)patch_res[i_gp]["ParLoc"]).X;      //gp coordinate in parameter space
            //////                        //            double v = ((Point3d)patch_res[i_gp]["ParLoc"]).Y;      //gp coordinate in parameter space
            //////                        //            double res = (double)patch_res[i_gp]["Result"];         //result
            //////                        //            Point3d pt = nrbs.PointAt(u, v);                           //gp coordinate in geometry space
            //////                        //            Vector3d dir = nrbs.NormalAt(u, v);                     //direction of result
            //////                        //            Line ln = new Line(pt, dir, res * result_scaling);
            //////                        //            if (ln != null && ln.IsValid)
            //////                        //            {
            //////                        //                //var guid_srf = RhinoDoc.ActiveDoc.Objects.AddSurface(geo, attr);
            //////                        //                var guid_ln = RhinoDoc.ActiveDoc.Objects.AddLine(ln, attr);
            //////                        //                if (guid_ln != Guid.Empty)
            //////                        //                {
            //////                        //                    PostprocessingObjects1DResults.Add(guid_ln);
            //////                        //                    ln_guid_list.Add(guid_ln);
            //////                        //                    //rc = Rhino.Commands.Result.Success;
            //////                        //                }
            //////                        //            }
            //////                        //        }
            //////                        //        string group_name = "1DRes_" + patch_id_master;
            //////                        //        int index = RhinoDoc.ActiveDoc.Groups.Add(group_name, ln_guid_list);
            //////                        //        //RhinoDoc.ActiveDoc.Views.Redraw();
            //////                        //    }
            //////                        //}
            //////                    }
            //////                }
            //////            }
            //////        }
            //////    }
            //////    RhinoDoc.ActiveDoc.Views.Redraw();
            //////    updateResultAnalysisMode = false;
            //////}
            //////updateResultPlot = false;

            //////updateGeometry = false;
        }

        public void UpdateEvaluationPoints(bool show_evaluation_points, bool Deformed)
        {
            if (mUpdateGaussPoints || CheckMissingGeometry(PostprocessingObjectsGaussPoints))
            {
                PostprocessingObjectsGaussPoints = DeleteGeometries(PostprocessingObjectsGaussPoints);

                if (show_evaluation_points)
                {

                    foreach (var brep_id_evaluation_points in mEvaluationPointList)
                    {
                        List<Point3d> gp_list = new List<Point3d>();

                        BrepFace brep_face = GetGeometry(brep_id_evaluation_points.Key, Deformed) as BrepFace;
                        if (brep_face == null) continue;

                        var nurbs_surface = brep_face.ToNurbsSurface();
                        foreach (var evaluation_point in brep_id_evaluation_points.Value)
                        {
                            nurbs_surface.Evaluate(evaluation_point.Value[0], evaluation_point.Value[1], 0, out Point3d gp_pt, out _);
                            gp_list.Add(gp_pt);
                        }

                        // Group points
                        var attr = new Rhino.DocObjects.ObjectAttributes();
                        attr.Name = "Points_" + brep_id_evaluation_points.Key;
                        attr.LayerIndex = GetLayerIndex("EvaluationPoints", System.Drawing.Color.FromArgb(0, 101, 189));

                        var point_guids = RhinoDoc.ActiveDoc.Objects.AddPoints(gp_list, attr);
                        if (point_guids != null)
                        {
                            string group_name = "Points_" + brep_id_evaluation_points.Key;
                            RhinoDoc.ActiveDoc.Groups.Add(group_name, point_guids);
                            PostprocessingObjectsGaussPoints.AddRange(point_guids);
                            RhinoDoc.ActiveDoc.Views.Redraw();
                        }
                    }
                }
                else
                {
                    RhinoDoc.ActiveDoc.Views.Redraw();
                }
            }
            mUpdateGaussPoints = false;
        }

        public void UpdateCouplingPoints(bool show_coupling_evaluation_points, bool Deformed)
        {
            if (mUpdateCouplingPoints || CheckMissingGeometry(PostprocessingObjectsCouplingPoints))
            {
                PostprocessingObjectsCouplingPoints = DeleteGeometries(PostprocessingObjectsCouplingPoints);

                if (show_coupling_evaluation_points)
                {
                    foreach (var brep_ids in mCouplingEvaluationPointList)
                    {
                        List<Point3d> evaluation_point_list = new List<Point3d>();
                        List<Line> line_list = new List<Line>();

                        var geometry_1 = GetGeometry(brep_ids.Key[0], Deformed);
                        var geometry_2 = GetGeometry(brep_ids.Key[1], Deformed);
                        if (geometry_1 == null || geometry_2 == null) continue;
                        if (geometry_1 is BrepFace && geometry_2 is BrepFace)
                        {
                            var nurbs_surface_1 = (geometry_1 as BrepFace).ToNurbsSurface();
                            var nurbs_surface_2 = (geometry_2 as BrepFace).ToNurbsSurface();

                            foreach (var evaluation_point in brep_ids.Value)
                            {
                                nurbs_surface_1.Evaluate(evaluation_point.Value[0], evaluation_point.Value[1], 0, out Point3d e_pt_1, out _);
                                evaluation_point_list.Add(e_pt_1);

                                nurbs_surface_2.Evaluate(evaluation_point.Value[2], evaluation_point.Value[3], 0, out Point3d e_pt_2, out _);
                                evaluation_point_list.Add(e_pt_2);

                                line_list.Add(new Line(e_pt_1, e_pt_2));
                            }
                        }

                        var attr = new Rhino.DocObjects.ObjectAttributes();
                        attr.Name = "CouplingPoints_" + brep_ids.Key[0] + "_" + brep_ids.Key[1];
                        attr.LayerIndex = GetLayerIndex("CouplingPoints", System.Drawing.Color.FromArgb(227, 114, 34));

                        if (evaluation_point_list.Count > 0)
                        {
                            var guids_pts = RhinoDoc.ActiveDoc.Objects.AddPoints(evaluation_point_list, attr);
                            if (guids_pts != null)
                            {
                                foreach (var line in line_list)
                                {
                                    var guid_ln = RhinoDoc.ActiveDoc.Objects.AddLine(line, attr);
                                    if (guid_ln != null)
                                        guids_pts.Add(guid_ln);
                                }
                                string group_name = "CouplingPoints_" + brep_ids.Key[0] + "_" + brep_ids.Key[1];
                                RhinoDoc.ActiveDoc.Groups.Add(group_name, guids_pts);
                                PostprocessingObjectsCouplingPoints.AddRange(guids_pts);
                            }
                        }
                    }
                }
            }

            mUpdateCouplingPoints = false;
        }

        public void VisualizeCouplingStresses(
            bool Deformed,
            double ResultScaling)
        {
            var stress_result_tangent_1_info = ResultList.Find(r => r.LoadCaseNumber == mCurrentResultInfo.LoadCaseNumber &&
                                                          r.ResultType == "\"PENALTY_FACTOR_TANGENT_1\"");
            var stress_result_tangent_2_info = ResultList.Find(r => r.LoadCaseNumber == mCurrentResultInfo.LoadCaseNumber &&
                                                          r.ResultType == "\"PENALTY_FACTOR_TANGENT_2\"");
            var stress_result_normal_info = ResultList.Find(r => r.LoadCaseNumber == mCurrentResultInfo.LoadCaseNumber &&
                                                          r.ResultType == "\"PENALTY_FACTOR_NORMAL\"");

            double current_min_tangent_1 = stress_result_tangent_1_info.Results.Min(p => p.Value.Min());
            double current_max_tangent_1 = stress_result_tangent_1_info.Results.Max(p => p.Value.Max());

            double current_min_tangent_2 = stress_result_tangent_2_info.Results.Min(p => p.Value.Min());
            double current_max_tangent_2 = stress_result_tangent_2_info.Results.Max(p => p.Value.Max());

            double current_min_normal = stress_result_normal_info.Results.Min(p => p.Value.Min());
            double current_max_normal = stress_result_normal_info.Results.Max(p => p.Value.Max());

            Interval min_max_interval_tangent_1 = new Interval(current_min_tangent_1, current_max_tangent_1);
            Interval min_max_interval_tangent_2 = new Interval(current_min_tangent_2, current_max_tangent_2);
            Interval min_max_interval_normal = new Interval(current_min_normal, current_max_normal);

            //PostprocessingObjectsCouplingPoints = DeleteGeometries(PostprocessingObjectsCouplingPoints);

            foreach (var brep_ids in mCouplingEvaluationPointList)
            {
                List<Point3d> evaluation_point_list = new List<Point3d>();
                List<Line> line_list = new List<Line>();

                var geometry_1 = GetGeometry(brep_ids.Key[0], Deformed);
                if (geometry_1 == null) continue;
                if (geometry_1 is BrepFace)
                {
                    var nurbs_surface_1 = (geometry_1 as BrepFace).ToNurbsSurface();
                    var brep = (geometry_1 as BrepFace).Brep;


                    foreach (var evaluation_point in brep_ids.Value)
                    {
                        var result_tangent_1 = stress_result_tangent_1_info.Results[evaluation_point.Key];
                        var result_tangent_2 = stress_result_tangent_2_info.Results[evaluation_point.Key];
                        var result_normal = stress_result_normal_info.Results[evaluation_point.Key];

                        nurbs_surface_1.Evaluate(evaluation_point.Value[0], evaluation_point.Value[1], 0, out Point3d point_1, out _);
                        var surface_normal = nurbs_surface_1.NormalAt(evaluation_point.Value[0], evaluation_point.Value[1]);
                        surface_normal.Unitize();
                        brep.FindCoincidentBrepComponents(point_1, 1e-1, out int[] faces, out int[] edges, out int[] vertices);
                        if (edges.Length > 0)
                        {
                            var brep_edge = brep.Edges[edges[0]];
                            brep_edge.ClosestPoint(point_1, out double t_edge);
                            var coordinate_2 = brep_edge.PointAt(t_edge + 0.001);
                            //var coordinate_2 = brep_edge.EdgeCurve.PointAt(t_edge + 0.01);
                            //var local_coordinate_2 = brep.Trims[brep_edge.TrimIndices()[0]].PointAt(t_edge + 0.001);

                            //nurbs_surface_1.Evaluate(coordinate_2[0], coordinate_2[1], 0, out Point3d point_direction_t2, out _);

                            Vector3d tangent_2 = coordinate_2 - point_1;
                            tangent_2.Unitize();

                            var tangent_1 = Vector3d.CrossProduct(surface_normal, tangent_2);

                            var line_1 = new Rhino.Geometry.Line(point_1, point_1 + tangent_1 * result_tangent_1[0] * ResultScaling);

                            var attributes_tangent_1 = PostProcessingUtilities.GetStressPatternObjectAttributes(result_tangent_1[0], min_max_interval_tangent_1);
                            PostprocessingObjectsForcePatterns.Add(RhinoDoc.ActiveDoc.Objects.AddLine(line_1, attributes_tangent_1));

                            var line_2 = new Rhino.Geometry.Line(point_1, point_1 + tangent_2 * result_tangent_2[0] * ResultScaling);

                            var attributes_tangent_2 = PostProcessingUtilities.GetStressPatternObjectAttributes(result_tangent_2[0], min_max_interval_tangent_2);
                            PostprocessingObjectsForcePatterns.Add(RhinoDoc.ActiveDoc.Objects.AddLine(line_2, attributes_tangent_2));

                            var line_3 = new Rhino.Geometry.Line(point_1, point_1 + surface_normal * result_normal[0] * ResultScaling);

                            var attributes_normal = PostProcessingUtilities.GetStressPatternObjectAttributes(result_normal[0], min_max_interval_normal);
                            PostprocessingObjectsForcePatterns.Add(RhinoDoc.ActiveDoc.Objects.AddLine(line_3, attributes_normal));
                        }
                    }
                }
                //var attr = new Rhino.DocObjects.ObjectAttributes();
                //attr.Name = "CouplingForces_" + brep_ids.Key[0] + "_" + brep_ids.Key[1];
                //attr.LayerIndex = GetLayerIndex("CouplingForces", System.Drawing.Color.FromArgb(227, 114, 34));

                string group_name = "CouplingForces_" + brep_ids.Key[0] + "_" + brep_ids.Key[1];
                int index = RhinoDoc.ActiveDoc.Groups.Add(group_name, PostprocessingObjectsForcePatterns);
            }
        }

        public void VisualizeStressPatterns(
            bool ShowForcePatterns,
            double ResultScaling,
            string ResultType,
            bool ShowPrincipal,
            bool Deformed)
        {
            PostprocessingObjectsForcePatterns = DeleteGeometries(PostprocessingObjectsForcePatterns);

            if (ShowForcePatterns && (mUpdateStressPatterns || CheckMissingGeometry(PostprocessingObjectsForcePatterns)))
            {
                var stress_result_info = ResultList.Find(r => r.LoadCaseNumber == mCurrentResultInfo.LoadCaseNumber &&
                                                              r.ResultType == ResultType);

                double current_min = (ShowPrincipal)
                    ? stress_result_info.Results.Min(p => PostProcessingUtilities.GetVonMises(p.Value))
                    : stress_result_info.Results.Min(p => p.Value.Min());
                double current_max = (ShowPrincipal)
                    ? stress_result_info.Results.Max(p => PostProcessingUtilities.GetVonMises(p.Value))
                    : stress_result_info.Results.Max(p => p.Value.Max());

                // To avoid numerical turbulances in the solution
                if (Math.Abs(current_max - current_min) < 0.001)
                {
                    current_max += 0.001;
                    current_min -= 0.001;
                }
                Interval min_max_interval = new Interval(current_min, current_max);

                foreach (var patch in mEvaluationPointList)
                {
                    BrepFace this_brep_face = GetGeometry(patch.Key, Deformed) as BrepFace;
                    if (this_brep_face != null)
                    {
                        foreach (var evaluation_point in patch.Value)
                        {
                            var result = stress_result_info.Results[evaluation_point.Key];

                            var u = evaluation_point.Value[0];
                            var v = evaluation_point.Value[1];

                            this_brep_face.Evaluate(u, v, 1, out Point3d point_1, out Vector3d[] derivatives_1);

                            var surface_normal = Vector3d.CrossProduct(derivatives_1[0], derivatives_1[1]);

                            if (!ShowPrincipal)
                            {
                                var direction_2_vector = derivatives_1[0];
                                direction_2_vector.Rotate(Math.PI / 2, surface_normal);

                                var line_1 = new Rhino.Geometry.Line(point_1, point_1 + derivatives_1[0] * result[0] * ResultScaling);
                                var line_2 = new Rhino.Geometry.Line(point_1, point_1 + direction_2_vector * result[1] * ResultScaling);

                                var attributes_1 = PostProcessingUtilities.GetStressPatternObjectAttributes(result[0], min_max_interval);
                                PostprocessingObjectsForcePatterns.Add(RhinoDoc.ActiveDoc.Objects.AddLine(line_1, attributes_1));

                                var attributes_2 = PostProcessingUtilities.GetStressPatternObjectAttributes(result[1], min_max_interval);
                                PostprocessingObjectsForcePatterns.Add(RhinoDoc.ActiveDoc.Objects.AddLine(line_2, attributes_2));
                            }
                            else
                            {
                                double theta = Math.Atan(result[2] / (result[0] - result[1])) / 2;
                                double sigma_max = ((result[0] + result[1]) / 2) + Math.Sqrt(Math.Pow((result[0] - result[1]) / 2, 2) + Math.Pow(result[2], 2));
                                double sigma_min = ((result[0] + result[1]) / 2) - Math.Sqrt(Math.Pow((result[0] - result[1]) / 2, 2) + Math.Pow(result[2], 2));

                                var direction_1_vector = derivatives_1[0];
                                var direction_2_vector = derivatives_1[0];
                                direction_1_vector.Rotate(theta, surface_normal);
                                direction_2_vector.Rotate(theta + Math.PI / 2, surface_normal);

                                var line_1 = new Rhino.Geometry.Line(point_1, point_1 + direction_1_vector * sigma_max * ResultScaling);
                                var line_2 = new Rhino.Geometry.Line(point_1, point_1 + direction_2_vector * sigma_min * ResultScaling);

                                var attributes_1 = PostProcessingUtilities.GetStressPatternObjectAttributes(sigma_max, min_max_interval);
                                PostprocessingObjectsForcePatterns.Add(RhinoDoc.ActiveDoc.Objects.AddLine(line_1, attributes_1));

                                var attributes_2 = PostProcessingUtilities.GetStressPatternObjectAttributes(sigma_min, min_max_interval);
                                PostprocessingObjectsForcePatterns.Add(RhinoDoc.ActiveDoc.Objects.AddLine(line_2, attributes_2));
                            }

                        }
                    }

                    string group_name = "Points_" + patch.Key;
                    int index = RhinoDoc.ActiveDoc.Groups.Add(group_name, PostprocessingObjectsForcePatterns);
                }
            }
        }

        public GeometryBase GetGeometry(int BrepId, bool Deformed)
        {
            if (Deformed)
            {
                foreach (var brep in BrepId_Breps_deformed)
                {
                    foreach (var brep_face in brep.Value.Faces)
                    {
                        var ud = UserData.UserDataUtilities.GetOrCreateUserDataSurface(brep_face);
                        if (ud.BrepId == BrepId)
                        {
                            return brep_face;
                        }
                    }
                }
            }
            foreach (var brep in BrepId_Breps)
            {
                foreach (var brep_face in brep.Value.Faces)
                {
                    var ud = UserData.UserDataUtilities.GetOrCreateUserDataSurface(brep_face);
                    if (ud.BrepId == BrepId)
                    {
                        return brep_face;
                    }
                }
            }
            return null;
        }
    }
}
