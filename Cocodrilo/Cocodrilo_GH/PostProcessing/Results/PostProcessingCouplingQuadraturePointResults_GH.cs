using System;
using System.Collections.Generic;
using System.Linq;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PostProcessing
{
    public class PostProcessingCouplingQuadraturePointResults_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PostProcessingResults2D class.
        /// </summary>
        public PostProcessingCouplingQuadraturePointResults_GH()
          : base("PostProcessingCouplingQuadraturePointResults", "CouplingQpResults",
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
            pManager.AddPointParameter("Coupling Quadrature Points", "Q", "Coupling quadrature points on the master geometry only.", GH_ParamAccess.list);
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
            foreach (var evaluation_point in ThisPostProcessing.mCouplingEvaluationPointList)
            {
                int evaluation_point_index = evaluation_point.Value[0].Key;

                Grasshopper.Kernel.Data.GH_Path path = new Grasshopper.Kernel.Data.GH_Path(evaluation_point.Key[0]);


                if (this_result_info.Results.ContainsKey(evaluation_point_index))
                {
                    if (this_result_info.Results.First().Value.Length <= ResultDirectionIndex)
                    {
                        if (this_result_info.ResultType == "\"DISPLACEMENT\"")
                        {
                            result_tree.Add(Cocodrilo.PostProcessing.PostProcessingUtilities.GetArrayLength(this_result_info.Results[evaluation_point_index]), path);
                        }
                        else
                        {
                            result_tree.Add(Cocodrilo.PostProcessing.PostProcessingUtilities.GetVonMises(this_result_info.Results[evaluation_point_index]), path);
                        }
                    }
                    else
                    {
                        result_tree.Add(this_result_info.Results[evaluation_point_index][ResultDirectionIndex], path);
                    }
                }
            }

            DA.SetDataTree(0, result_tree);

            var min_max = ThisPostProcessing.GetMinMax(this_result_info, ResultDirectionIndex);
            DA.SetDataList(1, min_max);

            bool Deformed = false;

            List<Point3d> list_of_coupling_points = new List<Point3d>();
            foreach (var brep_ids in ThisPostProcessing.mCouplingEvaluationPointList)
            {
                List<Point3d> evaluation_point_list = new List<Point3d>();
                List<Line> line_list = new List<Line>();

                var geometry_1 = ThisPostProcessing.GetGeometry(brep_ids.Key[0], Deformed);
                if (geometry_1 == null) continue;
                if (geometry_1 is BrepFace)
                {
                    var nurbs_surface_1 = (geometry_1 as BrepFace).ToNurbsSurface();
                    var brep = (geometry_1 as BrepFace).Brep;

                    foreach (var evaluation_point in brep_ids.Value)
                    {
                        nurbs_surface_1.Evaluate(evaluation_point.Value[0], evaluation_point.Value[1], 0, out Point3d point_1, out _);
                        list_of_coupling_points.Add(point_1);
                    }
                }
            }

            DA.SetDataList(2, list_of_coupling_points);
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
            get { return new Guid("2367EA86-349F-4C1B-9C45-FF812A09382D"); }
        }
    }
}