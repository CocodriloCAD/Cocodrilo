using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.Analysis
{
    public class EigenvalueAnalysis_GH : GH_Component
    {
        private string mEigenSolverType = "eigen_eigensystem";

        /// <summary>
        /// Initializes a new instance of the EigenvalueAnalysis_GH class.
        /// </summary>
        public EigenvalueAnalysis_GH()
          : base("EigenvalueAnalysis", "Eigenvalues",
              "Analysis of the eigen values.",
              "Cocodrilo", "Analyses")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "N", "Name of Analysis", GH_ParamAccess.item, "EigenvalueAnalysis");
            pManager.AddIntegerParameter("Number Of Eigenvalues", "NE", "Number of eigen values", GH_ParamAccess.item, 10);
            pManager.AddIntegerParameter("Maximum Iterations", "MI", "Maximum solving iterations", GH_ParamAccess.item, 20);        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis", "Ana", "NonLinearStaticAnalysis", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string Name = "";
            if (!DA.GetData(0, ref Name)) return;

            int NumEigenvalues = 0;
            if (!DA.GetData(1, ref NumEigenvalues)) return;

            int MaximumIterations = 0;
            if (!DA.GetData(2, ref MaximumIterations)) return;

            // Make name fit
            if (Name.Contains(" "))
            {
                Name = Name.Replace(" ", "");
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed.");
            }

            DA.SetData(0, new Cocodrilo.Analyses.AnalysisEigenvalue(Name, 0.001, NumEigenvalues, MaximumIterations, mEigenSolverType));
        }

        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            var toolStripMenuItemSolverType = GH_DocumentObject.Menu_AppendItem(menu, "Eigen Solver Type");
            GH_Component.Menu_AppendItem(toolStripMenuItemSolverType.DropDown, "eigen_eigensystem", Menu_SolverTypeChanged, true, "eigen_eigensystem" == mEigenSolverType).Tag = "eigen_eigensystem";
            GH_Component.Menu_AppendItem(toolStripMenuItemSolverType.DropDown, "spectra_sym_g_eigs_shift", Menu_SolverTypeChanged, true, "spectra_sym_g_eigs_shift" == mEigenSolverType).Tag = "spectra_sym_g_eigs_shift";
            GH_Component.Menu_AppendItem(toolStripMenuItemSolverType.DropDown, "feast", Menu_SolverTypeChanged, true, "feast" == mEigenSolverType).Tag = "feast";    
        }

        private void Menu_SolverTypeChanged(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.ToolStripMenuItem item && item.Tag is string)
            {
                mEigenSolverType = (string)item.Tag;
                item.Checked = true;
                ExpireSolution(true);
            }
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetString("EigenSolverType", mEigenSolverType);
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            // int eigen_solver_type_index = -1;
            // if (reader.TryGetInt32("EigenSolverType", ref eigen_solver_type_index))
            //     mEigenSolverType = (string)eigen_solver_type_index;
            reader.TryGetString("EigenSolverType", ref mEigenSolverType);
            return base.Read(reader);
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.analysis_eig; }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("33079E71-5B69-4627-8616-65DCFEC9DF93"); }
        }
    }
}