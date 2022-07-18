namespace Cocodrilo.Analyses
{
    public class AnalysisTransient : Analysis
    {
        public double tolerance { get; set; }
        public int Time { get; set; }
        public double mValue { get; set; }
        public int NumStep { get; set; }
        public int MaxIter { get; set; }
        public int mAdaptiveMaxLevel { get; set; }
        public double RayleighAlpha { get; set; }
        public double RayleighBeta { get; set; }
        public string TimeInteg { get; set; }
        public string Scheme { get; set; }

        public bool AutomaticRayleigh { get; set; }
        public double DampingRatio0 { get; set; }
        public double DampingRatio1 { get; set; }
        public double NumEigen { get; set; }

        public double mStepSize { get; set; }

        public AnalysisTransient() { }

        public AnalysisTransient(
            string Name,
            int NumStep,
            int NumIter,
            double _acc,
            int Time,
            double Value,
            int Adaptive_Max_Level,
            double RayleighAlpha,
            double RayleighBeta,
            string TimeInteg,
            string Scheme,
            bool AutomaticRayleigh,
            double DampingRatio0,
            double DampingRatio1,
            double NumEigen,
            double StepSize = 0.1)
        {
            this.Name = Name;
            this.NumStep = NumStep;
            this.MaxIter = NumIter;
            this.Time = Time;
            this.mValue = Value;
            this.tolerance = _acc;
            this.mAdaptiveMaxLevel = Adaptive_Max_Level;
            this.RayleighAlpha = RayleighAlpha;
            this.RayleighBeta = RayleighBeta;
            this.TimeInteg = TimeInteg;
            this.Scheme = Scheme;
            this.AutomaticRayleigh = AutomaticRayleigh;
            this.DampingRatio0 = DampingRatio0;
            this.DampingRatio1 = DampingRatio1;
            this.NumEigen = NumEigen;
            this.mStepSize = StepSize;
        }
    }
}
