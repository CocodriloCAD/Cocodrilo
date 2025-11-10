using System;

using Grasshopper.Kernel;

using Cocodrilo.Materials;

namespace Cocodrilo_GH.PreProcessing.Materials
{
    public class Steel_GH : GH_Component
    {
        public Steel_GH()
          : base("Steel", "Steel", "Plane Stress Linear Elastic", "Cocodrilo", "Materials")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Material", "Mat", "Material", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var material = new MaterialLinearElasticIsotropic("Steel", 20e7, 0.0);
            Cocodrilo.CocodriloPlugIn.Instance.AddMaterial(material);

            DA.SetData(0, material);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.materials_steel; }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("EDFF904B-3C68-4138-A940-FCE377A22283"); }
        }
    }
}