using Rhino;
using Rhino.Geometry;
using Rhino.Geometry.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cocodrilo.ElementProperties;

namespace Cocodrilo.IO
{
    using Dict = Dictionary<string, object>;
    using DictList = List<Dictionary<string, object>>;

    public static class OutputUtilitiesJSON
    {
        /// <summary>
        /// Create a knot vector from a given Rhino KnotList, with multiplicity
        /// of the first and the last last knot of p+1 times.
        /// </summary>
        /// <param name="knot_list"></param>
        /// <param name="is_periodic">Flag if periodic. Periodic geometries have repition of first and last knot of p times (not p+1)</param>
        /// <returns></returns>
        public static List<double> CreateKnotVector(NurbsSurfaceKnotList knot_list, bool is_periodic)
        {
            var knot_vector = new List<double>();
            if (!is_periodic)
                knot_vector.Add(knot_list[0]);
            for (int i = 0; i < knot_list.Count(); i++)
            {
                knot_vector.Add(knot_list[i]);
            }
            if (!is_periodic)
                knot_vector.Add(knot_list[knot_list.Count() - 1]);

            return knot_vector;
        }

        /// <summary>
        /// Create a knot vector from a given Rhino KnotList, with multiplicity
        /// of the first and the last last knot of p+1 times.
        /// </summary>
        /// <param name="knot_list"></param>
        /// <param name="is_periodic">Flag if periodic. Periodic geometries have repition of first and last knot of p times (not p+1)</param>
        /// <returns></returns>
        public static List<double> CreateCurveKnotVector(NurbsCurveKnotList knot_list, bool is_periodic)
        {
            var knot_vector = new List<double>();
            if (!is_periodic)
                knot_vector.Add(knot_list[0]);
            for (int i = 0; i < knot_list.Count(); i++)
            {
                knot_vector.Add(knot_list[i]);
            }
            if (!is_periodic)
                knot_vector.Add(knot_list[knot_list.Count() - 1]);

            return knot_vector;
        }

        public static List<List<object>> CreateSurfaceControlPointList(NurbsSurface ThisNurbsSurface, ref int rCpId)
        {
            var ControlPoints = new List<List<object>>();
            for (int v = 0; v < ThisNurbsSurface.Points.CountV; v++)
            {
                for (int u = 0; u < ThisNurbsSurface.Points.CountU; u++)
                {
                    // new control point is added
                    rCpId++;
                    var point = ThisNurbsSurface.Points.GetControlPoint(u, v);
                    ControlPoints.Add(new List<object>
                    {
                        rCpId,
                        new List<double> {point.Location.X, point.Location.Y, point.Location.Z, point.Weight}
                    });
                }
            }
            return ControlPoints;
        }

        /// <summary>
        /// Returns the loop type as string. The common types ar "outer" and "inner".
        /// </summary>
        /// <param name="loop">Investigated loop.</param>
        /// <returns></returns>
        public static string CreateLoopType(BrepLoop loop)
        {
            switch ((int)loop.LoopType)
            {
                case 0:
                    return "unknown";
                case 1:
                    return "outer";
                //  1   2d loop curves form a simple closed curve with a counterclockwise orientation.
                case 2:
                    return "inner";
                //   2   2d loop curves form a simple closed curve with a clockwise orientation.
                case 3:
                    return "slit"; //    3   Always closed -used internally during splitting operations
                case 4:
                    return "CurveOnSurface";
                // "loop" is a curveonsrf made from a single (open or closed) trim that has type TrimType.CurveOnSurface.
                case 5:
                    return "PointOnSurface";
                    //  5   "loop" is a PointOnSurface made from a single trim that has type TrimType.PointOnSurface.
            }
            return "unknown";
        }

        public static Dict CreateVertexDictionary(
            int BrepId,
            Coupling ThisCouplingTopology,
            Point3d global_location,
            Point2d local_location)
        {
            var topology = new DictList();
            foreach (var top in ThisCouplingTopology.mTopologies)
            {
                if (top.mTrimIndex == -1)
                    RhinoApp.WriteLine("ERROR: Trim index kept with -1");

                topology.Add(new Dict()
                {
                    {"brep_id", top.mBrepId},
                    {"local_coordinates", new double[] { local_location.X, local_location.Y, 0.0}}
                });
            }

            return new Dict()
            {
                {"brep_id", BrepId},
                {"topology", topology}
            };
        }

        public static Dict CreateVertexDictionary(
            int BrepId,
            Coupling ThisCouplingTopology,
            double local_location)
        {
            var topology = new DictList();
            foreach (var top in ThisCouplingTopology.mTopologies)
            {
                if (top.mTrimIndex == -1)
                    RhinoApp.WriteLine("ERROR: Trim index kept with -1");

                topology.Add(new Dict()
                {
                    {"brep_id", top.mBrepId},
                    {"local_coordinates", new double[] { local_location, 0.0, 0.0}}
                });
            }

            return new Dict()
            {
                {"brep_id", BrepId},
                {"topology", topology}
            };
        }

        public static Dict CreateVertexDictionary(
            int BrepId,
            Coupling ThisCouplingTopology,
            ref int CP_Counter,
            Point3d global_location)
        {
            CP_Counter++;

            var global_location_point = new object[]
            {
                CP_Counter,
                new double[] { global_location.X, global_location.Y, global_location.Z, 1.0}
            };

            var topology = new DictList();
            for (int i = 0; i < ThisCouplingTopology.mTopologies.Count(); i++)
            {
                if (ThisCouplingTopology.mTopologies[i] is TopologyLocalPoint)
                {
                    TopologyLocalPoint top_local_point = (TopologyLocalPoint)ThisCouplingTopology.mTopologies[i];
                    topology.Add(top_local_point.GetInputDictionary());
                }
            }

            return new Dict()
            {
                {"brep_id", BrepId},
                {"3d_point", global_location_point},
                {"topology", topology}
            };
        }

        public static Dict CreateCurveDictionary(
            int BrepId, Coupling ThisCouplingTopology, ref int CP_Counter, Curve ThisCurve)
        {
            var nurbs_curve = ThisCurve.ToNurbsCurve();

            var CurveControlPoints = new List<List<object>>();
            foreach (ControlPoint point in nurbs_curve.Points)
            {
                CP_Counter++;
                CurveControlPoints.Add(new List<object>
                    {
                        CP_Counter,
                        new List<double> {point.Location.X, point.Location.Y, point.Location.Z, point.Weight}
                    }
                );
            }

            var knot_vector = CreateCurveKnotVector(nurbs_curve.Knots, nurbs_curve.IsPeriodic);

            var CurveDict = new Dict
            {
                {"degree", nurbs_curve.Degree},
                {"knot_vector", knot_vector},
                {"active_range", new List<double>{knot_vector[0], knot_vector[knot_vector.Count-1] } },
                {"control_points", CurveControlPoints}
            };

            var topology = new DictList();
            foreach (var top in ThisCouplingTopology.mTopologies)
            {
                if (top.mTrimIndex == -1)
                    RhinoApp.WriteLine("ERROR: Trim index kept with -1");
                topology.Add(new Dict()
                {
                    {"brep_id", top.mBrepId},
                    {"trim_index", top.mTrimIndex},
                    {"relative_direction", true}
                });
            }

            return new Dict
            {
                {"brep_id", BrepId},
                {"3d_curve", CurveDict},
                {"topology", topology}
            };
        }
        public static Dict CreateEdgeDictionary(
            int BrepId, Coupling ThisCouplingTopology, ref int CP_Counter)
        {
            var topology = new DictList();
            foreach (var top in ThisCouplingTopology.mTopologies)
            {
                if (top.mTrimIndex == -1)
                    RhinoApp.WriteLine("ERROR: Trim index kept with -1");
                topology.Add(new Dict()
                {
                    {"brep_id", top.mBrepId},
                    {"trim_index", top.mTrimIndex},
                    {"relative_direction", true}
                });
            }

            return new Dict
            {
                {"brep_id", BrepId},
                {"topology", topology}
            };
        }
        public static Dict CreateEmbeddedCurveDictionary(
            int TrimIndex, ref int CP_Counter, Curve ThisCurve, bool is_reversed = false)
        {
            if (ThisCurve == null) {
                return new Dict { };
            }
            var nurbs_curve = ThisCurve.ToNurbsCurve();

            var knot_vector = CreateCurveKnotVector(nurbs_curve.Knots, nurbs_curve.IsPeriodic);

            var CurveControlPointsParameter = new List<List<object>>();
            foreach (ControlPoint point in nurbs_curve.Points)
            {
                CP_Counter++;
                CurveControlPointsParameter.Add(new List<object>
                {
                    CP_Counter,
                    new List<double> { point.Location.X, point.Location.Y, point.Location.Z, point.Weight }
                });
            }

            var active_range = new List<double>
            {
                nurbs_curve.Knots.First(),
                nurbs_curve.Knots.Last()
            };

            var parameter_curve = new Dict
            {
                {"is_rational", nurbs_curve.IsRational},
                {"degree", nurbs_curve.Degree},
                {"knot_vector", knot_vector},
                {"active_range", active_range},
                {"control_points", CurveControlPointsParameter}
            };

            return new Dict
            {
                {"trim_index", TrimIndex},
                {"curve_direction", !is_reversed}, //!trim.IsReversed()} ,
                {"parameter_curve", parameter_curve}
            };
        }
    }
}
