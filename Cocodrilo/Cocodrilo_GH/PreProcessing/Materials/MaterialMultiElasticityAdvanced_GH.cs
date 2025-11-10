using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;
using Cocodrilo.Materials;

namespace Cocodrilo_GH.PreProcessing.Materials
{
    public class MaterialMultiElasticityAdvanced_GH : GH_Component
    {
        public MaterialMultiElasticityAdvanced_GH()
          : base("Material Multi Elasticity Advanced", "MatAdv", "Structural material with advanced input parameters.", "Cocodrilo", "Materials")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Name of material", GH_ParamAccess.item, "Steel");
            pManager.AddNumberParameter("E", "E", "Vector of Young's moduli", GH_ParamAccess.list);
            pManager.AddNumberParameter("epsilon", "e", "Vector of strain limits", GH_ParamAccess.list);
            pManager.AddNumberParameter("nue", "n", "Poisson's ratio nue", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("Thickness", "T", "Corresponding thickness", GH_ParamAccess.item, 1.0);
            pManager.AddNumberParameter("rho", "r", "Density rho", GH_ParamAccess.item, 1.0);
            pManager.AddBooleanParameter("Self Weight", "SW", "Add self weight", GH_ParamAccess.item, true);
            pManager.AddVectorParameter("Gravity", "G", "Gravity vector", GH_ParamAccess.item, new Vector3d(0,0,1));
            pManager.AddNumberParameter("Compression Strength", "fc", "Strength in Compression", GH_ParamAccess.item, 10.0);
            pManager.AddNumberParameter("Tension Strength", "ft", "Strength in Tension", GH_ParamAccess.item, 0.1);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "M", "Material", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string name = "";
            if (!DA.GetData(0, ref name))  return;
            List<double> E_list = new List<double>();
            if (!DA.GetDataList(1, E_list)) return;
            List<double> epsilon_list = new List<double>();
            if (!DA.GetDataList(2, epsilon_list)) return;
            double nue = 0;
            if (!DA.GetData(3, ref nue)) return;
            double thickness = 0;
            if (!DA.GetData(4, ref thickness)) return;
            double rho = 0;
            if (!DA.GetData(5, ref rho)) return;
            bool add_self_weight = true;
            if (!DA.GetData(6, ref add_self_weight)) return;
            Vector3d gravity = new Vector3d(0,0,1);
            if (!DA.GetData(7, ref gravity)) return;
            double compression_strength = 0;
            if (!DA.GetData(8, ref compression_strength)) return;
            double tension_strength = 0;
            if (!DA.GetData(9, ref tension_strength)) return;

            var material = new MaterialMultiElasticityAdvanced(
                name, E_list, epsilon_list, nue, thickness, rho, add_self_weight, gravity, compression_strength, tension_strength);
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
            get { return new Guid("5B1A00AB-6AB0-40C6-A399-40B0053DEEB5"); }
        }
    }
}