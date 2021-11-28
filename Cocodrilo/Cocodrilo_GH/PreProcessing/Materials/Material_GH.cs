using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Cocodrilo.Materials;

namespace Cocodrilo_GH.PreProcessing.Materials
{
    public class Material_GH : GH_Component
    {
        public Material_GH()
          : base("Material", "Material", "Structural Material", "Cocodrilo", "Materials")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Material Name", "N", "Name of Material", GH_ParamAccess.item, "Steel");
            pManager.AddNumberParameter("E", "E", "Young's modulus", GH_ParamAccess.item, 200000);
            pManager.AddNumberParameter("Nue", "N", "Poisson's Ratio", GH_ParamAccess.item, 0.0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "M", "Material", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "";
            if (!DA.GetData(0, ref name))  return;
            double E = 0;
            if (!DA.GetData(1, ref E)) return;
            double nue = 0;
            if (!DA.GetData(1, ref nue)) return;

            var material = new MaterialLinearElasticIsotropic(name, E, nue);
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
            get { return new Guid("2901DF24-CA4D-460F-A9DC-2045F49B497C"); }
        }
    }
}