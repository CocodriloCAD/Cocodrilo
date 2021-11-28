using Rhino;
using System;
using System.Collections.Generic;
using Rhino.DocObjects;
using Cocodrilo.Elements;
using Rhino.Geometry;
using Cocodrilo.ElementProperties;
using Cocodrilo.Refinement;

namespace Cocodrilo.UserData
{
    public class ElementDataSurface : ElementData
    {
        public List<BrepElementSurfaceVertex> mBrepElementSurfaceVertices { get; set; }

        public List<GeometryElementSurface> mGeometryElementSurfaces { get; set; }

        public RefinementSurface mRefinementSurface { get; set; }


        public ElementDataSurface()
        {
            mBrepElementSurfaceVertices = new List<BrepElementSurfaceVertex>();

            mGeometryElementSurfaces = new List<GeometryElementSurface>();

            mRefinementSurface = new RefinementSurface();
        }

        #region BrepElementSurfaceVertex
        public void AddBrepElementSurfaceVertex(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface,
            bool overwrite = true)
        {
            if (overwrite)
                DeleteBrepElementSurfaceVertex(ThisProperty, ThisParameterLocationSurface);
            mBrepElementSurfaceVertices.Add(
                new BrepElementSurfaceVertex(
                    ThisProperty,
                    ThisParameterLocationSurface));
        }

        public void DeleteBrepElementSurfaceVertexPropertyType(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface)
        {
            int num = mBrepElementSurfaceVertices.RemoveAll(
                item => item.GetPropertyType() == ThisProperty.GetType());
            if (num > 0)
                RhinoApp.WriteLine(num + " BrepElementVertices removed.");
        }

        public void DeleteBrepElementSurfaceVertex(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface)
        {
            int num = mBrepElementSurfaceVertices.RemoveAll(
                item => (item.GetPropertyType() == ThisProperty.GetType()
                         && item.mParameterLocationSurface.Equals(ThisParameterLocationSurface)));
            if (num > 0)
                RhinoApp.WriteLine(num + " BrepElementVertices removed.");
        }

        public void DeleteBrepElementSurfaceVertices() => mBrepElementSurfaceVertices.Clear();

        public List<BrepElementSurfaceVertex> GetBrepElementSurfaceVerticesOfPropertyType(
            Type ThisPropertyType,
            ParameterLocationSurface ThisParameterLocationSurface)
        {
            var brep_element_vertex_list = new List<BrepElementSurfaceVertex>();
            foreach (var brep_element_vertex in mBrepElementSurfaceVertices)
            {
                if (brep_element_vertex.GetPropertyType() == ThisPropertyType)
                {
                    if (brep_element_vertex.mParameterLocationSurface.Equals(ThisParameterLocationSurface))
                    {
                        brep_element_vertex_list.Add(brep_element_vertex);
                    }
                }
            }
            return brep_element_vertex_list;
        }
        public List<BrepElementSurfaceVertex> GetBrepElementSurfaceVerticesOfPropertyType(
            Type ThisPropertyType)
        {
            var brep_element_vertex_list = new List<BrepElementSurfaceVertex>();
            foreach (var brep_element_vertex in mBrepElementSurfaceVertices)
            {
                if (brep_element_vertex.GetPropertyType() == ThisPropertyType)
                {
                    brep_element_vertex_list.Add(brep_element_vertex);
                }
            }
            return brep_element_vertex_list;
        }
        #endregion


        #region GeometryElementSurface
        public void AddGeometryElementSurface(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface,
            bool overwrite = true)
        {
            if (overwrite)
                DeleteGeometryElementSurface(
                    ThisProperty,
                    ThisParameterLocationSurface);
            mGeometryElementSurfaces.Add(
                new GeometryElementSurface(
                    ThisProperty,
                    ThisParameterLocationSurface));
        }

        public void DeleteGeometryElementSurfacePropertyType(
            Type ThisPropertyType)
        {
            int num = mGeometryElementSurfaces.RemoveAll(item => item.GetPropertyType() == ThisPropertyType);
            if (num > 0)
                RhinoApp.WriteLine(num + " GeometryElementSurfaces removed.");
        }

        public void DeleteGeometryElementSurface(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface)
        {
            int num = mGeometryElementSurfaces.RemoveAll(item =>
                (item.GetPropertyType() == ThisProperty.GetType())
                && item.mParameterLocationSurface.Equals(ThisParameterLocationSurface));
            if (num > 0)
                RhinoApp.WriteLine(num + " GeometryElementSurfaces removed.");
        }

        public void DeleteGeometryElementSurfaces() => mGeometryElementSurfaces.Clear();

        public List<GeometryElementSurface> GetGeometryElementSurfacesOfPropertyType(
            Type ThisPropertyType,
            ParameterLocationSurface ThisParameterLocationSurface)
        {
            var surface_element_list = new List<GeometryElementSurface>();
            foreach (var surface_element in mGeometryElementSurfaces)
            {
                if (surface_element.GetPropertyType() == ThisPropertyType)
                {
                    if (surface_element.mParameterLocationSurface.Equals(ThisParameterLocationSurface))
                    {
                        surface_element_list.Add(surface_element);
                    }
                }
            }
            return surface_element_list;
        }
        public List<GeometryElementSurface> GetGeometryElementSurfacesOfPropertyType(
            Type ThisPropertyType)
        {
            var surface_element_list = new List<GeometryElementSurface>();
            foreach (var surface_element in mGeometryElementSurfaces)
            {
                if (surface_element.GetPropertyType() == ThisPropertyType)
                {
                    surface_element_list.Add(surface_element);
                }
            }
            return surface_element_list;
        }
        #endregion

        #region KRATOS input
        public bool TryAddPropertyIdsBrepIds(ref Dictionary<int, List<int>> rPropertyElements, int BrepId)
        {
            bool success = false;
            /// BREP ELEMENTS HAVE THEIR OWN BREP ID ///
            foreach (var surface_vertex in mBrepElementSurfaceVertices)
            {
                UserDataUtilities.AddEntryToDictionary(
                    ref rPropertyElements,
                    surface_vertex.GetPropertyId(),
                    surface_vertex.mUserDataPoint.BrepId);
                success = true;
            }
            foreach (var geometry_surface in mGeometryElementSurfaces)
            {
                UserDataUtilities.AddEntryToDictionary(
                    ref rPropertyElements,
                    geometry_surface.GetPropertyId(),
                    BrepId);
                success = true;
            }
            return success;
        }
        #endregion
    }
}
