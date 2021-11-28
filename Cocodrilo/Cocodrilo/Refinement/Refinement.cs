using System.Collections.Generic;

namespace Cocodrilo.Refinement
{
    public abstract class Refinement
    {
        public Refinement()
        {

        }
        public abstract Dictionary<string, object> GetKratosRefinement(int BrepIndex);
    }
}
