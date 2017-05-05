namespace CHaMPWorkbench.Habitat
{
    partial class frmScavengeHabitatResults
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
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmdBrowseProject = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHabitatModelFolder = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCSVFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdBrowseCSV = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.rdoDB = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoCSV = new System.Windows.Forms.RadioButton();
            this.panelCSV = new System.Windows.Forms.Panel();
            this.cmdStop = new System.Windows.Forms.Button();
            this.cmdRun = new System.Windows.Forms.Button();
            this.prgBar = new System.Windows.Forms.ProgressBar();
            this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            this.panelCSV.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(399, 209);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 13;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            this.cmdClose.Click += new System.EventHandler(this.cmdClose_Click);
            // 
            // cmdBrowseProject
            // 
            this.cmdBrowseProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseProject.Location = new System.Drawing.Point(399, 45);
            this.cmdBrowseProject.Name = "cmdBrowseProject";
            this.cmdBrowseProject.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseProject.TabIndex = 5;
            this.cmdBrowseProject.Text = "Browse";
            this.cmdBrowseProject.UseVisualStyleBackColor = true;
            this.cmdBrowseProject.Click += new System.EventHandler(this.cmdBrowseProject_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(26, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(91, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Habitat root folder";
            // 
            // txtHabitatModelFolder
            // 
            this.txtHabitatModelFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHabitatModelFolder.Location = new System.Drawing.Point(120, 46);
            this.txtHabitatModelFolder.Name = "txtHabitatModelFolder";
            this.txtHabitatModelFolder.Size = new System.Drawing.Size(272, 20);
            this.txtHabitatModelFolder.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(463, 30);
            this.label2.TabIndex = 5;
            this.label2.Text = "This tool reads all simulation result values from a habitat results file and writ" +
    "es them to either the Access DB or a CSV text file.";
            // 
            // txtCSVFile
            // 
            this.txtCSVFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCSVFile.Location = new System.Drawing.Point(102, 7);
            this.txtCSVFile.Name = "txtCSVFile";
            this.txtCSVFile.Size = new System.Drawing.Size(272, 20);
            this.txtCSVFile.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Output CSV file";
            // 
            // cmdBrowseCSV
            // 
            this.cmdBrowseCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseCSV.Location = new System.Drawing.Point(380, 6);
            this.cmdBrowseCSV.Name = "cmdBrowseCSV";
            this.cmdBrowseCSV.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseCSV.TabIndex = 9;
            this.cmdBrowseCSV.Text = "Browse";
            this.cmdBrowseCSV.UseVisualStyleBackColor = true;
            this.cmdBrowseCSV.Click += new System.EventHandler(this.cmdBrowseCSV_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 209);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 12;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // rdoDB
            // 
            this.rdoDB.AutoSize = true;
            this.rdoDB.Checked = true;
            this.rdoDB.Location = new System.Drawing.Point(13, 19);
            this.rdoDB.Name = "rdoDB";
            this.rdoDB.Size = new System.Drawing.Size(93, 17);
            this.rdoDB.TabIndex = 6;
            this.rdoDB.TabStop = true;
            this.rdoDB.Text = "Worbench DB";
            this.rdoDB.UseVisualStyleBackColor = true;
            this.rdoDB.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoCSV);
            this.groupBox1.Controls.Add(this.rdoDB);
            this.groupBox1.Location = new System.Drawing.Point(16, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(459, 42);
            this.groupBox1.TabIndex = 45;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Destination";
            // 
            // rdoCSV
            // 
            this.rdoCSV.AutoSize = true;
            this.rdoCSV.Location = new System.Drawing.Point(140, 19);
            this.rdoCSV.Name = "rdoCSV";
            this.rdoCSV.Size = new System.Drawing.Size(65, 17);
            this.rdoCSV.TabIndex = 7;
            this.rdoCSV.Text = "CSV File";
            this.rdoCSV.UseVisualStyleBackColor = true;
            this.rdoCSV.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // panelCSV
            // 
            this.panelCSV.Controls.Add(this.txtCSVFile);
            this.panelCSV.Controls.Add(this.cmdBrowseCSV);
            this.panelCSV.Controls.Add(this.label3);
            this.panelCSV.Location = new System.Drawing.Point(16, 126);
            this.panelCSV.Name = "panelCSV";
            this.panelCSV.Size = new System.Drawing.Size(458, 38);
            this.panelCSV.TabIndex = 46;
            // 
            // cmdStop
            // 
            this.cmdStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdStop.Location = new System.Drawing.Point(398, 181);
            this.cmdStop.Name = "cmdStop";
            this.cmdStop.Size = new System.Drawing.Size(75, 23);
            this.cmdStop.TabIndex = 11;
            this.cmdStop.Text = "Stop";
            this.cmdStop.UseVisualStyleBackColor = true;
            this.cmdStop.Click += new System.EventHandler(this.cmdStop_Click);
            // 
            // cmdRun
            // 
            this.cmdRun.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdRun.Location = new System.Drawing.Point(318, 181);
            this.cmdRun.Name = "cmdRun";
            this.cmdRun.Size = new System.Drawing.Size(75, 23);
            this.cmdRun.TabIndex = 10;
            this.cmdRun.Text = "Run";
            this.cmdRun.UseVisualStyleBackColor = true;
            this.cmdRun.Click += new System.EventHandler(this.cmdRun_Click);
            // 
            // prgBar
            // 
            this.prgBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.prgBar.Location = new System.Drawing.Point(16, 181);
            this.prgBar.Name = "prgBar";
            this.prgBar.Size = new System.Drawing.Size(296, 23);
            this.prgBar.Step = 1;
            this.prgBar.TabIndex = 17;
            this.prgBar.Visible = false;
            // 
            // BackgroundWorker1
            // 
            this.BackgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.BackgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.BackgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // frmScavengeHabitatResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 244);
            this.Controls.Add(this.cmdStop);
            this.Controls.Add(this.cmdRun);
            this.Controls.Add(this.prgBar);
            this.Controls.Add(this.panelCSV);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtHabitatModelFolder);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdBrowseProject);
            this.Controls.Add(this.cmdClose);
            this.MinimumSize = new System.Drawing.Size(502, 184);
            this.Name = "frmScavengeHabitatResults";
            this.Text = "Scavenge Habitat Results";
            this.Load += new System.EventHandler(this.frmScavengeHabitatResults_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelCSV.ResumeLayout(false);
            this.panelCSV.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdBrowseProject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHabitatModelFolder;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCSVFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cmdBrowseCSV;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.RadioButton rdoDB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoCSV;
        private System.Windows.Forms.Panel panelCSV;
        internal System.Windows.Forms.Button cmdStop;
        internal System.Windows.Forms.Button cmdRun;
        internal System.Windows.Forms.ProgressBar prgBar;
        internal System.ComponentModel.BackgroundWorker BackgroundWorker1;
    }
}