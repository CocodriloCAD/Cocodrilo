using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.ElementProperties
{
    public class PropertyDem : Property
    {
        public PropertyDem(
            GeometryType ThisGeometryType,
            int MaterialId)
            : base(ThisGeometryType, MaterialId)
        {

        }

        public override string GetKratosModelPart()
        {
            return "SpheresPart";
        }

        public override bool Equals(Property ThisProperty)
        {
            if (!(ThisProperty is PropertyDem))
                return false;
            return mMaterialId == ThisProperty.mMaterialId;
        }
    }
}
