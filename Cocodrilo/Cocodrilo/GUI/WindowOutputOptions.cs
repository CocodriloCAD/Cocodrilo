using System;
using Eto.Forms;
using Eto.Drawing;

namespace Cocodrilo
{
    public partial class WindowOutputOptions : Dialog
    {
        CheckBox checkBoxElements;
        CheckBox checkBoxConditions;
        CheckBox checkBoxDisplacements;
        CheckBox checkBoxCauchyStress;
        CheckBox checkBoxPk2Stress;
        CheckBox checkBoxMoments;
        CheckBox checkBoxDamage;
        TextBox textBoxOutputFileName;
        TextBox textBoxOutputFrequency;
        TextBox textBoxOutputPrecision;

        public WindowOutputOptions()
        {
            InitializeComponent();
            WindowOutputOptions_Load(this, EventArgs.Empty);
        }

        void InitializeComponent()
        {
            Title = "Output Options";
            ClientSize = new Size(260, 420);
            Resizable = false;

            checkBoxElements = new CheckBox { Text = "Elements" };
            checkBoxConditions = new CheckBox { Text = "Conditions" };
            var groupBoxGeometry = new GroupBox
            {
                Text = "Geometry",
                Content = new StackLayout
                {
                    Orientation = Orientation.Vertical,
                    Spacing = 4,
                    Padding = 6,
                    Items = { checkBoxElements, checkBoxConditions }
                }
            };

            checkBoxDisplacements = new CheckBox { Text = "Displacements" };
            checkBoxCauchyStress = new CheckBox { Text = "Cauchy Stress" };
            checkBoxPk2Stress = new CheckBox { Text = "PK2 Stress" };
            checkBoxMoments = new CheckBox { Text = "Moments" };
            checkBoxDamage = new CheckBox { Text = "Damage" };
            var groupBoxResults = new GroupBox
            {
                Text = "Results",
                Content = new StackLayout
                {
                    Orientation = Orientation.Vertical,
                    Spacing = 4,
                    Padding = 6,
                    Items =
                    {
                        checkBoxDisplacements, checkBoxCauchyStress, checkBoxPk2Stress,
                        checkBoxMoments, checkBoxDamage
                    }
                }
            };

            textBoxOutputPrecision = new TextBox { Text = "14" };
            textBoxOutputFrequency = new TextBox { Text = "1" };
            textBoxOutputFileName = new TextBox();

            var buttonOK = new Button { Text = "OK" };
            buttonOK.Click += buttonOutputOptionsOK_Click;
            var buttonReset = new Button { Text = "Reset" };
            buttonReset.Click += buttonOutputOptionsReset_Click;

            var layout = new TableLayout
            {
                Padding = 8,
                Spacing = new Size(6, 6),
                Rows =
                {
                    groupBoxGeometry,
                    groupBoxResults,
                    new TableRow(new Label { Text = "Precision" }, textBoxOutputPrecision),
                    new TableRow(new Label { Text = "Output Frequency" }, textBoxOutputFrequency),
                    new TableRow(new Label { Text = "Output File Name" }, textBoxOutputFileName),
                    new TableRow(new StackLayout
                    {
                        Orientation = Orientation.Horizontal,
                        Spacing = 6,
                        Items = { buttonOK, buttonReset }
                    })
                }
            };

            Content = layout;
        }

        void WindowOutputOptions_Load(object sender, EventArgs e)
        {
            checkBoxElements.Checked = CocodriloPlugIn.Instance.OutputOptions.elements;
            checkBoxConditions.Checked = CocodriloPlugIn.Instance.OutputOptions.conditions;

            checkBoxDisplacements.Checked = CocodriloPlugIn.Instance.OutputOptions.displacements;
            checkBoxCauchyStress.Checked = CocodriloPlugIn.Instance.OutputOptions.cauchy_stress;
            checkBoxPk2Stress.Checked = CocodriloPlugIn.Instance.OutputOptions.pk2_stress;
            checkBoxMoments.Checked = CocodriloPlugIn.Instance.OutputOptions.moments;
            checkBoxDamage.Checked = CocodriloPlugIn.Instance.OutputOptions.damage;

            textBoxOutputPrecision.Text = CocodriloPlugIn.Instance.OutputOptions.precision.ToString();
            textBoxOutputFrequency.Text = CocodriloPlugIn.Instance.OutputOptions.output_frequency.ToString();
            textBoxOutputFileName.Text = CocodriloPlugIn.Instance.OutputOptions.output_file_name;
        }

        void buttonOutputOptionsOK_Click(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.OutputOptions.elements = checkBoxElements.Checked == true;
            CocodriloPlugIn.Instance.OutputOptions.conditions = checkBoxConditions.Checked == true;

            CocodriloPlugIn.Instance.OutputOptions.displacements = checkBoxDisplacements.Checked == true;
            CocodriloPlugIn.Instance.OutputOptions.cauchy_stress = checkBoxCauchyStress.Checked == true;
            CocodriloPlugIn.Instance.OutputOptions.pk2_stress = checkBoxPk2Stress.Checked == true;
            CocodriloPlugIn.Instance.OutputOptions.moments = checkBoxMoments.Checked == true;
            CocodriloPlugIn.Instance.OutputOptions.damage = checkBoxDamage.Checked == true;

            CocodriloPlugIn.Instance.OutputOptions.precision = Convert.ToInt32(textBoxOutputPrecision.Text);
            CocodriloPlugIn.Instance.OutputOptions.output_frequency = Convert.ToDouble(textBoxOutputFrequency.Text);
            CocodriloPlugIn.Instance.OutputOptions.output_file_name = textBoxOutputFileName.Text;

            Close();
        }

        void buttonOutputOptionsReset_Click(object sender, EventArgs e)
        {
            checkBoxElements.Checked = true;
            checkBoxConditions.Checked = false;

            checkBoxDisplacements.Checked = true;
            checkBoxCauchyStress.Checked = true;
            checkBoxPk2Stress.Checked = true;
            checkBoxMoments.Checked = true;
            checkBoxDamage.Checked = true;

            textBoxOutputPrecision.Text = "14";
            textBoxOutputFrequency.Text = "0.1";
            textBoxOutputFileName.Text = "";
        }
    }
}
