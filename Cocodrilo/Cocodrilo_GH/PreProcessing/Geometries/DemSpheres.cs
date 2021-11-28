using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Cocodrilo.ElementProperties;
using Cocodrilo.Materials;

namespace Cocodrilo_GH.PreProcessing.Geometries
{
    public class DemSpheres : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the DemSpheres class.
        /// </summary>
        public DemSpheres()
          : base("DemSpheres", "DemSpheres",
              "Description",
              "Cocodrilo", "DEM")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddPointParameter("Points", "Points", "DEM particles", GH_ParamAccess.list);
            pManager.AddNumberParameter("Radius", "Radius", "Radius of DEM particles", GH_ParamAccess.item);
            pManager.AddGenericParameter("Material", "Material", "Material of DEM particles", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometries", "Geo", "Geometries Enhanced with Element Formulations", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Point3d> point_3d_list = new List<Point3d>();
            if (!DA.GetDataList(0, point_3d_list)) return;

            double radius = 0.0;
            if (!DA.GetData(1, ref radius)) return;

            Material material = null;
            if (!DA.GetData(2, ref material)) return;

            var geometries = new Cocodrilo_GH.PreProcessing.Geometries.Geometries();
            foreach (var point_3d in point_3d_list)
            {
                var geometry_point = new Point(point_3d);
                geometry_point.UserDictionary.Set("RADIUS", radius);
                geometries.points.Add(new KeyValuePair<Point, Property>(geometry_point, new PropertyDem(GeometryType.Point, material.Id)));
            }

            DA.SetData(0, geometries);
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
            get { return new Guid("8C62B5BD-4C91-4606-AB28-D93BD7993A2F"); }
        }
    }
}