namespace CHaMPWorkbench.Data
{
    partial class frmImportCHaMPInfo
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
            this.label2 = new System.Windows.Forms.Label();
            this.txtDatabase = new System.Windows.Forms.TextBox();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.chkImportFish = new System.Windows.Forms.CheckBox();
            this.cmdBrowseSurveyDesign = new System.Windows.Forms.Button();
            this.txtSurveyDesign = new System.Windows.Forms.TextBox();
            this.lblSurveyDesign = new System.Windows.Forms.Label();
            this.cmdBrowseProgramMetrics = new System.Windows.Forms.Button();
            this.txtProgramMetrics = new System.Windows.Forms.TextBox();
            this.lblProgramMetrics = new System.Windows.Forms.Label();
            this.chkExtendedSiteInfo = new System.Windows.Forms.CheckBox();
            this.frmOpen = new System.Windows.Forms.OpenFileDialog();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(380, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Browse and select the CHaMP \"All Measurements\" exported Access database.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "All measreuments database";
            // 
            // txtDatabase
            // 
            this.txtDatabase.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDatabase.Location = new System.Drawing.Point(158, 37);
            this.txtDatabase.Name = "txtDatabase";
            this.txtDatabase.Size = new System.Drawing.Size(422, 20);
            this.txtDatabase.TabIndex = 2;
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowse.Location = new System.Drawing.Point(586, 36);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowse.TabIndex = 3;
            this.cmdBrowse.Text = "Browse";
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(586, 194);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 13;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(505, 194);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 12;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // chkImportFish
            // 
            this.chkImportFish.AutoSize = true;
            this.chkImportFish.Location = new System.Drawing.Point(16, 71);
            this.chkImportFish.Name = "chkImportFish";
            this.chkImportFish.Size = new System.Drawing.Size(167, 17);
            this.chkImportFish.TabIndex = 4;
            this.chkImportFish.Text = "Import fish species information";
            this.chkImportFish.UseVisualStyleBackColor = true;
            this.chkImportFish.CheckedChanged += new System.EventHandler(this.chkImportFish_CheckedChanged);
            // 
            // cmdBrowseSurveyDesign
            // 
            this.cmdBrowseSurveyDesign.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseSurveyDesign.Location = new System.Drawing.Point(586, 94);
            this.cmdBrowseSurveyDesign.Name = "cmdBrowseSurveyDesign";
            this.cmdBrowseSurveyDesign.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseSurveyDesign.TabIndex = 7;
            this.cmdBrowseSurveyDesign.Text = "Browse";
            this.cmdBrowseSurveyDesign.UseVisualStyleBackColor = true;
            this.cmdBrowseSurveyDesign.Click += new System.EventHandler(this.cmdBrowseSurveyDesign_Click);
            // 
            // txtSurveyDesign
            // 
            this.txtSurveyDesign.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSurveyDesign.Location = new System.Drawing.Point(158, 95);
            this.txtSurveyDesign.Name = "txtSurveyDesign";
            this.txtSurveyDesign.Size = new System.Drawing.Size(422, 20);
            this.txtSurveyDesign.TabIndex = 6;
            // 
            // lblSurveyDesign
            // 
            this.lblSurveyDesign.AutoSize = true;
            this.lblSurveyDesign.Location = new System.Drawing.Point(31, 99);
            this.lblSurveyDesign.Name = "lblSurveyDesign";
            this.lblSurveyDesign.Size = new System.Drawing.Size(121, 13);
            this.lblSurveyDesign.TabIndex = 5;
            this.lblSurveyDesign.Text = "Survey design database";
            // 
            // cmdBrowseProgramMetrics
            // 
            this.cmdBrowseProgramMetrics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseProgramMetrics.Location = new System.Drawing.Point(584, 153);
            this.cmdBrowseProgramMetrics.Name = "cmdBrowseProgramMetrics";
            this.cmdBrowseProgramMetrics.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseProgramMetrics.TabIndex = 11;
            this.cmdBrowseProgramMetrics.Text = "Browse";
            this.cmdBrowseProgramMetrics.UseVisualStyleBackColor = true;
            this.cmdBrowseProgramMetrics.Click += new System.EventHandler(this.cmdBrowseProgramMetrics_Click);
            // 
            // txtProgramMetrics
            // 
            this.txtProgramMetrics.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProgramMetrics.Location = new System.Drawing.Point(158, 154);
            this.txtProgramMetrics.Name = "txtProgramMetrics";
            this.txtProgramMetrics.Size = new System.Drawing.Size(422, 20);
            this.txtProgramMetrics.TabIndex = 10;
            // 
            // lblProgramMetrics
            // 
            this.lblProgramMetrics.AutoSize = true;
            this.lblProgramMetrics.Location = new System.Drawing.Point(23, 158);
            this.lblProgramMetrics.Name = "lblProgramMetrics";
            this.lblProgramMetrics.Size = new System.Drawing.Size(129, 13);
            this.lblProgramMetrics.TabIndex = 9;
            this.lblProgramMetrics.Text = "Program metrics database";
            // 
            // chkExtendedSiteInfo
            // 
            this.chkExtendedSiteInfo.AutoSize = true;
            this.chkExtendedSiteInfo.Location = new System.Drawing.Point(14, 130);
            this.chkExtendedSiteInfo.Name = "chkExtendedSiteInfo";
            this.chkExtendedSiteInfo.Size = new System.Drawing.Size(175, 17);
            this.chkExtendedSiteInfo.TabIndex = 8;
            this.chkExtendedSiteInfo.Text = "Import extended site information";
            this.chkExtendedSiteInfo.UseVisualStyleBackColor = true;
            this.chkExtendedSiteInfo.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // frmOpen
            // 
            this.frmOpen.FileName = "frmOpen";
            // 
            // frmImportCHaMPInfo
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(673, 229);
            this.Controls.Add(this.cmdBrowseProgramMetrics);
            this.Controls.Add(this.txtProgramMetrics);
            this.Controls.Add(this.lblProgramMetrics);
            this.Controls.Add(this.chkExtendedSiteInfo);
            this.Controls.Add(this.cmdBrowseSurveyDesign);
            this.Controls.Add(this.txtSurveyDesign);
            this.Controls.Add(this.lblSurveyDesign);
            this.Controls.Add(this.chkImportFish);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdBrowse);
            this.Controls.Add(this.txtDatabase);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(422, 267);
            this.Name = "frmImportCHaMPInfo";
            this.Text = "Import CHaMP Visit Information";
            this.Load += new System.EventHandler(this.frmImportCHaMPInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtDatabase;
        private System.Windows.Forms.Button cmdBrowse;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.CheckBox chkImportFish;
        private System.Windows.Forms.Button cmdBrowseSurveyDesign;
        private System.Windows.Forms.TextBox txtSurveyDesign;
        private System.Windows.Forms.Label lblSurveyDesign;
        private System.Windows.Forms.Button cmdBrowseProgramMetrics;
        private System.Windows.Forms.TextBox txtProgramMetrics;
        private System.Windows.Forms.Label lblProgramMetrics;
        private System.Windows.Forms.CheckBox chkExtendedSiteInfo;
        private System.Windows.Forms.OpenFileDialog frmOpen;
    }
}