using Rhino;
using System;
using System.Collections.Generic;
using System.Windows.Forms.VisualStyles;
using Cocodrilo.Elements;
using Cocodrilo.Refinement;
using Cocodrilo.ElementProperties;

namespace Cocodrilo.UserData
{
    public class ElementDataCurve : ElementData
    {
        public List<BrepElementCurveVertex> mBrepElementCurveVertices { get; set; }

        public List<GeometryElementCurve> mGeometryElementCurves { get; set; }

        public RefinementCurve mRefinementCurve { get; set; }

        public ElementDataCurve()
        {
            mBrepElementCurveVertices = new List<BrepElementCurveVertex>();

            mGeometryElementCurves = new List<GeometryElementCurve>();

            mRefinementCurve = new RefinementCurve();
        }


        #region BrepElementCurveVertex
        public void AddBrepElementCurveVertex(
            Property ThisProperty,
            ParameterLocationCurve ThisParameterLocationCurve,
            bool overwrite = true)
        {
            if (overwrite)
                DeleteBrepElementCurveVertex(ThisProperty, ThisParameterLocationCurve);
            mBrepElementCurveVertices.Add(
                new BrepElementCurveVertex(
                    ThisProperty,
                    ThisParameterLocationCurve));
        }

        public void DeleteBrepElementCurveVertexPropertyType(
            Type ThisPropertyType)
        {
            int num = mBrepElementCurveVertices.RemoveAll(
                item => item.GetPropertyType() == ThisPropertyType);
            if (num > 0)
                RhinoApp.WriteLine(num + " BrepElementCurveVertices removed.");
        }

        public void DeleteBrepElementCurveVertices() => mBrepElementCurveVertices.Clear();

        public void DeleteBrepElementCurveVertex(
            Property ThisProperty,
            ParameterLocationCurve ThisParameterLocationCurve)
        {
            int num = mBrepElementCurveVertices.RemoveAll(
                item => item.GetPropertyType() == ThisProperty.GetType()
                        && item.mParameterLocationCurve.Equals(ThisParameterLocationCurve));
            if (num > 0)
                RhinoApp.WriteLine(num + " BrepElementCurveVertices removed.");
            //foreach (var brep_element_vertex in mBrepElementCurveVertices)
            //{
            //    if (brep_element_vertex.GetPropertyType() != ThisProperty.GetType())
            //        continue;
            //    if (brep_element_vertex.GetProperty().Equals(ThisProperty))
            //    {
            //        if (brep_element_vertex.mParameterLocationCurve.Equals(ThisParameterLocationCurve))
            //        {
            //            RhinoApp.WriteLine(
            //                "BrepElementVertex with property of type " + ThisProperty.GetType().Name
            //                + " with property id: " +
            //                ThisProperty.mPropertyId + " removed.");
            //            mBrepElementCurveVertices.Remove(brep_element_vertex);
            //        }
            //    }
            //}
        }

        public List<BrepElementCurveVertex> GetBrepElementCurveVerticesOfPropertyType(
            Type ThisPropertyType,
            ParameterLocationCurve ThisParameterLocationCurve)
        {
            var brep_element_vertex_list = new List<BrepElementCurveVertex>();
            foreach (var brep_element_vertex in mBrepElementCurveVertices)
            {
                if (brep_element_vertex.GetPropertyType() == ThisPropertyType)
                {
                    if (brep_element_vertex.mParameterLocationCurve.Equals(ThisParameterLocationCurve))
                    {
                        brep_element_vertex_list.Add(brep_element_vertex);
                    }
                }
            }
            return brep_element_vertex_list;
        }
        public List<BrepElementCurveVertex> GetBrepElementCurveVerticesOfPropertyType(
            Type ThisPropertyType)
        {
            var brep_element_vertex_list = new List<BrepElementCurveVertex>();
            foreach (var brep_element_vertex in mBrepElementCurveVertices)
            {
                if (brep_element_vertex.GetPropertyType() == ThisPropertyType)
                {
                    brep_element_vertex_list.Add(brep_element_vertex);
                }
            }
            return brep_element_vertex_list;
        }
        #endregion
        #region GeometryElementCurve
        public void AddGeometryElementCurve(
            Property ThisProperty,
            ParameterLocationCurve ThisParameterLocationCurve,
            bool overwrite = true)
        {
            if (overwrite)
                DeleteGeometryElementCurve(ThisProperty, ThisParameterLocationCurve);
            mGeometryElementCurves.Add(
                new GeometryElementCurve(
                    ThisProperty,
                    ThisParameterLocationCurve));
        }

        public void DeleteGeometryElementCurvePropertyType(
            Type ThisPropertyType)
        {
            foreach (var curve_element in mGeometryElementCurves)
            {
                if (curve_element.GetPropertyType() == ThisPropertyType)
                {
                    RhinoApp.WriteLine(
                        "GeometryElementCurve with property of type " + ThisPropertyType.GetType().Name
                        + " with property id: " + curve_element.GetProperty(out _) + " removed.");
                    mGeometryElementCurves.Remove(curve_element);
                }
            }
        }
        public void DeleteGeometryElementCurves() => mGeometryElementCurves.Clear();

        public void DeleteGeometryElementCurve(
            Property ThisProperty,
            ParameterLocationCurve ThisParameterLocationCurve)
        {
            foreach (var curve_element in mGeometryElementCurves)
            {
                if (curve_element.GetPropertyType() != ThisProperty.GetType())
                    continue;
                if (curve_element.GetProperty(out _).Equals(ThisProperty) &&
                    curve_element.mParameterLocationCurve.Equals(ThisParameterLocationCurve))
                {
                    RhinoApp.WriteLine(
                        "GeometryElementCurve with property of type " + ThisProperty.GetType().Name
                        + " with property id: " +
                        ThisProperty.mPropertyId + " removed.");
                    mGeometryElementCurves.Remove(curve_element);
                }
            }
        }

        public List<GeometryElementCurve> GetGeometryElementCurvesOfPropertyType(
            Type ThisPropertyType,
            ParameterLocationCurve ThisParameterLocationCurve)
        {
            var curve_element_list = new List<GeometryElementCurve>();
            foreach (var curve_element in mGeometryElementCurves)
            {
                if (curve_element.GetPropertyType() == ThisPropertyType)
                {
                    if (curve_element.mParameterLocationCurve.Equals(ThisParameterLocationCurve))
                    {
                        curve_element_list.Add(curve_element);
                    }
                }
            }
            return curve_element_list;
        }
        public List<GeometryElementCurve> GetGeometryElementCurvesOfPropertyType(
            Type ThisPropertyType)
        {
            var curve_element_list = new List<GeometryElementCurve>();
            foreach (var curve_element in mGeometryElementCurves)
            {
                if (curve_element.GetPropertyType() == ThisPropertyType)
                {
                    curve_element_list.Add(curve_element);
                }
            }
            return curve_element_list;
        }
        #endregion

        #region KRATOS input
        public bool TryAddPropertyIdsBrepIds(ref Dictionary<int, List<int>> rPropertyElements, int BrepId)
        {
            bool success = false;
            foreach (var curve_vertex in mBrepElementCurveVertices)
            {
                UserDataUtilities.AddEntryToDictionary(
                    ref rPropertyElements,
                    curve_vertex.GetPropertyId(),
                    curve_vertex.mUserDataPoint.BrepId);
                success = true;
            }
            foreach (var geometry_curve in mGeometryElementCurves)
            {
                UserDataUtilities.AddEntryToDictionary(
                    ref rPropertyElements,
                    geometry_curve.GetPropertyId(),
                    BrepId);
                success = true;
            }
            return success;
        }
        #endregion

    }
}
