using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cocodrilo.IO;
using Cocodrilo.Materials;
using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.IO
{
    public class ModelModuleOptimization_GH : GH_Component
    {
        /// <summary>
        /// Initializes a new instance of the ModelModuleOptimization class.
        /// </summary>
        public ModelModuleOptimization_GH()
          : base("Module Optimization", "Module Optimization",
              "Employs an optimization of material application.",
              "Cocodrilo", "Models")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Project Path", "P", "Project path to the IGA model", GH_ParamAccess.item);
            pManager.AddGenericParameter("Materials", "Ma", "Possible materials for modules.", GH_ParamAccess.list);
            pManager.AddBooleanParameter("Write Input Files", "W", "Write input files for the simulation", GH_ParamAccess.item, false);
            pManager.AddBooleanParameter("Run Analysis", "R", "Runs the analysis", GH_ParamAccess.item, false);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string project_path = "";
            DA.GetData(0, ref project_path);

            List<Material> materials = new List<Material>();
            if (!DA.GetDataList(1, materials)) return;

            bool write_input_files = false;
            if (!DA.GetData(2, ref write_input_files)) return;

            bool run_analysis = false;
            if (!DA.GetData(3, ref run_analysis)) return;

            if (write_input_files)
            {
                List<int> material_ids = materials.Select(item => item.Id).ToList();
                OutputKratosModuleOptimization.WriteOptimizationFiles(material_ids, project_path);
            }

            if (run_analysis)
            {
                Process proc = null;
                try
                {
                    proc = new Process();
                    proc.StartInfo.WorkingDirectory = project_path + "/";
                    proc.StartInfo.FileName = project_path + "\\run_kratos.bat";
                    proc.StartInfo.CreateNoWindow = true;
                    proc.Start();
                }
                catch (Exception ex)
                {
                    RhinoApp.WriteLine(ex.StackTrace.ToString());
                }

                Message = "Kratos started.";
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("D234DCBD-328A-4065-A0B7-9139756E41C0"); }
        }
    }
}