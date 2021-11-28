using System;
using System.Windows.Forms;
using System.Collections.Generic;

namespace Cocodrilo
{
    public partial class WindowOutputOptions : Form
    {
        public WindowOutputOptions()
        {
            InitializeComponent();
        }

        private void WindowOutputOptions_Load(object sender, EventArgs e)
        {
            checkedListBoxOutputGeometry.SetItemChecked(
                0, CocodriloPlugIn.Instance.OutputOptions.elements);
            checkedListBoxOutputGeometry.SetItemChecked(
                1, CocodriloPlugIn.Instance.OutputOptions.conditions);

            //Results
            checkedListBoxOutputResults.SetItemChecked(
                0, CocodriloPlugIn.Instance.OutputOptions.displacements);
            checkedListBoxOutputResults.SetItemChecked(
                1, CocodriloPlugIn.Instance.OutputOptions.cauchy_stress);
            checkedListBoxOutputResults.SetItemChecked(
                2, CocodriloPlugIn.Instance.OutputOptions.pk2_stress);
            checkedListBoxOutputResults.SetItemChecked(
                3, CocodriloPlugIn.Instance.OutputOptions.moments);
            checkedListBoxOutputResults.SetItemChecked(
                4, CocodriloPlugIn.Instance.OutputOptions.damage);

            textBoxOutputPrecision.Text = CocodriloPlugIn.Instance.OutputOptions.precision.ToString();
            textBoxOutputFrequency.Text = CocodriloPlugIn.Instance.OutputOptions.output_frequency.ToString();
            textBoxOutputFileName.Text = CocodriloPlugIn.Instance.OutputOptions.output_file_name;
        }

        private void buttonOutputOptionsOK_Click(object sender, EventArgs e)
        {
            CocodriloPlugIn.Instance.OutputOptions.elements = checkedListBoxOutputGeometry.GetItemChecked(0);
            CocodriloPlugIn.Instance.OutputOptions.conditions = checkedListBoxOutputGeometry.GetItemChecked(1);

            CocodriloPlugIn.Instance.OutputOptions.displacements = checkedListBoxOutputResults.GetItemChecked(0);
            CocodriloPlugIn.Instance.OutputOptions.cauchy_stress = checkedListBoxOutputResults.GetItemChecked(1);
            CocodriloPlugIn.Instance.OutputOptions.pk2_stress = checkedListBoxOutputResults.GetItemChecked(2);
            CocodriloPlugIn.Instance.OutputOptions.moments = checkedListBoxOutputResults.GetItemChecked(3);
            CocodriloPlugIn.Instance.OutputOptions.damage = checkedListBoxOutputResults.GetItemChecked(4);

            CocodriloPlugIn.Instance.OutputOptions.precision = Convert.ToInt32(textBoxOutputPrecision.Text);
            CocodriloPlugIn.Instance.OutputOptions.output_frequency = Convert.ToDouble(textBoxOutputFrequency.Text);
            CocodriloPlugIn.Instance.OutputOptions.output_file_name = textBoxOutputFileName.Text;

            this.Close();
        }

        private void buttonOutputOptionsReset_Click(object sender, EventArgs e)
        {
            checkedListBoxOutputGeometry.SetItemChecked(0, true);
            checkedListBoxOutputGeometry.SetItemChecked(1, false);

            checkedListBoxOutputResults.SetItemChecked(0, true);
            checkedListBoxOutputResults.SetItemChecked(1, true);
            checkedListBoxOutputResults.SetItemChecked(2, true);
            checkedListBoxOutputResults.SetItemChecked(3, true);
            checkedListBoxOutputResults.SetItemChecked(4, true);

            textBoxOutputPrecision.Text = "14";
            textBoxOutputFrequency.Text = "0.1";
            textBoxOutputFileName.Text = "";
        }

        private void checkedListBoxOutputGeometry_SelectedIndexChanged(object sender, EventArgs e)
        {
        }
        private void WindowOutputOptions_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
        private void toolTip1_Popup(object sender, PopupEventArgs e)
        {
        }
    }
}
