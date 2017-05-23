namespace CHaMPWorkbench.Visits
{
    partial class frmVisitDataGrid
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.txtOrganization = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.lstPrograms = new System.Windows.Forms.CheckedListBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rdoAll = new System.Windows.Forms.RadioButton();
            this.rdoPrimary = new System.Windows.Forms.RadioButton();
            this.txtStreamName = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSiteName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstWatershed = new System.Windows.Forms.CheckedListBox();
            this.grpFieldSeason = new System.Windows.Forms.GroupBox();
            this.lstFieldSeason = new System.Windows.Forms.CheckedListBox();
            this.valVisitID = new System.Windows.Forms.NumericUpDown();
            this.chkVisitID = new System.Windows.Forms.CheckBox();
            this.grdVisits = new System.Windows.Forms.DataGridView();
            this.colWatershedID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProgramID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colWatershedName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFieldSeason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSiteName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVisitID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStreamName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colOrganization = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colHitchName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colProgram = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCrewName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVisitPhase = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVisitStatus = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsPrimary = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colQCVisit = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHasStreamTempLogger = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colHasFishData = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colCategoryName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSiteID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSampleDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPanel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCahnnelUnits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.grpFieldSeason.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valVisitID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdVisits)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.txtOrganization);
            this.splitContainer1.Panel1.Controls.Add(this.label3);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox3);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox2);
            this.splitContainer1.Panel1.Controls.Add(this.txtStreamName);
            this.splitContainer1.Panel1.Controls.Add(this.label2);
            this.splitContainer1.Panel1.Controls.Add(this.txtSiteName);
            this.splitContainer1.Panel1.Controls.Add(this.label1);
            this.splitContainer1.Panel1.Controls.Add(this.groupBox1);
            this.splitContainer1.Panel1.Controls.Add(this.grpFieldSeason);
            this.splitContainer1.Panel1.Controls.Add(this.valVisitID);
            this.splitContainer1.Panel1.Controls.Add(this.chkVisitID);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdVisits);
            this.splitContainer1.Size = new System.Drawing.Size(845, 608);
            this.splitContainer1.SplitterDistance = 193;
            this.splitContainer1.TabIndex = 0;
            // 
            // txtOrganization
            // 
            this.txtOrganization.Location = new System.Drawing.Point(15, 522);
            this.txtOrganization.Name = "txtOrganization";
            this.txtOrganization.Size = new System.Drawing.Size(161, 20);
            this.txtOrganization.TabIndex = 22;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 504);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(66, 13);
            this.label3.TabIndex = 21;
            this.label3.Text = "Organization";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.lstPrograms);
            this.groupBox3.Location = new System.Drawing.Point(16, 327);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(161, 76);
            this.groupBox3.TabIndex = 16;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Programs";
            // 
            // lstPrograms
            // 
            this.lstPrograms.CheckOnClick = true;
            this.lstPrograms.FormattingEnabled = true;
            this.lstPrograms.Location = new System.Drawing.Point(7, 20);
            this.lstPrograms.Name = "lstPrograms";
            this.lstPrograms.Size = new System.Drawing.Size(148, 49);
            this.lstPrograms.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rdoAll);
            this.groupBox2.Controls.Add(this.rdoPrimary);
            this.groupBox2.Location = new System.Drawing.Point(15, 548);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(162, 48);
            this.groupBox2.TabIndex = 23;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Primary";
            // 
            // rdoAll
            // 
            this.rdoAll.AutoSize = true;
            this.rdoAll.Checked = true;
            this.rdoAll.Location = new System.Drawing.Point(15, 19);
            this.rdoAll.Name = "rdoAll";
            this.rdoAll.Size = new System.Drawing.Size(36, 17);
            this.rdoAll.TabIndex = 0;
            this.rdoAll.TabStop = true;
            this.rdoAll.Text = "All";
            this.rdoAll.UseVisualStyleBackColor = true;
            // 
            // rdoPrimary
            // 
            this.rdoPrimary.AutoSize = true;
            this.rdoPrimary.Location = new System.Drawing.Point(72, 19);
            this.rdoPrimary.Name = "rdoPrimary";
            this.rdoPrimary.Size = new System.Drawing.Size(81, 17);
            this.rdoPrimary.TabIndex = 1;
            this.rdoPrimary.Text = "Primary only";
            this.rdoPrimary.UseVisualStyleBackColor = true;
            // 
            // txtStreamName
            // 
            this.txtStreamName.Location = new System.Drawing.Point(16, 475);
            this.txtStreamName.Name = "txtStreamName";
            this.txtStreamName.Size = new System.Drawing.Size(161, 20);
            this.txtStreamName.TabIndex = 20;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 457);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(69, 13);
            this.label2.TabIndex = 19;
            this.label2.Text = "Stream name";
            // 
            // txtSiteName
            // 
            this.txtSiteName.Location = new System.Drawing.Point(16, 429);
            this.txtSiteName.Name = "txtSiteName";
            this.txtSiteName.Size = new System.Drawing.Size(161, 20);
            this.txtSiteName.TabIndex = 18;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 411);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(54, 13);
            this.label1.TabIndex = 17;
            this.label1.Text = "Site name";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstWatershed);
            this.groupBox1.Location = new System.Drawing.Point(16, 155);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(161, 166);
            this.groupBox1.TabIndex = 15;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Watersheds";
            // 
            // lstWatershed
            // 
            this.lstWatershed.CheckOnClick = true;
            this.lstWatershed.FormattingEnabled = true;
            this.lstWatershed.Location = new System.Drawing.Point(7, 20);
            this.lstWatershed.Name = "lstWatershed";
            this.lstWatershed.Size = new System.Drawing.Size(148, 139);
            this.lstWatershed.TabIndex = 0;
            // 
            // grpFieldSeason
            // 
            this.grpFieldSeason.Controls.Add(this.lstFieldSeason);
            this.grpFieldSeason.Location = new System.Drawing.Point(16, 35);
            this.grpFieldSeason.Name = "grpFieldSeason";
            this.grpFieldSeason.Size = new System.Drawing.Size(161, 108);
            this.grpFieldSeason.TabIndex = 14;
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
            this.valVisitID.Location = new System.Drawing.Point(82, 9);
            this.valVisitID.Maximum = new decimal(new int[] {
            9999,
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
            this.valVisitID.TabIndex = 13;
            this.valVisitID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // chkVisitID
            // 
            this.chkVisitID.AutoSize = true;
            this.chkVisitID.Location = new System.Drawing.Point(16, 11);
            this.chkVisitID.Name = "chkVisitID";
            this.chkVisitID.Size = new System.Drawing.Size(59, 17);
            this.chkVisitID.TabIndex = 12;
            this.chkVisitID.Text = "Visit ID";
            this.chkVisitID.UseVisualStyleBackColor = true;
            // 
            // grdVisits
            // 
            this.grdVisits.AllowUserToAddRows = false;
            this.grdVisits.AllowUserToDeleteRows = false;
            this.grdVisits.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdVisits.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdVisits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdVisits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colWatershedID,
            this.colProgramID,
            this.colWatershedName,
            this.colFieldSeason,
            this.colSiteName,
            this.colVisitID,
            this.colStreamName,
            this.colOrganization,
            this.colHitchName,
            this.colProgram,
            this.colCrewName,
            this.colVisitPhase,
            this.colVisitStatus,
            this.colIsPrimary,
            this.colQCVisit,
            this.colHasStreamTempLogger,
            this.colHasFishData,
            this.colCategoryName,
            this.colSiteID,
            this.colSampleDate,
            this.colPanel,
            this.colCahnnelUnits});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdVisits.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdVisits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdVisits.Location = new System.Drawing.Point(0, 0);
            this.grdVisits.Name = "grdVisits";
            this.grdVisits.ReadOnly = true;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdVisits.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grdVisits.RowHeadersVisible = false;
            this.grdVisits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdVisits.Size = new System.Drawing.Size(648, 608);
            this.grdVisits.TabIndex = 1;
            // 
            // colWatershedID
            // 
            this.colWatershedID.DataPropertyName = "WatershedID";
            this.colWatershedID.HeaderText = "Watershed ID";
            this.colWatershedID.Name = "colWatershedID";
            this.colWatershedID.ReadOnly = true;
            this.colWatershedID.Visible = false;
            // 
            // colProgramID
            // 
            this.colProgramID.DataPropertyName = "ProgramID";
            this.colProgramID.HeaderText = "ProgramID";
            this.colProgramID.Name = "colProgramID";
            this.colProgramID.ReadOnly = true;
            this.colProgramID.Visible = false;
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
            // colStreamName
            // 
            this.colStreamName.DataPropertyName = "StreamName";
            this.colStreamName.HeaderText = "Stream";
            this.colStreamName.Name = "colStreamName";
            this.colStreamName.ReadOnly = true;
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
            // colProgram
            // 
            this.colProgram.DataPropertyName = "ProgramName";
            this.colProgram.HeaderText = "Program";
            this.colProgram.Name = "colProgram";
            this.colProgram.ReadOnly = true;
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
            this.colHasStreamTempLogger.DataPropertyName = "HasStreamTempLogger";
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
            // frmVisitDataGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(845, 608);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmVisitDataGrid";
            this.Text = "Visits";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.grpFieldSeason.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.valVisitID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.grdVisits)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.TextBox txtOrganization;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckedListBox lstPrograms;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rdoAll;
        private System.Windows.Forms.RadioButton rdoPrimary;
        private System.Windows.Forms.TextBox txtStreamName;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtSiteName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox lstWatershed;
        private System.Windows.Forms.GroupBox grpFieldSeason;
        private System.Windows.Forms.CheckedListBox lstFieldSeason;
        private System.Windows.Forms.NumericUpDown valVisitID;
        private System.Windows.Forms.CheckBox chkVisitID;
        private System.Windows.Forms.DataGridView grdVisits;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWatershedID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProgramID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWatershedName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFieldSeason;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSiteName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVisitID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStreamName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colOrganization;
        private System.Windows.Forms.DataGridViewTextBoxColumn colHitchName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProgram;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCrewName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVisitPhase;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVisitStatus;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colIsPrimary;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colQCVisit;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colHasStreamTempLogger;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colHasFishData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCategoryName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSiteID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSampleDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPanel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCahnnelUnits;
    }
}