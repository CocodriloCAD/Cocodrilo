using System;
using System.Collections.Generic;

namespace Cocodrilo.Materials
{
    public class MaterialLinearElasticIsotropic : Material
    {
        public double YoungsModulus { get; set; }
        public double Nue { get; set; }
        public MaterialLinearElasticIsotropic()
        {
        }

        public MaterialLinearElasticIsotropic(
            String Name, double YoungsModulus = 200000, double Nue = 0.0)
            : base(Name)
        {
            this.YoungsModulus = YoungsModulus;
            this.Nue = Nue;
        }

        public override Dictionary<string, object> GetKratosVariables()
        {
            return new Dictionary<string, object>
            {
                {"YOUNG_MODULUS", YoungsModulus},
                {"POISSON_RATIO", Nue}
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
            var mat = comparison as MaterialLinearElasticIsotropic;

            return (base.Equals(mat) &&
                    mat.YoungsModulus == YoungsModulus &&
                    mat.Nue == Nue); ;
        }
    }
}
