using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Grasshopper.GUI;
using Grasshopper.Kernel;
using Rhino.Geometry;

using Cocodrilo.ElementProperties;

namespace Cocodrilo_GH.PreProcessing.Elements
{
    enum LoadType
    {
        DEAD_LOAD,
        GEOMETRY_LOAD,
        PRESSURE_LOAD,
        PRESSURE_LOAD_FL
    }
    public class Load_GH : GH_Component
    {
        private string mLoadX = "0.0";
        private string mLoadY = "0.0";
        private string mLoadZ = "0.0";

        private LoadType mSelectedLoadType = LoadType.GEOMETRY_LOAD;
        public Load_GH()
          : base("Load", "Load",
              "Load Boundary Condition",
              "Cocodrilo", "Elements")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddSurfaceParameter("Surfaces", "Sur", "Geometries", GH_ParamAccess.list);
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
            pManager.AddGenericParameter("Geometries", "Geo", "Geometries enhanced with load information.", GH_ParamAccess.item);
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


            foreach (var surface in surfaces)
            {
                var load_type = mSelectedLoadType.ToString();
                if (load_type == "GEOMETRY_LOAD")
                {
                    load_type = "SURFACE_LOAD";
                }
                var load = new Load(mLoadX, mLoadY, mLoadZ, "1.0", load_type);

                var support_property = new PropertyLoad(
                    GeometryType.GeometrySurface, load, new TimeInterval());

                geometries.breps.Add(new KeyValuePair<Brep, Property>(surface.ToBrep(), support_property));
            }

            foreach (var curve in curves)
            {
                var load_type = mSelectedLoadType.ToString();
                if (load_type == "GEOMETRY_LOAD")
                {
                    load_type = "LINE_LOAD";
                }
                var load = new Load(mLoadX, mLoadY, mLoadZ, "1.0", load_type);

                var support_property = new PropertyLoad(
                    GeometryType.GeometryCurve, load, new TimeInterval());

                geometries.curves.Add(new KeyValuePair<Curve, Property>(curve, support_property));
            }

            foreach (var edge in edges)
            {
                var load_type = mSelectedLoadType.ToString();
                if (load_type == "GEOMETRY_LOAD")
                {
                    load_type = "LINE_LOAD";
                }
                var load = new Load(mLoadX, mLoadY, mLoadZ, "1.0", load_type);

                var support_property = new PropertyLoad(
                    GeometryType.SurfaceEdge, load, new TimeInterval());

                geometries.edges.Add(new KeyValuePair<Curve, Property>(edge, support_property));
            }

            foreach (var point in points)
            {
                var load_type = mSelectedLoadType.ToString();
                if (load_type == "GEOMETRY_LOAD")
                {
                    load_type = "POINT_LOAD";
                }
                var load = new Load(mLoadX, mLoadY, mLoadZ, "1.0", load_type);

                var support_property = new PropertyLoad(
                    GeometryType.Point, load, new TimeInterval());

                Point new_point = new Point(point);

                geometries.points.Add(new KeyValuePair<Point, Property>(new_point, support_property));
            }

            DA.SetData(0, geometries);
        }
        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            foreach (LoadType pt in Enum.GetValues(typeof(LoadType)))
                GH_Component.Menu_AppendItem(menu, pt.ToString(), Menu_LoadTypeChanged, true, pt == this.mSelectedLoadType).Tag = pt;

            Menu_AppendSeparator(menu);
            Menu_AppendTextItem(menu, mLoadX, Menu_SetLoadX, Menu_SetLoadXText, false);
            if (mSelectedLoadType != LoadType.PRESSURE_LOAD && mSelectedLoadType != LoadType.PRESSURE_LOAD_FL)
            {
                Menu_AppendTextItem(menu, mLoadY, Menu_SetLoadY, Menu_SetLoadYText, false);
                Menu_AppendTextItem(menu, mLoadZ, Menu_SetLoadZ, Menu_SetLoadZText, false);
            }
        }

        private void Menu_LoadTypeChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is LoadType)
            {
                this.mSelectedLoadType = (LoadType)item.Tag;
                item.Checked = true;
                ExpireSolution(true);
            }
        }

        private void Menu_SetLoadX(GH_MenuTextBox sender, KeyEventArgs e)
        {
        }

        private void Menu_SetLoadXText(GH_MenuTextBox sender, string newText) { mLoadX = newText; ExpireSolution(true); }
        private void Menu_SetLoadY(GH_MenuTextBox sender, KeyEventArgs e)
        {
        }

        private void Menu_SetLoadYText(GH_MenuTextBox sender, string newText) { mLoadY = newText; ExpireSolution(true); }
        private void Menu_SetLoadZ(GH_MenuTextBox sender, KeyEventArgs e)
        {
        }

        private void Menu_SetLoadZText(GH_MenuTextBox sender, string newText){ mLoadZ = newText; ExpireSolution(true); }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetString("LoadX", mLoadX);
            writer.SetString("LoadY", mLoadY);
            writer.SetString("LoadZ", mLoadZ);
            writer.SetInt32("SelectedLoadType", (int)mSelectedLoadType);
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            reader.TryGetString("LoadX", ref mLoadX);
            reader.TryGetString("LoadY", ref mLoadY);
            reader.TryGetString("LoadZ", ref mLoadZ);
            int load_type_index = -1;
            if (reader.TryGetInt32("SelectedLoadType", ref load_type_index))
                mSelectedLoadType = (LoadType)load_type_index;

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
            get { return new Guid("23DFE437-E8F8-41FB-ADF9-710BC86986E1"); }
        }
    }
}