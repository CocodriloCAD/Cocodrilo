using System;
using Rhino;
using Rhino.Commands;
using Rhino.Input;

namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("CF4C5E1A-ACD6-433F-A7DB-56E0C8994753")]
    public class CommandWriteInput : Command
    {
        public CommandWriteInput()
        {
            Instance = this;
        }

        public static CommandWriteInput Instance { get; private set; }

        public override string EnglishName
        {
            get { return "Cocodrilo_WriteInput"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            string analysis_name = "";
            var rc = RhinoGet.GetString("Analysis", true, ref analysis_name);
            if (rc != Result.Success)
                return rc;
            if (String.IsNullOrWhiteSpace(analysis_name))
                return Result.Nothing;
            string path = "";
            var rc1 = RhinoGet.GetString("Path", true, ref path);
            if (rc1 != Result.Success)
                return rc1;
            if (String.IsNullOrWhiteSpace(path))
                return Result.Nothing;

            var analysis = CocodriloPlugIn.Instance.Analyses.Find(a => a.Name == analysis_name);
            if (analysis != null)
            {
                var input = new IO.OutputKratosIGA(analysis);
                input.StartAnalysis(path);
            }
            else
            {
                RhinoApp.WriteLine("WARNING: Could not find analysis: " + analysis_name);
            }

            return Result.Success;
        }
    }
}
