namespace Cocodrilo.Analyses
{
    public class AnalysisEigenvalue : Analysis
    {
        public double mTolerance { get; set; }
        public int mNumEigenvalues { get; set; }
        public int mMaximumIterations { get; set; }
        public string mSolverType { get; set; }

        public AnalysisEigenvalue() { }

        public AnalysisEigenvalue(
            string Name,
            double Tolerance,
            int NumEigenvalues,
            int MaximumIterations,
            string SolverType)
        {
            this.Name = Name;
            this.mNumEigenvalues = NumEigenvalues;
            this.mMaximumIterations = MaximumIterations;
            this.mTolerance = Tolerance;
            this.mSolverType = SolverType;
        }

    }
}
