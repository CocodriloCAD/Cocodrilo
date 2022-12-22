using Rhino.Geometry;
using System;
using System.Collections.Generic;

namespace Cocodrilo.Materials
{
    public class MaterialMultiElasticityAdvanced : Material
    {
        public List<double> mYoungsModulusList { get; set; }
        public List<double> mEpsilonList { get; set; }
        public double mNue { get; set; }
        public double mDensity { get; set; }
        public double mThickness { get; set; }
        public bool mAddSelfWeight { get; set; }
        public Vector3d mGravity { get; set; }
        public double mYieldStressCompresion { get; set; }
        public double mYieldStressTension { get; set; }
        public MaterialMultiElasticityAdvanced()
        {
        }

        public MaterialMultiElasticityAdvanced(
            String Name,
            List<double> YoungsModulusList,
            List<double> EpsilonList,
            double Nue,
            double Density,
            double Thickness,
            bool AddSelfWeight,
            Vector3d Gravity,
            double YieldStressCompresion,
            double YieldStressTension)
            : base(Name)
        {
            mYoungsModulusList = YoungsModulusList;
            mEpsilonList = EpsilonList;
            mNue = Nue;
            mDensity = Density;
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
                {"MULTI_LINEAR_ELASTICITY_MODULI", mYoungsModulusList},
                {"MULTI_LINEAR_ELASTICITY_STRAINS", mEpsilonList},
                {"POISSON_RATIO", mNue},
                {"DENSITY", mDensity},
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
                {"name", "MultiLinearIsotropicPlaneStress2D"}
            };
        }

        public override bool Equals(Material comparison)
        {
            var mat = comparison as MaterialMultiElasticityAdvanced;

            return (base.Equals(mat) &&
                    mat.mYoungsModulusList == mYoungsModulusList &&
                    mat.mEpsilonList == mEpsilonList &&
                    mat.mNue == mNue &&
                    mat.mDensity == mDensity &&
                    mat.mThickness == mThickness &&
                    mat.mAddSelfWeight == mAddSelfWeight &&
                    mat.mGravity == mGravity &&
                    mat.mYieldStressCompresion == mYieldStressCompresion &&
                    mat.mYieldStressTension == mYieldStressTension);
        }
    }
}
