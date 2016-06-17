﻿namespace CHaMPWorkbench.Data
{
    partial class frmFTPVisit
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFTPVisit));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtLocalFolder = new System.Windows.Forms.TextBox();
            this.txtRemote = new System.Windows.Forms.TextBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.treFiles = new System.Windows.Forms.TreeView();
            this.chkCreateDir = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.cmdBrowseLocal = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.grpProgress = new System.Windows.Forms.GroupBox();
            this.txtProgress = new System.Windows.Forms.TextBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.label3 = new System.Windows.Forms.Label();
            this.rdoCHaMP = new System.Windows.Forms.RadioButton();
            this.rdoAEM = new System.Windows.Forms.RadioButton();
            this.grpProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(27, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Local folder";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(73, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Remote folder";
            // 
            // txtLocalFolder
            // 
            this.txtLocalFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtLocalFolder.Location = new System.Drawing.Point(95, 36);
            this.txtLocalFolder.Name = "txtLocalFolder";
            this.txtLocalFolder.Size = new System.Drawing.Size(409, 20);
            this.txtLocalFolder.TabIndex = 2;
            // 
            // txtRemote
            // 
            this.txtRemote.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRemote.Location = new System.Drawing.Point(95, 64);
            this.txtRemote.Name = "txtRemote";
            this.txtRemote.ReadOnly = true;
            this.txtRemote.Size = new System.Drawing.Size(438, 20);
            this.txtRemote.TabIndex = 3;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(458, 583);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(352, 583);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(100, 23);
            this.cmdOK.TabIndex = 5;
            this.cmdOK.Text = "Start Download";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // treFiles
            // 
            this.treFiles.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.treFiles.Location = new System.Drawing.Point(12, 144);
            this.treFiles.Name = "treFiles";
            this.treFiles.Size = new System.Drawing.Size(521, 251);
            this.treFiles.TabIndex = 6;
            this.treFiles.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treFiles_AfterCheck);
            // 
            // chkCreateDir
            // 
            this.chkCreateDir.AutoSize = true;
            this.chkCreateDir.Checked = true;
            this.chkCreateDir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreateDir.Location = new System.Drawing.Point(95, 92);
            this.chkCreateDir.Name = "chkCreateDir";
            this.chkCreateDir.Size = new System.Drawing.Size(216, 17);
            this.chkCreateDir.TabIndex = 7;
            this.chkCreateDir.Text = "Create directories that don\'t already exist";
            this.chkCreateDir.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(95, 117);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(130, 17);
            this.checkBox1.TabIndex = 8;
            this.checkBox1.Text = "Overwrite existing files";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 583);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 9;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // cmdBrowseLocal
            // 
            this.cmdBrowseLocal.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseLocal.Image = global::CHaMPWorkbench.Properties.Resources.BrowseFolder;
            this.cmdBrowseLocal.Location = new System.Drawing.Point(510, 35);
            this.cmdBrowseLocal.Name = "cmdBrowseLocal";
            this.cmdBrowseLocal.Size = new System.Drawing.Size(23, 23);
            this.cmdBrowseLocal.TabIndex = 10;
            this.cmdBrowseLocal.UseVisualStyleBackColor = true;
            this.cmdBrowseLocal.Click += new System.EventHandler(this.cmdBrowseLocal_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.progressBar1.Location = new System.Drawing.Point(10, 24);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(499, 13);
            this.progressBar1.TabIndex = 11;
            // 
            // grpProgress
            // 
            this.grpProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpProgress.Controls.Add(this.txtProgress);
            this.grpProgress.Controls.Add(this.progressBar1);
            this.grpProgress.Location = new System.Drawing.Point(12, 401);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.Size = new System.Drawing.Size(521, 176);
            this.grpProgress.TabIndex = 12;
            this.grpProgress.TabStop = false;
            this.grpProgress.Text = "Progress";
            // 
            // txtProgress
            // 
            this.txtProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtProgress.Location = new System.Drawing.Point(10, 43);
            this.txtProgress.Multiline = true;
            this.txtProgress.Name = "txtProgress";
            this.txtProgress.ReadOnly = true;
            this.txtProgress.Size = new System.Drawing.Size(499, 127);
            this.txtProgress.TabIndex = 12;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(43, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(46, 13);
            this.label3.TabIndex = 13;
            this.label3.Text = "Program";
            // 
            // rdoCHaMP
            // 
            this.rdoCHaMP.AutoSize = true;
            this.rdoCHaMP.Checked = true;
            this.rdoCHaMP.Location = new System.Drawing.Point(95, 11);
            this.rdoCHaMP.Name = "rdoCHaMP";
            this.rdoCHaMP.Size = new System.Drawing.Size(62, 17);
            this.rdoCHaMP.TabIndex = 14;
            this.rdoCHaMP.TabStop = true;
            this.rdoCHaMP.Text = "CHaMP";
            this.rdoCHaMP.UseVisualStyleBackColor = true;
            // 
            // rdoAEM
            // 
            this.rdoAEM.AutoSize = true;
            this.rdoAEM.Location = new System.Drawing.Point(168, 11);
            this.rdoAEM.Name = "rdoAEM";
            this.rdoAEM.Size = new System.Drawing.Size(48, 17);
            this.rdoAEM.TabIndex = 15;
            this.rdoAEM.TabStop = true;
            this.rdoAEM.Text = "AEM";
            this.rdoAEM.UseVisualStyleBackColor = true;
            this.rdoAEM.CheckedChanged += new System.EventHandler(this.rdoAEM_CheckedChanged);
            // 
            // frmFTPVisit
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(545, 618);
            this.Controls.Add(this.rdoAEM);
            this.Controls.Add(this.rdoCHaMP);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.grpProgress);
            this.Controls.Add(this.cmdBrowseLocal);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.chkCreateDir);
            this.Controls.Add(this.treFiles);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.txtRemote);
            this.Controls.Add(this.txtLocalFolder);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(561, 220);
            this.Name = "frmFTPVisit";
            this.Text = "Download Visit Data";
            this.Load += new System.EventHandler(this.frmFTPVisit_Load);
            this.grpProgress.ResumeLayout(false);
            this.grpProgress.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtLocalFolder;
        private System.Windows.Forms.TextBox txtRemote;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.TreeView treFiles;
        private System.Windows.Forms.CheckBox chkCreateDir;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.Button cmdBrowseLocal;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.GroupBox grpProgress;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.TextBox txtProgress;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.RadioButton rdoCHaMP;
        private System.Windows.Forms.RadioButton rdoAEM;
    }
}