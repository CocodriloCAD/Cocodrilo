using System;
using Rhino;
using Rhino.Commands;

namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("6A81D707-66FB-4A31-BE8D-45EEE6D0C44F")]
    public class CommandRunBenchmarks : Command
    {
        static CommandRunBenchmarks _instance;
        public CommandRunBenchmarks()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandDeleteAll command.</summary>
        public static CommandRunBenchmarks Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_RunBenchmarks"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            int bm_mode =0;

            //Path to the plugin
            var path = Rhino.PlugIns.PlugIn.PathFromId(new Guid("ce983e9d-72de-4a79-8832-7c374e6e26de"));
            var pathWithSlash = path.Replace("\\", "/");
            var idOfLastSlash = pathWithSlash.LastIndexOf("/");
            var pluginPath = pathWithSlash.Substring(0, idOfLastSlash );
            idOfLastSlash = pluginPath.LastIndexOf("/");
            pluginPath = pluginPath.Substring(0, idOfLastSlash );
            idOfLastSlash = pluginPath.LastIndexOf("/");
            pluginPath = pluginPath.Substring(0, idOfLastSlash + 1);

            if (bm_mode == 0 || bm_mode == 1)
            {
                var bm_path = pluginPath + "Benchmarks/Benchmark_runner.py";
                bm_path = bm_path.Replace( "/", "\\");
                string command = "!_-RunPythonScript " +  bm_path ;
                RhinoApp.RunScript(command, true);
            }

            if (bm_mode == 0 || bm_mode == 2)
            {
                var bm_path = pluginPath + "Benchmarks_GH/Benchmark_runner.py";
                bm_path = bm_path.Replace("/", "\\");
                string command = "!_-RunPythonScript " + @"" + bm_path + @"";
                RhinoApp.RunScript(command, true);

            }

            return Result.Success;
        }
    }
}
