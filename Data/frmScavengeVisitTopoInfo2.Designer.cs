namespace CHaMPWorkbench.Data
{
    partial class frmScavengeVisitTopoInfo2
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmScavengeVisitTopoInfo2));
            this.label1 = new System.Windows.Forms.Label();
            this.chkSetNull = new System.Windows.Forms.CheckBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtMonitoringDataFolder = new System.Windows.Forms.TextBox();
            this.cmdBrowseFolder = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoWSYV = new System.Windows.Forms.RadioButton();
            this.rdoWYSV = new System.Windows.Forms.RadioButton();
            this.rdoYWS = new System.Windows.Forms.RadioButton();
            this.grpStatus = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.pgrProgress = new System.Windows.Forms.ProgressBar();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.grpStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(598, 36);
            this.label1.TabIndex = 0;
            this.label1.Text = resources.GetString("label1.Text");
            // 
            // chkSetNull
            // 
            this.chkSetNull.AutoSize = true;
            this.chkSetNull.Location = new System.Drawing.Point(12, 109);
            this.chkSetNull.Name = "chkSetNull";
            this.chkSetNull.Size = new System.Drawing.Size(222, 17);
            this.chkSetNull.TabIndex = 7;
            this.chkSetNull.Text = "Clear all visit topo fields before processing";
            this.chkSetNull.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(536, 349);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(12, 82);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(67, 13);
            this.Label2.TabIndex = 4;
            this.Label2.Text = "Parent folder";
            // 
            // txtMonitoringDataFolder
            // 
            this.txtMonitoringDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMonitoringDataFolder.Location = new System.Drawing.Point(85, 78);
            this.txtMonitoringDataFolder.Name = "txtMonitoringDataFolder";
            this.txtMonitoringDataFolder.Size = new System.Drawing.Size(445, 20);
            this.txtMonitoringDataFolder.TabIndex = 5;
            // 
            // cmdBrowseFolder
            // 
            this.cmdBrowseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseFolder.Location = new System.Drawing.Point(536, 77);
            this.cmdBrowseFolder.Name = "cmdBrowseFolder";
            this.cmdBrowseFolder.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseFolder.TabIndex = 6;
            this.cmdBrowseFolder.Text = "Browse";
            this.cmdBrowseFolder.UseVisualStyleBackColor = true;
            this.cmdBrowseFolder.Click += new System.EventHandler(this.cmdBrowseFolder_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(455, 349);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "Run";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 37);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(584, 34);
            this.label3.TabIndex = 1;
            this.label3.Text = "A folder is considered to contain valid topo data when it contains a **folder** p" +
    "ossessing a name ending with *.gdb (i.e. a file geodatabase) and a folder posses" +
    "sing a name starting with TIN.";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoWSYV);
            this.groupBox1.Controls.Add(this.rdoWYSV);
            this.groupBox1.Controls.Add(this.rdoYWS);
            this.groupBox1.Location = new System.Drawing.Point(12, 137);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(598, 97);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Folder Structure Under Parent Folder";
            // 
            // rdoWSYV
            // 
            this.rdoWSYV.AutoSize = true;
            this.rdoWSYV.Enabled = false;
            this.rdoWSYV.Location = new System.Drawing.Point(15, 68);
            this.rdoWSYV.Name = "rdoWSYV";
            this.rdoWSYV.Size = new System.Drawing.Size(190, 17);
            this.rdoWSYV.TabIndex = 2;
            this.rdoWSYV.TabStop = true;
            this.rdoWSYV.Text = "Watershed\\Site\\Field Season\\Visit";
            this.rdoWSYV.UseVisualStyleBackColor = true;
            // 
            // rdoWYSV
            // 
            this.rdoWYSV.AutoSize = true;
            this.rdoWYSV.Enabled = false;
            this.rdoWYSV.Location = new System.Drawing.Point(15, 44);
            this.rdoWYSV.Name = "rdoWYSV";
            this.rdoWYSV.Size = new System.Drawing.Size(190, 17);
            this.rdoWYSV.TabIndex = 1;
            this.rdoWYSV.TabStop = true;
            this.rdoWYSV.Text = "Watershed\\Field Season\\Site\\Visit";
            this.rdoWYSV.UseVisualStyleBackColor = true;
            // 
            // rdoYWS
            // 
            this.rdoYWS.AutoSize = true;
            this.rdoYWS.Checked = true;
            this.rdoYWS.Location = new System.Drawing.Point(15, 20);
            this.rdoYWS.Name = "rdoYWS";
            this.rdoYWS.Size = new System.Drawing.Size(190, 17);
            this.rdoYWS.TabIndex = 0;
            this.rdoYWS.TabStop = true;
            this.rdoYWS.Text = "Field Season\\Watersehd\\Site\\Visit";
            this.rdoYWS.UseVisualStyleBackColor = true;
            // 
            // grpStatus
            // 
            this.grpStatus.Controls.Add(this.lblStatus);
            this.grpStatus.Controls.Add(this.pgrProgress);
            this.grpStatus.Location = new System.Drawing.Point(12, 247);
            this.grpStatus.Name = "grpStatus";
            this.grpStatus.Size = new System.Drawing.Size(598, 96);
            this.grpStatus.TabIndex = 11;
            this.grpStatus.TabStop = false;
            this.grpStatus.Text = "Status";
            this.grpStatus.Visible = false;
            // 
            // lblStatus
            // 
            this.lblStatus.Location = new System.Drawing.Point(15, 50);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(577, 43);
            this.lblStatus.TabIndex = 1;
            this.lblStatus.Text = "lblStatus";
            // 
            // pgrProgress
            // 
            this.pgrProgress.Location = new System.Drawing.Point(15, 20);
            this.pgrProgress.Name = "pgrProgress";
            this.pgrProgress.Size = new System.Drawing.Size(577, 23);
            this.pgrProgress.TabIndex = 0;
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // frmScavengeVisitTopoInfo2
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(623, 384);
            this.Controls.Add(this.grpStatus);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkSetNull);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtMonitoringDataFolder);
            this.Controls.Add(this.cmdBrowseFolder);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScavengeVisitTopoInfo2";
            this.ShowIcon = false;
            this.Text = "Update Topo and Hydro Paths In Workbench Database";
            this.Load += new System.EventHandler(this.frmScavengeVisitTopoInfo2_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.grpStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkSetNull;
        private System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtMonitoringDataFolder;
        internal System.Windows.Forms.Button cmdBrowseFolder;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoWSYV;
        private System.Windows.Forms.RadioButton rdoWYSV;
        private System.Windows.Forms.RadioButton rdoYWS;
        private System.Windows.Forms.GroupBox grpStatus;
        private System.Windows.Forms.Label lblStatus;
        private System.Windows.Forms.ProgressBar pgrProgress;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}