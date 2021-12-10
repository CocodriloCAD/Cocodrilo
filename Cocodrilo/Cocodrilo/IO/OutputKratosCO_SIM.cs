using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace Cocodrilo.IO
{
    public class OutputKratosCO_SIM : Output
    {
        public OutputKratosCO_SIM() : base(new Analyses.AnalysisCoSimulation("CoSimulation"))
        {
        }

        public void StartAnalysis(List<Output> OutputList)
        {
            string project_path = UserData.UserDataUtilities.GetProjectPath("DEM");

            StartAnalysis(project_path, OutputList);
        }

        public void StartAnalysis(string ProjectPath, List<Output> OutputList)
        {
            System.IO.File.WriteAllLines(ProjectPath + "/" + "cosim_parameters.json",
                new List<string> { GetCoSimulationProjectParameters(OutputList) });
        }

        public string GetCoSimulationProjectParameters(List<Output> OutputList)
        {
            var problem_data = new Dictionary<string, object> {
                { "start_time", 0.0 },
                { "end_time", 0.2 },
                { "echo_level", analysis.GetEchoLevel() },
                { "print_colors", true },
                { "parallel_type", "OpenMP" }
            };

            var mapper_1 = new Dictionary<string, object> {
                { "type", "kratos_mapping" },
                { "mapper_settings", new Dictionary<string, object> { { "mapper_type", "nearest_neighbor" } } }
            };

            var solver_settings = new Dictionary<string, object> {
                { "type", "coupled_solvers.gauss_seidel_weak" },
                { "echo_level", analysis.GetEchoLevel() },
                { "data_transfer_operators", new Dictionary<string, object> { { "mapper_1", mapper_1 } } },
                { "parallel_type", "OpenMP" }
            };

            var coupling_sequence = new List<Dictionary<string, object>> { };
            var solvers = new Dictionary<string, object> { };
            foreach (var output in OutputList)
            {
                coupling_sequence.Add(output.GetCouplingSequence(
                    new List<Analyses.Analysis> { },
                    new List<Analyses.Analysis> { }));
                solvers.Add(output.analysis.Name, output.GetCouplingSolver());
            }

            var project_parameters = new Dictionary<string, object>
            {
                { "problem_data", problem_data },
                { "solver_settings", solver_settings }
            };

            var serializer = new JavaScriptSerializer();
            string json = serializer.Serialize((object)project_parameters);

            return json;
        }
    }
}
