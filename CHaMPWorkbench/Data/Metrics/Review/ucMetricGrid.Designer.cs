namespace CHaMPWorkbench.Data
{
    partial class ucMetricGrid
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grdData = new System.Windows.Forms.DataGridView();
            this.cmdExport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.exportMetricDataToCSVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.cmdExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdData
            // 
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.ContextMenuStrip = this.cmdExport;
            this.grdData.Location = new System.Drawing.Point(82, 16);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(240, 150);
            this.grdData.TabIndex = 0;
            this.grdData.SelectionChanged += new System.EventHandler(this.grdData_SelectionChanged);
            // 
            // cmdExport
            // 
            this.cmdExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportMetricDataToCSVToolStripMenuItem});
            this.cmdExport.Name = "cmdExport";
            this.cmdExport.Size = new System.Drawing.Size(219, 26);
            // 
            // exportMetricDataToCSVToolStripMenuItem
            // 
            this.exportMetricDataToCSVToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.Save;
            this.exportMetricDataToCSVToolStripMenuItem.Name = "exportMetricDataToCSVToolStripMenuItem";
            this.exportMetricDataToCSVToolStripMenuItem.Size = new System.Drawing.Size(218, 22);
            this.exportMetricDataToCSVToolStripMenuItem.Text = "Export Metric Data to CSV...";
            this.exportMetricDataToCSVToolStripMenuItem.Click += new System.EventHandler(this.exportMetricDataToCSVToolStripMenuItem_Click);
            // 
            // ucMetricGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdData);
            this.Name = "ucMetricGrid";
            this.Size = new System.Drawing.Size(713, 169);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.cmdExport.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.ContextMenuStrip cmdExport;
        private System.Windows.Forms.ToolStripMenuItem exportMetricDataToCSVToolStripMenuItem;
    }
}
