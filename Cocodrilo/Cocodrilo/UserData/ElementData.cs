using Cocodrilo.Elements;
using Rhino;
using System.Collections.Generic;
using System.Linq;

namespace Cocodrilo.UserData
{
    public class ElementData
    {
        public int mBrepGoupId { get; set; }
        public List<int> mBrepGroupCouplingIds { get; set; }

        public List<Elements.NumericalElement> mNumericalElements { get; set; }

        public ElementData()
        {
            mBrepGoupId = 0;
            mBrepGroupCouplingIds = new List<int> { 0 };
            mNumericalElements = new List<NumericalElement>();
        }

        public void AddBrepGroupCouplingId(int BrepCouplingId)
        {
            if (mBrepGroupCouplingIds.Contains(BrepCouplingId))
                return;

            mBrepGroupCouplingIds.Add(BrepCouplingId);
        }
        public void AddBrepGroupCouplingIds(List<int> BrepCouplingIds)
        {
            foreach (int id in BrepCouplingIds)
            {
                if (mBrepGroupCouplingIds.Contains(id))
                    continue;
                mBrepGroupCouplingIds.Add(id);
            }
        }
        public bool IsBrepGroupCoupledWith(int BrepCouplingId)
        {
            foreach (int id in mBrepGroupCouplingIds)
            {
                if (id == BrepCouplingId)
                    return true;
            }
            return false;
        }
        public bool HasGeometryElementCurvesOfPropertyType(
            System.Type ThisPropertyType)
        {
            return mNumericalElements.Any(item => item.GetPropertyType() == ThisPropertyType);
        }

        public List<NumericalElement> GetNumericalElementsOfPropertyType(
            System.Type ThisPropertyType)
        {
            return mNumericalElements?.Where(item => item.GetPropertyType() == ThisPropertyType).ToList();
        }
        public List<NumericalElement> GetNumericalElementsOfPropertyType(
            System.Type ThisPropertyType,
            ParameterLocation ThisParameterLocation)
        {
            return mNumericalElements?.Where(item =>
                item.GetPropertyType() == ThisPropertyType &&
                item.mParameterLocation.Equals(ThisParameterLocation)).ToList();
        }

        public void AddNumericalElement(
            ElementProperties.Property ThisProperty,
            Elements.ParameterLocation ThisParameterLocation,
            bool overwrite = true)
        {
            if (overwrite)
                DeleteNumericalElement(ThisProperty, ThisParameterLocation);
            mNumericalElements?.Add(new NumericalElement(ThisProperty, ThisParameterLocation));
        }
        public void AddNumericalElementPoint(
            ElementProperties.Property ThisProperty,
            Elements.ParameterLocation ThisParameterLocation,
            bool overwrite = true)
        {
            if (overwrite)
                DeleteNumericalElement(ThisProperty, ThisParameterLocation);
            mNumericalElements?.Add(new NumericalElementPoint(
                ThisProperty, ThisParameterLocation as Elements.ParameterLocationSurface));
        }
        public void DeleteNumericalElementOfPropertyType(
            System.Type ThisPropertyType)
        {
            mNumericalElements?.RemoveAll(item => item.GetPropertyType() == ThisPropertyType);
        }
        public void DeleteNumericalElement(
            ElementProperties.Property ThisProperty)
        {
            int number_of_removed_elements = mNumericalElements.RemoveAll(item => item.GetProperty(out _).Equals(ThisProperty));
            RhinoApp.WriteLine(number_of_removed_elements.ToString() + " of elements have been removed");
        }
        public void DeleteNumericalElement(
            ElementProperties.Property ThisProperty,
            Elements.ParameterLocation ThisParameterLocation)
        {
            mNumericalElements?.RemoveAll(item => item.GetPropertyType() == ThisProperty.GetType() &&
                    item.mParameterLocation.Equals(ThisParameterLocation));
        }
        public void DeleteNumericalElements() => mNumericalElements?.Clear();
        public virtual bool TryAddPropertyIdsBrepIds(ref Dictionary<int, List<Cocodrilo.IO.BrepToParameterLocations>> rPropertyElements, int BrepId)
        {
            bool success = false;
            foreach (var numerical_element in mNumericalElements)
            {
                if (numerical_element.HasBrepId())
                {
                    UserDataUtilities.AddEntryToDictionary(
                        ref rPropertyElements,
                        numerical_element.GetPropertyId(),
                        numerical_element.GetBrepId(),
                        numerical_element.mParameterLocation);
                    success = true;
                }
                else
                {
                    UserDataUtilities.AddEntryToDictionary(
                        ref rPropertyElements,
                        numerical_element.GetPropertyId(),
                        BrepId,
                        numerical_element.mParameterLocation);
                    success = true;
                }
            }
            return success;
        }
    }
}
