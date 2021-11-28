namespace Cocodrilo
{
    partial class WindowOutputOptions
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.groupBoxOutputGeometry = new System.Windows.Forms.GroupBox();
            this.checkedListBoxOutputGeometry = new System.Windows.Forms.CheckedListBox();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.checkedListBoxOutputResults = new System.Windows.Forms.CheckedListBox();
            this.buttonOutputOptionsOK = new System.Windows.Forms.Button();
            this.buttonOutputOptionsReset = new System.Windows.Forms.Button();
            this.groupBoxOutputResults = new System.Windows.Forms.GroupBox();
            this.labelOutputFileName = new System.Windows.Forms.Label();
            this.textBoxOutputFileName = new System.Windows.Forms.TextBox();
            this.labelOutputFrequency = new System.Windows.Forms.Label();
            this.textBoxOutputFrequency = new System.Windows.Forms.TextBox();
            this.labelOutputPrecision = new System.Windows.Forms.Label();
            this.textBoxOutputPrecision = new System.Windows.Forms.TextBox();
            this.toolTipOutputOptions = new System.Windows.Forms.ToolTip(this.components);
            this.groupBoxOutputGeometry.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            this.groupBoxOutputResults.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBoxOutputGeometry
            // 
            this.groupBoxOutputGeometry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOutputGeometry.Controls.Add(this.checkedListBoxOutputGeometry);
            this.groupBoxOutputGeometry.Location = new System.Drawing.Point(6, 10);
            this.groupBoxOutputGeometry.Margin = new System.Windows.Forms.Padding(2);
            this.groupBoxOutputGeometry.Name = "groupBoxOutputGeometry";
            this.groupBoxOutputGeometry.Padding = new System.Windows.Forms.Padding(2);
            this.groupBoxOutputGeometry.Size = new System.Drawing.Size(236, 54);
            this.groupBoxOutputGeometry.TabIndex = 1;
            this.groupBoxOutputGeometry.TabStop = false;
            this.groupBoxOutputGeometry.Text = "Geometry";
            // 
            // checkedListBoxOutputGeometry
            // 
            this.checkedListBoxOutputGeometry.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxOutputGeometry.CheckOnClick = true;
            this.checkedListBoxOutputGeometry.FormattingEnabled = true;
            this.checkedListBoxOutputGeometry.Items.AddRange(new object[] {
            "Elements",
            "Conditions"});
            this.checkedListBoxOutputGeometry.Location = new System.Drawing.Point(5, 15);
            this.checkedListBoxOutputGeometry.Name = "checkedListBoxOutputGeometry";
            this.checkedListBoxOutputGeometry.Size = new System.Drawing.Size(226, 34);
            this.checkedListBoxOutputGeometry.TabIndex = 0;
            this.checkedListBoxOutputGeometry.SelectedIndexChanged += new System.EventHandler(this.checkedListBoxOutputGeometry_SelectedIndexChanged);
            // 
            // checkedListBoxOutputResults
            // 
            this.checkedListBoxOutputResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.checkedListBoxOutputResults.CheckOnClick = true;
            this.checkedListBoxOutputResults.FormattingEnabled = true;
            this.checkedListBoxOutputResults.Items.AddRange(new object[] {
            "Displacements",
            "Cauchy Stress",
            "PK2 Stress",
            "Moments",
            "Damage"});
            this.checkedListBoxOutputResults.Location = new System.Drawing.Point(6, 19);
            this.checkedListBoxOutputResults.Name = "checkedListBoxOutputResults";
            this.checkedListBoxOutputResults.Size = new System.Drawing.Size(225, 139);
            this.checkedListBoxOutputResults.TabIndex = 0;
            // 
            // buttonOutputOptionsOK
            // 
            this.buttonOutputOptionsOK.Location = new System.Drawing.Point(6, 386);
            this.buttonOutputOptionsOK.Name = "buttonOutputOptionsOK";
            this.buttonOutputOptionsOK.Size = new System.Drawing.Size(90, 23);
            this.buttonOutputOptionsOK.TabIndex = 2;
            this.buttonOutputOptionsOK.Text = "OK";
            this.toolTipOutputOptions.SetToolTip(this.buttonOutputOptionsOK, "saves the options and closes the window");
            this.buttonOutputOptionsOK.UseVisualStyleBackColor = true;
            this.buttonOutputOptionsOK.Click += new System.EventHandler(this.buttonOutputOptionsOK_Click);
            // 
            // buttonOutputOptionsReset
            // 
            this.buttonOutputOptionsReset.Location = new System.Drawing.Point(102, 386);
            this.buttonOutputOptionsReset.Name = "buttonOutputOptionsReset";
            this.buttonOutputOptionsReset.Size = new System.Drawing.Size(90, 23);
            this.buttonOutputOptionsReset.TabIndex = 3;
            this.buttonOutputOptionsReset.Text = "Reset";
            this.toolTipOutputOptions.SetToolTip(this.buttonOutputOptionsReset, "reset to default values");
            this.buttonOutputOptionsReset.UseVisualStyleBackColor = true;
            this.buttonOutputOptionsReset.Click += new System.EventHandler(this.buttonOutputOptionsReset_Click);
            // 
            // groupBoxOutputResults
            // 
            this.groupBoxOutputResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBoxOutputResults.Controls.Add(this.checkedListBoxOutputResults);
            this.groupBoxOutputResults.Location = new System.Drawing.Point(6, 69);
            this.groupBoxOutputResults.Name = "groupBoxOutputResults";
            this.groupBoxOutputResults.Size = new System.Drawing.Size(236, 172);
            this.groupBoxOutputResults.TabIndex = 4;
            this.groupBoxOutputResults.TabStop = false;
            this.groupBoxOutputResults.Text = "Results";
            // 
            // labelOutputFileName
            // 
            this.labelOutputFileName.AutoSize = true;
            this.labelOutputFileName.Location = new System.Drawing.Point(3, 363);
            this.labelOutputFileName.Name = "labelOutputFileName";
            this.labelOutputFileName.Size = new System.Drawing.Size(89, 13);
            this.labelOutputFileName.TabIndex = 5;
            this.labelOutputFileName.Text = "Output File Name";
            // 
            // textBoxOutputFileName
            // 
            this.textBoxOutputFileName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxOutputFileName.Location = new System.Drawing.Point(102, 360);
            this.textBoxOutputFileName.Name = "textBoxOutputFileName";
            this.textBoxOutputFileName.Size = new System.Drawing.Size(135, 20);
            this.textBoxOutputFileName.TabIndex = 6;
            // 
            // labelOutputFrequency
            // 
            this.labelOutputFrequency.AutoSize = true;
            this.labelOutputFrequency.Location = new System.Drawing.Point(3, 337);
            this.labelOutputFrequency.Name = "labelOutputFrequency";
            this.labelOutputFrequency.Size = new System.Drawing.Size(92, 13);
            this.labelOutputFrequency.TabIndex = 7;
            this.labelOutputFrequency.Text = "Output Frequency";
            // 
            // textBoxOutputFrequency
            // 
            this.textBoxOutputFrequency.Location = new System.Drawing.Point(102, 334);
            this.textBoxOutputFrequency.Name = "textBoxOutputFrequency";
            this.textBoxOutputFrequency.Size = new System.Drawing.Size(135, 20);
            this.textBoxOutputFrequency.TabIndex = 8;
            this.textBoxOutputFrequency.Text = "1";
            // 
            // labelOutputPrecision
            // 
            this.labelOutputPrecision.AutoSize = true;
            this.labelOutputPrecision.Location = new System.Drawing.Point(3, 311);
            this.labelOutputPrecision.Name = "labelOutputPrecision";
            this.labelOutputPrecision.Size = new System.Drawing.Size(50, 13);
            this.labelOutputPrecision.TabIndex = 9;
            this.labelOutputPrecision.Text = "Precision";
            // 
            // textBoxOutputPrecision
            // 
            this.textBoxOutputPrecision.Location = new System.Drawing.Point(102, 308);
            this.textBoxOutputPrecision.Name = "textBoxOutputPrecision";
            this.textBoxOutputPrecision.Size = new System.Drawing.Size(135, 20);
            this.textBoxOutputPrecision.TabIndex = 10;
            this.textBoxOutputPrecision.Text = "14";
            // 
            // toolTipOutputOptions
            // 
            this.toolTipOutputOptions.Popup += new System.Windows.Forms.PopupEventHandler(this.toolTip1_Popup);
            // 
            // WindowOutputOptions
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 414);
            this.Controls.Add(this.textBoxOutputPrecision);
            this.Controls.Add(this.labelOutputPrecision);
            this.Controls.Add(this.textBoxOutputFrequency);
            this.Controls.Add(this.labelOutputFrequency);
            this.Controls.Add(this.textBoxOutputFileName);
            this.Controls.Add(this.labelOutputFileName);
            this.Controls.Add(this.groupBoxOutputResults);
            this.Controls.Add(this.buttonOutputOptionsReset);
            this.Controls.Add(this.buttonOutputOptionsOK);
            this.Controls.Add(this.groupBoxOutputGeometry);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "WindowOutputOptions";
            this.Text = "Output Options";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.WindowOutputOptions_FormClosing);
            this.Load += new System.EventHandler(this.WindowOutputOptions_Load);
            this.groupBoxOutputGeometry.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            this.groupBoxOutputResults.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBoxOutputGeometry;
        private System.Windows.Forms.BindingSource bs;
        private System.Windows.Forms.CheckedListBox checkedListBoxOutputResults;
        private System.Windows.Forms.Button buttonOutputOptionsOK;
        private System.Windows.Forms.Button buttonOutputOptionsReset;
        private System.Windows.Forms.GroupBox groupBoxOutputResults;
        private System.Windows.Forms.CheckedListBox checkedListBoxOutputGeometry;
        private System.Windows.Forms.Label labelOutputFileName;
        private System.Windows.Forms.TextBox textBoxOutputFileName;
        private System.Windows.Forms.Label labelOutputFrequency;
        private System.Windows.Forms.TextBox textBoxOutputFrequency;
        private System.Windows.Forms.Label labelOutputPrecision;
        private System.Windows.Forms.TextBox textBoxOutputPrecision;
        private System.Windows.Forms.ToolTip toolTipOutputOptions;
    }
}