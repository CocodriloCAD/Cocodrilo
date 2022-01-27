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

        public virtual void StartAnalysis() { }
        public virtual void StartAnalysis(List<Brep> BrepList, List<Curve> CurveList, List<Point> PointList) { }

        #region CO SIMULATION
        public virtual Dictionary<string, object> GetCouplingSequence(
            List<Analyses.Analysis> InputAnalyses,
            List<Analyses.Analysis> OutputAnalyses)
        {
            return new Dictionary<string, object> {
                    { "name", analysis.Name },
                    { "input_data_list", new List<int>{ } },
                    { "output_data_list", new List<int>{ } } };
        }

        public virtual Dictionary<string, object> GetCouplingSolver()
        {
            return new Dictionary<string, object> {
                    { analysis.Name, new Dictionary<string, object> { } } };
        }
        #endregion
    }
}