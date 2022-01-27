using System;
using System.Collections.Generic;

using Grasshopper.Kernel;
using Grasshopper.Kernel.Data;
using Grasshopper.Kernel.Types;
using Rhino.Geometry;
using Cocodrilo.ElementProperties;

namespace Cocodrilo_GH.PreProcessing.IO
{
    public class Model_IGA_GH : GH_Component
    {
        private List<Brep> mBrepList = new List<Brep>();
        private List<Curve> mCurveList = new List<Curve>();

        private bool mShowPreProcessing = false;
        private bool mShowElements = true;
        private bool mShowSupports = true;
        private bool mShowLoads = true;
        private bool mShowCouplings = true;
        private bool mShowIds = false;

        public Model_IGA_GH()
          : base("IGA Model", "IGA Model",
              "Isogeometric analysis model.",
              "Cocodrilo", "Models")
        {
        }

        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
            pManager.AddGenericParameter("Analysis", "Ana", "Analysis", GH_ParamAccess.item);
            pManager.AddGenericParameter("Geometries", "Geo", "Geometries", GH_ParamAccess.tree);
            pManager.AddBooleanParameter("Run", "Run", "Run output", GH_ParamAccess.item, false);
        }

        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
            pManager.AddGenericParameter("IGA Model", "Model", "IGA Model", GH_ParamAccess.item);
            pManager[0].Optional = true;
            pManager.AddGenericParameter("Path", "Path", "Path to input files.", GH_ParamAccess.item);
            pManager[1].Optional = true;
        }

        protected override void SolveInstance(IGH_DataAccess DA)
        {
            Cocodrilo.Analyses.Analysis this_analysis = null;
            if ((!DA.GetData(0, ref this_analysis))) return;

            if (!DA.GetDataTree(1, out GH_Structure<IGH_Goo> geometries)) return;
            var geometries_flat = geometries.FlattenData();

            bool run_analysis = false;
            if (!DA.GetData(2, ref run_analysis)) return;

            if (run_analysis)
            {
                /// Resets the entire user data stored on the geometries.
                ResetUserData(geometries_flat);

                mBrepList = new List<Brep>();
                mCurveList = new List<Curve>();
                List<Point> point_list = new List<Point>();
                foreach (var obj in geometries_flat)
                {
                    bool success = obj.CastTo(out Cocodrilo_GH.PreProcessing.Geometries.Geometries geoms);
                    if (success)
                    {
                        foreach (var brep in geoms.breps)
                        {
                            bool add_brep = true;
                            foreach (var surface in brep.Key.Surfaces)
                            {
                                var ud = surface.UserData.Find(typeof(Cocodrilo.UserData.UserDataSurface)) as Cocodrilo.UserData.UserDataSurface;
                                if (ud == null)
                                {
                                    ud = new Cocodrilo.UserData.UserDataSurface();
                                    surface.UserData.Add(ud);
                                }
                                else
                                {
                                    foreach (var old_brep in mBrepList)
                                    {
                                        foreach (var old_surface in old_brep.Surfaces)
                                        {
                                            var ud2 = old_surface.UserData.Find(typeof(Cocodrilo.UserData.UserDataSurface)) as Cocodrilo.UserData.UserDataSurface;

                                            if (ReferenceEquals(ud2.GetCurrentElementData(), ud.GetCurrentElementData()))
                                                add_brep = false;
                                        }
                                    }
                                }

                                ud.AddNumericalElement(brep.Value);
                            }
                            if (add_brep)
                            {
                                mBrepList.Add(brep.Key);
                            }
                        }
                        foreach (var curve in geoms.curves)
                        {
                            bool add_curve = true;
                            var ud = curve.Key.UserData.Find(typeof(Cocodrilo.UserData.UserDataCurve)) as Cocodrilo.UserData.UserDataCurve;
                            if (ud == null)
                            {
                                ud = new Cocodrilo.UserData.UserDataCurve();
                                curve.Key.UserData.Add(ud);
                            }
                            else
                            {
                                foreach (var old_curve in mCurveList)
                                {
                                    var ud2 = old_curve.UserData.Find(typeof(Cocodrilo.UserData.UserDataCurve)) as Cocodrilo.UserData.UserDataCurve;

                                    if (ReferenceEquals(ud2.GetCurrentElementData(), ud.GetCurrentElementData()))
                                        add_curve = false;
                                }
                            }

                            ud.AddNumericalElement(curve.Value);

                            if (add_curve)
                            {
                                mCurveList.Add(curve.Key);
                            }
                        }
                        foreach (var edge in geoms.edges)
                        {
                            bool add_edge = true;
                            var ud = edge.Key.UserData.Find(typeof(Cocodrilo.UserData.UserDataEdge)) as Cocodrilo.UserData.UserDataEdge;
                            if (ud == null)
                            {
                                ud = new Cocodrilo.UserData.UserDataEdge();
                                edge.Key.UserData.Add(ud);
                            }
                            else
                            {
                                foreach (var old_edge in mCurveList)
                                {
                                    var ud2 = old_edge.UserData.Find(typeof(Cocodrilo.UserData.UserDataEdge)) as Cocodrilo.UserData.UserDataEdge;

                                    if (ReferenceEquals(ud2.GetCurrentElementData(), ud.GetCurrentElementData()))
                                        add_edge = false;
                                }
                            }

                            ud.AddNumericalElement(edge.Value);

                            if (add_edge)
                            {
                                mCurveList.Add(edge.Key);
                            }
                        }
                        foreach (var point in geoms.points)
                        {
                            bool add_point = true;
                            var ud = point.Key.UserData.Find(typeof(Cocodrilo.UserData.UserDataPoint)) as Cocodrilo.UserData.UserDataPoint;
                            if (ud == null)
                            {
                                ud = new Cocodrilo.UserData.UserDataPoint();
                                point.Key.UserData.Add(ud);
                            }
                            else
                            {
                                foreach (var old_edge in mCurveList)
                                {
                                    var ud2 = old_edge.UserData.Find(typeof(Cocodrilo.UserData.UserDataPoint)) as Cocodrilo.UserData.UserDataPoint;
                                    if (ud2 != null)
                                    {
                                        if (ReferenceEquals(ud2.GetCurrentElementData(), ud.GetCurrentElementData()))
                                            add_point = false;
                                    }
                                }
                            }

                            ud.AddNumericalElement(point.Value);

                            if (add_point)
                            {
                                point_list.Add(point.Key);
                            }
                        }
                    }
                    else
                    {
                        if (obj is GH_Brep)
                        {
                            Brep brep = null;
                            GH_Convert.ToBrep(obj, ref brep, GH_Conversion.Primary);
                            if (!mBrepList.Contains(brep))
                            {
                                mBrepList.Add(brep);
                            }
                        }
                        else if (obj is GH_Curve)
                        {
                            Curve curve = null;
                            GH_Convert.ToCurve(obj, ref curve, GH_Conversion.Primary);
                            if (!mCurveList.Contains(curve))
                            {
                                mCurveList.Add(curve);
                            }
                        }
                    }
                }

                var output_kratos_iga = new Cocodrilo.IO.OutputKratosIGA(this_analysis);
                output_kratos_iga.StartAnalysis(mBrepList, mCurveList, point_list);

                DA.SetData(1, output_kratos_iga);

                string project_path = Cocodrilo.UserData.UserDataUtilities.GetProjectPath(this_analysis.Name);
                DA.SetData(1, project_path);
            }
        }

        /// <summary>
        /// Resets the UserData of the geometries.
        ///
        /// Required once paths are removed to clear the obsulete memory.
        /// </summary>
        /// <param name="geometries_flat">list of 'Geometries' objects</param>
        private void ResetUserData(List<IGH_Goo> geometries_flat)
        {
            foreach (var obj in geometries_flat)
            {
                bool success = obj.CastTo(out Cocodrilo_GH.PreProcessing.Geometries.Geometries geoms);
                if (success)
                {
                    foreach (var brep in geoms.breps)
                    {
                        foreach (var surface in brep.Key?.Surfaces)
                        {
                            var ud = surface.UserData.Find(typeof(Cocodrilo.UserData.UserDataSurface)) as Cocodrilo.UserData.UserDataSurface;
                            ud?.DeleteNumericalElements();
                            if (ud != null)
                            {
                                ud.BrepId = -1;
                            }
                        }
                    }
                    foreach (var curve in geoms.curves)
                    {
                        var ud = curve.Key.UserData.Find(typeof(Cocodrilo.UserData.UserDataCurve)) as Cocodrilo.UserData.UserDataCurve;
                        ud?.DeleteNumericalElements();
                        if (ud != null)
                        {
                            ud.BrepId = -1;
                        }
                    }
                    foreach (var edge in geoms.edges)
                    {
                        var ud = edge.Key.UserData.Find(typeof(Cocodrilo.UserData.UserDataEdge)) as Cocodrilo.UserData.UserDataEdge;
                        ud?.DeleteNumericalElements();
                        if (ud != null)
                        {
                            ud.BrepId = -1;
                        }
                    }
                    foreach (var point in geoms.points)
                    {
                        var ud = point.Key.UserData.Find(typeof(Cocodrilo.UserData.UserDataPoint)) as Cocodrilo.UserData.UserDataPoint;
                        ud?.DeleteNumericalElements();
                        if (ud != null)
                        {
                            ud.BrepId = -1;
                        }
                    }
                }
            }
        }

        #region Menu Items
        protected override void AppendAdditionalComponentMenuItems(System.Windows.Forms.ToolStripDropDown menu)
        {
            var toolStripMenuItemCouplingMethod = GH_DocumentObject.Menu_AppendItem(menu, "Coupling Method");
            foreach (CouplingType pt in Enum.GetValues(typeof(CouplingType)))
                GH_Component.Menu_AppendItem(toolStripMenuItemCouplingMethod.DropDown, pt.ToString(), Menu_CouplingMethodChanged, true, pt == Cocodrilo.CocodriloPlugIn.Instance.GlobalCouplingMethod).Tag = pt;

            if (Cocodrilo.CocodriloPlugIn.Instance.GlobalCouplingMethod == Cocodrilo.ElementProperties.CouplingType.CouplingPenaltyCondition)
            {
                var toolStripMenuItemPenaltyFactor = GH_DocumentObject.Menu_AppendItem(menu, "Penalty Factor");
                var penaltyMenuItem = new Grasshopper.GUI.GH_MenuTextBox(toolStripMenuItemPenaltyFactor.DropDown, Convert.ToString(Cocodrilo.CocodriloPlugIn.Instance.GlobPenaltyFactor), true);
                penaltyMenuItem.KeyDown += new Grasshopper.GUI.GH_MenuTextBox.KeyDownEventHandler(Menu_SetPenaltyFactor);
            }

            var toolStripMenuItemPreProcessing = GH_DocumentObject.Menu_AppendItem(menu, "PreProcessing");
            Menu_AppendItem(toolStripMenuItemPreProcessing.DropDown, "ShowPreProcessing", Menu_DoClick_ShowPreProcessing, true, mShowPreProcessing);
            Menu_AppendItem(toolStripMenuItemPreProcessing.DropDown, "ShowElements", Menu_DoClick_ShowElements, mShowPreProcessing, mShowElements);
            Menu_AppendItem(toolStripMenuItemPreProcessing.DropDown, "ShowSupports", Menu_DoClick_ShowSupports, mShowPreProcessing, mShowSupports);
            Menu_AppendItem(toolStripMenuItemPreProcessing.DropDown, "ShowLoads", Menu_DoClick_ShowLoads, mShowPreProcessing, mShowLoads);
            Menu_AppendItem(toolStripMenuItemPreProcessing.DropDown, "ShowCouplings", Menu_DoClick_ShowCouplings, mShowPreProcessing, mShowCouplings);
            Menu_AppendItem(toolStripMenuItemPreProcessing.DropDown, "ShowIds", Menu_DoClick_ShowIds, mShowPreProcessing, mShowIds);
        }

        private void Menu_CouplingMethodChanged(object sender, EventArgs e)
        {
            if (sender is System.Windows.Forms.ToolStripMenuItem item && item.Tag is CouplingType)
            {
                Cocodrilo.CocodriloPlugIn.Instance.GlobalCouplingMethod = (CouplingType)item.Tag;
                item.Checked = true;
                ExpireSolution(true);
            }
        }
        private void Menu_DoClick_ShowPreProcessing(object sender, EventArgs e) => mShowPreProcessing = !mShowPreProcessing;
        private void Menu_DoClick_ShowElements(object sender, EventArgs e) => mShowElements = !mShowElements;
        private void Menu_DoClick_ShowSupports(object sender, EventArgs e) => mShowSupports = !mShowSupports;
        private void Menu_DoClick_ShowLoads(object sender, EventArgs e) => mShowLoads = !mShowLoads;
        private void Menu_DoClick_ShowCouplings(object sender, EventArgs e) => mShowCouplings = !mShowCouplings;
        private void Menu_DoClick_ShowIds(object sender, EventArgs e) => mShowIds = !mShowIds;
        private void Menu_SetPenaltyFactor(Grasshopper.GUI.GH_MenuTextBox sender, System.Windows.Forms.KeyEventArgs e)
        {
            System.Windows.Forms.Keys keyCode = e.KeyCode;
            if (keyCode == System.Windows.Forms.Keys.Return)
            {
                try
                {
                    Cocodrilo.CocodriloPlugIn.Instance.GlobPenaltyFactor = Convert.ToDouble(sender.Text);
                }
                catch (FormatException)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, "Unable to convert '" + sender.Text + "' to a double value.");
                }
                catch (OverflowException)
                {
                    AddRuntimeMessage(GH_RuntimeMessageLevel.Warning, sender.Text + " is outside the range of a double value.");
                }
            }
            else if (keyCode == System.Windows.Forms.Keys.Escape || keyCode == System.Windows.Forms.Keys.Cancel)
            {
            }
        }
        #endregion
        public override void DrawViewportWires(IGH_PreviewArgs args)
        {
            if (Hidden) return;
            if (Locked) return;

            if (mShowPreProcessing)
            {
                Cocodrilo.Visualizer.Visualizer.DrawElements(args.Display, mBrepList, mCurveList,
                    new List<Curve>(), new List<Point>(), mShowElements, mShowSupports, mShowLoads, mShowCouplings, mShowIds);
            }
            Rhino.RhinoDoc.ActiveDoc.Views.Redraw();
        }

        public override bool Write(GH_IO.Serialization.GH_IWriter writer)
        {
            writer.SetBoolean("ShowPreProcessing", mShowPreProcessing);
            writer.SetBoolean("ShowElements", mShowElements);
            writer.SetBoolean("ShowSupports", mShowSupports);
            writer.SetBoolean("ShowLoads", mShowLoads);
            writer.SetBoolean("ShowCouplings", mShowCouplings);
            writer.SetBoolean("ShowIds", mShowIds);
            return base.Write(writer);
        }

        public override bool Read(GH_IO.Serialization.GH_IReader reader)
        {
            reader.TryGetBoolean("ShowPreProcessing", ref mShowPreProcessing);
            reader.TryGetBoolean("ShowElements", ref mShowElements);
            reader.TryGetBoolean("ShowSupports", ref mShowSupports);
            reader.TryGetBoolean("ShowLoads", ref mShowLoads);
            reader.TryGetBoolean("ShowCouplings", ref mShowCouplings);
            reader.TryGetBoolean("ShowIds", ref mShowIds);
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
            get { return new Guid("696D1EA6-9806-455B-A345-4E47078D5C49"); }
        }
    }
}