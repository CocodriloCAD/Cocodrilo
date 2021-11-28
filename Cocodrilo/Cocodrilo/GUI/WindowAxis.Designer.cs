namespace Cocodrilo
{
    partial class WindowAxis
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(WindowAxis));
            this.PropertiesBoxAxis = new System.Windows.Forms.GroupBox();
            this.dataGridViewAxis = new System.Windows.Forms.DataGridView();
            this.GridViewAxisU = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GridViewAxisNx = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GridViewAxisNy = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.GridViewAxisNz = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.buttonAxisAddSelect = new System.Windows.Forms.Button();
            this.buttonDeleteAxis = new System.Windows.Forms.Button();
            this.buttonAxisSelCurve = new System.Windows.Forms.Button();
            this.pictureBoxAxis = new System.Windows.Forms.PictureBox();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.textBoxAxisCurveID = new System.Windows.Forms.TextBox();
            this.labelAxisCurveID = new System.Windows.Forms.Label();
            this.buttonAxisAddTable = new System.Windows.Forms.Button();
            this.textBoxAddTableU = new System.Windows.Forms.TextBox();
            this.textBoxAddTableNx = new System.Windows.Forms.TextBox();
            this.textBoxAddTableNy = new System.Windows.Forms.TextBox();
            this.textBoxAddTableNz = new System.Windows.Forms.TextBox();
            this.buttonAxisAddCopy = new System.Windows.Forms.Button();
            this.PropertiesBoxAxis.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAxis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAxis)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            this.SuspendLayout();
            // 
            // PropertiesBoxAxis
            // 
            this.PropertiesBoxAxis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.PropertiesBoxAxis.Controls.Add(this.dataGridViewAxis);
            this.PropertiesBoxAxis.Location = new System.Drawing.Point(9, 52);
            this.PropertiesBoxAxis.Margin = new System.Windows.Forms.Padding(2);
            this.PropertiesBoxAxis.Name = "PropertiesBoxAxis";
            this.PropertiesBoxAxis.Padding = new System.Windows.Forms.Padding(2);
            this.PropertiesBoxAxis.Size = new System.Drawing.Size(443, 217);
            this.PropertiesBoxAxis.TabIndex = 1;
            this.PropertiesBoxAxis.TabStop = false;
            this.PropertiesBoxAxis.Text = "Axis";
            // 
            // dataGridViewAxis
            // 
            this.dataGridViewAxis.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewAxis.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewAxis.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.GridViewAxisU,
            this.GridViewAxisNx,
            this.GridViewAxisNy,
            this.GridViewAxisNz});
            this.dataGridViewAxis.Location = new System.Drawing.Point(0, 18);
            this.dataGridViewAxis.Name = "dataGridViewAxis";
            this.dataGridViewAxis.Size = new System.Drawing.Size(443, 194);
            this.dataGridViewAxis.TabIndex = 6;
            this.dataGridViewAxis.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewAxis_CellEndEdit);
            // 
            // GridViewAxisU
            // 
            this.GridViewAxisU.HeaderText = "U";
            this.GridViewAxisU.Name = "GridViewAxisU";
            // 
            // GridViewAxisNx
            // 
            this.GridViewAxisNx.HeaderText = "Nx";
            this.GridViewAxisNx.Name = "GridViewAxisNx";
            // 
            // GridViewAxisNy
            // 
            this.GridViewAxisNy.HeaderText = "Ny";
            this.GridViewAxisNy.Name = "GridViewAxisNy";
            // 
            // GridViewAxisNz
            // 
            this.GridViewAxisNz.HeaderText = "Nz";
            this.GridViewAxisNz.Name = "GridViewAxisNz";
            // 
            // buttonAxisAddSelect
            // 
            this.buttonAxisAddSelect.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonAxisAddSelect.Location = new System.Drawing.Point(9, 276);
            this.buttonAxisAddSelect.Name = "buttonAxisAddSelect";
            this.buttonAxisAddSelect.Size = new System.Drawing.Size(85, 27);
            this.buttonAxisAddSelect.TabIndex = 2;
            this.buttonAxisAddSelect.Text = "Add by Select";
            this.buttonAxisAddSelect.UseVisualStyleBackColor = true;
            this.buttonAxisAddSelect.Click += new System.EventHandler(this.buttonAxisAddSelect_Click);
            // 
            // buttonDeleteAxis
            // 
            this.buttonDeleteAxis.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.buttonDeleteAxis.Location = new System.Drawing.Point(378, 276);
            this.buttonDeleteAxis.Name = "buttonDeleteAxis";
            this.buttonDeleteAxis.Size = new System.Drawing.Size(71, 27);
            this.buttonDeleteAxis.TabIndex = 4;
            this.buttonDeleteAxis.Text = "Delete";
            this.buttonDeleteAxis.UseVisualStyleBackColor = true;
            this.buttonDeleteAxis.Click += new System.EventHandler(this.buttonDeleteAxis_Click);
            // 
            // buttonAxisSelCurve
            // 
            this.buttonAxisSelCurve.Location = new System.Drawing.Point(177, 9);
            this.buttonAxisSelCurve.Name = "buttonAxisSelCurve";
            this.buttonAxisSelCurve.Size = new System.Drawing.Size(125, 27);
            this.buttonAxisSelCurve.TabIndex = 5;
            this.buttonAxisSelCurve.Text = "Select Curve";
            this.buttonAxisSelCurve.UseVisualStyleBackColor = true;
            this.buttonAxisSelCurve.Click += new System.EventHandler(this.buttonSelCurve_Click);
            // 
            // pictureBoxAxis
            // 
            this.pictureBoxAxis.Image = global::Cocodrilo.Properties.Resources.Check_small;
            this.pictureBoxAxis.InitialImage = ((System.Drawing.Image)(resources.GetObject("pictureBoxAxis.InitialImage")));
            this.pictureBoxAxis.Location = new System.Drawing.Point(146, 9);
            this.pictureBoxAxis.Name = "pictureBoxAxis";
            this.pictureBoxAxis.Size = new System.Drawing.Size(25, 27);
            this.pictureBoxAxis.TabIndex = 6;
            this.pictureBoxAxis.TabStop = false;
            this.pictureBoxAxis.Visible = false;
            // 
            // textBoxAxisCurveID
            // 
            this.textBoxAxisCurveID.Location = new System.Drawing.Point(67, 13);
            this.textBoxAxisCurveID.Name = "textBoxAxisCurveID";
            this.textBoxAxisCurveID.Size = new System.Drawing.Size(73, 20);
            this.textBoxAxisCurveID.TabIndex = 7;
            // 
            // labelAxisCurveID
            // 
            this.labelAxisCurveID.AutoSize = true;
            this.labelAxisCurveID.Location = new System.Drawing.Point(12, 16);
            this.labelAxisCurveID.Name = "labelAxisCurveID";
            this.labelAxisCurveID.Size = new System.Drawing.Size(49, 13);
            this.labelAxisCurveID.TabIndex = 8;
            this.labelAxisCurveID.Text = "Curve-ID";
            // 
            // buttonAxisAddTable
            // 
            this.buttonAxisAddTable.Location = new System.Drawing.Point(9, 309);
            this.buttonAxisAddTable.Name = "buttonAxisAddTable";
            this.buttonAxisAddTable.Size = new System.Drawing.Size(85, 27);
            this.buttonAxisAddTable.TabIndex = 9;
            this.buttonAxisAddTable.Text = "Add Table";
            this.buttonAxisAddTable.UseVisualStyleBackColor = true;
            this.buttonAxisAddTable.Click += new System.EventHandler(this.buttonAxisAddTable_Click);
            // 
            // textBoxAddTableU
            // 
            this.textBoxAddTableU.Location = new System.Drawing.Point(100, 313);
            this.textBoxAddTableU.Name = "textBoxAddTableU";
            this.textBoxAddTableU.Size = new System.Drawing.Size(79, 20);
            this.textBoxAddTableU.TabIndex = 10;
            // 
            // textBoxAddTableNx
            // 
            this.textBoxAddTableNx.Location = new System.Drawing.Point(185, 313);
            this.textBoxAddTableNx.Name = "textBoxAddTableNx";
            this.textBoxAddTableNx.Size = new System.Drawing.Size(84, 20);
            this.textBoxAddTableNx.TabIndex = 11;
            // 
            // textBoxAddTableNy
            // 
            this.textBoxAddTableNy.Location = new System.Drawing.Point(275, 313);
            this.textBoxAddTableNy.Name = "textBoxAddTableNy";
            this.textBoxAddTableNy.Size = new System.Drawing.Size(84, 20);
            this.textBoxAddTableNy.TabIndex = 12;
            // 
            // textBoxAddTableNz
            // 
            this.textBoxAddTableNz.Location = new System.Drawing.Point(365, 313);
            this.textBoxAddTableNz.Name = "textBoxAddTableNz";
            this.textBoxAddTableNz.Size = new System.Drawing.Size(84, 20);
            this.textBoxAddTableNz.TabIndex = 13;
            // 
            // buttonAxisAddCopy
            // 
            this.buttonAxisAddCopy.Location = new System.Drawing.Point(100, 276);
            this.buttonAxisAddCopy.Name = "buttonAxisAddCopy";
            this.buttonAxisAddCopy.Size = new System.Drawing.Size(85, 27);
            this.buttonAxisAddCopy.TabIndex = 14;
            this.buttonAxisAddCopy.Text = "Copy";
            this.buttonAxisAddCopy.UseVisualStyleBackColor = true;
            this.buttonAxisAddCopy.Click += new System.EventHandler(this.buttonAxisAddCopy_Click);
            // 
            // WindowAxis
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 341);
            this.Controls.Add(this.buttonAxisAddCopy);
            this.Controls.Add(this.textBoxAddTableNz);
            this.Controls.Add(this.textBoxAddTableNy);
            this.Controls.Add(this.textBoxAddTableNx);
            this.Controls.Add(this.textBoxAddTableU);
            this.Controls.Add(this.buttonAxisAddTable);
            this.Controls.Add(this.labelAxisCurveID);
            this.Controls.Add(this.textBoxAxisCurveID);
            this.Controls.Add(this.pictureBoxAxis);
            this.Controls.Add(this.buttonAxisSelCurve);
            this.Controls.Add(this.buttonDeleteAxis);
            this.Controls.Add(this.buttonAxisAddSelect);
            this.Controls.Add(this.PropertiesBoxAxis);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "WindowAxis";
            this.Text = "Define Axis";
            this.Load += new System.EventHandler(this.WindowAxis_Load);
            this.PropertiesBoxAxis.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewAxis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxAxis)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.GroupBox PropertiesBoxAxis;
        private System.Windows.Forms.BindingSource bs;
        private System.Windows.Forms.Button buttonAxisAddSelect;
        private System.Windows.Forms.Button buttonDeleteAxis;
        private System.Windows.Forms.DataGridView dataGridViewAxis;
        private System.Windows.Forms.DataGridViewTextBoxColumn GridViewAxisU;
        private System.Windows.Forms.DataGridViewTextBoxColumn GridViewAxisNx;
        private System.Windows.Forms.DataGridViewTextBoxColumn GridViewAxisNy;
        private System.Windows.Forms.DataGridViewTextBoxColumn GridViewAxisNz;
        private System.Windows.Forms.Button buttonAxisSelCurve;
        private System.Windows.Forms.PictureBox pictureBoxAxis;
        private System.Windows.Forms.TextBox textBoxAxisCurveID;
        private System.Windows.Forms.Label labelAxisCurveID;
        private System.Windows.Forms.Button buttonAxisAddTable;
        private System.Windows.Forms.TextBox textBoxAddTableU;
        private System.Windows.Forms.TextBox textBoxAddTableNx;
        private System.Windows.Forms.TextBox textBoxAddTableNy;
        private System.Windows.Forms.TextBox textBoxAddTableNz;
        private System.Windows.Forms.Button buttonAxisAddCopy;
    }
}