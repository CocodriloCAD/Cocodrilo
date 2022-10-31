using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PostProcessing
{
    public class PostProcessingCouplingQuadraturePointResults2_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PostProcessingResults2D class.
        /// </summary>
        public PostProcessingCouplingQuadraturePointResults2_GH()
          : base("PostProcessingCouplingQuadraturePointResults2", "CouplingQpResults2",
              "Extracts the quadrature point results of the solution.",
              "Cocodrilo", "PostProcessing")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("PostProcessing", "P", "Post Processing", GH_ParamAccess.item);
            pManager.AddIntegerParameter("Step", "S", "Select Result Step, requires Slider Component", GH_ParamAccess.item, -1);
            pManager[1].Optional = true;
            pManager.AddIntegerParameter("ResultType", "R", "Select Type of Result, requires ValueList Component", GH_ParamAccess.item, 0);
            pManager[2].Optional = true;
            pManager.AddIntegerParameter("ResultIndex", "I", "Select Index of Result, requires ValueList Component", GH_ParamAccess.item, 0);
            pManager[3].Optional = true;
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddNumberParameter("QP Results", "R", "Results at Coupling Quadrature Point", GH_ParamAccess.tree);
            pManager.AddNumberParameter("Min Max", "M", "Min and Max values of selected result type.", GH_ParamAccess.list);
            pManager.AddPointParameter("Coupling Quadrature Points", "Q", "Coupling quadrature points on the master geometry only.", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Cocodrilo.PostProcessing.PostProcessing ThisPostProcessing = null;
            if (!DA.GetData(0, ref ThisPostProcessing)) return;

            var result_steps = ThisPostProcessing.DistinctLoadCaseNumbers;

            int StepIndex = 0;
            if (!DA.GetData(1, ref StepIndex)) return;

            // sets default step index to last entry.
            if (StepIndex == -1) StepIndex = result_steps.Count - 1;
            bool check = ThisPostProcessing.ResultList[0].ResultType.Contains("PENALTY_FACTOR_TANGENT_1");
            SetStepSlider(result_steps, ref StepIndex);
            var result_types = ThisPostProcessing.ResultList.Where(
                item => item.NodeOrGauss == "OnGaussPoints"
                     && (item.ResultType.Contains("PENALTY_FACTOR_TANGENT_1")
                      || item.ResultType.Contains("PENALTY_FACTOR_TANGENT_2")
                      || item.ResultType.Contains("PENALTY_FACTOR_NORMAL"))).Select(item => item.ResultType).Distinct().ToList();

            SetResultTypes(result_types);

            int ResultTypeIndex = 0;
            if (!DA.GetData(2, ref ResultTypeIndex)) return;
            if (result_types.Count <= ResultTypeIndex) return;

            int ResultDirectionIndex = 0;
            if (!DA.GetData(3, ref ResultDirectionIndex)) return;

            var this_result_info = ThisPostProcessing.ResultList.Find(item => 
                item.LoadCaseNumber == StepIndex && item.ResultType == result_types[ResultTypeIndex]);
            var result_indices = Cocodrilo.PostProcessing.PostProcessingUtilities.GetResultIndices(this_result_info);
            SetResultIndices(result_indices);
            if (ResultDirectionIndex >= result_indices.Count) ResultDirectionIndex = result_indices.Count - 2;

            Grasshopper.DataTree<double> result_tree = new Grasshopper.DataTree<double>();
            //foreach (var patch in ThisPostProcessing.mEvaluationPointList)
            //{
            //    Grasshopper.Kernel.Data.GH_Path path = new Grasshopper.Kernel.Data.GH_Path(patch.Key);

            //    foreach (var evaluation_point in patch.Value)
            //    {
            //        if (this_result_info.Results.ContainsKey(evaluation_point.Key))
            //        {
            //            if (this_result_info.Results.First().Value.Length <= ResultDirectionIndex)
            //            {
            //                if (this_result_info.ResultType == "\"DISPLACEMENT\"")
            //                {
            //                    result_tree.Add(Cocodrilo.PostProcessing.PostProcessingUtilities.GetArrayLength(this_result_info.Results[evaluation_point.Key]), path);
            //                }
            //                else
            //                {
            //                    result_tree.Add(Cocodrilo.PostProcessing.PostProcessingUtilities.GetVonMises(this_result_info.Results[evaluation_point.Key]), path);
            //                }
            //            }
            //            else
            //            {
            //                result_tree.Add(this_result_info.Results[evaluation_point.Key][ResultDirectionIndex], path);
            //            }
            //        }
            //    }
            //}


            var min_max = ThisPostProcessing.GetMinMax(this_result_info, ResultDirectionIndex);
            DA.SetDataList(1, min_max);

            bool Deformed = false;

            Grasshopper.DataTree<Point3d> list_of_coupling_points = new Grasshopper.DataTree<Point3d>();
            //List<Point3d> list_of_coupling_points = new List<Point3d>();
            foreach (var evaluation_point in ThisPostProcessing.mCouplingEvaluationPointList)
            {
                List<Point3d> evaluation_point_list = new List<Point3d>();
                List<Line> line_list = new List<Line>();

                var geometry_1 = ThisPostProcessing.GetGeometry(evaluation_point.Key[0], Deformed);
                var geometry_2 = ThisPostProcessing.GetGeometry(evaluation_point.Key[1], Deformed);
                if (geometry_1 == null) continue;

                Grasshopper.Kernel.Data.GH_Path path_1 = new Grasshopper.Kernel.Data.GH_Path(evaluation_point.Key[0]);
                Grasshopper.Kernel.Data.GH_Path path_2 = new Grasshopper.Kernel.Data.GH_Path(evaluation_point.Key[1]);

                int evaluation_point_index = evaluation_point.Value[0].Key;

                double result = 0.0;
                if (this_result_info.Results.ContainsKey(evaluation_point_index))
                {
                    if (this_result_info.Results.First().Value.Length <= ResultDirectionIndex)
                    {
                        //if (this_result_info.ResultType == "\"DISPLACEMENT\"")
                        //{
                        //    result_tree.Add(Cocodrilo.PostProcessing.PostProcessingUtilities.GetArrayLength(this_result_info.Results[evaluation_point_index]), path);
                        //}
                        //else
                        //{
                        //    result_tree.Add(Cocodrilo.PostProcessing.PostProcessingUtilities.GetVonMises(this_result_info.Results[evaluation_point_index]), path);
                        //}
                    }
                    else
                    {
                        result = this_result_info.Results[evaluation_point_index][ResultDirectionIndex];
                        //result_tree.Add(this_result_info.Results[evaluation_point_index][ResultDirectionIndex], path);
                    }
                }

                if (geometry_1 is BrepFace)
                {
                    var nurbs_surface_1 = (geometry_1 as BrepFace).ToNurbsSurface();
                    var brep = (geometry_1 as BrepFace).Brep;

                    nurbs_surface_1.Evaluate(evaluation_point.Value[0].Value[0], evaluation_point.Value[0].Value[1], 0, out Point3d point_1, out _);
                    list_of_coupling_points.Add(point_1, path_1);
                    result_tree.Add(result, path_1);
                }
                if (geometry_2 is BrepFace)
                {
                    var nurbs_surface_2 = (geometry_2 as BrepFace).ToNurbsSurface();
                    var brep = (geometry_2 as BrepFace).Brep;

                    nurbs_surface_2.Evaluate(evaluation_point.Value[0].Value[2], evaluation_point.Value[0].Value[3], 0, out Point3d point_2, out _);
                    list_of_coupling_points.Add(point_2, path_2);
                    result_tree.Add(result, path_2);
                }
            }

            DA.SetDataTree(0, result_tree);
            DA.SetDataTree(2, list_of_coupling_points);
        }

        private void SetStepSlider(List<int> ResultSteps, ref int StepIndex)
        {
            if (this.Params.Input[1].SourceCount == 1)
            {
                if (this.Params.Input[1].Sources[0] is Grasshopper.Kernel.Special.GH_NumberSlider)
                {
                    var NumberSilder = Params.Input[1].Sources[0] as Grasshopper.Kernel.Special.GH_NumberSlider;
                    NumberSilder.Name = "Step";
                    NumberSilder.ClearData();
                    NumberSilder.Slider.DecimalPlaces = 0;
                    NumberSilder.Slider.Minimum = 0;
                    NumberSilder.Slider.Maximum = ResultSteps.Count - 1;
                    StepIndex = Math.Min(ResultSteps.Count - 1, StepIndex);
                    NumberSilder.SetSliderValue(StepIndex);
                }
            }
        }
        private void SetResultTypes(List<string> ResultTypes)
        {
            if (this.Params.Input[2].SourceCount == 1)
            {
                if (this.Params.Input[2]?.Sources[0] is Grasshopper.Kernel.Special.GH_ValueList)
                {
                    var ValueList = Params.Input[2].Sources[0] as Grasshopper.Kernel.Special.GH_ValueList;
                    ValueList.ListMode = Grasshopper.Kernel.Special.GH_ValueListMode.DropDown;

                    if (ValueList.ListItems.Count != ResultTypes.Count)
                    {
                        ValueList.ListItems.Clear();
                        for (int i = 0; i < ResultTypes.Count; i++)
                        {
                            ValueList.ListItems.Add(new Grasshopper.Kernel.Special.GH_ValueListItem(ResultTypes[i], i.ToString()));
                        }
                    }
                }
            }
        }

        private void SetResultIndices(List<string> ResultIndices)
        {
            if (this.Params.Input[3].SourceCount == 1)
            {
                if (this.Params.Input[3]?.Sources[0] is Grasshopper.Kernel.Special.GH_ValueList)
                {
                    var ValueList = Params.Input[3].Sources[0] as Grasshopper.Kernel.Special.GH_ValueList;
                    ValueList.ListMode = Grasshopper.Kernel.Special.GH_ValueListMode.DropDown;

                    if (ValueList.ListItems.Count != ResultIndices.Count)
                    {
                        ValueList.ListItems.Clear();
                        for (int i = 0; i < ResultIndices.Count; i++)
                        {
                            ValueList.ListItems.Add(new Grasshopper.Kernel.Special.GH_ValueListItem(ResultIndices[i], i.ToString()));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("62BD15CA-58AA-40B7-B5B8-CEC6AC78E69D"); }
        }
    }
}