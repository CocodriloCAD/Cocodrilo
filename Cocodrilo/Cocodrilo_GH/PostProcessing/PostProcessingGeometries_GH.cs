using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
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
            pManager.AddGenericParameter("Post Processing", "P", "Post-processing of apparent solution", GH_ParamAccess.item);
            pManager.AddGenericParameter("Results", "R", "Control point results.", GH_ParamAccess.item);
            pManager.AddNumberParameter("Displacement Scaling", "S", "Scaling factor for the apparent solution", GH_ParamAccess.item, 1.0);
            pManager.AddNumberParameter("Flying Node Limit", "F", "Flying node limit for the apparent solution", GH_ParamAccess.item, 1.0e4);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Breps", "B", "Breps of the current solution", GH_ParamAccess.tree);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Cocodrilo.PostProcessing.PostProcessing post_processing = null;
            if (!DA.GetData(0, ref post_processing)) return;

            Dictionary<int, double[]> results = new Dictionary<int, double[]>();
            bool results_received = DA.GetData(1, ref results);

            double displacement_scaling = 1.0;
            if (!DA.GetData(2, ref displacement_scaling)) return;

            double flying_node_limit = 1.0e6;
            if (!DA.GetData(3, ref flying_node_limit)) return;

            var geometries = (results_received)
                ? post_processing.GetDeformedGeometries(displacement_scaling, flying_node_limit, results)
                : post_processing.BrepId_Breps;

            Grasshopper.DataTree<Brep> result_tree = new Grasshopper.DataTree<Brep>();
            foreach (var geometry in geometries)
            {
                Grasshopper.Kernel.Data.GH_Path path = new Grasshopper.Kernel.Data.GH_Path(geometry.Key);
                result_tree.Add(geometry.Value, path);
            }

            DA.SetDataTree(0, result_tree);
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