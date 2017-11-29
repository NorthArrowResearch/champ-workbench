namespace CHaMPWorkbench.Data
{
    partial class frmAPIDownloadVisitFiles
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAPIDownloadVisitFiles));
            this.label1 = new System.Windows.Forms.Label();
            this.txtLocalFolder = new System.Windows.Forms.TextBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.treFiles = new System.Windows.Forms.TreeView();
            this.chkCreateDir = new System.Windows.Forms.CheckBox();
            this.chkOverwrite = new System.Windows.Forms.CheckBox();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.cmdBrowseLocal = new System.Windows.Forms.Button();
            this.progressOverall = new System.Windows.Forms.ProgressBar();
            this.grpProgress = new System.Windows.Forms.GroupBox();
            this.lblProgress2 = new System.Windows.Forms.Label();
            this.progressFile = new System.Windows.Forms.ProgressBar();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.lblSelectedVisits = new System.Windows.Forms.Label();
            this.grpProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Local folder";
            // 
            // txtLocalFolder
            // 
            this.txtLocalFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocalFolder.Location = new System.Drawing.Point(95, 43);
            this.txtLocalFolder.Name = "txtLocalFolder";
            this.txtLocalFolder.Size = new System.Drawing.Size(409, 20);
            this.txtLocalFolder.TabIndex = 1;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(458, 583);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 11;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(352, 583);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(100, 23);
            this.cmdOK.TabIndex = 10;
            this.cmdOK.Text = "Start Download";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // treFiles
            // 
            this.treFiles.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.treFiles.Location = new System.Drawing.Point(12, 130);
            this.treFiles.Name = "treFiles";
            this.treFiles.Size = new System.Drawing.Size(521, 228);
            this.treFiles.TabIndex = 6;
            this.treFiles.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treFiles_AfterCheck);
            // 
            // chkCreateDir
            // 
            this.chkCreateDir.AutoSize = true;
            this.chkCreateDir.Checked = true;
            this.chkCreateDir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateDir.Location = new System.Drawing.Point(95, 69);
            this.chkCreateDir.Name = "chkCreateDir";
            this.chkCreateDir.Size = new System.Drawing.Size(216, 17);
            this.chkCreateDir.TabIndex = 3;
            this.chkCreateDir.Text = "Create directories that don\'t already exist";
            this.chkCreateDir.UseVisualStyleBackColor = true;
            // 
            // chkOverwrite
            // 
            this.chkOverwrite.AutoSize = true;
            this.chkOverwrite.Location = new System.Drawing.Point(95, 94);
            this.chkOverwrite.Name = "chkOverwrite";
            this.chkOverwrite.Size = new System.Drawing.Size(130, 17);
            this.chkOverwrite.TabIndex = 4;
            this.chkOverwrite.Text = "Overwrite existing files";
            this.chkOverwrite.UseVisualStyleBackColor = true;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 583);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 12;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // cmdBrowseLocal
            // 
            this.cmdBrowseLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseLocal.Image = global::CHaMPWorkbench.Properties.Resources.BrowseFolder;
            this.cmdBrowseLocal.Location = new System.Drawing.Point(510, 42);
            this.cmdBrowseLocal.Name = "cmdBrowseLocal";
            this.cmdBrowseLocal.Size = new System.Drawing.Size(23, 23);
            this.cmdBrowseLocal.TabIndex = 2;
            this.cmdBrowseLocal.UseVisualStyleBackColor = true;
            this.cmdBrowseLocal.Click += new System.EventHandler(this.cmdBrowseLocal_Click);
            // 
            // progressOverall
            // 
            this.progressOverall.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressOverall.Location = new System.Drawing.Point(10, 24);
            this.progressOverall.Name = "progressOverall";
            this.progressOverall.Size = new System.Drawing.Size(499, 13);
            this.progressOverall.Step = 1;
            this.progressOverall.TabIndex = 11;
            // 
            // grpProgress
            // 
            this.grpProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpProgress.Controls.Add(this.lblProgress2);
            this.grpProgress.Controls.Add(this.progressFile);
            this.grpProgress.Controls.Add(this.txtProgress);
            this.grpProgress.Controls.Add(this.progressOverall);
            this.grpProgress.Location = new System.Drawing.Point(12, 364);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.Size = new System.Drawing.Size(521, 213);
            this.grpProgress.TabIndex = 9;
            this.grpProgress.TabStop = false;
            this.grpProgress.Text = "Progress";
            // 
            // lblProgress2
            // 
            this.lblProgress2.AutoSize = true;
            this.lblProgress2.Location = new System.Drawing.Point(10, 44);
            this.lblProgress2.Name = "lblProgress2";
            this.lblProgress2.Size = new System.Drawing.Size(23, 13);
            this.lblProgress2.TabIndex = 14;
            this.lblProgress2.Text = "File";
            // 
            // progressFile
            // 
            this.progressFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressFile.Location = new System.Drawing.Point(10, 60);
            this.progressFile.Name = "progressFile";
            this.progressFile.Size = new System.Drawing.Size(499, 14);
            this.progressFile.Step = 1;
            this.progressFile.TabIndex = 13;
            // 
            // txtProgress
            // 
            this.txtProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProgress.Location = new System.Drawing.Point(10, 80);
            this.txtProgress.Multiline = true;
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.ReadOnly = true;
            this.txtProgress.Size = new System.Drawing.Size(499, 127);
            this.txtProgress.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 114);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(89, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Files to download";
            // 
            // lblSelectedVisits
            // 
            this.lblSelectedVisits.AutoSize = true;
            this.lblSelectedVisits.Location = new System.Drawing.Point(12, 24);
            this.lblSelectedVisits.Name = "lblSelectedVisits";
            this.lblSelectedVisits.Size = new System.Drawing.Size(125, 13);
            this.lblSelectedVisits.TabIndex = 13;
            this.lblSelectedVisits.Text = "With XX selected Visits...";
            // 
            // frmAPIDownloadVisitFiles
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 618);
            this.Controls.Add(this.lblSelectedVisits);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.grpProgress);
            this.Controls.Add(this.cmdBrowseLocal);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.chkOverwrite);
            this.Controls.Add(this.chkCreateDir);
            this.Controls.Add(this.treFiles);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.txtLocalFolder);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(561, 220);
            this.Name = "frmAPIDownloadVisitFiles";
            this.Text = "Download Visit Data";
            this.Load += new System.EventHandler(this.frmFTPVisit_Load);
            this.grpProgress.ResumeLayout(false);
            this.grpProgress.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtLocalFolder;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.TreeView treFiles;
        private System.Windows.Forms.CheckBox chkCreateDir;
        private System.Windows.Forms.CheckBox chkOverwrite;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.Button cmdBrowseLocal;
        private System.Windows.Forms.ProgressBar progressOverall;
        private System.Windows.Forms.GroupBox grpProgress;
        private System.Windows.Forms.TextBox txtProgress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lblSelectedVisits;
        private System.Windows.Forms.Label lblProgress2;
        private System.Windows.Forms.ProgressBar progressFile;
    }
}