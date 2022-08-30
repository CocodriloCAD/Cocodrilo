using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PostProcessing
{
    public class PostProcessingSurfaceQuadraturePointResults_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PostProcessingResults2D class.
        /// </summary>
        public PostProcessingSurfaceQuadraturePointResults_GH()
          : base("PostProcessingSurfaceQuadraturePointResults", "SurfaceQpResults",
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
            pManager.AddNumberParameter("QP Results", "R", "Results at Quadrature Point", GH_ParamAccess.tree);
            pManager.AddNumberParameter("Min Max", "M", "Min and Max values of selected result type.", GH_ParamAccess.list);
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

            SetStepSlider(result_steps, ref StepIndex);
            var result_types = ThisPostProcessing.ResultList.Where(item => item.NodeOrGauss == "OnGaussPoints").Select(item => item.ResultType).Distinct().ToList();

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
            foreach (var patch in ThisPostProcessing.mEvaluationPointList)
            {
                Grasshopper.Kernel.Data.GH_Path path = new Grasshopper.Kernel.Data.GH_Path(patch.Key);

                foreach (var evaluation_point in patch.Value)
                {
                    if (this_result_info.Results.ContainsKey(evaluation_point.Key))
                    {
                        if (this_result_info.Results.First().Value.Length <= ResultDirectionIndex)
                        {
                            if (this_result_info.ResultType == "\"DISPLACEMENT\"")
                            {
                                result_tree.Add(Cocodrilo.PostProcessing.PostProcessingUtilities.GetArrayLength(this_result_info.Results[evaluation_point.Key]), path);
                            }
                            else
                            {
                                result_tree.Add(Cocodrilo.PostProcessing.PostProcessingUtilities.GetVonMises(this_result_info.Results[evaluation_point.Key]), path);
                            }
                        }
                        else
                        {
                            result_tree.Add(this_result_info.Results[evaluation_point.Key][ResultDirectionIndex], path);
                        }
                    }
                }
            }

            DA.SetDataTree(0, result_tree);

            var min_max = ThisPostProcessing.GetMinMax(this_result_info, ResultDirectionIndex);
            DA.SetDataList(1, min_max);
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
            get { return new Guid("342663B9-620E-45AA-96EA-1340B681CFE7"); }
        }
    }
}