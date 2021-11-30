using System;
using System.Collections.Generic;
using Grasshopper.Kernel;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.Elements
{
    public class Refinement_Surface_GH : GH_Component
    {
        bool mRefineWithinRhino = true;

        public Refinement_Surface_GH()
          : base("Refinement_Surface", "Refinement_Surface",
              "Refinement of Surface Geometries",
              "Cocodrilo", "Elements")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Surface", "Sur", "Geometry of Element", GH_ParamAccess.list);
            pManager.AddIntegerParameter("p", "p", "Final Polynomial Degree p", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("q", "q", "Polynomial Degree q", GH_ParamAccess.item, 1);
            pManager.AddIntegerParameter("InsertKnotU", "u", "Number of Knots inserted per Span in u", GH_ParamAccess.item, 0);
            pManager.AddIntegerParameter("InsertKnotV", "v", "Number of Knots inserted per Span in v", GH_ParamAccess.item, 0);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surface", "Sur", "Refined Surface", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            var breps = new List<Brep>();
            if (!DA.GetDataList(0, breps)) return;
            int p = 1;
            if (!DA.GetData(1, ref p)) return;
            int q = 1;
            if (!DA.GetData(2, ref q)) return;
            int insert_knot_u = 0;
            if (!DA.GetData(3, ref insert_knot_u)) return;
            int insert_knot_v = 0;
            if (!DA.GetData(4, ref insert_knot_v)) return;

            var surface_list_out = new List<Surface>();
            foreach (var brep in breps)
            {
                foreach (var surface in brep.Surfaces)
                {
                    var nurbs_surface = surface.ToNurbsSurface();
                    nurbs_surface.IncreaseDegreeU(p);
                    nurbs_surface.IncreaseDegreeV(q);

                    surface_list_out.Add(nurbs_surface);

                    if (mRefineWithinRhino)
                    {
                        int ref_u = insert_knot_u + 1;
                        int ref_v = insert_knot_v + 1;

                        var span_u = nurbs_surface.GetSpanVector(0);
                        for (int k = 1; k < span_u.Length; k++)
                        {
                            if (span_u[k - 1] < span_u[k])//nonzero knot span
                            {
                                var knotspansize = span_u[k] - span_u[k - 1];
                                for (int l = 1; l < ref_u; l++) // dividing in #ref_u elements
                                    nurbs_surface.KnotsU.InsertKnot(span_u[k - 1] + l * knotspansize / ref_u);
                            }
                        }

                        var span_v = nurbs_surface.GetSpanVector(1);
                        for (int k = 1; k < span_v.Length; k++)
                        {
                            if (span_v[k - 1] < span_v[k])//nonzero knot span
                            {
                                var knotspansize = span_v[k] - span_v[k - 1];
                                for (int l = 1; l < ref_v; l++) // dividing in #ref_u elements
                                    nurbs_surface.KnotsV.InsertKnot(span_v[k - 1] + l * knotspansize / ref_v);
                            }
                        }
                    }
                }
            }

            var refinement_surface = (mRefineWithinRhino)
                ? new Cocodrilo.Refinement.RefinementSurface(p, q, 0, 0)
                : new Cocodrilo.Refinement.RefinementSurface(p,q, insert_knot_u, insert_knot_v);
            foreach (var surface in surface_list_out)
            {
                var ud = surface.UserData.Find(typeof(Cocodrilo.UserData.UserDataSurface)) as Cocodrilo.UserData.UserDataSurface;
                if (ud == null)
                {
                    ud = new Cocodrilo.UserData.UserDataSurface();
                    surface.UserData.Add(ud);
                }
                ud.ChangeRefinement(refinement_surface);
            }

            DA.SetDataList(0, surface_list_out);
        }

        #region Menu Items
        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "RefineWithinRhino", Menu_DoClick_RefineWithinRhino, true, mRefineWithinRhino);
        }
        private void Menu_DoClick_RefineWithinRhino(object sender, EventArgs e) => mRefineWithinRhino = !mRefineWithinRhino;

        #endregion

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetBoolean("RefineWithinRhino", mRefineWithinRhino);
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            reader.TryGetBoolean("RefineWithinRhino", ref mRefineWithinRhino);
            return base.Read(reader);
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

        public override Guid ComponentGuid
        {
            get { return new Guid("A7FD8173-4FB8-4873-B966-361CE04DD622"); }
        }
    }
}