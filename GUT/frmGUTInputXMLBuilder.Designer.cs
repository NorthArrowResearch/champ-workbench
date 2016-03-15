namespace CHaMPWorkbench.GUT
{
    partial class frmGUTInputXMLBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGUTInputXMLBuilder));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.chkClearOtherBatches = new System.Windows.Forms.CheckBox();
            this.lblBatchName = new System.Windows.Forms.Label();
            this.txtBatch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.cmdBrowseInputOutput = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtMonitoringDataFolder = new System.Windows.Forms.TextBox();
            this.cmdBrowseMonitoring = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lstVisits = new System.Windows.Forms.ListBox();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.valHighRelief = new System.Windows.Forms.NumericUpDown();
            this.valLowRelief = new System.Windows.Forms.NumericUpDown();
            this.label12 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.val_fw_reliedf = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.val_high_bf_distance = new System.Windows.Forms.NumericUpDown();
            this.val_low_bf_distance = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.val_up_hadbf = new System.Windows.Forms.NumericUpDown();
            this.val_low_hadbf = new System.Windows.Forms.NumericUpDown();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.val_high_cm_slope = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.val_low_cm_slope = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.val_high_slope = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.val_low_slope = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valHighRelief)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valLowRelief)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_fw_reliedf)).BeginInit();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.val_high_bf_distance)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_low_bf_distance)).BeginInit();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.val_up_hadbf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_low_hadbf)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_high_cm_slope)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_low_cm_slope)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_high_slope)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_low_slope)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(556, 399);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(475, 399);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.CausesValidation = false;
            this.cmdHelp.Location = new System.Drawing.Point(12, 399);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 2;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            this.cmdHelp.Click += new System.EventHandler(this.cmdHelp_Click);
            // 
            // chkClearOtherBatches
            // 
            this.chkClearOtherBatches.AutoSize = true;
            this.chkClearOtherBatches.Location = new System.Drawing.Point(389, 18);
            this.chkClearOtherBatches.Name = "chkClearOtherBatches";
            this.chkClearOtherBatches.Size = new System.Drawing.Size(214, 17);
            this.chkClearOtherBatches.TabIndex = 5;
            this.chkClearOtherBatches.Text = "Set this as the only batch queued to run";
            this.chkClearOtherBatches.UseVisualStyleBackColor = true;
            // 
            // lblBatchName
            // 
            this.lblBatchName.AutoSize = true;
            this.lblBatchName.Location = new System.Drawing.Point(121, 20);
            this.lblBatchName.Name = "lblBatchName";
            this.lblBatchName.Size = new System.Drawing.Size(64, 13);
            this.lblBatchName.TabIndex = 3;
            this.lblBatchName.Text = "Batch name";
            // 
            // txtBatch
            // 
            this.txtBatch.Location = new System.Drawing.Point(190, 16);
            this.txtBatch.MaxLength = 255;
            this.txtBatch.Name = "txtBatch";
            this.txtBatch.Size = new System.Drawing.Size(192, 20);
            this.txtBatch.TabIndex = 4;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(21, 83);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Output folder (\"InputOutputFiles\")";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.Location = new System.Drawing.Point(190, 79);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(325, 20);
            this.txtOutputFolder.TabIndex = 12;
            this.txtOutputFolder.Text = "C:\\CHaMP\\RBTInputOutputFiles";
            // 
            // cmdBrowseInputOutput
            // 
            this.cmdBrowseInputOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseInputOutput.Location = new System.Drawing.Point(521, 78);
            this.cmdBrowseInputOutput.Name = "cmdBrowseInputOutput";
            this.cmdBrowseInputOutput.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseInputOutput.TabIndex = 13;
            this.cmdBrowseInputOutput.Text = "Browse";
            this.cmdBrowseInputOutput.UseVisualStyleBackColor = true;
            this.cmdBrowseInputOutput.Click += new System.EventHandler(this.cmdBrowseInputOutput_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(50, 50);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(135, 13);
            this.Label2.TabIndex = 8;
            this.Label2.Text = "Parent folder (\"Monitoring\")";
            // 
            // txtMonitoringDataFolder
            // 
            this.txtMonitoringDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMonitoringDataFolder.Location = new System.Drawing.Point(190, 46);
            this.txtMonitoringDataFolder.Name = "txtMonitoringDataFolder";
            this.txtMonitoringDataFolder.Size = new System.Drawing.Size(325, 20);
            this.txtMonitoringDataFolder.TabIndex = 9;
            // 
            // cmdBrowseMonitoring
            // 
            this.cmdBrowseMonitoring.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseMonitoring.Location = new System.Drawing.Point(521, 45);
            this.cmdBrowseMonitoring.Name = "cmdBrowseMonitoring";
            this.cmdBrowseMonitoring.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseMonitoring.TabIndex = 10;
            this.cmdBrowseMonitoring.Text = "Browse";
            this.cmdBrowseMonitoring.UseVisualStyleBackColor = true;
            this.cmdBrowseMonitoring.Click += new System.EventHandler(this.cmdBrowseMonitoring_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(619, 381);
            this.tabControl1.TabIndex = 14;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox4);
            this.tabPage1.Controls.Add(this.txtInputFile);
            this.tabPage1.Controls.Add(this.label14);
            this.tabPage1.Controls.Add(this.txtBatch);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.lblBatchName);
            this.tabPage1.Controls.Add(this.txtOutputFolder);
            this.tabPage1.Controls.Add(this.chkClearOtherBatches);
            this.tabPage1.Controls.Add(this.cmdBrowseInputOutput);
            this.tabPage1.Controls.Add(this.cmdBrowseMonitoring);
            this.tabPage1.Controls.Add(this.Label2);
            this.tabPage1.Controls.Add(this.txtMonitoringDataFolder);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(611, 355);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Site and Visit";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.lstVisits);
            this.groupBox4.Location = new System.Drawing.Point(10, 136);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(593, 208);
            this.groupBox4.TabIndex = 17;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Visits";
            // 
            // lstVisits
            // 
            this.lstVisits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstVisits.FormattingEnabled = true;
            this.lstVisits.Location = new System.Drawing.Point(8, 19);
            this.lstVisits.Name = "lstVisits";
            this.lstVisits.Size = new System.Drawing.Size(579, 173);
            this.lstVisits.TabIndex = 0;
            // 
            // txtInputFile
            // 
            this.txtInputFile.Location = new System.Drawing.Point(190, 110);
            this.txtInputFile.MaxLength = 255;
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(192, 20);
            this.txtInputFile.TabIndex = 15;
            this.txtInputFile.Text = "gut_input.xml";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(84, 114);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(101, 13);
            this.label14.TabIndex = 14;
            this.label14.Text = "GUT input file name";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Controls.Add(this.val_fw_reliedf);
            this.tabPage2.Controls.Add(this.label11);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Controls.Add(this.groupBox1);
            this.tabPage2.Controls.Add(this.val_high_cm_slope);
            this.tabPage2.Controls.Add(this.label6);
            this.tabPage2.Controls.Add(this.val_low_cm_slope);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.val_high_slope);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.val_low_slope);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(611, 355);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "GUT Configuration";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.valHighRelief);
            this.groupBox3.Controls.Add(this.valLowRelief);
            this.groupBox3.Controls.Add(this.label12);
            this.groupBox3.Controls.Add(this.label13);
            this.groupBox3.Location = new System.Drawing.Point(237, 234);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(260, 100);
            this.groupBox3.TabIndex = 14;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Relief";
            // 
            // valHighRelief
            // 
            this.valHighRelief.DecimalPlaces = 1;
            this.valHighRelief.Location = new System.Drawing.Point(97, 52);
            this.valHighRelief.Name = "valHighRelief";
            this.valHighRelief.Size = new System.Drawing.Size(120, 20);
            this.valHighRelief.TabIndex = 12;
            this.valHighRelief.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // valLowRelief
            // 
            this.valLowRelief.DecimalPlaces = 1;
            this.valLowRelief.Location = new System.Drawing.Point(97, 26);
            this.valLowRelief.Name = "valLowRelief";
            this.valLowRelief.Size = new System.Drawing.Size(120, 20);
            this.valLowRelief.TabIndex = 10;
            this.valLowRelief.Value = new decimal(new int[] {
            8,
            0,
            0,
            65536});
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(4, 52);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(29, 13);
            this.label12.TabIndex = 11;
            this.label12.Text = "High";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(4, 26);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(27, 13);
            this.label13.TabIndex = 9;
            this.label13.Text = "Low";
            // 
            // val_fw_reliedf
            // 
            this.val_fw_reliedf.DecimalPlaces = 1;
            this.val_fw_reliedf.Location = new System.Drawing.Point(97, 138);
            this.val_fw_reliedf.Name = "val_fw_reliedf";
            this.val_fw_reliedf.Size = new System.Drawing.Size(120, 20);
            this.val_fw_reliedf.TabIndex = 15;
            this.val_fw_reliedf.Value = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(37, 142);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(54, 13);
            this.label11.TabIndex = 14;
            this.label11.Text = "FW Relief";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.val_high_bf_distance);
            this.groupBox2.Controls.Add(this.val_low_bf_distance);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Location = new System.Drawing.Point(237, 128);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(260, 100);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Bankfull Distance";
            // 
            // val_high_bf_distance
            // 
            this.val_high_bf_distance.DecimalPlaces = 1;
            this.val_high_bf_distance.Location = new System.Drawing.Point(97, 52);
            this.val_high_bf_distance.Name = "val_high_bf_distance";
            this.val_high_bf_distance.Size = new System.Drawing.Size(120, 20);
            this.val_high_bf_distance.TabIndex = 12;
            this.val_high_bf_distance.Value = new decimal(new int[] {
            2,
            0,
            0,
            65536});
            // 
            // val_low_bf_distance
            // 
            this.val_low_bf_distance.DecimalPlaces = 1;
            this.val_low_bf_distance.Location = new System.Drawing.Point(97, 26);
            this.val_low_bf_distance.Name = "val_low_bf_distance";
            this.val_low_bf_distance.Size = new System.Drawing.Size(120, 20);
            this.val_low_bf_distance.TabIndex = 10;
            this.val_low_bf_distance.Value = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(4, 52);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 13);
            this.label9.TabIndex = 11;
            this.label9.Text = "High";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(4, 26);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(27, 13);
            this.label10.TabIndex = 9;
            this.label10.Text = "Low";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.val_up_hadbf);
            this.groupBox1.Controls.Add(this.val_low_hadbf);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Location = new System.Drawing.Point(237, 22);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(260, 100);
            this.groupBox1.TabIndex = 8;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Height Above Detrended Bankfull (HADBF)";
            // 
            // val_up_hadbf
            // 
            this.val_up_hadbf.DecimalPlaces = 1;
            this.val_up_hadbf.Location = new System.Drawing.Point(97, 52);
            this.val_up_hadbf.Name = "val_up_hadbf";
            this.val_up_hadbf.Size = new System.Drawing.Size(120, 20);
            this.val_up_hadbf.TabIndex = 12;
            this.val_up_hadbf.Value = new decimal(new int[] {
            12,
            0,
            0,
            65536});
            // 
            // val_low_hadbf
            // 
            this.val_low_hadbf.DecimalPlaces = 1;
            this.val_low_hadbf.Location = new System.Drawing.Point(97, 26);
            this.val_low_hadbf.Name = "val_low_hadbf";
            this.val_low_hadbf.Size = new System.Drawing.Size(120, 20);
            this.val_low_hadbf.TabIndex = 10;
            this.val_low_hadbf.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(4, 52);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 13);
            this.label7.TabIndex = 11;
            this.label7.Text = "High";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(4, 26);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(27, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Low";
            // 
            // val_high_cm_slope
            // 
            this.val_high_cm_slope.Location = new System.Drawing.Point(97, 109);
            this.val_high_cm_slope.Name = "val_high_cm_slope";
            this.val_high_cm_slope.Size = new System.Drawing.Size(120, 20);
            this.val_high_cm_slope.TabIndex = 7;
            this.val_high_cm_slope.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(25, 113);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Up cm slope";
            // 
            // val_low_cm_slope
            // 
            this.val_low_cm_slope.Location = new System.Drawing.Point(97, 80);
            this.val_low_cm_slope.Name = "val_low_cm_slope";
            this.val_low_cm_slope.Size = new System.Drawing.Size(120, 20);
            this.val_low_cm_slope.TabIndex = 5;
            this.val_low_cm_slope.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(19, 84);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 13);
            this.label5.TabIndex = 4;
            this.label5.Text = "Low cm slope";
            // 
            // val_high_slope
            // 
            this.val_high_slope.Location = new System.Drawing.Point(97, 51);
            this.val_high_slope.Name = "val_high_slope";
            this.val_high_slope.Size = new System.Drawing.Size(120, 20);
            this.val_high_slope.TabIndex = 3;
            this.val_high_slope.Value = new decimal(new int[] {
            15,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(22, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 13);
            this.label4.TabIndex = 2;
            this.label4.Text = "Upland slope";
            // 
            // val_low_slope
            // 
            this.val_low_slope.Location = new System.Drawing.Point(97, 22);
            this.val_low_slope.Name = "val_low_slope";
            this.val_low_slope.Size = new System.Drawing.Size(120, 20);
            this.val_low_slope.TabIndex = 1;
            this.val_low_slope.Value = new decimal(new int[] {
            10,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(55, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Low slope";
            // 
            // frmGUTInputXMLBuilder
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(643, 434);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(659, 472);
            this.Name = "frmGUTInputXMLBuilder";
            this.Text = "GUT Input File Builder";
            this.Load += new System.EventHandler(this.frmGUTInputXMLBuilder_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valHighRelief)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valLowRelief)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_fw_reliedf)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.val_high_bf_distance)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_low_bf_distance)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.val_up_hadbf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_low_hadbf)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_high_cm_slope)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_low_cm_slope)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_high_slope)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.val_low_slope)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.CheckBox chkClearOtherBatches;
        internal System.Windows.Forms.Label lblBatchName;
        internal System.Windows.Forms.TextBox txtBatch;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.TextBox txtOutputFolder;
        internal System.Windows.Forms.Button cmdBrowseInputOutput;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtMonitoringDataFolder;
        internal System.Windows.Forms.Button cmdBrowseMonitoring;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.NumericUpDown val_fw_reliedf;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.NumericUpDown val_high_bf_distance;
        private System.Windows.Forms.NumericUpDown val_low_bf_distance;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown val_up_hadbf;
        private System.Windows.Forms.NumericUpDown val_low_hadbf;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.NumericUpDown val_high_cm_slope;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown val_low_cm_slope;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.NumericUpDown val_high_slope;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown val_low_slope;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.NumericUpDown valHighRelief;
        private System.Windows.Forms.NumericUpDown valLowRelief;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label13;
        internal System.Windows.Forms.TextBox txtInputFile;
        internal System.Windows.Forms.Label label14;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lstVisits;
    }
}