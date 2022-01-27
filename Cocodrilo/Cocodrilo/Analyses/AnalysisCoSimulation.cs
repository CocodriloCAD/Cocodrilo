using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.Analyses
{
    class AnalysisCoSimulation : Analysis
    {
        public AnalysisCoSimulation()
        { }

        public AnalysisCoSimulation(string AnalysisName) : base(AnalysisName)
        {
        }

        public string GetSolver()
        {
            return "coupled_solvers.gauss_seidel_weak";
        }

    }
}
