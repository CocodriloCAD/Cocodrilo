using System;

using Grasshopper.Kernel;

namespace Cocodrilo_GH.PreProcessing.Analysis
{
    public class LinearStaticAnalysis_GH : GH_Component
    {
        public LinearStaticAnalysis_GH()
          : base("LinearStaticAnalysis", "Linear",
              "Geometrically Linear Structural Static Analysis",
              "Cocodrilo", "Analyses")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Nam", "Name of Analysis", GH_ParamAccess.item, "LinearStaticAnalysis");
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis", "Ana", "LinearStaticAnalysis", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string Name = "";
            if (!DA.GetData(0, ref Name)) return;

            // Make name fit
            if (Name.Contains(" "))
            {
                Name = Name.Replace(" ", "");
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed.");
            }

            var analysis2 = new Cocodrilo.Analyses.AnalysisLinear(Name);

            DA.SetData(0, analysis2);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.analysis_l; }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("2BE9496F-5813-41CE-82A0-946286784498"); }
        }
    }
}