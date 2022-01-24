using System;
using System.Collections.Generic;
using Grasshopper.Kernel;

using Cocodrilo.IO;

namespace Cocodrilo_GH.PreProcessing.IO
{
    public class Model_CO_SIM_GH : GH_Component
    {
        public Model_CO_SIM_GH()
          : base("Co-Simulation Model", "Co-Sim",
              "Co-Simulation Model",
              "Cocodrilo", "Models")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Models", "Models", "Physics Model", GH_ParamAccess.list);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Output> output_list = new List<Output>();
            DA.GetDataList(0, output_list);

            var output = new OutputKratosCO_SIM();
            output.StartAnalysis(output_list);

            DA.SetData(0, output);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        public override Guid ComponentGuid { get { return new Guid("03548239-FC4E-476A-8329-4DF7703703ED"); } }
    }
}