using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cocodrilo.Materials;

namespace Cocodrilo.Analyses
{

    //enum AnalysisType
    //{
    //    Static,
    //    QuasiStatic,
    //    Dynamic
    //}

    public class AnalysisMpm_new : Analysis
	{
		public string name;

		// Not yet finished: change datatype so that only "static, dynamic and quasi-static" can
		// be chosen for analysisType
		public string mAnalysisType { get; set; }

		public string mMaterial { get; set; }

		public string mSolverType { get; set; }

		public string mMaterialLaw { get; set; }

		public int mNumberOfParticles { get; set; }

		public AnalysisMpm_new() { }
		
		public AnalysisMpm_new(
			string Name, 
			string analysisType,
			string material,
			string solverType,
			string materialLaw,
			int numberOfParticles)
        {
			this.name = Name;
			mAnalysisType = analysisType;
			mMaterial = material;
			mMaterialLaw = materialLaw;
			mSolverType = solverType;
			mNumberOfParticles = numberOfParticles;

        }





	}
}
