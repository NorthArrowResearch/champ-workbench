namespace CHaMPWorkbench.Data
{
    partial class frmDataUnPacker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataUnPacker));
            this.txtSurvey3 = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.txtWSTIN = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.txtTopoTIN = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtSurvey2 = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtSurvey1 = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.chkHydroInputs = new System.Windows.Forms.CheckBox();
            this.lblHydroInputs = new System.Windows.Forms.Label();
            this.txtHydroInputs = new System.Windows.Forms.TextBox();
            this.txtHydroResults = new System.Windows.Forms.TextBox();
            this.lblHydroResults = new System.Windows.Forms.Label();
            this.chkHydroResults = new System.Windows.Forms.CheckBox();
            this.rdoSame = new System.Windows.Forms.RadioButton();
            this.rdoDifferent = new System.Windows.Forms.RadioButton();
            this.cmdBrowseOutput = new System.Windows.Forms.Button();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.lblDifferent = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.pgrProgress = new System.Windows.Forms.ProgressBar();
            this.lblStatus = new System.Windows.Forms.Label();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSurvey3
            // 
            this.txtSurvey3.Location = new System.Drawing.Point(86, 71);
            this.txtSurvey3.Name = "txtSurvey3";
            this.txtSurvey3.Size = new System.Drawing.Size(500, 20);
            this.txtSurvey3.TabIndex = 5;
            this.txtSurvey3.Text = "*gdb.zip";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(35, 75);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(47, 13);
            this.Label6.TabIndex = 4;
            this.Label6.Text = "Option 3";
            // 
            // txtWSTIN
            // 
            this.txtWSTIN.Location = new System.Drawing.Point(86, 45);
            this.txtWSTIN.Name = "txtWSTIN";
            this.txtWSTIN.Size = new System.Drawing.Size(500, 20);
            this.txtWSTIN.TabIndex = 8;
            this.txtWSTIN.Text = "WettedSurfaceTIN*.zip";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(8, 49);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(74, 13);
            this.Label5.TabIndex = 7;
            this.Label5.Text = "Water surface";
            // 
            // txtTopoTIN
            // 
            this.txtTopoTIN.Location = new System.Drawing.Point(86, 19);
            this.txtTopoTIN.Name = "txtTopoTIN";
            this.txtTopoTIN.Size = new System.Drawing.Size(500, 20);
            this.txtTopoTIN.TabIndex = 6;
            this.txtTopoTIN.Text = "TIN*.zip";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(50, 23);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(32, 13);
            this.Label4.TabIndex = 5;
            this.Label4.Text = "Topo";
            // 
            // txtSurvey2
            // 
            this.txtSurvey2.Location = new System.Drawing.Point(86, 45);
            this.txtSurvey2.Name = "txtSurvey2";
            this.txtSurvey2.Size = new System.Drawing.Size(500, 20);
            this.txtSurvey2.TabIndex = 3;
            this.txtSurvey2.Text = "Orthogonal*.zip";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(35, 49);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(47, 13);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "Option 2";
            // 
            // txtSurvey1
            // 
            this.txtSurvey1.Location = new System.Drawing.Point(86, 19);
            this.txtSurvey1.Name = "txtSurvey1";
            this.txtSurvey1.Size = new System.Drawing.Size(500, 20);
            this.txtSurvey1.TabIndex = 1;
            this.txtSurvey1.Text = "SurveyGDB*.zip";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(35, 23);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(47, 13);
            this.Label2.TabIndex = 0;
            this.Label2.Text = "Option 1";
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(465, 439);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 10;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(546, 439);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 11;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Location = new System.Drawing.Point(520, 13);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(71, 23);
            this.cmdBrowse.TabIndex = 4;
            this.cmdBrowse.Text = "Browse";
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(168, 14);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(346, 20);
            this.txtFolder.TabIndex = 3;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(12, 18);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(153, 13);
            this.Label1.TabIndex = 2;
            this.Label1.Text = "Top level folder containing zips";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(10, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(578, 42);
            this.label7.TabIndex = 0;
            this.label7.Text = resources.GetString("label7.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSurvey1);
            this.groupBox1.Controls.Add(this.Label2);
            this.groupBox1.Controls.Add(this.txtSurvey3);
            this.groupBox1.Controls.Add(this.Label3);
            this.groupBox1.Controls.Add(this.Label6);
            this.groupBox1.Controls.Add(this.txtSurvey2);
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(592, 103);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Survey GDB Search Patterns";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(10, 55);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(578, 35);
            this.label8.TabIndex = 1;
            this.label8.Text = resources.GetString("label8.Text");
            // 
            // chkHydroInputs
            // 
            this.chkHydroInputs.AutoSize = true;
            this.chkHydroInputs.Location = new System.Drawing.Point(8, 9);
            this.chkHydroInputs.Name = "chkHydroInputs";
            this.chkHydroInputs.Size = new System.Drawing.Size(182, 17);
            this.chkHydroInputs.TabIndex = 12;
            this.chkHydroInputs.Text = "Search for hydraulic model inputs";
            this.chkHydroInputs.UseVisualStyleBackColor = true;
            this.chkHydroInputs.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // lblHydroInputs
            // 
            this.lblHydroInputs.AutoSize = true;
            this.lblHydroInputs.Location = new System.Drawing.Point(41, 33);
            this.lblHydroInputs.Name = "lblHydroInputs";
            this.lblHydroInputs.Size = new System.Drawing.Size(36, 13);
            this.lblHydroInputs.TabIndex = 13;
            this.lblHydroInputs.Text = "Inputs";
            // 
            // txtHydroInputs
            // 
            this.txtHydroInputs.Location = new System.Drawing.Point(87, 29);
            this.txtHydroInputs.Name = "txtHydroInputs";
            this.txtHydroInputs.Size = new System.Drawing.Size(408, 20);
            this.txtHydroInputs.TabIndex = 14;
            this.txtHydroInputs.Text = "HydroModelInputs.zip";
            // 
            // txtHydroResults
            // 
            this.txtHydroResults.Location = new System.Drawing.Point(87, 73);
            this.txtHydroResults.Name = "txtHydroResults";
            this.txtHydroResults.Size = new System.Drawing.Size(408, 20);
            this.txtHydroResults.TabIndex = 17;
            this.txtHydroResults.Text = "HydroModelResults.zip";
            // 
            // lblHydroResults
            // 
            this.lblHydroResults.AutoSize = true;
            this.lblHydroResults.Location = new System.Drawing.Point(35, 77);
            this.lblHydroResults.Name = "lblHydroResults";
            this.lblHydroResults.Size = new System.Drawing.Size(42, 13);
            this.lblHydroResults.TabIndex = 16;
            this.lblHydroResults.Text = "Results";
            // 
            // chkHydroResults
            // 
            this.chkHydroResults.AutoSize = true;
            this.chkHydroResults.Checked = true;
            this.chkHydroResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkHydroResults.Location = new System.Drawing.Point(8, 53);
            this.chkHydroResults.Name = "chkHydroResults";
            this.chkHydroResults.Size = new System.Drawing.Size(184, 17);
            this.chkHydroResults.TabIndex = 15;
            this.chkHydroResults.Text = "Search for hydraulic model results";
            this.chkHydroResults.UseVisualStyleBackColor = true;
            this.chkHydroResults.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // rdoSame
            // 
            this.rdoSame.AutoSize = true;
            this.rdoSame.Checked = true;
            this.rdoSame.Location = new System.Drawing.Point(9, 21);
            this.rdoSame.Name = "rdoSame";
            this.rdoSame.Size = new System.Drawing.Size(181, 17);
            this.rdoSame.TabIndex = 19;
            this.rdoSame.TabStop = true;
            this.rdoSame.Text = "Unpack into same location as zip";
            this.rdoSame.UseVisualStyleBackColor = true;
            // 
            // rdoDifferent
            // 
            this.rdoDifferent.AutoSize = true;
            this.rdoDifferent.Location = new System.Drawing.Point(9, 47);
            this.rdoDifferent.Name = "rdoDifferent";
            this.rdoDifferent.Size = new System.Drawing.Size(164, 17);
            this.rdoDifferent.TabIndex = 20;
            this.rdoDifferent.Text = "Unpack into different location";
            this.rdoDifferent.UseVisualStyleBackColor = true;
            this.rdoDifferent.CheckedChanged += new System.EventHandler(this.UpdateControls);
            // 
            // cmdBrowseOutput
            // 
            this.cmdBrowseOutput.Location = new System.Drawing.Point(501, 67);
            this.cmdBrowseOutput.Name = "cmdBrowseOutput";
            this.cmdBrowseOutput.Size = new System.Drawing.Size(71, 23);
            this.cmdBrowseOutput.TabIndex = 23;
            this.cmdBrowseOutput.Text = "Browse";
            this.cmdBrowseOutput.UseVisualStyleBackColor = true;
            this.cmdBrowseOutput.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Location = new System.Drawing.Point(161, 69);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(337, 20);
            this.txtOutputFolder.TabIndex = 22;
            // 
            // lblDifferent
            // 
            this.lblDifferent.AutoSize = true;
            this.lblDifferent.Location = new System.Drawing.Point(29, 73);
            this.lblDifferent.Name = "lblDifferent";
            this.lblDifferent.Size = new System.Drawing.Size(128, 13);
            this.lblDifferent.TabIndex = 21;
            this.lblDifferent.Text = "Top level folder for output";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtTopoTIN);
            this.groupBox3.Controls.Add(this.Label4);
            this.groupBox3.Controls.Add(this.txtWSTIN);
            this.groupBox3.Controls.Add(this.Label5);
            this.groupBox3.Location = new System.Drawing.Point(6, 129);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(592, 80);
            this.groupBox3.TabIndex = 24;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "TIN Search Patterns";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.txtOutputFolder);
            this.groupBox4.Controls.Add(this.rdoSame);
            this.groupBox4.Controls.Add(this.cmdBrowseOutput);
            this.groupBox4.Controls.Add(this.rdoDifferent);
            this.groupBox4.Controls.Add(this.lblDifferent);
            this.groupBox4.Location = new System.Drawing.Point(15, 42);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(578, 100);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Output location";
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(10, 92);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(612, 247);
            this.tabControl1.TabIndex = 26;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtFolder);
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.Label1);
            this.tabPage1.Controls.Add(this.cmdBrowse);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(604, 221);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Folder Paths";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(604, 221);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Topo Data";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chkHydroInputs);
            this.tabPage3.Controls.Add(this.txtHydroResults);
            this.tabPage3.Controls.Add(this.lblHydroInputs);
            this.tabPage3.Controls.Add(this.chkHydroResults);
            this.tabPage3.Controls.Add(this.lblHydroResults);
            this.tabPage3.Controls.Add(this.txtHydroInputs);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(604, 221);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Hydraulic Model Data";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // pgrProgress
            // 
            this.pgrProgress.Location = new System.Drawing.Point(8, 19);
            this.pgrProgress.Name = "pgrProgress";
            this.pgrProgress.Size = new System.Drawing.Size(598, 23);
            this.pgrProgress.TabIndex = 27;
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(8, 51);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(47, 13);
            this.lblStatus.TabIndex = 28;
            this.lblStatus.Text = "lblStatus";
            // 
            // backgroundWorker1
            // 
            this.backgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker1_DoWork);
            this.backgroundWorker1.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker1_ProgressChanged);
            this.backgroundWorker1.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.backgroundWorker1_RunWorkerCompleted);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.pgrProgress);
            this.groupBox2.Controls.Add(this.lblStatus);
            this.groupBox2.Location = new System.Drawing.Point(10, 350);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(612, 77);
            this.groupBox2.TabIndex = 29;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Status";
            // 
            // frmDataUnPacker
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(634, 475);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDataUnPacker";
            this.Text = "CHaMP Monitoring Data Unpacker";
            this.Load += new System.EventHandler(this.frmDataUnPacker_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        internal System.Windows.Forms.TextBox txtSurvey3;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.TextBox txtWSTIN;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox txtTopoTIN;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox txtSurvey2;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox txtSurvey1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.Button cmdBrowse;
        internal System.Windows.Forms.TextBox txtFolder;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.CheckBox chkHydroInputs;
        private System.Windows.Forms.Label lblHydroInputs;
        internal System.Windows.Forms.TextBox txtHydroInputs;
        internal System.Windows.Forms.TextBox txtHydroResults;
        private System.Windows.Forms.Label lblHydroResults;
        private System.Windows.Forms.CheckBox chkHydroResults;
        private System.Windows.Forms.RadioButton rdoSame;
        private System.Windows.Forms.RadioButton rdoDifferent;
        internal System.Windows.Forms.Button cmdBrowseOutput;
        internal System.Windows.Forms.TextBox txtOutputFolder;
        internal System.Windows.Forms.Label lblDifferent;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.ProgressBar pgrProgress;
        private System.Windows.Forms.Label lblStatus;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}