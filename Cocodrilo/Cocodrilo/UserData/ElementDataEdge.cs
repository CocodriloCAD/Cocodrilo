using System;
using Rhino;
using System.Collections.Generic;
using System.Linq;
using Cocodrilo.ElementProperties;
using Cocodrilo.Elements;
using Cocodrilo.Refinement;
using static Cocodrilo.UserData.UserDataUtilities;

namespace Cocodrilo.UserData
{
    public class ElementDataEdge : ElementData
    {
        public List<BrepElementEdge> mBrepElementEdges { get; set; }

        public RefinementEdge mRefinementEdge { get; set; }

        public List<int> mAdditionalPropertyIds { get; set; }

        public ElementDataEdge():base()
        {
            mBrepElementEdges = new List<BrepElementEdge>();

            mRefinementEdge = new RefinementEdge();

            mAdditionalPropertyIds = new List<int>();
        }

        #region Additional Properties
        public void TryAddAdditionalPropertyIds(List<int> PropertyIds)
        {
            foreach (var id in PropertyIds)
            {
                if (!mAdditionalPropertyIds.Contains(id))
                    mAdditionalPropertyIds.Add(id);
            }
        }
        #endregion
        #region BrepElementEdge
        public void AddBrepElementEdge(
            Property ThisProperty,
            bool overwrite = true)
        {
            if (overwrite)
                DeleteBrepElementEdge(ThisProperty);
            mBrepElementEdges.Add(new BrepElementEdge(ThisProperty));
        }
        public void DeleteBrepElementEdgePropertyType(Type ThisPropertyType)
        {
            int num = mBrepElementEdges.RemoveAll(
                brep_element_edge => brep_element_edge.GetPropertyType() == ThisPropertyType);
            if (num > 0)
                RhinoApp.WriteLine(num + " BrepElementEdges removed.");
        }
        public void DeleteBrepElementEdge(Property ThisProperty)
        {
            int num = mBrepElementEdges.RemoveAll(
                brep_element_edge => brep_element_edge.GetPropertyType() == ThisProperty.GetType()
                        && brep_element_edge.GetProperty(out _).Equals(ThisProperty));
            if (num > 0)
                RhinoApp.WriteLine(num + " BrepElementEdges removed.");
        }
        public void DeleteBrepElementEdges() => mBrepElementEdges.Clear();

        public List<BrepElementEdge> GetBrepElementEdgeOfPropertyType(Type ThisPropertyType)
        {
            return mBrepElementEdges.Where(
                brep_element_edge => brep_element_edge.GetPropertyType() == ThisPropertyType).ToList();
        }
        public List<BrepElementEdge> GetBrepElementEdges()
        {
            return mBrepElementEdges;
        }
        #endregion
        #region Input
        public List<int> GetPropertyIds()
        {
            var property_ids = mBrepElementEdges.Select(
                brep_element_edge => brep_element_edge.GetPropertyId()).ToList();
            property_ids.AddRange(mAdditionalPropertyIds);
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
            foreach (var brep_element_edge in mBrepElementEdges)
            {
                AddEntryToDictionary(
                    ref rPropertyElements,
                    brep_element_edge.GetPropertyId(),
                    BrepId);
                success = true;
            }
            foreach (var id in mAdditionalPropertyIds)
            {
                AddEntryToDictionary(
                    ref rPropertyElements,
                    id,
                    BrepId);
                success = true;
            }
            return success;
        }
        #endregion
    }
}
