using Rhino;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;
using Cocodrilo.UserData;

namespace Cocodrilo
{
    public partial class WindowAxis : Form
    {
        ObjRef tmp_curve = null;
        public WindowAxis()
        {
            InitializeComponent();

            //CocodriloPlugIn.Instance.materialUpdate += new MaterialChanged(updateAxisData);

            //comboBoxMaterials.DataSource = CocodriloPlugIn.Instance.Materials;
            ////comboBoxMaterials.DisplayMember = "Name";
            //comboBoxMaterials.ValueMember = "ID";
        }

        private void WindowAxis_Load(object sender, EventArgs e)
        {

        }

        private void buttonAddAxis_Click(object sender, EventArgs e)
        {
            try
            {
                //int MaterialID = Convert.ToInt32(textBoxMaterialID.Text);
                //string Name = textBoxMaterialName.Text;
                //double YoungsModulus = Convert.ToDouble(textBoxYoungsModulus.Text);
                //double Nue = Convert.ToDouble(textBoxNue.Text);

                //CocodriloPlugIn.Instance.AddMaterial(MaterialID, Name, YoungsModulus, Nue);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No axis added!");
            }
        }


        private void buttonDelete_Click(object sender, EventArgs e)
        {

        }

        private void buttonSelCurve_Click(object sender, EventArgs e)
        {
            var filter = ObjectType.Curve;
            ObjRef objref = null;
            var rc = RhinoGet.GetOneObject("Select Curve", false, filter, out objref);

            if (rc != Result.Success || objref == null)
                new Exception();

            //if (comboBoxCableType.Text.ToString() == "Edge")
            //{
            //    foreach (var trim in curve.Brep().Trims.Where(trim => trim.Edge?.EdgeIndex == curve.Brep().Edges[curve.GeometryComponentIndex.Index].EdgeIndex))
            //    {
            //        var ud = curve.Brep().Curves2D[trim.TrimIndex].UserData.Find(typeof(UserData.UserDataEdge)) as UserData.UserDataEdge;
            //        if (ud == null)
            //        {
            //            ud = new UserDataEdge();
            //            curve.Brep().Curves2D[trim.TrimIndex].UserData.Add(ud);
            //            RhinoApp.WriteLine("New Userdata Added");
            //        }
            //        ud.addCord(Prestress, Area, MaterialID);
            //    }
            //}
            //else if (comboBoxCableType.Text.ToString() == "Curve")
            //{
            var ud = objref.Curve().UserData.Find(typeof(UserData.UserDataCurve)) as UserData.UserDataCurve;                             //BA TBA 
            if (ud == null)
            {
                ud = new UserDataCurve();
                objref.Curve().UserData.Add(ud);
                RhinoApp.WriteLine("New Userdata Added");
            }
            else
            {
                List<double[]> base_vecs = ud.getBaseVecs();
                //dataGridViewAxis.SelectAll();
                //dataGridViewAxis.ClearSelection();
                int rows_n =  dataGridViewAxis.Rows.Count - 1;
                for (
                    int i = 0; i < rows_n; i++)
                    dataGridViewAxis.Rows.RemoveAt(0);
                dataGridViewAxis.DataSource = null;
                for (int i = 0; i<base_vecs.Count(); i++ )
                    this.dataGridViewAxis.Rows.Add(base_vecs[i][0], base_vecs[i][1], base_vecs[i][2], base_vecs[i][3]);
            }
            textBoxAxisCurveID.Text = Convert.ToString(objref.ObjectId);
            tmp_curve = objref;
            //}

        }

        private void buttonAxisAddSelect_Click(object sender, EventArgs e)
        {
            var filter = ObjectType.Curve;
            ObjRef[] objref = null;
            var rc = RhinoGet.GetMultipleObjects("Select Curve", false, filter, out objref);

            if (rc != Result.Success || objref == null)
                new Exception();

            //Rhino.DocObjects.ObjectEnumeratorSettings settings = new Rhino.DocObjects.ObjectEnumeratorSettings();
            //settings.NameFilter = textBoxAxisCurveID.Text;
            //System.Collections.Generic.List<Guid> ids = new System.Collections.Generic.List<Guid>();
            //Rhino.DocObjects.Tables.ObjectTable[]
            //foreach (Rhino.DocObjects.RhinoObject rhObj in doc.Objects.GetObjectList(settings))
            //    ids.Add(rhObj.Id);

            pictureBoxAxis.Visible = true;

            //Rhino.DocObjects.ObjRef beam_curve = new Rhino.DocObjects.ObjRef(ids[0]);
            ObjRef beam_curve = tmp_curve;
            var ud = beam_curve.Curve().UserData.Find(typeof(UserData.UserDataCurve)) as UserData.UserDataCurve;                             //BA TBA 
            if (ud == null)
            {
                ud = new UserDataCurve();
                beam_curve.Curve().UserData.Add(ud);
            }

            foreach (var curve in objref)
            {
                double ui=0, nxi=0, nyi=0, nzi=0;
                nxi = curve.Curve().PointAtEnd[0] - curve.Curve().PointAtStart[0];
                nyi = curve.Curve().PointAtEnd[1] - curve.Curve().PointAtStart[1];
                nzi = curve.Curve().PointAtEnd[2] - curve.Curve().PointAtStart[2];

                const double intersection_tolerance = 0.1;
                const double overlap_tolerance = 0.1;
                var events = Rhino.Geometry.Intersect.Intersection.CurveCurve(beam_curve.Curve(), curve.Curve(), intersection_tolerance, overlap_tolerance);
                // Process the results
                if (events != null)
                {
                    for (int i = 0; i < events.Count; i++)
                    {
                        var ccx_event = events[i];
                        //doc.Objects.AddPoint(ccx_event.PointA);
                        ui = ccx_event.ParameterA;
                    }
                    ud.addBaseVec(ui, nxi, nyi, nzi);
                    addBaseVecToDataGridViewAxis(ui, nxi, nyi, nzi);
                }
            }

        }

        private void dataGridViewAxis_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            //if (dataGridViewAxis[] == )
            {
                ObjRef beam_curve = tmp_curve;
                var ud = beam_curve.Curve().UserData.Find(typeof(UserData.UserDataCurve)) as UserData.UserDataCurve;                             //BA TBA 
                if (ud == null)
                {
                    ud = new UserDataCurve();
                    beam_curve.Curve().UserData.Add(ud);
                }
                int row = dataGridViewAxis.CurrentCell.RowIndex;
                int col = dataGridViewAxis.CurrentCell.ColumnIndex;
                if (Convert.ToString( dataGridViewAxis[0, row].Value) == "")
                {
                    dataGridViewAxis.CurrentCell.Value = null;
                }
                for (int i = 0; i < ud.base_vecs.Count; i++)
                {
                    if (ud.base_vecs[i][0] == Convert.ToDouble(this.dataGridViewAxis[0, row].Value))
                    {
                        ud.base_vecs[i][col] = Convert.ToDouble(this.dataGridViewAxis[col, row].Value);
                    }
                }
            }

        }
        private void addBaseVecToDataGridViewAxis(double _ui, double _nxi, double _nyi, double _nzi )
        {
            if (dataGridViewAxis.RowCount == 0)
            {
                //dataGridViewAxis.Rows.Add();
                dataGridViewAxis[0, 0].Value = _ui;
                dataGridViewAxis[1, 0].Value = _nxi;
                dataGridViewAxis[2, 0].Value = _nyi;
                dataGridViewAxis[3, 0].Value = _nzi;
            }
            if (dataGridViewAxis.RowCount == 1)
            {
                if (dataGridViewAxis[0, 0].Value == null)
                {
                    //dataGridViewAxis.Rows.Add();
                    dataGridViewAxis[0, 0].Value = _ui;
                    dataGridViewAxis[1, 0].Value = _nxi;
                    dataGridViewAxis[2, 0].Value = _nyi;
                    dataGridViewAxis[3, 0].Value = _nzi;
                }
                else
                {
                    if (Convert.ToDouble(dataGridViewAxis[0, 0].Value) < _ui)
                    {
                        dataGridViewAxis.Rows.Add();
                        dataGridViewAxis[0, 1].Value = _ui;
                        dataGridViewAxis[1, 1].Value = _nxi;
                        dataGridViewAxis[2, 1].Value = _nyi;
                        dataGridViewAxis[3, 1].Value = _nzi;
                    }
                    else if (Convert.ToDouble(dataGridViewAxis[0, 0].Value) > _ui)
                    {
                        dataGridViewAxis.Rows.Insert(0, 1);
                        dataGridViewAxis[0, 0].Value = _ui;
                        dataGridViewAxis[1, 0].Value = _nxi;
                        dataGridViewAxis[2, 0].Value = _nyi;
                        dataGridViewAxis[3, 0].Value = _nzi;
                    }
                }
            }
            else if (Convert.ToDouble(dataGridViewAxis[0, dataGridViewAxis.RowCount - 1].Value) < _ui)
            {
                dataGridViewAxis.Rows.Add();
                dataGridViewAxis[0, dataGridViewAxis.RowCount-1].Value = _ui;
                dataGridViewAxis[1, dataGridViewAxis.RowCount-1].Value = _nxi;
                dataGridViewAxis[2, dataGridViewAxis.RowCount-1].Value = _nyi;
                dataGridViewAxis[3, dataGridViewAxis.RowCount-1].Value = _nzi;
            }
            else
            {
                for (int i = 0; i < dataGridViewAxis.RowCount - 1; i++)
                {
                    if (Convert.ToDouble(dataGridViewAxis[0, i]) < _ui && Convert.ToDouble(dataGridViewAxis[0, i+1]) >= _ui)
                    {
                        dataGridViewAxis.Rows.Insert(i, 1);
                        dataGridViewAxis[0, i].Value = _ui;
                        dataGridViewAxis[1, i].Value = _nxi;
                        dataGridViewAxis[2, i].Value = _nyi;
                        dataGridViewAxis[3, i].Value = _nzi;
                    }

                    else if (Convert.ToDouble(dataGridViewAxis[0, i+1]) == _ui)
                    {
                        RhinoApp.WriteLine("U not allowed. Already defined");
                    }
                }
            }

        }

        private void buttonDeleteAxis_Click(object sender, EventArgs e)
        {
            ObjRef beam_curve = tmp_curve;
            var ud = beam_curve.Curve().UserData.Find(typeof(UserData.UserDataCurve)) as UserData.UserDataCurve;                             //BA TBA 
            if (ud == null)
            {

            }
            int sel_row = dataGridViewAxis.SelectedRows.Count;
            for (int i = 0; i < sel_row; i++)
            {
                //double u_tmp = Convert.ToDouble(dataGridViewAxis[dataGridViewAxis.SelectedRows[i].Index, 0]);
                ud.base_vecs.RemoveAt(dataGridViewAxis.SelectedRows[i].Index);
                dataGridViewAxis.Rows.RemoveAt(dataGridViewAxis.SelectedRows[i].Index);
            }
        }

        private void buttonAxisAddTable_Click(object sender, EventArgs e)
        {
            ObjRef beam_curve = tmp_curve;
            var ud = beam_curve.Curve().UserData.Find(typeof(UserDataCurve)) as UserDataCurve;                             
            if (ud == null)
            {
                ud = new UserDataCurve();
                beam_curve.Curve().UserData.Add(ud);
            }
            //check if value in respective text boxes
            //bool is_value = false;
            var curve = beam_curve.Curve().ToNurbsCurve();
            double u_tmp = Convert.ToDouble(textBoxAddTableU.Text);
            double nx_tmp = Convert.ToDouble(textBoxAddTableNx.Text);
            double ny_tmp = Convert.ToDouble(textBoxAddTableNy.Text);
            double nz_tmp = Convert.ToDouble(textBoxAddTableNz.Text);
            if (u_tmp < curve.Knots[0] || u_tmp > curve.Knots[curve.Knots.Count() - 1])
            {

            }
            else
            {
                ud.addBaseVec(u_tmp, nx_tmp, ny_tmp, nz_tmp);
                addBaseVecToDataGridViewAxis(u_tmp, nx_tmp, ny_tmp, nz_tmp);
            }
        }

        private void buttonAxisAddCopy_Click(object sender, EventArgs e)
        {
            ObjRef beam_curve = tmp_curve;
            var filter = ObjectType.Curve;
            ObjRef objref = null;
            var rc = RhinoGet.GetOneObject("Select Curve", false, filter, out objref);

            if (rc != Result.Success || objref == null)
                new Exception();

            var ud = objref.Curve().UserData.Find(typeof(UserData.UserDataCurve)) as UserData.UserDataCurve;                             //BA TBA 
            if (ud == null)
            {
                RhinoApp.WriteLine("No Userdata Found");
            }
            else
            {
                var crv_master = beam_curve.Curve().ToNurbsCurve();
                double u_min_m = crv_master.Knots[0];
                double u_max_m = crv_master.Knots[crv_master.Knots.Count() - 1];
                var crv = objref.Curve().ToNurbsCurve();
                double u_min = crv.Knots[0];
                double u_max = crv.Knots[crv.Knots.Count() - 1];

                List<double[]> base_vecs = ud.getBaseVecs();
                int rows_n = dataGridViewAxis.Rows.Count - 1;
                for ( int i = 0; i < rows_n; i++)
                    dataGridViewAxis.Rows.RemoveAt(0);
                dataGridViewAxis.DataSource = null;
                for (int i = 0; i < base_vecs.Count(); i++)
                    this.dataGridViewAxis.Rows.Add((base_vecs[i][0]-u_min)/(u_max - u_min)*(u_max_m - u_min_m)+ u_min_m, base_vecs[i][1], base_vecs[i][2], base_vecs[i][3]);
            }

        }
    }
}
