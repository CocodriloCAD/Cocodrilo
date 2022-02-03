namespace Cocodrilo.Analyses
{
    public class AnalysisShapeOptimization : Analysis
    {
        public int mMaxOptimizationIterations { get; set; }
        public double mStepSize { get; set; }

        public AnalysisShapeOptimization() { }

        public AnalysisShapeOptimization(
            string Name,
            int MaxOptimizationIterations,
            double StepSize)
        {
            this.Name = Name;
            mMaxOptimizationIterations = MaxOptimizationIterations;
            mStepSize = StepSize;
        }
    }
}