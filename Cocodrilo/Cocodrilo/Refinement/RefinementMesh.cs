using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


// This class is just a draft; not finished yet!!
namespace Cocodrilo.Refinement
{
    class RefinementMesh : Refinement
    {
        public RefinementMesh() { }

        public override Dictionary<string, object> GetKratosRefinement(int BrepIndex)
        {
            //var parameters = new Dictionary<string, object>
            //{
            //    { "insert_nb_per_span_u", KnotSubDivU},
            //    { "insert_nb_per_span_v", KnotSubDivV},
            //    { "increase_degree_u", PDeg},
            //    { "increase_degree_v", QDeg}
            //};

            return new Dictionary<string, object>
            {
                { "Test", new double[] { 1,2 } },
                //{ "geometry_type", "NurbsSurface"},
                //{ "model_part_name", "IgaModelPart"},
                //{ "parameters", parameters}
            };
        }
    }
}
