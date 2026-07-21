using Rhino;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Eto.Forms;
using Eto.Drawing;
using Rhino.Commands;
using Rhino.DocObjects;
using Rhino.Input;
using Rhino.Input.Custom;
using Cocodrilo.UserData;

namespace Cocodrilo
{
    public class AxisRow
    {
        public double U { get; set; }
        public double Nx { get; set; }
        public double Ny { get; set; }
        public double Nz { get; set; }

        public double this[int column]
        {
            get
            {
                switch (column)
                {
                    case 0: return U;
                    case 1: return Nx;
                    case 2: return Ny;
                    default: return Nz;
                }
            }
            set
            {
                switch (column)
                {
                    case 0: U = value; break;
                    case 1: Nx = value; break;
                    case 2: Ny = value; break;
                    default: Nz = value; break;
                }
            }
        }
    }

    public partial class WindowAxis : Dialog
    {
        ObjRef tmp_curve = null;
        ObservableCollection<AxisRow> axisRows = new ObservableCollection<AxisRow>();
        GridView dataGridViewAxis;
        TextBox textBoxAxisCurveID;
        TextBox textBoxAddTableU;
        TextBox textBoxAddTableNx;
        TextBox textBoxAddTableNy;
        TextBox textBoxAddTableNz;
        ImageView pictureBoxAxis;

        public WindowAxis()
        {
            InitializeComponent();
        }

        void InitializeComponent()
        {
            Title = "Define Axis";
            ClientSize = new Size(470, 350);
            Resizable = true;

            dataGridViewAxis = new GridView
            {
                DataStore = axisRows,
                Columns =
                {
                    new GridColumn { HeaderText = "U", DataCell = new TextBoxCell(nameof(AxisRow.U)), Editable = true },
                    new GridColumn { HeaderText = "Nx", DataCell = new TextBoxCell(nameof(AxisRow.Nx)), Editable = true },
                    new GridColumn { HeaderText = "Ny", DataCell = new TextBoxCell(nameof(AxisRow.Ny)), Editable = true },
                    new GridColumn { HeaderText = "Nz", DataCell = new TextBoxCell(nameof(AxisRow.Nz)), Editable = true },
                }
            };
            dataGridViewAxis.CellEdited += dataGridViewAxis_CellEdited;

            var propertiesBoxAxis = new GroupBox
            {
                Text = "Axis",
                Content = dataGridViewAxis
            };

            var buttonAxisSelCurve = new Button { Text = "Select Curve" };
            buttonAxisSelCurve.Click += buttonSelCurve_Click;

            textBoxAxisCurveID = new TextBox();
            pictureBoxAxis = new ImageView { Visible = false, Size = new Size(25, 27) };
            try
            {
                pictureBoxAxis.Image = ToEtoBitmap(Cocodrilo.Properties.Resources.Check_small);
            }
            catch { /* icon is decorative only */ }

            var buttonAxisAddSelect = new Button { Text = "Add by Select" };
            buttonAxisAddSelect.Click += buttonAxisAddSelect_Click;
            var buttonAxisAddCopy = new Button { Text = "Copy" };
            buttonAxisAddCopy.Click += buttonAxisAddCopy_Click;
            var buttonDeleteAxis = new Button { Text = "Delete" };
            buttonDeleteAxis.Click += buttonDeleteAxis_Click;

            var buttonAxisAddTable = new Button { Text = "Add Table" };
            buttonAxisAddTable.Click += buttonAxisAddTable_Click;
            textBoxAddTableU = new TextBox();
            textBoxAddTableNx = new TextBox();
            textBoxAddTableNy = new TextBox();
            textBoxAddTableNz = new TextBox();

            Content = new TableLayout
            {
                Padding = 8,
                Spacing = new Size(6, 6),
                Rows =
                {
                    new TableRow(
                        new Label { Text = "Curve-ID", VerticalAlignment = VerticalAlignment.Center },
                        textBoxAxisCurveID, pictureBoxAxis, buttonAxisSelCurve),
                    new TableRow(propertiesBoxAxis) { ScaleHeight = true },
                    new TableRow(new StackLayout
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 6,
                        Items = { buttonAxisAddSelect, buttonAxisAddCopy, buttonDeleteAxis }
                    }),
                    new TableRow(new StackLayout
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 6,
                        Items = { buttonAxisAddTable, textBoxAddTableU, textBoxAddTableNx, textBoxAddTableNy, textBoxAddTableNz }
                    })
                }
            };
        }

        static Bitmap ToEtoBitmap(System.Drawing.Bitmap bmp)
        {
            using (var stream = new MemoryStream())
            {
                bmp.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;
                return new Bitmap(stream);
            }
        }

        void buttonSelCurve_Click(object sender, EventArgs e)
        {
            var filter = ObjectType.Curve;
            ObjRef objref = null;
            var rc = RhinoGet.GetOneObject("Select Curve", false, filter, out objref);

            if (rc != Result.Success || objref == null)
                new Exception();

            var ud = objref.Curve().UserData.Find(typeof(UserData.UserDataCurve)) as UserData.UserDataCurve;
            if (ud == null)
            {
                ud = new UserDataCurve();
                objref.Curve().UserData.Add(ud);
                RhinoApp.WriteLine("New Userdata Added");
            }
            else
            {
                List<double[]> base_vecs = ud.getBaseVecs();
                axisRows.Clear();
                for (int i = 0; i < base_vecs.Count(); i++)
                    axisRows.Add(new AxisRow { U = base_vecs[i][0], Nx = base_vecs[i][1], Ny = base_vecs[i][2], Nz = base_vecs[i][3] });
            }
            textBoxAxisCurveID.Text = Convert.ToString(objref.ObjectId);
            tmp_curve = objref;
        }

        void buttonAxisAddSelect_Click(object sender, EventArgs e)
        {
            var filter = ObjectType.Curve;
            ObjRef[] objref = null;
            var rc = RhinoGet.GetMultipleObjects("Select Curve", false, filter, out objref);

            if (rc != Result.Success || objref == null)
                new Exception();

            pictureBoxAxis.Visible = true;

            ObjRef beam_curve = tmp_curve;
            var ud = beam_curve.Curve().UserData.Find(typeof(UserData.UserDataCurve)) as UserData.UserDataCurve;
            if (ud == null)
            {
                ud = new UserDataCurve();
                beam_curve.Curve().UserData.Add(ud);
            }

            foreach (var curve in objref)
            {
                double ui = 0, nxi = 0, nyi = 0, nzi = 0;
                nxi = curve.Curve().PointAtEnd[0] - curve.Curve().PointAtStart[0];
                nyi = curve.Curve().PointAtEnd[1] - curve.Curve().PointAtStart[1];
                nzi = curve.Curve().PointAtEnd[2] - curve.Curve().PointAtStart[2];

                const double intersection_tolerance = 0.1;
                const double overlap_tolerance = 0.1;
                var events = Rhino.Geometry.Intersect.Intersection.CurveCurve(beam_curve.Curve(), curve.Curve(), intersection_tolerance, overlap_tolerance);
                if (events != null)
                {
                    for (int i = 0; i < events.Count; i++)
                    {
                        var ccx_event = events[i];
                        ui = ccx_event.ParameterA;
                    }
                    ud.addBaseVec(ui, nxi, nyi, nzi);
                    AddBaseVecToGrid(ui, nxi, nyi, nzi);
                }
            }
        }

        void dataGridViewAxis_CellEdited(object sender, GridViewCellEventArgs e)
        {
            var editedRow = e.Item as AxisRow;
            if (editedRow == null || tmp_curve == null)
                return;

            ObjRef beam_curve = tmp_curve;
            var ud = beam_curve.Curve().UserData.Find(typeof(UserData.UserDataCurve)) as UserData.UserDataCurve;
            if (ud == null)
            {
                ud = new UserDataCurve();
                beam_curve.Curve().UserData.Add(ud);
            }
            for (int i = 0; i < ud.base_vecs.Count; i++)
            {
                if (ud.base_vecs[i][0] == editedRow.U)
                {
                    ud.base_vecs[i][e.Column] = editedRow[e.Column];
                }
            }
        }

        void AddBaseVecToGrid(double _ui, double _nxi, double _nyi, double _nzi)
        {
            if (axisRows.Count == 0)
            {
                axisRows.Add(new AxisRow { U = _ui, Nx = _nxi, Ny = _nyi, Nz = _nzi });
            }
            else if (axisRows.Count == 1)
            {
                if (axisRows[0].U < _ui)
                {
                    axisRows.Add(new AxisRow { U = _ui, Nx = _nxi, Ny = _nyi, Nz = _nzi });
                }
                else if (axisRows[0].U > _ui)
                {
                    axisRows.Insert(0, new AxisRow { U = _ui, Nx = _nxi, Ny = _nyi, Nz = _nzi });
                }
            }
            else if (axisRows[axisRows.Count - 1].U < _ui)
            {
                axisRows.Add(new AxisRow { U = _ui, Nx = _nxi, Ny = _nyi, Nz = _nzi });
            }
            else
            {
                for (int i = 0; i < axisRows.Count - 1; i++)
                {
                    if (axisRows[i].U < _ui && axisRows[i + 1].U >= _ui)
                    {
                        axisRows.Insert(i, new AxisRow { U = _ui, Nx = _nxi, Ny = _nyi, Nz = _nzi });
                        break;
                    }
                    else if (axisRows[i + 1].U == _ui)
                    {
                        RhinoApp.WriteLine("U not allowed. Already defined");
                        break;
                    }
                }
            }
        }

        void buttonDeleteAxis_Click(object sender, EventArgs e)
        {
            ObjRef beam_curve = tmp_curve;
            var ud = beam_curve.Curve().UserData.Find(typeof(UserData.UserDataCurve)) as UserData.UserDataCurve;
            var selected = dataGridViewAxis.SelectedRows.OrderByDescending(i => i).ToList();
            foreach (var index in selected)
            {
                ud?.base_vecs.RemoveAt(index);
                axisRows.RemoveAt(index);
            }
        }

        void buttonAxisAddTable_Click(object sender, EventArgs e)
        {
            ObjRef beam_curve = tmp_curve;
            var ud = beam_curve.Curve().UserData.Find(typeof(UserDataCurve)) as UserDataCurve;
            if (ud == null)
            {
                ud = new UserDataCurve();
                beam_curve.Curve().UserData.Add(ud);
            }
            var curve = beam_curve.Curve().ToNurbsCurve();
            double u_tmp = Convert.ToDouble(textBoxAddTableU.Text);
            double nx_tmp = Convert.ToDouble(textBoxAddTableNx.Text);
            double ny_tmp = Convert.ToDouble(textBoxAddTableNy.Text);
            double nz_tmp = Convert.ToDouble(textBoxAddTableNz.Text);
            if (u_tmp < curve.Knots[0] || u_tmp > curve.Knots[curve.Knots.Count() - 1])
            {
                // out of range - ignored, matching original behavior
            }
            else
            {
                ud.addBaseVec(u_tmp, nx_tmp, ny_tmp, nz_tmp);
                AddBaseVecToGrid(u_tmp, nx_tmp, ny_tmp, nz_tmp);
            }
        }

        void buttonAxisAddCopy_Click(object sender, EventArgs e)
        {
            ObjRef beam_curve = tmp_curve;
            var filter = ObjectType.Curve;
            ObjRef objref = null;
            var rc = RhinoGet.GetOneObject("Select Curve", false, filter, out objref);

            if (rc != Result.Success || objref == null)
                new Exception();

            var ud = objref.Curve().UserData.Find(typeof(UserData.UserDataCurve)) as UserData.UserDataCurve;
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
                axisRows.Clear();
                for (int i = 0; i < base_vecs.Count(); i++)
                    axisRows.Add(new AxisRow
                    {
                        U = (base_vecs[i][0] - u_min) / (u_max - u_min) * (u_max_m - u_min_m) + u_min_m,
                        Nx = base_vecs[i][1],
                        Ny = base_vecs[i][2],
                        Nz = base_vecs[i][3]
                    });
            }
        }
    }
}
