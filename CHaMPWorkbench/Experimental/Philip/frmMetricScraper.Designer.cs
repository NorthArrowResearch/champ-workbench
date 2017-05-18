namespace CHaMPWorkbench.Experimental.Philip
{
    partial class frmMetricScraper
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboScavengeType = new System.Windows.Forms.ComboBox();
            this.grpModelVersion = new System.Windows.Forms.GroupBox();
            this.txtModelVersion = new System.Windows.Forms.TextBox();
            this.rdoSpecifiedModelVersion = new System.Windows.Forms.RadioButton();
            this.rdoXMLModelVersion = new System.Windows.Forms.RadioButton();
            this.chkVerify = new System.Windows.Forms.CheckBox();
            this.grpModelVersion.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(507, 240);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(406, 240);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(95, 23);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "Scrape Metrics";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(80, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Top level folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Metric result file";
            // 
            // txtFolder
            // 
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.Location = new System.Drawing.Point(102, 15);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(451, 20);
            this.txtFolder.TabIndex = 1;
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowse.Image = global::CHaMPWorkbench.Properties.Resources.BrowseFolder;
            this.cmdBrowse.Location = new System.Drawing.Point(559, 14);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(23, 23);
            this.cmdBrowse.TabIndex = 2;
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // txtFileName
            // 
            this.txtFileName.Location = new System.Drawing.Point(102, 44);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(172, 20);
            this.txtFileName.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Scavenge type";
            // 
            // cboScavengeType
            // 
            this.cboScavengeType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboScavengeType.FormattingEnabled = true;
            this.cboScavengeType.Location = new System.Drawing.Point(101, 73);
            this.cboScavengeType.Name = "cboScavengeType";
            this.cboScavengeType.Size = new System.Drawing.Size(173, 21);
            this.cboScavengeType.TabIndex = 6;
            // 
            // grpModelVersion
            // 
            this.grpModelVersion.Controls.Add(this.txtModelVersion);
            this.grpModelVersion.Controls.Add(this.rdoSpecifiedModelVersion);
            this.grpModelVersion.Controls.Add(this.rdoXMLModelVersion);
            this.grpModelVersion.Location = new System.Drawing.Point(18, 130);
            this.grpModelVersion.Name = "grpModelVersion";
            this.grpModelVersion.Size = new System.Drawing.Size(555, 100);
            this.grpModelVersion.TabIndex = 7;
            this.grpModelVersion.TabStop = false;
            this.grpModelVersion.Text = "Model Version";
            // 
            // txtModelVersion
            // 
            this.txtModelVersion.Location = new System.Drawing.Point(84, 67);
            this.txtModelVersion.Name = "txtModelVersion";
            this.txtModelVersion.Size = new System.Drawing.Size(172, 20);
            this.txtModelVersion.TabIndex = 2;
            // 
            // rdoSpecifiedModelVersion
            // 
            this.rdoSpecifiedModelVersion.AutoSize = true;
            this.rdoSpecifiedModelVersion.Location = new System.Drawing.Point(18, 44);
            this.rdoSpecifiedModelVersion.Name = "rdoSpecifiedModelVersion";
            this.rdoSpecifiedModelVersion.Size = new System.Drawing.Size(174, 17);
            this.rdoSpecifiedModelVersion.TabIndex = 1;
            this.rdoSpecifiedModelVersion.Text = "Use the following model version";
            this.rdoSpecifiedModelVersion.UseVisualStyleBackColor = true;
            // 
            // rdoXMLModelVersion
            // 
            this.rdoXMLModelVersion.AutoSize = true;
            this.rdoXMLModelVersion.Checked = true;
            this.rdoXMLModelVersion.Location = new System.Drawing.Point(18, 20);
            this.rdoXMLModelVersion.Name = "rdoXMLModelVersion";
            this.rdoXMLModelVersion.Size = new System.Drawing.Size(263, 17);
            this.rdoXMLModelVersion.TabIndex = 0;
            this.rdoXMLModelVersion.TabStop = true;
            this.rdoXMLModelVersion.Text = "Use the model version specified in metric XML files";
            this.rdoXMLModelVersion.UseVisualStyleBackColor = true;
            this.rdoXMLModelVersion.CheckedChanged += new System.EventHandler(this.rdoXMLModelVersion_CheckedChanged);
            // 
            // chkVerify
            // 
            this.chkVerify.AutoSize = true;
            this.chkVerify.Checked = true;
            this.chkVerify.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVerify.Location = new System.Drawing.Point(102, 107);
            this.chkVerify.Name = "chkVerify";
            this.chkVerify.Size = new System.Drawing.Size(441, 17);
            this.chkVerify.TabIndex = 10;
            this.chkVerify.Text = "Only scrape metrics if all Workbench metrics validate with CHaMP automation defin" +
    "itions";
            this.chkVerify.UseVisualStyleBackColor = true;
            // 
            // frmMetricScraper
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(594, 275);
            this.Controls.Add(this.chkVerify);
            this.Controls.Add(this.grpModelVersion);
            this.Controls.Add(this.cboScavengeType);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtFileName);
            this.Controls.Add(this.cmdBrowse);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "frmMetricScraper";
            this.Text = "Metric Result Scraper";
            this.Load += new System.EventHandler(this.frmMetricScraper_Load);
            this.grpModelVersion.ResumeLayout(false);
            this.grpModelVersion.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtFolder;
        private System.Windows.Forms.Button cmdBrowse;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboScavengeType;
        private System.Windows.Forms.GroupBox grpModelVersion;
        private System.Windows.Forms.TextBox txtModelVersion;
        private System.Windows.Forms.RadioButton rdoSpecifiedModelVersion;
        private System.Windows.Forms.RadioButton rdoXMLModelVersion;
        private System.Windows.Forms.CheckBox chkVerify;
    }
}