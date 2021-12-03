using Cocodrilo.Materials;
using Grasshopper.Kernel;
using System;

namespace Cocodrilo_GH.PreProcessing.Materials
{
    public class Masonry_GH : GH_Component
    {
        private string MasonryType = "MasonryEindhoven";

        public Masonry_GH()
          : base("Masonry", "Masonry", "Plane Stress Orthotropic Damage Model", "Cocodrilo", "Materials")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Material", MasonryType, GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var material = new MaterialOrthotropicDamage("MasonryEindhoven");
            Cocodrilo.CocodriloPlugIn.Instance.AddMaterial(material);

            DA.SetData(0, material);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.Masonry; }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("FC8185FB-148C-4748-B2BB-FEDD9A5B73B1"); }
        }
    }
}