using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.Materials
{

    public struct DemMaterialRelationProperties
    {
        public Dictionary<string, object> GetKratosVariables()
        {
            return new Dictionary<string, object>
            {
                { "DEM_DISCONTINUUM_CONSTITUTIVE_LAW_NAME", "DEM_D_Hertz_viscous_Coulomb" },
                {"PARTICLE_COHESION", 0.0 },
                {"STATIC_FRICTION", 0.5 },
                {"DYNAMIC_FRICTION", 0.4 },
                {"FRICTION_DECAY", 500 },
                {"COEFFICIENT_OF_RESTITUTION", 0.2 },
                {"ROLLING_FRICTION", 0.01 },
                {"ROLLING_FRICTION_WITH_WALLS", 0.01 }
            };
        }
    }

    public class MaterialDem : Material
    {
        public double mYoungsModulus { get; set; }
        public double mNue { get; set; }
        public double mDensity { get; set; }
        public double mParticleSphericity { get; set; }

        public Dictionary<int, DemMaterialRelationProperties> mMaterialRelations;
        DemMaterialRelationProperties mDemMaterialRelationProperties;

        public MaterialDem()
        {
        }
        public MaterialDem(
            string Name,
            double YoungsModulus = 100000,
            double Nue = 0.2,
            double Density = 2500,
            double ParticleSphericity = 1.0)
            : base(Name)
        {
            mYoungsModulus = YoungsModulus;
            mNue = Nue;
            mDensity = Density;
            mParticleSphericity = ParticleSphericity;

            mDemMaterialRelationProperties = new DemMaterialRelationProperties();
            mMaterialRelations = new Dictionary<int, DemMaterialRelationProperties>();
            mMaterialRelations.Add(Id, mDemMaterialRelationProperties);
        }

        public void AddMaterialRelation(MaterialDem Material)
        {
            mMaterialRelations.Add(Material.Id, Material.mDemMaterialRelationProperties);
        }

        public override Dictionary<string, object> GetKratosVariables()
        {
            return new Dictionary<string, object>
            {
                {"YOUNG_MODULUS", mYoungsModulus},
                {"POISSON_RATIO", mNue},
                {"PARTICLE_DENSITY", mDensity},
                {"PARTICLE_SPHERICITY", mParticleSphericity}
            };
        }

        public List<Dictionary<string, object>> GetKratosMaterialRelations()
        {
            var materials_list_dict = new List<Dictionary<string, object>> { };
            foreach (var material_relation in mMaterialRelations)
            {
                int material_id = (material_relation.Key == -1)
                    ? Id
                    : material_relation.Key;
                var related_material = CocodriloPlugIn.Instance.GetMaterial(material_id);
                materials_list_dict.Add(new Dictionary<string, object>
                {
                    {"material_names_list", new string[]{ Name, related_material.Name } },
                    {"material_ids_list", new int[]{ Id, material_id } },
                    {"Variables", material_relation.Value.GetKratosVariables()}
                });
            }
            return materials_list_dict;
        }

        public override bool Equals(Material comparison)
        {
            var mat = comparison as MaterialDem;

            foreach(var pair_id in mMaterialRelations.Keys)
                if (!mat.mMaterialRelations.ContainsKey(pair_id)) return false;

            return base.Equals(mat) &&
                    mat.mYoungsModulus == mYoungsModulus &&
                    mat.mNue == mNue &&
                    mat.mDensity == mDensity &&
                    mat.mParticleSphericity == mParticleSphericity;
        }
    }
}
