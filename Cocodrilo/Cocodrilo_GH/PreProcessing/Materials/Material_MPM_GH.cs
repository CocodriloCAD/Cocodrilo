using System;
using Grasshopper.Kernel;
using Cocodrilo.Materials;

namespace Cocodrilo_GH.PreProcessing.Materials
{
	public class Material_MPM_GH : GH_Component
	{

        public Material_MPM_GH() : base("MPM Material", "Material", "MPM Material", "Cocodrilo", "Materials")
        { }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
		{
			pManager.AddTextParameter("Material Name", "Name", "Name of Material", GH_ParamAccess.item, "Sand");
			pManager.AddTextParameter("Constitutive Law", "CL", "Constitutive Law", GH_ParamAccess.item, "HenckyMCPlasticPlaneStrain2DLaw");
			pManager.AddNumberParameter("rho", "rho", "Density", GH_ParamAccess.item, 1.0);
			pManager.AddNumberParameter("E", "E", "Young's modulus", GH_ParamAccess.item, 200000);
			pManager.AddNumberParameter("nue", "nue", "Poisson's Ratio", GH_ParamAccess.item, 0.0);
			pManager.AddNumberParameter("c", "c", "Cohesion", GH_ParamAccess.item, 1.0);
			pManager.AddNumberParameter("phi", "phi", "Internal Friction Angle", GH_ParamAccess.item, 25);
			pManager.AddNumberParameter("psi", "psi", "Internal Dilatancy Angle", GH_ParamAccess.item, 0);
			pManager.AddIntegerParameter("#n", "#n", "Number of Particles per Element", GH_ParamAccess.item, 3);

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

			var material = new MaterialNonLinear(name, constitutivelaw, rho, E, nue, c, phi, psi, numberofparticles);
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

		// ADD MEANINGFULL COMPONENT GUID
		public override Guid ComponentGuid
		{
			get { return new Guid("1b3412d4-1234-1234-1234-123412341234"); }
		}

	}
}


