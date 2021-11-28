using System;
using Rhino;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Cocodrilo.UserData;
using System.Collections.Generic;

namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("747E0F2D-C9E2-461A-A23C-39E4EF8DE75B")]
    public class CommandSetCouplingTolerances : Command
    {
        static CommandSetCouplingTolerances _instance;
        public CommandSetCouplingTolerances()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandDeleteAll command.</summary>
        public static CommandSetCouplingTolerances Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_SetCouplingTolerances"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            double coup_tol = 0.01;
            var rcCoupTol = RhinoGet.GetNumber("Set global coupling tolerances:", false, ref coup_tol);
            if (rcCoupTol != Result.Success)
                return Result.Success;

            //CocodriloPlugIn.Instance.CouplingTolerance = coup_tol;

            return Result.Success;
        }
    }
}
