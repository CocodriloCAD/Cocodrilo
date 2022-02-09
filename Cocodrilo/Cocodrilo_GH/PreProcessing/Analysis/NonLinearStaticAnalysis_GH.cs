using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.Analysis
{
    public class NonLinearStaticAnalysis_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the NonLinearStaticAnalysis class.
        /// </summary>
        public NonLinearStaticAnalysis_GH()
          : base("NonLinearStaticAnalysis", "Non-Linear",
              "Geometrically Non-Linear Structural Static Analysis",
              "Cocodrilo", "Analyses")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of Analysis", GH_ParamAccess.item, "NonLinearStaticAnalysis");
            pManager.AddIntegerParameter("Number Of Steps", "NumSteps", "Number of simulation steps", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Step Size", "StepSize", "Step size of simulation steps", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Solver Tolerance", "SolverTolerance", "Absolut displacement and resiudal tolerance", GH_ParamAccess.item, 1e-6);
            pManager.AddIntegerParameter("Maximal Solver Iterations", "MaxSolverIteration", "Maximal solver iterations per time step", GH_ParamAccess.item, 20);
        }

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

            int NumberOfSimulationSteps = 0;
            if (!DA.GetData(1, ref NumberOfSimulationSteps)) return;

            double StepSize = 0;
            if (!DA.GetData(2, ref StepSize)) return;

            double SolverTolerance = 0;
            if (!DA.GetData(3, ref SolverTolerance)) return;

            int MaxSolverIteration = 0;
            if (!DA.GetData(4, ref MaxSolverIteration)) return;

            // Make name fit
            if (Name.Contains(" "))
            {
                Name = Name.Replace(" ", "");
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed.");
            }

            DA.SetData(0, new Cocodrilo.Analyses.AnalysisNonLinear(Name, NumberOfSimulationSteps, MaxSolverIteration, SolverTolerance, StepSize));
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.analysis_nln; }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("EA61E5B3-0DB6-4634-A9F6-8A852C709CE6"); }
        }
    }
}