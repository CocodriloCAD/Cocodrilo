using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.Analysis
{
    public class MPM_GH : GH_Component
    {
        int m_p;
        int m_q;
        int m_u;
        int m_v;
        Point3d m_point_1;
        Point3d m_point_2;

        /// <summary>
        /// Initializes a new instance of the MPM_GH class.
        /// </summary>
        public MPM_GH()
          : base("MPM", "MPM",
              "Material Point Method",
              "Cocodrilo", "Analyses")
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddTextParameter("Name", "Name", "Name of Analysis", GH_ParamAccess.item, "MpmAnalysis");
            pManager.AddIntegerParameter("Polynomial Degree p", "p", "Polynomial Degree p", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("Polynomial Degree q", "q", "Polynomial Degree q", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("Elements in u", "u", "Elements in u", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("Elements in v", "v", "Elements in v", GH_ParamAccess.item, 1);
            pManager.AddPointParameter("Corner Point 1", "p1", "Corner Point 1", GH_ParamAccess.item);
            pManager.AddPointParameter("Corner Point 2", "p2", "Corner Point 2", GH_ParamAccess.item);
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis", "A", "MPM", GH_ParamAccess.item);
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
            string Name = "";
            if (!DA.GetData(0, ref Name)) return;

            if (!DA.GetData(1, ref m_p)) return;
            if (!DA.GetData(2, ref m_q)) return;
            if (!DA.GetData(3, ref m_u)) return;
            if (!DA.GetData(4, ref m_v)) return;
            if (!DA.GetData(5, ref m_point_1)) return;
            if (!DA.GetData(6, ref m_point_2)) return;

            // Make name fit
            if (Name.Contains(" "))
            {
                Name = Name.Replace(" ", "");
                AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Spaces removed.");
            }

            DA.SetData(0, new Cocodrilo.Analyses.AnalysisMpm(Name));
        }

        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            if (Hidden) return;
            if (Locked) return;


            NurbsSurface background_surface = NurbsSurface.CreateFromCorners(
                m_point_1,
                new Point3d(m_point_2.X, m_point_1.Y, m_point_1.Z),
                m_point_2,
                new Point3d(m_point_1.X, m_point_2.Y, m_point_1.Z));

            background_surface.IncreaseDegreeU(m_p);
            background_surface.IncreaseDegreeV(m_q);

            NurbsSurface newsurface = new NurbsSurface(background_surface);

            int ref_u = m_u + 1;
            int ref_v = m_v + 1;
            for (int j = 1; j < ref_u; j++)
            {
                for (int k = 1; k < background_surface.KnotsU.Count; k++)
                {
                    if (background_surface.KnotsU[k - 1] < background_surface.KnotsU[k])//nonzero knot span
                    {
                        var knotspansize = background_surface.KnotsU[k] - background_surface.KnotsU[k - 1];
                        for (int l = 1; l < ref_u; l++) // dividing in #ref_u elements
                            newsurface.KnotsU.InsertKnot(background_surface.KnotsU[k - 1] + l * knotspansize / ref_u);
                    }
                }
            }
            for (int j = 1; j < ref_v; j++)
            {
                for (int k = 1; k < background_surface.KnotsV.Count; k++)
                {
                    if (background_surface.KnotsV[k - 1] < background_surface.KnotsV[k])//nonzero knot span
                    {
                        var knotspansize = background_surface.KnotsV[k] - background_surface.KnotsV[k - 1];
                        for (int l = 1; l < ref_v; l++) // dividing in #ref_u elements
                            newsurface.KnotsV.InsertKnot(background_surface.KnotsV[k - 1] + l * knotspansize / ref_v);
                    }
                }
            }
            int span1 = background_surface.SpanCount(0);

            if (m_point_1 != null && m_point_2 != null && background_surface != null)
            {
                args.Display.DrawSurface(newsurface, System.Drawing.Color.FromArgb(0, 101, 189), 1);

                Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
            }
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.analysis_mpm; }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("F35FC966-715D-43E4-A918-539FE323CDDF"); }
        }
    }
}