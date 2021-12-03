using Cocodrilo.ElementProperties;

namespace Cocodrilo.Elements
{
    public class NumericalElement
    {
        public int mPropertyId { get; set; }
        public ParameterLocation mParameterLocation { get; set; }

        public NumericalElement()
        {
        }
        public NumericalElement(Property ThisProperty, ParameterLocation ThisParameterLocation)
        {
            CocodriloPlugIn.Instance.AddProperty(ThisProperty);
            Rhino.RhinoApp.WriteLine("Property: " + ThisProperty.ToString() + " with id: " + ThisProperty.mPropertyId + " was added.");
            mPropertyId = ThisProperty.mPropertyId;
            mParameterLocation = ThisParameterLocation;
        }
        public virtual bool HasBrepId()
        {
            return false;
        }
        public virtual void SetBrepId(int BrepId) { }
        public virtual int GetBrepId()
        {
            return -1;
        }
        public bool HasProperty()
        {
            return CocodriloPlugIn.Instance.HasProperty(mPropertyId);
        }
        public Property GetProperty(out bool success)
        {
            return CocodriloPlugIn.Instance.GetProperty(mPropertyId, out success);
        }
        public T GetProperty<T>() where T : Property
        {
            return GetProperty(out _) as T;
        }
        public System.Type GetPropertyType()
        {
            var property = GetProperty(out bool success);
            return success ? property.GetType() : null;
        }
        public int GetPropertyId()
        {
            return mPropertyId;
        }
    }
}
