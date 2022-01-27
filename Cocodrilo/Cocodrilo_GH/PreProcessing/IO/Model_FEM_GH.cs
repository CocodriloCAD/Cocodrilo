using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

using Cocodrilo.IO;

namespace Cocodrilo_GH.PreProcessing.IO
{
    public class Model_FEM_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Model_FEM_GH class.
        /// </summary>
        public Model_FEM_GH()
          : base("FEM Model", "FEM",
              "Mesh-based FEM Model",
              "Cocodrilo", "Models")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddMeshParameter("Wall", "Mesh", "DEM Wall", GH_ParamAccess.list);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("FEM Model", "Model", "FEM Model", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Mesh> mesh_list = new List<Mesh>();
            DA.GetDataList(0, mesh_list);

            var output_fem = new OutputKratosFEM();
            output_fem.StartAnalysis(mesh_list);

            DA.SetData(0, output_fem);
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
            get { return new Guid("7A9782AB-F79D-466F-8F8F-90B61E0A1CC7"); }
        }
    }
}