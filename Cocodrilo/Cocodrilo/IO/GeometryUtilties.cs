using Rhino;
using Rhino.DocObjects;
using System.Collections.Generic;
using System.Linq;
using Rhino.Geometry;
using Cocodrilo.ElementProperties;
using Cocodrilo.UserData;
using System;

namespace Cocodrilo.IO
{
    public static class GeometryUtilities
    {
        public static void AssignBrepIds(List<Brep> rBreps, List<Curve> rCurves, List<Point> rPoints, ref int rBrepId)
        {
            foreach (var brep in rBreps)
            {
                var user_data_brep = UserDataUtilities.GetOrCreateUserDataBrep(brep);
                user_data_brep.BrepId = rBrepId;
                rBrepId++;
            }
            foreach (var brep in rBreps)
            {
                foreach (var surface in brep.Surfaces)
                {
                    var user_data_surface = UserDataUtilities.GetOrCreateUserDataSurface(surface);
                    user_data_surface.BrepId = rBrepId;
                    rBrepId++;
                }
            }
            foreach (var brep in rBreps)
            {
                foreach (var edge in brep.Edges)
                {
                    if (edge.TrimIndices().Count() < 1)
                        continue;

                    var trimIndex = edge.TrimIndices()[0];
                    var trim = edge.Brep.Trims[trimIndex];
                    var curve = edge.Brep.Curves2D[trim.TrimCurveIndex];
                    var user_data_edge = UserDataUtilities.GetOrCreateUserDataEdge(curve);
                    user_data_edge.BrepId = rBrepId;
                    rBrepId++;
                }
            }
            foreach (var curve in rCurves)
            {
                var user_data_curve = curve.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                var user_data_edge = curve.UserData.Find(typeof(UserDataEdge)) as UserDataEdge;
                if (user_data_curve != null)
                {
                    user_data_curve.BrepId = rBrepId;
                    rBrepId++;
                }
                if (user_data_edge != null)
                {
                    user_data_edge.BrepId = rBrepId;
                    rBrepId++;
                }
                if (user_data_curve == null && user_data_edge == null)
                {
                    user_data_curve = new UserData.UserDataCurve();
                    user_data_curve.BrepId = rBrepId;
                    rBrepId++;
                    curve.UserData.Add(user_data_curve);
                    RhinoApp.WriteLine("WARNING: Curve with no user data found. New curve user data is added with brep id: " + user_data_curve.BrepId);
                }
            }
            foreach (var brep in rBreps)
            {
                foreach (var surface in brep.Surfaces)
                {
                    var user_data_surface = UserDataUtilities.GetOrCreateUserDataSurface(surface);
                    var numerical_elements = user_data_surface.GetNumericalElements();
                    foreach (var numerical_element in numerical_elements)
                    {
                        if (numerical_element.HasBrepId())
                        {
                            numerical_element.SetBrepId(rBrepId);
                            rBrepId++;
                        }
                    }
                }
            }
            foreach (var curve in rCurves)
            {
                var user_data_curve = curve.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
                if (user_data_curve == null)
                    continue;
                var numerical_elements = user_data_curve.GetNumericalElements();
                foreach (var numerical_element in numerical_elements)
                {
                    if (numerical_element.HasBrepId())
                    {
                        numerical_element.SetBrepId(rBrepId);
                        rBrepId++;
                    }
                }
            }
            foreach (var point in rPoints)
            {
                var user_data_point = UserDataUtilities.GetOrCreateUserDataPoint(point);
                user_data_point
                    .BrepId = rBrepId;
                rBrepId++;
            }
            
        }
        ///Additional function to assign a brep-id to a mesh
        ///to avoid changing all the other occurences of AssignBrepIds

        public static void AssignBrepIdToMesh(List<Mesh> rMeshes, ref int rBrepId)
        {
            foreach (var mesh in rMeshes)
            {
                var user_data_brep = UserDataUtilities.GetOrCreateUserDataMesh(mesh);
                user_data_brep.BrepId = rBrepId;
                rBrepId++;
            }
        }

        /// <summary>
        /// This function obtains all active Rhino Brep objects which have at least one
        /// UserDataSurface or UserDataEdge
        /// </summary>
        /// <returns></returns>
        public static List<Brep> GetActiveBrepList()
        {
            //get the active Objects
            Rhino.DocObjects.Tables.ObjectTable obj = RhinoDoc.ActiveDoc.Objects;
            var object_enumerator_settings = new ObjectEnumeratorSettings {NormalObjects = true};
            //all elements that are not deleted and visible
            var active_elements = obj.FindByFilter(object_enumerator_settings);
            //all brep elements
            var brep_elements = active_elements.Where(
                element => element.Geometry.HasBrepForm).ToList();

            var elements_with_user_data = new List<Brep>();
            foreach (var active_brep_element in brep_elements)
            {
                if ((active_brep_element.Geometry as Brep) == null)
                    continue;

                if ((active_brep_element.Geometry as Brep).Surfaces.Any(
                    surface => surface.UserData.Find(typeof(UserDataSurface)) != null))
                {
                    elements_with_user_data.Add((Brep) active_brep_element.Geometry);
                }
                else if ((active_brep_element.Geometry as Brep).Curves2D.Any(
                    curve => curve.UserData.Find(typeof(UserDataEdge)) != null))
                {
                    elements_with_user_data.Add((Brep) active_brep_element.Geometry);
                }
            }

             return elements_with_user_data;
        }


        public static List<Brep> GetBrepList()
        {
            //get the active Objects
            Rhino.DocObjects.Tables.ObjectTable obj = RhinoDoc.ActiveDoc.Objects;
            var object_enumerator_settings = new ObjectEnumeratorSettings { NormalObjects = true };
            //all elements that are not deleted and visible
            var active_elements = obj.FindByFilter(object_enumerator_settings);
            //all brep elements
            var brep_elements = active_elements.Where(
                element => element.Geometry.HasBrepForm).ToList();

            var elements = new List<Brep>();
            foreach (var active_brep_element in brep_elements)
            {
                elements.Add((Brep) active_brep_element.Geometry);
            }

            return elements;
        }

        public static List<Curve> GetActiveCurveList()
        {
            //get the active Objects
            Rhino.DocObjects.Tables.ObjectTable obj = RhinoDoc.ActiveDoc.Objects;
            var object_enumerator_settings = new ObjectEnumeratorSettings {NormalObjects = true};

            //all elements that are not deleted and visible
            var active_elements = obj.FindByFilter(object_enumerator_settings);

            //all brep elements
            var Elements = active_elements.Where(
                element => !element.Geometry.HasBrepForm).ToList();

            var elements_with_user_data = new List<Curve>();
            foreach (var element in Elements)
            {
                if (element.Geometry as Curve != null)
                {
                    if ((element.Geometry as Curve).UserData.Find(typeof(UserDataEdge)) != null)
                        elements_with_user_data.Add(element.Geometry as Curve);

                    else if ((element.Geometry as Curve).UserData.Find(typeof(UserDataCurve)) != null)
                        elements_with_user_data.Add(element.Geometry as Curve);
                }
            }

            return elements_with_user_data;
        }

        public static List<Curve> GetCurveList()
        {
            //get the active Objects
            Rhino.DocObjects.Tables.ObjectTable obj = RhinoDoc.ActiveDoc.Objects;
            var object_enumerator_settings = new ObjectEnumeratorSettings {NormalObjects = true};

            //all elements that are not deleted and visible
            var active_elements = obj.FindByFilter(object_enumerator_settings);

            //all brep elements
            var curve_elements = active_elements.Where(
                element => !element.Geometry.HasBrepForm && element.Geometry is Curve).ToList();

            var curves = new List<Curve>(curve_elements.Count);
            for (int i = 0; i < curve_elements.Count; i++)
            {
                curves.Add(curve_elements[i].Geometry as Curve);
            }

            return curves;
        }

        public static void GetIntersection(
            List<Brep> Breps,
            List<Curve> Curves,
            List<Point> PointList,
            ref List<Curve> IntersectionCurveList,
            ref List<Point> IntersectionPointList,
            List<Curve> PreviousIntersectionCurveList,
            List<Point> PreviousIntersectionPointList)
        {
            /// Brep - Brep intersection
            if (Breps.Count > 1)
            {
                for (int i = 0; i < Breps.Count; i++)
                {
                    for (int j = i + 1; j < Breps.Count; j++)
                    {
                        GetBrepBrepIntersections(
                            Breps[i],
                            Breps[j],
                            ref IntersectionCurveList,
                            ref IntersectionPointList,
                            PreviousIntersectionCurveList,
                            PreviousIntersectionPointList);
                    }
                }
            }

            /// Brep - Curve intersection
            for (int i = 0; i < Breps.Count; i++)
            {
                for (int j = 0; j < Curves.Count; j++)
                {
                    GetBrepCurveIntersections(
                        Breps[i],
                        Curves[j],
                        ref IntersectionCurveList,
                        ref IntersectionPointList,
                        PreviousIntersectionCurveList,
                        PreviousIntersectionPointList);
                }
            }

            /// Brep - Point intersection
            if (Breps.Count > 0)
            {
                for (int i = 0; i < Breps.Count; i++)
                {
                    GetBrepPointIntersections(
                        Breps[i],
                        PointList,
                        ref IntersectionPointList);
                }
            }

            /// Curve - Curve intersection
            if (Curves.Count > 1)
            {
                for (int i = 0; i < Curves.Count; i++)
                {
                    for (int j = i + 1; j < Curves.Count; j++)
                    {
                        GetCurveCurveIntersections(
                            Curves[i],
                            Curves[j],
                            ref IntersectionCurveList,
                            ref IntersectionPointList,
                            PreviousIntersectionPointList);
                    }
                }
            }

            // Edge - Edge intersection (Join)
            if (Panels.UserControlCocodriloPanel.Instance.getIsEdgeCoupling())
            {
               foreach (var brep in Breps)
               {
                   Rhino.Geometry.Collections.BrepEdgeList edges = brep.Edges;
                   foreach (var edge in edges)
                   {               
                       if (edge.TrimIndices().Count() > 1)
                       {
                           GetEdgeEdgeIntersection(edge, brep, brep, ref IntersectionPointList, PreviousIntersectionPointList);                      
                           IntersectionCurveList.Add(edge);
                       }
                   }
               }
            }   
        }

        public static void GetBrepBrepIntersections(
            Brep Brep1,
            Brep Brep2,
            ref List<Curve> IntersectionCurveList,
            ref List<Point> IntersectionPointList,
            List<Curve> PreviousIntersectionCurveList,
            List<Point> PreviousIntersectionPointList)
        {
            //var intersection_brep = BoundingBox.Intersection(Brep1.GetBoundingBox(true), Brep2.GetBoundingBox(true));
            //if (intersection_brep.Volume > 0.0)
            //{
                var Intersection = Rhino.Geometry.Intersect.Intersection.BrepBrep(
                    Brep1,
                    Brep2,
                    RhinoDoc.ActiveDoc.ModelAbsoluteTolerance*10,
                    out var overlap_curves,
                    out var intersection_points);

                for (var index = 0; index < overlap_curves.Length; index++)
                {
                    var overlap_curve = overlap_curves[index];
                    var user_data_surface_1 = GetIntersectedSurface(overlap_curve, Brep1);
                    var user_data_surface_2 = GetIntersectedSurface(overlap_curve, Brep2);

                    if (user_data_surface_1 == null || user_data_surface_2 == null)
                        continue;

                    foreach (var previous_curve in PreviousIntersectionCurveList)
                    {
                        if (overlap_curve.Equals(previous_curve))
                            overlap_curve = previous_curve;
                    }

                    var user_data_edge = UserDataUtilities.GetOrCreateUserDataEdge(overlap_curve);

                    user_data_edge.AddCoupling(
                        user_data_surface_1.BrepId,
                        user_data_surface_2.BrepId);

                    if (user_data_surface_1.IsBrepGroupCoupledWith(
                            user_data_surface_2.GetThisBrepGroupCouplingId())
                     && user_data_surface_2.IsBrepGroupCoupledWith(
                            user_data_surface_1.GetThisBrepGroupCouplingId()))
                    {
                        bool rotational_continuity = user_data_surface_1.CheckRotationalContinuity()
                                                     && user_data_surface_2.CheckRotationalContinuity();

                        var this_support = new Support(
                            true, true, true,
                            "0.0", "0.0", "0.0",
                            rotational_continuity, false);

                        var property_edge_coupling = new PropertyCoupling(
                            GeometryType.SurfaceEdgeSurfaceEdge,
                            this_support,
                            new TimeInterval());

                        user_data_edge.AddNumericalElement(property_edge_coupling);
                    }
                    else
                    {
                        user_data_edge.DeleteNumericalElementOfPropertyType(typeof(PropertyCoupling));
                    }

                    IntersectionCurveList.Add(overlap_curve);

                    //if(Panels.UserControlCocodriloPanel.Instance.getIsEdgeCoupling())
                    //{
                    //    GetEdgeEdgeIntersection(overlap_curve, Brep1, Brep2, ref IntersectionPointList, PreviousIntersectionPointList);
                    //}
                }

                for (var i = 0; i < intersection_points.Length; i++)
                {
                    var user_data_surface_1 = GetIntersectedSurface(intersection_points[i], Brep1);
                    var user_data_surface_2 = GetIntersectedSurface(intersection_points[i], Brep2);

                    Point intersection_point = new Point(intersection_points[i]);
                    foreach (var previous_point in PreviousIntersectionPointList)
                    {
                        if (previous_point.Location.Equals(intersection_points[i]))
                            intersection_point = previous_point;
                    }

                    var user_data_point = UserDataUtilities.GetOrCreateUserDataPoint(
                        intersection_point);

                    user_data_point.AddCoupling(
                        user_data_surface_1.BrepId,
                        user_data_surface_2.BrepId);

                    if (user_data_surface_1.IsBrepGroupCoupledWith(
                            user_data_surface_2.GetThisBrepGroupCouplingId())
                     && user_data_surface_2.IsBrepGroupCoupledWith(
                            user_data_surface_1.GetThisBrepGroupCouplingId()))
                    {
                        bool rotational_continuity = user_data_surface_1.CheckRotationalContinuity()
                                                     && user_data_surface_2.CheckRotationalContinuity();

                        var this_support = new Support(
                            true, true, true,
                            "0.0", "0.0", "0.0",
                            rotational_continuity, false);

                        var property_coupling = new PropertyCoupling(
                            GeometryType.SurfaceEdgeSurfaceEdge,
                            this_support,
                            new TimeInterval());

                        user_data_point.AddNumericalElement(property_coupling);
                    }
                    else
                    {
                        user_data_point.DeleteNumericalElementOfPropertyType(typeof(PropertyCoupling));
                    }

                    IntersectionPointList.Add(intersection_point);
                }
        }


        public static void GetBrepCurveIntersections(
            Brep Brep1,
            Curve Curve1,
            ref List<Curve> rIntersectionCurveList,
            ref List<Point> rIntersectionPointList,
            List<Curve> PreviousIntersectionCurveList,
            List<Point> PreviousIntersectionPointList)
        {
            var user_data_edge = Curve1.UserData.Find(typeof(Cocodrilo.UserData.UserDataEdge)) as Cocodrilo.UserData.UserDataEdge;
            if (user_data_edge != null)
            {
                var nurbs_curve_1 = Curve1.ToNurbsCurve();

                bool has_intersection = Rhino.Geometry.Intersect.Intersection.CurveBrep(
                    nurbs_curve_1,
                    Brep1,
                    RhinoDoc.ActiveDoc.ModelAbsoluteTolerance,
                    out Curve[] overlap_curves,
                    out Point3d[] intersection_points);

                if (has_intersection && overlap_curves.Length == 0 && intersection_points.Length == 0)
                    goto CurveUserData;

                var numerical_elements = user_data_edge.GetNumericalElements();
                if (numerical_elements.Count > 0)
                {
                    foreach (var overlap_curve in overlap_curves)
                    {
                        var user_data_surface = GetIntersectedSurface(overlap_curve, Brep1);
                        var user_data_edge_overlap_curve = UserDataUtilities.GetOrCreateUserDataEdge(overlap_curve);

                        foreach (var brep_element in numerical_elements.ToList())
                        {
                            var property = brep_element.GetProperty(out bool success);
                            if (!success)
                                continue;
                            if (property is PropertySupport)
                            {
                                var support_property = (property as PropertySupport);
                                if (support_property.mSupport.mIsSupportStrong)
                                {
                                    var nurbs_surface = GetIntersectedNurbsSurface(overlap_curve, Brep1);
                                    if (nurbs_surface == null)
                                        continue;
                                    var parameter_curve = nurbs_surface.Pullback(overlap_curve, RhinoDoc.ActiveDoc.ModelAbsoluteTolerance);
                                    if (GeometryUtilities.TryGetParamaterLocationSurface(
                                        parameter_curve,
                                        nurbs_surface,
                                        out Elements.ParameterLocationSurface parameter_location))
                                    {
                                        user_data_surface.AddNumericalElement(property, parameter_location);
                                        // This check may be revised. It has been introduced to allow the definition of a single line which spans multiple patches.
                                        if (Math.Abs(overlap_curve.GetLength() - nurbs_curve_1.GetLength()) < 0.01)
                                        {
                                            user_data_edge.DeleteNumericalElement(property);
                                        }
                                    }
                                    else
                                    {
                                        RhinoApp.WriteLine("WARNING: Could not find iso curve for support.");
                                    }
                                }
                                else
                                {
                                    user_data_edge_overlap_curve.AddNumericalElement(property);
                                    user_data_edge_overlap_curve.AddCoupling(user_data_surface.BrepId);
                                    rIntersectionCurveList.Add(overlap_curve);
                                    if (Math.Abs(overlap_curve.GetLength() - nurbs_curve_1.GetLength()) < 0.01)
                                    {
                                        user_data_edge.DeleteNumericalElement(property);
                                    }
                                }
                            }
                            else
                            {
                                user_data_edge_overlap_curve.AddNumericalElement(property);
                                user_data_edge_overlap_curve.AddCoupling(user_data_surface.BrepId);
                                rIntersectionCurveList.Add(overlap_curve);
                                if (Math.Abs(overlap_curve.GetLength() - nurbs_curve_1.GetLength()) < 0.01)
                                {
                                    user_data_edge.DeleteNumericalElement(property);
                                }
                            }
                        }
                    }
                }
            }

            CurveUserData:
            var user_data_curve = Curve1.UserData.Find(typeof(Cocodrilo.UserData.UserDataCurve)) as Cocodrilo.UserData.UserDataCurve;
            if (user_data_curve != null)
            {
                if (!(user_data_curve.HasNumericalElementsOfPropertyType(typeof(PropertyCable))
                    || user_data_curve.HasNumericalElementsOfPropertyType(typeof(PropertyBeam))))
                    return;

                var cable_property_elements = user_data_curve.GetNumericalElementsOfPropertyType(typeof(PropertyCable));

                foreach (var cable_property_element in cable_property_elements)
                {
                    var cable_property = cable_property_element.GetProperty(out bool success) as PropertyCable;
                    if (cable_property.mCableProperties.mCableCouplingTypes == CableCouplingType.StartAndEnd)
                    {
                        List<KeyValuePair<Point3d, double>> relevant_curve_points = new List<KeyValuePair<Point3d, double>>();
                        double first_knot = Curve1.ToNurbsCurve().Knots.First();
                        double last_knot = Curve1.ToNurbsCurve().Knots.Last();
                        relevant_curve_points.Add(new KeyValuePair<Point3d, double>(Curve1.PointAtStart, first_knot));
                        relevant_curve_points.Add(new KeyValuePair<Point3d, double>(Curve1.PointAtEnd, last_knot));

                        foreach (var point_parameter_pair in relevant_curve_points)
                        {
                            Brep1.ClosestPoint(point_parameter_pair.Key, out Point3d closest_point, out ComponentIndex component_index, out double u, out double v, 0.01, out _);

                            if (component_index.ComponentIndexType == ComponentIndexType.InvalidType) continue;

                            var intersection_point = new Point(closest_point);

                            var user_data_point = UserDataUtilities.GetOrCreateUserDataPoint(intersection_point);

                            UserDataSurface user_data_surface;
                            if (component_index.ComponentIndexType == ComponentIndexType.BrepEdge)
                            {
                                var brep_edge = Brep1.Edges[component_index.Index];
                                user_data_surface = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(Brep1.Trims[brep_edge.TrimIndices()[0]].Face);
                                var local_coordinates = Brep1.Trims[brep_edge.TrimIndices()[0]].PointAt(u);
                                u = local_coordinates[0];
                                v = local_coordinates[1];
                            }
                            else
                            {
                                var variable = Brep1.Faces[component_index.Index].UnderlyingSurface();
                                user_data_surface = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(Brep1.Faces[component_index.Index]);
                                user_data_surface = Cocodrilo.UserData.UserDataUtilities.GetOrCreateUserDataSurface(variable);
                            }

                            user_data_point.AddCoupling(
                                user_data_surface.BrepId,
                                user_data_curve.BrepId);
                            user_data_point.GetCoupling().TryAddTrimIndexToBrepId(user_data_curve.BrepId, point_parameter_pair.Value, 0.0, 0.0);
                            user_data_point.GetCoupling().TryAddTrimIndexToBrepId(user_data_surface.BrepId, u, v, 0.0);

                            var this_support = new Support(
                                true, true, true,
                                "0.0", "0.0", "0.0",
                                false, false);

                            var property_point_coupling = new PropertyCoupling(
                                GeometryType.SurfacePointCurvePoint,
                                this_support,
                                new TimeInterval());

                            user_data_point.AddNumericalElement(property_point_coupling);

                            rIntersectionPointList.Add(intersection_point);
                        }
                    }

                    /// Missing the Coupling through the entire curve
                    //foreach (var overlap_curve in overlap_curves)
                    //{
                    //    //Add Informatin of this Coupling Element ID zu related 2dPatches
                    //    //Findout which Surface it is connected to
                    //    //if Curves overlap different Surfaces of same Brep, several Overlap Curvs will be created
                    //    //Check closest Surface to MidPoint of OverlapCurv --> get Surface Index
                    //    //var StartPtOverlCrv = overlap_curve.PointAtNormalizedLength(0.5);

                    //    var user_data_surface = GetIntersectedSurface(overlap_curve, Brep1);

                    //    if (user_data_curve.IsBrepGroupCoupledWith(user_data_surface.GetThisBrepGroupCouplingId()))
                    //    {
                    //        var user_data_edge_overlap_curve = UserDataUtilities.GetOrCreateUserDataEdge(overlap_curve);

                    //        var this_support = new Support(
                    //            true, true, true,
                    //            "0.0", "0.0", "0.0",
                    //            false, false);

                    //        var property_edge_coupling = new PropertyCoupling(
                    //            GeometryType.SurfaceEdgeCurveEdge,
                    //            this_support,
                    //            new TimeInterval());

                    //        user_data_curve.AddNumericalElement(property_edge_coupling);

                    //        rIntersectionCurveList.Add(overlap_curve);
                    //        user_data_edge_overlap_curve.AddCoupling(user_data_surface.BrepId, user_data_curve.BrepId);

                    //        rIntersectionCurveList.Add(overlap_curve);
                    //    }
                    //}

                    //for (var i = 0; i < intersection_points.Length; i++)
                    //{
                    //    Brep1.ClosestPoint(
                    //        intersection_points[i],
                    //        out Point3d closest_point,
                    //        out var component_index,
                    //        out double s,
                    //        out double t,
                    //        RhinoDoc.ActiveDoc.ModelAbsoluteTolerance,
                    //        out Vector3d normal);

                    //    var user_data_surface = GetIntersectedSurface(intersection_points[i], Brep1);

                    //    Point intersection_point = new Point(intersection_points[i]);
                    //    foreach (var previous_point in PreviousIntersectionPointList)
                    //    {
                    //        if (previous_point.Location.Equals(intersection_points[i]))
                    //            intersection_point = previous_point;
                    //    }

                    //    var user_data_point = UserDataUtilities.GetOrCreateUserDataPoint(intersection_point);

                    //    user_data_point.AddCoupling(
                    //        user_data_surface.BrepId,
                    //        user_data_curve.BrepId);

                    //    if (user_data_surface.IsBrepGroupCoupledWith(
                    //            user_data_curve.GetThisBrepGroupCouplingId())
                    //        && user_data_curve.IsBrepGroupCoupledWith(
                    //            user_data_surface.GetThisBrepGroupCouplingId()))
                    //    {
                    //        var this_support = new Support(
                    //            true, true, true,
                    //            "0.0", "0.0", "0.0",
                    //            false, false,
                    //            false);

                    //        var property_coupling = new PropertyCoupling(
                    //            GeometryType.SurfacePointCurvePoint,
                    //            this_support,
                    //            new TimeInterval());

                    //        user_data_point.AddNumericalElement(property_coupling);
                    //    }
                    //    else
                    //    {
                    //        user_data_point.DeleteNumericalElementOfPropertyType(typeof(PropertyCoupling));
                    //    }


                    //    rIntersectionPointList.Add(intersection_point);
                    //}
                }
            }
        }

        public static void GetBrepPointIntersections(
            Brep Brep,
            List<Point> PointList,
            ref List<Point> rIntersectionPointList)
        {
            double tolerance = RhinoDoc.ActiveDoc.ModelAbsoluteTolerance;

            foreach (var point in PointList)
            {
                var user_data_point = point.UserData.Find(typeof(UserDataPoint)) as UserDataPoint;
                if (user_data_point == null)
                    continue;
                var numerical_elements = user_data_point.GetNumericalElements();
                if (numerical_elements.Count == 0)
                    continue;

                Brep.FindCoincidentBrepComponents(point.Location, tolerance, out int[] face_ids, out int[] edge_ids, out int[] vertice_ids);

                if (edge_ids.Count() > 0)
                {
                    var edge = Brep.Edges[edge_ids[0]];

                    var trimIndex = edge.TrimIndices()[0];
                    var trim = edge.Brep.Trims[trimIndex];
                    var curve = edge.Brep.Curves2D[trim.TrimCurveIndex];

                    var user_data_edge = UserDataUtilities.GetOrCreateUserDataEdge(curve);

                    edge.ClosestPoint(point.Location, out double t);

                    edge.NormalizedLengthParameter(t, out double t_normalized);

                    var parameter_location_edge = new Elements.ParameterLocationCurve(GeometryType.CurvePoint, t, t_normalized);

                    foreach (var numerical_element in numerical_elements.ToList())
                    {
                        var property = numerical_element.GetProperty(out _);
                        CocodriloPlugIn.Instance.AddProperty(property);

                        if (property.GetType() == typeof(PropertyConnector))
                        {
                            var old_point = rIntersectionPointList.Find(item => item.Location.Equals(point.Location));
                            if (old_point != null)
                            {
                                var user_data_point_old = UserDataUtilities.GetOrCreateUserDataPoint(old_point);

                                user_data_point_old.AddCoupling(user_data_edge.BrepId);
                                user_data_point.GetCoupling().TryAddTrimIndexToBrepId(user_data_edge.BrepId, t, 0.0, 0.0);
                            }
                            else
                            {
                                //user_data_point.DeleteNumericalElement(property);
                                //user_data_point.AddNumericalElementPoint(property, parameter_location_edge);
                                user_data_point.AddCoupling(user_data_edge.BrepId);
                                user_data_point.GetCoupling().TryAddTrimIndexToBrepId(user_data_edge.BrepId, t, 0.0, 0.0);
                                rIntersectionPointList.Add(point);
                            }
                        }
                    }
                }

                if (face_ids.Length > 0)
                {
                    var brep_face = Brep.Faces[face_ids[0]];
                    var surface = Brep.Surfaces[brep_face.SurfaceIndex];
                    surface.ClosestPoint(point.Location, out double u, out double v);
                    double u_normalized = surface.Domain(0).NormalizedParameterAt(u);
                    if (u_normalized != 0 && u_normalized != 1) // needed if triangular surface
                    {
                        var test_point = surface.PointAt(surface.Domain(0).ParameterAt(1), u);
                        if (test_point.DistanceTo(point.Location) < tolerance)
                            u_normalized = 1;
                    }
                    double v_normalized = surface.Domain(1).NormalizedParameterAt(v);
                    if (v_normalized != 0 && v_normalized != 1) // needed if triangular surface
                    {
                        var test_point = surface.PointAt(u, surface.Domain(1).ParameterAt(1));
                        if (test_point.DistanceTo(point.Location) < tolerance)
                            v_normalized = 1;
                    }
                    var parameter_location = new Elements.ParameterLocationSurface(GeometryType.SurfacePoint, u, v, u_normalized, v_normalized);

                    var user_data_surface = UserDataUtilities.GetOrCreateUserDataSurface(surface);

                    foreach (var numerical_element in numerical_elements.ToList())
                    {
                        var property = numerical_element.GetProperty(out _);
                        CocodriloPlugIn.Instance.AddProperty(property);

                        bool can_be_strong = (property.GetType() == typeof(PropertySupport))
                            ? (property as PropertySupport).mSupport.mIsSupportStrong
                            : true;

                        if (property.GetType() == typeof(PropertyConnector))
                        {
                            /// To be added if surface to surface connectors are considered.
                            //var old_point = rIntersectionPointList.Find(item => item.Location.Equals(point.Location));
                            //if (old_point != null)
                            //{
                            //    var user_data_point_old = UserDataUtilities.GetOrCreateUserDataPoint(old_point);

                            //    user_data_point_old.AddCoupling(
                            //        user_data_surface.BrepId);
                            //}
                            //else
                            //{
                            //    user_data_point.AddNumericalElementPoint(property, parameter_location);
                            //    user_data_point.AddCoupling(
                            //        user_data_surface.BrepId);
                            //    rIntersectionPointList.Add(point);
                            //}
                        }
                        else if (parameter_location.IsOnNodes() && can_be_strong)
                        {
                            if (property.GetType() == typeof(PropertyCheck))
                                (property as PropertyCheck).mOnNode = true;

                            user_data_surface.AddNumericalElement(property, parameter_location);
                            user_data_point.DeleteNumericalElement(property);
                        }
                        else
                        {
                            if (property.GetType() == typeof(PropertyCheck))
                                (property as PropertyCheck).mOnNode = false;

                            user_data_surface.AddNumericalElementPoint(property, parameter_location);
                            user_data_point.DeleteNumericalElement(property);
                        }
                    }
                }
            }
        }

        public static void GetCurveCurveIntersections(
            Curve Curve1,
            Curve Curve2,
            ref List<Curve> IntersectionCurveList,
            ref List<Point> IntersectionPointList,
            List<Point> PreviousIntersectionPointList)
        {
            var Intersection = Rhino.Geometry.Intersect.Intersection.CurveCurve(
                Curve1,
                Curve2,
                RhinoDoc.ActiveDoc.ModelAbsoluteTolerance, 
                0.01);

            if (Intersection == null)
                return;

            var intersection_points = new List<Point3d>();

            var user_data_curve_1 = Curve1.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
            var user_data_curve_2 = Curve2.UserData.Find(typeof(UserDataCurve)) as UserDataCurve;

            // stop of no curve user data provided.
            if (user_data_curve_1 == null || user_data_curve_2 == null)
                return;
            // stop of neither of the curves do have any proprietary property as beam or cable.
            if (!((user_data_curve_1.HasNumericalElementsOfPropertyType(typeof(PropertyBeam)) || user_data_curve_1.HasNumericalElementsOfPropertyType(typeof(PropertyCable)))
                && (user_data_curve_2.HasNumericalElementsOfPropertyType(typeof(PropertyBeam)) || user_data_curve_2.HasNumericalElementsOfPropertyType(typeof(PropertyCable)))))
                return;

            for (int i = 0; i < Intersection.Count; i++)
            {
                if(Intersection[i].PointA.DistanceTo(Intersection[i].PointB) < double.Epsilon)
                {
                    intersection_points.Add(Intersection[i].PointB);
                }
            }

            for (var i = 0; i < intersection_points.Count; i++)
            {
                Point intersection_point = new Point(intersection_points[i]);
                foreach (var previous_point in PreviousIntersectionPointList)
                {
                    if (previous_point.Location.Equals(intersection_points[i]))
                        intersection_point = previous_point;
                }
                var user_data_point = UserDataUtilities.GetOrCreateUserDataPoint(
                    intersection_point);

                user_data_point.AddCoupling(
                    user_data_curve_1.BrepId,
                    user_data_curve_2.BrepId);

                if (user_data_curve_1.IsBrepGroupCoupledWith(
                        user_data_curve_2.GetThisBrepGroupCouplingId())
                    && user_data_curve_2.IsBrepGroupCoupledWith(
                        user_data_curve_1.GetThisBrepGroupCouplingId()))
                {
                    var this_support = new Support(
                        true, true, true,
                        "0.0", "0.0", "0.0",
                        false, false,
                        false);

                    var property_coupling = new PropertyCoupling(
                        GeometryType.CurvePointCurvePoint,
                        this_support,
                        new TimeInterval());

                    user_data_point.AddNumericalElement(property_coupling);
                }
                else
                {
                    user_data_point.DeleteNumericalElementOfPropertyType(typeof(PropertyCoupling));
                }
                
                IntersectionPointList.Add(intersection_point);
            }
        }

        public static void GetEdgeEdgeIntersection(
            Curve IntersectionCurve,
            Brep Brep1,
            Brep Brep2,
            ref List<Point> IntersectionPointList,
            List<Point> PreviousIntersectionPointList)
        {
            var tolerance = 1e-9;
            //1. Loop over the first surface
            foreach (var edge_1 in Brep1.Edges)
            {
                var edge_intersection_points = new List<Point3d>();

                //2. Find the intersection between the overlap_curve and embedded edge of Brep1
                var trim_index_1 = edge_1.TrimIndices()[0];
                var trim_1 = edge_1.Brep.Trims[trim_index_1];
                var curve_1 = edge_1.Brep.Curves2D[trim_1.TrimCurveIndex];

                bool is_intersection_curve_equal_edge_1 = (IntersectionCurve.PointAtStart.DistanceTo(edge_1.PointAtStart) < tolerance && IntersectionCurve.PointAtEnd.DistanceTo(edge_1.PointAtEnd) < tolerance) ||
                                                          (IntersectionCurve.PointAtStart.DistanceTo(edge_1.PointAtEnd) < tolerance && IntersectionCurve.PointAtEnd.DistanceTo(edge_1.PointAtStart) < tolerance);

                if (is_intersection_curve_equal_edge_1 ||  edge_1.TrimIndices().Count() > 1)
                    continue;

                var Intersection_point = Rhino.Geometry.Intersect.Intersection.CurveCurve(
                    IntersectionCurve,
                    edge_1,
                    RhinoDoc.ActiveDoc.ModelAbsoluteTolerance,
                    0.01);

                if (Intersection_point.Count == 0)
                    continue;

                for (int i = 0; i < Intersection_point.Count; i++)
                {
                    if (Intersection_point[i].PointA.DistanceTo(Intersection_point[i].PointB) < tolerance)
                    {
                        edge_intersection_points.Add(Intersection_point[i].PointB);
                    }
                }

                //3. Store the brep1 id
                var user_data_edge_1 = (UserDataEdge)curve_1.UserData.Find(typeof(UserDataEdge));
                int brep_id_edge_1 = user_data_edge_1.BrepId;

                //4. Find the intersection betweem the edge_1 and all embedded edges of Brep2, then store its id
                foreach (var edge_2 in Brep2.Edges)
                {
                    var trim_index_2 = edge_2.TrimIndices()[0];
                    var trim_2 = edge_2.Brep.Trims[trim_index_2];
                    var curve_2 = edge_2.Brep.Curves2D[trim_2.TrimCurveIndex];

                    if (Brep1 == Brep2 && trim_index_1 > trim_index_2)
                        continue;

                    bool is_intersection_curve_equal_edge_2 = (IntersectionCurve.PointAtStart.DistanceTo(edge_2.PointAtStart) < tolerance && IntersectionCurve.PointAtEnd.DistanceTo(edge_2.PointAtEnd) < tolerance) ||
                                                              (IntersectionCurve.PointAtStart.DistanceTo(edge_2.PointAtEnd) < tolerance && IntersectionCurve.PointAtEnd.DistanceTo(edge_2.PointAtStart) < tolerance);
                    bool is_edge_1_equal_edge_2 = (edge_1.PointAtStart.DistanceTo(edge_2.PointAtStart) < tolerance && edge_1.PointAtEnd.DistanceTo(edge_2.PointAtEnd) < tolerance) ||
                                                  (edge_1.PointAtStart.DistanceTo(edge_2.PointAtEnd) < tolerance && edge_1.PointAtEnd.DistanceTo(edge_2.PointAtStart) < tolerance);

                    if (is_intersection_curve_equal_edge_2 ||  edge_2.TrimIndices().Count() > 1 || is_edge_1_equal_edge_2)
                        continue;

                    var Intersection_point_2 = Rhino.Geometry.Intersect.Intersection.CurveCurve(
                        IntersectionCurve,
                        edge_2,
                        RhinoDoc.ActiveDoc.ModelAbsoluteTolerance,
                        0.01);

                    var Intersection_point_3 = Rhino.Geometry.Intersect.Intersection.CurveCurve(
                        edge_1,
                        edge_2,
                        RhinoDoc.ActiveDoc.ModelAbsoluteTolerance,
                        0.1);

                    if (Intersection_point_2.Count == 0 || Intersection_point_3.Count == 0)
                        continue;

                    var user_data_edge_2 = (UserDataEdge)curve_2.UserData.Find(typeof(UserDataEdge));
                    int brep_id_edge_2 = user_data_edge_2.BrepId;

                    //5. Create the user_data_point
                    for (var i = 0; i < edge_intersection_points.Count; i++)
                    {
                        Point intersection_point = new Point(edge_intersection_points[i]);
                        foreach (var previous_point in PreviousIntersectionPointList)
                        {
                            if (previous_point.Location.Equals(edge_intersection_points[i]))
                                intersection_point = previous_point;
                        }

                        var user_data_point = UserDataUtilities.GetOrCreateUserDataPoint(
                            intersection_point);

                        user_data_point.AddCoupling(
                            brep_id_edge_1,
                            brep_id_edge_2);

                        if (user_data_edge_1.IsBrepGroupCoupledWith(
                               user_data_edge_2.GetThisBrepGroupCouplingId())
                           && user_data_edge_2.IsBrepGroupCoupledWith(
                               user_data_edge_1.GetThisBrepGroupCouplingId()))
                        {
                            var this_support = new Support(
                                true, true, true,
                                "0.0", "0.0", "0.0",
                                false, false,
                                false);

                            var property_coupling = new PropertyCoupling(
                                GeometryType.CurvePointCurvePoint,
                                this_support,
                                new TimeInterval());

                            user_data_point.AddNumericalElement(property_coupling);
                        }
                        else
                        {
                           user_data_point.DeleteNumericalElementOfPropertyType(typeof(PropertyCoupling));
                        }

                        IntersectionPointList.Add(intersection_point);
                    }

                    var user_data_point_2 = UserDataUtilities.GetOrCreateUserDataPoint(IntersectionPointList.Last());
                    if (user_data_point_2.IsCoupledWith(brep_id_edge_2))
                    {
                        edge_2.ClosestPoint(
                            IntersectionPointList.Last().Location,
                            out double u);

                        //mapping from edge domain to the curve domain
                        double v = (curve_2.Domain.Length) / (edge_2.Domain.Length) * (u - edge_2.Domain.T0) + (curve_2.Domain.T0);

                        user_data_point_2.TryAddLocalCoordinates(
                            brep_id_edge_2,
                            v, 0.0, 0.0);
                    }
                }
                var user_data_point_1 = UserDataUtilities.GetOrCreateUserDataPoint(IntersectionPointList.Last());
                if (user_data_point_1.IsCoupledWith(brep_id_edge_1))
                {
                    edge_1.ClosestPoint(
                        IntersectionPointList.Last().Location,
                        out double u);

                    //mapping from edge domain to the curve domain
                    double v = (curve_1.Domain.Length)/(edge_1.Domain.Length)*(u - edge_1.Domain.T0) + (curve_1.Domain.T0);

                    user_data_point_1.TryAddLocalCoordinates(
                        brep_id_edge_1,
                        v, 0.0, 0.0);
                }
            }
            
            for (int i = 0; i < IntersectionPointList.Count(); i++)
            {
                var user_data_point = UserDataUtilities.GetOrCreateUserDataPoint(IntersectionPointList[i]);
                if (user_data_point.GetCoupling().mTopologies.Count() > 2)
                {
                    IntersectionPointList.Remove(IntersectionPointList[i]);
                    i--;
                }
            }
        }

        public static NurbsSurface GetIntersectedNurbsSurface(Curve ThisCurve, Brep ThisBrep)
        {
            var StartPtOverlCrv = ThisCurve.PointAtNormalizedLength(0.5);

            return GetIntersectedNurbsSurface(StartPtOverlCrv, ThisBrep);
        }
        public static NurbsSurface GetIntersectedNurbsSurface(Point3d ThisPoint, Brep ThisBrep)
        {
            Surface surface = null;

            ThisBrep.ClosestPoint(
                ThisPoint,
                out Point3d closest_point,
                out var component_index,
                out double s,
                out double t,
                0.1,
                out Vector3d normal);

            var oao = component_index.ComponentIndexType.GetHashCode();

            if (component_index.ComponentIndexType.GetHashCode() == 3) //3 = BrepFace
            {
                surface = ThisBrep.Faces[component_index.Index].UnderlyingSurface();
                var distance = closest_point.DistanceTo(ThisPoint);

                if (closest_point.DistanceTo(ThisPoint) > RhinoDoc.ActiveDoc.ModelAbsoluteTolerance * 100)
                    return null;
            }
            else if (component_index.ComponentIndexType.GetHashCode() == 2)
            {
                surface = ThisBrep.Faces[ThisBrep.Edges[component_index.Index]
                        .AdjacentFaces()[0]]
                    .UnderlyingSurface();
            }

            if (surface == null)
                return null;

            return surface.ToNurbsSurface();
        }

        public static UserDataSurface GetIntersectedSurface(Curve ThisCurve, Brep ThisBrep)
        {
            var StartPtOverlCrv = ThisCurve.PointAtNormalizedLength(0.5);

            return GetIntersectedSurface(StartPtOverlCrv, ThisBrep);
        }

        public static UserDataSurface GetIntersectedSurface(Point3d ThisPoint, Brep ThisBrep)
        {
            Surface surface = null;

            ThisBrep.ClosestPoint(
                ThisPoint,
                out Point3d closest_point,
                out var component_index,
                out double s,
                out double t,
                0.1,
                out Vector3d normal);

            if (component_index.ComponentIndexType.GetHashCode() == 3) //3 = BrepFace
            {
                surface = ThisBrep.Faces[component_index.Index].UnderlyingSurface();
                var distance = closest_point.DistanceTo(ThisPoint);

                if (closest_point.DistanceTo(ThisPoint) > RhinoDoc.ActiveDoc.ModelAbsoluteTolerance * 100)
                    return null;
            }
            else if (component_index.ComponentIndexType.GetHashCode() == 2)
            {
                surface = ThisBrep.Faces[ThisBrep.Edges[component_index.Index]
                        .AdjacentFaces()[0]]
                    .UnderlyingSurface();
            }

            if (surface == null)
                return null;

            return UserDataUtilities.GetOrCreateUserDataSurface(surface);
        }

        public static bool TryGetParamaterLocationSurface(Curve ThisCurve2d, NurbsSurface ThisNurbsSurface, out Elements.ParameterLocationSurface ParameterLocation)
        {
            ParameterLocation = new Elements.ParameterLocationSurface(GeometryType.SurfaceEdge, - 1, -1, -1, -1);

            double u_start = ThisNurbsSurface.KnotsU[0];
            double u_end = ThisNurbsSurface.KnotsU[ThisNurbsSurface.KnotsU.Count - 1];
            double v_start = ThisNurbsSurface.KnotsV[0];
            double v_end = ThisNurbsSurface.KnotsV[ThisNurbsSurface.KnotsV.Count - 1];

            var point_start = ThisCurve2d.PointAtStart;
            var point_end = ThisCurve2d.PointAtEnd;

            var tolerance = 0.0001;
            if (Math.Abs(point_end.X - point_start.X) < tolerance)
            {
                if ((point_start.X - u_start) < tolerance)
                {
                    ParameterLocation.mU_Normalized = 0;
                    ParameterLocation.mU = u_start;
                }
                else if ((point_start.X - u_end) < tolerance)
                {
                    ParameterLocation.mU_Normalized = 1;
                    ParameterLocation.mU = u_end;
                }
            }
            if (Math.Abs(point_end.Y - point_start.Y) < tolerance)
            {
                if ((point_start.Y - v_start) < tolerance)
                {
                    ParameterLocation.mV_Normalized = 0;
                    ParameterLocation.mV = v_start;
                }
                else if ((point_start.Y - v_end) < tolerance)
                {
                    ParameterLocation.mV_Normalized = 1;
                    ParameterLocation.mV = v_end;
                }
            }

            return (ParameterLocation.mU_Normalized != -1 || ParameterLocation.mV_Normalized != -1);
        }
    }
}
