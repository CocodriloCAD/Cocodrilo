using Cocodrilo.ElementProperties;

namespace Cocodrilo.Elements
{
    public class GeometryElementCurve : Element
    {
        public ParameterLocationCurve mParameterLocationCurve { get; set; }

        public GeometryElementCurve() : base()
        {
        }
        public GeometryElementCurve(
            Property ThisProperty,
            ParameterLocationCurve ThisParameterLocationCurve
        ) : base(ThisProperty)
        {
            mParameterLocationCurve = ThisParameterLocationCurve;
        }

    }
}