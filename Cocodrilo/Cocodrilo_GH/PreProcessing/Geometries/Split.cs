using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino;
using Rhino.Geometry;

namespace Cocodrilo_GH.PreProcessing.Geometries
{
    public class Split : GH_Component
    {
        public Split()
          : base("Split", "Split Brep",
              "Splits a Brep by a set of curves.",
              "Cocodrilo", "Geometries")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Breps", "Breps", "Breps", GH_ParamAccess.list);
            pManager.AddCurveParameter("Curves", "Curves", "Trimming Curves", GH_ParamAccess.list);
            pManager.AddNumberParameter("Tolerance", "T", "Tolerance", GH_ParamAccess.item, 0.01);
            pManager[2].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddBrepParameter("Breps", "Breps", "Breps", GH_ParamAccess.list);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Brep> breps = new List<Brep>();
            DA.GetDataList(0, breps);

            List<Curve> curves = new List<Curve>();
            DA.GetDataList(1, curves);

            double tolerance = 0.0;
            DA.GetData(2, ref tolerance);

            Vector3d normal = new Vector3d(0, 0, 1.0);
            var new_breps = new List<Brep>();
            foreach (var brep in breps)
            {
                new_breps.AddRange(brep.Split(curves, normal, false, tolerance));
            }

            DA.SetDataList(0, new_breps);
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
            get { return new Guid("845ED291-A457-45D0-BCAD-93CF76B5DC0E"); }
        }
    }
}