using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Rhino.Geometry.Morphs;

namespace Cocodrilo.ElementProperties
{
    public class PropertyBeam : Property, IEquatable<Property>
    {
        public BeamProperties mBeamProperties { get; set; }
        public PropertyBeam(
            GeometryType ThisGeometryType,
            int MaterialId,
            BeamProperties ThisBeamProperties,
            bool IsFormFinding = true
            ) : base(ThisGeometryType, MaterialId, IsFormFinding)
        {
            mBeamProperties = ThisBeamProperties;
        }

        public PropertyBeam(int MaterialId, int beam_type, double diameter = 0.0, double height = 0.0, double width = 0.0, double area = 0.0, double iy = 0.0, double iz = 0.0, double it = 0.0, List<double[]> base_vecs = null, double gaussU = 2, double gaussV = 2, bool is_prestress_bend1_auto = false, bool is_prestress_bend2_auto = false, bool is_prestress_tor_auto = false, double prestress = 0.0, double prestress_bend1 = 0.0, double prestress_bend2 = 0.0, double prestress_tor = 0.0) 
            : base(GeometryType.GeometryCurve, MaterialId)
        {
        }
        public PropertyBeam(
           PropertyBeam previousPropertyBeam)
           : base(previousPropertyBeam)
        {
            mBeamProperties = previousPropertyBeam.mBeamProperties;
        }

        public override bool Equals(Property ThisProperty)
        {
            var beam = ThisProperty as PropertyBeam;
            return mBeamProperties.Equals(beam.mBeamProperties)
                   && mMaterialId == beam.mMaterialId
                   && mIsFormFinding == beam.mIsFormFinding;
        }
        public override Property Clone() =>
           new PropertyBeam(this);

        public override string ToString()
        {
            return "beam property";
        }
        public override List<Dictionary<string, object>> GetKratosPhysic(List<int> BrepIds)
        {
            Dictionary<string, object> Parameters = new Dictionary<string, object>
            {
                { "type", "element"},
                { "name", mBeamProperties.mBeamFormulation.ToString() },
                { "shape_function_derivatives_order", 3}
            };

            Dictionary<string, object> property_element = new Dictionary<string, object>
            {
                {"brep_ids", BrepIds},
                {"geometry_type", GeometryTypeString},
                {"iga_model_part", GetKratosModelPart() },
                {"parameters", Parameters}
            };
            return new List<Dictionary<string, object>> { property_element };
        }
    }
}
