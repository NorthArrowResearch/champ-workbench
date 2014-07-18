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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.closeDatabaseToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rBTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scavengeVisitInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createInputFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.individualFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.batchToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.rBTToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.runRBTConsoleBatchesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectBatchesToRunToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.scavengeRBTResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.rBTToolStripMenuItem});
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
            this.openDatabaseToolStripMenuItem.Name = "openDatabaseToolStripMenuItem";
            this.openDatabaseToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.openDatabaseToolStripMenuItem.Text = "Open Database";
            this.openDatabaseToolStripMenuItem.Click += new System.EventHandler(this.openDatabaseToolStripMenuItem_Click);
            // 
            // closeDatabaseToolStripMenuItem
            // 
            this.closeDatabaseToolStripMenuItem.Name = "closeDatabaseToolStripMenuItem";
            this.closeDatabaseToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.closeDatabaseToolStripMenuItem.Text = "Close Database";
            this.closeDatabaseToolStripMenuItem.Click += new System.EventHandler(this.closeDatabaseToolStripMenuItem_Click);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            // 
            // rBTToolStripMenuItem
            // 
            this.rBTToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.scavengeVisitInfoToolStripMenuItem,
            this.createInputFileToolStripMenuItem,
            this.rBTToolStripMenuItem1});
            this.rBTToolStripMenuItem.Name = "rBTToolStripMenuItem";
            this.rBTToolStripMenuItem.Size = new System.Drawing.Size(48, 20);
            this.rBTToolStripMenuItem.Text = "Tools";
            // 
            // scavengeVisitInfoToolStripMenuItem
            // 
            this.scavengeVisitInfoToolStripMenuItem.Name = "scavengeVisitInfoToolStripMenuItem";
            this.scavengeVisitInfoToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.scavengeVisitInfoToolStripMenuItem.Text = "Scavenge Visit Topo Data Information";
            this.scavengeVisitInfoToolStripMenuItem.Click += new System.EventHandler(this.scavengeVisitInfoToolStripMenuItem_Click);
            // 
            // createInputFileToolStripMenuItem
            // 
            this.createInputFileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.individualFileToolStripMenuItem,
            this.batchToolStripMenuItem});
            this.createInputFileToolStripMenuItem.Name = "createInputFileToolStripMenuItem";
            this.createInputFileToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.createInputFileToolStripMenuItem.Text = "Create RBT Input XML File";
            // 
            // individualFileToolStripMenuItem
            // 
            this.individualFileToolStripMenuItem.Name = "individualFileToolStripMenuItem";
            this.individualFileToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.individualFileToolStripMenuItem.Text = "Single";
            this.individualFileToolStripMenuItem.Click += new System.EventHandler(this.individualFileToolStripMenuItem_Click);
            // 
            // batchToolStripMenuItem
            // 
            this.batchToolStripMenuItem.Name = "batchToolStripMenuItem";
            this.batchToolStripMenuItem.Size = new System.Drawing.Size(106, 22);
            this.batchToolStripMenuItem.Text = "Batch";
            this.batchToolStripMenuItem.Click += new System.EventHandler(this.batchToolStripMenuItem_Click);
            // 
            // rBTToolStripMenuItem1
            // 
            this.rBTToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectBatchesToRunToolStripMenuItem,
            this.runRBTConsoleBatchesToolStripMenuItem,
            this.scavengeRBTResultsToolStripMenuItem});
            this.rBTToolStripMenuItem1.Name = "rBTToolStripMenuItem1";
            this.rBTToolStripMenuItem1.Size = new System.Drawing.Size(273, 22);
            this.rBTToolStripMenuItem1.Text = "RBT";
            // 
            // runRBTConsoleBatchesToolStripMenuItem
            // 
            this.runRBTConsoleBatchesToolStripMenuItem.Name = "runRBTConsoleBatchesToolStripMenuItem";
            this.runRBTConsoleBatchesToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.runRBTConsoleBatchesToolStripMenuItem.Text = "Run RBT Console Batches";
            this.runRBTConsoleBatchesToolStripMenuItem.Click += new System.EventHandler(this.runRBTConsoleBatchesToolStripMenuItem_Click);
            // 
            // selectBatchesToRunToolStripMenuItem
            // 
            this.selectBatchesToRunToolStripMenuItem.Name = "selectBatchesToRunToolStripMenuItem";
            this.selectBatchesToRunToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.selectBatchesToRunToolStripMenuItem.Text = "Select Batches to Run";
            this.selectBatchesToRunToolStripMenuItem.Click += new System.EventHandler(this.selectBatchesToRunToolStripMenuItem_Click);
            // 
            // scavengeRBTResultsToolStripMenuItem
            // 
            this.scavengeRBTResultsToolStripMenuItem.Name = "scavengeRBTResultsToolStripMenuItem";
            this.scavengeRBTResultsToolStripMenuItem.Size = new System.Drawing.Size(209, 22);
            this.scavengeRBTResultsToolStripMenuItem.Text = "Scavenge RBT Results";
            this.scavengeRBTResultsToolStripMenuItem.Click += new System.EventHandler(this.scavengeRBTResultsToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(572, 329);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "CHaMP Workbench";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem createInputFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem individualFileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem batchToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scavengeVisitInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem rBTToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem runRBTConsoleBatchesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectBatchesToRunToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem scavengeRBTResultsToolStripMenuItem;
    }
}

