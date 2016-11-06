namespace MshToMatConverter
{
    partial class PreferencesForm
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
            this.lblVertexMatrixName = new System.Windows.Forms.Label();
            this.lblElementMatrixName = new System.Windows.Forms.Label();
            this.lblCutMatrixName = new System.Windows.Forms.Label();
            this.txtVertexMatrixName = new System.Windows.Forms.TextBox();
            this.txtElementMatrixName = new System.Windows.Forms.TextBox();
            this.txtCutMatrixName = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblCurveType = new System.Windows.Forms.Label();
            this.cmbCurveType = new System.Windows.Forms.ComboBox();
            this.lblAreaRelMatrixName = new System.Windows.Forms.Label();
            this.txtAreaRelMatrixName = new System.Windows.Forms.TextBox();
            this.chkAreaRelAllow = new System.Windows.Forms.CheckBox();
            this.chkWeightFunctionCalculate = new System.Windows.Forms.CheckBox();
            this.lblWeightFunctionMatrixName = new System.Windows.Forms.Label();
            this.txtWeightFunctionMatrixName = new System.Windows.Forms.TextBox();
            this.lblRadiusesDifference = new System.Windows.Forms.Label();
            this.numRadiusesDifference = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numRadiusesDifference)).BeginInit();
            this.SuspendLayout();
            // 
            // lblVertexMatrixName
            // 
            this.lblVertexMatrixName.AutoSize = true;
            this.lblVertexMatrixName.Location = new System.Drawing.Point(40, 15);
            this.lblVertexMatrixName.Name = "lblVertexMatrixName";
            this.lblVertexMatrixName.Size = new System.Drawing.Size(37, 13);
            this.lblVertexMatrixName.TabIndex = 0;
            this.lblVertexMatrixName.Text = "Vertex";
            // 
            // lblElementMatrixName
            // 
            this.lblElementMatrixName.AutoSize = true;
            this.lblElementMatrixName.Location = new System.Drawing.Point(32, 41);
            this.lblElementMatrixName.Name = "lblElementMatrixName";
            this.lblElementMatrixName.Size = new System.Drawing.Size(45, 13);
            this.lblElementMatrixName.TabIndex = 1;
            this.lblElementMatrixName.Text = "Element";
            // 
            // lblCutMatrixName
            // 
            this.lblCutMatrixName.AutoSize = true;
            this.lblCutMatrixName.Location = new System.Drawing.Point(12, 67);
            this.lblCutMatrixName.Name = "lblCutMatrixName";
            this.lblCutMatrixName.Size = new System.Drawing.Size(65, 13);
            this.lblCutMatrixName.TabIndex = 2;
            this.lblCutMatrixName.Text = "Cut triangles";
            // 
            // txtVertexMatrixName
            // 
            this.txtVertexMatrixName.Location = new System.Drawing.Point(83, 12);
            this.txtVertexMatrixName.Name = "txtVertexMatrixName";
            this.txtVertexMatrixName.Size = new System.Drawing.Size(100, 20);
            this.txtVertexMatrixName.TabIndex = 3;
            // 
            // txtElementMatrixName
            // 
            this.txtElementMatrixName.Location = new System.Drawing.Point(83, 38);
            this.txtElementMatrixName.Name = "txtElementMatrixName";
            this.txtElementMatrixName.Size = new System.Drawing.Size(100, 20);
            this.txtElementMatrixName.TabIndex = 4;
            // 
            // txtCutMatrixName
            // 
            this.txtCutMatrixName.Location = new System.Drawing.Point(83, 64);
            this.txtCutMatrixName.Name = "txtCutMatrixName";
            this.txtCutMatrixName.Size = new System.Drawing.Size(100, 20);
            this.txtCutMatrixName.TabIndex = 5;
            // 
            // btnOK
            // 
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(220, 123);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 0;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(301, 123);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 7;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // lblCurveType
            // 
            this.lblCurveType.AutoSize = true;
            this.lblCurveType.Location = new System.Drawing.Point(19, 93);
            this.lblCurveType.Name = "lblCurveType";
            this.lblCurveType.Size = new System.Drawing.Size(58, 13);
            this.lblCurveType.TabIndex = 8;
            this.lblCurveType.Text = "Curve type";
            // 
            // cmbCurveType
            // 
            this.cmbCurveType.FormattingEnabled = true;
            this.cmbCurveType.Location = new System.Drawing.Point(83, 90);
            this.cmbCurveType.Name = "cmbCurveType";
            this.cmbCurveType.Size = new System.Drawing.Size(100, 21);
            this.cmbCurveType.TabIndex = 9;
            this.cmbCurveType.SelectedIndexChanged += new System.EventHandler(this.cmbCurveType_SelectedIndexChanged);
            // 
            // lblAreaRelMatrixName
            // 
            this.lblAreaRelMatrixName.AutoSize = true;
            this.lblAreaRelMatrixName.Location = new System.Drawing.Point(205, 34);
            this.lblAreaRelMatrixName.Name = "lblAreaRelMatrixName";
            this.lblAreaRelMatrixName.Size = new System.Drawing.Size(66, 13);
            this.lblAreaRelMatrixName.TabIndex = 11;
            this.lblAreaRelMatrixName.Text = "Area relation";
            // 
            // txtAreaRelMatrixName
            // 
            this.txtAreaRelMatrixName.Location = new System.Drawing.Point(277, 31);
            this.txtAreaRelMatrixName.Name = "txtAreaRelMatrixName";
            this.txtAreaRelMatrixName.Size = new System.Drawing.Size(100, 20);
            this.txtAreaRelMatrixName.TabIndex = 12;
            // 
            // chkAreaRelAllow
            // 
            this.chkAreaRelAllow.AutoSize = true;
            this.chkAreaRelAllow.Location = new System.Drawing.Point(189, 11);
            this.chkAreaRelAllow.Name = "chkAreaRelAllow";
            this.chkAreaRelAllow.Size = new System.Drawing.Size(131, 17);
            this.chkAreaRelAllow.TabIndex = 13;
            this.chkAreaRelAllow.Text = "Calculate area relation";
            this.chkAreaRelAllow.UseVisualStyleBackColor = true;
            this.chkAreaRelAllow.CheckedChanged += new System.EventHandler(this.chkAreaRelAllow_CheckedChanged);
            // 
            // chkWeightFunctionCalculate
            // 
            this.chkWeightFunctionCalculate.AutoSize = true;
            this.chkWeightFunctionCalculate.Location = new System.Drawing.Point(189, 54);
            this.chkWeightFunctionCalculate.Name = "chkWeightFunctionCalculate";
            this.chkWeightFunctionCalculate.Size = new System.Drawing.Size(145, 17);
            this.chkWeightFunctionCalculate.TabIndex = 14;
            this.chkWeightFunctionCalculate.Text = "Calculate weight function";
            this.chkWeightFunctionCalculate.UseVisualStyleBackColor = true;
            this.chkWeightFunctionCalculate.CheckedChanged += new System.EventHandler(this.chkWeightFunctionCalculate_CheckedChanged);
            // 
            // lblWeightFunctionMatrixName
            // 
            this.lblWeightFunctionMatrixName.AutoSize = true;
            this.lblWeightFunctionMatrixName.Location = new System.Drawing.Point(189, 74);
            this.lblWeightFunctionMatrixName.Name = "lblWeightFunctionMatrixName";
            this.lblWeightFunctionMatrixName.Size = new System.Drawing.Size(82, 13);
            this.lblWeightFunctionMatrixName.TabIndex = 15;
            this.lblWeightFunctionMatrixName.Text = "Weight function";
            // 
            // txtWeightFunctionMatrixName
            // 
            this.txtWeightFunctionMatrixName.Location = new System.Drawing.Point(277, 71);
            this.txtWeightFunctionMatrixName.Name = "txtWeightFunctionMatrixName";
            this.txtWeightFunctionMatrixName.Size = new System.Drawing.Size(100, 20);
            this.txtWeightFunctionMatrixName.TabIndex = 16;
            // 
            // lblRadiusesDifference
            // 
            this.lblRadiusesDifference.AutoSize = true;
            this.lblRadiusesDifference.Location = new System.Drawing.Point(189, 98);
            this.lblRadiusesDifference.Name = "lblRadiusesDifference";
            this.lblRadiusesDifference.Size = new System.Drawing.Size(135, 13);
            this.lblRadiusesDifference.TabIndex = 17;
            this.lblRadiusesDifference.Text = "Distance between radiuses";
            // 
            // numRadiusesDifference
            // 
            this.numRadiusesDifference.DecimalPlaces = 1;
            this.numRadiusesDifference.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numRadiusesDifference.Location = new System.Drawing.Point(330, 97);
            this.numRadiusesDifference.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            -2147483648});
            this.numRadiusesDifference.Name = "numRadiusesDifference";
            this.numRadiusesDifference.Size = new System.Drawing.Size(47, 20);
            this.numRadiusesDifference.TabIndex = 18;
            // 
            // PreferencesForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(388, 152);
            this.Controls.Add(this.numRadiusesDifference);
            this.Controls.Add(this.lblRadiusesDifference);
            this.Controls.Add(this.txtWeightFunctionMatrixName);
            this.Controls.Add(this.lblWeightFunctionMatrixName);
            this.Controls.Add(this.chkWeightFunctionCalculate);
            this.Controls.Add(this.chkAreaRelAllow);
            this.Controls.Add(this.txtAreaRelMatrixName);
            this.Controls.Add(this.lblAreaRelMatrixName);
            this.Controls.Add(this.cmbCurveType);
            this.Controls.Add(this.lblCurveType);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.txtCutMatrixName);
            this.Controls.Add(this.txtElementMatrixName);
            this.Controls.Add(this.txtVertexMatrixName);
            this.Controls.Add(this.lblCutMatrixName);
            this.Controls.Add(this.lblElementMatrixName);
            this.Controls.Add(this.lblVertexMatrixName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "PreferencesForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Preferences";
            ((System.ComponentModel.ISupportInitialize)(this.numRadiusesDifference)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblVertexMatrixName;
        private System.Windows.Forms.Label lblElementMatrixName;
        private System.Windows.Forms.Label lblCutMatrixName;
        private System.Windows.Forms.TextBox txtVertexMatrixName;
        private System.Windows.Forms.TextBox txtElementMatrixName;
        private System.Windows.Forms.TextBox txtCutMatrixName;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Label lblCurveType;
        private System.Windows.Forms.ComboBox cmbCurveType;
        private System.Windows.Forms.Label lblAreaRelMatrixName;
        private System.Windows.Forms.TextBox txtAreaRelMatrixName;
        private System.Windows.Forms.CheckBox chkAreaRelAllow;
        private System.Windows.Forms.CheckBox chkWeightFunctionCalculate;
        private System.Windows.Forms.Label lblWeightFunctionMatrixName;
        private System.Windows.Forms.TextBox txtWeightFunctionMatrixName;
        private System.Windows.Forms.Label lblRadiusesDifference;
        private System.Windows.Forms.NumericUpDown numRadiusesDifference;
    }
}