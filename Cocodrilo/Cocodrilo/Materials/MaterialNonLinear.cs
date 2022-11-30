using System;
using System.Collections.Generic;

namespace Cocodrilo.Materials
{
    public class MaterialNonLinear : Material
    {
        //fields
        public string constitutiveLaw { get; set; }

        public double density { get; set; }

        public double youngsModulus { get; set; }

        public double poissonRatio { get; set; }

        public double cohesion { get; set; }

        public double internalFrictionAngle { get; set; }

        public double internalDilatancyAngle { get; set; }

        public int particlesPerElement { get; set; }

        public double thickness { get; set; }



        public MaterialNonLinear()
        {
        }

        public MaterialNonLinear(
            String Name, string ConstitutiveLaw, double Density = 1.0, double YoungsModulus = 200000, double Nue = 0.0, double Cohesion = 0.0, double InternalFrictionAngle = 0.0, 
            double InternalDilatationAngle = 0.0, int NumberOfParticles = 3, double Thickness = 1.0)
            : base(Name)
        {
            this.constitutiveLaw = ConstitutiveLaw;
            this.density = Density;
            this.youngsModulus = YoungsModulus;
            this.poissonRatio = Nue;
            this.cohesion = Cohesion;
            this.internalFrictionAngle = InternalFrictionAngle;
            this.internalDilatancyAngle = InternalDilatationAngle;
            this.particlesPerElement = NumberOfParticles;
            this.thickness = Thickness;
        }

        
        public override Dictionary<string, object> GetKratosVariables()
        {
            return new Dictionary<string, object>
            {
                {"THICKNESS", thickness },
                {"YOUNG_MODULUS", youngsModulus},
                {"POISSON_RATIO", poissonRatio},
                {"DENSITY", density},
                {"COHESION", cohesion},
                {"INTERNAL_FRICTION_ANGLE", internalFrictionAngle },
                {"INTERNAL_DILATANCY_ANGLE", internalDilatancyAngle },
            };
        }

        
        public override Dictionary<string, object> GetKratosConstitutiveLaw()
        {
            return new Dictionary<string, object>
            {
                {"NAME", constitutiveLaw}
            };
        }
        
        //What does this function???
        //public override bool Equals(Material comparison)
        //{
        //    var mat = comparison as MaterialLinearElasticIsotropic;

        //    return (base.Equals(mat) &&
        //            mat.YoungsModulus == YoungsModulus &&
        //            mat.Nue == Nue &&
        //            mat.Density == Density);
        //}
    }
}
