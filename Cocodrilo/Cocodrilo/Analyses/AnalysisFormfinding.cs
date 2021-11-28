namespace Cocodrilo.Analyses
{
    public class AnalysisFormfinding : Analysis
    {
        public int maxSteps { get; set; }
        public int maxIterations { get; set; }
        public double tolerance { get; set; }

        public AnalysisFormfinding() { }
        public AnalysisFormfinding(string name, int maxSteps = 10,
            int maxIterations = 1, double tolerance = 0.001)
            : base(name)
        {
            this.maxSteps = maxSteps;
            this.maxIterations = maxIterations;
            this.tolerance = tolerance;
        }
    }
}
