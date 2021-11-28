using System.Collections.Generic;

namespace Cocodrilo.Refinement
{
    public class RefinementCurve : Refinement
    {
        public int PolynomialDegree { get; set; }
        public double KnotSubDivU { get; set; }
        public int KnotInsertType { get; set; }     // 0 = knot subdivision, 1 = approx element size

        public RefinementCurve() { }
        public RefinementCurve(int PolynomialDegree = 0, double KnotSubDivU = 1, int KnotInsertType = 0)
        {
            this.PolynomialDegree = PolynomialDegree;

            if (KnotSubDivU == 0)
                this.KnotSubDivU = 1;
            else
                this.KnotSubDivU = KnotSubDivU;

            this.KnotInsertType = KnotInsertType;
        }

        public override Dictionary<string, object> GetKratosRefinement(int Index)
        {
            return new Dictionary<string, object> { };
        }
    }
}
