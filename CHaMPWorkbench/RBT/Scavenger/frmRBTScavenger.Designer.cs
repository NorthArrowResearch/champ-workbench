namespace CHaMPWorkbench
{
    partial class frmRBTScavenger
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
            this.chkLogs = new System.Windows.Forms.CheckBox();
            this.txtMatch = new System.Windows.Forms.TextBox();
            this.radMatch = new System.Windows.Forms.RadioButton();
            this.radAllFiles = new System.Windows.Forms.RadioButton();
            this.chkEmptyDB = new System.Windows.Forms.CheckBox();
            this.chkRecursive = new System.Windows.Forms.CheckBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.cmdStop = new System.Windows.Forms.Button();
            this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.txtLog = new System.Windows.Forms.TextBox();
            this.tTip = new System.Windows.Forms.ToolTip(this.components);
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.grpFolder = new System.Windows.Forms.GroupBox();
            this.cmdBrowseFolder = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.dlgDatabase = new System.Windows.Forms.OpenFileDialog();
            this.dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.cmdRun = new System.Windows.Forms.Button();
            this.cmdCloseCancel = new System.Windows.Forms.Button();
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.grpFolder.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkLogs
            // 
            this.chkLogs.AutoSize = true;
            this.chkLogs.Checked = true;
            this.chkLogs.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLogs.Location = new System.Drawing.Point(95, 139);
            this.chkLogs.Name = "chkLogs";
            this.chkLogs.Size = new System.Drawing.Size(141, 17);
            this.chkLogs.TabIndex = 6;
            this.chkLogs.Text = "Attempt to parse log files";
            this.chkLogs.UseVisualStyleBackColor = true;
            this.chkLogs.CheckedChanged += new System.EventHandler(this.chkLogs_CheckedChanged);
            // 
            // txtMatch
            // 
            this.txtMatch.Location = new System.Drawing.Point(115, 113);
            this.txtMatch.Name = "txtMatch";
            this.txtMatch.Size = new System.Drawing.Size(212, 20);
            this.txtMatch.TabIndex = 5;
            this.txtMatch.Text = "result*.xml";
            // 
            // radMatch
            // 
            this.radMatch.AutoSize = true;
            this.radMatch.Checked = true;
            this.radMatch.Location = new System.Drawing.Point(95, 90);
            this.radMatch.Name = "radMatch";
            this.radMatch.Size = new System.Drawing.Size(227, 17);
            this.radMatch.TabIndex = 4;
            this.radMatch.TabStop = true;
            this.radMatch.Text = "Just those that match the following pattern:";
            this.radMatch.UseVisualStyleBackColor = true;
            this.radMatch.CheckedChanged += new System.EventHandler(this.radMatch_CheckedChanged);
            // 
            // radAllFiles
            // 
            this.radAllFiles.AutoSize = true;
            this.radAllFiles.Location = new System.Drawing.Point(95, 67);
            this.radAllFiles.Name = "radAllFiles";
            this.radAllFiles.Size = new System.Drawing.Size(82, 17);
            this.radAllFiles.TabIndex = 3;
            this.radAllFiles.Text = "All XML files";
            this.radAllFiles.UseVisualStyleBackColor = true;
            // 
            // chkEmptyDB
            // 
            this.chkEmptyDB.AutoSize = true;
            this.chkEmptyDB.Location = new System.Drawing.Point(12, 205);
            this.chkEmptyDB.Name = "chkEmptyDB";
            this.chkEmptyDB.Size = new System.Drawing.Size(251, 17);
            this.chkEmptyDB.TabIndex = 3;
            this.chkEmptyDB.Text = "Empty metric database tables before processing";
            this.chkEmptyDB.UseVisualStyleBackColor = true;
            // 
            // chkRecursive
            // 
            this.chkRecursive.AutoSize = true;
            this.chkRecursive.Checked = true;
            this.chkRecursive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRecursive.Location = new System.Drawing.Point(95, 44);
            this.chkRecursive.Name = "chkRecursive";
            this.chkRecursive.Size = new System.Drawing.Size(74, 17);
            this.chkRecursive.TabIndex = 2;
            this.chkRecursive.Text = "Recursive";
            this.chkRecursive.UseVisualStyleBackColor = true;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(6, 22);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(83, 13);
            this.Label1.TabIndex = 0;
            this.Label1.Text = "Top level folder:";
            // 
            // cmdStop
            // 
            this.cmdStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdStop.Location = new System.Drawing.Point(585, 231);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(75, 23);
            this.cmdStop.TabIndex = 16;
            this.cmdStop.Text = "Stop";
            this.cmdStop.UseVisualStyleBackColor = true;
            this.cmdStop.Click += new System.EventHandler(this.cancelAsyncButton_Click);
            // 
            // BackgroundWorker1
            // 
            this.BackgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.BackgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.BackgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // txtLog
            // 
            this.txtLog.Location = new System.Drawing.Point(115, 161);
            this.txtLog.Name = "txtLog";
            this.txtLog.Size = new System.Drawing.Size(212, 20);
            this.txtLog.TabIndex = 7;
            this.txtLog.Text = "log*.xml";
            // 
            // txtFolder
            // 
            this.txtFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtFolder.Location = new System.Drawing.Point(95, 18);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(466, 20);
            this.txtFolder.TabIndex = 0;
            // 
            // grpFolder
            // 
            this.grpFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpFolder.Controls.Add(this.txtLog);
            this.grpFolder.Controls.Add(this.chkLogs);
            this.grpFolder.Controls.Add(this.txtMatch);
            this.grpFolder.Controls.Add(this.radMatch);
            this.grpFolder.Controls.Add(this.radAllFiles);
            this.grpFolder.Controls.Add(this.chkRecursive);
            this.grpFolder.Controls.Add(this.Label1);
            this.grpFolder.Controls.Add(this.txtFolder);
            this.grpFolder.Controls.Add(this.cmdBrowseFolder);
            this.grpFolder.Location = new System.Drawing.Point(12, 12);
            this.grpFolder.Name = "grpFolder";
            this.grpFolder.Size = new System.Drawing.Size(648, 187);
            this.grpFolder.TabIndex = 11;
            this.grpFolder.TabStop = false;
            this.grpFolder.Text = "Input Folder && Files";
            // 
            // cmdBrowseFolder
            // 
            this.cmdBrowseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseFolder.Location = new System.Drawing.Point(567, 17);
            this.cmdBrowseFolder.Name = "cmdBrowseFolder";
            this.cmdBrowseFolder.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseFolder.TabIndex = 1;
            this.cmdBrowseFolder.Text = "Browse";
            this.cmdBrowseFolder.UseVisualStyleBackColor = true;
            this.cmdBrowseFolder.Click += new System.EventHandler(this.cmdBrowseFolder_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 273);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 14;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // dlgDatabase
            // 
            this.dlgDatabase.FileName = "OpenFileDialog1";
            // 
            // cmdRun
            // 
            this.cmdRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRun.Location = new System.Drawing.Point(504, 231);
            this.cmdRun.Name = "cmdRun";
            this.cmdRun.Size = new System.Drawing.Size(75, 23);
            this.cmdRun.TabIndex = 15;
            this.cmdRun.Text = "Run";
            this.cmdRun.UseVisualStyleBackColor = true;
            this.cmdRun.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCloseCancel
            // 
            this.cmdCloseCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCloseCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCloseCancel.Location = new System.Drawing.Point(588, 268);
            this.cmdCloseCancel.Name = "cmdCloseCancel";
            this.cmdCloseCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCloseCancel.TabIndex = 10;
            this.cmdCloseCancel.Text = "Close";
            this.cmdCloseCancel.UseVisualStyleBackColor = true;
            // 
            // prgBar
            // 
            this.prgBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgBar.Location = new System.Drawing.Point(12, 231);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(486, 23);
            this.prgBar.Step = 1;
            this.prgBar.TabIndex = 13;
            this.prgBar.Visible = false;
            // 
            // frmRBTScavenger
            // 
            this.AcceptButton = this.cmdCloseCancel;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCloseCancel;
            this.ClientSize = new System.Drawing.Size(672, 308);
            this.Controls.Add(this.cmdStop);
            this.Controls.Add(this.chkEmptyDB);
            this.Controls.Add(this.grpFolder);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdRun);
            this.Controls.Add(this.cmdCloseCancel);
            this.Controls.Add(this.prgBar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRBTScavenger";
            this.Text = "RBT Result Scavenger";
            this.Load += new System.EventHandler(this.frmRBTScavenger_Load);
            this.grpFolder.ResumeLayout(false);
            this.grpFolder.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.CheckBox chkLogs;
        internal System.Windows.Forms.TextBox txtMatch;
        internal System.Windows.Forms.RadioButton radMatch;
        internal System.Windows.Forms.RadioButton radAllFiles;
        internal System.Windows.Forms.CheckBox chkEmptyDB;
        internal System.Windows.Forms.CheckBox chkRecursive;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.Button cmdStop;
        internal System.ComponentModel.BackgroundWorker BackgroundWorker1;
        internal System.Windows.Forms.TextBox txtLog;
        internal System.Windows.Forms.ToolTip tTip;
        internal System.Windows.Forms.TextBox txtFolder;
        internal System.Windows.Forms.GroupBox grpFolder;
        internal System.Windows.Forms.Button cmdBrowseFolder;
        internal System.Windows.Forms.Button cmdHelp;
        internal System.Windows.Forms.OpenFileDialog dlgDatabase;
        internal System.Windows.Forms.FolderBrowserDialog dlgFolder;
        internal System.Windows.Forms.Button cmdRun;
        internal System.Windows.Forms.Button cmdCloseCancel;
        internal System.Windows.Forms.ProgressBar prgBar;
    }
}