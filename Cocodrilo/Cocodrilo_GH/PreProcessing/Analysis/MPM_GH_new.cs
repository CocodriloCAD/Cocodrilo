using System;

using Grasshopper.Kernel;

using Cocodrilo.Materials;
//using Cocodrilo.Analyses.AnalysisMPM_new;

namespace Cocodrilo_GH.PreProcessing.Analysis
{
	//enum AnalysisType
 //  {
	//	Static,
	//	QuasiStatic,
	//	Dynamic
 //  }
	
	public class MPM_GH_new : GH_Component
	{
		//empty constructor to override base class constructor

		/// <summary>
		/// Initializes a new instance of the MPM_GH class.
		/// </summary>
		public MPM_GH_new()
		  : base("MPM", "MPM",
			  "Material Point Method",
			  "Cocodrilo", "Analyses")
		{
		}

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>

        protected override void RegisterInputParams(GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of Analysis", GH_ParamAccess.item, "MpmAnalysis");
			pManager.AddGenericParameter("Analysis Type", "Analysis Type", "Type of Analysis", GH_ParamAccess.item);
			
			pManager.AddGenericParameter("Material", "Mat", "Material of Element", GH_ParamAccess.item);
			pManager.AddTextParameter("Material law", "MatLaw", "Material law of Material", GH_ParamAccess.item);
			pManager.AddTextParameter("Solver", "Solver", "Type of solver", GH_ParamAccess.item, "LinearSolver");
			pManager.AddIntegerParameter("N_p", "N_p", "Number of particles per Element", GH_ParamAccess.item, 3);

			pManager.AddBooleanParameter("Run", "Run", "Run output", GH_ParamAccess.item, false);
		}
		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Analysis", "A", "MPM_new", GH_ParamAccess.item);
		}

		/// <summary>
		/// This is the method that actually does the work. But why?
		/// </summary>
		/// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			string Name = "";
			string AnalysisType = "";
			string material ="";
			string solverType ="";
			string materialLaw = "";
			int numberOfParticles = 0;
			bool run = false;

			//private bool run;
			if (!DA.GetData(0, ref Name)) return;

			if (!DA.GetData(1, ref AnalysisType)) return;
			if (!DA.GetData(2, ref material)) return;
			if (!DA.GetData(3, ref materialLaw)) return;
			if (!DA.GetData(4, ref solverType)) return;
			if (!DA.GetData(5, ref numberOfParticles)) return;
			if (!DA.GetData(6, ref run)) return;

			// Make name fit
			if (Name.Contains(" "))
			{
				Name = Name.Replace(" ", "");
				AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed.");
			}

			DA.SetData(0, new Cocodrilo.Analyses.AnalysisMpm_new(Name, AnalysisType, material, materialLaw, solverType, numberOfParticles));
		}

		//Replace Guid with real value
		public override Guid ComponentGuid
		{
			get { return new Guid("1234ABCD-123A-4BAC-B912-A1B2C3D4E5F6"); }
		}

	}

}
