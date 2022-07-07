using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Cocodrilo.ElementProperties;


using Cocodrilo.Analyses;
using Cocodrilo.Materials;





namespace Cocodrilo_GH.PreProcessing.Analysis
{		
	public class MPM_GH_new : GH_Component
	{
		private List<Mesh> mMeshList = new List<Mesh>();
		public Cocodrilo.Analyses.AnalysisMpm_new mNewAnalysis = new Cocodrilo.Analyses.AnalysisMpm_new();

		/// empty constructor to override base class constructor
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

			/// Analysis type determines whether the analysis is static, dynamic or quasi-static
			pManager.AddGenericParameter("Analysis Type", "Analysis Type", "Type of Analysis", GH_ParamAccess.item);

			pManager.AddGenericParameter("Geometry", "Geo", "Meshed geometry of body", GH_ParamAccess.tree);
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
		/// This is the method that actually does the work. 
		/// </summary>
		/// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
		protected override void SolveInstance(IGH_DataAccess DA)
		{
			string Name = "";
			if (!DA.GetData(0, ref Name)) return;

			/// Make name fitting
			if (Name.Contains(" "))
			{
				Name = Name.Replace(" ", "");
				AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed.");
			}
			
			Cocodrilo.Analyses.Analysis AnalysisType = null;
			if (!DA.GetData(1, ref AnalysisType)) return;
			
			if (!DA.GetDataTree(2, out GH_Structure<IGH_Goo> geometries)) return;
			var geometries_flat = geometries.FlattenData();

			bool run_analysis = false;
			if (!DA.GetData(3, ref run_analysis)) return;

			if (run_analysis)
            {
				/// Resets the entire user data stored on the geometries
				//ResetUserData(geometries_flat);

				mMeshList = new List<Mesh>();
				mNewAnalysis = new AnalysisMpm_new();

				mNewAnalysis.Name = Name;
				mNewAnalysis.mAnalysisType_static_dynamic_quasi_static = AnalysisType;

				foreach ( var obj in geometries_flat)
                {
					bool success = obj.CastTo(out Cocodrilo_GH.PreProcessing.Geometries.Geometries geoms);
					if (success)
                    {
						foreach(var mesh in geoms.meshes)
                        {
							bool add_mesh = true;
							var ud = mesh.Key.UserData.Find(typeof(Cocodrilo.UserData.UserDataMesh)) as Cocodrilo.UserData.UserDataMesh;
							
							if(ud==null)
                            {
								ud = new Cocodrilo.UserData.UserDataMesh();
								mesh.Key.UserData.Add(ud);
                            }
							else
                            {
								foreach(var old_mesh in mMeshList)
                                {
									var ud2 = old_mesh.UserData.Find(typeof(Cocodrilo.UserData.UserDataCurve)) as Cocodrilo.UserData.UserDataMesh;

									if (ReferenceEquals(ud2.GetCurrentElementData(), ud.GetCurrentElementData()))
										add_mesh = false;
                                }
                            }

							ud.AddNumericalElement(mesh.Value);

							if (add_mesh)
								mMeshList.Add(mesh.Key);
                        }
                    }
					else
                    {
						if(obj is GH_Mesh)
                        {
							Mesh mesh = null;
							GH_Convert.ToMesh(obj, ref mesh, GH_Conversion.Primary);
							if (!mMeshList.Contains(mesh))
                            {
								mMeshList.Add(mesh);
                            }
                        }

						/// Add error message in case none of the above mentioned cases fits
					}
                }

				mNewAnalysis.mBodyMesh = mMeshList;

				var castedAnalysis = (Cocodrilo.Analyses.Analysis)mNewAnalysis;

				string project_path = Cocodrilo.UserData.UserDataUtilities.GetProjectPath(mNewAnalysis.Name);

				var output_kratos_fem = new Cocodrilo.IO.OutputKratosFEM(AnalysisType);

				output_kratos_fem.StartAnalysis(project_path, mMeshList, ref castedAnalysis);

				DA.SetData(0, output_kratos_fem);
				
				//DA.SetData(1, project_path);

			}

			

			//DA.SetData(0, geometries);
			//DA.SetData(0, new Cocodrilo.Analyses.AnalysisMpm_new(Name, AnalysisType, material, mesh_list));
			//Cocodrilo.Analyses.AnalysisMpm_new newAnalysis = new Cocodrilo.Analyses.AnalysisMpm_new(Name, AnalysisType, material, meshList);// geometries.); //, mesh_list)
		}

		//Replace Guid with real value
		public override Guid ComponentGuid
		{
			get { return new Guid("1234ABCD-123A-4BAC-B912-A1B2C3D4E5F6"); }
		}

	}

}
