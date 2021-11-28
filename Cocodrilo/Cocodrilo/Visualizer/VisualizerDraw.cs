using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Display;
using Rhino.Geometry;
using Cocodrilo.ElementProperties;
namespace Cocodrilo.Visualizer
{
    using VU = VisualizerUtilities;

    public static class VisualizerDraw
    {
        #region Supports

        public static void drawSupport(DisplayPipeline d, Point3d supportPoint, Support ThisSupport)
        {
            var height = 0.5;
            var radius = 0.25;


            if (ThisSupport.mSupportX)
            {
                var xPlane = new Plane(supportPoint, new Vector3d(-1, 0, 0));
                d.DrawCone(new Cone(xPlane, height, radius), VU.SupportColor);
            }

            if (ThisSupport.mSupportY)
            {
                var yPlane = new Plane(supportPoint, new Vector3d(0, -1, 0));
                d.DrawCone(new Cone(yPlane, height, radius), VU.SupportColor);
            }

            if (ThisSupport.mSupportZ)
            {
                var zPlane = new Plane(supportPoint, new Vector3d(0, 0, -1));
                d.DrawCone(new Cone(zPlane, height, radius), VU.SupportColor);
            }

            if (ThisSupport.mSupportRotation)
            {
                var zPlane = new Plane(supportPoint, new Vector3d(0, 0, -1));
                Interval xInterval = new Interval(-radius, radius);
                Interval yInterval = new Interval(-radius, radius);
                Interval zInterval = new Interval(-radius, radius);
                d.DrawBox(
                    new Box(zPlane, xInterval, yInterval, zInterval),
                    VU.SupportColor);
            }
        }

        public static void drawPointSupport(DisplayPipeline d, Point3d supportPoint, Support ThisSupport)
        {
            var scale = 1.3;
            var height = 0.5 * scale;
            var radius = 0.25 * scale;

            if (ThisSupport.mSupportX)
            {
                var xPlane = new Plane(supportPoint, new Vector3d(-1, 0, 0));
                d.DrawCone(new Cone(xPlane, height, radius), VU.SupportColor);
                //var co = new Cone(xPlane, height, radius);
                //var sphere = RhinoDoc.ActiveDoc.Objects.AddBrep(co.ToBrep(true));
            }

            if (ThisSupport.mSupportY)
            {
                var yPlane = new Plane(supportPoint, new Vector3d(0, -1, 0));
                d.DrawCone(new Cone(yPlane, height, radius), VU.SupportColor);
                //var co = new Cone(yPlane, height, radius);
                //var sphere = RhinoDoc.ActiveDoc.Objects.AddBrep(co.ToBrep(true));
            }

            if (ThisSupport.mSupportZ)
            {
                var zPlane = new Plane(supportPoint, new Vector3d(0, 0, -1));
                d.DrawCone(new Cone(zPlane, height, radius), VU.SupportColor);
                //var co = new Cone(zPlane, height, radius);
                //var sphere = RhinoDoc.ActiveDoc.Objects.AddBrep(co.ToBrep(true));
            }

            if (ThisSupport.mSupportRotation)
            {
                var zPlane = new Plane(supportPoint, new Vector3d(0, 0, -1));
                Interval xInterval = new Interval(-radius, radius);
                Interval yInterval = new Interval(-radius, radius);
                Interval zInterval = new Interval(-radius, radius);
                d.DrawBox(new Box(zPlane, xInterval, yInterval, zInterval), VU.SupportColor);
            }
        }

        public static void drawCurveSupport(DisplayPipeline d, Curve Crv, Support ThisSupport)
        {
            foreach (var pt in VisualizerUtilities.GetCurvePoints(Crv))
                drawPointSupport(d, pt, ThisSupport);
        }



        public static void drawSurfaceSupport(DisplayPipeline d, Surface surface, Support ThisSupport)
        {
            var bb = surface.GetBoundingBox(false);

            var ptList = VU.SurfaceDivide(surface);
            foreach (var pt in ptList)
                drawPointSupport(d, pt, ThisSupport);
        }

        public static void drawPointConnect(DisplayPipeline d, Point3d CctPoint1, Point3d CctPoint2, bool disp = true,
            bool rotation = false)
        {
            var scale = 1.3;
            var height = 0.5 * scale;
            var radius = 0.25 * scale;

            if (disp)
            {
                d.DrawSphere(new Sphere(CctPoint1, radius / 2), VU.ConnectColor);
                d.DrawSphere(new Sphere(CctPoint2, radius / 2), VU.ConnectColor);
                d.DrawLine(new Line(CctPoint1, CctPoint2), VU.ConnectColor);
            }
        }

        #endregion

        #region Elements

        public static void drawCable(DisplayPipeline d, Curve curve)
        {
            d.DrawCurve(curve, VU.CableColor, 2);
        }

        public static void drawSurface(DisplayPipeline d, Surface surface)
        {
            d.DrawSurface(surface, Color.MediumTurquoise, 3);
        }

        #endregion

        #region Loads

        public static void drawPointLoad(DisplayPipeline d, Point3d Point, Load ThisLoad)
        {
            Vector3d loadVec = ThisLoad.GetLoadVector();
            var dispLine = new Line(Point - loadVec, loadVec);
            d.DrawArrow(dispLine, VU.LoadColor, 0.0, 0.2);
        }

        public static void drawPointLoad(DisplayPipeline d, Point3d Point, double LoadX, double LoadY, double LoadZ)
        {
            Vector3d loadVec = new Vector3d(LoadX, LoadY, LoadZ);
            var dispLine = new Line(Point - loadVec, loadVec);
            d.DrawArrow(dispLine, VU.LoadColor, 0.0, 0.2);
        }

        public static void drawCurveLoad(DisplayPipeline d, Curve Crv, Load ThisLoad)
        {
            Point3d[] ptsDiff;
            var count = Convert.ToInt32(Crv.GetLength() * 1.5);
            if (count == 0)
                count = 5;

            double[] ptsDiffpar = Crv.DivideByCount(count, true, out ptsDiff);

            foreach (var pt in ptsDiff.ToList())
                drawPointLoad(d, pt, ThisLoad);
        }

        public static void drawSurfaceLoad(DisplayPipeline d, Surface surface, Load ThisLoad)
        {
            var bb = surface.GetBoundingBox(false);

            var ptList = VU.SurfaceDivide(surface);
            foreach (var pt in ptList)
                drawPointLoad(d, pt, ThisLoad);
        }

        public static void drawSurfacePressureLoad(DisplayPipeline d, Surface surface, double value)
        {
            var bb = surface.GetBoundingBox(false);

            var ptList = VU.SurfaceDividePressure(surface.ToBrep());
            foreach (var pt in ptList)
                drawPointLoad(d, pt.Item1, pt.Item2.X * value, pt.Item2.Y * value, pt.Item2.Z * value);
        }

        public static void drawSurfaceSnowLoad(DisplayPipeline d, Surface surface, double loadX, double loadY,
            double loadZ)
        {
            var bb = surface.GetBoundingBox(false);
            Vector3d direction = new Vector3d(loadX, loadY, loadZ);

            var ptList = VU.SurfaceDivideSnow(surface.ToBrep(), direction);
            foreach (var pt in ptList)
                drawPointLoad(d, pt.Item1, pt.Item2.X, pt.Item2.Y, pt.Item2.Z);
        }

        public static void drawLoad(DisplayPipeline d, Point3d supportPoint, Load ThisLoad)
        {
            Line line = new Line(supportPoint, new Vector3d(ThisLoad.GetLoadVector()));
            d.DrawArrow(line, VU.LoadColor, 0.0, 0.4);
        }

        #endregion
    }
}
