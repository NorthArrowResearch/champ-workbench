namespace CHaMPWorkbench.Data
{
    partial class frmMetricGrid
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
            this.exportToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportMetricsToCSVFileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ucMetricGrid1 = new CHaMPWorkbench.Data.ucMetricGrid();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(383, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // exportToolStripMenuItem
            // 
            this.exportToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportMetricsToCSVFileToolStripMenuItem});
            this.exportToolStripMenuItem.MergeAction = System.Windows.Forms.MergeAction.Insert;
            this.exportToolStripMenuItem.MergeIndex = 2;
            this.exportToolStripMenuItem.Name = "exportToolStripMenuItem";
            this.exportToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.exportToolStripMenuItem.Text = "Export";
            // 
            // exportMetricsToCSVFileToolStripMenuItem
            // 
            this.exportMetricsToCSVFileToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.Save;
            this.exportMetricsToCSVFileToolStripMenuItem.Name = "exportMetricsToCSVFileToolStripMenuItem";
            this.exportMetricsToCSVFileToolStripMenuItem.Size = new System.Drawing.Size(217, 22);
            this.exportMetricsToCSVFileToolStripMenuItem.Text = "Export Metrics to CSV File...";
            this.exportMetricsToCSVFileToolStripMenuItem.Click += new System.EventHandler(this.exportMetricsToCSVFileToolStripMenuItem_Click);
            // 
            // ucMetricGrid1
            // 
            this.ucMetricGrid1.DBCon = null;
            this.ucMetricGrid1.Location = new System.Drawing.Point(23, 28);
            this.ucMetricGrid1.Name = "ucMetricGrid1";
            this.ucMetricGrid1.Size = new System.Drawing.Size(183, 106);
            this.ucMetricGrid1.TabIndex = 0;
            this.ucMetricGrid1.VisitIDs = null;
            // 
            // frmMetricGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 192);
            this.Controls.Add(this.ucMetricGrid1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMetricGrid";
            this.Text = "frmMetricGrid";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ucMetricGrid ucMetricGrid1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem exportToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportMetricsToCSVFileToolStripMenuItem;
    }
}