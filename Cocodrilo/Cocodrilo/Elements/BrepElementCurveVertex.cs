using Cocodrilo.ElementProperties;
using Cocodrilo.UserData;

namespace Cocodrilo.Elements
{
    public class BrepElementCurveVertex : Element
    {
        public ParameterLocationCurve mParameterLocationCurve { get; set; }
        /// <summary>
        /// mUserDataPoint provides the brep id of the underlying
        /// geometrical point. It is set in the constructor of the BRepElementVertex
        /// and cannot be modified later.
        /// </summary>
        public readonly UserDataPoint mUserDataPoint;

        public BrepElementCurveVertex() : base()
        {
        }
        public BrepElementCurveVertex(
            Property ThisProperty,
            ParameterLocationCurve ThisParameterLocationCurve
        ) : base(ThisProperty)
        {
            mParameterLocationCurve = ThisParameterLocationCurve;

            mUserDataPoint = new UserDataPoint();
        }

        public int GetBrepId() => mUserDataPoint.BrepId;

        public double GetPoint()
        {
            return mParameterLocationCurve.mU;
        }
    }
}