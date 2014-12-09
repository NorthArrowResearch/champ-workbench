namespace CHaMPWorkbench
{
    partial class frmRBTInputSingle
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkChangeDetection = new System.Windows.Forms.CheckBox();
            this.chkOrthogonal = new System.Windows.Forms.CheckBox();
            this.chkCalculateMetrics = new System.Windows.Forms.CheckBox();
            this.cboVisit = new System.Windows.Forms.ComboBox();
            this.cHAMPVisitsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rBTWorkbenchDataSet = new CHaMPWorkbench.RBTWorkbenchDataSet();
            this.cHAMPSitesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboSite = new System.Windows.Forms.ComboBox();
            this.cHAMP_WatershedsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.cboWatershed = new System.Windows.Forms.ComboBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.chkCopyPath = new System.Windows.Forms.CheckBox();
            this.txtBatchName = new System.Windows.Forms.TextBox();
            this.lblBatchName = new System.Windows.Forms.Label();
            this.chkBatch = new System.Windows.Forms.CheckBox();
            this.chkOpenWhenComplete = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkRequireWSTIN = new System.Windows.Forms.CheckBox();
            this.rdoSelectedOnly = new System.Windows.Forms.RadioButton();
            this.rdoPrimaryOnly = new System.Windows.Forms.RadioButton();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.ucConfig = new CHaMPWorkbench.RBTConfiguration();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.ucRBTChangeDetection1 = new CHaMPWorkbench.RBT.InputFiles.ucRBTChangeDetection();
            this.cmdBrowseInputFile = new System.Windows.Forms.Button();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtSourceFolder = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.cmdBrowseSourceDataFolder = new System.Windows.Forms.Button();
            this.cmdBrowseOutputDataFolder = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cHAMP_WatershedsTableAdapter = new CHaMPWorkbench.RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter();
            this.cHAMP_SitesTableAdapter = new CHaMPWorkbench.RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter();
            this.cHAMP_VisitsTableAdapter = new CHaMPWorkbench.RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter();
            this.dlgFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.chkForcePrimary = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cHAMPVisitsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rBTWorkbenchDataSet)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cHAMPSitesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cHAMP_WatershedsBindingSource)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Watershed:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 48);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Site:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 32);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 13);
            this.label3.TabIndex = 0;
            this.label3.Text = "Primary visit:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkChangeDetection);
            this.groupBox1.Controls.Add(this.chkOrthogonal);
            this.groupBox1.Controls.Add(this.chkCalculateMetrics);
            this.groupBox1.Controls.Add(this.cboVisit);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Location = new System.Drawing.Point(17, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(510, 130);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Main Visit";
            // 
            // chkChangeDetection
            // 
            this.chkChangeDetection.AutoSize = true;
            this.chkChangeDetection.Location = new System.Drawing.Point(77, 106);
            this.chkChangeDetection.Name = "chkChangeDetection";
            this.chkChangeDetection.Size = new System.Drawing.Size(112, 17);
            this.chkChangeDetection.TabIndex = 4;
            this.chkChangeDetection.Text = "Change Detection";
            this.chkChangeDetection.UseVisualStyleBackColor = true;
            // 
            // chkOrthogonal
            // 
            this.chkOrthogonal.AutoSize = true;
            this.chkOrthogonal.Checked = true;
            this.chkOrthogonal.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOrthogonal.Location = new System.Drawing.Point(77, 82);
            this.chkOrthogonal.Name = "chkOrthogonal";
            this.chkOrthogonal.Size = new System.Drawing.Size(140, 17);
            this.chkOrthogonal.TabIndex = 3;
            this.chkOrthogonal.Text = "Make DEMs Orthogonal";
            this.chkOrthogonal.UseVisualStyleBackColor = true;
            // 
            // chkCalculateMetrics
            // 
            this.chkCalculateMetrics.AutoSize = true;
            this.chkCalculateMetrics.Checked = true;
            this.chkCalculateMetrics.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCalculateMetrics.Location = new System.Drawing.Point(77, 59);
            this.chkCalculateMetrics.Name = "chkCalculateMetrics";
            this.chkCalculateMetrics.Size = new System.Drawing.Size(107, 17);
            this.chkCalculateMetrics.TabIndex = 2;
            this.chkCalculateMetrics.Text = "Calculate Metrics";
            this.chkCalculateMetrics.UseVisualStyleBackColor = true;
            // 
            // cboVisit
            // 
            this.cboVisit.DataSource = this.cHAMPVisitsBindingSource;
            this.cboVisit.DisplayMember = "DisplayName";
            this.cboVisit.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboVisit.FormattingEnabled = true;
            this.cboVisit.Location = new System.Drawing.Point(79, 28);
            this.cboVisit.Name = "cboVisit";
            this.cboVisit.Size = new System.Drawing.Size(419, 21);
            this.cboVisit.TabIndex = 1;
            this.cboVisit.ValueMember = "VisitID";
            this.cboVisit.SelectedIndexChanged += new System.EventHandler(this.cboVisit_SelectedIndexChanged);
            // 
            // cHAMPVisitsBindingSource
            // 
            this.cHAMPVisitsBindingSource.DataMember = "CHAMP_Visits";
            this.cHAMPVisitsBindingSource.DataSource = this.rBTWorkbenchDataSet;
            // 
            // rBTWorkbenchDataSet
            // 
            this.rBTWorkbenchDataSet.DataSetName = "RBTWorkbenchDataSet";
            this.rBTWorkbenchDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // cHAMPSitesBindingSource
            // 
            this.cHAMPSitesBindingSource.DataMember = "CHAMP_Sites";
            this.cHAMPSitesBindingSource.DataSource = this.rBTWorkbenchDataSet;
            // 
            // cboSite
            // 
            this.cboSite.DataSource = this.cHAMPSitesBindingSource;
            this.cboSite.DisplayMember = "SiteName";
            this.cboSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboSite.FormattingEnabled = true;
            this.cboSite.Location = new System.Drawing.Point(96, 45);
            this.cboSite.Name = "cboSite";
            this.cboSite.Size = new System.Drawing.Size(419, 21);
            this.cboSite.TabIndex = 3;
            this.cboSite.ValueMember = "SiteID";
            this.cboSite.SelectedIndexChanged += new System.EventHandler(this.cboSite_SelectedIndexChanged);
            // 
            // cHAMP_WatershedsBindingSource
            // 
            this.cHAMP_WatershedsBindingSource.DataMember = "CHAMP_Watersheds";
            this.cHAMP_WatershedsBindingSource.DataSource = this.rBTWorkbenchDataSet;
            // 
            // cboWatershed
            // 
            this.cboWatershed.DataSource = this.cHAMP_WatershedsBindingSource;
            this.cboWatershed.DisplayMember = "WatershedName";
            this.cboWatershed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWatershed.FormattingEnabled = true;
            this.cboWatershed.Location = new System.Drawing.Point(96, 16);
            this.cboWatershed.Name = "cboWatershed";
            this.cboWatershed.Size = new System.Drawing.Size(419, 21);
            this.cboWatershed.TabIndex = 1;
            this.cboWatershed.ValueMember = "WatershedID";
            this.cboWatershed.SelectedIndexChanged += new System.EventHandler(this.cboWatershed_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(12, 104);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(761, 571);
            this.tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkCopyPath);
            this.tabPage1.Controls.Add(this.txtBatchName);
            this.tabPage1.Controls.Add(this.lblBatchName);
            this.tabPage1.Controls.Add(this.chkBatch);
            this.tabPage1.Controls.Add(this.chkOpenWhenComplete);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.cboSite);
            this.tabPage1.Controls.Add(this.cboWatershed);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(753, 545);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Site and Visit";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // chkCopyPath
            // 
            this.chkCopyPath.AutoSize = true;
            this.chkCopyPath.Checked = true;
            this.chkCopyPath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCopyPath.Location = new System.Drawing.Point(17, 396);
            this.chkCopyPath.Name = "chkCopyPath";
            this.chkCopyPath.Size = new System.Drawing.Size(290, 17);
            this.chkCopyPath.TabIndex = 10;
            this.chkCopyPath.Text = "Copy the path of the generated XML file to the clipbaord";
            this.chkCopyPath.UseVisualStyleBackColor = true;
            // 
            // txtBatchName
            // 
            this.txtBatchName.Location = new System.Drawing.Point(119, 338);
            this.txtBatchName.Name = "txtBatchName";
            this.txtBatchName.Size = new System.Drawing.Size(408, 20);
            this.txtBatchName.TabIndex = 9;
            // 
            // lblBatchName
            // 
            this.lblBatchName.AutoSize = true;
            this.lblBatchName.Location = new System.Drawing.Point(46, 342);
            this.lblBatchName.Name = "lblBatchName";
            this.lblBatchName.Size = new System.Drawing.Size(67, 13);
            this.lblBatchName.TabIndex = 8;
            this.lblBatchName.Text = "Batch name:";
            // 
            // chkBatch
            // 
            this.chkBatch.AutoSize = true;
            this.chkBatch.Checked = true;
            this.chkBatch.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkBatch.Location = new System.Drawing.Point(17, 318);
            this.chkBatch.Name = "chkBatch";
            this.chkBatch.Size = new System.Drawing.Size(188, 17);
            this.chkBatch.TabIndex = 7;
            this.chkBatch.Text = "Create RBT batch for this input file";
            this.chkBatch.UseVisualStyleBackColor = true;
            this.chkBatch.CheckedChanged += new System.EventHandler(this.chkBatch_CheckedChanged);
            // 
            // chkOpenWhenComplete
            // 
            this.chkOpenWhenComplete.AutoSize = true;
            this.chkOpenWhenComplete.Checked = true;
            this.chkOpenWhenComplete.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkOpenWhenComplete.Location = new System.Drawing.Point(17, 373);
            this.chkOpenWhenComplete.Name = "chkOpenWhenComplete";
            this.chkOpenWhenComplete.Size = new System.Drawing.Size(143, 17);
            this.chkOpenWhenComplete.TabIndex = 6;
            this.chkOpenWhenComplete.Text = "Open file when complete";
            this.chkOpenWhenComplete.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkForcePrimary);
            this.groupBox2.Controls.Add(this.chkRequireWSTIN);
            this.groupBox2.Controls.Add(this.rdoSelectedOnly);
            this.groupBox2.Controls.Add(this.rdoPrimaryOnly);
            this.groupBox2.Controls.Add(this.rdoAll);
            this.groupBox2.Location = new System.Drawing.Point(17, 219);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(510, 92);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Include Other Visits To This Site";
            // 
            // chkRequireWSTIN
            // 
            this.chkRequireWSTIN.AutoSize = true;
            this.chkRequireWSTIN.Checked = true;
            this.chkRequireWSTIN.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkRequireWSTIN.Location = new System.Drawing.Point(282, 64);
            this.chkRequireWSTIN.Name = "chkRequireWSTIN";
            this.chkRequireWSTIN.Size = new System.Drawing.Size(225, 17);
            this.chkRequireWSTIN.TabIndex = 3;
            this.chkRequireWSTIN.Text = "Require visits to have a water surface TIN";
            this.chkRequireWSTIN.UseVisualStyleBackColor = true;
            // 
            // rdoSelectedOnly
            // 
            this.rdoSelectedOnly.AutoSize = true;
            this.rdoSelectedOnly.Location = new System.Drawing.Point(77, 19);
            this.rdoSelectedOnly.Name = "rdoSelectedOnly";
            this.rdoSelectedOnly.Size = new System.Drawing.Size(128, 17);
            this.rdoSelectedOnly.TabIndex = 0;
            this.rdoSelectedOnly.Text = "Only the selected visit";
            this.rdoSelectedOnly.UseVisualStyleBackColor = true;
            // 
            // rdoPrimaryOnly
            // 
            this.rdoPrimaryOnly.AutoSize = true;
            this.rdoPrimaryOnly.Location = new System.Drawing.Point(77, 41);
            this.rdoPrimaryOnly.Name = "rdoPrimaryOnly";
            this.rdoPrimaryOnly.Size = new System.Drawing.Size(158, 17);
            this.rdoPrimaryOnly.TabIndex = 1;
            this.rdoPrimaryOnly.Text = "Only primary visits to this site";
            this.rdoPrimaryOnly.UseVisualStyleBackColor = true;
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = true;
            this.rdoAll.Checked = true;
            this.rdoAll.Location = new System.Drawing.Point(77, 64);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(112, 17);
            this.rdoAll.TabIndex = 2;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "All visits to this site";
            this.rdoAll.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.ucConfig);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(753, 545);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "RBT Configuration";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // ucConfig
            // 
            this.ucConfig.Location = new System.Drawing.Point(6, 6);
            this.ucConfig.Name = "ucConfig";
            this.ucConfig.Size = new System.Drawing.Size(743, 541);
            this.ucConfig.TabIndex = 0;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.ucRBTChangeDetection1);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(753, 545);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Change Detection";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // ucRBTChangeDetection1
            // 
            this.ucRBTChangeDetection1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucRBTChangeDetection1.Location = new System.Drawing.Point(3, 3);
            this.ucRBTChangeDetection1.Name = "ucRBTChangeDetection1";
            this.ucRBTChangeDetection1.Size = new System.Drawing.Size(747, 539);
            this.ucRBTChangeDetection1.TabIndex = 0;
            // 
            // cmdBrowseInputFile
            // 
            this.cmdBrowseInputFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseInputFile.Location = new System.Drawing.Point(697, 71);
            this.cmdBrowseInputFile.Name = "cmdBrowseInputFile";
            this.cmdBrowseInputFile.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseInputFile.TabIndex = 8;
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
            this.txtInputFile.Size = new System.Drawing.Size(468, 20);
            this.txtInputFile.TabIndex = 7;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(14, 76);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(203, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "RBT input XML file that will be generated:";
            // 
            // txtSourceFolder
            // 
            this.txtSourceFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtSourceFolder.Location = new System.Drawing.Point(223, 16);
            this.txtSourceFolder.Name = "txtSourceFolder";
            this.txtSourceFolder.Size = new System.Drawing.Size(468, 20);
            this.txtSourceFolder.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(88, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(129, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Parent source data folder:";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.Location = new System.Drawing.Point(223, 42);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(468, 20);
            this.txtOutputFolder.TabIndex = 4;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(90, 45);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(127, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Parent output data folder:";
            // 
            // cmdBrowseSourceDataFolder
            // 
            this.cmdBrowseSourceDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseSourceDataFolder.Location = new System.Drawing.Point(697, 14);
            this.cmdBrowseSourceDataFolder.Name = "cmdBrowseSourceDataFolder";
            this.cmdBrowseSourceDataFolder.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseSourceDataFolder.TabIndex = 2;
            this.cmdBrowseSourceDataFolder.Text = "Browse";
            this.cmdBrowseSourceDataFolder.UseVisualStyleBackColor = true;
            this.cmdBrowseSourceDataFolder.Click += new System.EventHandler(this.cmdBrowseSourceDataFolder_Click);
            // 
            // cmdBrowseOutputDataFolder
            // 
            this.cmdBrowseOutputDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseOutputDataFolder.Location = new System.Drawing.Point(697, 42);
            this.cmdBrowseOutputDataFolder.Name = "cmdBrowseOutputDataFolder";
            this.cmdBrowseOutputDataFolder.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseOutputDataFolder.TabIndex = 5;
            this.cmdBrowseOutputDataFolder.Text = "Browse";
            this.cmdBrowseOutputDataFolder.UseVisualStyleBackColor = true;
            this.cmdBrowseOutputDataFolder.Click += new System.EventHandler(this.cmdBrowseOutputDataFolder_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(616, 686);
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
            this.cmdCancel.Location = new System.Drawing.Point(697, 686);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 11;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cHAMP_WatershedsTableAdapter
            // 
            this.cHAMP_WatershedsTableAdapter.ClearBeforeFill = true;
            // 
            // cHAMP_SitesTableAdapter
            // 
            this.cHAMP_SitesTableAdapter.ClearBeforeFill = true;
            // 
            // cHAMP_VisitsTableAdapter
            // 
            this.cHAMP_VisitsTableAdapter.ClearBeforeFill = true;
            // 
            // chkForcePrimary
            // 
            this.chkForcePrimary.AutoSize = true;
            this.chkForcePrimary.Location = new System.Drawing.Point(282, 41);
            this.chkForcePrimary.Name = "chkForcePrimary";
            this.chkForcePrimary.Size = new System.Drawing.Size(155, 17);
            this.chkForcePrimary.TabIndex = 4;
            this.chkForcePrimary.Text = "Force all visits to be primary";
            this.chkForcePrimary.UseVisualStyleBackColor = true;
            // 
            // frmRBTInputSingle
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(783, 721);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdBrowseOutputDataFolder);
            this.Controls.Add(this.cmdBrowseSourceDataFolder);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtSourceFolder);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdBrowseInputFile);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRBTInputSingle";
            this.Text = "Configure Single RBT Run";
            this.Load += new System.EventHandler(this.frmRBTRun_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cHAMPVisitsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rBTWorkbenchDataSet)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cHAMPSitesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cHAMP_WatershedsBindingSource)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private RBTConfiguration ucConfig;
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
        internal System.Windows.Forms.Button cmdBrowseSourceDataFolder;
        internal System.Windows.Forms.Button cmdBrowseOutputDataFolder;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoSelectedOnly;
        private System.Windows.Forms.RadioButton rdoPrimaryOnly;
        private System.Windows.Forms.RadioButton rdoAll;
        private RBTWorkbenchDataSet rBTWorkbenchDataSet;
        private System.Windows.Forms.BindingSource cHAMP_WatershedsBindingSource;
        private RBTWorkbenchDataSetTableAdapters.CHAMP_WatershedsTableAdapter cHAMP_WatershedsTableAdapter;
        private System.Windows.Forms.BindingSource cHAMPSitesBindingSource;
        private RBTWorkbenchDataSetTableAdapters.CHAMP_SitesTableAdapter cHAMP_SitesTableAdapter;
        private System.Windows.Forms.BindingSource cHAMPVisitsBindingSource;
        private RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter cHAMP_VisitsTableAdapter;
        private System.Windows.Forms.FolderBrowserDialog dlgFolder;
        private System.Windows.Forms.CheckBox chkOpenWhenComplete;
        private System.Windows.Forms.TextBox txtBatchName;
        private System.Windows.Forms.Label lblBatchName;
        private System.Windows.Forms.CheckBox chkBatch;
        private System.Windows.Forms.TabPage tabPage3;
        private RBT.InputFiles.ucRBTChangeDetection ucRBTChangeDetection1;
        private System.Windows.Forms.CheckBox chkCopyPath;
        private System.Windows.Forms.CheckBox chkRequireWSTIN;
        private System.Windows.Forms.CheckBox chkForcePrimary;
    }
}