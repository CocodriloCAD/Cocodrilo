using System;
using System.Collections.Generic;
using System.Linq;
using Grasshopper.Kernel;
using Rhino.Geometry;

using Cocodrilo.PostProcessing;

namespace Cocodrilo_GH.PostProcessing
{
    public class PostProcessingGeometryMeshes_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the PostProcessingGeometries class.
        /// </summary>
        public PostProcessingGeometryMeshes_GH()
          : base("PostProcessingGeometryMeshes", "PostProcessingGeometryMeshes",
              "Ceraets meshes on the post processing geometries",
              "Cocodrilo", "PostProcessing")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Post-Processing", "P", "Post-processing of apparent solution.", GH_ParamAccess.item);
            pManager.AddBooleanParameter("Consider Edges", "CE", "Consider the edges of the brep faces for the meshes.", GH_ParamAccess.item, false);
            pManager.AddNumberParameter("Max Edge Length", "ML", "Maximum edge length on the mesh boundary.", GH_ParamAccess.item, 0.1);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddMeshParameter("Meshes", "Meshs", "Meshes of the current solution.", GH_ParamAccess.list);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Cocodrilo.PostProcessing.PostProcessing post_processing = null;
            if (!DA.GetData(0, ref post_processing)) return;

            bool consider_edges = false;
            if (!DA.GetData(1, ref consider_edges)) return;

            double max_edge_length = 0.1;
            if (!DA.GetData(2, ref max_edge_length)) return;

            var geometries = post_processing.BrepId_Breps;

            var breps = geometries.Values.ToList();
            List<BrepFace> brep_faces = new List<BrepFace>();
            foreach (var brep in breps)
            {
                brep_faces.AddRange(brep.Faces);
            }

            var meshes = brep_faces.Select(item => PostProcessingUtilities.CreatePostProcessingMesh(
                item, post_processing.mEvaluationPointList, consider_edges, max_edge_length)).ToList();

            DA.SetDataList(0, meshes);
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
            get { return new Guid("8A723182-A23C-4509-9CC6-858A96612A51"); }
        }
    }
}