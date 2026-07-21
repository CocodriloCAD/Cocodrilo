using Rhino;
using System;
using Eto.Forms;
using Eto.Drawing;
using Cocodrilo.Materials;

namespace Cocodrilo
{
    public partial class WindowMaterial : Dialog
    {
        TextBox textBoxMaterialID;
        TextBox textBoxMaterialName;
        TextBox textBoxYoungsModulus;
        TextBox textBoxNue;
        TextBox textBoxMaterialAlphaT;
        TextBox textBoxMaterialDensity;
        DropDown comboBoxMaterials;

        public WindowMaterial()
        {
            InitializeComponent();

            CocodriloPlugIn.Instance.materialUpdate += new MaterialChanged(updateMaterialData);

            updateMaterialData();
        }

        void InitializeComponent()
        {
            Title = "WindowMaterial";
            ClientSize = new Size(260, 230);
            Resizable = false;

            textBoxMaterialID = new TextBox();
            textBoxMaterialName = new TextBox();
            textBoxYoungsModulus = new TextBox();
            textBoxNue = new TextBox();
            textBoxMaterialAlphaT = new TextBox { Text = "0.0" };
            textBoxMaterialDensity = new TextBox { Text = "1.0" };

            var propertiesBox = new GroupBox
            {
                Text = "Material Properties:",
                Content = new TableLayout
                {
                    Padding = 4,
                    Spacing = new Size(4, 4),
                    Rows =
                    {
                        new TableRow(new Label { Text = "Material ID:" }, textBoxMaterialID,
                                     new Label { Text = "Material Name:" }, textBoxMaterialName),
                        new TableRow(new Label { Text = "Young's Modulus:" }, textBoxYoungsModulus,
                                     new Label { Text = "Nue:" }, textBoxNue),
                        new TableRow(new Label { Text = "Alpha T:" }, textBoxMaterialAlphaT,
                                     new Label { Text = "Density:" }, textBoxMaterialDensity)
                    }
                }
            };

            comboBoxMaterials = new DropDown
            {
                ItemTextBinding = Eto.Forms.Binding.Property<Material, string>(m => m.Id.ToString())
            };
            comboBoxMaterials.SelectedIndexChanged += comboBoxMaterials_SelectedIndexChanged;

            var buttonAddMaterial = new Button { Text = "Add Material" };
            buttonAddMaterial.Click += buttonAddMaterial_Click;
            var buttonChange = new Button { Text = "Change" };
            buttonChange.Click += buttonChange_Click;
            var buttonDelete = new Button { Text = "Delete" };
            buttonDelete.Click += buttonDelete_Click;

            Content = new TableLayout
            {
                Padding = 8,
                Spacing = new Size(6, 6),
                Rows =
                {
                    propertiesBox,
                    comboBoxMaterials,
                    new TableRow(new StackLayout
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 6,
                        Items = { buttonDelete, buttonChange, buttonAddMaterial }
                    })
                }
            };
        }

        void buttonAddMaterial_Click(object sender, EventArgs e)
        {
            try
            {
                int MaterialID = Convert.ToInt32(textBoxMaterialID.Text);
                string Name = textBoxMaterialName.Text;
                double YoungsModulus = Convert.ToDouble(textBoxYoungsModulus.Text);
                double Nue = Convert.ToDouble(textBoxNue.Text);
                double Alpha_T = Convert.ToDouble(textBoxMaterialAlphaT.Text);
                double Density = Convert.ToDouble(textBoxMaterialDensity.Text);

                CocodriloPlugIn.Instance.AddMaterial(MaterialID, Name, "LIN_ELAST_ISOTROPIC", YoungsModulus, Nue, Density, Alpha_T);
            }
            catch
            {
                RhinoApp.WriteLine("WARNING: No material added!");
            }
        }

        public void updateMaterialData()
        {
            comboBoxMaterials.DataStore = CocodriloPlugIn.Instance.Materials;
        }

        void comboBoxMaterials_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewMaterial();
        }

        void ViewMaterial()
        {
            try
            {
                textBoxMaterialID.Text = CocodriloPlugIn.Instance.Materials[comboBoxMaterials.SelectedIndex].Id.ToString();
            }
            catch
            {
                RhinoApp.WriteLine("Error");
            }
        }

        void buttonChange_Click(object sender, EventArgs e)
        {
            try
            {
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

        void buttonDelete_Click(object sender, EventArgs e)
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
    }
}
