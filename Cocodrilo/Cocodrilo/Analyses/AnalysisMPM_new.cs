using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cocodrilo.Materials;

using Rhino.Geometry;

namespace Cocodrilo.Analyses
{
	public class AnalysisMpm_new : Analysis
	{
		// Not yet finished: change datatype so that only "static, dynamic and quasi-static" can
		// be chosen for analysisType
		public Analysis mAnalysisType_static_dynamic_quasi_static { get; set; }
		public Material mMaterial { get; set; }

		//public List<Mesh> BodyMesh { get; set; }

		//public Cocodrilo_GH.PreProcessing.Geometries Geometry { get; set; }

		public double stepSize { get; set; }

		public AnalysisMpm_new() { }

		public AnalysisMpm_new(
			string name,
			Analysis analysisType,
			Material material) //,
			//List<Mesh> bodymesh)
		{
			this.Name = name; //Accessing of inherited attribute
			mAnalysisType_static_dynamic_quasi_static = analysisType;
			mMaterial = material;
			//BodyMesh = bodymesh;
		}

		public AnalysisMpm_new(
			string name,
			AnalysisTransient analysisType,
			Material material,
			List<Mesh> bodymesh)
		{
			this.Name = name; //Accessing of inherited attribute
			mAnalysisType_static_dynamic_quasi_static = analysisType;
			mMaterial = material;
			//BodyMesh = bodymesh;
		}




	}
}
