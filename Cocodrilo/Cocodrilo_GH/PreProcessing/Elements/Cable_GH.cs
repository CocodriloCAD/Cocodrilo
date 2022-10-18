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
        private CableCouplingType mCouplingType = CableCouplingType.EntireCurve;
        public Cable_GH()
          : base("Cable", "Cable", "Cable Element", "Cocodrilo", "Elements")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddCurveParameter("Curves", "C", "Geometries", GH_ParamAccess.list);
            pManager[0].Optional = true;
            pManager.AddCurveParameter("Edges", "E", "Geometries", GH_ParamAccess.list);
            pManager[1].Optional = true;
            pManager.AddGenericParameter("Material", "M", "Material of element", GH_ParamAccess.item);
            pManager[2].Optional = true;
            pManager.AddNumberParameter("Prestress", "P", "Cauchy pre-stress within truss/cable", GH_ParamAccess.item, 0.0);
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

            double prestress = 0.0;
            if (!DA.GetData(3, ref prestress)) return;

            Property this_property = null;
            var geometries = new Cocodrilo_GH.PreProcessing.Geometries.Geometries();

            foreach (var edge in edges)
            {
                CableProperties cable_properties = new CableProperties(prestress, mArea, mCouplingType);
                this_property = new PropertyCable(GeometryType.SurfaceEdge ,material.Id, cable_properties, false);
                geometries.edges.Add(new KeyValuePair<Curve, Property>(edge, this_property));
            }

            foreach (var curve in curves)
            {
                CableProperties cable_properties = new CableProperties(prestress, mArea, mCouplingType);
                this_property = new PropertyCable(GeometryType.GeometryCurve ,material.Id, cable_properties, false);
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
            Menu_AppendSeparator(menu);
            foreach (CableCouplingType pt in Enum.GetValues(typeof(CableCouplingType)))
                GH_Component.Menu_AppendItem(menu, pt.ToString(), Menu_CouplingTypeChanged, true, pt == mCouplingType).Tag = pt;
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

        private void Menu_CouplingTypeChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is CableCouplingType)
            {
                mCouplingType = (CableCouplingType)item.Tag;
                item.Checked = true;
                ExpireSolution(true);
            }
        }

        #endregion
        protected override System.Drawing.Bitmap Icon
        {
            get { return Properties.Resources.elements_cable; }
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetDouble("Area", this.mArea);
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            reader.TryGetDouble("Area", ref mArea);
            return base.Read(reader);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("A3A15CCC-83A4-4244-8B2B-E51E6F126216"); }
        }
    }
}