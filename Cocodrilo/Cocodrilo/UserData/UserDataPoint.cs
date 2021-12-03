using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Cocodrilo.Elements;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using System.Web.UI.WebControls;
using Rhino.DocObjects;
using Cocodrilo.ElementProperties;


namespace Cocodrilo.UserData
{
    [Guid("E9F92FF3-F067-46BB-8F93-AD14B5C13654")]
    public class UserDataPoint : UserDataCocodrilo
    {
        public UserDataPoint() : base()
        {
        }

        public override void AddNumericalElement(
            Property ThisProperty,
            bool Overwrite = true,
            int StageId = -1)
        {
            var parameter_location_curve = new ParameterLocationPoint(GeometryType.Point);

            GetCurrentElementData(StageId).AddNumericalElement(
                ThisProperty,
                parameter_location_curve,
                Overwrite);
        }

        public bool TryAddLocalCoordinates(int BrepId, double x, double y, double z)
        {
            if (mCoupling.IsCoupledWith(BrepId))
                if (mCoupling.TryAddTrimIndexToBrepId(BrepId, x, y, z))
                    return true;
            return false;
        }

        public override Refinement.Refinement GetRefinement(int StageId = -1)
        {
            return null;
        }

        public override void ChangeRefinement(
            Refinement.Refinement ThisRefinement,
            int StageId = -1)
        {
            Rhino.RhinoApp.WriteLine("WARNING: Trying to assign a refinement to a point.");
        }
    }
}
