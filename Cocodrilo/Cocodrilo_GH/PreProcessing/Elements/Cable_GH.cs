using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Rhino.Geometry;

using Cocodrilo.Materials;
using Cocodrilo.UserData;
using Cocodrilo.ElementProperties;
using System.Windows.Forms;
using Grasshopper.GUI;

namespace Cocodrilo_GH.PreProcessing.Elements
{

    public class Cable_GH : GH_Component
    {
        private double mArea = 1.0;
        private double mPrestress = 1.0;

        public Cable_GH()
          : base("Cable", "Cable", "Cable Element", "Cocodrilo", "Elements")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "Cur", "Geometries", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddCurveParameter("Edges", "Edg", "Geometries", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Material", "Mat", "Material of Element", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometries", "Geo", "Geometries Enhanced with Element Formulations", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {

            List<Curve> curves = new List<Curve>();
            DA.GetDataList(0, curves);

            List<Curve> edges = new List<Curve>();
            DA.GetDataList(1, edges);

            Material material = null;
            if (!DA.GetData(2, ref material)) return;

            Property this_property = null;
            var geometries = new Cocodrilo_GH.PreProcessing.Geometries.Geometries();

            foreach (var edge in edges)
            {
                CableProperties cable_properties = new CableProperties(mPrestress, mArea);
                this_property = new PropertyCable(GeometryType.SurfaceEdge ,material.Id, cable_properties, false);
                geometries.edges.Add(new KeyValuePair<Curve, Property>(edge, this_property));
            }

            foreach (var curve in curves)
            {
                CableProperties cable_properties = new CableProperties(mPrestress, mArea);
                this_property = new PropertyCable(GeometryType.SurfaceEdge ,material.Id, cable_properties, false);
                geometries.curves.Add(new KeyValuePair<Curve, Property>(curve, this_property));
            }
            
            DA.SetData(0, geometries);
        }
        #region Menu Items
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Area:");
            Menu_AppendTextItem(menu, Convert.ToString(mArea), Menu_Area_KeyDown, Menu_Area_EventHandler, false);
            Menu_AppendItem(menu, "Prestress:");
            Menu_AppendTextItem(menu, Convert.ToString(mPrestress), Menu_Prestress_KeyDown, Menu_Prestress_EventHandler, false);
        }

        private void Menu_Area_EventHandler(GH_MenuTextBox sender, string newText)
        {
            if (newText != "")
            {
                mArea = Convert.ToDouble(newText);
            }
            else
                mArea = 1;
            ExpireSolution(true);
        }
        private void Menu_Area_KeyDown(GH_MenuTextBox sender, KeyEventArgs e) {}

        private void Menu_Prestress_EventHandler(GH_MenuTextBox sender, string newText)
        {
            if (newText != "")
            {
                mPrestress = Convert.ToDouble(newText);
            }
            else
                mPrestress = 1;
            ExpireSolution(true);
        }
        private void Menu_Prestress_KeyDown(GH_MenuTextBox sender, KeyEventArgs e) {}

        #endregion
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                return null;
            }
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetDouble("Area", this.mArea);
            writer.SetDouble("Prestress", this.mPrestress);
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            reader.TryGetDouble("Area", ref mArea);
            reader.TryGetDouble("Prestress", ref mPrestress);
            return base.Read(reader);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("A3A15CCC-83A4-4244-8B2B-E51E6F126216"); }
        }
    }
}