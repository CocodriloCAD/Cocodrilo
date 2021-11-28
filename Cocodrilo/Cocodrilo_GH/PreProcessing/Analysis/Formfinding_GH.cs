using System;
using Grasshopper.Kernel;

namespace Cocodrilo_GH.PreProcessing.Analysis
{
    public class Formfinding_GH : GH_Component
    {
        public Formfinding_GH()
          : base("Formfinding", "Formfinding", "Structural Formfinding", "Cocodrilo", "Analyses")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Nam", "Name of Analysis", GH_ParamAccess.item, "Formfinding");
            pManager.AddIntegerParameter("Formfinding Steps", "Ste", "Number of Formfinding Iterations", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("Iterations", "Ite", "Number of Iterations per Formfinding Step", GH_ParamAccess.item, 15);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis", "Ana", "Formfinding", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string Name = "";
            int FormFindingSteps = 0;
            int Iterations = 0;
            if (!DA.GetData(0, ref Name)) return;
            if (!DA.GetData(1, ref FormFindingSteps)) return;
            if (!DA.GetData(2, ref Iterations)) return;

            // Make name fit
            if (Name.Contains(" "))
            {
                Name = Name.Replace(" ", "");
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed.");
            }

            DA.SetData(0, new Cocodrilo.Analyses.AnalysisFormfinding(Name, FormFindingSteps, Iterations, 0.001));
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
            get { return new Guid("42951F3B-CB4E-49A2-BD3B-9599B2B02AB8"); }
        }
    }
}