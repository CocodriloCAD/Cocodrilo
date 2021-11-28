using Rhino;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Cocodrilo
{
    public partial class WindowMaterial : Form
    {
        public WindowMaterial()
        {
            InitializeComponent();

            CocodriloPlugIn.Instance.materialUpdate += new MaterialChanged(updateMaterialData);

            comboBoxMaterials.DataSource = CocodriloPlugIn.Instance.Materials;
            //comboBoxMaterials.DisplayMember = "Name";
            comboBoxMaterials.ValueMember = "ID";
        }

        private void WindowMaterial_Load(object sender, EventArgs e)
        {

        }

        private void buttonAddMaterial_Click(object sender, EventArgs e)
        {
            try {
                int MaterialID = Convert.ToInt32(textBoxMaterialID.Text);
                string Name = textBoxMaterialName.Text;
                double YoungsModulus = Convert.ToDouble(textBoxYoungsModulus.Text);
                double Nue = Convert.ToDouble(textBoxNue.Text);
                double Alpha_T = Convert.ToDouble(textBoxMaterialAlphaT.Text);
                double Density = Convert.ToDouble(textBoxMaterialDensity.Text);

                CocodriloPlugIn.Instance.AddMaterial(MaterialID, Name, "LIN_ELAST_ISOTROPIC", YoungsModulus, Nue,Density,Alpha_T);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No material added!");
            }
        }

        public void updateMaterialData()
        {
            comboBoxMaterials.DataSource = null;
            comboBoxMaterials.DataSource = CocodriloPlugIn.Instance.Materials;
            //comboBoxMaterials.DisplayMember = "Name";
            comboBoxMaterials.ValueMember = "ID";
            comboBoxMaterials.DisplayMember = comboBoxMaterials.ValueMember;
        }
        private void comboBoxMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewMaterial();
        }
        private void ViewMaterial()
        {
            try
            {
                textBoxMaterialID.Text = CocodriloPlugIn.Instance.Materials[comboBoxMaterials.SelectedIndex].Id.ToString();
                //textBoxMaterialName.Text = CocodriloPlugIn.Instance.Materials[comboBoxMaterials.SelectedIndex].Type.ToString();
                //textBoxYoungsModulus.Text = CocodriloPlugIn.Instance.Materials[comboBoxMaterials.SelectedIndex].YoungsModulus.ToString();
                //textBoxNue.Text = CocodriloPlugIn.Instance.Materials[comboBoxMaterials.SelectedIndex].Nue.ToString();
                //textBoxMaterialAlphaT.Text = CocodriloPlugIn.Instance.Materials[comboBoxMaterials.SelectedIndex].AlphaT.ToString();
                //textBoxMaterialDensity.Text = CocodriloPlugIn.Instance.Materials[comboBoxMaterials.SelectedIndex].Density.ToString();
            }
            catch
            {
                RhinoApp.WriteLine("Error");
            }
        }

        private void buttonChange_Click(object sender, EventArgs e)
        {
            try {
                int MaterialID = Convert.ToInt32(textBoxMaterialID.Text);
                string Name = textBoxMaterialName.Text;
                double YoungsModulus = Convert.ToDouble(textBoxYoungsModulus.Text);
                double Nue = Convert.ToDouble(textBoxNue.Text);
                double Alpha_T = Convert.ToDouble(textBoxMaterialAlphaT.Text);
                double Density = Convert.ToDouble(textBoxMaterialDensity.Text);

                CocodriloPlugIn.Instance.ModifyMaterialWithMaterialID(
                    MaterialID, Name, "LIN_ELAST_ISOTROPIC", YoungsModulus, Nue, Density, Alpha_T);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: Change not possible!");
            }
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            try
            {
                int MaterialID = Convert.ToInt32(textBoxMaterialID.Text);

                if (!CocodriloPlugIn.Instance.DeleteMaterial(MaterialID))
                {
                    RhinoApp.WriteLine("WARNING: Material could not be deleted! Material Id was not found.");
                }
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: Material could not be deleted!");
            }
        }

        private void labelMaterialAlphaT_Click(object sender, EventArgs e)
        {

        }
    }
}
