using Rhino.Geometry;
using Cocodrilo.ElementProperties;
using Cocodrilo.UserData;

namespace Cocodrilo.Elements
{
    public class NumericalElementPoint : NumericalElement
    {
        public ParameterLocationSurface mParameterLocationSurface { get; set; }
        /// <summary>
        /// mUserDataPoint provides the brep id of the underlying
        /// geometrical point. It is set in the constructor of the BRepElementVertex
        /// and cannot be modified later.
        /// </summary>
        public readonly UserDataPoint mUserDataPoint;

        public NumericalElementPoint() : base()
        {
        }

        public NumericalElementPoint(
            Property ThisProperty,
            ParameterLocationSurface ThisParameterLocationSurface
            ):base(ThisProperty, ThisParameterLocationSurface)
        {
            mParameterLocationSurface = ThisParameterLocationSurface;

            mUserDataPoint = new UserDataPoint();
        }

        public override bool HasBrepId()
        {
            return true;
        }
        public override void SetBrepId(int BrepId)
        {
            mUserDataPoint.BrepId = BrepId;
        }
        public override int GetBrepId() => mUserDataPoint.BrepId;

        public Point2d GetPoint2d()
        {
            return new Point2d(
                mParameterLocationSurface.mU,
                mParameterLocationSurface.mV);
        }
    }
}
