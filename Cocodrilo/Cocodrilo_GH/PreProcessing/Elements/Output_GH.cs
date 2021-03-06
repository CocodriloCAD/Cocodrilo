using System;
using System.Collections.Generic;

using Cocodrilo.ElementProperties;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.Elements
{
    public class Output_GH : GH_Component
    {
        public Output_GH()
          : base("Output", "Output",
              "Output Condition",
              "Cocodrilo", "Elements")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Surfaces", "Sur", "Geometries", GH_ParamAccess.list);
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
            List<Brep> breps = new List<Brep>();
            DA.GetDataList(0, breps);

            List<Curve> curves = new List<Curve>();
            DA.GetDataList(1, curves);

            List<Curve> edges = new List<Curve>();
            DA.GetDataList(2, edges);

            List<Point3d> points = new List<Point3d>();
            DA.GetDataList(3, points);

            var geometries = new Geometries.Geometries();

            var check_properties = new CheckProperties(true, true, true, true);
            foreach (var brep in breps)
            {
                var check_property = new PropertyCheck(
                    GeometryType.GeometrySurface, check_properties, false, new TimeInterval());

                geometries.breps.Add(new KeyValuePair<Brep, Property>(brep, check_property));
            }

            foreach (var curve in curves)
            {
                var check_property = new Cocodrilo.ElementProperties.PropertyCheck(
                    GeometryType.GeometryCurve, check_properties, false, new TimeInterval());

                geometries.curves.Add(new KeyValuePair<Curve, Property>(curve, check_property));
            }

            foreach (var edge in edges)
            {
                var check_property = new Cocodrilo.ElementProperties.PropertyCheck(
                    GeometryType.SurfaceEdge, check_properties, false, new TimeInterval());

                geometries.edges.Add(new KeyValuePair<Curve, Property>(edge, check_property));
            }

            foreach (var point in points)
            {
                var check_property = new PropertyCheck(
                    GeometryType.Point, check_properties, true, new TimeInterval());

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
            get { return Properties.Resources.elements_check; }
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