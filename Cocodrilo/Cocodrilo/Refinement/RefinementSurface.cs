using System.Collections.Generic;

namespace Cocodrilo.Refinement
{
    public class RefinementSurface : Refinement
    {
        public int PDeg { get; set; }
        public int QDeg { get; set; }
        public int KnotSubDivU { get; set; }
        public int KnotSubDivV { get; set; }
        public int KnotInsertType { get; set; }     // 0 = knot subdivision, 1 = approx element size

        public RefinementSurface() { }

        public RefinementSurface(int PDeg = 1, int QDeg = 1, int KnotSubDivU = 1, int KnotSubDivV = 1, int KnotInsertType=0)
        {
            this.PDeg = PDeg;
            this.QDeg = QDeg;

            if(KnotSubDivU==0)
                this.KnotSubDivU = 1;
            else
                this.KnotSubDivU = KnotSubDivU;

            if(KnotSubDivV==0)
                this.KnotSubDivV = 1;
            else
                this.KnotSubDivV = KnotSubDivV;

            this.KnotInsertType = KnotInsertType;
        }

        public override Dictionary<string, object> GetKratosRefinement(int BrepIndex)
        {
            var parameters = new Dictionary<string, object>
            {
                { "insert_nb_per_span_u", KnotSubDivU},
                { "insert_nb_per_span_v", KnotSubDivV},
            };

            return new Dictionary<string, object>
            {
                { "brep_ids", new double[] { BrepIndex  } },
                { "geometry_type", "NurbsSurface"},
                { "model_part_name", "IgaModelPart"},
                { "parameters", parameters}
            };
        }
    }
}
