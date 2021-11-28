namespace Cocodrilo.Analyses
{
    public class AnalysisNonLinear : Analysis
    {
        public double mSolverTolerance { get; set; }
        public int mNumSimulationSteps { get; set; }
        public int mMaxSolverIteration { get; set; }
        public double mStepSize { get; set; }

        public AnalysisNonLinear() { }

        public AnalysisNonLinear(
            string Name,
            int NumSimulationSteps,
            int MaxSolverIteration,
            double SolverTolerance,
            double StepSize)
        {
            this.Name = Name;
            mNumSimulationSteps = NumSimulationSteps;
            mMaxSolverIteration = MaxSolverIteration;
            mSolverTolerance = SolverTolerance;
            mStepSize = StepSize;
        }
    }
}
