using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cocodrilo.Materials;
using Rhino.Geometry;

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
		//attribute Name is inherited by base class Analysis

		// Not yet finished: change datatype so that only "static, dynamic and quasi-static" can
		// be chosen for analysisType
		public string mAnalysisType_static_dynamic_quasi_static { get; set; }
		public Material mMaterial { get; set; }
		public List<Mesh> BodyMesh { get; set; }

		public AnalysisMpm_new() { }
		
		public AnalysisMpm_new(
			string name, 
			string analysisType,
			Material material,
			List<Mesh> bodymesh)
        {
			this.Name = name;
			mAnalysisType_static_dynamic_quasi_static = analysisType;
			mMaterial = material;
			BodyMesh = bodymesh;

        }





	}
}
