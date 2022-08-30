using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;

namespace Cocodrilo.ElementProperties
{
    public enum CableCouplingType
    {
        StartAndEnd,
        EntireCurve
    }
    public struct CableProperties : IEquatable<CableProperties>
    {
        public double mPrestress { get; set; }
        public double mArea { get; set; }
        public CableCouplingType mCableCouplingTypes { get; set; }

        public CableProperties(
            double Prestress,
            double Area,
            CableCouplingType CableCouplingType)
        {
            mPrestress = Prestress;
            mArea = Area;
            mCableCouplingTypes = CableCouplingType;
        }

        public bool Equals(CableProperties comp)
        {
            return comp.mPrestress == mPrestress &&
                   comp.mArea == mArea;
        }
    }
    public class PropertyCable : Property
    {
        public CableProperties mCableProperties { get; set; }

        public PropertyCable() : base()
        {
        }

        public PropertyCable(
            GeometryType ThisGeometryType,
            int MaterialId,
            CableProperties ThisCableProperties,
            bool IsFormFinding = true)
            : base(ThisGeometryType, MaterialId, IsFormFinding)
        {
            mCableProperties = ThisCableProperties;
        }

        public override List<Dictionary<string, object>> GetKratosPhysic(List<int> BrepIds)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>{};

            if (GeometryType.GeometryCurve == mGeometryType)
            {
                Parameters["type"] = "element";
                Parameters["name"] = "TrussElement";
                Parameters["properties_id"] = mMaterialId;
                Parameters["shape_function_derivatives_order"] = 2;
            }
            
            if (GeometryType.SurfaceEdge == mGeometryType)
            {
                Parameters["type"] = "element";
                Parameters["name"] = "TrussEmbeddedEdgeElement";
                Parameters["properties_id"] = mMaterialId;
                Parameters["shape_function_derivatives_order"] = 2;
            }

            Dictionary<string, object> property_element = new Dictionary<string, object>
            {
                {"brep_ids", BrepIds},
                {"geometry_type", GeometryTypeString},
                {"iga_model_part", GetKratosModelPart() },
                {"parameters", Parameters}
            };
            return new List<Dictionary<string, object>> { property_element };
        }

        public override Dictionary<string, object> GetKratosVariables()
        {
            return new Dictionary<string, object>
            {
                { "CROSS_AREA", mCableProperties.mArea},
                { "PRESTRESS_CAUCHY", mCableProperties.mPrestress}
            };
        }

        public override string GetKratosModelPart()
        {
            if (mIsFormFinding)
                return "FormFinding_" + mPropertyId;
            return "StructuralAnalysis_" + mPropertyId;
        }

        public override bool Equals(Property ThisProperty)
        {
            var cable = ThisProperty as PropertyCable;
            return cable.mMaterialId == mMaterialId &&
                   cable.mIsFormFinding == mIsFormFinding &&
                   cable.mCableProperties.Equals(mCableProperties);
        }
    }
}
