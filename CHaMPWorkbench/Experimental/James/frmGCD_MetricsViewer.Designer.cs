namespace CHaMPWorkbench.Experimental.James
{
    partial class frmGCD_MetricsViewer
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            this.cmdGetData = new System.Windows.Forms.Button();
            this.msnChart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.lblSite = new System.Windows.Forms.Label();
            this.cmbSite = new System.Windows.Forms.ComboBox();
            this.lblWatershed = new System.Windows.Forms.Label();
            this.cmbWatershed = new System.Windows.Forms.ComboBox();
            this.grbUSGS_Gage = new System.Windows.Forms.GroupBox();
            this.dgvVisits = new System.Windows.Forms.DataGridView();
            this.NewVisitID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NewSampleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OldVisitID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.OldSampleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MaskValueName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmbInterval = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbOldYear = new System.Windows.Forms.ComboBox();
            this.cmbNewYear = new System.Windows.Forms.ComboBox();
            this.chkHighlightSite = new System.Windows.Forms.CheckBox();
            this.lblMask = new System.Windows.Forms.Label();
            this.cmbMask = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbYaxis = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cmbXaxis = new System.Windows.Forms.ComboBox();
            this.cmsFigureOptions = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.miSaveImage = new System.Windows.Forms.ToolStripMenuItem();
            this.resetZoomToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.cmdClose = new System.Windows.Forms.Button();
            this.cmsGCD_Visit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.gcdRunReviewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.msnChart)).BeginInit();
            this.grbUSGS_Gage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVisits)).BeginInit();
            this.cmsFigureOptions.SuspendLayout();
            this.cmsGCD_Visit.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdGetData
            // 
            this.cmdGetData.Location = new System.Drawing.Point(372, 150);
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
            chartArea2.Name = "ChartArea";
            chartArea2.Position.Auto = false;
            chartArea2.Position.Height = 94F;
            chartArea2.Position.Width = 85F;
            chartArea2.Position.X = 1F;
            chartArea2.Position.Y = 3F;
            this.msnChart.ChartAreas.Add(chartArea2);
            legend2.Name = "Legend";
            this.msnChart.Legends.Add(legend2);
            this.msnChart.Location = new System.Drawing.Point(15, 231);
            this.msnChart.Name = "msnChart";
            this.msnChart.Size = new System.Drawing.Size(974, 404);
            this.msnChart.TabIndex = 0;
            this.msnChart.Text = "chart1";
            this.msnChart.MouseClick += new System.Windows.Forms.MouseEventHandler(this.msnChart_MouseClick);
            // 
            // lblSite
            // 
            this.lblSite.AutoSize = true;
            this.lblSite.Location = new System.Drawing.Point(483, 19);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(65, 13);
            this.lblSite.TabIndex = 3;
            this.lblSite.Text = "CHaMP Site";
            // 
            // cmbSite
            // 
            this.cmbSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSite.FormattingEnabled = true;
            this.cmbSite.Location = new System.Drawing.Point(554, 15);
            this.cmbSite.Name = "cmbSite";
            this.cmbSite.Size = new System.Drawing.Size(414, 21);
            this.cmbSite.TabIndex = 2;
            this.cmbSite.SelectedIndexChanged += new System.EventHandler(this.cmbCHaMPSite_SelectedIndexChanged);
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
            this.cmbWatershed.Size = new System.Drawing.Size(344, 21);
            this.cmbWatershed.TabIndex = 0;
            this.cmbWatershed.SelectedIndexChanged += new System.EventHandler(this.WatershedComboChanged);
            // 
            // grbUSGS_Gage
            // 
            this.grbUSGS_Gage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbUSGS_Gage.Controls.Add(this.dgvVisits);
            this.grbUSGS_Gage.Controls.Add(this.cmbInterval);
            this.grbUSGS_Gage.Controls.Add(this.label5);
            this.grbUSGS_Gage.Controls.Add(this.label4);
            this.grbUSGS_Gage.Controls.Add(this.label3);
            this.grbUSGS_Gage.Controls.Add(this.cmbOldYear);
            this.grbUSGS_Gage.Controls.Add(this.cmbNewYear);
            this.grbUSGS_Gage.Controls.Add(this.chkHighlightSite);
            this.grbUSGS_Gage.Controls.Add(this.lblMask);
            this.grbUSGS_Gage.Controls.Add(this.cmbMask);
            this.grbUSGS_Gage.Controls.Add(this.label2);
            this.grbUSGS_Gage.Controls.Add(this.cmbYaxis);
            this.grbUSGS_Gage.Controls.Add(this.label1);
            this.grbUSGS_Gage.Controls.Add(this.cmbXaxis);
            this.grbUSGS_Gage.Controls.Add(this.lblSite);
            this.grbUSGS_Gage.Controls.Add(this.cmbSite);
            this.grbUSGS_Gage.Controls.Add(this.cmdGetData);
            this.grbUSGS_Gage.Controls.Add(this.lblWatershed);
            this.grbUSGS_Gage.Controls.Add(this.cmbWatershed);
            this.grbUSGS_Gage.Location = new System.Drawing.Point(15, 12);
            this.grbUSGS_Gage.Name = "grbUSGS_Gage";
            this.grbUSGS_Gage.Size = new System.Drawing.Size(974, 213);
            this.grbUSGS_Gage.TabIndex = 9;
            this.grbUSGS_Gage.TabStop = false;
            this.grbUSGS_Gage.Text = "USGS Stream Gage";
            // 
            // dgvVisits
            // 
            this.dgvVisits.AllowUserToAddRows = false;
            this.dgvVisits.AllowUserToDeleteRows = false;
            this.dgvVisits.AllowUserToResizeRows = false;
            this.dgvVisits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvVisits.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dgvVisits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvVisits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.NewVisitID,
            this.NewSampleDate,
            this.OldVisitID,
            this.OldSampleDate,
            this.MaskValueName});
            this.dgvVisits.Location = new System.Drawing.Point(486, 72);
            this.dgvVisits.MultiSelect = false;
            this.dgvVisits.Name = "dgvVisits";
            this.dgvVisits.ReadOnly = true;
            this.dgvVisits.RowHeadersVisible = false;
            this.dgvVisits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvVisits.Size = new System.Drawing.Size(482, 135);
            this.dgvVisits.TabIndex = 26;
            this.dgvVisits.TabStop = false;
            this.dgvVisits.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvVisits_CellMouseClick);
            // 
            // NewVisitID
            // 
            this.NewVisitID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.NewVisitID.FillWeight = 162.4366F;
            this.NewVisitID.HeaderText = "New Visit ID";
            this.NewVisitID.Name = "NewVisitID";
            this.NewVisitID.ReadOnly = true;
            this.NewVisitID.Width = 73;
            // 
            // NewSampleDate
            // 
            this.NewSampleDate.FillWeight = 79.18781F;
            this.NewSampleDate.HeaderText = "New Sample Date";
            this.NewSampleDate.Name = "NewSampleDate";
            this.NewSampleDate.ReadOnly = true;
            // 
            // OldVisitID
            // 
            this.OldVisitID.FillWeight = 79.18781F;
            this.OldVisitID.HeaderText = "Old Visit ID";
            this.OldVisitID.Name = "OldVisitID";
            this.OldVisitID.ReadOnly = true;
            // 
            // OldSampleDate
            // 
            this.OldSampleDate.FillWeight = 79.18781F;
            this.OldSampleDate.HeaderText = "Old Sample Date";
            this.OldSampleDate.Name = "OldSampleDate";
            this.OldSampleDate.ReadOnly = true;
            // 
            // MaskValueName
            // 
            this.MaskValueName.HeaderText = "Mask";
            this.MaskValueName.Name = "MaskValueName";
            this.MaskValueName.ReadOnly = true;
            // 
            // cmbInterval
            // 
            this.cmbInterval.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInterval.FormattingEnabled = true;
            this.cmbInterval.Location = new System.Drawing.Point(421, 115);
            this.cmbInterval.Name = "cmbInterval";
            this.cmbInterval.Size = new System.Drawing.Size(43, 21);
            this.cmbInterval.TabIndex = 25;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(329, 118);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(86, 13);
            this.label5.TabIndex = 24;
            this.label5.Text = "Spanning year(s)";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(192, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 23;
            this.label4.Text = "Old Year";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(61, 118);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 13);
            this.label3.TabIndex = 22;
            this.label3.Text = "New Year";
            // 
            // cmbOldYear
            // 
            this.cmbOldYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOldYear.FormattingEnabled = true;
            this.cmbOldYear.Location = new System.Drawing.Point(246, 115);
            this.cmbOldYear.Name = "cmbOldYear";
            this.cmbOldYear.Size = new System.Drawing.Size(58, 21);
            this.cmbOldYear.TabIndex = 21;
            // 
            // cmbNewYear
            // 
            this.cmbNewYear.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbNewYear.FormattingEnabled = true;
            this.cmbNewYear.Location = new System.Drawing.Point(120, 115);
            this.cmbNewYear.Name = "cmbNewYear";
            this.cmbNewYear.Size = new System.Drawing.Size(58, 21);
            this.cmbNewYear.TabIndex = 20;
            // 
            // chkHighlightSite
            // 
            this.chkHighlightSite.AutoSize = true;
            this.chkHighlightSite.Location = new System.Drawing.Point(486, 49);
            this.chkHighlightSite.Name = "chkHighlightSite";
            this.chkHighlightSite.Size = new System.Drawing.Size(88, 17);
            this.chkHighlightSite.TabIndex = 19;
            this.chkHighlightSite.Text = "Highlight Site";
            this.chkHighlightSite.UseVisualStyleBackColor = true;
            // 
            // lblMask
            // 
            this.lblMask.AutoSize = true;
            this.lblMask.Location = new System.Drawing.Point(81, 155);
            this.lblMask.Name = "lblMask";
            this.lblMask.Size = new System.Drawing.Size(33, 13);
            this.lblMask.TabIndex = 18;
            this.lblMask.Text = "Mask";
            // 
            // cmbMask
            // 
            this.cmbMask.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMask.FormattingEnabled = true;
            this.cmbMask.Location = new System.Drawing.Point(120, 152);
            this.cmbMask.Name = "cmbMask";
            this.cmbMask.Size = new System.Drawing.Size(123, 21);
            this.cmbMask.TabIndex = 17;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 86);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Y Axis";
            // 
            // cmbYaxis
            // 
            this.cmbYaxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbYaxis.FormattingEnabled = true;
            this.cmbYaxis.Location = new System.Drawing.Point(120, 78);
            this.cmbYaxis.Name = "cmbYaxis";
            this.cmbYaxis.Size = new System.Drawing.Size(344, 21);
            this.cmbYaxis.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(74, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(36, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "X Axis";
            // 
            // cmbXaxis
            // 
            this.cmbXaxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbXaxis.FormattingEnabled = true;
            this.cmbXaxis.Location = new System.Drawing.Point(120, 47);
            this.cmbXaxis.Name = "cmbXaxis";
            this.cmbXaxis.Size = new System.Drawing.Size(344, 21);
            this.cmbXaxis.TabIndex = 13;
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
            this.cmdClose.Location = new System.Drawing.Point(914, 641);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 10;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            // 
            // cmsGCD_Visit
            // 
            this.cmsGCD_Visit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gcdRunReviewToolStripMenuItem,
            this.createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem});
            this.cmsGCD_Visit.Name = "cmsGCD";
            this.cmsGCD_Visit.Size = new System.Drawing.Size(404, 48);
            // 
            // gcdRunReviewToolStripMenuItem
            // 
            this.gcdRunReviewToolStripMenuItem.Name = "gcdRunReviewToolStripMenuItem";
            this.gcdRunReviewToolStripMenuItem.Size = new System.Drawing.Size(403, 22);
            this.gcdRunReviewToolStripMenuItem.Text = "Create Record of GCD Run in Post GCD QA/QC Form";
            this.gcdRunReviewToolStripMenuItem.Click += new System.EventHandler(this.gcdRunReviewToolStripMenuItem_Click);
            // 
            // createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem
            // 
            this.createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem.Enabled = false;
            this.createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem.Name = "createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem";
            this.createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem.Size = new System.Drawing.Size(403, 22);
            this.createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem.Text = "Create Record of GCD Run and View in Post GCD QA/QC Form";
            this.createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem.Click += new System.EventHandler(this.createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem_Click);
            // 
            // frmGCD_MetricsViewer
            // 
            this.AcceptButton = this.cmdClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(1004, 676);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.msnChart);
            this.Controls.Add(this.grbUSGS_Gage);
            this.MinimumSize = new System.Drawing.Size(792, 577);
            this.Name = "frmGCD_MetricsViewer";
            this.ShowIcon = false;
            this.Text = "GCD Metrics Viewer";
            this.Load += new System.EventHandler(this.frmGCD_AnalysisWatershed_Load);
            ((System.ComponentModel.ISupportInitialize)(this.msnChart)).EndInit();
            this.grbUSGS_Gage.ResumeLayout(false);
            this.grbUSGS_Gage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvVisits)).EndInit();
            this.cmsFigureOptions.ResumeLayout(false);
            this.cmsGCD_Visit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdGetData;
        private System.Windows.Forms.Label lblSite;
        private System.Windows.Forms.ComboBox cmbSite;
        private System.Windows.Forms.Label lblWatershed;
        private System.Windows.Forms.ComboBox cmbWatershed;
        private System.Windows.Forms.GroupBox grbUSGS_Gage;
        private System.Windows.Forms.DataVisualization.Charting.Chart msnChart;
        private System.Windows.Forms.ContextMenuStrip cmsFigureOptions;
        private System.Windows.Forms.ToolStripMenuItem miSaveImage;
        private System.Windows.Forms.ToolStripMenuItem resetZoomToolStripMenuItem;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbYaxis;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cmbXaxis;
        private System.Windows.Forms.Label lblMask;
        private System.Windows.Forms.ComboBox cmbMask;
        private System.Windows.Forms.CheckBox chkHighlightSite;
        private System.Windows.Forms.ComboBox cmbInterval;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbOldYear;
        private System.Windows.Forms.ComboBox cmbNewYear;
        private System.Windows.Forms.DataGridView dgvVisits;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewVisitID;
        private System.Windows.Forms.DataGridViewTextBoxColumn NewSampleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn OldVisitID;
        private System.Windows.Forms.DataGridViewTextBoxColumn OldSampleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn MaskValueName;
        private System.Windows.Forms.ContextMenuStrip cmsGCD_Visit;
        private System.Windows.Forms.ToolStripMenuItem gcdRunReviewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createRecordOfGCDRunAndViewInPostGCDQAQCFormToolStripMenuItem;
    }
}