using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Rhino;
using Rhino.Geometry;
using Rhino.Display;
using Cocodrilo.Elements;
using Cocodrilo.UserData;
using Cocodrilo.ElementProperties;

namespace Cocodrilo.Visualizer
{
    using VD = VisualizerDraw;
    public class Visualizer : Rhino.Display.DisplayConduit
    {
        private List<Surface> surfaces = new List<Surface>();
        public static Color supportColor = Color.FromArgb(0, 101, 189);
        public static Color loadColor = Color.FromArgb(0, 124, 48);
        public static Color shellColor = Color.FromArgb(0, 51, 89);
        public static Color membraneColor = Color.FromArgb(100, 160, 200);
        public static Color cableColor = Color.FromArgb(249, 186, 0);
        public static Color beamColor = Color.FromArgb(227, 114, 34);
        public static Color connectColor = Color.FromArgb(0, 124, 48);
        public bool show_elements = false;
        public bool show_supports = false;
        public bool show_loads = false;
        public bool show_couplings = false;

        protected override void CalculateBoundingBox(CalculateBoundingBoxEventArgs e)
        {
            base.CalculateBoundingBox(e);
            e.BoundingBox.Union(e.Display.Viewport.ConstructionPlane().Origin);
        }

        protected override void PostDrawObjects(DrawEventArgs e)
        {
            base.PostDrawObjects(e);

            e.Display.EnableDepthWriting(false);
            e.Display.EnableDepthTesting(false);

            var active_breps = IO.GeometryUtilities.GetActiveBrepList();
            var active_curves = IO.GeometryUtilities.GetActiveCurveList();

            var intersection_curve_list = CocodriloPlugIn.Instance.IntersectionCurveList;
            var intersection_point_list = CocodriloPlugIn.Instance.IntersectionPointList;

            DrawElements(e.Display,
                active_breps, active_curves, intersection_curve_list, intersection_point_list,
                show_elements, show_supports, show_loads, show_couplings, false);

            e.Display.EnableDepthWriting(false);
            e.Display.EnableDepthTesting(false);
        }

        public static void DrawElements(DisplayPipeline d,
            List<Brep> BrepList,
            List<Curve> CurveList,
            List<Curve> IntersectionCurveList,
            List<Rhino.Geometry.Point> IntersectionPointList,
            bool ShowElements, bool ShowSupports, bool ShowLoads, bool ShowCouplings, bool ShowIds)
        {
            foreach (var active_brep in BrepList)
            {
                for (int index = 0; index < active_brep.Surfaces.Count; index++)
                {
                    var surface = active_brep.Surfaces[index];

                    var user_data_surface = surface.UserData.Find(typeof(UserDataSurface)) as UserDataSurface;
                    if (user_data_surface == null)
                        continue;

                    if (ShowSupports)
                    {
                        // Draw weak surface points.
                        //foreach (var brep_vertex in user_data_surface.GetNumericalElementsOfPropertyType(typeof(PropertySupport)))
                        //{
                        //    var property_vertex_support = brep_vertex.GetProperty<PropertySupport>();
                        //    var point = brep_vertex.GetPoint2d();
                        //    VD.drawPointSupport(d, surface.PointAt(point.X, point.Y),
                        //        property_vertex_support.mSupport);
                        //}

                        // Draw strong surface points.
                        foreach (var support in user_data_surface.GetNumericalElementsOfPropertyType(typeof(PropertySupport)))
                        {
                            if (support.mParameterLocation.IsPoint())
                            {
                                var parameters = support.mParameterLocation.GetParameters();
                                VD.drawPointSupport(d, surface.PointAt(parameters[0], parameters[1]),
                                    support.GetProperty<PropertySupport>().mSupport);
                            }
                            else if (support.mParameterLocation.IsEdge())
                            {
                                Curve curve = null;
                                var nurb_surface = surface.ToNurbsSurface();
                                var parameters = support.mParameterLocation.GetParameters();
                                var normalized_parameters = support.mParameterLocation.GetNormalizedParameters();
                                if (normalized_parameters[0] == 0 || normalized_parameters[0] == 1)
                                {
                                    curve = surface.IsoCurve(1, parameters[0]);
                                }
                                else if (normalized_parameters[1] == 0 || normalized_parameters[1] == 1)
                                {
                                    curve = surface.IsoCurve(0, parameters[1]);
                                }
                                if (curve == null)
                                {
                                    RhinoApp.WriteLine("WARNING: Not able to detect curve of strong support.");
                                    continue;
                                }

                                VD.drawCurveSupport(d, curve,
                                    support.GetProperty<PropertySupport>().mSupport);
                            }
                            else if (support.mParameterLocation.IsSurface())
                            {
                                VD.drawSurfaceSupport(d, surface,
                                    support.GetProperty<PropertySupport>().mSupport);
                            }
                        }
                    }

                    var shell_elements = user_data_surface
                        .GetNumericalElementsOfPropertyType(
                            typeof(PropertyShell));
                    var membrane_elements = user_data_surface
                        .GetNumericalElementsOfPropertyType(
                            typeof(PropertyMembrane));
                    
                    var tmp_srf = surface.ToNurbsSurface();
                    NurbsSurface newsurface = new NurbsSurface(tmp_srf);

                    //var index = surface.ComponentIndex().Index;
                    //var aha = active_brep.Faces[0].UnderlyingSurface;

                    if (ShowElements && (shell_elements.Count > 0 || membrane_elements.Count > 0))
                    {
                        var refinement = user_data_surface.GetRefinement() as Refinement.RefinementSurface;
                        if (refinement != null)
                        {
                            newsurface.IncreaseDegreeU(refinement.PDeg);
                            newsurface.IncreaseDegreeV(refinement.QDeg);
                            if (refinement.KnotInsertType == 0)
                            {
                                int ref_u = refinement.KnotSubDivU + 1;
                                int ref_v = refinement.KnotSubDivV + 1;
                                for (int j = 1; j < ref_u; j++)
                                {
                                    for (int k = 1; k < tmp_srf.KnotsU.Count; k++)
                                    {
                                        if (tmp_srf.KnotsU[k - 1] < tmp_srf.KnotsU[k])//nonzero knot span
                                        {
                                            var knotspansize = tmp_srf.KnotsU[k] - tmp_srf.KnotsU[k - 1];
                                            for (int l = 1; l < ref_u; l++) // dividing in #ref_u elements
                                                newsurface.KnotsU.InsertKnot(tmp_srf.KnotsU[k - 1] + l * knotspansize / ref_u);
                                        }
                                    }
                                }
                                for (int j = 1; j < ref_v; j++)
                                {
                                    for (int k = 1; k < tmp_srf.KnotsV.Count; k++)
                                    {
                                        if (tmp_srf.KnotsV[k - 1] < tmp_srf.KnotsV[k])//nonzero knot span
                                        {
                                            var knotspansize = tmp_srf.KnotsV[k] - tmp_srf.KnotsV[k - 1];
                                            for (int l = 1; l < ref_v; l++) // dividing in #ref_u elements
                                                newsurface.KnotsV.InsertKnot(tmp_srf.KnotsV[k - 1] + l * knotspansize / ref_v);
                                        }
                                    }
                                }
                            }
                        }
                        //var draw_brep = newsurface.ToBrep();
                        var face = active_brep.Faces.First(item => item.SurfaceIndex == index);

                        //if (face != null)
                        //{
                        //    draw_brep = Brep.CreateTrimmedSurface(active_brep.Faces[0], newsurface, 0.001);
                        //    draw_brep = Brep.CreateTrimmedSurface(active_brep.Faces[1], newsurface, 0.001);
                        //    draw_brep = Brep.CopyTrimCurves(active_brep.Faces[0], newsurface, 0.001);
                        //    draw_brep = Brep.CopyTrimCurves(active_brep.Faces[1], newsurface, 0.001);
                        //}
                        var draw_brep = Brep.CopyTrimCurves(face, newsurface, 0.001);
                        foreach (var shell in shell_elements)
                            d.DrawBrepWires(draw_brep, shellColor);

                        foreach (var membrane in membrane_elements)
                            d.DrawBrepWires(draw_brep, membraneColor);

                        if (ShowIds)
                        {
                            double middle_u =
                                (newsurface.KnotsU[0] + newsurface.KnotsU[newsurface.KnotsU.Count() - 1]) / 2;
                            double middle_v =
                                (newsurface.KnotsV[0] + newsurface.KnotsV[newsurface.KnotsV.Count() - 1]) / 2;

                            var point = newsurface.PointAt(middle_u, middle_v);
                            d.Draw3dText(new Text3d(user_data_surface.BrepId.ToString()), shellColor, point);
                        }
                    }

                    if (ShowLoads)
                    {
                        var face = active_brep.Faces.First(item => item.SurfaceIndex == index);
                        foreach (var geometry_element in
                            user_data_surface.GetNumericalElementsOfPropertyType(typeof(PropertyLoad)))
                        {
                            var property_load = geometry_element.GetProperty<PropertyLoad>();
                            if (geometry_element.mParameterLocation.IsPoint())
                            {
                                var parameters = geometry_element.mParameterLocation.GetParameters();
                                VD.drawPointLoad(d,
                                    newsurface.PointAt(parameters[0], parameters[1]),
                                    property_load.mLoad);
                            }
                            else if (geometry_element.mParameterLocation.IsSurface())
                            {
                                if (property_load.mLoad.mLoadType == "SNOW")
                                {
                                    var direction = property_load.mLoad.GetLoadVector();
                                    VD.drawSurfaceSnowLoad(d,
                                        face,
                                        direction[0],
                                        direction[1],
                                        direction[2]);
                                }
                                else if (property_load.mLoad.mLoadType == "PRES"
                                         || property_load.mLoad.mLoadType == "PRES_FL")
                                    VD.drawSurfacePressureLoad(d,
                                        face,
                                        Convert.ToDouble(property_load.mLoad.mLoadX));
                                else
                                    VD.drawSurfaceLoad(d, face, property_load.mLoad);
                            }
                        }
                    }
                }

                for (int i = 0; i < active_brep.Edges.Count; i++)
                {
                    if (active_brep.Edges[i].TrimIndices().Count() < 1)
                        continue;

                    var trimIndex = active_brep.Edges[i].TrimIndices()[0];
                    var trim = active_brep.Trims[trimIndex];
                    var user_data_edge = active_brep.Curves2D[trim.TrimCurveIndex].UserData.Find(typeof(UserDataEdge)) as UserDataEdge;

                    if (user_data_edge == null)
                        continue;
                    if (i >= active_brep.Curves3D.Count)
                        continue;

                    var curve_3d = active_brep.Edges[i].EdgeCurve;

                    if (ShowSupports)
                    {
                        foreach (var brep_element in user_data_edge.GetNumericalElementsOfPropertyType(typeof(PropertySupport)))
                        {
                            VD.drawCurveSupport(
                                d,
                                curve_3d,
                                brep_element.GetProperty<PropertySupport>().mSupport);
                        }
                    }

                    if (ShowLoads)
                    {
                        foreach (var brep_element in user_data_edge.GetNumericalElementsOfPropertyType(typeof(PropertyLoad)))
                        {
                            VD.drawCurveLoad(
                                d,
                                curve_3d,
                                brep_element.GetProperty<PropertyLoad>().mLoad);
                        }
                    }

                    if (ShowElements)
                    {
                        foreach (var _ in user_data_edge.GetNumericalElementsOfPropertyType(typeof(PropertyCable)))
                        {
                            VD.drawCable(d, curve_3d);
                        }
                    }
                }
            }
            foreach (var curve in CurveList)
            {
                if (curve != null)
                {
                    var ud_curve = curve.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                    var ud_edge = curve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;
                    if (ud_curve == null || ud_edge == null)
                        continue;
                    if (ud_curve != null)
                    {
                        if (ShowSupports)
                        {
                            var numerical_elements = ud_curve.GetNumericalElementsOfPropertyType(typeof(PropertySupport));
                            // Draw weak surface points.
                            foreach (var numerical_element in numerical_elements)
                            {
                                if (numerical_element.mParameterLocation.IsPoint())
                                {
                                    var parameters = numerical_element.mParameterLocation.GetParameters();
                                    VD.drawPointSupport(d, curve.PointAt(parameters[0]), numerical_element.GetProperty<PropertySupport>().mSupport);
                                }
                                else if (numerical_element.mParameterLocation.IsEdge())
                                {
                                    VD.drawCurveSupport(d, curve, numerical_element.GetProperty<PropertySupport>().mSupport);
                                }
                            }
                        }
                        if (ShowLoads)
                        {
                            var numerical_elements = ud_curve.GetNumericalElementsOfPropertyType(typeof(PropertyLoad));
                            // Draw weak surface points.
                            foreach (var numerical_element in numerical_elements)
                            {
                                var property_load = numerical_element.GetProperty<PropertyLoad>();
                                if (numerical_element.mParameterLocation.IsPoint())
                                {
                                    var parameters = numerical_element.mParameterLocation.GetParameters();
                                    VD.drawPointLoad(d, curve.PointAt(parameters[0]), property_load.mLoad);
                                }
                                else if (numerical_element.mParameterLocation.IsEdge())
                                {
                                    VD.drawCurveLoad(d, curve, property_load.mLoad);
                                }
                            }
                        }
                        if (ShowElements)
                        {
                            if (ud_curve.GetNumericalElementsOfPropertyType(typeof(PropertyCable)).Count > 0)
                            {
                                var refinement = ud_curve.GetRefinement() as Refinement.RefinementCurve;
                                var tmp_crv = curve.ToNurbsCurve();
                                NurbsCurve newcurve = new NurbsCurve(tmp_crv);
                                newcurve.IncreaseDegree(refinement.PolynomialDegree);
                                if (refinement.KnotInsertType == 0)
                                {
                                    int ref_u = (int)refinement.KnotSubDivU;
                                    for (int j = 1; j < ref_u; j++)
                                    {
                                        for (int k = 1; k < tmp_crv.Knots.Count; k++)
                                        {
                                            if (tmp_crv.Knots[k - 1] < tmp_crv.Knots[k])//nonzero knot span
                                            {
                                                var knotspansize = tmp_crv.Knots[k] - tmp_crv.Knots[k - 1];
                                                for (int l = 1; l < ref_u; l++) // dividing in #ref_u elements
                                                    newcurve.Knots.InsertKnot(tmp_crv.Knots[k - 1] + l * knotspansize / ref_u);
                                            }
                                        }
                                    }
                                }

                                List<double> unique_knots = newcurve.Knots.Distinct().ToList();
                                List<Point3d> knot_points = new List<Point3d>();
                                foreach (var knot in unique_knots)
                                    knot_points.Add(newcurve.PointAt(knot));
                                PointCloud pt_cloud = new PointCloud(knot_points);

                                d.DrawCurve(newcurve, cableColor);
                                d.DrawPointCloud(pt_cloud, 3, cableColor);
                            }
                        }
                        if (ShowCouplings)
                        {
                            if (ud_curve.GetNumericalElementsOfPropertyType(typeof(PropertyCoupling)).Count > 0)
                            {
                                d.DrawCurve(curve, Color.Orange);
                            }
                        }
                    }
                    if (ud_edge != null)
                    {
                        if (ShowSupports)
                        {
                            var numerical_elements = ud_edge.GetNumericalElementsOfPropertyType(typeof(PropertySupport));
                            // Draw weak surface points.
                            foreach (var numerical_element in numerical_elements)
                            {
                                var support_parameters = numerical_element.GetProperty<PropertySupport>().mSupport;
                                if (!support_parameters.mIsSupportStrong)
                                {
                                    VD.drawCurveSupport(d, curve, support_parameters);
                                }
                            }
                        }
                        if (ShowLoads)
                        {
                            var numerical_elements = ud_edge.GetNumericalElementsOfPropertyType(typeof(PropertyLoad));
                            // Draw weak surface points.
                            foreach (var numerical_element in numerical_elements)
                            {
                                var property_load = numerical_element.GetProperty<PropertyLoad>();
                                VD.drawCurveLoad(d, curve, property_load.mLoad);
                            }
                        }
                        if (ShowElements)
                        {
                            if (ud_edge.GetNumericalElementsOfPropertyType(typeof(PropertyCable)).Count > 0)
                            {
                                d.DrawCurve(curve, cableColor);
                            }
                        }
                        if (ShowCouplings)
                        {
                            if (ud_edge.GetNumericalElementsOfPropertyType(typeof(PropertyCoupling)).Count > 0)
                            {
                                d.DrawCurve(curve, Color.Orange);
                            }
                        }
                    }
                }
            }
            if (ShowCouplings)
            {
                if (true)
                {
                    foreach (var curve in IntersectionCurveList)
                    {
                        var ud = curve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;
                        if (ud != null)
                        {
                            foreach (var brep_edge in ud.GetNumericalElementsOfPropertyType(typeof(PropertySupport)))
                            {
                                PropertySupport property = brep_edge.GetProperty<PropertySupport>();
                                VD.drawCurveSupport(d, curve, property.mSupport);
                            }
                        }
                        else
                        {
                            d.DrawCurve(curve, Color.Orange);
                        }
                    }
                    foreach (var point in IntersectionPointList)
                    {
                        d.DrawPoint(point.Location, Color.Orange);
                    }
                }
            }
        }


        public static void DrawDemElements(DisplayPipeline d,
            List<Rhino.Geometry.Point> PointList)
        {
            foreach (var point in PointList)
            {
                if (point.UserDictionary.TryGetDouble("RADIUS", out double radius))
                    d.DrawSphere(new Sphere(point.Location, radius), Color.BlueViolet);
            }
        }
    }
}