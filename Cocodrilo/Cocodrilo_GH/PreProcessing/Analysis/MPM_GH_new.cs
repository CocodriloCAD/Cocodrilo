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
		private List<Curve> mCurveList = new List<Curve>();
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
			pManager.AddGenericParameter("Analysis", "Ana", "MPM_new", GH_ParamAccess.item);
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
			DA.GetData(1, ref AnalysisType);
			//if (!DA.GetData(1, ref AnalysisType)) return;

			if (!DA.GetDataTree(2, out GH_Structure<IGH_Goo> geometries)) return;
			var geometries_flat = geometries.FlattenData();

			bool run_analysis = false;
			if (!DA.GetData(3, ref run_analysis)) return;

			if (run_analysis)
			{
				/// Resets the entire user data stored on the geometries 
				ResetUserData(geometries_flat); // look up at shell

				mMeshList = new List<Mesh>();
				mCurveList = new List<Curve>();
				mNewAnalysis = new AnalysisMpm_new();

				mNewAnalysis.Name = Name;
				mNewAnalysis.mAnalysisType_static_dynamic_quasi_static = AnalysisType;

				foreach (var obj in geometries_flat)
				{
					bool success = obj.CastTo(out Cocodrilo_GH.PreProcessing.Geometries.Geometries geoms);
					if (success)
					{
						foreach (var mesh in geoms.meshes)
						{
							bool add_mesh = true;
							var ud = mesh.Key.UserData.Find(typeof(Cocodrilo.UserData.UserDataMesh)) as Cocodrilo.UserData.UserDataMesh;

							if (ud == null)
							{
								ud = new Cocodrilo.UserData.UserDataMesh();
								mesh.Key.UserData.Add(ud);
							}
							else
							{
								foreach (var old_mesh in mMeshList)
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
						
						foreach (var edge in geoms.edges)
						{
							bool add_edge = true;
							var ud = edge.Key.UserData.Find(typeof(Cocodrilo.UserData.UserDataEdge)) as Cocodrilo.UserData.UserDataEdge;
							if (ud == null)
							{
								ud = new Cocodrilo.UserData.UserDataEdge();
								edge.Key.UserData.Add(ud);
							}
							else
							{
								// what is the purpose of this for-loop? Is it okay that it goes over mCurveList??
								foreach (var old_edge in mCurveList)
								{
									var ud2 = old_edge.UserData.Find(typeof(Cocodrilo.UserData.UserDataEdge)) as Cocodrilo.UserData.UserDataEdge;

									if (ReferenceEquals(ud2.GetCurrentElementData(), ud.GetCurrentElementData()))
										add_edge = false;
								}
							}

							ud.AddNumericalElement(edge.Value);

							if (add_edge)
							{
								mCurveList.Add(edge.Key);
							}
						}

						//Change 04.11: adding of curves to represent strong boundary conditions
						foreach (var curve in geoms.curves)
						{
							bool add_curve = true;
							var ud = curve.Key.UserData.Find(typeof(Cocodrilo.UserData.UserDataCurve)) as Cocodrilo.UserData.UserDataCurve;
							if (ud == null)
							{
								ud = new Cocodrilo.UserData.UserDataCurve();
								curve.Key.UserData.Add(ud);
							}
							else
							{
								foreach (var old_curve in mCurveList)
								{
									var ud2 = old_curve.UserData.Find(typeof(Cocodrilo.UserData.UserDataCurve)) as Cocodrilo.UserData.UserDataCurve;

									if (ud2 != null)
									{

										if (ReferenceEquals(ud2.GetCurrentElementData(), ud.GetCurrentElementData()))
											add_curve = false;
									}
								}
							}

							ud.AddNumericalElement(curve.Value);

							if (add_curve)
							{
								mCurveList.Add(curve.Key);
							}
						}
					}
					else
					{
						if (obj is GH_Mesh)
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
				mNewAnalysis.mCurveList = mCurveList;
				//var castedAnalysis = (Cocodrilo.Analyses.Analysis)mNewAnalysis;
				DA.SetData(0, mNewAnalysis);
			}
		}	
				/////////////////////////////////////////////////////////////////////////////////////////////////
				/// <summary>
				/// Resets the UserData of the geometries.
				///
				/// Required once paths are removed to clear the obsulete memory.
				/// </summary>
				/// <param name="geometries_flat">list of 'Geometries' objects</param>
		private void ResetUserData(List<IGH_Goo> geometries_flat)
		{
			foreach (var obj in geometries_flat)
			{
				bool success = obj.CastTo(out Cocodrilo_GH.PreProcessing.Geometries.Geometries geoms);
				if (success)
				{
					foreach (var mesh in geoms.meshes)
					{
						var ud = mesh.Key.UserData.Find(typeof(Cocodrilo.UserData.UserDataCurve)) as Cocodrilo.UserData.UserDataCurve;
						ud?.DeleteNumericalElements();
						if (ud != null)
						{
							ud.BrepId = -1;
						}
					}
					
				}
			}
						
		}
				
		public override Guid ComponentGuid
		{
			get { return new Guid("19A7EA35-DC3A-456A-A7B2-14EC582B1CA4"); }
		}

	}

}
