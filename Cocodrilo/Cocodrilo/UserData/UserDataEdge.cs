using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Cocodrilo.Elements;
using System.Runtime.InteropServices;
using System.Web.Script.Serialization;
using Cocodrilo.ElementProperties;
using Cocodrilo.Refinement;

namespace Cocodrilo.UserData
{
    [Guid("E9F92FF3-F067-46BB-8F93-AD14B5C13554")]
    public class UserDataEdge : UserDataCocodrilo
    {
        public UserDataEdge() : base() { }

        public override void AddNumericalElement(
            Property ThisProperty,
            bool Overwrite = true,
            int StageId = -1)
        {
            var parameter_location_curve = new ParameterLocationCurve(GeometryType.SurfaceEdge, -1, -1);

            GetCurrentElementData(StageId).AddNumericalElement(
                ThisProperty,
                parameter_location_curve,
                Overwrite);
        }

        public override Refinement.Refinement GetRefinement(int StageId = -1)
        {
            return null;
        }

        public override void ChangeRefinement(
            Refinement.Refinement ThisRefinement,
            int StageId = -1)
        {
            Rhino.RhinoApp.WriteLine("WARNING: Trying to assign a refinement to an edge.");
        }
    }
}
