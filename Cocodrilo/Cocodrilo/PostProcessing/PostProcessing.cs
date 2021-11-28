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

        public static Dictionary<int, List<KeyValuePair<int, List<double>>>> s_EvaluationPointList;
        public Dictionary<int[], List<KeyValuePair<int, List<double>>>> mCouplingEvaluationPointList;

        //public Dictionary<int, NurbsSurface> BrepId_NurbsSurface = new Dictionary<int, NurbsSurface>();

        private Dictionary<int, Brep> BrepId_Breps;

        public static Dictionary<int, List<KeyValuePair<int, List<double>>>> s_BrepId_NodeId_Coordinates = new Dictionary<int, List<KeyValuePair<int, List<double>>>>();

        /// Variables to keep track through the
        public List<Guid> mPostprocessingObjects { get; set; }
        public List<Guid> mPostprocessingInitialObjects { get; set; }
        public List<Guid> PostprocessingObjectsGaussPoints { get; set; }
        public List<Guid> PostprocessingObjectsCouplingPoints { get; set; }
        public List<Guid> PostprocessingObjects1DResults { get; set; }
        public List<Guid> PostprocessingObjectsForcePatterns { get; set; }

        /// Variables to detect the selected object
        public List<string> CurrentDistinctResultTypes { get; set; }
        public List<string> DistinctLoadCaseTypes { get; set; }
        public List<int> DistinctLoadCaseNumbers { get; set; }
        public static RESULT_INFO s_CurrentResultInfo;
        public RESULT_INFO ResultInfo(string LoadCaseType, int LoadCaseNumber, string ResultType) => ResultList.Find(p => p.LoadCaseType == LoadCaseType &&
                                                       p.LoadCaseNumber == LoadCaseNumber &&
                                                       p.ResultType == ResultType);
        public static int s_SelectedCurrentResultDirectionIndex = 0;

        public double[] CurrentMinMax = new double[] { 0, 1 };
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
        private bool mRegisterVisualanalysisMode = true;

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
                int post_processing_index = RhinoDoc.ActiveDoc.Layers.Add("PostProcessing", System.Drawing.Color.Black);
                layer_post_processing = RhinoDoc.ActiveDoc.Layers[post_processing_index];
            }
            layer_post_processing.IsLocked = true;

            ReadData(path);
            VisualizeInitialGeometry(true);

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
                    s_BrepId_NodeId_Coordinates = new Dictionary<int, List<KeyValuePair<int, List<double>>>>();
                    if (System.IO.File.Exists(path))
                    {
                        PostProcessingImportUtilities.LoadRhinoGeometries(
                            path, ref BrepId_Breps, ref s_BrepId_NodeId_Coordinates);
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
                Parallel.ForEach(result_files, file =>
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
                });


                string[] evaluation_point_files = System.IO.Directory.GetFiles(analysis_folder, analysis_name + "*_integrationdomain.json");
                s_EvaluationPointList = new Dictionary<int, List<KeyValuePair<int, List<double>>>>();
                mCouplingEvaluationPointList = new Dictionary<int[], List<KeyValuePair<int, List<double>>>>();

                foreach (var file in evaluation_point_files)
                {
                    if (File.Exists(file))
                    {
                        try
                        {
                            PostProcessingImportUtilities.LoadEvaluationPoints(
                                file, ref s_EvaluationPointList, ref mCouplingEvaluationPointList);
                        }
                        catch
                        {
                            RhinoApp.WriteLine("ERROR: could not read evaluation points from file: " + file);
                        }
                    }
                }
            }

            if (!ResultList.Exists(p => p.LoadCaseNumber == 0))
            {
                var Results = new Dictionary<int, double[]>();
                Results.Add(0, new double[] { 0, 0, 0 });
                var default_result_info = new RESULT_INFO() { LoadCaseType = "Load Case", LoadCaseNumber = 0, ResultType = "\"DISPLACEMENT\"", NodeOrGauss = "\"OnNodes\"", VectorOrScalar = "Vector", Results = Results };
                ResultList.Add(default_result_info);

                /// Set initial values.
                s_CurrentResultInfo = default_result_info;
            }
            var load_cases_per_time_step = ResultList.Where(p =>
                    p.LoadCaseNumber == s_CurrentResultInfo.LoadCaseNumber
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
                var geometry_layer = RhinoDoc.ActiveDoc.Layers.FindName(LayerName);
                if (geometry_layer == null)
                {
                    attr.LayerIndex = RhinoDoc.ActiveDoc.Layers.Add(LayerName, LayerColor);
                    geometry_layer = RhinoDoc.ActiveDoc.Layers[attr.LayerIndex];
                    if (RhinoDoc.ActiveDoc.Layers.FindName("PostProcessing") != null)
                    {
                        geometry_layer.ParentLayerId = RhinoDoc.ActiveDoc.Layers.FindName("PostProcessing").Id;
                    }
                    else
                    {
                        int parent_layer_index = RhinoDoc.ActiveDoc.Layers.Add("PostProcessing", System.Drawing.Color.Black);
                        geometry_layer.ParentLayerId = RhinoDoc.ActiveDoc.Layers[parent_layer_index].Id;
                    }
                }
                else
                    attr.LayerIndex = geometry_layer.Index;

                ObjectsList.Add(RhinoDoc.ActiveDoc.Objects.AddBrep(brep.Value, attr));
            }
        }

        public void RealMinMax()
        {
            Interval tmp_min_max = new Interval(100000, -100000);
            int result_index = s_SelectedCurrentResultDirectionIndex;



            if(s_CurrentResultInfo.NodeOrGauss == "OnNodes")
            {
                // Nodal values
                for (int i = 0; i < mPostprocessingObjects.Count; i++)
                {
                    var objc = RhinoDoc.ActiveDoc.Objects.Find(mPostprocessingObjects[i]);
                    (mVisualizationMode as ResultPlotVisualAnalysisMode).RealMinMax(objc, ref tmp_min_max);
                }
            }
            else
            {
                // Todo!! Past des?
                // Gauss Poitn values
                var current_results = PostProcessing.s_CurrentResultInfo.Results;
                foreach (var brep_id in PostProcessing.s_EvaluationPointList.Keys)
                {
                    var brep_evaluation_point_list = PostProcessing.s_EvaluationPointList[brep_id];
                    for (int point_index = 0; point_index < brep_evaluation_point_list.Count; ++point_index)
                    {
                        var evaluation_point_id = brep_evaluation_point_list[point_index].Key;
                        var current_result = current_results[evaluation_point_id];
                        var value = (result_index >= current_result.Length)
                                        ? PostProcessingUtilities.GetVonMises(current_result)
                                        : current_result[result_index];

                        if (value < tmp_min_max[0])
                            tmp_min_max[0] = value;
                        if (value > tmp_min_max[1])
                            tmp_min_max[1] = value;
                    }
                }
            }
            s_MinMax = tmp_min_max;
        }

        public void UpdateCurrentResults(
            string LoadCaseType, int LoadCaseNumber, string ResultType)
        {
            bool update_load_case_numbers = (s_CurrentResultInfo.LoadCaseNumber != LoadCaseNumber);

            if (ResultType == "")
            {
                ResultType = s_CurrentResultInfo.ResultType;
            }

            LoadCaseNumber = Math.Max(LoadCaseNumber, 1);

            s_CurrentResultInfo = ResultList.Find(p => p.LoadCaseType == LoadCaseType &&
                                                       p.LoadCaseNumber == LoadCaseNumber &&
                                                       p.ResultType == ResultType);

            if (update_load_case_numbers)
            {
                var load_cases_per_time_step = ResultList.Where(p =>
                        p.LoadCaseNumber == s_CurrentResultInfo.LoadCaseNumber
                    && (p.NodeOrGauss == "OnNodes" || p.NodeOrGauss == "OnGaussPoints")).ToList();
                CurrentDistinctResultTypes = load_cases_per_time_step.Select(p => p.ResultType).Distinct().ToList();
                // Stress pattern updates once a new load case is considered.
                mUpdateStressPatterns = true;
            }

            UpdateCurrentMinMax();

            if (s_CurrentResultInfo.ResultType == "\"DISPLACEMENT\"")
            {
                mUpdateGeometry = true;
                mUpdateGaussPoints = true;
                mUpdateCouplingPoints = true;
            }
            mUpdateResultPlot = true;
        }

        public void UpdateCurrentMinMax()
        {
            if (s_CurrentResultInfo.Results == null)
            {
                CurrentMinMax[0] = 0;
                CurrentMinMax[1] = 1;
            }
            else
            {
                if (s_CurrentResultInfo.Results.First().Value.Length <= s_SelectedCurrentResultDirectionIndex)
                {
                    if (s_CurrentResultInfo.ResultType == "\"DISPLACEMENT\"")
                    {
                        CurrentMinMax[0] = s_CurrentResultInfo.Results.Min(p => PostProcessingUtilities.GetArrayLength(p.Value));
                        CurrentMinMax[1] = s_CurrentResultInfo.Results.Max(p => PostProcessingUtilities.GetArrayLength(p.Value));
                    }
                    else
                    {
                        CurrentMinMax[0] = s_CurrentResultInfo.Results.Min(p => PostProcessingUtilities.GetVonMises(p.Value));
                        CurrentMinMax[1] = s_CurrentResultInfo.Results.Max(p => PostProcessingUtilities.GetVonMises(p.Value));
                    }
                }
                else
                {
                    CurrentMinMax[0] = s_CurrentResultInfo.Results.Min(p => p.Value[s_SelectedCurrentResultDirectionIndex]);
                    CurrentMinMax[1] = s_CurrentResultInfo.Results.Max(p => p.Value[s_SelectedCurrentResultDirectionIndex]);
                }
            }
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
            VisualizeInitialGeometry(ShowUndeformed);

            VisualizeGeometry(scalingFactor, flyingNodeLimit, ShowResultColorPlot);
            VisualizeEvaluationPoints(ShowEvaluationPoints);
            VisualizeCouplingPoints(ShowCouplingEvaluationPoints);

            if (ShowCauchyStresses)
                VisualizeStressPatterns(true, ResultScaling, "\"CAUCHY_STRESS\"", ShowPrincipalStresses);
            if (ShowPK2Stresses)
                VisualizeStressPatterns(true, ResultScaling, "\"PK2_STRESS\"", ShowPrincipalStresses);

            RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public void ClearPostProcessing()
        {
            ResultList.Clear();
            var Results = new Dictionary<int, double[]> ();
            Results.Add(0, new double[] { 0, 0, 0 });
            var default_result_info = new RESULT_INFO() { ResultType = "", LoadCaseNumber = 0, LoadCaseType = "", NodeOrGauss = "", VectorOrScalar = "", Results = Results };
            ResultList.Add(default_result_info);
            s_CurrentResultInfo = default_result_info;
            DistinctLoadCaseTypes = ResultList.Select(p => p.LoadCaseType).Distinct().ToList();
            var load_cases_per_time_step = ResultList.Where(p => p.LoadCaseNumber == s_CurrentResultInfo.LoadCaseNumber).ToList();
            CurrentDistinctResultTypes = load_cases_per_time_step.Select(p => p.ResultType).Distinct().ToList();
            DistinctLoadCaseNumbers = ResultList.Select(p => p.LoadCaseNumber).Distinct().ToList();
            CurrentMinMax = new double[] { 0, 1 };

            s_EvaluationPointList?.Clear();
            mCouplingEvaluationPointList?.Clear();

            mPostprocessingObjects.ForEach(p => RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(p), false, true));
            mPostprocessingObjects.Clear();
            mPostprocessingInitialObjects.ForEach(p => RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(p), false, true));
            mPostprocessingInitialObjects.Clear();
            PostprocessingObjectsGaussPoints.ForEach(p => RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(p), false, true));
            PostprocessingObjectsGaussPoints.Clear();
            PostprocessingObjectsCouplingPoints.ForEach(p => RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(p), false, true));
            PostprocessingObjectsCouplingPoints.Clear();
            PostprocessingObjects1DResults.ForEach(p => RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(p), false, true));
            PostprocessingObjects1DResults.Clear();
            PostprocessingObjectsForcePatterns.ForEach(p => RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(p), false, true));
            PostprocessingObjectsForcePatterns.Clear();

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
            s_EvaluationPointList.Clear();
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
        public void VisualizeInitialGeometry(
            bool ShowUndeformed)
        {
            if (ShowUndeformed)
            {
                mPostprocessingInitialObjects.ForEach(p => RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(p), false, true));
                mPostprocessingInitialObjects.Clear();

                DrawGeometries(BrepId_Breps, mPostprocessingInitialObjects, System.Drawing.Color.FromArgb(150, 150, 150), "GeometriesInitial");
            }
            else
            {
                mPostprocessingInitialObjects.ForEach(p => RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(p), false, true));
                mPostprocessingInitialObjects.Clear();
            }
        }

        public void VisualizeGeometry(
            double ScalingFactor,
            double FlyingNodeLimit,
            bool ShowResultColorPlot)
        {
            bool check_geometries_missing = false;
            if (!mUpdateGeometry)
            {
                /// Checks if all geometries are apparent. If some are missing the whole viewport is being rebuilt.
                for (int i = 0; i < mPostprocessingObjects.Count; i++)
                {
                    var objc = RhinoDoc.ActiveDoc.Objects.Find(mPostprocessingObjects[i]);
                    if (objc == null)
                    {
                        check_geometries_missing = true;
                        break;
                    }
                }
            }

            if (mUpdateGeometry || check_geometries_missing)
            {
                for (int i = 0; i < mPostprocessingObjects.Count; i++)
                {
                    var check = RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(mPostprocessingObjects[i]), false, true);
                }
                mPostprocessingObjects.Clear();


                var displacement_results = ResultList.Find(r => r.LoadCaseNumber == s_CurrentResultInfo.LoadCaseNumber &&
                                                                r.ResultType == "\"DISPLACEMENT\"").Results;

                var deformed_breps = new Dictionary<int, Brep>();
                foreach (var brep in BrepId_Breps)
                //for (int i = 0; i < Breps.Count; i++)
                {
                    //    brep.Value.EnsurePrivateCopy();
                    //    //Brep new_brep = new Brep();
                    //    //new_brep = brep.Value;
                    //    //new_brep.EnsurePrivateCopy();
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
                            ud = ud_old;
                        }

                        if (s_BrepId_NodeId_Coordinates.ContainsKey(ud.BrepId))
                        {
                            var surface = new_brep_face.UnderlyingSurface();
                            var nurbs_surface = surface.ToNurbsSurface();

                            int u = 0;
                            int v = 0;
                            Brep this_updated_face_brep = new Brep();
                            foreach (var control_point in s_BrepId_NodeId_Coordinates[ud.BrepId])
                            {
                                var ControlPointID = v * nurbs_surface.Points.CountU + u;

                                var nodal_displacements = displacement_results.ContainsKey(control_point.Key)
                                    ? displacement_results[control_point.Key]
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
                            new_brep_face.RebuildEdges(0.001, false, true);

                            new_brep.CullUnusedSurfaces();
                            //new_brep.Repair(0.0001);
                        }
                        deformed_breps.Add(ud.BrepId, new_brep);
                    }
                }

                DrawGeometries(deformed_breps, mPostprocessingObjects, System.Drawing.Color.FromArgb(90, 90, 90), "Geometries");
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
            if (mUpdateResultPlot || check_geometries_missing || mUpdateVisualizationMesh)
            {
                // Enables that color plot is being updated.
                s_UpdateResultPlotInAnalysisMode = true;

                if (mRegisterVisualanalysisMode)
                {
                    // Colorplot
                    mVisualizationMode = Rhino.Display.VisualAnalysisMode.Register(typeof(ResultPlotVisualAnalysisMode));
                }
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
            mRegisterVisualanalysisMode = false;

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

        public void VisualizeEvaluationPoints(bool show_evaluation_points)
        {
            bool check_geometries_missing = false;
            if (!mUpdateGaussPoints) // Check if points are missing in the active doc
            {
                for (int i = 0; i < PostprocessingObjectsGaussPoints.Count; i++)
                {
                    var objc = RhinoDoc.ActiveDoc.Objects.Find(PostprocessingObjectsGaussPoints[i]);
                    if (objc == null)
                    {
                        check_geometries_missing = true;
                        break;
                    }
                }
            }
            if (mUpdateGaussPoints || check_geometries_missing)
            {
                for (int i = 0; i < PostprocessingObjectsGaussPoints.Count; i++)
                {
                    RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(PostprocessingObjectsGaussPoints[i]), false, true);
                }
                PostprocessingObjectsGaussPoints.Clear();

                if (show_evaluation_points)
                {
                    // Get respective layer
                    var gauss_points_layer = RhinoDoc.ActiveDoc.Layers.FindName("EvaluationPoints");
                    int gauss_points_layer_index;
                    if (gauss_points_layer == null)
                    {
                        var gauss_points_layer_new = new Rhino.DocObjects.Layer();
                        gauss_points_layer_new.Name = "EvaluationPoints";
                        gauss_points_layer_new.ParentLayerId = RhinoDoc.ActiveDoc.Layers.FindName("PostProcessing").Id;
                        gauss_points_layer_new.Color = System.Drawing.Color.FromArgb(0, 101, 189);
                        gauss_points_layer_index = RhinoDoc.ActiveDoc.Layers.Add(gauss_points_layer_new);
                    }
                    else
                        gauss_points_layer_index = gauss_points_layer.Index;

                    foreach (var brep_id_evaluation_points in s_EvaluationPointList)
                    {
                        List<Point3d> gp_list = new List<Point3d>();

                        var geometry = GetGeometry(brep_id_evaluation_points.Key);
                        if (geometry == null) continue;
                        if (geometry is BrepFace)
                        {
                            var gp_pt = new Point3d();

                            var nurbs_surface = (geometry as BrepFace).ToNurbsSurface();
                            foreach (var evaluation_point in brep_id_evaluation_points.Value)
                            {
                                nurbs_surface.Evaluate(evaluation_point.Value[0], evaluation_point.Value[1], 0, out gp_pt, out _);
                                gp_list.Add(gp_pt);
                            }
                        }

                        // Group points
                        var attr = new Rhino.DocObjects.ObjectAttributes();
                        attr.Name = "Points_" + brep_id_evaluation_points.Key;
                        attr.LayerIndex = gauss_points_layer_index;
                        if (gp_list != null)
                        {
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
                }
                else
                {
                    RhinoDoc.ActiveDoc.Views.Redraw();
                }
            }
            mUpdateGaussPoints = false;
        }

        public void VisualizeCouplingPoints(bool show_coupling_evaluation_points)
        {
            bool check_geometries_missing = false;
            if (!mUpdateCouplingPoints)
            {
                for (int i = 0; i < PostprocessingObjectsCouplingPoints.Count; i++)
                {
                    var objc = RhinoDoc.ActiveDoc.Objects.Find(PostprocessingObjectsCouplingPoints[i]);
                    if (objc == null)
                    {
                        check_geometries_missing = true;
                        break;
                    }
                }
            }
            if (mUpdateCouplingPoints || check_geometries_missing)
            {
                for (int i = 0; i < PostprocessingObjectsCouplingPoints.Count; i++)
                {
                    RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(PostprocessingObjectsCouplingPoints[i]), false, true);
                }
                PostprocessingObjectsCouplingPoints.Clear();

                if (show_coupling_evaluation_points)
                {
                    var layer_gp = RhinoDoc.ActiveDoc.Layers.FindName("CouplingPoints");
                    int layerIndex;
                    if (layer_gp == null)
                    {
                        var layer_gp_new = new Rhino.DocObjects.Layer();
                        layer_gp_new.Name = "CouplingPoints";
                        layer_gp_new.ParentLayerId = RhinoDoc.ActiveDoc.Layers.FindName("PostProcessing").Id;
                        layer_gp_new.Color = System.Drawing.Color.FromArgb(227, 114, 34);
                        layerIndex = RhinoDoc.ActiveDoc.Layers.Add(layer_gp_new);
                    }
                    else
                        layerIndex = layer_gp.Index;

                    foreach (var brep_ids in mCouplingEvaluationPointList)
                    {
                        List<Point3d> evaluation_point_list = new List<Point3d>();
                        List<Line> line_list = new List<Line>();

                        var geometry_1 = GetGeometry(brep_ids.Key[0]);
                        var geometry_2 = GetGeometry(brep_ids.Key[1]);
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
                        attr.LayerIndex = layerIndex;

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
        public void VisualizeContactForces(
            bool ShowForcePatterns,
            double ResultScaling,
            string ResultType,
            bool ShowPrincipal)
        {
            bool check_geometries_missing = false;
            if (!mUpdateStressPatterns)
            {
                for (int i = 0; i < PostprocessingObjectsForcePatterns.Count; i++)
                {
                    var objc = RhinoDoc.ActiveDoc.Objects.Find(PostprocessingObjectsForcePatterns[i]);
                    if (objc == null)
                    {
                        check_geometries_missing = true;
                        break;
                    }
                }
            }
            if (mUpdateStressPatterns || check_geometries_missing)
            {
                for (int i = 0; i < PostprocessingObjectsForcePatterns.Count; i++)
                {
                    RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(PostprocessingObjectsForcePatterns[i]), false, true);
                }
                PostprocessingObjectsForcePatterns.Clear();
                if (ShowForcePatterns)
                {
                    var layer_fp = RhinoDoc.ActiveDoc.Layers.FindName("ForcePatterns");
                    int layerIndex;
                    if (layer_fp == null)
                    {
                        var layer_fp_new = new Rhino.DocObjects.Layer();
                        layer_fp_new.Name = "ForcePatterns";
                        layer_fp_new.ParentLayerId = RhinoDoc.ActiveDoc.Layers.FindName("PostProcessing").Id;
                        layer_fp_new.Color = System.Drawing.Color.FromArgb(0, 0, 200);
                        layerIndex = RhinoDoc.ActiveDoc.Layers.Add(layer_fp_new);
                    }
                    else
                        layerIndex = layer_fp.Index;


                }
            }
        }

        public void VisualizeStressPatterns(
            bool ShowForcePatterns,
            double ResultScaling,
            string ResultType,
            bool ShowPrincipal)
        {
            bool check_geometries_missing = false;
            if (!mUpdateStressPatterns)
            {
                for (int i = 0; i < PostprocessingObjectsForcePatterns.Count; i++)
                {
                    var objc = RhinoDoc.ActiveDoc.Objects.Find(PostprocessingObjectsForcePatterns[i]);
                    if (objc == null)
                    {
                        check_geometries_missing = true;
                        break;
                    }
                }
            }
            if (mUpdateStressPatterns || check_geometries_missing)
            {
                for (int i = 0; i < PostprocessingObjectsForcePatterns.Count; i++)
                {
                    RhinoDoc.ActiveDoc.Objects.Delete(RhinoDoc.ActiveDoc.Objects.Find(PostprocessingObjectsForcePatterns[i]), false, true);
                }
                PostprocessingObjectsForcePatterns.Clear();
                if (ShowForcePatterns)
                {
                    var layer_fp = RhinoDoc.ActiveDoc.Layers.FindName("ForcePatterns");
                    int layerIndex;
                    if (layer_fp == null)
                    {
                        var layer_fp_new = new Rhino.DocObjects.Layer();
                        layer_fp_new.Name = "ForcePatterns";
                        layer_fp_new.ParentLayerId = RhinoDoc.ActiveDoc.Layers.FindName("PostProcessing").Id;
                        layer_fp_new.Color = System.Drawing.Color.FromArgb(0, 0, 200);
                        layerIndex = RhinoDoc.ActiveDoc.Layers.Add(layer_fp_new);
                    }
                    else
                        layerIndex = layer_fp.Index;

                    var stress_result_info = ResultList.Find(r => r.LoadCaseNumber == s_CurrentResultInfo.LoadCaseNumber &&
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

                    foreach (var patch in s_EvaluationPointList)
                    {
                        BrepFace this_brep_face = null;
                        foreach (var brep in BrepId_Breps)
                        {
                            foreach (var brep_face in brep.Value.Faces)
                            {
                                var ud = UserData.UserDataUtilities.GetOrCreateUserDataSurface(brep_face);
                                if (ud.BrepId == patch.Key)
                                {
                                    this_brep_face = brep_face;
                                    continue;
                                }
                            }
                        }
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
        }
        GeometryBase GetGeometry(int BrepId)
        {
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

        private bool IndexClosestPoint(Point3d unset, List<int> pos_gp_ids, Surface srf, out int ev_id)
        {
            // Function Core is copied from Grasshopper Source Code

            int index1 = -1;
            //double num1 = double.MaxValue;
            //int num2 = 0;
            //int num3 = checked(pos_gp_ids.Count - 1);
            //int index2 = num2;
            //while (index2 <= num3)
            //{
            //    //if (pos_gp_ids[index2] != null)
            //    {
            //        var evpt = Example.GeoRhino.EVAL_POINT_LIST[pos_gp_ids[index2]];
            //        double num4 = unset.DistanceTo(srf.PointAt((double)evpt[1], (double)evpt[2]));
            //        if (num4 < num1)
            //        {
            //            num1 = num4;
            //            index1 = pos_gp_ids[index2];
            //        }
            //    }
            //    checked { ++index2; }
            //}

            ev_id = index1;
            //if (index1 < 0)
            //{
            //    return false;
            //}
            //else
            //{
            return true;
            //}
        }
    }
}
