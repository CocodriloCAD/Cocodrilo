namespace Cocodrilo.Analyses
{
    public class AnalysisCuttingPattern : Analysis
    {
        public int maxSteps { get; set; }
        public int maxIterations { get; set; }
        public double tolerance { get; set; }
        public string solution_strategy { get; set; }
        public bool Prestress { get; set; }

        public AnalysisCuttingPattern() { }
        public AnalysisCuttingPattern(string name, int maxSteps = 10, 
            int maxIterations = 1, double tolerance = 0.001, string solutionstrategy= "Newton-Raphson", bool considerPrestress = false )
            :base(name)
        {
            this.maxSteps = maxSteps;
            this.maxIterations = maxIterations;
            this.tolerance = tolerance;
            this.solution_strategy = solutionstrategy;
            this.Prestress = considerPrestress;
        }
    }
}
