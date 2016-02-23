namespace CHaMPWorkbench
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.openDatabaseInAccessToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.prepareDatabaseForDeploymentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rBTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rBTToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.buildInputFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.selectBatchesToRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runRBTConsoleBatchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.scavengeRBTResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.validationReportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateGCDProjectFromCHaMPSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.habitatToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateBatchHabitatProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.writeSimulationResultsToCSVFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.delft3DToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cSVToRasterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.experimentalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutExperimentalToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ericWallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jamesHensleighToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kellyWhiteheadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hydroModelInputGeneratorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.extractRBTErrorsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.konradHaffenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.philipBaileyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testXPathReferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.queueBridgeCreekBatchesRBTRunsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportAWSLookupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.filterVisitsFromVisitIDCSVFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saraBangenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cHaMPWorkbenchWebSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkForUpdatesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutTheCHaMPWorkbenchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssDatabasePath = new System.Windows.Forms.ToolStripStatusLabel();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grpSite = new System.Windows.Forms.GroupBox();
            this.lstSite = new System.Windows.Forms.CheckedListBox();
            this.cmsSiteAllNone = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstWatershed = new System.Windows.Forms.CheckedListBox();
            this.cmsWatershed = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.selectAllToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.selectNoneToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.grpFieldSeason = new System.Windows.Forms.GroupBox();
            this.lstFieldSeason = new System.Windows.Forms.CheckedListBox();
            this.valVisitID = new System.Windows.Forms.NumericUpDown();
            this.chkVisitID = new System.Windows.Forms.CheckBox();
            this.grdVisits = new System.Windows.Forms.DataGridView();
            this.colWatershedID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWatershedName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFieldSeason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSiteName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVisitID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrganization = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHitchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCrewName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVisitPhase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVisitStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAEM = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colIsPrimary = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colQCVisit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHasStreamTempLogger = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHasFishData = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSiteID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSampleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPanel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCahnnelUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmsVisit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.visitPropertiesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseMonitoringDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.browseModelInputOutputFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.copyMonitoringDataFolderPathToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyModelInputOutputFolderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
            this.filterForAllVisitsToThisSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator11 = new System.Windows.Forms.ToolStripSeparator();
            this.generateChannelUnitCSVFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateRBTRunForThisVisitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator12 = new System.Windows.Forms.ToolStripSeparator();
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.viewSiteLocationMapToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectRandomNumberOfVisitsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpSite.SuspendLayout();
            this.cmsSiteAllNone.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.cmsWatershed.SuspendLayout();
            this.grpFieldSeason.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valVisitID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdVisits)).BeginInit();
            this.cmsVisit.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataToolStripMenuItem,
            this.rBTToolStripMenuItem,
            this.experimentalToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(784, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDatabaseToolStripMenuItem,
            this.closeDatabaseToolStripMenuItem,
            this.toolStripSeparator6,
            this.openDatabaseInAccessToolStripMenuItem,
            this.toolStripSeparator7,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openDatabaseToolStripMenuItem
            // 
            this.openDatabaseToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.database;
            this.openDatabaseToolStripMenuItem.Name = "openDatabaseToolStripMenuItem";
            this.openDatabaseToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.openDatabaseToolStripMenuItem.Text = "Open Database...";
            this.openDatabaseToolStripMenuItem.Click += new System.EventHandler(this.openDatabaseToolStripMenuItem_Click);
            // 
            // closeDatabaseToolStripMenuItem
            // 
            this.closeDatabaseToolStripMenuItem.Name = "closeDatabaseToolStripMenuItem";
            this.closeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.closeDatabaseToolStripMenuItem.Text = "Close Database";
            this.closeDatabaseToolStripMenuItem.Click += new System.EventHandler(this.closeDatabaseToolStripMenuItem_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(212, 6);
            // 
            // openDatabaseInAccessToolStripMenuItem
            // 
            this.openDatabaseInAccessToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.Access1;
            this.openDatabaseInAccessToolStripMenuItem.Name = "openDatabaseInAccessToolStripMenuItem";
            this.openDatabaseInAccessToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.openDatabaseInAccessToolStripMenuItem.Text = "Open Database in Access...";
            this.openDatabaseInAccessToolStripMenuItem.Click += new System.EventHandler(this.openDatabaseInAccessToolStripMenuItem_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(212, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(215, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem,
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem,
            this.toolStripSeparator4,
            this.prepareDatabaseForDeploymentToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // unpackMonitoringData7ZipArchiveToolStripMenuItem
            // 
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.zip;
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem.Name = "unpackMonitoringData7ZipArchiveToolStripMenuItem";
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem.Size = new System.Drawing.Size(429, 22);
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem.Text = "Unpack Monitoring Data Zip Archives...";
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem.Click += new System.EventHandler(this.unpackMonitoringData7ZipArchiveToolStripMenuItem_Click);
            // 
            // scavengeVisitDataFromCHaMPExportToolStripMenuItem
            // 
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.import;
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem.Name = "scavengeVisitDataFromCHaMPExportToolStripMenuItem";
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem.Size = new System.Drawing.Size(429, 22);
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem.Text = "Import CHaMP Watershed, Site and Visit Data From CHaMP Exports";
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem.Click += new System.EventHandler(this.scavengeVisitDataFromCHaMPExportToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(426, 6);
            // 
            // prepareDatabaseForDeploymentToolStripMenuItem
            // 
            this.prepareDatabaseForDeploymentToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.database;
            this.prepareDatabaseForDeploymentToolStripMenuItem.Name = "prepareDatabaseForDeploymentToolStripMenuItem";
            this.prepareDatabaseForDeploymentToolStripMenuItem.Size = new System.Drawing.Size(429, 22);
            this.prepareDatabaseForDeploymentToolStripMenuItem.Text = "Manage Workbench Database Contents...";
            this.prepareDatabaseForDeploymentToolStripMenuItem.Click += new System.EventHandler(this.prepareDatabaseForDeploymentToolStripMenuItem_Click);
            // 
            // rBTToolStripMenuItem
            // 
            this.rBTToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rBTToolStripMenuItem1,
            this.gCDToolStripMenuItem,
            this.habitatToolStripMenuItem,
            this.delft3DToolStripMenuItem,
            this.toolStripSeparator1,
            this.optionsToolStripMenuItem});
            this.rBTToolStripMenuItem.Name = "rBTToolStripMenuItem";
            this.rBTToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.rBTToolStripMenuItem.Text = "Tools";
            // 
            // rBTToolStripMenuItem1
            // 
            this.rBTToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.buildInputFilesToolStripMenuItem,
            this.toolStripSeparator2,
            this.selectBatchesToRunToolStripMenuItem,
            this.runRBTConsoleBatchesToolStripMenuItem,
            this.toolStripSeparator3,
            this.scavengeRBTResultsToolStripMenuItem,
            this.validationReportToolStripMenuItem});
            this.rBTToolStripMenuItem1.Image = global::CHaMPWorkbench.Properties.Resources.RBT_Dark_32x32;
            this.rBTToolStripMenuItem1.Name = "rBTToolStripMenuItem1";
            this.rBTToolStripMenuItem1.Size = new System.Drawing.Size(125, 22);
            this.rBTToolStripMenuItem1.Text = "RBT";
            // 
            // buildInputFilesToolStripMenuItem
            // 
            this.buildInputFilesToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.xml;
            this.buildInputFilesToolStripMenuItem.Name = "buildInputFilesToolStripMenuItem";
            this.buildInputFilesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.buildInputFilesToolStripMenuItem.Text = "Build Input File(s)...";
            this.buildInputFilesToolStripMenuItem.Click += new System.EventHandler(this.buildInputFilesToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(210, 6);
            // 
            // selectBatchesToRunToolStripMenuItem
            // 
            this.selectBatchesToRunToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.CheckControlNetwork;
            this.selectBatchesToRunToolStripMenuItem.Name = "selectBatchesToRunToolStripMenuItem";
            this.selectBatchesToRunToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.selectBatchesToRunToolStripMenuItem.Text = "Select Batches to Run...";
            this.selectBatchesToRunToolStripMenuItem.Click += new System.EventHandler(this.selectBatchesToRunToolStripMenuItem_Click);
            // 
            // runRBTConsoleBatchesToolStripMenuItem
            // 
            this.runRBTConsoleBatchesToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.RBT_Dark_32x32;
            this.runRBTConsoleBatchesToolStripMenuItem.Name = "runRBTConsoleBatchesToolStripMenuItem";
            this.runRBTConsoleBatchesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.runRBTConsoleBatchesToolStripMenuItem.Text = "Run Selected Batch Runs...";
            this.runRBTConsoleBatchesToolStripMenuItem.Click += new System.EventHandler(this.runRBTConsoleBatchesToolStripMenuItem_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(210, 6);
            // 
            // scavengeRBTResultsToolStripMenuItem
            // 
            this.scavengeRBTResultsToolStripMenuItem.Name = "scavengeRBTResultsToolStripMenuItem";
            this.scavengeRBTResultsToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.scavengeRBTResultsToolStripMenuItem.Text = "Scavenge RBT Results...";
            this.scavengeRBTResultsToolStripMenuItem.Click += new System.EventHandler(this.scavengeRBTResultsToolStripMenuItem_Click);
            // 
            // validationReportToolStripMenuItem
            // 
            this.validationReportToolStripMenuItem.Name = "validationReportToolStripMenuItem";
            this.validationReportToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.validationReportToolStripMenuItem.Text = "Validation Report...";
            this.validationReportToolStripMenuItem.Click += new System.EventHandler(this.validationReportToolStripMenuItem_Click);
            // 
            // gCDToolStripMenuItem
            // 
            this.gCDToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateGCDProjectFromCHaMPSiteToolStripMenuItem});
            this.gCDToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.gcd_icon;
            this.gCDToolStripMenuItem.Name = "gCDToolStripMenuItem";
            this.gCDToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.gCDToolStripMenuItem.Text = "GCD";
            this.gCDToolStripMenuItem.Visible = false;
            // 
            // generateGCDProjectFromCHaMPSiteToolStripMenuItem
            // 
            this.generateGCDProjectFromCHaMPSiteToolStripMenuItem.Name = "generateGCDProjectFromCHaMPSiteToolStripMenuItem";
            this.generateGCDProjectFromCHaMPSiteToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.generateGCDProjectFromCHaMPSiteToolStripMenuItem.Text = "Generate GCD Project for CHaMP Site";
            // 
            // habitatToolStripMenuItem
            // 
            this.habitatToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateBatchHabitatProjectToolStripMenuItem,
            this.writeSimulationResultsToCSVFileToolStripMenuItem});
            this.habitatToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources._32_habitat_logo;
            this.habitatToolStripMenuItem.Name = "habitatToolStripMenuItem";
            this.habitatToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.habitatToolStripMenuItem.Text = "Habitat";
            // 
            // generateBatchHabitatProjectToolStripMenuItem
            // 
            this.generateBatchHabitatProjectToolStripMenuItem.Name = "generateBatchHabitatProjectToolStripMenuItem";
            this.generateBatchHabitatProjectToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.generateBatchHabitatProjectToolStripMenuItem.Text = "Generate Batch Habitat Project...";
            this.generateBatchHabitatProjectToolStripMenuItem.Click += new System.EventHandler(this.generateBatchHabitatProjectToolStripMenuItem_Click);
            // 
            // writeSimulationResultsToCSVFileToolStripMenuItem
            // 
            this.writeSimulationResultsToCSVFileToolStripMenuItem.Name = "writeSimulationResultsToCSVFileToolStripMenuItem";
            this.writeSimulationResultsToCSVFileToolStripMenuItem.Size = new System.Drawing.Size(264, 22);
            this.writeSimulationResultsToCSVFileToolStripMenuItem.Text = "Write simulation results to CSV file...";
            this.writeSimulationResultsToCSVFileToolStripMenuItem.Click += new System.EventHandler(this.writeSimulationResultsToCSVFileToolStripMenuItem_Click);
            // 
            // delft3DToolStripMenuItem
            // 
            this.delft3DToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cSVToRasterToolStripMenuItem});
            this.delft3DToolStripMenuItem.Name = "delft3DToolStripMenuItem";
            this.delft3DToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.delft3DToolStripMenuItem.Text = "Delft 3D";
            this.delft3DToolStripMenuItem.Visible = false;
            // 
            // cSVToRasterToolStripMenuItem
            // 
            this.cSVToRasterToolStripMenuItem.Name = "cSVToRasterToolStripMenuItem";
            this.cSVToRasterToolStripMenuItem.Size = new System.Drawing.Size(156, 22);
            this.cSVToRasterToolStripMenuItem.Text = "CSV To Raster...";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(122, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.Settings;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(125, 22);
            this.optionsToolStripMenuItem.Text = "Options...";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // experimentalToolStripMenuItem
            // 
            this.experimentalToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aboutExperimentalToolsToolStripMenuItem,
            this.toolStripSeparator5,
            this.ericWallToolStripMenuItem,
            this.jamesHensleighToolStripMenuItem,
            this.kellyWhiteheadToolStripMenuItem,
            this.konradHaffenToolStripMenuItem,
            this.philipBaileyToolStripMenuItem,
            this.saraBangenToolStripMenuItem});
            this.experimentalToolStripMenuItem.Name = "experimentalToolStripMenuItem";
            this.experimentalToolStripMenuItem.Size = new System.Drawing.Size(87, 20);
            this.experimentalToolStripMenuItem.Text = "Experimental";
            // 
            // aboutExperimentalToolsToolStripMenuItem
            // 
            this.aboutExperimentalToolsToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.Help;
            this.aboutExperimentalToolsToolStripMenuItem.Name = "aboutExperimentalToolsToolStripMenuItem";
            this.aboutExperimentalToolsToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.aboutExperimentalToolsToolStripMenuItem.Text = "About Experimental Tools...";
            this.aboutExperimentalToolsToolStripMenuItem.Click += new System.EventHandler(this.aboutExperimentalToolsToolStripMenuItem_Click);
            // 
            // toolStripSeparator5
            // 
            this.toolStripSeparator5.Name = "toolStripSeparator5";
            this.toolStripSeparator5.Size = new System.Drawing.Size(216, 6);
            // 
            // ericWallToolStripMenuItem
            // 
            this.ericWallToolStripMenuItem.Name = "ericWallToolStripMenuItem";
            this.ericWallToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.ericWallToolStripMenuItem.Text = "Eric Wall";
            // 
            // jamesHensleighToolStripMenuItem
            // 
            this.jamesHensleighToolStripMenuItem.Name = "jamesHensleighToolStripMenuItem";
            this.jamesHensleighToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.jamesHensleighToolStripMenuItem.Text = "James Hensleigh";
            // 
            // kellyWhiteheadToolStripMenuItem
            // 
            this.kellyWhiteheadToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.hydroModelInputGeneratorToolStripMenuItem,
            this.extractRBTErrorsToolStripMenuItem});
            this.kellyWhiteheadToolStripMenuItem.Name = "kellyWhiteheadToolStripMenuItem";
            this.kellyWhiteheadToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.kellyWhiteheadToolStripMenuItem.Text = "Kelly Whitehead";
            // 
            // hydroModelInputGeneratorToolStripMenuItem
            // 
            this.hydroModelInputGeneratorToolStripMenuItem.Name = "hydroModelInputGeneratorToolStripMenuItem";
            this.hydroModelInputGeneratorToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.hydroModelInputGeneratorToolStripMenuItem.Text = "Hydro Model Input Generator";
            this.hydroModelInputGeneratorToolStripMenuItem.Click += new System.EventHandler(this.hydroModelInputGeneratorToolStripMenuItem_Click);
            // 
            // extractRBTErrorsToolStripMenuItem
            // 
            this.extractRBTErrorsToolStripMenuItem.Name = "extractRBTErrorsToolStripMenuItem";
            this.extractRBTErrorsToolStripMenuItem.Size = new System.Drawing.Size(230, 22);
            this.extractRBTErrorsToolStripMenuItem.Text = "Extract RBT Errors";
            this.extractRBTErrorsToolStripMenuItem.Click += new System.EventHandler(this.extractRBTErrorsToolStripMenuItem_Click);
            // 
            // konradHaffenToolStripMenuItem
            // 
            this.konradHaffenToolStripMenuItem.Name = "konradHaffenToolStripMenuItem";
            this.konradHaffenToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.konradHaffenToolStripMenuItem.Text = "Konrad Hafen";
            // 
            // philipBaileyToolStripMenuItem
            // 
            this.philipBaileyToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.testXPathReferencesToolStripMenuItem,
            this.queueBridgeCreekBatchesRBTRunsToolStripMenuItem,
            this.exportAWSLookupToolStripMenuItem,
            this.filterVisitsFromVisitIDCSVFileToolStripMenuItem});
            this.philipBaileyToolStripMenuItem.Name = "philipBaileyToolStripMenuItem";
            this.philipBaileyToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.philipBaileyToolStripMenuItem.Text = "Philip Bailey";
            // 
            // testXPathReferencesToolStripMenuItem
            // 
            this.testXPathReferencesToolStripMenuItem.Name = "testXPathReferencesToolStripMenuItem";
            this.testXPathReferencesToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.testXPathReferencesToolStripMenuItem.Text = "Test XPath References";
            this.testXPathReferencesToolStripMenuItem.Click += new System.EventHandler(this.testXPathReferencesToolStripMenuItem_Click);
            // 
            // queueBridgeCreekBatchesRBTRunsToolStripMenuItem
            // 
            this.queueBridgeCreekBatchesRBTRunsToolStripMenuItem.Enabled = false;
            this.queueBridgeCreekBatchesRBTRunsToolStripMenuItem.Name = "queueBridgeCreekBatchesRBTRunsToolStripMenuItem";
            this.queueBridgeCreekBatchesRBTRunsToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.queueBridgeCreekBatchesRBTRunsToolStripMenuItem.Text = "Queue Bridge Creek Batches RBT Runs";
            this.queueBridgeCreekBatchesRBTRunsToolStripMenuItem.Click += new System.EventHandler(this.queueBridgeCreekBatchesRBTRunsToolStripMenuItem_Click);
            // 
            // exportAWSLookupToolStripMenuItem
            // 
            this.exportAWSLookupToolStripMenuItem.Name = "exportAWSLookupToolStripMenuItem";
            this.exportAWSLookupToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.exportAWSLookupToolStripMenuItem.Text = "Export AWS Lookup...";
            this.exportAWSLookupToolStripMenuItem.Click += new System.EventHandler(this.exportAWSLookupToolStripMenuItem_Click);
            // 
            // filterVisitsFromVisitIDCSVFileToolStripMenuItem
            // 
            this.filterVisitsFromVisitIDCSVFileToolStripMenuItem.Name = "filterVisitsFromVisitIDCSVFileToolStripMenuItem";
            this.filterVisitsFromVisitIDCSVFileToolStripMenuItem.Size = new System.Drawing.Size(276, 22);
            this.filterVisitsFromVisitIDCSVFileToolStripMenuItem.Text = "Filter Visits From Visit ID CSV file...";
            this.filterVisitsFromVisitIDCSVFileToolStripMenuItem.Click += new System.EventHandler(this.filterVisitsFromVisitIDCSVFileToolStripMenuItem_Click);
            // 
            // saraBangenToolStripMenuItem
            // 
            this.saraBangenToolStripMenuItem.Name = "saraBangenToolStripMenuItem";
            this.saraBangenToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.saraBangenToolStripMenuItem.Text = "Sara Bangen";
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cHaMPWorkbenchWebSiteToolStripMenuItem,
            this.checkForUpdatesToolStripMenuItem,
            this.aboutTheCHaMPWorkbenchToolStripMenuItem});
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.aboutToolStripMenuItem.Text = "About";
            // 
            // cHaMPWorkbenchWebSiteToolStripMenuItem
            // 
            this.cHaMPWorkbenchWebSiteToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.WebSite;
            this.cHaMPWorkbenchWebSiteToolStripMenuItem.Name = "cHaMPWorkbenchWebSiteToolStripMenuItem";
            this.cHaMPWorkbenchWebSiteToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.cHaMPWorkbenchWebSiteToolStripMenuItem.Text = "CHaMP Workbench Web Site";
            this.cHaMPWorkbenchWebSiteToolStripMenuItem.Click += new System.EventHandler(this.cHaMPWorkbenchWebSiteToolStripMenuItem_Click);
            // 
            // checkForUpdatesToolStripMenuItem
            // 
            this.checkForUpdatesToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.update;
            this.checkForUpdatesToolStripMenuItem.Name = "checkForUpdatesToolStripMenuItem";
            this.checkForUpdatesToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.checkForUpdatesToolStripMenuItem.Text = "Check for updates";
            this.checkForUpdatesToolStripMenuItem.Click += new System.EventHandler(this.checkForUpdatesToolStripMenuItem_Click);
            // 
            // aboutTheCHaMPWorkbenchToolStripMenuItem
            // 
            this.aboutTheCHaMPWorkbenchToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.CHaMP_Logo_32;
            this.aboutTheCHaMPWorkbenchToolStripMenuItem.Name = "aboutTheCHaMPWorkbenchToolStripMenuItem";
            this.aboutTheCHaMPWorkbenchToolStripMenuItem.Size = new System.Drawing.Size(244, 22);
            this.aboutTheCHaMPWorkbenchToolStripMenuItem.Text = "About the CHaMP Workbench...";
            this.aboutTheCHaMPWorkbenchToolStripMenuItem.Click += new System.EventHandler(this.aboutTheCHaMPWorkbenchToolStripMenuItem_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tssDatabasePath});
            this.statusStrip1.Location = new System.Drawing.Point(0, 539);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(784, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssDatabasePath
            // 
            this.tssDatabasePath.Name = "tssDatabasePath";
            this.tssDatabasePath.Size = new System.Drawing.Size(118, 17);
            this.tssDatabasePath.Text = "toolStripStatusLabel1";
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.IsSplitterFixed = true;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.grpSite);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.grpFieldSeason);
            this.splitContainer1.Panel1.Controls.Add(this.valVisitID);
            this.splitContainer1.Panel1.Controls.Add(this.chkVisitID);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdVisits);
            this.splitContainer1.Size = new System.Drawing.Size(784, 515);
            this.splitContainer1.SplitterDistance = 193;
            this.splitContainer1.TabIndex = 2;
            // 
            // grpSite
            // 
            this.grpSite.Controls.Add(this.lstSite);
            this.grpSite.Location = new System.Drawing.Point(12, 332);
            this.grpSite.Name = "grpSite";
            this.grpSite.Size = new System.Drawing.Size(161, 166);
            this.grpSite.TabIndex = 4;
            this.grpSite.TabStop = false;
            this.grpSite.Text = "Sites";
            // 
            // lstSite
            // 
            this.lstSite.CheckOnClick = true;
            this.lstSite.ContextMenuStrip = this.cmsSiteAllNone;
            this.lstSite.FormattingEnabled = true;
            this.lstSite.Location = new System.Drawing.Point(7, 20);
            this.lstSite.Name = "lstSite";
            this.lstSite.Size = new System.Drawing.Size(148, 139);
            this.lstSite.TabIndex = 0;
            // 
            // cmsSiteAllNone
            // 
            this.cmsSiteAllNone.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem,
            this.selectNoneToolStripMenuItem});
            this.cmsSiteAllNone.Name = "cmsAllNone";
            this.cmsSiteAllNone.Size = new System.Drawing.Size(138, 48);
            // 
            // selectAllToolStripMenuItem
            // 
            this.selectAllToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.SelectAll;
            this.selectAllToolStripMenuItem.Name = "selectAllToolStripMenuItem";
            this.selectAllToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.selectAllToolStripMenuItem.Text = "Select All";
            this.selectAllToolStripMenuItem.Click += new System.EventHandler(this.AllNoneSitesClick);
            // 
            // selectNoneToolStripMenuItem
            // 
            this.selectNoneToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.SelectNone;
            this.selectNoneToolStripMenuItem.Name = "selectNoneToolStripMenuItem";
            this.selectNoneToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.selectNoneToolStripMenuItem.Text = "Select None";
            this.selectNoneToolStripMenuItem.Click += new System.EventHandler(this.AllNoneSitesClick);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstWatershed);
            this.groupBox1.Location = new System.Drawing.Point(13, 160);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(161, 166);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Watersheds";
            // 
            // lstWatershed
            // 
            this.lstWatershed.CheckOnClick = true;
            this.lstWatershed.ContextMenuStrip = this.cmsWatershed;
            this.lstWatershed.FormattingEnabled = true;
            this.lstWatershed.Location = new System.Drawing.Point(7, 20);
            this.lstWatershed.Name = "lstWatershed";
            this.lstWatershed.Size = new System.Drawing.Size(148, 139);
            this.lstWatershed.TabIndex = 0;
            // 
            // cmsWatershed
            // 
            this.cmsWatershed.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectAllToolStripMenuItem1,
            this.selectNoneToolStripMenuItem1});
            this.cmsWatershed.Name = "cmsWatershed";
            this.cmsWatershed.Size = new System.Drawing.Size(138, 48);
            // 
            // selectAllToolStripMenuItem1
            // 
            this.selectAllToolStripMenuItem1.Image = global::CHaMPWorkbench.Properties.Resources.SelectAll;
            this.selectAllToolStripMenuItem1.Name = "selectAllToolStripMenuItem1";
            this.selectAllToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.selectAllToolStripMenuItem1.Text = "Select All";
            this.selectAllToolStripMenuItem1.Click += new System.EventHandler(this.AllNoneWatershedsClick);
            // 
            // selectNoneToolStripMenuItem1
            // 
            this.selectNoneToolStripMenuItem1.Image = global::CHaMPWorkbench.Properties.Resources.SelectNone;
            this.selectNoneToolStripMenuItem1.Name = "selectNoneToolStripMenuItem1";
            this.selectNoneToolStripMenuItem1.Size = new System.Drawing.Size(137, 22);
            this.selectNoneToolStripMenuItem1.Text = "Select None";
            this.selectNoneToolStripMenuItem1.Click += new System.EventHandler(this.AllNoneWatershedsClick);
            // 
            // grpFieldSeason
            // 
            this.grpFieldSeason.Controls.Add(this.lstFieldSeason);
            this.grpFieldSeason.Location = new System.Drawing.Point(13, 40);
            this.grpFieldSeason.Name = "grpFieldSeason";
            this.grpFieldSeason.Size = new System.Drawing.Size(161, 108);
            this.grpFieldSeason.TabIndex = 2;
            this.grpFieldSeason.TabStop = false;
            this.grpFieldSeason.Text = "Field Seasons";
            // 
            // lstFieldSeason
            // 
            this.lstFieldSeason.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstFieldSeason.CheckOnClick = true;
            this.lstFieldSeason.FormattingEnabled = true;
            this.lstFieldSeason.Location = new System.Drawing.Point(7, 20);
            this.lstFieldSeason.Name = "lstFieldSeason";
            this.lstFieldSeason.Size = new System.Drawing.Size(148, 79);
            this.lstFieldSeason.TabIndex = 0;
            // 
            // valVisitID
            // 
            this.valVisitID.Location = new System.Drawing.Point(79, 14);
            this.valVisitID.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.valVisitID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valVisitID.Name = "valVisitID";
            this.valVisitID.Size = new System.Drawing.Size(95, 20);
            this.valVisitID.TabIndex = 1;
            this.valVisitID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valVisitID.ValueChanged += new System.EventHandler(this.valVisitID_ValueChanged);
            // 
            // chkVisitID
            // 
            this.chkVisitID.AutoSize = true;
            this.chkVisitID.Location = new System.Drawing.Point(13, 16);
            this.chkVisitID.Name = "chkVisitID";
            this.chkVisitID.Size = new System.Drawing.Size(59, 17);
            this.chkVisitID.TabIndex = 0;
            this.chkVisitID.Text = "Visit ID";
            this.chkVisitID.UseVisualStyleBackColor = true;
            this.chkVisitID.CheckedChanged += new System.EventHandler(this.FilterVisits);
            // 
            // grdVisits
            // 
            this.grdVisits.AllowUserToAddRows = false;
            this.grdVisits.AllowUserToDeleteRows = false;
            this.grdVisits.AllowUserToResizeRows = false;
            this.grdVisits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdVisits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWatershedID,
            this.colWatershedName,
            this.colFieldSeason,
            this.colSiteName,
            this.colVisitID,
            this.colOrganization,
            this.colHitchName,
            this.colCrewName,
            this.colVisitPhase,
            this.colVisitStatus,
            this.colAEM,
            this.colIsPrimary,
            this.colQCVisit,
            this.colHasStreamTempLogger,
            this.colHasFishData,
            this.colCategoryName,
            this.colSiteID,
            this.colSampleDate,
            this.colPanel,
            this.colCahnnelUnits});
            this.grdVisits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdVisits.Location = new System.Drawing.Point(0, 0);
            this.grdVisits.Name = "grdVisits";
            this.grdVisits.ReadOnly = true;
            this.grdVisits.RowHeadersVisible = false;
            this.grdVisits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdVisits.Size = new System.Drawing.Size(587, 515);
            this.grdVisits.TabIndex = 0;
            this.grdVisits.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grdVisits_CellMouseClick);
            // 
            // colWatershedID
            // 
            this.colWatershedID.DataPropertyName = "WatershedID";
            this.colWatershedID.HeaderText = "Watershed ID";
            this.colWatershedID.Name = "colWatershedID";
            this.colWatershedID.ReadOnly = true;
            this.colWatershedID.Visible = false;
            // 
            // colWatershedName
            // 
            this.colWatershedName.DataPropertyName = "WatershedName";
            this.colWatershedName.HeaderText = "Watershed";
            this.colWatershedName.Name = "colWatershedName";
            this.colWatershedName.ReadOnly = true;
            // 
            // colFieldSeason
            // 
            this.colFieldSeason.DataPropertyName = "VisitYear";
            this.colFieldSeason.HeaderText = "Field Season";
            this.colFieldSeason.Name = "colFieldSeason";
            this.colFieldSeason.ReadOnly = true;
            // 
            // colSiteName
            // 
            this.colSiteName.DataPropertyName = "SiteName";
            this.colSiteName.HeaderText = "Site";
            this.colSiteName.Name = "colSiteName";
            this.colSiteName.ReadOnly = true;
            // 
            // colVisitID
            // 
            this.colVisitID.DataPropertyName = "VisitID";
            this.colVisitID.HeaderText = "Visit ID";
            this.colVisitID.Name = "colVisitID";
            this.colVisitID.ReadOnly = true;
            // 
            // colOrganization
            // 
            this.colOrganization.DataPropertyName = "Organization";
            this.colOrganization.HeaderText = "Organization";
            this.colOrganization.Name = "colOrganization";
            this.colOrganization.ReadOnly = true;
            // 
            // colHitchName
            // 
            this.colHitchName.DataPropertyName = "HitchName";
            this.colHitchName.HeaderText = "Hitch";
            this.colHitchName.Name = "colHitchName";
            this.colHitchName.ReadOnly = true;
            // 
            // colCrewName
            // 
            this.colCrewName.DataPropertyName = "CrewName";
            this.colCrewName.HeaderText = "Crew";
            this.colCrewName.Name = "colCrewName";
            this.colCrewName.ReadOnly = true;
            // 
            // colVisitPhase
            // 
            this.colVisitPhase.DataPropertyName = "VisitPhase";
            this.colVisitPhase.HeaderText = "Visit Phase";
            this.colVisitPhase.Name = "colVisitPhase";
            this.colVisitPhase.ReadOnly = true;
            // 
            // colVisitStatus
            // 
            this.colVisitStatus.DataPropertyName = "VisitStatus";
            this.colVisitStatus.HeaderText = "Visit Status";
            this.colVisitStatus.Name = "colVisitStatus";
            this.colVisitStatus.ReadOnly = true;
            // 
            // colAEM
            // 
            this.colAEM.DataPropertyName = "AEM";
            this.colAEM.HeaderText = "AEM";
            this.colAEM.Name = "colAEM";
            this.colAEM.ReadOnly = true;
            this.colAEM.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colAEM.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colIsPrimary
            // 
            this.colIsPrimary.DataPropertyName = "IsPrimary";
            this.colIsPrimary.HeaderText = "Primary";
            this.colIsPrimary.Name = "colIsPrimary";
            this.colIsPrimary.ReadOnly = true;
            // 
            // colQCVisit
            // 
            this.colQCVisit.DataPropertyName = "QCVisit";
            this.colQCVisit.HeaderText = "QC Visit";
            this.colQCVisit.Name = "colQCVisit";
            this.colQCVisit.ReadOnly = true;
            this.colQCVisit.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colQCVisit.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colHasStreamTempLogger
            // 
            this.colHasStreamTempLogger.DataPropertyName = "StreamTempLogger";
            this.colHasStreamTempLogger.HeaderText = "Stream Temp Logger";
            this.colHasStreamTempLogger.Name = "colHasStreamTempLogger";
            this.colHasStreamTempLogger.ReadOnly = true;
            this.colHasStreamTempLogger.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colHasStreamTempLogger.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colHasFishData
            // 
            this.colHasFishData.DataPropertyName = "HasFishData";
            this.colHasFishData.HeaderText = "Has Fish Data";
            this.colHasFishData.Name = "colHasFishData";
            this.colHasFishData.ReadOnly = true;
            this.colHasFishData.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.colHasFishData.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // colCategoryName
            // 
            this.colCategoryName.DataPropertyName = "CategoryName";
            this.colCategoryName.HeaderText = "Category Name";
            this.colCategoryName.Name = "colCategoryName";
            this.colCategoryName.ReadOnly = true;
            // 
            // colSiteID
            // 
            this.colSiteID.DataPropertyName = "SiteID";
            this.colSiteID.HeaderText = "Site ID";
            this.colSiteID.Name = "colSiteID";
            this.colSiteID.ReadOnly = true;
            this.colSiteID.Visible = false;
            // 
            // colSampleDate
            // 
            this.colSampleDate.DataPropertyName = "SampleDate";
            this.colSampleDate.HeaderText = "Sample Date";
            this.colSampleDate.Name = "colSampleDate";
            this.colSampleDate.ReadOnly = true;
            // 
            // colPanel
            // 
            this.colPanel.DataPropertyName = "PanelName";
            this.colPanel.HeaderText = "Panel";
            this.colPanel.Name = "colPanel";
            this.colPanel.ReadOnly = true;
            // 
            // colCahnnelUnits
            // 
            this.colCahnnelUnits.DataPropertyName = "ChannelUnits";
            this.colCahnnelUnits.HeaderText = "Channel Units";
            this.colCahnnelUnits.Name = "colCahnnelUnits";
            this.colCahnnelUnits.ReadOnly = true;
            // 
            // cmsVisit
            // 
            this.cmsVisit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.visitPropertiesToolStripMenuItem,
            this.browseMonitoringDataToolStripMenuItem,
            this.browseModelInputOutputFolderToolStripMenuItem,
            this.toolStripSeparator9,
            this.copyMonitoringDataFolderPathToolStripMenuItem,
            this.copyModelInputOutputFolderToolStripMenuItem,
            this.toolStripSeparator10,
            this.filterForAllVisitsToThisSiteToolStripMenuItem,
            this.selectRandomNumberOfVisitsToolStripMenuItem,
            this.toolStripSeparator11,
            this.generateChannelUnitCSVFileToolStripMenuItem,
            this.generateRBTRunForThisVisitToolStripMenuItem,
            this.toolStripSeparator12,
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem,
            this.viewSiteLocationMapToolStripMenuItem});
            this.cmsVisit.Name = "cmsVisit";
            this.cmsVisit.Size = new System.Drawing.Size(357, 292);
            // 
            // visitPropertiesToolStripMenuItem
            // 
            this.visitPropertiesToolStripMenuItem.Enabled = false;
            this.visitPropertiesToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.Settings;
            this.visitPropertiesToolStripMenuItem.Name = "visitPropertiesToolStripMenuItem";
            this.visitPropertiesToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
            this.visitPropertiesToolStripMenuItem.Text = "Visit Properties...";
            // 
            // browseMonitoringDataToolStripMenuItem
            // 
            this.browseMonitoringDataToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.explorer;
            this.browseMonitoringDataToolStripMenuItem.Name = "browseMonitoringDataToolStripMenuItem";
            this.browseMonitoringDataToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
            this.browseMonitoringDataToolStripMenuItem.Text = "Browse Monitoring Data Folder...";
            this.browseMonitoringDataToolStripMenuItem.Click += new System.EventHandler(this.browseMonitoringDataToolStripMenuItem_Click);
            // 
            // browseModelInputOutputFolderToolStripMenuItem
            // 
            this.browseModelInputOutputFolderToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.explorer;
            this.browseModelInputOutputFolderToolStripMenuItem.Name = "browseModelInputOutputFolderToolStripMenuItem";
            this.browseModelInputOutputFolderToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
            this.browseModelInputOutputFolderToolStripMenuItem.Text = "Browse Model Input/Output Folder...";
            this.browseModelInputOutputFolderToolStripMenuItem.Click += new System.EventHandler(this.browseModelInputOutputFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator9
            // 
            this.toolStripSeparator9.Name = "toolStripSeparator9";
            this.toolStripSeparator9.Size = new System.Drawing.Size(353, 6);
            // 
            // copyMonitoringDataFolderPathToolStripMenuItem
            // 
            this.copyMonitoringDataFolderPathToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.Copy;
            this.copyMonitoringDataFolderPathToolStripMenuItem.Name = "copyMonitoringDataFolderPathToolStripMenuItem";
            this.copyMonitoringDataFolderPathToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
            this.copyMonitoringDataFolderPathToolStripMenuItem.Text = "Copy Monitoring Data Folder Path to Clipboard...";
            this.copyMonitoringDataFolderPathToolStripMenuItem.Click += new System.EventHandler(this.copyMonitoringDataFolderPathToolStripMenuItem_Click);
            // 
            // copyModelInputOutputFolderToolStripMenuItem
            // 
            this.copyModelInputOutputFolderToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.Copy;
            this.copyModelInputOutputFolderToolStripMenuItem.Name = "copyModelInputOutputFolderToolStripMenuItem";
            this.copyModelInputOutputFolderToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
            this.copyModelInputOutputFolderToolStripMenuItem.Text = "Copy Model Input/Output Folder PAth to Clipboard...";
            this.copyModelInputOutputFolderToolStripMenuItem.Click += new System.EventHandler(this.copyModelInputOutputFolderToolStripMenuItem_Click);
            // 
            // toolStripSeparator10
            // 
            this.toolStripSeparator10.Name = "toolStripSeparator10";
            this.toolStripSeparator10.Size = new System.Drawing.Size(353, 6);
            // 
            // filterForAllVisitsToThisSiteToolStripMenuItem
            // 
            this.filterForAllVisitsToThisSiteToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.selection;
            this.filterForAllVisitsToThisSiteToolStripMenuItem.Name = "filterForAllVisitsToThisSiteToolStripMenuItem";
            this.filterForAllVisitsToThisSiteToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
            this.filterForAllVisitsToThisSiteToolStripMenuItem.Text = "Filter For All Visits to this Site";
            this.filterForAllVisitsToThisSiteToolStripMenuItem.Click += new System.EventHandler(this.filterForAllVisitsToThisSiteToolStripMenuItem_Click);
            // 
            // toolStripSeparator11
            // 
            this.toolStripSeparator11.Name = "toolStripSeparator11";
            this.toolStripSeparator11.Size = new System.Drawing.Size(353, 6);
            // 
            // generateChannelUnitCSVFileToolStripMenuItem
            // 
            this.generateChannelUnitCSVFileToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.ConcaveHull;
            this.generateChannelUnitCSVFileToolStripMenuItem.Name = "generateChannelUnitCSVFileToolStripMenuItem";
            this.generateChannelUnitCSVFileToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
            this.generateChannelUnitCSVFileToolStripMenuItem.Text = "Generate Channel Unit CSV File";
            this.generateChannelUnitCSVFileToolStripMenuItem.Click += new System.EventHandler(this.generateChannelUnitCSVFileToolStripMenuItem_Click);
            // 
            // generateRBTRunForThisVisitToolStripMenuItem
            // 
            this.generateRBTRunForThisVisitToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.rbt_16x16;
            this.generateRBTRunForThisVisitToolStripMenuItem.Name = "generateRBTRunForThisVisitToolStripMenuItem";
            this.generateRBTRunForThisVisitToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
            this.generateRBTRunForThisVisitToolStripMenuItem.Text = "Generate RBT Run For Selected Visit(s)...";
            this.generateRBTRunForThisVisitToolStripMenuItem.Click += new System.EventHandler(this.generateRBTRunForThisVisitToolStripMenuItem_Click);
            // 
            // toolStripSeparator12
            // 
            this.toolStripSeparator12.Name = "toolStripSeparator12";
            this.toolStripSeparator12.Size = new System.Drawing.Size(353, 6);
            // 
            // downloadTopoAndHydroDataFromCmorgToolStripMenuItem
            // 
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.download;
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem.Name = "downloadTopoAndHydroDataFromCmorgToolStripMenuItem";
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem.Text = "Download Topo and Hydro Data From cm.org";
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem.Click += new System.EventHandler(this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem_Click);
            // 
            // viewSiteLocationMapToolStripMenuItem
            // 
            this.viewSiteLocationMapToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.map;
            this.viewSiteLocationMapToolStripMenuItem.Name = "viewSiteLocationMapToolStripMenuItem";
            this.viewSiteLocationMapToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
            this.viewSiteLocationMapToolStripMenuItem.Text = "View site location map...";
            this.viewSiteLocationMapToolStripMenuItem.Click += new System.EventHandler(this.viewSiteLocationMapToolStripMenuItem_Click);
            // 
            // selectRandomNumberOfVisitsToolStripMenuItem
            // 
            this.selectRandomNumberOfVisitsToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.selection;
            this.selectRandomNumberOfVisitsToolStripMenuItem.Name = "selectRandomNumberOfVisitsToolStripMenuItem";
            this.selectRandomNumberOfVisitsToolStripMenuItem.Size = new System.Drawing.Size(356, 22);
            this.selectRandomNumberOfVisitsToolStripMenuItem.Text = "Select random number of visits...";
            this.selectRandomNumberOfVisitsToolStripMenuItem.Click += new System.EventHandler(this.selectRandomNumberOfVisitsToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MinimumSize = new System.Drawing.Size(300, 200);
            this.Name = "MainForm";
            this.Text = "CHaMP Workbench";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpSite.ResumeLayout(false);
            this.cmsSiteAllNone.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.cmsWatershed.ResumeLayout(false);
            this.grpFieldSeason.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.valVisitID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdVisits)).EndInit();
            this.cmsVisit.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem closeDatabaseToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rBTToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rBTToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem runRBTConsoleBatchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectBatchesToRunToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scavengeRBTResultsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem unpackMonitoringData7ZipArchiveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem buildInputFilesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutTheCHaMPWorkbenchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cHaMPWorkbenchWebSiteToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel tssDatabasePath;
        private System.Windows.Forms.ToolStripMenuItem scavengeVisitDataFromCHaMPExportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem experimentalToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem ericWallToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem jamesHensleighToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kellyWhiteheadToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem konradHaffenToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem philipBaileyToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saraBangenToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
        private System.Windows.Forms.ToolStripMenuItem prepareDatabaseForDeploymentToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutExperimentalToolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
        private System.Windows.Forms.ToolStripMenuItem hydroModelInputGeneratorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gCDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateGCDProjectFromCHaMPSiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkForUpdatesToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
        private System.Windows.Forms.ToolStripMenuItem openDatabaseInAccessToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
        private System.Windows.Forms.ToolStripMenuItem testXPathReferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem delft3DToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem cSVToRasterToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem queueBridgeCreekBatchesRBTRunsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem extractRBTErrorsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem habitatToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateBatchHabitatProjectToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView grdVisits;
        private System.Windows.Forms.NumericUpDown valVisitID;
        private System.Windows.Forms.CheckBox chkVisitID;
        private System.Windows.Forms.GroupBox grpFieldSeason;
        private System.Windows.Forms.CheckedListBox lstFieldSeason;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox lstWatershed;
        private System.Windows.Forms.ContextMenuStrip cmsVisit;
        private System.Windows.Forms.ToolStripMenuItem visitPropertiesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browseMonitoringDataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem browseModelInputOutputFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
        private System.Windows.Forms.ToolStripMenuItem copyMonitoringDataFolderPathToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyModelInputOutputFolderToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
        private System.Windows.Forms.ToolStripMenuItem filterForAllVisitsToThisSiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator11;
        private System.Windows.Forms.ToolStripMenuItem generateChannelUnitCSVFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateRBTRunForThisVisitToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator12;
        private System.Windows.Forms.ToolStripMenuItem downloadTopoAndHydroDataFromCmorgToolStripMenuItem;
        private System.Windows.Forms.GroupBox grpSite;
        private System.Windows.Forms.CheckedListBox lstSite;
        private System.Windows.Forms.ContextMenuStrip cmsSiteAllNone;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip cmsWatershed;
        private System.Windows.Forms.ToolStripMenuItem selectAllToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem selectNoneToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem viewSiteLocationMapToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem writeSimulationResultsToCSVFileToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWatershedID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWatershedName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFieldSeason;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSiteName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVisitID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrganization;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHitchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCrewName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVisitPhase;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVisitStatus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colAEM;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsPrimary;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colQCVisit;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colHasStreamTempLogger;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colHasFishData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSiteID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSampleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCahnnelUnits;
        private System.Windows.Forms.ToolStripMenuItem exportAWSLookupToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem filterVisitsFromVisitIDCSVFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem validationReportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectRandomNumberOfVisitsToolStripMenuItem;

    }
}

