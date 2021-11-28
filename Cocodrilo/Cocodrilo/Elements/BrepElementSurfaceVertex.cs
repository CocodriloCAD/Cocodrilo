using Rhino.Geometry;
using Cocodrilo.ElementProperties;
using Cocodrilo.UserData;

namespace Cocodrilo.Elements
{
    public class BrepElementSurfaceVertex : Element
    {
        public ParameterLocationSurface mParameterLocationSurface { get; set; }
        /// <summary>
        /// mUserDataPoint provides the brep id of the underlying
        /// geometrical point. It is set in the constructor of the BRepElementVertex
        /// and cannot be modified later.
        /// </summary>
        public readonly UserDataPoint mUserDataPoint;

        public BrepElementSurfaceVertex() : base()
        {
        }

        public BrepElementSurfaceVertex(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface
            ):base(ThisProperty)
        {
            mParameterLocationSurface = ThisParameterLocationSurface;

            mUserDataPoint = new UserDataPoint();
        }

        public int GetBrepId() => mUserDataPoint.BrepId;

        public Point2d GetPoint2d()
        {
            return new Point2d(
                mParameterLocationSurface.mU,
                mParameterLocationSurface.mV);
        }
    }
}
