namespace CHaMPWorkbench
{
    partial class frmRBTRun
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
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkChangeDetection = new System.Windows.Forms.CheckBox();
            this.chkOrthogonal = new System.Windows.Forms.CheckBox();
            this.chkCalculateMetrics = new System.Windows.Forms.CheckBox();
            this.cboFieldSeason = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboVisit = new System.Windows.Forms.ComboBox();
            this.cboSite = new System.Windows.Forms.ComboBox();
            this.cboWatershed = new System.Windows.Forms.ComboBox();
            this.chkAddAllVisits = new System.Windows.Forms.CheckBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.cmdBrowseInputFile = new System.Windows.Forms.Button();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSourceFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.ucConfig = new CHaMPWorkbench.RBTConfiguration();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Watershed:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(51, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Site:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(14, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Primary visit:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkChangeDetection);
            this.groupBox1.Controls.Add(this.chkOrthogonal);
            this.groupBox1.Controls.Add(this.chkCalculateMetrics);
            this.groupBox1.Controls.Add(this.cboFieldSeason);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cboVisit);
            this.groupBox1.Controls.Add(this.cboSite);
            this.groupBox1.Controls.Add(this.cboWatershed);
            this.groupBox1.Controls.Add(this.chkAddAllVisits);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(11, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(514, 236);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "CHaMP";
            // 
            // chkChangeDetection
            // 
            this.chkChangeDetection.AutoSize = true;
            this.chkChangeDetection.Location = new System.Drawing.Point(84, 207);
            this.chkChangeDetection.Name = "chkChangeDetection";
            this.chkChangeDetection.Size = new System.Drawing.Size(112, 17);
            this.chkChangeDetection.TabIndex = 11;
            this.chkChangeDetection.Text = "Change Detection";
            this.chkChangeDetection.UseVisualStyleBackColor = true;
            // 
            // chkOrthogonal
            // 
            this.chkOrthogonal.AutoSize = true;
            this.chkOrthogonal.Checked = true;
            this.chkOrthogonal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOrthogonal.Location = new System.Drawing.Point(84, 183);
            this.chkOrthogonal.Name = "chkOrthogonal";
            this.chkOrthogonal.Size = new System.Drawing.Size(140, 17);
            this.chkOrthogonal.TabIndex = 10;
            this.chkOrthogonal.Text = "Make DEMs Orthogonal";
            this.chkOrthogonal.UseVisualStyleBackColor = true;
            // 
            // chkCalculateMetrics
            // 
            this.chkCalculateMetrics.AutoSize = true;
            this.chkCalculateMetrics.Checked = true;
            this.chkCalculateMetrics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCalculateMetrics.Location = new System.Drawing.Point(84, 160);
            this.chkCalculateMetrics.Name = "chkCalculateMetrics";
            this.chkCalculateMetrics.Size = new System.Drawing.Size(107, 17);
            this.chkCalculateMetrics.TabIndex = 9;
            this.chkCalculateMetrics.Text = "Calculate Metrics";
            this.chkCalculateMetrics.UseVisualStyleBackColor = true;
            // 
            // cboFieldSeason
            // 
            this.cboFieldSeason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFieldSeason.FormattingEnabled = true;
            this.cboFieldSeason.Location = new System.Drawing.Point(84, 17);
            this.cboFieldSeason.Name = "cboFieldSeason";
            this.cboFieldSeason.Size = new System.Drawing.Size(419, 21);
            this.cboFieldSeason.TabIndex = 8;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 21);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(69, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Field season:";
            // 
            // cboVisit
            // 
            this.cboVisit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVisit.FormattingEnabled = true;
            this.cboVisit.Location = new System.Drawing.Point(84, 104);
            this.cboVisit.Name = "cboVisit";
            this.cboVisit.Size = new System.Drawing.Size(419, 21);
            this.cboVisit.TabIndex = 6;
            this.cboVisit.SelectedIndexChanged += new System.EventHandler(this.cboVisit_SelectedIndexChanged);
            // 
            // cboSite
            // 
            this.cboSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSite.FormattingEnabled = true;
            this.cboSite.Location = new System.Drawing.Point(84, 75);
            this.cboSite.Name = "cboSite";
            this.cboSite.Size = new System.Drawing.Size(419, 21);
            this.cboSite.TabIndex = 5;
            this.cboSite.SelectedIndexChanged += new System.EventHandler(this.cboSite_SelectedIndexChanged);
            // 
            // cboWatershed
            // 
            this.cboWatershed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWatershed.FormattingEnabled = true;
            this.cboWatershed.Location = new System.Drawing.Point(84, 46);
            this.cboWatershed.Name = "cboWatershed";
            this.cboWatershed.Size = new System.Drawing.Size(419, 21);
            this.cboWatershed.TabIndex = 4;
            this.cboWatershed.SelectedIndexChanged += new System.EventHandler(this.cboWatershed_SelectedIndexChanged);
            // 
            // chkAddAllVisits
            // 
            this.chkAddAllVisits.AutoSize = true;
            this.chkAddAllVisits.Checked = true;
            this.chkAddAllVisits.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAddAllVisits.Location = new System.Drawing.Point(84, 136);
            this.chkAddAllVisits.Name = "chkAddAllVisits";
            this.chkAddAllVisits.Size = new System.Drawing.Size(150, 17);
            this.chkAddAllVisits.TabIndex = 3;
            this.chkAddAllVisits.Text = "Include all visits to this site";
            this.chkAddAllVisits.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 120);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(964, 478);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(956, 452);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Site and Visit";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ucConfig);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(956, 452);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "RBT Configuration";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // cmdBrowseInputFile
            // 
            this.cmdBrowseInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseInputFile.Location = new System.Drawing.Point(793, 71);
            this.cmdBrowseInputFile.Name = "cmdBrowseInputFile";
            this.cmdBrowseInputFile.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseInputFile.TabIndex = 6;
            this.cmdBrowseInputFile.Text = "Browse";
            this.cmdBrowseInputFile.UseVisualStyleBackColor = true;
            this.cmdBrowseInputFile.Click += new System.EventHandler(this.cmdBrowseInputFile_Click);
            // 
            // txtInputFile
            // 
            this.txtInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtInputFile.Location = new System.Drawing.Point(223, 72);
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(564, 20);
            this.txtInputFile.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(203, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "RBT input XML file that will be generated:";
            // 
            // txtSourceFolder
            // 
            this.txtSourceFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceFolder.Location = new System.Drawing.Point(164, 16);
            this.txtSourceFolder.Name = "txtSourceFolder";
            this.txtSourceFolder.Size = new System.Drawing.Size(564, 20);
            this.txtSourceFolder.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Parent source data folder:";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.Location = new System.Drawing.Point(164, 42);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(564, 20);
            this.txtOutputFolder.TabIndex = 11;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(15, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Parent output data folder:";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(744, 14);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Browse";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(744, 42);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 14;
            this.button2.Text = "Browse";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(616, 628);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 15;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(711, 617);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 16;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // ucConfig
            // 
            this.ucConfig.Location = new System.Drawing.Point(6, 6);
            this.ucConfig.Name = "ucConfig";
            this.ucConfig.Size = new System.Drawing.Size(835, 384);
            this.ucConfig.TabIndex = 0;
            // 
            // frmRBTRun
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(1136, 671);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSourceFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdBrowseInputFile);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tabControl1);
            this.Name = "frmRBTRun";
            this.Text = "Configure Single RBT Run";
            this.Load += new System.EventHandler(this.frmRBTRun_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkAddAllVisits;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private RBTConfiguration ucConfig;
        private System.Windows.Forms.ComboBox cboFieldSeason;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboVisit;
        private System.Windows.Forms.ComboBox cboSite;
        private System.Windows.Forms.ComboBox cboWatershed;
        internal System.Windows.Forms.Button cmdBrowseInputFile;
        internal System.Windows.Forms.TextBox txtInputFile;
        internal System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox chkChangeDetection;
        private System.Windows.Forms.CheckBox chkOrthogonal;
        private System.Windows.Forms.CheckBox chkCalculateMetrics;
        internal System.Windows.Forms.TextBox txtSourceFolder;
        internal System.Windows.Forms.Label label4;
        internal System.Windows.Forms.TextBox txtOutputFolder;
        internal System.Windows.Forms.Label label7;
        internal System.Windows.Forms.Button button1;
        internal System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
    }
}