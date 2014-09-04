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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scavengeVisitTopoDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.prepareDatabaseForDeploymentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rBTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rBTToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.buildInputFilesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.singleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.selectBatchesToRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runRBTConsoleBatchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.scavengeRBTResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gCDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.generateGCDProjectFromCHaMPSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.experimentalToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutExperimentalToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ericWallToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.jamesHensleighToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kellyWhiteheadToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.konradHaffenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.philipBaileyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saraBangenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cHaMPWorkbenchWebSiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutTheCHaMPWorkbenchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.tssDatabasePath = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1.SuspendLayout();
            this.statusStrip1.SuspendLayout();
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
            this.menuStrip1.Size = new System.Drawing.Size(572, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openDatabaseToolStripMenuItem,
            this.closeDatabaseToolStripMenuItem,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openDatabaseToolStripMenuItem
            // 
            this.openDatabaseToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.database;
            this.openDatabaseToolStripMenuItem.Name = "openDatabaseToolStripMenuItem";
            this.openDatabaseToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.openDatabaseToolStripMenuItem.Text = "Open Database...";
            this.openDatabaseToolStripMenuItem.Click += new System.EventHandler(this.openDatabaseToolStripMenuItem_Click);
            // 
            // closeDatabaseToolStripMenuItem
            // 
            this.closeDatabaseToolStripMenuItem.Name = "closeDatabaseToolStripMenuItem";
            this.closeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.closeDatabaseToolStripMenuItem.Text = "Close Database";
            this.closeDatabaseToolStripMenuItem.Click += new System.EventHandler(this.closeDatabaseToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(163, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem,
            this.scavengeVisitTopoDataToolStripMenuItem,
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
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem.Text = "Unpack Monitoring data 7Zip Archive...";
            this.unpackMonitoringData7ZipArchiveToolStripMenuItem.Click += new System.EventHandler(this.unpackMonitoringData7ZipArchiveToolStripMenuItem_Click);
            // 
            // scavengeVisitTopoDataToolStripMenuItem
            // 
            this.scavengeVisitTopoDataToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.import;
            this.scavengeVisitTopoDataToolStripMenuItem.Name = "scavengeVisitTopoDataToolStripMenuItem";
            this.scavengeVisitTopoDataToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.scavengeVisitTopoDataToolStripMenuItem.Text = "Scavenge Visit Topo Data...";
            this.scavengeVisitTopoDataToolStripMenuItem.Click += new System.EventHandler(this.scavengeVisitTopoDataToolStripMenuItem_Click);
            // 
            // scavengeVisitDataFromCHaMPExportToolStripMenuItem
            // 
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem.Name = "scavengeVisitDataFromCHaMPExportToolStripMenuItem";
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem.Text = "Scavenge Visit Data From CHaMP Export";
            this.scavengeVisitDataFromCHaMPExportToolStripMenuItem.Click += new System.EventHandler(this.scavengeVisitDataFromCHaMPExportToolStripMenuItem_Click);
            // 
            // toolStripSeparator4
            // 
            this.toolStripSeparator4.Name = "toolStripSeparator4";
            this.toolStripSeparator4.Size = new System.Drawing.Size(284, 6);
            // 
            // prepareDatabaseForDeploymentToolStripMenuItem
            // 
            this.prepareDatabaseForDeploymentToolStripMenuItem.Name = "prepareDatabaseForDeploymentToolStripMenuItem";
            this.prepareDatabaseForDeploymentToolStripMenuItem.Size = new System.Drawing.Size(287, 22);
            this.prepareDatabaseForDeploymentToolStripMenuItem.Text = "Prepare Database for Deployment...";
            this.prepareDatabaseForDeploymentToolStripMenuItem.Click += new System.EventHandler(this.prepareDatabaseForDeploymentToolStripMenuItem_Click);
            // 
            // rBTToolStripMenuItem
            // 
            this.rBTToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.rBTToolStripMenuItem1,
            this.gCDToolStripMenuItem,
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
            this.scavengeRBTResultsToolStripMenuItem});
            this.rBTToolStripMenuItem1.Image = global::CHaMPWorkbench.Properties.Resources.RBT_Dark_32x32;
            this.rBTToolStripMenuItem1.Name = "rBTToolStripMenuItem1";
            this.rBTToolStripMenuItem1.Size = new System.Drawing.Size(152, 22);
            this.rBTToolStripMenuItem1.Text = "RBT";
            // 
            // buildInputFilesToolStripMenuItem
            // 
            this.buildInputFilesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.singleToolStripMenuItem,
            this.batchToolStripMenuItem1});
            this.buildInputFilesToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.xml;
            this.buildInputFilesToolStripMenuItem.Name = "buildInputFilesToolStripMenuItem";
            this.buildInputFilesToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.buildInputFilesToolStripMenuItem.Text = "Build Input File(s)";
            // 
            // singleToolStripMenuItem
            // 
            this.singleToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.xml;
            this.singleToolStripMenuItem.Name = "singleToolStripMenuItem";
            this.singleToolStripMenuItem.Size = new System.Drawing.Size(115, 22);
            this.singleToolStripMenuItem.Text = "Single...";
            this.singleToolStripMenuItem.Click += new System.EventHandler(this.singleToolStripMenuItem_Click);
            // 
            // batchToolStripMenuItem1
            // 
            this.batchToolStripMenuItem1.Image = global::CHaMPWorkbench.Properties.Resources.xml_batch;
            this.batchToolStripMenuItem1.Name = "batchToolStripMenuItem1";
            this.batchToolStripMenuItem1.Size = new System.Drawing.Size(115, 22);
            this.batchToolStripMenuItem1.Text = "Batch...";
            this.batchToolStripMenuItem1.Click += new System.EventHandler(this.batchToolStripMenuItem1_Click);
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
            // gCDToolStripMenuItem
            // 
            this.gCDToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateGCDProjectFromCHaMPSiteToolStripMenuItem});
            this.gCDToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.gcd_icon;
            this.gCDToolStripMenuItem.Name = "gCDToolStripMenuItem";
            this.gCDToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.gCDToolStripMenuItem.Text = "GCD";
            // 
            // generateGCDProjectFromCHaMPSiteToolStripMenuItem
            // 
            this.generateGCDProjectFromCHaMPSiteToolStripMenuItem.Name = "generateGCDProjectFromCHaMPSiteToolStripMenuItem";
            this.generateGCDProjectFromCHaMPSiteToolStripMenuItem.Size = new System.Drawing.Size(272, 22);
            this.generateGCDProjectFromCHaMPSiteToolStripMenuItem.Text = "Generate GCD Project for CHaMP Site";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(149, 6);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.Settings;
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
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
            this.kellyWhiteheadToolStripMenuItem.Name = "kellyWhiteheadToolStripMenuItem";
            this.kellyWhiteheadToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.kellyWhiteheadToolStripMenuItem.Text = "Kelly Whitehead";
            // 
            // konradHaffenToolStripMenuItem
            // 
            this.konradHaffenToolStripMenuItem.Name = "konradHaffenToolStripMenuItem";
            this.konradHaffenToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.konradHaffenToolStripMenuItem.Text = "Konrad Hafen";
            // 
            // philipBaileyToolStripMenuItem
            // 
            this.philipBaileyToolStripMenuItem.Name = "philipBaileyToolStripMenuItem";
            this.philipBaileyToolStripMenuItem.Size = new System.Drawing.Size(219, 22);
            this.philipBaileyToolStripMenuItem.Text = "Philip Bailey";
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
            this.statusStrip1.Location = new System.Drawing.Point(0, 307);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(572, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // tssDatabasePath
            // 
            this.tssDatabasePath.Name = "tssDatabasePath";
            this.tssDatabasePath.Size = new System.Drawing.Size(118, 17);
            this.tssDatabasePath.Text = "toolStripStatusLabel1";
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 329);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "CHaMP Workbench";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem singleToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem batchToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripMenuItem scavengeVisitTopoDataToolStripMenuItem;
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
        private System.Windows.Forms.ToolStripMenuItem gCDToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem generateGCDProjectFromCHaMPSiteToolStripMenuItem;
    }
}

