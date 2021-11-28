namespace Cocodrilo
{
    partial class WindowMaterial
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
            this.labelMaterialID = new System.Windows.Forms.Label();
            this.PropertiesBox = new System.Windows.Forms.GroupBox();
            this.textBoxMaterialDensity = new System.Windows.Forms.TextBox();
            this.textBoxMaterialAlphaT = new System.Windows.Forms.TextBox();
            this.labelMaterialDensity = new System.Windows.Forms.Label();
            this.labelMaterialAlphaT = new System.Windows.Forms.Label();
            this.textBoxNue = new System.Windows.Forms.TextBox();
            this.textBoxYoungsModulus = new System.Windows.Forms.TextBox();
            this.textBoxMaterialName = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxMaterialID = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.buttonAddMaterial = new System.Windows.Forms.Button();
            this.comboBoxMaterials = new System.Windows.Forms.ComboBox();
            this.bs = new System.Windows.Forms.BindingSource(this.components);
            this.buttonChange = new System.Windows.Forms.Button();
            this.buttonDelete = new System.Windows.Forms.Button();
            this.PropertiesBox.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).BeginInit();
            this.SuspendLayout();
            // 
            // labelMaterialID
            // 
            this.labelMaterialID.AutoSize = true;
            this.labelMaterialID.Location = new System.Drawing.Point(4, 18);
            this.labelMaterialID.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelMaterialID.Name = "labelMaterialID";
            this.labelMaterialID.Size = new System.Drawing.Size(61, 13);
            this.labelMaterialID.TabIndex = 0;
            this.labelMaterialID.Text = "Material ID:";
            // 
            // PropertiesBox
            // 
            this.PropertiesBox.Controls.Add(this.textBoxMaterialDensity);
            this.PropertiesBox.Controls.Add(this.textBoxMaterialAlphaT);
            this.PropertiesBox.Controls.Add(this.labelMaterialDensity);
            this.PropertiesBox.Controls.Add(this.labelMaterialAlphaT);
            this.PropertiesBox.Controls.Add(this.textBoxNue);
            this.PropertiesBox.Controls.Add(this.textBoxYoungsModulus);
            this.PropertiesBox.Controls.Add(this.textBoxMaterialName);
            this.PropertiesBox.Controls.Add(this.label4);
            this.PropertiesBox.Controls.Add(this.textBoxMaterialID);
            this.PropertiesBox.Controls.Add(this.label3);
            this.PropertiesBox.Controls.Add(this.label2);
            this.PropertiesBox.Controls.Add(this.labelMaterialID);
            this.PropertiesBox.Location = new System.Drawing.Point(9, 10);
            this.PropertiesBox.Margin = new System.Windows.Forms.Padding(2);
            this.PropertiesBox.Name = "PropertiesBox";
            this.PropertiesBox.Padding = new System.Windows.Forms.Padding(2);
            this.PropertiesBox.Size = new System.Drawing.Size(229, 141);
            this.PropertiesBox.TabIndex = 1;
            this.PropertiesBox.TabStop = false;
            this.PropertiesBox.Text = "Material Properties:";
            // 
            // textBoxMaterialDensity
            // 
            this.textBoxMaterialDensity.Location = new System.Drawing.Point(97, 109);
            this.textBoxMaterialDensity.Name = "textBoxMaterialDensity";
            this.textBoxMaterialDensity.Size = new System.Drawing.Size(127, 20);
            this.textBoxMaterialDensity.TabIndex = 8;
            this.textBoxMaterialDensity.Text = "1.0";
            // 
            // textBoxMaterialAlphaT
            // 
            this.textBoxMaterialAlphaT.Location = new System.Drawing.Point(4, 109);
            this.textBoxMaterialAlphaT.Name = "textBoxMaterialAlphaT";
            this.textBoxMaterialAlphaT.Size = new System.Drawing.Size(68, 20);
            this.textBoxMaterialAlphaT.TabIndex = 7;
            this.textBoxMaterialAlphaT.Text = "0.0";
            // 
            // labelMaterialDensity
            // 
            this.labelMaterialDensity.AutoSize = true;
            this.labelMaterialDensity.Location = new System.Drawing.Point(94, 93);
            this.labelMaterialDensity.Name = "labelMaterialDensity";
            this.labelMaterialDensity.Size = new System.Drawing.Size(45, 13);
            this.labelMaterialDensity.TabIndex = 6;
            this.labelMaterialDensity.Text = "Density:";
            // 
            // labelMaterialAlphaT
            // 
            this.labelMaterialAlphaT.AutoSize = true;
            this.labelMaterialAlphaT.Location = new System.Drawing.Point(5, 93);
            this.labelMaterialAlphaT.Name = "labelMaterialAlphaT";
            this.labelMaterialAlphaT.Size = new System.Drawing.Size(47, 13);
            this.labelMaterialAlphaT.TabIndex = 5;
            this.labelMaterialAlphaT.Text = "Alpha T:";
            this.labelMaterialAlphaT.Click += new System.EventHandler(this.labelMaterialAlphaT_Click);
            // 
            // textBoxNue
            // 
            this.textBoxNue.Location = new System.Drawing.Point(97, 71);
            this.textBoxNue.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxNue.Name = "textBoxNue";
            this.textBoxNue.Size = new System.Drawing.Size(128, 20);
            this.textBoxNue.TabIndex = 4;
            // 
            // textBoxYoungsModulus
            // 
            this.textBoxYoungsModulus.Location = new System.Drawing.Point(4, 71);
            this.textBoxYoungsModulus.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxYoungsModulus.Name = "textBoxYoungsModulus";
            this.textBoxYoungsModulus.Size = new System.Drawing.Size(68, 20);
            this.textBoxYoungsModulus.TabIndex = 3;
            // 
            // textBoxMaterialName
            // 
            this.textBoxMaterialName.Location = new System.Drawing.Point(97, 34);
            this.textBoxMaterialName.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMaterialName.Name = "textBoxMaterialName";
            this.textBoxMaterialName.Size = new System.Drawing.Size(128, 20);
            this.textBoxMaterialName.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(94, 56);
            this.label4.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(30, 13);
            this.label4.TabIndex = 1;
            this.label4.Text = "Nue:";
            // 
            // textBoxMaterialID
            // 
            this.textBoxMaterialID.Location = new System.Drawing.Point(4, 34);
            this.textBoxMaterialID.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMaterialID.Name = "textBoxMaterialID";
            this.textBoxMaterialID.Size = new System.Drawing.Size(68, 20);
            this.textBoxMaterialID.TabIndex = 1;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(4, 56);
            this.label3.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(91, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Young\'s Modulus:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(94, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Material Name:";
            // 
            // buttonAddMaterial
            // 
            this.buttonAddMaterial.Location = new System.Drawing.Point(154, 180);
            this.buttonAddMaterial.Margin = new System.Windows.Forms.Padding(2);
            this.buttonAddMaterial.Name = "buttonAddMaterial";
            this.buttonAddMaterial.Size = new System.Drawing.Size(80, 26);
            this.buttonAddMaterial.TabIndex = 6;
            this.buttonAddMaterial.Text = "Add Material";
            this.buttonAddMaterial.UseVisualStyleBackColor = true;
            this.buttonAddMaterial.Click += new System.EventHandler(this.buttonAddMaterial_Click);
            // 
            // comboBoxMaterials
            // 
            this.comboBoxMaterials.DataSource = this.bs;
            this.comboBoxMaterials.DisplayMember = "Key";
            this.comboBoxMaterials.FormattingEnabled = true;
            this.comboBoxMaterials.Location = new System.Drawing.Point(13, 155);
            this.comboBoxMaterials.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxMaterials.Name = "comboBoxMaterials";
            this.comboBoxMaterials.Size = new System.Drawing.Size(221, 21);
            this.comboBoxMaterials.TabIndex = 5;
            this.comboBoxMaterials.ValueMember = "Value";
            this.comboBoxMaterials.SelectedIndexChanged += new System.EventHandler(this.comboBoxMaterials_SelectedIndexChanged);
            // 
            // buttonChange
            // 
            this.buttonChange.Location = new System.Drawing.Point(72, 180);
            this.buttonChange.Margin = new System.Windows.Forms.Padding(2);
            this.buttonChange.Name = "buttonChange";
            this.buttonChange.Size = new System.Drawing.Size(78, 26);
            this.buttonChange.TabIndex = 7;
            this.buttonChange.Text = "Change";
            this.buttonChange.UseVisualStyleBackColor = true;
            this.buttonChange.Click += new System.EventHandler(this.buttonChange_Click);
            // 
            // buttonDelete
            // 
            this.buttonDelete.Location = new System.Drawing.Point(9, 180);
            this.buttonDelete.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDelete.Name = "buttonDelete";
            this.buttonDelete.Size = new System.Drawing.Size(59, 26);
            this.buttonDelete.TabIndex = 8;
            this.buttonDelete.Text = "Delete";
            this.buttonDelete.UseVisualStyleBackColor = true;
            this.buttonDelete.Click += new System.EventHandler(this.buttonDelete_Click);
            // 
            // WindowMaterial
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(247, 217);
            this.Controls.Add(this.comboBoxMaterials);
            this.Controls.Add(this.buttonDelete);
            this.Controls.Add(this.buttonChange);
            this.Controls.Add(this.buttonAddMaterial);
            this.Controls.Add(this.PropertiesBox);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "WindowMaterial";
            this.Text = "WindowMaterial";
            this.Load += new System.EventHandler(this.WindowMaterial_Load);
            this.PropertiesBox.ResumeLayout(false);
            this.PropertiesBox.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bs)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelMaterialID;
        private System.Windows.Forms.GroupBox PropertiesBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBoxMaterialID;
        private System.Windows.Forms.TextBox textBoxNue;
        private System.Windows.Forms.TextBox textBoxYoungsModulus;
        private System.Windows.Forms.TextBox textBoxMaterialName;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button buttonAddMaterial;
        private System.Windows.Forms.ComboBox comboBoxMaterials;
        private System.Windows.Forms.BindingSource bs;
        private System.Windows.Forms.Button buttonChange;
        private System.Windows.Forms.Button buttonDelete;
        private System.Windows.Forms.TextBox textBoxMaterialDensity;
        private System.Windows.Forms.TextBox textBoxMaterialAlphaT;
        private System.Windows.Forms.Label labelMaterialDensity;
        private System.Windows.Forms.Label labelMaterialAlphaT;
    }
}