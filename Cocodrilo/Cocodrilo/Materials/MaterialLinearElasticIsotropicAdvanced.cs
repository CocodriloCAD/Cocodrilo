using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace Cocodrilo.Materials
{
    public class MaterialLinearElasticIsotropicAdvanced : MaterialLinearElasticIsotropic
    {
        public double mThickness { get; set; }
        public bool mAddSelfWeight { get; set; }
        public Vector3d mGravity { get; set; }
        public double mYieldStressCompresion { get; set; }
        public double mYieldStressTension { get; set; }
        public MaterialLinearElasticIsotropicAdvanced()
        {
        }

        public MaterialLinearElasticIsotropicAdvanced(
            String Name,
            double YoungsModulus,
            double Nue,
            double Density,
            double Thickness,
            bool AddSelfWeight,
            Vector3d Gravity,
            double YieldStressCompresion,
            double YieldStressTension)
            : base(Name, YoungsModulus, Nue, Density)
        {
            mThickness = Thickness;
            mAddSelfWeight = AddSelfWeight;
            mGravity = Gravity;
            mYieldStressCompresion = YieldStressCompresion;
            mYieldStressTension = YieldStressTension;
        }

        public override Dictionary<string, object> GetKratosVariables()
        {
            return new Dictionary<string, object>
            {
                {"YOUNG_MODULUS", YoungsModulus},
                {"POISSON_RATIO", Nue},
                {"THICKNESS", mThickness},
                {"DENSITY", Density},
                {"ADD_SELF_WEIGHT", mAddSelfWeight},
                {"GRAVITY", new double[]{ mGravity.X, mGravity.Y, mGravity.Z } },
                {"YIELD_STRESS_COMPRESSION", mYieldStressCompresion},
                {"YIELD_STRESS_TENSION", mYieldStressTension},
            };
        }

        public override Dictionary<string, object> GetKratosConstitutiveLaw()
        {
            return new Dictionary<string, object>
            {
                {"name", "LinearElasticPlaneStress2DLaw"}
            };
        }

        public override bool Equals(Material comparison)
        {
            var mat = comparison as MaterialLinearElasticIsotropicAdvanced;

            return (base.Equals(mat) &&
                    mat.mThickness == mThickness &&
                    mat.mAddSelfWeight == mAddSelfWeight &&
                    mat.mGravity == mGravity &&
                    mat.mYieldStressCompresion == mYieldStressCompresion &&
                    mat.mYieldStressTension == mYieldStressTension);
        }
    }
}
