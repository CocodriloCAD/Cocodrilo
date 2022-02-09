using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.Analysis
{
    public class EigenvalueAnalysis_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the EigenvalueAnalysis_GH class.
        /// </summary>
        public EigenvalueAnalysis_GH()
          : base("EigenvalueAnalysis", "Eigenvalues",
              "Analysis of the eigen values.",
              "Cocodrilo", "Analyses")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Name of Analysis", GH_ParamAccess.item, "EigenvalueAnalysis");
            pManager.AddIntegerParameter("Number Of Eigenvalues", "NE", "Number of eigen values", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("Maximum Iterations", "MI", "Maximum solving iterations", GH_ParamAccess.item, 20);        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis", "Ana", "NonLinearStaticAnalysis", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string Name = "";
            if (!DA.GetData(0, ref Name)) return;

            int NumEigenvalues = 0;
            if (!DA.GetData(1, ref NumEigenvalues)) return;

            int MaximumIterations = 0;
            if (!DA.GetData(2, ref MaximumIterations)) return;

            // Make name fit
            if (Name.Contains(" "))
            {
                Name = Name.Replace(" ", "");
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed.");
            }

            DA.SetData(0, new Cocodrilo.Analyses.AnalysisEigenvalue(Name, 0.001, NumEigenvalues, MaximumIterations, "spectra_sym_g_eigs_shift"));
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.analysis_eig; }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("33079E71-5B69-4627-8616-65DCFEC9DF93"); }
        }
    }
}