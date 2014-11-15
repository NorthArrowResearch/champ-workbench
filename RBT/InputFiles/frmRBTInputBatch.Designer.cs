namespace CHaMPWorkbench.RBTInputFile
{
    partial class frmRBTInputBatch
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkIncludeOtherVisits = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.chkChangeDetection = new System.Windows.Forms.CheckBox();
            this.chkCalculateMetrics = new System.Windows.Forms.CheckBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtMonitoringDataFolder = new System.Windows.Forms.TextBox();
            this.cmdBrowseFolder = new System.Windows.Forms.Button();
            this.txtInputFileRoot = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.lstFieldSeasons = new System.Windows.Forms.CheckedListBox();
            this.lblBatchName = new System.Windows.Forms.Label();
            this.txtBatch = new System.Windows.Forms.TextBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ucConfig = new CHaMPWorkbench.RBTConfiguration();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ucRBTChangeDetection1 = new CHaMPWorkbench.RBT.InputFiles.ucRBTChangeDetection();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.chkRequireWSTIN = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(856, 575);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkRequireWSTIN);
            this.tabPage1.Controls.Add(this.chkIncludeOtherVisits);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.txtOutputFolder);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.GroupBox1);
            this.tabPage1.Controls.Add(this.Label2);
            this.tabPage1.Controls.Add(this.txtMonitoringDataFolder);
            this.tabPage1.Controls.Add(this.cmdBrowseFolder);
            this.tabPage1.Controls.Add(this.txtInputFileRoot);
            this.tabPage1.Controls.Add(this.Label3);
            this.tabPage1.Controls.Add(this.Label5);
            this.tabPage1.Controls.Add(this.lstFieldSeasons);
            this.tabPage1.Controls.Add(this.lblBatchName);
            this.tabPage1.Controls.Add(this.txtBatch);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(848, 549);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Site and Visit";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkIncludeOtherVisits
            // 
            this.chkIncludeOtherVisits.AutoSize = true;
            this.chkIncludeOtherVisits.Checked = true;
            this.chkIncludeOtherVisits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkIncludeOtherVisits.Location = new System.Drawing.Point(184, 321);
            this.chkIncludeOtherVisits.Name = "chkIncludeOtherVisits";
            this.chkIncludeOtherVisits.Size = new System.Drawing.Size(114, 17);
            this.chkIncludeOtherVisits.TabIndex = 13;
            this.chkIncludeOtherVisits.Text = "Include other visits";
            this.chkIncludeOtherVisits.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(11, 77);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(167, 13);
            this.label1.TabIndex = 5;
            this.label1.Text = "Output folder (\"InputOutputFiles\"):";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.Location = new System.Drawing.Point(184, 73);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(572, 20);
            this.txtOutputFolder.TabIndex = 6;
            this.txtOutputFolder.Text = "C:\\CHaMP\\RBTInputOutputFiles";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(762, 72);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 7;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.cmdBrowseFolder_Click);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.chkChangeDetection);
            this.GroupBox1.Controls.Add(this.chkCalculateMetrics);
            this.GroupBox1.Location = new System.Drawing.Point(184, 241);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(239, 68);
            this.GroupBox1.TabIndex = 12;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Target Visit Attributes";
            // 
            // chkChangeDetection
            // 
            this.chkChangeDetection.AutoSize = true;
            this.chkChangeDetection.Checked = true;
            this.chkChangeDetection.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkChangeDetection.Location = new System.Drawing.Point(11, 43);
            this.chkChangeDetection.Name = "chkChangeDetection";
            this.chkChangeDetection.Size = new System.Drawing.Size(112, 17);
            this.chkChangeDetection.TabIndex = 1;
            this.chkChangeDetection.Text = "Change Detection";
            this.chkChangeDetection.UseVisualStyleBackColor = true;
            // 
            // chkCalculateMetrics
            // 
            this.chkCalculateMetrics.AutoSize = true;
            this.chkCalculateMetrics.Checked = true;
            this.chkCalculateMetrics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCalculateMetrics.Location = new System.Drawing.Point(11, 20);
            this.chkCalculateMetrics.Name = "chkCalculateMetrics";
            this.chkCalculateMetrics.Size = new System.Drawing.Size(107, 17);
            this.chkCalculateMetrics.TabIndex = 0;
            this.chkCalculateMetrics.Text = "Calculate Metrics";
            this.chkCalculateMetrics.UseVisualStyleBackColor = true;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(40, 46);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(138, 13);
            this.Label2.TabIndex = 2;
            this.Label2.Text = "Parent folder (\"Monitoring\"):";
            // 
            // txtMonitoringDataFolder
            // 
            this.txtMonitoringDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMonitoringDataFolder.Location = new System.Drawing.Point(184, 42);
            this.txtMonitoringDataFolder.Name = "txtMonitoringDataFolder";
            this.txtMonitoringDataFolder.Size = new System.Drawing.Size(572, 20);
            this.txtMonitoringDataFolder.TabIndex = 3;
            this.txtMonitoringDataFolder.Text = "E:\\Local Cloud\\Shared\\CHaMP\\MonitoringData";
            // 
            // cmdBrowseFolder
            // 
            this.cmdBrowseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseFolder.Location = new System.Drawing.Point(762, 41);
            this.cmdBrowseFolder.Name = "cmdBrowseFolder";
            this.cmdBrowseFolder.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseFolder.TabIndex = 4;
            this.cmdBrowseFolder.Text = "Browse";
            this.cmdBrowseFolder.UseVisualStyleBackColor = true;
            this.cmdBrowseFolder.Click += new System.EventHandler(this.cmdBrowseFolder_Click);
            // 
            // txtInputFileRoot
            // 
            this.txtInputFileRoot.Location = new System.Drawing.Point(184, 109);
            this.txtInputFileRoot.Name = "txtInputFileRoot";
            this.txtInputFileRoot.Size = new System.Drawing.Size(239, 20);
            this.txtInputFileRoot.TabIndex = 9;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(104, 141);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(74, 13);
            this.Label3.TabIndex = 10;
            this.Label3.Text = "Field seasons:";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(78, 113);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(100, 13);
            this.Label5.TabIndex = 8;
            this.Label5.Text = "Input file root name:";
            // 
            // lstFieldSeasons
            // 
            this.lstFieldSeasons.FormattingEnabled = true;
            this.lstFieldSeasons.Location = new System.Drawing.Point(184, 141);
            this.lstFieldSeasons.Name = "lstFieldSeasons";
            this.lstFieldSeasons.Size = new System.Drawing.Size(239, 94);
            this.lstFieldSeasons.TabIndex = 11;
            // 
            // lblBatchName
            // 
            this.lblBatchName.AutoSize = true;
            this.lblBatchName.Location = new System.Drawing.Point(111, 17);
            this.lblBatchName.Name = "lblBatchName";
            this.lblBatchName.Size = new System.Drawing.Size(67, 13);
            this.lblBatchName.TabIndex = 0;
            this.lblBatchName.Text = "Batch name:";
            // 
            // txtBatch
            // 
            this.txtBatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtBatch.Location = new System.Drawing.Point(184, 13);
            this.txtBatch.MaxLength = 255;
            this.txtBatch.Name = "txtBatch";
            this.txtBatch.Size = new System.Drawing.Size(181, 20);
            this.txtBatch.TabIndex = 1;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ucConfig);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(848, 549);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "RBT Configuration";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ucConfig
            // 
            this.ucConfig.Location = new System.Drawing.Point(6, 6);
            this.ucConfig.Name = "ucConfig";
            this.ucConfig.Size = new System.Drawing.Size(835, 547);
            this.ucConfig.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ucRBTChangeDetection1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(848, 549);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Change Detection";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ucRBTChangeDetection1
            // 
            this.ucRBTChangeDetection1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucRBTChangeDetection1.Location = new System.Drawing.Point(3, 3);
            this.ucRBTChangeDetection1.Name = "ucRBTChangeDetection1";
            this.ucRBTChangeDetection1.Size = new System.Drawing.Size(842, 543);
            this.ucRBTChangeDetection1.TabIndex = 0;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(789, 597);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(708, 597);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // chkRequireWSTIN
            // 
            this.chkRequireWSTIN.AutoSize = true;
            this.chkRequireWSTIN.Checked = true;
            this.chkRequireWSTIN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRequireWSTIN.Location = new System.Drawing.Point(184, 344);
            this.chkRequireWSTIN.Name = "chkRequireWSTIN";
            this.chkRequireWSTIN.Size = new System.Drawing.Size(225, 17);
            this.chkRequireWSTIN.TabIndex = 14;
            this.chkRequireWSTIN.Text = "Require visits to have a water surface TIN";
            this.chkRequireWSTIN.UseVisualStyleBackColor = true;
            // 
            // frmRBTInputBatch
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(876, 632);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRBTInputBatch";
            this.Text = "Batch RBT Input File Builder";
            this.Load += new System.EventHandler(this.frmRBTInputBatch_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private RBTConfiguration ucConfig;
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.CheckBox chkChangeDetection;
        internal System.Windows.Forms.CheckBox chkCalculateMetrics;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtMonitoringDataFolder;
        internal System.Windows.Forms.Button cmdBrowseFolder;
        internal System.Windows.Forms.TextBox txtInputFileRoot;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.CheckedListBox lstFieldSeasons;
        internal System.Windows.Forms.Label lblBatchName;
        internal System.Windows.Forms.TextBox txtBatch;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox txtOutputFolder;
        internal System.Windows.Forms.Button button1;
        private System.Windows.Forms.CheckBox chkIncludeOtherVisits;
        private System.Windows.Forms.TabPage tabPage3;
        private RBT.InputFiles.ucRBTChangeDetection ucRBTChangeDetection1;
        private System.Windows.Forms.CheckBox chkRequireWSTIN;
    }
}