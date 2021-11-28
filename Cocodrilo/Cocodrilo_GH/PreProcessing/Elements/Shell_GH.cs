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
    enum FormulationType
    {
        Membrane,
        Shell3p,
        Shell5pHierarchic,
        Shell5p
    }

    public class Shell_GH : GH_Component
    {
        private FormulationType mFormulationType = FormulationType.Shell3p;
        private double mThickness = 1.0;

        public Shell_GH()
          : base("Shell", "Shell", "Shell Element", "Cocodrilo", "Elements")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Surface", "Sur", "Geometry of Element", GH_ParamAccess.list);
            pManager.AddGenericParameter("Material", "Mat", "Material of Element", GH_ParamAccess.item);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("Geometries", "Geo", "Geometries Enhanced with Element Formulations", GH_ParamAccess.item);
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            List<Surface> surfaces = new List<Surface>();
            DA.GetDataList(0, surfaces);

            Material material = null;
            if (!DA.GetData(1, ref material)) return;

            Property this_property = null;
            if (mFormulationType == FormulationType.Membrane)
            {
                MembraneProperties membrane_properties = new MembraneProperties(
                    1, new double[] { 1, 0, 0 }, new double[] { 0, 1, 0 }, 0.0, 0.0);

                this_property = new PropertyMembrane(material.Id, false, membrane_properties);
            }
            else if (mFormulationType == FormulationType.Shell3p)
            {
                ShellProperties shell_properties = new ShellProperties(mThickness, true, "Shell3pElement");

                this_property = new PropertyShell(material.Id, shell_properties);
            }
            else if (mFormulationType == FormulationType.Shell5pHierarchic)
            {
                ShellProperties shell_properties = new ShellProperties(mThickness, true, "Shell5pHierarchicElement");

                this_property = new PropertyShell(material.Id, shell_properties);
            }
            else if (mFormulationType == FormulationType.Shell5p)
            {
                ShellProperties shell_properties = new ShellProperties(mThickness, true, "Shell5pElement");

                this_property = new PropertyShell(material.Id, shell_properties);
            }

            var geometries = new Cocodrilo_GH.PreProcessing.Geometries.Geometries();
            foreach (var brep in surfaces)
            {
                geometries.breps.Add(new KeyValuePair<Brep, Property>(brep.ToBrep(), this_property));
            }

            DA.SetData(0, geometries);
        }
        #region Menu Items
        protected override void AppendAdditionalComponentMenuItems(ToolStripDropDown menu)
        {
            foreach (FormulationType pt in Enum.GetValues(typeof(FormulationType)))
                GH_Component.Menu_AppendItem(menu, pt.ToString(), Menu_FormulationTypeChanged, true, pt == mFormulationType).Tag = pt;
            Menu_AppendSeparator(menu);
            Menu_AppendItem(menu, "Thickness:");
            Menu_AppendTextItem(menu, Convert.ToString(mThickness), Menu_Thickness_KeyDown, Menu_Thickness_EventHandler, false);
        }

        private void Menu_Thickness_EventHandler(GH_MenuTextBox sender, string newText)
        {
            if (newText != "")
            {
                mThickness = Convert.ToDouble(newText);
            }
            else
                mThickness = 1;
            ExpireSolution(true);
        }
        private void Menu_Thickness_KeyDown(GH_MenuTextBox sender, KeyEventArgs e) {}

        private void Menu_FormulationTypeChanged(object sender, EventArgs e)
        {
            if (sender is ToolStripMenuItem item && item.Tag is FormulationType)
            {
                mFormulationType = (FormulationType)item.Tag;
                item.Checked = true;
                ExpireSolution(true);
            }
        }
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
            writer.SetInt32("FormulationType", (int)mFormulationType);
            writer.SetDouble("Thickness", this.mThickness);
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            int formulation_type_index = -1;
            if (reader.TryGetInt32("FormulationType", ref formulation_type_index))
                mFormulationType = (FormulationType)formulation_type_index;
            reader.TryGetDouble("Thickness", ref mThickness);

            return base.Read(reader);
        }

        public override Guid ComponentGuid
        {
            get { return new Guid("F109FC45-89F7-40F8-8EF5-17C7F6ACC332"); }
        }
    }
}