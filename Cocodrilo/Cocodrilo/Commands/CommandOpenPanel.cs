using System;
using Rhino;
using Rhino.Commands;

namespace Cocodrilo.Commands
{
    [System.Runtime.InteropServices.Guid("42b1e72f-00d2-47de-8bfb-6d505172cf6e")]
    public class CommandOpenPanel : Command
    {
        public CommandOpenPanel()
        {
            Instance = this;
        }

        public static CommandOpenPanel Instance { get; private set; }

        public override string EnglishName
        {
            get { return "Cocodrilo_OpenPanel"; }
        }

        protected override Result RunCommand(RhinoDoc doc, RunMode mode)
        {
            var panelType = typeof(Panels.UserControlCocodriloPanel);
            Rhino.UI.Panels.OpenPanel(panelType.GUID);

            return Result.Success;
        }
    }
}
