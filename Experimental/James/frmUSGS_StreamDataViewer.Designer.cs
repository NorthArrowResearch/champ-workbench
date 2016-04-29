namespace CHaMPWorkbench.Experimental.James
{
    partial class frmUSGS_StreamDataViewer
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.cmdGetData = new System.Windows.Forms.Button();
            this.msnChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblSite = new System.Windows.Forms.Label();
            this.cmbCHaMPSite = new System.Windows.Forms.ComboBox();
            this.lblWatershed = new System.Windows.Forms.Label();
            this.cmbWatershed = new System.Windows.Forms.ComboBox();
            this.grbUSGS_Gage = new System.Windows.Forms.GroupBox();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.lblWarningNoUSGS_Gage = new System.Windows.Forms.Label();
            this.txtUSGS_SiteNumber = new System.Windows.Forms.TextBox();
            this.lblManualUSGS_SiteNumber = new System.Windows.Forms.Label();
            this.cmbUSGS_Gage = new System.Windows.Forms.ComboBox();
            this.cmsFigureOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.resetZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdClose = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.msnChart)).BeginInit();
            this.grbUSGS_Gage.SuspendLayout();
            this.cmsFigureOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdGetData
            // 
            this.cmdGetData.Location = new System.Drawing.Point(275, 126);
            this.cmdGetData.Name = "cmdGetData";
            this.cmdGetData.Size = new System.Drawing.Size(92, 23);
            this.cmdGetData.TabIndex = 1;
            this.cmdGetData.Text = "Populate Figure";
            this.cmdGetData.UseVisualStyleBackColor = true;
            this.cmdGetData.Click += new System.EventHandler(this.cmdGetData_Click);
            // 
            // msnChart
            // 
            this.msnChart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.msnChart.BorderlineColor = System.Drawing.Color.Black;
            chartArea1.Name = "ChartArea";
            chartArea1.Position.Auto = false;
            chartArea1.Position.Height = 94F;
            chartArea1.Position.Width = 85F;
            chartArea1.Position.X = 1F;
            chartArea1.Position.Y = 3F;
            this.msnChart.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend";
            this.msnChart.Legends.Add(legend1);
            this.msnChart.Location = new System.Drawing.Point(15, 176);
            this.msnChart.Name = "msnChart";
            this.msnChart.Size = new System.Drawing.Size(1177, 322);
            this.msnChart.TabIndex = 0;
            this.msnChart.Text = "chart1";
            this.msnChart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.msnChart_MouseClick);
            // 
            // lblSite
            // 
            this.lblSite.AutoSize = true;
            this.lblSite.Location = new System.Drawing.Point(45, 50);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(65, 13);
            this.lblSite.TabIndex = 3;
            this.lblSite.Text = "CHaMP Site";
            // 
            // cmbCHaMPSite
            // 
            this.cmbCHaMPSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCHaMPSite.FormattingEnabled = true;
            this.cmbCHaMPSite.Location = new System.Drawing.Point(120, 46);
            this.cmbCHaMPSite.Name = "cmbCHaMPSite";
            this.cmbCHaMPSite.Size = new System.Drawing.Size(247, 21);
            this.cmbCHaMPSite.TabIndex = 2;
            this.cmbCHaMPSite.SelectedIndexChanged += new System.EventHandler(this.cmbCHaMPSite_SelectedIndexChanged);
            // 
            // lblWatershed
            // 
            this.lblWatershed.AutoSize = true;
            this.lblWatershed.Location = new System.Drawing.Point(11, 23);
            this.lblWatershed.Name = "lblWatershed";
            this.lblWatershed.Size = new System.Drawing.Size(99, 13);
            this.lblWatershed.TabIndex = 1;
            this.lblWatershed.Text = "CHaMP Watershed";
            // 
            // cmbWatershed
            // 
            this.cmbWatershed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWatershed.FormattingEnabled = true;
            this.cmbWatershed.Location = new System.Drawing.Point(120, 19);
            this.cmbWatershed.Name = "cmbWatershed";
            this.cmbWatershed.Size = new System.Drawing.Size(247, 21);
            this.cmbWatershed.TabIndex = 0;
            this.cmbWatershed.SelectedIndexChanged += new System.EventHandler(this.WatershedComboChanged);
            // 
            // grbUSGS_Gage
            // 
            this.grbUSGS_Gage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbUSGS_Gage.Controls.Add(this.linkLabel1);
            this.grbUSGS_Gage.Controls.Add(this.lblSite);
            this.grbUSGS_Gage.Controls.Add(this.lblWarningNoUSGS_Gage);
            this.grbUSGS_Gage.Controls.Add(this.cmbCHaMPSite);
            this.grbUSGS_Gage.Controls.Add(this.cmdGetData);
            this.grbUSGS_Gage.Controls.Add(this.txtUSGS_SiteNumber);
            this.grbUSGS_Gage.Controls.Add(this.lblWatershed);
            this.grbUSGS_Gage.Controls.Add(this.lblManualUSGS_SiteNumber);
            this.grbUSGS_Gage.Controls.Add(this.cmbWatershed);
            this.grbUSGS_Gage.Controls.Add(this.cmbUSGS_Gage);
            this.grbUSGS_Gage.Location = new System.Drawing.Point(15, 12);
            this.grbUSGS_Gage.Name = "grbUSGS_Gage";
            this.grbUSGS_Gage.Size = new System.Drawing.Size(1177, 158);
            this.grbUSGS_Gage.TabIndex = 9;
            this.grbUSGS_Gage.TabStop = false;
            this.grbUSGS_Gage.Text = "USGS Stream Gage";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(19, 77);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(91, 13);
            this.linkLabel1.TabIndex = 13;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "USGS gage code";
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.linkLabel1_LinkClicked);
            // 
            // lblWarningNoUSGS_Gage
            // 
            this.lblWarningNoUSGS_Gage.AutoSize = true;
            this.lblWarningNoUSGS_Gage.Location = new System.Drawing.Point(388, 131);
            this.lblWarningNoUSGS_Gage.Name = "lblWarningNoUSGS_Gage";
            this.lblWarningNoUSGS_Gage.Size = new System.Drawing.Size(52, 13);
            this.lblWarningNoUSGS_Gage.TabIndex = 12;
            this.lblWarningNoUSGS_Gage.Text = "lblManual";
            // 
            // txtUSGS_SiteNumber
            // 
            this.txtUSGS_SiteNumber.Location = new System.Drawing.Point(120, 100);
            this.txtUSGS_SiteNumber.Name = "txtUSGS_SiteNumber";
            this.txtUSGS_SiteNumber.Size = new System.Drawing.Size(247, 20);
            this.txtUSGS_SiteNumber.TabIndex = 11;
            // 
            // lblManualUSGS_SiteNumber
            // 
            this.lblManualUSGS_SiteNumber.AutoSize = true;
            this.lblManualUSGS_SiteNumber.Location = new System.Drawing.Point(14, 104);
            this.lblManualUSGS_SiteNumber.Name = "lblManualUSGS_SiteNumber";
            this.lblManualUSGS_SiteNumber.Size = new System.Drawing.Size(96, 13);
            this.lblManualUSGS_SiteNumber.TabIndex = 10;
            this.lblManualUSGS_SiteNumber.Text = "Manual gage code";
            // 
            // cmbUSGS_Gage
            // 
            this.cmbUSGS_Gage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUSGS_Gage.FormattingEnabled = true;
            this.cmbUSGS_Gage.Location = new System.Drawing.Point(120, 73);
            this.cmbUSGS_Gage.Name = "cmbUSGS_Gage";
            this.cmbUSGS_Gage.Size = new System.Drawing.Size(247, 21);
            this.cmbUSGS_Gage.TabIndex = 8;
            this.cmbUSGS_Gage.SelectedIndexChanged += new System.EventHandler(this.cmbUSGS_Gage_SelectedIndexChanged);
            // 
            // cmsFigureOptions
            // 
            this.cmsFigureOptions.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miSaveImage,
            this.resetZoomToolStripMenuItem});
            this.cmsFigureOptions.Name = "contextMenuStrip1";
            this.cmsFigureOptions.Size = new System.Drawing.Size(138, 48);
            // 
            // miSaveImage
            // 
            this.miSaveImage.Name = "miSaveImage";
            this.miSaveImage.Size = new System.Drawing.Size(137, 22);
            this.miSaveImage.Text = "Save Image";
            this.miSaveImage.Click += new System.EventHandler(this.miSaveImage_Click);
            // 
            // resetZoomToolStripMenuItem
            // 
            this.resetZoomToolStripMenuItem.Name = "resetZoomToolStripMenuItem";
            this.resetZoomToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.resetZoomToolStripMenuItem.Text = "Reset Zoom";
            this.resetZoomToolStripMenuItem.Click += new System.EventHandler(this.resetZoomToolStripMenuItem_Click);
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdClose.Location = new System.Drawing.Point(1117, 504);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 10;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            // 
            // frmUSGS_StreamDataViewer
            // 
            this.AcceptButton = this.cmdClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(1207, 539);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.msnChart);
            this.Controls.Add(this.grbUSGS_Gage);
            this.MinimumSize = new System.Drawing.Size(792, 577);
            this.Name = "frmUSGS_StreamDataViewer";
            this.ShowIcon = false;
            this.Text = "USGS Stream Gage Data Viewer";
            this.Load += new System.EventHandler(this.frmUSGS_StreamDataViewer_Load);
            ((System.ComponentModel.ISupportInitialize)(this.msnChart)).EndInit();
            this.grbUSGS_Gage.ResumeLayout(false);
            this.grbUSGS_Gage.PerformLayout();
            this.cmsFigureOptions.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdGetData;
        private System.Windows.Forms.Label lblSite;
        private System.Windows.Forms.ComboBox cmbCHaMPSite;
        private System.Windows.Forms.Label lblWatershed;
        private System.Windows.Forms.ComboBox cmbWatershed;
        private System.Windows.Forms.GroupBox grbUSGS_Gage;
        private System.Windows.Forms.ComboBox cmbUSGS_Gage;
        private System.Windows.Forms.TextBox txtUSGS_SiteNumber;
        private System.Windows.Forms.Label lblManualUSGS_SiteNumber;
        private System.Windows.Forms.Label lblWarningNoUSGS_Gage;
        private System.Windows.Forms.DataVisualization.Charting.Chart msnChart;
        private System.Windows.Forms.ContextMenuStrip cmsFigureOptions;
        private System.Windows.Forms.ToolStripMenuItem miSaveImage;
        private System.Windows.Forms.ToolStripMenuItem resetZoomToolStripMenuItem;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Button cmdClose;
    }
}