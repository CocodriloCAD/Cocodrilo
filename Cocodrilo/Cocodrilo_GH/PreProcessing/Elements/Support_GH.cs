using System;
using System.Collections.Generic;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using Rhino.Geometry;

using Cocodrilo.ElementProperties;

namespace Cocodrilo_GH.PreProcessing.Elements
{
    public class Support_GH : GH_Component
    {
        private bool mFixX = true;
        private bool mFixY = true;
        private bool mFixZ = true;
        private bool mFixRotation = false;
        private string mInitialDisplacementX = "0.0";
        private string mInitialDisplacementY = "0.0";
        private string mInitialDisplacementZ = "0.0";
        private bool mStrong = true;
        private SupportType mSupportType = SupportType.SupportPenaltyCondition;

        public Support_GH()
          : base("Support", "Support", "Dirichlet Boundary Conditions", "Cocodrilo", "Elements")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddBrepParameter("Surfaces", "Sur", "Geometries", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddCurveParameter("Curves", "Cur", "Geometries", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddCurveParameter("Edges", "Edg", "Geometries", GH_ParamAccess.list);
            pManager[2].Optional = true;
            pManager.AddPointParameter("Points", "Pts", "Geometries", GH_ParamAccess.list);
            pManager[3].Optional = true;
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometries", "Geo", "Geometries enhanced with support information.", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Surface> surfaces = new List<Surface>();
            DA.GetDataList(0, surfaces);

            List<Curve> curves = new List<Curve>();
            DA.GetDataList(1, curves);

            List<Curve> edges = new List<Curve>();
            DA.GetDataList(2, edges);

            List<Point3d> points = new List<Point3d>();
            DA.GetDataList(3, points);

            var geometries = new Cocodrilo_GH.PreProcessing.Geometries.Geometries();

            var support = new Support(mFixX, mFixY, mFixZ,
                mInitialDisplacementX, mInitialDisplacementY, mInitialDisplacementZ,
                mFixRotation, mFixRotation,
                mStrong, mSupportType.ToString());

            foreach (var brep in surfaces)
            {
                if (brep == null)
                    continue;

                var support_property = new PropertySupport(
                    GeometryType.GeometrySurface, support, new TimeInterval());

                geometries.breps.Add(new KeyValuePair<Brep, Property>(brep.ToBrep(), support_property));
            }

            foreach (var curve in curves)
            {
                var support_property = new PropertySupport(
                    GeometryType.GeometryCurve, support, new TimeInterval());

                geometries.curves.Add(new KeyValuePair<Curve, Property>(curve, support_property));
            }

            foreach (var edge in edges)
            {
                var support_property = new PropertySupport(
                    GeometryType.SurfaceEdge, support, new TimeInterval());

                geometries.edges.Add(new KeyValuePair<Curve, Property>(edge, support_property));
            }

            foreach (var point in points)
            {
                var support_property = new PropertySupport(
                    GeometryType.Point, support, new TimeInterval());

                Point new_point = new Point(point);

                geometries.points.Add(new KeyValuePair<Point, Property>(new_point, support_property));
            }

            DA.SetData(0, geometries);
        }

        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            Menu_AppendItem(menu, "Fix X", Menu_DoClick_FixX, true, mFixX);
            Menu_AppendItem(menu, "Fix Y", Menu_DoClick_FixY, true, mFixY);
            Menu_AppendItem(menu, "Fix Z", Menu_DoClick_FixZ, true, mFixZ);
            Menu_AppendItem(menu, "Fix Rotation", Menu_DoClick_FixRotation, true, mFixRotation);
            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Initial Displacements");
            Menu_AppendTextItem(menu, mInitialDisplacementX, Menu_SetDisplacementX, Menu_SetDisplacementXText, false);
            Menu_AppendTextItem(menu, mInitialDisplacementY, Menu_SetDisplacementY, Menu_SetDisplacementYText, false);
            Menu_AppendTextItem(menu, mInitialDisplacementZ, Menu_SetDisplacementZ, Menu_SetDisplacementZText, false);
            Menu_AppendSeparator(menu);
            var toolStripMenuItemSupportType = GH_DocumentObject.Menu_AppendItem(menu, "Support Type");
            foreach (SupportType pt in Enum.GetValues(typeof(SupportType)))
                GH_Component.Menu_AppendItem(toolStripMenuItemSupportType.DropDown, pt.ToString(), Menu_SupportTypeChanged, true, pt == mSupportType).Tag = pt;
            Menu_AppendItem(menu, "Strong/ Directly Fix DoFs", Menu_DoClick_Strong, true, mStrong);
        }

        private void Menu_SupportTypeChanged(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.ToolStripMenuItem item && item.Tag is SupportType)
            {
                mSupportType = (SupportType)item.Tag;
                item.Checked = true;
                ExpireSolution(true);
            }
        }

        private void Menu_DoClick_FixX(object sender, EventArgs e) { mFixX = !mFixX; ExpireSolution(true); }
        private void Menu_DoClick_FixY(object sender, EventArgs e) { mFixY = !mFixY; ExpireSolution(true); }
        private void Menu_DoClick_FixZ(object sender, EventArgs e) { mFixZ = !mFixZ; ExpireSolution(true); }
        private void Menu_DoClick_FixRotation(object sender, EventArgs e) { mFixRotation = !mFixRotation; ExpireSolution(true); }
        private void Menu_SetDisplacementX(Grasshopper.GUI.GH_MenuTextBox sender, System.Windows.Forms.KeyEventArgs e){}
        private void Menu_SetDisplacementXText(GH_MenuTextBox sender, string newText) { mInitialDisplacementX = newText; ExpireSolution(true); }
        private void Menu_SetDisplacementY(Grasshopper.GUI.GH_MenuTextBox sender, System.Windows.Forms.KeyEventArgs e){}
        private void Menu_SetDisplacementYText(GH_MenuTextBox sender, string newText) { mInitialDisplacementY = newText; ExpireSolution(true); }
        private void Menu_SetDisplacementZ(Grasshopper.GUI.GH_MenuTextBox sender, System.Windows.Forms.KeyEventArgs e){}
        private void Menu_SetDisplacementZText(GH_MenuTextBox sender, string newText) { mInitialDisplacementZ = newText; ExpireSolution(true); }
        private void Menu_DoClick_Strong(object sender, EventArgs e) { mStrong = !mStrong; ExpireSolution(true); }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetBoolean("FixX", mFixX);
            writer.SetBoolean("FixY", mFixY);
            writer.SetBoolean("FixZ", mFixZ);
            writer.SetBoolean("FixRotation", mFixRotation);
            writer.SetString("InitialDisplacementX", mInitialDisplacementX);
            writer.SetString("InitialDisplacementY", mInitialDisplacementY);
            writer.SetString("InitialDisplacementZ", mInitialDisplacementZ);
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            reader.TryGetBoolean("FixX", ref mFixX);
            reader.TryGetBoolean("FixY", ref mFixY);
            reader.TryGetBoolean("FixZ", ref mFixZ);
            reader.TryGetBoolean("FixRotation", ref mFixRotation);
            reader.TryGetString("InitialDisplacementX", ref mInitialDisplacementX);
            reader.TryGetString("InitialDisplacementY", ref mInitialDisplacementY);
            reader.TryGetString("InitialDisplacementZ", ref mInitialDisplacementZ);
            return base.Read(reader);
        }

        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("217C3E6E-DA4A-4194-8018-931F9450C34F"); }
        }
    }
}