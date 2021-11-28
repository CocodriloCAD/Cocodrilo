using System.Collections.Generic;

namespace Cocodrilo.Refinement
{
    public class RefinementEdge : Refinement
    {
        public int PDeg { get; set; }
        public int KnotSubDivU { get; set; }
        public int KnotInsertType { get; set; }     // 0 = knot subdivision, 1 = approx element size

        public RefinementEdge() { }

        public RefinementEdge(int PDeg = 0, int KnotSubDivU = 1, int KnotInsertType = 0)
        {
            this.PDeg = PDeg;
            this.KnotSubDivU = KnotSubDivU;
            this.KnotInsertType = 0;
        }

        public override Dictionary<string, object> GetKratosRefinement(int Index)
        {
            return new Dictionary<string, object> { };
        }
    }
}
