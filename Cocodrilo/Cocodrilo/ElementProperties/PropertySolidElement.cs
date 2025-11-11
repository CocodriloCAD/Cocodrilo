using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.ElementProperties
{
    public struct SolidElementProperties : IEquatable<SolidElementProperties>
    {
        public double mThickness { get; set; }

        public SolidElementProperties(
            double Thickness)
        {
            mThickness = Thickness;
        }

        public bool Equals(SolidElementProperties comp)
        {
            return comp.mThickness == mThickness; 
        }
    }

    public class PropertySolidElement : Property
    {
        public SolidElementProperties mSolidElementProperties { get; set; }

        public PropertySolidElement() : base()
        {
        }

        public PropertySolidElement(
            int mMaterialId,
            SolidElementProperties ThisSolidElementProperties)
            : base(
                GeometryType.Mesh,
                mMaterialId)
        {
            mSolidElementProperties = ThisSolidElementProperties;
        }

        public override string GetKratosModelPart()
        {
            //base.GetKratosModelPart(); // überschriebene base class function

            return "Solid_Auto_" + mPropertyId; //anpassen
                       
        }
    }


}

