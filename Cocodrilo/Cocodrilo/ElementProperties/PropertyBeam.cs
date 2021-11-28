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
        //public int type { get; set; }   // 0 - no special cross section type, 1 - circular cross section, 2 - rectangular cross section
        //public double diameter { get; set; }
        //public double height { get; set; }
        //public double width { get; set; }
        //public double area { get; set; }
        //public double iy { get; set; }
        //public double iz { get; set; }
        //public double it { get; set; }
        //public bool is_prestress_bend1_auto { get; set; }
        //public bool is_prestress_bend2_auto { get; set; }
        //public bool is_prestress_tor_auto { get; set; }
        //public double prestress { get; set; }
        //public double prestress_bend1 { get; set; }
        //public double prestress_bend2 { get; set; }
        //public double prestress_tor { get; set; }
        //public double[] gauss { get; set; }
        //public List<double[]> base_vecs { get; set; }

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


        public override bool Equals(Property ThisProperty)
        {
            var beam = ThisProperty as PropertyBeam;
            return mBeamProperties.Equals(beam.mBeamProperties)
                   && mMaterialId == beam.mMaterialId
                   && mIsFormFinding == beam.mIsFormFinding;
        }
    }
}
