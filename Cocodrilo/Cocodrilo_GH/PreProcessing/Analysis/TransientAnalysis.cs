using System;
using Grasshopper.Kernel;

namespace Cocodrilo_GH.PreProcessing.Analysis
{
    public class TransientAnalysis_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the NonLinearStaticAnalysis class.
        /// </summary>
        public TransientAnalysis_GH()
          : base("TransientAnalysis", "Transient Analysis",
              "Non-Linear Dynamic Analysis",
              "Cocodrilo", "Analyses")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of Analysis", GH_ParamAccess.item, "Transient Analysis");
            pManager.AddIntegerParameter("Maximal Solver Iterations", "MaxSolverIteration", "Maximal solver iterations per time step", GH_ParamAccess.item, 20);
            pManager.AddIntegerParameter("Number Of Steps", "NumSteps", "Number of simulation steps", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("Solver Tolerance", "SolverTolerance", "Absolut displacement and resiudal tolerance", GH_ParamAccess.item, 1e-6);
            pManager.AddNumberParameter("Step Size", "StepSize", "Step size of simulation steps", GH_ParamAccess.item, 0.01);

            //public int Adaptive_Max_Level { get; set; };
            pManager.AddIntegerParameter("Adaptive_Max_Level", "??", "??", GH_ParamAccess.item, 1);

            pManager.AddNumberParameter("RayleighAlpha", "alpha", "Coefficient Alpha for Rayleigh damping", GH_ParamAccess.item, 1);
            pManager.AddNumberParameter("RayleighBeta", "beta", "Coefficient Beta for Rayleigh damping", GH_ParamAccess.item, 1);

            //public string TimeInteg { get; set; }
            pManager.AddTextParameter("Time Integration", "TimeInteg", "Time integration method", GH_ParamAccess.item, "implicit");

            //public string Scheme { get; set; }
            pManager.AddTextParameter("Scheme", "Scheme", "Time step scheme", GH_ParamAccess.item, "newmark");

            //public bool AutomaticRayleigh { get; set; }
            pManager.AddBooleanParameter("AutomaticRayleigh", "AutomaticRayleigh", "?", GH_ParamAccess.item, false);

            //public double DampingRatio0 { get; set; }
            pManager.AddNumberParameter("DampingRatio0", "Damping Ratio 0", "alpha", GH_ParamAccess.item, 1);

            //public double DampingRatio1 { get; set; }
            pManager.AddNumberParameter("DampingRatio1", "Damping Ratio 1", "beta", GH_ParamAccess.item, 1);

            //public double NumEigen { get; set; }
            pManager.AddNumberParameter("NumEigen", "Number of Eigenvalues", "Number of Eigenvalues", GH_ParamAccess.item, 1);

            //public number Time { get; set; }
            pManager.AddNumberParameter("Time", "End Time", "Duration of simulation in [s]", GH_ParamAccess.item, 1.0);

            //public double Value { get; set; }
            pManager.AddNumberParameter("Value", "Value", "Value", GH_ParamAccess.item, 1);
            
            //End time


        }

            /// <summary>
            /// Registers all the output parameters for this component.
            /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis", "Ana", "TransientAnalysis", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "";
            if (!DA.GetData(0, ref name)) return;

            int maxSolverIteration = 0;
            if (!DA.GetData(1, ref maxSolverIteration)) return;

            int numberOfSimulationSteps = 0;
            if (!DA.GetData(2, ref numberOfSimulationSteps)) return;

            double solverTolerance = 0;
            if (!DA.GetData(3, ref solverTolerance)) return;

            double stepSize = 0;
            if (!DA.GetData(4, ref stepSize)) return;

            int adaptiveMaxLevel = 0;
            if (!DA.GetData(5, ref adaptiveMaxLevel)) return;

            double rayleighAlpha = 0;
            if (!DA.GetData(6, ref rayleighAlpha)) return;

            double rayleighBeta = 0;
            if (!DA.GetData(7, ref rayleighBeta)) return;

            string timeIntegration = "";
            if (!DA.GetData(8, ref timeIntegration)) return;

            string scheme = "";
            if (!DA.GetData(9, ref scheme)) return;

            bool automaticRayleigh = false;
            if (!DA.GetData(10, ref automaticRayleigh)) return;

            double dampingRatio0 = 0;
            if (!DA.GetData(11, ref dampingRatio0)) return;

            double dampingRatio1 = 0;
            if (!DA.GetData(12, ref dampingRatio1)) return;

            double numEigen = 0;
            if (!DA.GetData(13, ref numEigen)) return;
                       
            double endTime = 0;
            if (!DA.GetData(14, ref endTime)) return;

            double value = 0;
            if (!DA.GetData(15, ref value)) return;


            // Make name fit
            if (Name.Contains(" "))
            {
                Name = Name.Replace(" ", "");
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed.");
            }

            var analysis2 = new Cocodrilo.Analyses.AnalysisTransient(name, numberOfSimulationSteps, maxSolverIteration, solverTolerance, endTime, value, adaptiveMaxLevel, rayleighAlpha, rayleighBeta, timeIntegration, scheme, automaticRayleigh, dampingRatio0, dampingRatio1,
            numEigen, stepSize = 0.1);

            DA.SetData(0, analysis2);
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
        /// ENTER MEANINGFUL ID
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("EA61F5C3-0DB6-4934-A9F6-8A852C709CE6"); }
        }
    }
}
