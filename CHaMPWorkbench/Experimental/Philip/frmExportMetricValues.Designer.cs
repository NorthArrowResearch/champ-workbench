namespace CHaMPWorkbench.Experimental.Philip
{
    partial class frmExportMetricValues
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
            this.label1 = new System.Windows.Forms.Label();
            this.txtVisits = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboMetricSchema = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.cmdBrowseOutput = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(125, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Number of visits selected";
            // 
            // txtVisits
            // 
            this.txtVisits.Location = new System.Drawing.Point(144, 16);
            this.txtVisits.Name = "txtVisits";
            this.txtVisits.Size = new System.Drawing.Size(100, 20);
            this.txtVisits.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Metric schema";
            // 
            // cboMetricSchema
            // 
            this.cboMetricSchema.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMetricSchema.FormattingEnabled = true;
            this.cboMetricSchema.Location = new System.Drawing.Point(144, 46);
            this.cboMetricSchema.Name = "cboMetricSchema";
            this.cboMetricSchema.Size = new System.Drawing.Size(388, 21);
            this.cboMetricSchema.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(22, 81);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(111, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Output top level folder";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(144, 77);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(388, 20);
            this.txtOutputFolder.TabIndex = 5;
            // 
            // cmdBrowseOutput
            // 
            this.cmdBrowseOutput.Image = global::CHaMPWorkbench.Properties.Resources.BrowseFolder;
            this.cmdBrowseOutput.Location = new System.Drawing.Point(538, 76);
            this.cmdBrowseOutput.Name = "cmdBrowseOutput";
            this.cmdBrowseOutput.Size = new System.Drawing.Size(23, 23);
            this.cmdBrowseOutput.TabIndex = 6;
            this.cmdBrowseOutput.UseVisualStyleBackColor = true;
            this.cmdBrowseOutput.Click += new System.EventHandler(this.cmdBrowseOutput_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(488, 117);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(407, 117);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 7;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 117);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 9;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // frmExportMetricValues
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(575, 152);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdBrowseOutput);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboMetricSchema);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtVisits);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmExportMetricValues";
            this.Text = "Export Metric Values";
            this.Load += new System.EventHandler(this.frmExportMetricValues_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtVisits;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboMetricSchema;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.Button cmdBrowseOutput;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
    }
}