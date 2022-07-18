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

		//public Material mMaterial { get; set; } now in element already included

		public List<Mesh> mBodyMesh { get; set; }

		//public Cocodrilo_GH.PreProcessing.Geometries Geometry { get; set; }

		public AnalysisMpm_new() { }

		public AnalysisMpm_new(
			string name,
			Analysis analysisType,
			List<Mesh> bodyMesh) //,
			//List<Mesh> bodymesh)
			//Material material,
		{
			this.Name = name; //Accessing of inherited attribute
			mAnalysisType_static_dynamic_quasi_static = analysisType;
			mBodyMesh = bodyMesh;
		}


        //public AnalysisMpm_new(
        //    string name,
        //    AnalysisTransient analysisType,
        //    List<Mesh> bodymesh)
        //{
        //    this.Name = name; //Accessing of inherited attribute
        //    mAnalysisType_static_dynamic_quasi_static = analysisType;
        //    mBodyMesh = bodymesh;
        //}




    }
}
