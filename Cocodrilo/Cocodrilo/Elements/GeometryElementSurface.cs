using Cocodrilo.ElementProperties;

namespace Cocodrilo.Elements
{
    public class GeometryElementSurface : Element
    {
        public int mElementId { get; set; }

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

            mElementId = -1;
        }
    }
}
