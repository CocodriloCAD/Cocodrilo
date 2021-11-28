using System;
using System.Collections.Generic;

namespace Cocodrilo.Analyses
{
    public class Analysis
    {
        public String Name { get; set; }

        public Analysis() { }

        public Analysis(string _name)
        {
            this.Name = _name;
        }
    }

}
