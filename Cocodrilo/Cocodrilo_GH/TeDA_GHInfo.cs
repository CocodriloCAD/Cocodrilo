using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace Cocodrilo_GH
{
    public class Cocodrilo_GHInfo : GH_AssemblyInfo
    {
        public override string Name => "Cocodrilo_GH";

        //Return a 24x24 pixel bitmap to represent this GHA library.
        public override Bitmap Icon => null;

        //Return a short string describing the purpose of this GHA library.
        public override string Description => "";

        public override Guid Id => new Guid("BF047E8A-07D0-4D5E-A6D9-212E4B1356C1");

        //Return a string identifying you or your company.
        public override string AuthorName => "Lehrstuhl für Statik, Technische Universität München";

        //Return a string representing your preferred contact details.
        public override string AuthorContact => "Arcisstraße 21, 80333 München";
    }
}