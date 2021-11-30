using System;
using System.Collections.Generic;

using Cocodrilo.ElementProperties;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.Elements
{
    public class Output_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the Output_GH class.
        /// </summary>
        public Output_GH()
          : base("Output", "Output",
              "Output Condition",
              "Cocodrilo", "Elements")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surfaces", "Sur", "Geometries", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddCurveParameter("Curves", "Cur", "Geometries", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddCurveParameter("Edges", "Edg", "Geometries", GH_ParamAccess.list);
            pManager[2].Optional = true;
            pManager.AddPointParameter("Points", "Pts", "Geometries", GH_ParamAccess.list);
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometries", "Geo", "Geometries enhanced with load information.", GH_ParamAccess.item);
        }
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Surface> surfaces = new List<Surface>();
            DA.GetDataList(0, surfaces);

            List<Curve> curves = new List<Curve>();
            DA.GetDataList(1, curves);

            List<Curve> edges = new List<Curve>();
            DA.GetDataList(2, edges);

            List<Point3d> points = new List<Point3d>();
            DA.GetDataList(3, points);

            var geometries = new Cocodrilo_GH.PreProcessing.Geometries.Geometries();

            var check_properties = new Cocodrilo.ElementProperties.CheckProperties(true, true, true, true);
            foreach (var surface in surfaces)
            {
                var check_property = new Cocodrilo.ElementProperties.PropertyCheck(
                    GeometryType.GeometrySurface, check_properties, false, new Cocodrilo.ElementProperties.TimeInterval());

                geometries.breps.Add(new KeyValuePair<Brep, Property>(surface.ToBrep(), check_property));
            }

            foreach (var curve in curves)
            {
                var check_property = new Cocodrilo.ElementProperties.PropertyCheck(
                    GeometryType.GeometryCurve, check_properties, false, new Cocodrilo.ElementProperties.TimeInterval());

                geometries.curves.Add(new KeyValuePair<Curve, Property>(curve, check_property));
            }

            foreach (var edge in edges)
            {
                var check_property = new Cocodrilo.ElementProperties.PropertyCheck(
                    GeometryType.SurfaceEdge, check_properties, false, new Cocodrilo.ElementProperties.TimeInterval());

                geometries.edges.Add(new KeyValuePair<Curve, Property>(edge, check_property));
            }

            foreach (var point in points)
            {
                var check_property = new Cocodrilo.ElementProperties.PropertyCheck(
                    GeometryType.Point, check_properties, false, new Cocodrilo.ElementProperties.TimeInterval());

                Point new_point = new Point(point);

                geometries.points.Add(new KeyValuePair<Point, Property>(new_point, check_property));
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
            get { return new Guid("7509A4B1-FBBF-4780-95E9-9287745A4E88"); }
        }
    }
}