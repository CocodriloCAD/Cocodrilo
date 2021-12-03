using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Cocodrilo.IO;

namespace Cocodrilo_GH.PreProcessing.IO
{
    public class Model_DEM_GH : GH_Component
    {
        public List<Point> mPointList = new List<Point>();

        /// <summary>
        /// Initializes a new instance of the Model_DEM_GH class.
        /// </summary>
        public Model_DEM_GH()
          : base("Model DEM", "Model DEM",
              "Entire DEM model",
              "Cocodrilo", "DEM")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometries", "Geoms", "DEM particles wrapped within geometries", GH_ParamAccess.list);
            pManager.AddMeshParameter("Wall", "Mesh", "DEM Wall", GH_ParamAccess.list);
            pManager[1].Optional = true;
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
            List<Geometries.Geometries> geometries_list = new List<Geometries.Geometries>();
            DA.GetDataList(0, geometries_list);

            List<Mesh> mesh_list = new List<Mesh>();
            DA.GetDataList(1, mesh_list);

            List<Point> point_list = new List<Point>();
            foreach(var geom in geometries_list)
            {
                foreach (var point in geom.points)
                {
                    Cocodrilo.CocodriloPlugIn.Instance.AddProperty(point.Value);
                    point_list.Add(point.Key);
                }
            }

            var output_dem = new OutputKratosDEM();
            output_dem.StartAnalysis(point_list, mesh_list);

            mPointList = point_list;
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

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            if (Hidden) return;
            if (Locked) return;

            if (true)
            {
                Cocodrilo.Visualizer.Visualizer.DrawDemElements(args.Display, mPointList);
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("A86E7BE6-E37D-466F-9644-E383BF3F604E"); }
        }
    }
}