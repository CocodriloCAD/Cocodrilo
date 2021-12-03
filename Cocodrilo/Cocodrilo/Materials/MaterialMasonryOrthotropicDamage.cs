using System;
using System.Collections.Generic;

namespace Cocodrilo.Materials
{
    public class MaterialOrthotropicDamage : Material
    {
        public int MaterialIdDirection1 { get; set; }
        public int MaterialIdDirection2 { get; set; }

        public MaterialOrthotropicDamage()
        {
        }

        public MaterialOrthotropicDamage(
            String Name)
            : base(Name)
        {
            MaterialIdDirection1 = -1;
            MaterialIdDirection2 = -1;
        }
        public override void SetId(int Id)
        {
            this.Id = Id;
            MaterialIdDirection1 = Id + 1;
            MaterialIdDirection2 = Id + 2;
        }
        public override int GetLastId() => Math.Max(Math.Max(MaterialIdDirection1, MaterialIdDirection2), Id);
        public override Dictionary<string, object> GetKratosVariables()
        {
            return new Dictionary<string, object>
            {
                {"CONSIDER_PERTURBATION_THRESHOLD", false},
                {"TANGENT_OPERATOR_ESTIMATION", 0},
                {"TENSION_YIELD_MODEL", 0},
                {"PROJECTION_OPERATOR", 0}
            };
        }

        public override Dictionary<string, object> GetKratosConstitutiveLaw()
        {
            return new Dictionary<string, object>
            {
                {"name", "MasonryOrthotropicDamagePlaneStress2DLaw"}
            };
        }

        public override bool HasKratosSubProperties() => true;
        public override List<Dictionary<string, object>> GetKratosSubProperties()
        {
            if (Name == "MasonryEindhoven")
            {
                var variables1 = new Dictionary<string, object> {
                { "YOUNG_MODULUS", 3.96e+9 },
                { "POISSON_RATIO", 0.06 },
                { "SHEAR_MODULUS", 1.46e+9 },
                { "YIELD_STRESS_TENSION", 0.55e+6 },
                { "FRACTURE_ENERGY_TENSION", 160.0 },
                { "DAMAGE_ONSET_STRESS_COMPRESSION", 4.5e+6 },
                { "YIELD_STRESS_COMPRESSION", 8.8e+6 },
                { "YIELD_STRAIN_COMPRESSION", 0.003 },
                { "RESIDUAL_STRESS_COMPRESSION", 1.6e+6 },
                { "FRACTURE_ENERGY_COMPRESSION", 2.00e+4 },
                { "YIELD_STRESS_SHEAR_TENSION", 0.3e+6 },
                { "YIELD_STRESS_SHEAR_COMPRESSION", 3e+6 },
                { "BIAXIAL_COMPRESSION_MULTIPLIER", 1.3 },
                { "SHEAR_COMPRESSION_REDUCTOR", 0.2 },
                { "BEZIER_CONTROLLER_C1", 0.65 },
                { "BEZIER_CONTROLLER_C2", 0.5 },
                { "BEZIER_CONTROLLER_C3", 1.5 } };
                var sub_property_1 = new Dictionary<string, object> {
                { "properties_id", MaterialIdDirection1 },
                { "Material", new Dictionary<string, object>{ { "Variables", variables1 } } },
                { "Tables", new Dictionary<string, object>{ } } };

                var variables2 = new Dictionary<string, object> {
                 { "YOUNG_MODULUS", 7.52e+9 },
                 { "POISSON_RATIO", 0.09 },
                 { "SHEAR_MODULUS", 1.46e+9 },
                 { "YIELD_STRESS_TENSION", 0.35e+6 },
                 { "FRACTURE_ENERGY_TENSION", 50.0 },
                 { "DAMAGE_ONSET_STRESS_COMPRESSION", 6.8e+6 },
                 { "YIELD_STRESS_COMPRESSION", 10e+6 },
                 { "YIELD_STRAIN_COMPRESSION", 0.003 },
                 { "RESIDUAL_STRESS_COMPRESSION", 2.0e+6 },
                 { "FRACTURE_ENERGY_COMPRESSION", 2.00e+4 },
                 { "YIELD_STRESS_SHEAR_TENSION", 0.3e+6 },
                 { "YIELD_STRESS_SHEAR_COMPRESSION", 3e+6 },
                 { "BIAXIAL_COMPRESSION_MULTIPLIER", 1.3 },
                 { "SHEAR_COMPRESSION_REDUCTOR", 0.2 },
                 { "BEZIER_CONTROLLER_C1", 0.65 },
                 { "BEZIER_CONTROLLER_C2", 0.5 },
                 { "BEZIER_CONTROLLER_C3", 1.5 } };
                var sub_property_2 = new Dictionary<string, object> {
                { "properties_id", MaterialIdDirection2 },
                { "Material", new Dictionary<string, object>{ { "Variables", variables2 } } },
                { "Tables", new Dictionary<string, object>{ } } };

                return new List<Dictionary<string, object>> { sub_property_1, sub_property_2 };
            }
            else if (Name == "MasonryBrisbane")
            {
                return new List<Dictionary<string, object>> {  };
            }
            return new List<Dictionary<string, object>> { };
        }

        public override List<string> GetKratosOutputValuesIntegrationPoints(Cocodrilo.IO.OutputOptions ThisOutputOptions)
        {
            return (ThisOutputOptions.damage)
                ? new List<string> { "DAMAGE_TENSION", "DAMAGE_COMPRESSION" }
                : new List<string> { };
        }

        public override bool Equals(Material comparison)
        {
            var mat = comparison as MaterialOrthotropicDamage;

            return (base.Equals(mat) &&
                    mat.MaterialIdDirection1 == MaterialIdDirection1 &&
                    mat.MaterialIdDirection2 == MaterialIdDirection2);
        }
    }
}
