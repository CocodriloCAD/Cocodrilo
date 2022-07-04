using System;
using System.Collections.Generic;
using Rhino.Geometry;
using Grasshopper.Kernel;

using Cocodrilo.Analyses;
using Cocodrilo.Materials;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;

using Cocodrilo.ElementProperties;
//using Cocodrilo.Analyses.AnalysisMPM_new;

namespace Cocodrilo_GH.PreProcessing.Analysis
{		
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

			///Analysis type determines whether the analysis is static, dynamic or quasi-static
			pManager.AddGenericParameter("Analysis Type", "Analysis Type", "Type of Analysis", GH_ParamAccess.item);

			pManager.AddGenericParameter("Material", "Mat", "Material of Element", GH_ParamAccess.item);
					
			
			pManager.AddGeometryParameter("BodyMesh", "Mesh", "BodyMesh", GH_ParamAccess.list);
			pManager.AddBooleanParameter("Run", "Run", "Run output", GH_ParamAccess.item, false);

		}
		/// <summary>
		/// Registers all the output parameters for this component.
		/// </summary>
		protected override void RegisterOutputParams(GH_OutputParamManager pManager)
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
			Cocodrilo.Analyses.Analysis AnalysisType = null;
			Material material = null;
			bool run = false;
			
			//private bool run;
			if (!DA.GetData(0, ref Name)) return;

			if (!DA.GetData(1, ref AnalysisType)) return;

			if (!DA.GetData(2, ref material)) return;

			if (!DA.GetDataTree(3, out GH_Structure<IGH_Goo> geometries)) return;
			var geometries_flat = geometries.FlattenData();

			if (!DA.GetData(4, ref run)) return;

			// Make name fit
			if (Name.Contains(" "))
			{
				Name = Name.Replace(" ", "");
				AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed.");
			}

			DA.SetData(0, geometries);
			//DA.SetData(0, new Cocodrilo.Analyses.AnalysisMpm_new(Name, AnalysisType, material, mesh_list));
			Cocodrilo.Analyses.AnalysisMpm_new newAnalysis = new Cocodrilo.Analyses.AnalysisMpm_new(Name, AnalysisType, material); //, mesh_list)
		}

		//Replace Guid with real value
		public override Guid ComponentGuid
		{
			get { return new Guid("1234ABCD-123A-4BAC-B912-A1B2C3D4E5F6"); }
		}

	}

}
