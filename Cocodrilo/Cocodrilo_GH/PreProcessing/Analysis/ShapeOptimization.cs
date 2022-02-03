using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.Analysis
{
    public class ShapeOptimization : GH_Component
    {
        public ShapeOptimization()
          : base("ShapeOptimization", "Shape Optimization",
              "Shape Optimization",
              "Cocodrilo", "Analyses")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of Analysis", GH_ParamAccess.item, "LinearStaticAnalysis");
            pManager.AddNumberParameter("Max Optimization Iterations", "MaxOptimizationIterations", "Max processed Optimization Iterations", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Step Size", "StepSize", "Step size of gradient step", GH_ParamAccess.item, 1);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis", "Analysis", "Shape Optimization", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string Name = "";
            if (!DA.GetData(0, ref Name)) return;

            int MaxOptimizationIterations = 0;
            if (!DA.GetData(1, ref MaxOptimizationIterations)) return;

            double StepSize = 0;
            if (!DA.GetData(2, ref StepSize)) return;

            if (Name.Contains(" "))
            {
                Name = Name.Replace(" ", "");
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed within Name.");
            }

            DA.SetData(0, new Cocodrilo.Analyses.AnalysisShapeOptimization(Name, MaxOptimizationIterations, StepSize));
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
            get { return new Guid("B4E61B2C-A0AB-4C8B-9888-FCC4C73FAAA1"); }
        }
    }
}