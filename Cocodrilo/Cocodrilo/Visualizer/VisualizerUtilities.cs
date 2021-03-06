using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rhino.Geometry;

namespace Cocodrilo.Visualizer
{
    public static class VisualizerUtilities
    {
        public static Color SupportColor = Color.FromArgb(0, 101, 189);
        public static Color LoadColor = Color.FromArgb(0, 124, 48);
        public static Color ShellColor = Color.FromArgb(0, 51, 89);
        public static Color MembraneColor = Color.FromArgb(100, 160, 200);
        public static Color CableColor = Color.FromArgb(249, 186, 0);
        public static Color BeamColor = Color.FromArgb(227, 114, 34);
        public static Color ConnectColor = Color.FromArgb(0, 124, 48);

        public static Point3d[] GetCurvePoints(Curve ThisCurve)
        {
            var divide = Math.Max(3, Convert.ToInt32(ThisCurve.GetLength() * 1.5));
            ThisCurve.DivideByCount(divide, true, out var ptsDiff);

            return ptsDiff;
        }

        public static List<Point3d> SurfaceDivide(BrepFace Face)
        {
            var points = new List<Point3d>();
            Face.GetSurfaceSize(out double width, out double height);

            var divide_u = Math.Max(5, Convert.ToInt32(width / 1.5));
            var divide_v = Math.Max(5, Convert.ToInt32(height / 1.5));

            var u_t0 = Face.Domain(0).T0;
            var length_u = Face.Domain(0).Length;

            var v_t0 = Face.Domain(1).T0;
            var length_v = Face.Domain(1).Length;

            for (var i = 0; i <= divide_u; i++)
            {
                for (var j = 0; j <= divide_v; j++)
                {
                    var u = u_t0 + i * (length_u / divide_u);
                    var v = v_t0 + j * (length_v / divide_v);
                    var point_face_direction = Face.IsPointOnFace(u, v);
                    if (PointFaceRelation.Exterior != point_face_direction)
                    {
                        points.Add(Face.PointAt(u, v));
                    }
                }
            }

            return points;
        }


        public static List<Tuple<Point3d, Vector3d>> SurfaceDividePressure(Brep Brep)
        {
            var Points_with_normal = new List<Tuple<Point3d, Vector3d>>();
            var PointsOnSrf = new List<Point3d>();
            var area = AreaMassProperties.Compute(Brep).Area;

            var U = 5;
            //if (area > 100)
            //    U = 20;
            //else if (area > 500)
            //    U = 50;

            var V = U;

            foreach (var Srf in Brep.Surfaces)
            {

                var _uT0 = Srf.Domain(0).T0;
                var _length_u = Srf.Domain(0).Length;

                var _vT0 = Srf.Domain(1).T0;
                var _length_v = Srf.Domain(1).Length;

                var PointsAll = new List<Point3d>();

                for (var i = 0; i <= U; i++)
                {
                    for (var j = 0; j <= V; j++)
                    {
                        var _u = _uT0 + i * (_length_u / U);
                        var _v = _vT0 + j * (_length_v / V);
                        PointsAll.Add(new Point3d(_u, _v, 0));
                    }
                }

                foreach (var pt in PointsAll)
                {
                    var ptTest = Srf.PointAt(pt.X, pt.Y);

                    double u, v;
                    Srf.ClosestPoint(ptTest, out u, out v);
                    var ptSrf = Srf.PointAt(u, v);
                    var ptBrep = Brep.ClosestPoint(ptTest);
                    if (ptBrep.DistanceTo(ptSrf) < 0.0001)
                    {
                        Vector3d normal = Srf.NormalAt(u, v);
                        Tuple<Point3d, Vector3d> pt_w_n = new Tuple<Point3d, Vector3d>(ptTest, normal);
                        Points_with_normal.Add(pt_w_n);
                    }
                }
            }

            return Points_with_normal;
        }

        public static List<Tuple<Point3d, Vector3d>> SurfaceDivideSnow(Brep Brep, Vector3d ld_dir)
        {
            var Points_with_normal = new List<Tuple<Point3d, Vector3d>>();
            var PointsOnSrf = new List<Point3d>();
            var area = AreaMassProperties.Compute(Brep).Area;

            var U = 5;
            //if (area > 100)
            //    U = 20;
            //else if (area > 500)
            //    U = 50;

            var V = U;

            foreach (var Srf in Brep.Surfaces)
            {

                var _uT0 = Srf.Domain(0).T0;
                var _length_u = Srf.Domain(0).Length;

                var _vT0 = Srf.Domain(1).T0;
                var _length_v = Srf.Domain(1).Length;

                var PointsAll = new List<Point3d>();

                for (var i = 0; i <= U; i++)
                {
                    for (var j = 0; j <= V; j++)
                    {
                        var _u = _uT0 + i * (_length_u / U);
                        var _v = _vT0 + j * (_length_v / V);
                        PointsAll.Add(new Point3d(_u, _v, 0));
                    }
                }

                foreach (var pt in PointsAll)
                {
                    var ptTest = Srf.PointAt(pt.X, pt.Y);

                    double u, v;
                    Srf.ClosestPoint(ptTest, out u, out v);
                    var ptSrf = Srf.PointAt(u, v);
                    var ptBrep = Brep.ClosestPoint(ptTest);
                    if (ptBrep.DistanceTo(ptSrf) < 0.0001)
                    {
                        Vector3d normal = Srf.NormalAt(u, v);
                        normal.Unitize();
                        Vector3d ld_dir_unit = new Vector3d(ld_dir);
                        ld_dir_unit.Unitize();
                        double fac = ld_dir_unit * normal;
                        if (fac < 0)
                            fac = -fac;
                        Tuple<Point3d, Vector3d> pt_w_n = new Tuple<Point3d, Vector3d>(ptTest, fac * ld_dir);
                        Points_with_normal.Add(pt_w_n);
                    }
                }
            }

            return Points_with_normal;
        }
    }
}
