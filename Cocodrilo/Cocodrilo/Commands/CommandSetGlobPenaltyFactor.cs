using Rhino;
using Rhino.Commands;
using Rhino.Input;

namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("FEF7A924-135A-4AE4-AB8F-E69391E70BFE")]
    public class CommandSetGlobPenaltyFactor : Command
    {
        static CommandSetGlobPenaltyFactor _instance;
        public CommandSetGlobPenaltyFactor()
        {
            _instance = this;
        }

        ///<summary>The only instance of the Cocodrilo_SetGlobPenaltyFactor command.</summary>
        public static CommandSetGlobPenaltyFactor Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_SetGlobPenaltyFactor"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            double penalty_factor = 1e7;
            var rcPenFac = RhinoGet.GetNumber("Set global penalty factor:", false, ref penalty_factor);
            if (rcPenFac != Result.Success)
                return Result.Success;

            CocodriloPlugIn.Instance.GlobPenaltyFactor = penalty_factor;

            return Result.Success;
        }
    }
}
