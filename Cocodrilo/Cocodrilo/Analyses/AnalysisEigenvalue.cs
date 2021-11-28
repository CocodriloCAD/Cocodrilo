namespace Cocodrilo.Analyses
{
    public class AnalysisEigenvalue : Analysis
    {
        public double tolerance { get; set; }
        public int NumEigen { get; set; }
        public int MaxIter { get; set; }
        public string SolverType { get; set; }

        public AnalysisEigenvalue() { }

        public AnalysisEigenvalue(
            string Name,
            double _acc,
            int NumEigen,
            int MaxIter,
            string SolverType)
        {
            this.Name = Name;
            this.NumEigen = NumEigen;
            this.MaxIter = MaxIter;
            tolerance = _acc;
            this.SolverType = SolverType;
        }

    }
}
