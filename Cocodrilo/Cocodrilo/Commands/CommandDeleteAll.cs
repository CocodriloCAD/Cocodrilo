using System;
using Rhino;
using Rhino.Commands;

namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("d0b81c93-7f29-4d3a-b01b-f039eecd9247")]
    public class CommandDeleteAll : Command
    {
        static CommandDeleteAll _instance;
        public CommandDeleteAll()
        {
            _instance = this;
        }

        ///<summary>The only instance of the CommandDeleteAll command.</summary>
        public static CommandDeleteAll Instance
        {
            get { return _instance; }
        }

        public override string EnglishName
        {
            get { return "Cocodrilo_DeleteAll"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            CocodriloPlugIn.Instance.DeleteAll();

            doc.Views.Redraw();

            return Result.Success;
        }
    }
}
