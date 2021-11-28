using Cocodrilo.ElementProperties;

namespace Cocodrilo.Elements
{
    public class Element
    {
        public int mPropertyId { get; set; }

        protected Element()
        {
        }
        protected Element(Property ThisProperty)
        {
            CocodriloPlugIn.Instance.AddProperty(ThisProperty);
            Rhino.RhinoApp.WriteLine("Property: " + ThisProperty.ToString() + " with id: " + ThisProperty.mPropertyId + " was added.");
            mPropertyId = ThisProperty.mPropertyId;
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
