namespace CHaMPWorkbench.Data.MetricDefinitions
{
    partial class frmMetricProperties
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMetricProperties));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdSave = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtShortName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboModel = new System.Windows.Forms.ComboBox();
            this.cboDataType = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.Calculation = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.lblPrecision = new System.Windows.Forms.Label();
            this.valPrecision = new System.Windows.Forms.NumericUpDown();
            this.tabSchemas = new System.Windows.Forms.TabPage();
            this.chkSchemas = new System.Windows.Forms.CheckedListBox();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.cmdAltHelp = new System.Windows.Forms.Button();
            this.txtAltLink = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.cmdOnlineHelp = new System.Windows.Forms.Button();
            this.txtMMLink = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtLastUpdated = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lblThreshold = new System.Windows.Forms.Label();
            this.lblMaximum = new System.Windows.Forms.Label();
            this.lblMinimum = new System.Windows.Forms.Label();
            this.chkValidation = new System.Windows.Forms.CheckBox();
            this.valThreshold = new System.Windows.Forms.NumericUpDown();
            this.valMaxValue = new System.Windows.Forms.NumericUpDown();
            this.valMinValue = new System.Windows.Forms.NumericUpDown();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.txtXPath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.Calculation.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valPrecision)).BeginInit();
            this.tabSchemas.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMaxValue)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMinValue)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(374, 352);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 13;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdSave
            // 
            this.cmdSave.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdSave.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdSave.Location = new System.Drawing.Point(293, 352);
            this.cmdSave.Name = "cmdSave";
            this.cmdSave.Size = new System.Drawing.Size(75, 23);
            this.cmdSave.TabIndex = 12;
            this.cmdSave.Text = "Save";
            this.cmdSave.UseVisualStyleBackColor = true;
            this.cmdSave.Click += new System.EventHandler(this.cmdSave_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 352);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 14;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(61, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Short name";
            // 
            // txtShortName
            // 
            this.txtShortName.Location = new System.Drawing.Point(73, 43);
            this.txtShortName.MaxLength = 255;
            this.txtShortName.Name = "txtShortName";
            this.txtShortName.Size = new System.Drawing.Size(165, 20);
            this.txtShortName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 76);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Model";
            // 
            // cboModel
            // 
            this.cboModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModel.FormattingEnabled = true;
            this.cboModel.Location = new System.Drawing.Point(73, 72);
            this.cboModel.Name = "cboModel";
            this.cboModel.Size = new System.Drawing.Size(165, 21);
            this.cboModel.TabIndex = 5;
            // 
            // cboDataType
            // 
            this.cboDataType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboDataType.FormattingEnabled = true;
            this.cboDataType.Location = new System.Drawing.Point(96, 19);
            this.cboDataType.Name = "cboDataType";
            this.cboDataType.Size = new System.Drawing.Size(234, 21);
            this.cboDataType.TabIndex = 12;
            this.cboDataType.SelectedIndexChanged += new System.EventHandler(this.UpdateControls);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(37, 23);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Data type";
            // 
            // Calculation
            // 
            this.Calculation.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Calculation.Controls.Add(this.tabPage1);
            this.Calculation.Controls.Add(this.tabSchemas);
            this.Calculation.Controls.Add(this.tabPage3);
            this.Calculation.Controls.Add(this.tabPage2);
            this.Calculation.Location = new System.Drawing.Point(12, 162);
            this.Calculation.Name = "Calculation";
            this.Calculation.SelectedIndex = 0;
            this.Calculation.Size = new System.Drawing.Size(437, 184);
            this.Calculation.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblPrecision);
            this.tabPage1.Controls.Add(this.valPrecision);
            this.tabPage1.Controls.Add(this.cboDataType);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(429, 158);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Data Storage";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // lblPrecision
            // 
            this.lblPrecision.AutoSize = true;
            this.lblPrecision.Location = new System.Drawing.Point(40, 50);
            this.lblPrecision.Name = "lblPrecision";
            this.lblPrecision.Size = new System.Drawing.Size(50, 13);
            this.lblPrecision.TabIndex = 15;
            this.lblPrecision.Text = "Precision";
            // 
            // valPrecision
            // 
            this.valPrecision.Location = new System.Drawing.Point(96, 46);
            this.valPrecision.Maximum = new decimal(new int[] {
            8,
            0,
            0,
            0});
            this.valPrecision.Name = "valPrecision";
            this.valPrecision.Size = new System.Drawing.Size(71, 20);
            this.valPrecision.TabIndex = 14;
            this.valPrecision.ValueChanged += new System.EventHandler(this.UpdateControls);
            // 
            // tabSchemas
            // 
            this.tabSchemas.Controls.Add(this.chkSchemas);
            this.tabSchemas.Location = new System.Drawing.Point(4, 22);
            this.tabSchemas.Name = "tabSchemas";
            this.tabSchemas.Padding = new System.Windows.Forms.Padding(3);
            this.tabSchemas.Size = new System.Drawing.Size(429, 158);
            this.tabSchemas.TabIndex = 3;
            this.tabSchemas.Text = "Schemas";
            this.tabSchemas.UseVisualStyleBackColor = true;
            // 
            // chkSchemas
            // 
            this.chkSchemas.CheckOnClick = true;
            this.chkSchemas.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkSchemas.FormattingEnabled = true;
            this.chkSchemas.Location = new System.Drawing.Point(3, 3);
            this.chkSchemas.Name = "chkSchemas";
            this.chkSchemas.Size = new System.Drawing.Size(423, 152);
            this.chkSchemas.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.cmdAltHelp);
            this.tabPage3.Controls.Add(this.txtAltLink);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.cmdOnlineHelp);
            this.tabPage3.Controls.Add(this.txtMMLink);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.txtLastUpdated);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(429, 158);
            this.tabPage3.TabIndex = 5;
            this.tabPage3.Text = "MetaData";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // cmdAltHelp
            // 
            this.cmdAltHelp.Enabled = false;
            this.cmdAltHelp.Image = global::CHaMPWorkbench.Properties.Resources.WebSite;
            this.cmdAltHelp.Location = new System.Drawing.Point(330, 66);
            this.cmdAltHelp.Name = "cmdAltHelp";
            this.cmdAltHelp.Size = new System.Drawing.Size(23, 23);
            this.cmdAltHelp.TabIndex = 11;
            this.cmdAltHelp.UseVisualStyleBackColor = true;
            this.cmdAltHelp.Click += new System.EventHandler(this.cmdAltHelp_Click);
            // 
            // txtAltLink
            // 
            this.txtAltLink.Location = new System.Drawing.Point(86, 67);
            this.txtAltLink.Name = "txtAltLink";
            this.txtAltLink.Size = new System.Drawing.Size(241, 20);
            this.txtAltLink.TabIndex = 10;
            this.txtAltLink.TextChanged += new System.EventHandler(this.UpdateControls);
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 71);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(76, 13);
            this.label12.TabIndex = 9;
            this.label12.Text = "Alt. online help";
            // 
            // cmdOnlineHelp
            // 
            this.cmdOnlineHelp.Enabled = false;
            this.cmdOnlineHelp.Image = global::CHaMPWorkbench.Properties.Resources.WebSite;
            this.cmdOnlineHelp.Location = new System.Drawing.Point(330, 40);
            this.cmdOnlineHelp.Name = "cmdOnlineHelp";
            this.cmdOnlineHelp.Size = new System.Drawing.Size(23, 23);
            this.cmdOnlineHelp.TabIndex = 8;
            this.cmdOnlineHelp.UseVisualStyleBackColor = true;
            this.cmdOnlineHelp.Click += new System.EventHandler(this.cmdOnlineHelp_Click);
            // 
            // txtMMLink
            // 
            this.txtMMLink.Location = new System.Drawing.Point(86, 41);
            this.txtMMLink.Name = "txtMMLink";
            this.txtMMLink.Size = new System.Drawing.Size(241, 20);
            this.txtMMLink.TabIndex = 7;
            this.txtMMLink.TextChanged += new System.EventHandler(this.UpdateControls);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(20, 45);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(60, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "Online help";
            // 
            // txtLastUpdated
            // 
            this.txtLastUpdated.Location = new System.Drawing.Point(86, 15);
            this.txtLastUpdated.Name = "txtLastUpdated";
            this.txtLastUpdated.ReadOnly = true;
            this.txtLastUpdated.Size = new System.Drawing.Size(241, 20);
            this.txtLastUpdated.TabIndex = 1;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(11, 19);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 13);
            this.label13.TabIndex = 0;
            this.label13.Text = "Last updated";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lblThreshold);
            this.tabPage2.Controls.Add(this.lblMaximum);
            this.tabPage2.Controls.Add(this.lblMinimum);
            this.tabPage2.Controls.Add(this.chkValidation);
            this.tabPage2.Controls.Add(this.valThreshold);
            this.tabPage2.Controls.Add(this.valMaxValue);
            this.tabPage2.Controls.Add(this.valMinValue);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(429, 158);
            this.tabPage2.TabIndex = 6;
            this.tabPage2.Text = "Validation";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lblThreshold
            // 
            this.lblThreshold.AutoSize = true;
            this.lblThreshold.Location = new System.Drawing.Point(56, 95);
            this.lblThreshold.Name = "lblThreshold";
            this.lblThreshold.Size = new System.Drawing.Size(71, 13);
            this.lblThreshold.TabIndex = 27;
            this.lblThreshold.Text = "Threshold (%)";
            // 
            // lblMaximum
            // 
            this.lblMaximum.AutoSize = true;
            this.lblMaximum.Location = new System.Drawing.Point(47, 69);
            this.lblMaximum.Name = "lblMaximum";
            this.lblMaximum.Size = new System.Drawing.Size(80, 13);
            this.lblMaximum.TabIndex = 26;
            this.lblMaximum.Text = "Maximum value";
            // 
            // lblMinimum
            // 
            this.lblMinimum.AutoSize = true;
            this.lblMinimum.Location = new System.Drawing.Point(50, 40);
            this.lblMinimum.Name = "lblMinimum";
            this.lblMinimum.Size = new System.Drawing.Size(77, 13);
            this.lblMinimum.TabIndex = 25;
            this.lblMinimum.Text = "Minimum value";
            // 
            // chkValidation
            // 
            this.chkValidation.AutoSize = true;
            this.chkValidation.Location = new System.Drawing.Point(15, 13);
            this.chkValidation.Name = "chkValidation";
            this.chkValidation.Size = new System.Drawing.Size(150, 17);
            this.chkValidation.TabIndex = 24;
            this.chkValidation.Text = "Include in validation report";
            this.chkValidation.UseVisualStyleBackColor = true;
            this.chkValidation.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // valThreshold
            // 
            this.valThreshold.Location = new System.Drawing.Point(132, 91);
            this.valThreshold.Name = "valThreshold";
            this.valThreshold.Size = new System.Drawing.Size(71, 20);
            this.valThreshold.TabIndex = 23;
            // 
            // valMaxValue
            // 
            this.valMaxValue.DecimalPlaces = 2;
            this.valMaxValue.Location = new System.Drawing.Point(132, 65);
            this.valMaxValue.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.valMaxValue.Name = "valMaxValue";
            this.valMaxValue.Size = new System.Drawing.Size(71, 20);
            this.valMaxValue.TabIndex = 22;
            // 
            // valMinValue
            // 
            this.valMinValue.DecimalPlaces = 2;
            this.valMinValue.Location = new System.Drawing.Point(132, 36);
            this.valMinValue.Maximum = new decimal(new int[] {
            1000000,
            0,
            0,
            0});
            this.valMinValue.Name = "valMinValue";
            this.valMinValue.Size = new System.Drawing.Size(71, 20);
            this.valMinValue.TabIndex = 21;
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Location = new System.Drawing.Point(73, 100);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(56, 17);
            this.chkActive.TabIndex = 8;
            this.chkActive.Text = "Active";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // txtXPath
            // 
            this.txtXPath.Location = new System.Drawing.Point(73, 126);
            this.txtXPath.MaxLength = 255;
            this.txtXPath.Name = "txtXPath";
            this.txtXPath.Size = new System.Drawing.Size(376, 20);
            this.txtXPath.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "XPath";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(73, 14);
            this.txtName.MaxLength = 255;
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(376, 20);
            this.txtName.TabIndex = 1;
            // 
            // frmMetricProperties
            // 
            this.AcceptButton = this.cmdSave;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(461, 387);
            this.Controls.Add(this.txtXPath);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.chkActive);
            this.Controls.Add(this.Calculation);
            this.Controls.Add(this.cboModel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtShortName);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdSave);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmMetricProperties";
            this.Text = "Metric Definition";
            this.Load += new System.EventHandler(this.frmMetricProperties_Load);
            this.Calculation.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valPrecision)).EndInit();
            this.tabSchemas.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMaxValue)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valMinValue)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdSave;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtShortName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboModel;
        private System.Windows.Forms.ComboBox cboDataType;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TabControl Calculation;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.NumericUpDown valPrecision;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.TextBox txtXPath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabSchemas;
        private System.Windows.Forms.CheckedListBox chkSchemas;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.TextBox txtLastUpdated;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Button cmdAltHelp;
        private System.Windows.Forms.TextBox txtAltLink;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Button cmdOnlineHelp;
        private System.Windows.Forms.TextBox txtMMLink;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label lblPrecision;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label lblThreshold;
        private System.Windows.Forms.Label lblMaximum;
        private System.Windows.Forms.Label lblMinimum;
        private System.Windows.Forms.CheckBox chkValidation;
        private System.Windows.Forms.NumericUpDown valThreshold;
        private System.Windows.Forms.NumericUpDown valMaxValue;
        private System.Windows.Forms.NumericUpDown valMinValue;
        private System.Windows.Forms.TextBox txtName;
    }
}