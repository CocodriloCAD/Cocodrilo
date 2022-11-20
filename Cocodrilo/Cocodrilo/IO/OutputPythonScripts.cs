using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cocodrilo.IO
{
    public static class OutputPythonScripts
    {
        public static void WriteKratosMainIga(string Path)
        {
            string[] lines =
            {
                "import KratosMultiphysics",
                "import KratosMultiphysics.IgaApplication",
                "from KratosMultiphysics.StructuralMechanicsApplication.structural_mechanics_analysis import StructuralMechanicsAnalysis",
                "",
                "if __name__ == \"__main__\":",
                "    with open(\"ProjectParameters.json\",'r') as parameter_file:",
                "        parameters = KratosMultiphysics.Parameters(parameter_file.read())",
                "",
                "    model = KratosMultiphysics.Model()",
                "    simulation = StructuralMechanicsAnalysis(model, parameters)",
                "    simulation.Run()",
            };

            System.IO.File.WriteAllLines(Path, lines);
        }

        public static void WriteKratosOptimization(string Path, List<int> MaterialIds)
        {
            string[] lines =
            {
                "import sys",
                "import subprocess",
                "import pkg_resources",
                "",
                "required  = {'numpy', 'geneticalgorithm'}",
                "installed = {pkg.key for pkg in pkg_resources.working_set}",
                "missing   = required - installed",
                "",
                "if missing:",
                "    # implement pip as a subprocess:",
                "    subprocess.check_call([sys.executable, '-m', 'pip', 'install', *missing])",
                "",
                "from geneticalgorithm import geneticalgorithm as ga",
                "import numpy as np",
                "",
                "import KratosMultiphysics",
                "import KratosMultiphysics.IgaApplication",
                "import KratosMultiphysics.IgaApplication.kratos_wrapper_for_optimization as k_o",
                "",
                "kratos_optimizer_wrapper = k_o.KratosWrapperForOptimization()",
                "",
                "optimization_dimension = kratos_optimizer_wrapper.CountNumberOfOptimizationModelParts()",
                "",
                "varbound=np.array([[" + MaterialIds.Min() + ", " + MaterialIds.Max() + "]]*optimization_dimension)",
                "genetic_opimization_model = ga(function = kratos_optimizer_wrapper.OptimizableFunction, dimension = optimization_dimension, variable_type = 'int', variable_boundaries = varbound)",
                "genetic_opimization_model.run()",
            };

            System.IO.File.WriteAllLines(Path, lines);
        }
    }
}
