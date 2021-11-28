using System;
using Rhino;
using System.Collections.Generic;
using System.Linq;
using Cocodrilo.ElementProperties;
using Cocodrilo.Elements;

using static Cocodrilo.UserData.UserDataUtilities;

namespace Cocodrilo.UserData
{
    public class ElementDataPoint : ElementData
    {
        public List<GeometryElementVertex> mGeometryElementVertices { get; set; }

        public List<int> mAdditionalPropertyIds { get; set; }

        public ElementDataPoint():base()
        {
            mGeometryElementVertices = new List<GeometryElementVertex>();

            mAdditionalPropertyIds = new List<int>();
        }

        #region Additional Properties

        public void TryAddAdditionalPropertyIds(List<int> PropertyIds)
        {
            foreach (var id in PropertyIds)
            {
                int i = id;
                bool success = false;
                foreach (var existing_id in mAdditionalPropertyIds)
                    if (existing_id == id)
                    {
                        success = true;
                        continue;
                    }
                if (!success)
                    mAdditionalPropertyIds.Add(id);
            }
        }

        #endregion

        #region GeometryElementVertex
        public void AddGeometryElementVertex(
            Property ThisProperty,
            bool overwrite = true)
        {
            if (overwrite)
                DeleteGeometryElementVertex(ThisProperty);
            mGeometryElementVertices.Add(new GeometryElementVertex(ThisProperty));
        }

        public void DeleteGeometryElementVertexPropertyType(Type ThisPropertyType)
        {
            foreach (var brep_element_point in mGeometryElementVertices)
            {
                if (brep_element_point.GetPropertyType() == ThisPropertyType)
                {
                    RhinoApp.WriteLine(
                        "GeometryElementVertex with property of type " + ThisPropertyType.Name
                        + " with property id: " + brep_element_point.GetProperty(out _) + " removed.");
                    mGeometryElementVertices.Remove(brep_element_point);
                }
            }
        }

        public void DeleteGeometryElementVertex(Property ThisProperty)
        {
            int num = mGeometryElementVertices.RemoveAll(
                item => item.GetPropertyType() == ThisProperty.GetType());
            if (num > 0)
                RhinoApp.WriteLine(num + " GeometryElementVertices removed.");
        }

        public void DeleteGeometryElementVertices() => mGeometryElementVertices.Clear();

        public List<GeometryElementVertex> GetGeometryElementVertexOfPropertyType(Type ThisPropertyType)
        {
            var brep_element_point_list = new List<GeometryElementVertex>();
            foreach (var brep_element_point in mGeometryElementVertices)
            {
                if (brep_element_point.GetPropertyType() == ThisPropertyType)
                {
                    brep_element_point_list.Add(brep_element_point);
                }
            }
            return brep_element_point_list;
        }

        public List<GeometryElementVertex> GetGeometryElementVertices()
        {
            return mGeometryElementVertices;
        }
        #endregion

        #region Input
        public List<int> GetPropertyIds()
        {
            var property_ids = new List<int>();
            foreach (var brep_element_vertex in mGeometryElementVertices)
            {
                property_ids.Add(brep_element_vertex.GetPropertyId());
            }
            foreach (var id in mAdditionalPropertyIds)
            {
                property_ids.Add(id);
            }

            return property_ids;
        }
        #endregion

        #region KRATOS input
        public bool TryAddPropertyIdsBrepIds(
            ref Dictionary<int,
            List<int>> rPropertyElements,
            int BrepId)
        {
            bool success = false;
            foreach (var vertex in mGeometryElementVertices)
            {
                AddEntryToDictionary(
                    ref rPropertyElements,
                    vertex.GetPropertyId(),
                    BrepId);
                success = true;
            }
            foreach (var id in mAdditionalPropertyIds)
            {
                AddEntryToDictionary(
                    ref rPropertyElements,
                    id,
                    BrepId);
            }
            return success;
        }
        #endregion
    }
}
