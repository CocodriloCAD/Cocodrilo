using System;
using System.Collections.Generic;
using System.Linq;
using Cocodrilo.IO;
using Cocodrilo.Materials;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.IO
{
    public class ModelModuleOptimization_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ModelModuleOptimization class.
        /// </summary>
        public ModelModuleOptimization_GH()
          : base("Module Optimization", "Module Optimization",
              "Employs an optimization of material application.",
              "Cocodrilo", "Models")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("IGA Model", "M", "IGA Model", GH_ParamAccess.list);
            pManager.AddGenericParameter("Materials", "Ma", "Possible materials for modules.", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Output> output_list = new List<Output>();
            DA.GetDataList(0, output_list);

            List<Material> materials = new List<Material>();
            if (!DA.GetDataList(1, materials)) return;

            List<int> material_ids = materials.Select(item => item.Id).ToList();

            if (output_list[0] is OutputKratosIGA)
            {
                (output_list[0] as OutputKratosIGA).WriteOptimizationFiles(material_ids);
            }

        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("D234DCBD-328A-4065-A0B7-9139756E41C0"); }
        }
    }
}