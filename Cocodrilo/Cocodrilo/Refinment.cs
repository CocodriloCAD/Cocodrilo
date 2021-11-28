using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Rhino;
using Rhino.Commands;
using Rhino.Geometry;
using Rhino.Input;
using Rhino.Input.Custom;
using Rhino.DocObjects;

namespace TeDaSharp
{
    public abstract class Refinment
    {
        public abstract string getCaratRefinment(int Index);
    }

    public class RefinmentCurve : Refinment
    {
        public int PDeg { get; set; }
        public int minElementU { get; set; }

        public RefinmentCurve() { }
        public RefinmentCurve(int PDeg = 0, int minElementU = 0)
        {
            this.PDeg = PDeg;
            this.minElementU = minElementU;
        }

        public void changeRefinement(int PDeg, int minElementU)
        {
            this.PDeg = PDeg;
            this.minElementU = minElementU;
        }
        public override string getCaratRefinment(int EdgeIndex)
        {
            string refinment = " DE-BREP-EL   " + EdgeIndex;
            if (PDeg == 0 && minElementU == 0)
                refinment += "   dp=auto    ru=auto ";
            else
                refinment += "   dp=" + PDeg + "    ru=" + minElementU;
            return refinment;
        }
    }

    public class RefinmentSurface : Refinment
    {
        public int PDeg { get; set; }
        public int QDeg { get; set; }
        public int minElementU { get; set; }
        public int minElementV { get; set; }

        public RefinmentSurface() { }
        public RefinmentSurface(int PDeg = 1, int QDeg = 1, int minElementU = 1, int minElementV = 1)
        {
            this.PDeg = PDeg;
            this.QDeg = QDeg;
            this.minElementU = minElementU;
            this.minElementV = minElementV;
        }

        public override string getCaratRefinment(int SufaceIndex)
        {
            string refinment = " DE-EL   " + SufaceIndex;
            if (PDeg == 0 && minElementU == 0)
                refinment += "   ep=2   eq=2   gu=12   gv=12";
            else
                refinment += "   ep=" + PDeg + "   eq=" + QDeg + "    ru=" + minElementU + "    rv=" + minElementV;
            return refinment;
        }
    }
}
