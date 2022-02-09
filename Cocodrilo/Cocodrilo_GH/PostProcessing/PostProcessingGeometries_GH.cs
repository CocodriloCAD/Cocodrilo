using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PostProcessing
{
    public class PostProcessingGeometries_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PostProcessingGeometries class.
        /// </summary>
        public PostProcessingGeometries_GH()
          : base("PostProcessingGeometries", "PostProcessingGeometries",
              "Extracts the Post Processing Geometries",
              "Cocodrilo", "PostProcessing")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Post Processing", "Post", "PostProcessing of Apparent Solution", GH_ParamAccess.item);
            pManager.AddNumberParameter("Displacement Scaling", "Scaling", "Scaling Factor for the Apparent Solution", GH_ParamAccess.item, 1.0);
            pManager.AddNumberParameter("Flying Node Limit", "FlyingNodeLimit", "Flying Node Limit for the Apparent Solution", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Breps", "Breps", "Breps of the current solution.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Cocodrilo.PostProcessing.PostProcessing post_processing = null;
            if (!DA.GetData(0, ref post_processing)) return;

            double displacement_scaling = 1.0;
            if (!DA.GetData(1, ref displacement_scaling)) return;

            double flying_node_limit = 1.0e6;
            if (!DA.GetData(2, ref flying_node_limit)) return;

            var geometries = post_processing.GetDeformedGeometries(displacement_scaling, flying_node_limit, post_processing.CurrentDisplacements());

            var breps = geometries.Values.ToList();

            DA.SetDataList(0, breps);
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
            get { return new Guid("633ADFAB-F920-458A-9292-18BFF28531AD"); }
        }
    }
}