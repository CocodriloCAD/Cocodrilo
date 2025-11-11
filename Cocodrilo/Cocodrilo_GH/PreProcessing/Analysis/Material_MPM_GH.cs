using System;
using Grasshopper.Kernel;
using Cocodrilo.Materials;

/// <summary>
/// This material class was introduced to allow a direct modeling of MPM materials. Due to the way, how KRATOS reads the material parameters for MPM, the thickness
/// of the regarded elements was introduced here. ~phfranz, Nov '22
/// </summary>



namespace Cocodrilo_GH.PreProcessing.Materials
{
	public class Material_MPM_GH : GH_Component
	{

		public Material_MPM_GH() : base("Material", "Material", "MPM Material", "Cocodrilo", "Materials")
		{ }
		
		protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddTextParameter("Material Name", "N", "Name of Material", GH_ParamAccess.item, "Steel");
			pManager.AddTextParameter("Constitutive Law", "CL", "Constitutive Law", GH_ParamAccess.item, "LinearElasticIsotropicPlaneStrain2DLaw");
			pManager.AddNumberParameter("rho", "rho", "Density", GH_ParamAccess.item, 7850.0);
			pManager.AddNumberParameter("E", "E", "Young's modulus", GH_ParamAccess.item, 206900000000.0);
			pManager.AddNumberParameter("nue", "nue", "Poisson's Ratio", GH_ParamAccess.item, 0.29);
			pManager.AddNumberParameter("c", "c", "Cohesion", GH_ParamAccess.item, 0.0);
			pManager.AddNumberParameter("phi", "phi", "Internal Friction Angle", GH_ParamAccess.item, 0.0);
			pManager.AddNumberParameter("psi", "psi", "Internal Dilatancy Angle", GH_ParamAccess.item, 0.0);
			pManager.AddIntegerParameter("#n", "n", "Number of Particles per Element", GH_ParamAccess.item, 3);
			pManager.AddNumberParameter("t", "t", "Thickness of Element", GH_ParamAccess.item, 1.0);
		}

		protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
		{
			pManager.AddGenericParameter("Material", "M", "Material", GH_ParamAccess.item);
		}

		protected override void SolveInstance(IGH_DataAccess DA)
		{
			string name = "";
			if (!DA.GetData(0, ref name)) return;
			string constitutivelaw = "";
			if (!DA.GetData(1, ref constitutivelaw)) return;
			double rho = 0;
			if (!DA.GetData(2, ref rho)) return;
			double E = 0;
			if (!DA.GetData(3, ref E)) return;
			double nue = 0;
			if (!DA.GetData(4, ref nue)) return;
			double c = 0;
			if (!DA.GetData(5, ref c)) return;
			double phi = 0;
			if (!DA.GetData(6, ref phi)) return;
			double psi = 0;
			if (!DA.GetData(7, ref psi)) return;
			int numberofparticles = 0;
			if (!DA.GetData(8, ref numberofparticles)) return;
			double thickness = 0;
			if (!DA.GetData(9, ref thickness)) return;

			var material = new MaterialNonLinear(name, constitutivelaw, rho, E, nue, c, phi, psi, numberofparticles, thickness);
			Cocodrilo.CocodriloPlugIn.Instance.AddMaterial(material);

			DA.SetData(0, material);
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
			get { return new Guid("2DDA1D29-D805-41F8-BEDE-AC089B852EEA"); }
		}
	}
}


