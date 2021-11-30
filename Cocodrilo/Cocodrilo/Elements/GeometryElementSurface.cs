using Cocodrilo.ElementProperties;

namespace Cocodrilo.Elements
{
    public class GeometryElementSurface : Element
    {
        public ParameterLocationSurface mParameterLocationSurface { get; set; }

        public GeometryElementSurface() : base()
        {
        }
        public GeometryElementSurface(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface
            ):base(ThisProperty)
        {
            mParameterLocationSurface = ThisParameterLocationSurface;
        }
    }
}
