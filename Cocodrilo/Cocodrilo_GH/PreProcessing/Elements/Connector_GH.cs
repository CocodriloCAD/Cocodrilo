using System;
using System.Collections.Generic;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using Rhino.Geometry;

using Cocodrilo.ElementProperties;

namespace Cocodrilo_GH.PreProcessing.Elements
{
    public class Connector_GH : GH_Component
    {
        public Connector_GH()
          : base("Connector", "Connector", "Connector which models phyiscal properties in the interface.", "Cocodrilo", "Elements")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Edges", "Edg", "Geometries", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddPointParameter("Points", "Pts", "Geometries", GH_ParamAccess.list);
            pManager[1].Optional = true;

            pManager.AddNumberParameter("EA/l", "N", "Stiffness in normal direction.", GH_ParamAccess.item, 1.0);
            pManager.AddNumberParameter("EG/l", "T1", "Stiffness in tangent 1 direction.", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("EG/l", "T2", "Stiffness in tangent 2 direction (this is the direction of the boundary curve).", GH_ParamAccess.item, 1.0);
            pManager.AddNumberParameter("EI/l", "M1", "Stiffness moment in tangent 1 direction.", GH_ParamAccess.item, 0.0);
            pManager.AddNumberParameter("EI/l", "M2", "Stiffness moment in tangent 2 direction (this is the direction of the boundary curve).", GH_ParamAccess.item, 0.0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometries", "Geo", "Geometries enhanced with support information.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Curve> edges = new List<Curve>();
            DA.GetDataList(0, edges);

            List<Point3d> points = new List<Point3d>();
            DA.GetDataList(1, points);

            double n = 0;
            DA.GetData(2, ref n);
            double t1 = 0;
            DA.GetData(3, ref t1);
            double t2 = 0;
            DA.GetData(4, ref t2);
            double m1 = 0;
            DA.GetData(5, ref m1);
            double m2 = 0;
            DA.GetData(6, ref m2);

            var geometries = new Cocodrilo_GH.PreProcessing.Geometries.Geometries();
            var connector = new ConnectorProperties(n, t1, t2, m1, m2);

            foreach (var edge in edges)
            {
                var connector_property = new PropertyConnector(
                    GeometryType.Point, connector, new TimeInterval());

                geometries.edges.Add(new KeyValuePair<Curve, Property>(
                    edge, connector_property));
            }

            foreach (var point in points)
            {
                var connector_property = new PropertyConnector(
                    GeometryType.Point, connector, new TimeInterval());

                Point new_point = new Point(point);

                geometries.points.Add(new KeyValuePair<Point, Property>(new_point, connector_property));
            }

            DA.SetData(0, geometries);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.elements_support; }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("E916C51F-0FD2-41EB-A24A-7A79593B307E"); }
        }
    }
}