using Rhino.Geometry;
using System.Collections.Generic;

namespace Cocodrilo.IO
{
    public abstract class Output
    {
        public Analyses.Analysis analysis { get; set; }

        protected Output(Analyses.Analysis analysis)
        {
            this.analysis = analysis;
        }

        public abstract void StartAnalysis();
        public abstract void StartAnalysis(List<Brep> BrepList, List<Curve> CurveList, List<Point> PointList);
    }
}