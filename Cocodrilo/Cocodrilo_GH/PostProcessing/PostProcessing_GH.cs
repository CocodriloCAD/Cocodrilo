using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PostProcessing
{
    public class PostProcessing_GH : GH_Component
    {
        Cocodrilo.PostProcessing.PostProcessing mPostProcessing = null;

        bool mShowResultColorPlot = false;
        bool mEvaluationPoints = false;
        bool mCouplingEvaluationPoints = false;

        string last_path = "";
        public PostProcessing_GH()
          : base("PostProcessing", "PostProcessing",
              "Controls the PostProcessing",
              "Cocodrilo", "PostProcessing")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Path", "Pat", "Path of Analysis", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("PostProcessing", "Pos", "PostProcessing of Apparent Solution", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string path = null;
            if (!DA.GetData(0, ref path)) return;
            string[] files = System.IO.Directory.GetFiles(path, "*_kratos_0.georhino.json");
            if (files.Length > 0)
            {
                if (last_path != files[0])
                {
                    mPostProcessing = new Cocodrilo.PostProcessing.PostProcessing(files[0]);
                    last_path = files[0];
                }
            }

            Cocodrilo.PostProcessing.PostProcessing.s_MinMax[0] = mPostProcessing.CurrentMinMax[0];
            Cocodrilo.PostProcessing.PostProcessing.s_MinMax[1] = mPostProcessing.CurrentMinMax[1];

            mPostProcessing.ShowPostProcessing(1,1e9,1, mShowResultColorPlot, mEvaluationPoints, mCouplingEvaluationPoints, false, false, false, false);
        }
        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            if (mPostProcessing != null)
            {
                var toolStripMenuItemPreProcessing = Menu_AppendItem(menu, "ResultType");
                int counter = 0;
                foreach (var result_type in mPostProcessing.CurrentDistinctResultTypes)
                {
                    if (result_type == "\"DISPLACEMENT\"")
                    {
                        var toolStripMenuDisplacement = Menu_AppendItem(toolStripMenuItemPreProcessing.DropDown, result_type);
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown, "X", Menu_DoClick_DisplacementResultTypes, mPostProcessing != null, Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 0).Tag = 0;
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown, "Y", Menu_DoClick_DisplacementResultTypes, mPostProcessing != null, Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 1).Tag = 1;
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown, "Z", Menu_DoClick_DisplacementResultTypes, mPostProcessing != null, Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 2).Tag = 2;
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown, "Length", Menu_DoClick_DisplacementResultTypes, mPostProcessing != null, Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 3).Tag = 3;
                        counter++;
                    }
                    else if (result_type == "\"CAUCHY_STRESS\"")
                    {
                        var toolStripMenuDisplacement = Menu_AppendItem(toolStripMenuItemPreProcessing.DropDown, result_type);
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown, "11", Menu_DoClick_CauchyStressResultTypes, mPostProcessing != null, Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 0).Tag = 0;
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown, "22", Menu_DoClick_CauchyStressResultTypes, mPostProcessing != null, Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 1).Tag = 1;
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown, "12", Menu_DoClick_CauchyStressResultTypes, mPostProcessing != null, Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 2).Tag = 2;
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown, "von Mises", Menu_DoClick_CauchyStressResultTypes, mPostProcessing != null, Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 3).Tag = 3;
                        counter++;
                    }
                    else if (result_type == "\"PK2_STRESS\"")
                    {
                        var toolStripMenuDisplacement = Menu_AppendItem(toolStripMenuItemPreProcessing.DropDown,result_type);
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown,"11",Menu_DoClick_PK2StressResultTypes,mPostProcessing != null,Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 0).Tag = 0;
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown,"22",Menu_DoClick_PK2StressResultTypes,mPostProcessing != null,Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 1).Tag = 1;
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown,"12",Menu_DoClick_PK2StressResultTypes,mPostProcessing != null,Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 2).Tag = 2;
                        Menu_AppendItem(toolStripMenuDisplacement.DropDown,"von Mises",Menu_DoClick_PK2StressResultTypes,mPostProcessing != null,Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex == 3).Tag = 3;
                        counter++;
                    }
                    else
                    {
                        Menu_AppendItem(toolStripMenuItemPreProcessing.DropDown, result_type, Menu_DoClick_ResultTypes, mPostProcessing != null, Cocodrilo.PostProcessing.PostProcessing.s_CurrentResultInfo.ResultType == result_type).Tag = counter;
                        counter++;
                    }
                }
                Menu_AppendItem(menu, "Show Result Color Plot", Menu_DoClick_ShowResultColorPlot, mPostProcessing != null, mShowResultColorPlot);
                Menu_AppendItem(menu, "Show Knot Span Iso Curves", Menu_DoClick_ShowKnotSpanIsoCurves, mPostProcessing != null, Cocodrilo.PostProcessing.PostProcessing.s_ShowKnotSpanIsoCurves);
                Menu_AppendItem(menu, "Show Evaluation Points", Menu_DoClick_ShowEvaluationPoints, mPostProcessing != null, mEvaluationPoints);
                Menu_AppendItem(menu, "Show Coupling Evaluation Points", Menu_DoClick_ShowCouplingEvaluationPoints, mPostProcessing != null, mCouplingEvaluationPoints);
                Menu_AppendSeparator(menu);
                Menu_AppendItem(menu, "Update Read of Post Processing", Menu_DoClick_UpdatePostProcessing, mPostProcessing != null);
                Menu_AppendItem(menu, "Transfer PostProcessing to Rhino", Menu_DoClick_TransferPostProcessingToRhino, mPostProcessing != null);
            }
        }

        private void Menu_DoClick_CauchyStressResultTypes(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.ToolStripMenuItem item && item.Tag is int)
            {
                mPostProcessing.UpdateCurrentResults(
                    Cocodrilo.PostProcessing.PostProcessing.s_CurrentResultInfo.LoadCaseType,
                    Cocodrilo.PostProcessing.PostProcessing.s_CurrentResultInfo.LoadCaseNumber,
                    "\"CAUCHY_STRESS\"");

                Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex = Convert.ToInt32(item.Tag);
            }
            ExpireSolution(true);
        }
        private void Menu_DoClick_PK2StressResultTypes(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.ToolStripMenuItem item && item.Tag is int)
            {
                mPostProcessing.UpdateCurrentResults(
                    Cocodrilo.PostProcessing.PostProcessing.s_CurrentResultInfo.LoadCaseType,
                    Cocodrilo.PostProcessing.PostProcessing.s_CurrentResultInfo.LoadCaseNumber,
                    "\"PK2_STRESS\"");

                Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex = Convert.ToInt32(item.Tag);
            }
            ExpireSolution(true);
        }
        private void Menu_DoClick_UpdatePostProcessing(object sender, EventArgs e)
        {
            mPostProcessing = null;
            last_path = "";
            ExpireSolution(true);
        }

        private void Menu_DoClick_DisplacementResultTypes(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.ToolStripMenuItem item && item.Tag is int)
            {
                mPostProcessing.UpdateCurrentResults(
                    Cocodrilo.PostProcessing.PostProcessing.s_CurrentResultInfo.LoadCaseType,
                    Cocodrilo.PostProcessing.PostProcessing.s_CurrentResultInfo.LoadCaseNumber,
                    "\"DISPLACEMENT\"");

                Cocodrilo.PostProcessing.PostProcessing.s_SelectedCurrentResultDirectionIndex = Convert.ToInt32(item.Tag);
            }
            ExpireSolution(true);
        }

        private void Menu_DoClick_ShowCouplingEvaluationPoints(object sender, EventArgs e)
        {
            mCouplingEvaluationPoints = !mCouplingEvaluationPoints;
            mPostProcessing.mUpdateCouplingPoints = true;
            ExpireSolution(true);
        }

        private void Menu_DoClick_ShowEvaluationPoints(object sender, EventArgs e)
        {
            mEvaluationPoints = !mEvaluationPoints;
            mPostProcessing.mUpdateGaussPoints = true;
            ExpireSolution(true);
        }

        private void Menu_DoClick_ShowResultColorPlot(object sender, EventArgs e)
        {
            mShowResultColorPlot = !mShowResultColorPlot;
            mPostProcessing.mUpdateResultPlot = true;
            ExpireSolution(true);
        }
        private void Menu_DoClick_ShowKnotSpanIsoCurves(object sender, EventArgs e)
        {
            Cocodrilo.PostProcessing.PostProcessing.s_ShowKnotSpanIsoCurves = !Cocodrilo.PostProcessing.PostProcessing.s_ShowKnotSpanIsoCurves;
            mPostProcessing.mUpdateResultPlot = true;
            ExpireSolution(true);
        }

        private void Menu_DoClick_TransferPostProcessingToRhino(object sender, EventArgs e)
        {
            Cocodrilo.CocodriloPlugIn.Instance.PostProcessingCocodrilo = mPostProcessing;
            Cocodrilo.Panels.UserControlCocodriloPanel.Instance.UpdatePostProcessingVariables();
        }

        private void Menu_DoClick_ResultTypes(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.ToolStripMenuItem item && item.Tag is int)
            {
                int tag_id = Convert.ToInt32(item.Tag);
                item.Checked = true;

                string result_type = mPostProcessing.CurrentDistinctResultTypes[tag_id];
                mPostProcessing.UpdateCurrentResults(
                    Cocodrilo.PostProcessing.PostProcessing.s_CurrentResultInfo.LoadCaseType,
                    Cocodrilo.PostProcessing.PostProcessing.s_CurrentResultInfo.LoadCaseNumber,
                    result_type);

                ExpireSolution(true);
            }
        }

        public override void RemovedFromDocument(GH_Document document)
        {
            if (mPostProcessing != null)
            {
                mPostProcessing.ClearPostProcessing();
            }
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("AC7D16B6-F709-4DD9-B7B6-CB1B493DB374"); }
        }
    }
}
