using Rhino.Geometry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.ElementProperties
{
    public struct CableProperties : IEquatable<CableProperties>
    {
        public double mPrestress { get; set; }
        public double mArea { get; set; }


        public CableProperties(
            double Prestress,
            double Area)
        {
            mPrestress = Prestress;
            mArea = Area;
        }

        public bool Equals(CableProperties comp)
        {
            return comp.mPrestress == mPrestress &&
                   comp.mArea == mArea;
        }
    }

    public enum BeamType
    {
        Standard,
        Circular,
        Rectangular
    }
    public struct BeamProperties : IEquatable<BeamProperties>
    {
        public BeamType mCrossSection { get; set; }   // 0 - no special cross section type, 1 - circular cross section, 2 - rectangular cross section

        public int mTorsionType { get; set; }

        public double mDiameter { get; set; }
        public double mHeight { get; set; }
        public double mWidth { get; set; }
        public double mArea { get; set; }

        public double mIy { get; set; }
        public double mIz { get; set; }
        public double mIt { get; set; }

        public bool mIsPrestressBending1Auto { get; set; }
        public bool mIsPrestressBending2Auto { get; set; }
        public bool mIsPrestressTorsionAuto { get; set; }

        public double mPrestress { get; set; }
        public double mPrestressBending1 { get; set; }
        public double mPrestressBending2 { get; set; }
        public double mPrestressTorsion { get; set; }

        public Vector2d mIntegration { get; set; }
        public List<double[]> mBaseVectors { get; set; }

        public BeamProperties(
            BeamType CrossSection,
            int TorsionType,
            double Diameter,
            double Height,
            double Width,
            double Area,
            double Iy,
            double Iz,
            double It,
            bool IsPrestressBending1Auto,
            bool IsPrestressBending2Auto,
            bool IsPrestressTorsionAuto,
            double Prestress,
            double PrestressBending1,
            double PrestressBending2,
            double PrestressTorsion,
            List<double[]> BaseVectors,
            double IntegrationU,
            double IntegrationV
            )
        {
            mCrossSection = BeamType.Standard;

            mTorsionType = TorsionType;

            mDiameter = Diameter;
            mHeight = Height;
            mWidth = Width;
            mArea = Area;

            mIy = Iy;
            mIz = Iz;
            mIt = It;
            if (Height > 0.0 &&
                Width > 0.0 &&
                CrossSection == BeamType.Rectangular)
            {
                mCrossSection = CrossSection;
                mDiameter = 0.0;
                mArea = Width * Height;
                mIy = Width * Math.Pow(Height, 3) / 12;
                mIz = Math.Pow(Width, 3) * Height / 12;
                mIt = Math.Pow(Math.Min(Width, Height), 3) * Math.Max(Width, Height);
            }
            else if (Diameter > 0.0 &&
                     CrossSection == BeamType.Circular)
            {
                mCrossSection = CrossSection;
                mHeight = 0.0;
                mWidth = 0.0;
                mArea = Math.Pow(Diameter, 2) / 4 * Math.PI;
                mIy = mArea / 16 * Math.Pow(Diameter, 2);
                mIz = mIy;
                mIt = Math.PI / 32 * Math.Pow(Diameter, 4);
            }

            mIsPrestressBending1Auto = IsPrestressBending1Auto;
            mIsPrestressBending2Auto = IsPrestressBending2Auto;
            mIsPrestressTorsionAuto = IsPrestressTorsionAuto;

            mPrestress = Prestress;
            mPrestressBending1 = PrestressBending1;
            mPrestressBending2 = PrestressBending2;
            mPrestressTorsion = PrestressTorsion;
            mBaseVectors = BaseVectors;
            mIntegration = new Vector2d(IntegrationU, IntegrationV);
        }

        public bool Equals(BeamProperties comp)
        {
            return comp.mCrossSection == mCrossSection &&
                   comp.mTorsionType == mTorsionType &&
                   comp.mDiameter == mDiameter &&
                   comp.mHeight == mHeight &&
                   comp.mWidth == mWidth &&
                   comp.mArea == mArea &&
                   comp.mIy == mIy &&
                   comp.mIz == mIz &&
                   comp.mIt == mIt &&
                   comp.mIsPrestressBending1Auto == mIsPrestressBending1Auto &&
                   comp.mIsPrestressBending2Auto == mIsPrestressBending2Auto &&
                   comp.mIsPrestressTorsionAuto == mIsPrestressTorsionAuto &&
                   comp.mPrestress == mPrestress &&
                   comp.mPrestressBending1 == mPrestressBending1 &&
                   comp.mPrestressBending2 == mPrestressBending2 &&
                   comp.mPrestressTorsion == mPrestressTorsion &&
                   comp.mIntegration == mIntegration &&
                   comp.mBaseVectors.Equals(mBaseVectors);
        }
    }
}
