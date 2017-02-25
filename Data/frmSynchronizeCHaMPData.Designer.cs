namespace CHaMPWorkbench.Data
{
    partial class frmSynchronizeCHaMPData
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSynchronizeCHaMPData));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.grpPrograms = new System.Windows.Forms.GroupBox();
            this.lstPrograms = new System.Windows.Forms.CheckedListBox();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.grpProgress = new System.Windows.Forms.GroupBox();
            this.lblCurrentProcess = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.pgrBar = new System.Windows.Forms.ProgressBar();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstWatersheds = new System.Windows.Forms.CheckedListBox();
            this.cmsWatersheds = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grpPrograms.SuspendLayout();
            this.grpProgress.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.cmsWatersheds.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(384, 235);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 4;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(303, 235);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 3;
            this.cmdOK.Text = "Synchonize";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // grpPrograms
            // 
            this.grpPrograms.Controls.Add(this.lstPrograms);
            this.grpPrograms.Location = new System.Drawing.Point(13, 8);
            this.grpPrograms.Name = "grpPrograms";
            this.grpPrograms.Size = new System.Drawing.Size(220, 142);
            this.grpPrograms.TabIndex = 0;
            this.grpPrograms.TabStop = false;
            this.grpPrograms.Text = "Programs to Synchronize";
            // 
            // lstPrograms
            // 
            this.lstPrograms.CheckOnClick = true;
            this.lstPrograms.FormattingEnabled = true;
            this.lstPrograms.Location = new System.Drawing.Point(10, 21);
            this.lstPrograms.Name = "lstPrograms";
            this.lstPrograms.Size = new System.Drawing.Size(204, 109);
            this.lstPrograms.TabIndex = 0;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(13, 235);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 5;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // grpProgress
            // 
            this.grpProgress.Controls.Add(this.lblCurrentProcess);
            this.grpProgress.Controls.Add(this.label1);
            this.grpProgress.Controls.Add(this.pgrBar);
            this.grpProgress.Location = new System.Drawing.Point(13, 158);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.Size = new System.Drawing.Size(446, 67);
            this.grpProgress.TabIndex = 2;
            this.grpProgress.TabStop = false;
            this.grpProgress.Text = "Progress";
            // 
            // lblCurrentProcess
            // 
            this.lblCurrentProcess.Location = new System.Drawing.Point(101, 42);
            this.lblCurrentProcess.Name = "lblCurrentProcess";
            this.lblCurrentProcess.Size = new System.Drawing.Size(339, 23);
            this.lblCurrentProcess.TabIndex = 2;
            this.lblCurrentProcess.Text = "lblCurrentProcess";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(84, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Current process:";
            // 
            // pgrBar
            // 
            this.pgrBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pgrBar.Location = new System.Drawing.Point(10, 19);
            this.pgrBar.Name = "pgrBar";
            this.pgrBar.Size = new System.Drawing.Size(430, 16);
            this.pgrBar.TabIndex = 0;
            // 
            // bgWorker
            // 
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorker_ProgressChanged);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstWatersheds);
            this.groupBox1.Location = new System.Drawing.Point(239, 8);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(220, 142);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Watersheds to Synchronize";
            // 
            // lstWatersheds
            // 
            this.lstWatersheds.CheckOnClick = true;
            this.lstWatersheds.ContextMenuStrip = this.cmsWatersheds;
            this.lstWatersheds.FormattingEnabled = true;
            this.lstWatersheds.Location = new System.Drawing.Point(10, 21);
            this.lstWatersheds.Name = "lstWatersheds";
            this.lstWatersheds.Size = new System.Drawing.Size(204, 109);
            this.lstWatersheds.TabIndex = 0;
            // 
            // cmsWatersheds
            // 
            this.cmsWatersheds.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem});
            this.cmsWatersheds.Name = "cmsWatersheds";
            this.cmsWatersheds.Size = new System.Drawing.Size(138, 48);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.AllNoneWatershedsClick);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.AllNoneWatershedsClick);
            // 
            // frmSynchronizeCHaMPData
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(469, 270);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.grpProgress);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.grpPrograms);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmSynchronizeCHaMPData";
            this.Text = "Synchronize CHaMP Data";
            this.Load += new System.EventHandler(this.frmSynchronizeCHaMPData_Load);
            this.grpPrograms.ResumeLayout(false);
            this.grpProgress.ResumeLayout(false);
            this.grpProgress.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.cmsWatersheds.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.GroupBox grpPrograms;
        private System.Windows.Forms.CheckedListBox lstPrograms;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.GroupBox grpProgress;
        private System.Windows.Forms.Label lblCurrentProcess;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ProgressBar pgrBar;
        private System.ComponentModel.BackgroundWorker bgWorker;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox lstWatersheds;
        private System.Windows.Forms.ContextMenuStrip cmsWatersheds;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
    }
}