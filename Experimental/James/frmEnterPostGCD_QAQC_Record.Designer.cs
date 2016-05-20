namespace CHaMPWorkbench.Experimental.James
{
    partial class frmEnterPostGCD_QAQC_Record
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
            this.lblNewVisitID = new System.Windows.Forms.Label();
            this.lblOldVisitID = new System.Windows.Forms.Label();
            this.lblErrorType = new System.Windows.Forms.Label();
            this.lblVisitSourceOfError = new System.Windows.Forms.Label();
            this.lblEnteredBy = new System.Windows.Forms.Label();
            this.lblComment = new System.Windows.Forms.Label();
            this.txtEnteredBy = new System.Windows.Forms.TextBox();
            this.rdoResultsValidTrue = new System.Windows.Forms.RadioButton();
            this.grbResultsValid = new System.Windows.Forms.GroupBox();
            this.rdoResultsValidFalse = new System.Windows.Forms.RadioButton();
            this.cboErrorType = new System.Windows.Forms.ComboBox();
            this.cboErrorDEM = new System.Windows.Forms.ComboBox();
            this.valNewVisitID = new System.Windows.Forms.NumericUpDown();
            this.valOldVisitID = new System.Windows.Forms.NumericUpDown();
            this.txtComments = new System.Windows.Forms.TextBox();
            this.grbBasicGCDInfo = new System.Windows.Forms.GroupBox();
            this.txtMask = new System.Windows.Forms.TextBox();
            this.lblMask = new System.Windows.Forms.Label();
            this.txtOldVisitDate = new System.Windows.Forms.TextBox();
            this.txtNewVisitDate = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNewVisitDate = new System.Windows.Forms.Label();
            this.txtWatershed = new System.Windows.Forms.TextBox();
            this.txtSite = new System.Windows.Forms.TextBox();
            this.lblWatershed = new System.Windows.Forms.Label();
            this.lblSite = new System.Windows.Forms.Label();
            this.txtReasonForFlag = new System.Windows.Forms.TextBox();
            this.lblReasonForFlagging = new System.Windows.Forms.Label();
            this.grbQAQC_Info = new System.Windows.Forms.GroupBox();
            this.chkProcessed = new System.Windows.Forms.CheckBox();
            this.dgvGCD_Review = new System.Windows.Forms.DataGridView();
            this.cmdSubmit = new System.Windows.Forms.Button();
            this.cmdOutputToJSON = new System.Windows.Forms.Button();
            this.grbGCD_ReviewTable = new System.Windows.Forms.GroupBox();
            this.cmsGCD_Visit = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exploreSiteLevelUSGSStreamGageDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grbResultsValid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valNewVisitID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valOldVisitID)).BeginInit();
            this.grbBasicGCDInfo.SuspendLayout();
            this.grbQAQC_Info.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGCD_Review)).BeginInit();
            this.grbGCD_ReviewTable.SuspendLayout();
            this.cmsGCD_Visit.SuspendLayout();
            this.SuspendLayout();
            // 
            // lblNewVisitID
            // 
            this.lblNewVisitID.AutoSize = true;
            this.lblNewVisitID.Location = new System.Drawing.Point(43, 93);
            this.lblNewVisitID.Name = "lblNewVisitID";
            this.lblNewVisitID.Size = new System.Drawing.Size(65, 13);
            this.lblNewVisitID.TabIndex = 102;
            this.lblNewVisitID.Text = "New Visit ID";
            // 
            // lblOldVisitID
            // 
            this.lblOldVisitID.AutoSize = true;
            this.lblOldVisitID.Location = new System.Drawing.Point(168, 93);
            this.lblOldVisitID.Name = "lblOldVisitID";
            this.lblOldVisitID.Size = new System.Drawing.Size(59, 13);
            this.lblOldVisitID.TabIndex = 103;
            this.lblOldVisitID.Text = "Old Visit ID";
            // 
            // lblErrorType
            // 
            this.lblErrorType.AutoSize = true;
            this.lblErrorType.Location = new System.Drawing.Point(143, 19);
            this.lblErrorType.Name = "lblErrorType";
            this.lblErrorType.Size = new System.Drawing.Size(56, 13);
            this.lblErrorType.TabIndex = 106;
            this.lblErrorType.Text = "Error Type";
            // 
            // lblVisitSourceOfError
            // 
            this.lblVisitSourceOfError.AutoSize = true;
            this.lblVisitSourceOfError.Location = new System.Drawing.Point(143, 75);
            this.lblVisitSourceOfError.Name = "lblVisitSourceOfError";
            this.lblVisitSourceOfError.Size = new System.Drawing.Size(56, 13);
            this.lblVisitSourceOfError.TabIndex = 107;
            this.lblVisitSourceOfError.Text = "Error DEM";
            // 
            // lblEnteredBy
            // 
            this.lblEnteredBy.AutoSize = true;
            this.lblEnteredBy.Location = new System.Drawing.Point(19, 264);
            this.lblEnteredBy.Name = "lblEnteredBy";
            this.lblEnteredBy.Size = new System.Drawing.Size(59, 13);
            this.lblEnteredBy.TabIndex = 108;
            this.lblEnteredBy.Text = "Entered By";
            // 
            // lblComment
            // 
            this.lblComment.AutoSize = true;
            this.lblComment.Location = new System.Drawing.Point(19, 113);
            this.lblComment.Name = "lblComment";
            this.lblComment.Size = new System.Drawing.Size(56, 13);
            this.lblComment.TabIndex = 110;
            this.lblComment.Text = "Comments";
            // 
            // txtEnteredBy
            // 
            this.txtEnteredBy.Location = new System.Drawing.Point(84, 261);
            this.txtEnteredBy.Name = "txtEnteredBy";
            this.txtEnteredBy.Size = new System.Drawing.Size(156, 20);
            this.txtEnteredBy.TabIndex = 9;
            // 
            // rdoResultsValidTrue
            // 
            this.rdoResultsValidTrue.AutoSize = true;
            this.rdoResultsValidTrue.Location = new System.Drawing.Point(29, 19);
            this.rdoResultsValidTrue.Name = "rdoResultsValidTrue";
            this.rdoResultsValidTrue.Size = new System.Drawing.Size(47, 17);
            this.rdoResultsValidTrue.TabIndex = 4;
            this.rdoResultsValidTrue.TabStop = true;
            this.rdoResultsValidTrue.Text = "True";
            this.rdoResultsValidTrue.UseVisualStyleBackColor = true;
            // 
            // grbResultsValid
            // 
            this.grbResultsValid.Controls.Add(this.rdoResultsValidFalse);
            this.grbResultsValid.Controls.Add(this.rdoResultsValidTrue);
            this.grbResultsValid.Location = new System.Drawing.Point(22, 19);
            this.grbResultsValid.Name = "grbResultsValid";
            this.grbResultsValid.Size = new System.Drawing.Size(97, 69);
            this.grbResultsValid.TabIndex = 105;
            this.grbResultsValid.TabStop = false;
            this.grbResultsValid.Text = "Results Valid";
            // 
            // rdoResultsValidFalse
            // 
            this.rdoResultsValidFalse.AutoSize = true;
            this.rdoResultsValidFalse.Location = new System.Drawing.Point(29, 42);
            this.rdoResultsValidFalse.Name = "rdoResultsValidFalse";
            this.rdoResultsValidFalse.Size = new System.Drawing.Size(50, 17);
            this.rdoResultsValidFalse.TabIndex = 5;
            this.rdoResultsValidFalse.TabStop = true;
            this.rdoResultsValidFalse.Text = "False";
            this.rdoResultsValidFalse.UseVisualStyleBackColor = true;
            // 
            // cboErrorType
            // 
            this.cboErrorType.FormattingEnabled = true;
            this.cboErrorType.Items.AddRange(new object[] {
            "None",
            "Datum Shift",
            "Rod Height Bust",
            "Other"});
            this.cboErrorType.Location = new System.Drawing.Point(143, 41);
            this.cboErrorType.Name = "cboErrorType";
            this.cboErrorType.Size = new System.Drawing.Size(221, 21);
            this.cboErrorType.TabIndex = 6;
            // 
            // cboErrorDEM
            // 
            this.cboErrorDEM.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboErrorDEM.FormattingEnabled = true;
            this.cboErrorDEM.Items.AddRange(new object[] {
            "Neither",
            "New Visit",
            "Old Visit",
            "Both",
            "Unknown"});
            this.cboErrorDEM.Location = new System.Drawing.Point(143, 97);
            this.cboErrorDEM.Name = "cboErrorDEM";
            this.cboErrorDEM.Size = new System.Drawing.Size(221, 21);
            this.cboErrorDEM.TabIndex = 7;
            // 
            // valNewVisitID
            // 
            this.valNewVisitID.Enabled = false;
            this.valNewVisitID.Location = new System.Drawing.Point(46, 109);
            this.valNewVisitID.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.valNewVisitID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valNewVisitID.Name = "valNewVisitID";
            this.valNewVisitID.ReadOnly = true;
            this.valNewVisitID.Size = new System.Drawing.Size(78, 20);
            this.valNewVisitID.TabIndex = 1;
            this.valNewVisitID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // valOldVisitID
            // 
            this.valOldVisitID.Enabled = false;
            this.valOldVisitID.Location = new System.Drawing.Point(171, 109);
            this.valOldVisitID.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.valOldVisitID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valOldVisitID.Name = "valOldVisitID";
            this.valOldVisitID.ReadOnly = true;
            this.valOldVisitID.Size = new System.Drawing.Size(77, 20);
            this.valOldVisitID.TabIndex = 2;
            this.valOldVisitID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // txtComments
            // 
            this.txtComments.Location = new System.Drawing.Point(22, 129);
            this.txtComments.Multiline = true;
            this.txtComments.Name = "txtComments";
            this.txtComments.Size = new System.Drawing.Size(342, 122);
            this.txtComments.TabIndex = 8;
            // 
            // grbBasicGCDInfo
            // 
            this.grbBasicGCDInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grbBasicGCDInfo.Controls.Add(this.txtMask);
            this.grbBasicGCDInfo.Controls.Add(this.lblMask);
            this.grbBasicGCDInfo.Controls.Add(this.txtOldVisitDate);
            this.grbBasicGCDInfo.Controls.Add(this.txtNewVisitDate);
            this.grbBasicGCDInfo.Controls.Add(this.label1);
            this.grbBasicGCDInfo.Controls.Add(this.lblNewVisitDate);
            this.grbBasicGCDInfo.Controls.Add(this.txtWatershed);
            this.grbBasicGCDInfo.Controls.Add(this.txtSite);
            this.grbBasicGCDInfo.Controls.Add(this.lblWatershed);
            this.grbBasicGCDInfo.Controls.Add(this.lblSite);
            this.grbBasicGCDInfo.Controls.Add(this.txtReasonForFlag);
            this.grbBasicGCDInfo.Controls.Add(this.lblReasonForFlagging);
            this.grbBasicGCDInfo.Controls.Add(this.lblOldVisitID);
            this.grbBasicGCDInfo.Controls.Add(this.valOldVisitID);
            this.grbBasicGCDInfo.Controls.Add(this.lblNewVisitID);
            this.grbBasicGCDInfo.Controls.Add(this.valNewVisitID);
            this.grbBasicGCDInfo.Location = new System.Drawing.Point(12, 13);
            this.grbBasicGCDInfo.Name = "grbBasicGCDInfo";
            this.grbBasicGCDInfo.Size = new System.Drawing.Size(385, 219);
            this.grbBasicGCDInfo.TabIndex = 100;
            this.grbBasicGCDInfo.TabStop = false;
            this.grbBasicGCDInfo.Text = "Basic GCD Info";
            // 
            // txtMask
            // 
            this.txtMask.Location = new System.Drawing.Point(262, 109);
            this.txtMask.Name = "txtMask";
            this.txtMask.ReadOnly = true;
            this.txtMask.Size = new System.Drawing.Size(78, 20);
            this.txtMask.TabIndex = 114;
            // 
            // lblMask
            // 
            this.lblMask.AutoSize = true;
            this.lblMask.Location = new System.Drawing.Point(259, 93);
            this.lblMask.Name = "lblMask";
            this.lblMask.Size = new System.Drawing.Size(33, 13);
            this.lblMask.TabIndex = 113;
            this.lblMask.Text = "Mask";
            // 
            // txtOldVisitDate
            // 
            this.txtOldVisitDate.Location = new System.Drawing.Point(170, 137);
            this.txtOldVisitDate.Name = "txtOldVisitDate";
            this.txtOldVisitDate.ReadOnly = true;
            this.txtOldVisitDate.Size = new System.Drawing.Size(78, 20);
            this.txtOldVisitDate.TabIndex = 112;
            // 
            // txtNewVisitDate
            // 
            this.txtNewVisitDate.Location = new System.Drawing.Point(46, 137);
            this.txtNewVisitDate.Name = "txtNewVisitDate";
            this.txtNewVisitDate.ReadOnly = true;
            this.txtNewVisitDate.Size = new System.Drawing.Size(78, 20);
            this.txtNewVisitDate.TabIndex = 111;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(131, 140);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(33, 13);
            this.label1.TabIndex = 110;
            this.label1.Text = "Date:";
            // 
            // lblNewVisitDate
            // 
            this.lblNewVisitDate.AutoSize = true;
            this.lblNewVisitDate.Location = new System.Drawing.Point(9, 140);
            this.lblNewVisitDate.Name = "lblNewVisitDate";
            this.lblNewVisitDate.Size = new System.Drawing.Size(33, 13);
            this.lblNewVisitDate.TabIndex = 109;
            this.lblNewVisitDate.Text = "Date:";
            // 
            // txtWatershed
            // 
            this.txtWatershed.Location = new System.Drawing.Point(77, 57);
            this.txtWatershed.Name = "txtWatershed";
            this.txtWatershed.ReadOnly = true;
            this.txtWatershed.Size = new System.Drawing.Size(302, 20);
            this.txtWatershed.TabIndex = 108;
            // 
            // txtSite
            // 
            this.txtSite.Location = new System.Drawing.Point(44, 27);
            this.txtSite.Name = "txtSite";
            this.txtSite.ReadOnly = true;
            this.txtSite.Size = new System.Drawing.Size(335, 20);
            this.txtSite.TabIndex = 107;
            // 
            // lblWatershed
            // 
            this.lblWatershed.AutoSize = true;
            this.lblWatershed.Location = new System.Drawing.Point(9, 60);
            this.lblWatershed.Name = "lblWatershed";
            this.lblWatershed.Size = new System.Drawing.Size(62, 13);
            this.lblWatershed.TabIndex = 106;
            this.lblWatershed.Text = "Watershed:";
            // 
            // lblSite
            // 
            this.lblSite.AutoSize = true;
            this.lblSite.Location = new System.Drawing.Point(10, 30);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(28, 13);
            this.lblSite.TabIndex = 105;
            this.lblSite.Text = "Site:";
            // 
            // txtReasonForFlag
            // 
            this.txtReasonForFlag.Location = new System.Drawing.Point(11, 180);
            this.txtReasonForFlag.Multiline = true;
            this.txtReasonForFlag.Name = "txtReasonForFlag";
            this.txtReasonForFlag.ReadOnly = true;
            this.txtReasonForFlag.Size = new System.Drawing.Size(368, 33);
            this.txtReasonForFlag.TabIndex = 3;
            // 
            // lblReasonForFlagging
            // 
            this.lblReasonForFlagging.AutoSize = true;
            this.lblReasonForFlagging.Location = new System.Drawing.Point(8, 164);
            this.lblReasonForFlagging.Name = "lblReasonForFlagging";
            this.lblReasonForFlagging.Size = new System.Drawing.Size(85, 13);
            this.lblReasonForFlagging.TabIndex = 104;
            this.lblReasonForFlagging.Text = "Reason For Flag";
            // 
            // grbQAQC_Info
            // 
            this.grbQAQC_Info.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.grbQAQC_Info.Controls.Add(this.chkProcessed);
            this.grbQAQC_Info.Controls.Add(this.grbResultsValid);
            this.grbQAQC_Info.Controls.Add(this.lblErrorType);
            this.grbQAQC_Info.Controls.Add(this.lblVisitSourceOfError);
            this.grbQAQC_Info.Controls.Add(this.txtEnteredBy);
            this.grbQAQC_Info.Controls.Add(this.txtComments);
            this.grbQAQC_Info.Controls.Add(this.lblComment);
            this.grbQAQC_Info.Controls.Add(this.lblEnteredBy);
            this.grbQAQC_Info.Controls.Add(this.cboErrorDEM);
            this.grbQAQC_Info.Controls.Add(this.cboErrorType);
            this.grbQAQC_Info.Location = new System.Drawing.Point(12, 233);
            this.grbQAQC_Info.Name = "grbQAQC_Info";
            this.grbQAQC_Info.Size = new System.Drawing.Size(385, 306);
            this.grbQAQC_Info.TabIndex = 104;
            this.grbQAQC_Info.TabStop = false;
            this.grbQAQC_Info.Text = "QA/QC Info";
            // 
            // chkProcessed
            // 
            this.chkProcessed.AutoSize = true;
            this.chkProcessed.Enabled = false;
            this.chkProcessed.Location = new System.Drawing.Point(284, 264);
            this.chkProcessed.Name = "chkProcessed";
            this.chkProcessed.Size = new System.Drawing.Size(76, 17);
            this.chkProcessed.TabIndex = 111;
            this.chkProcessed.Text = "Processed";
            this.chkProcessed.UseVisualStyleBackColor = true;
            // 
            // dgvGCD_Review
            // 
            this.dgvGCD_Review.AllowUserToAddRows = false;
            this.dgvGCD_Review.AllowUserToDeleteRows = false;
            this.dgvGCD_Review.AllowUserToResizeRows = false;
            this.dgvGCD_Review.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvGCD_Review.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvGCD_Review.Location = new System.Drawing.Point(3, 16);
            this.dgvGCD_Review.MultiSelect = false;
            this.dgvGCD_Review.Name = "dgvGCD_Review";
            this.dgvGCD_Review.ReadOnly = true;
            this.dgvGCD_Review.RowHeadersVisible = false;
            this.dgvGCD_Review.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dgvGCD_Review.Size = new System.Drawing.Size(738, 510);
            this.dgvGCD_Review.TabIndex = 12;
            this.dgvGCD_Review.TabStop = false;
            this.dgvGCD_Review.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dgvGCD_Review_CellClick);
            // 
            // cmdSubmit
            // 
            this.cmdSubmit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdSubmit.Location = new System.Drawing.Point(274, 553);
            this.cmdSubmit.Name = "cmdSubmit";
            this.cmdSubmit.Size = new System.Drawing.Size(123, 23);
            this.cmdSubmit.TabIndex = 10;
            this.cmdSubmit.Text = "Update Record";
            this.cmdSubmit.UseVisualStyleBackColor = true;
            this.cmdSubmit.Click += new System.EventHandler(this.cmdSubmit_Click);
            // 
            // cmdOutputToJSON
            // 
            this.cmdOutputToJSON.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOutputToJSON.Location = new System.Drawing.Point(1043, 553);
            this.cmdOutputToJSON.Name = "cmdOutputToJSON";
            this.cmdOutputToJSON.Size = new System.Drawing.Size(104, 23);
            this.cmdOutputToJSON.TabIndex = 11;
            this.cmdOutputToJSON.Text = "Output to JSON";
            this.cmdOutputToJSON.UseVisualStyleBackColor = true;
            this.cmdOutputToJSON.Click += new System.EventHandler(this.cmdOutputToJSON_Click);
            // 
            // grbGCD_ReviewTable
            // 
            this.grbGCD_ReviewTable.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbGCD_ReviewTable.Controls.Add(this.dgvGCD_Review);
            this.grbGCD_ReviewTable.Location = new System.Drawing.Point(403, 13);
            this.grbGCD_ReviewTable.Name = "grbGCD_ReviewTable";
            this.grbGCD_ReviewTable.Size = new System.Drawing.Size(744, 529);
            this.grbGCD_ReviewTable.TabIndex = 106;
            this.grbGCD_ReviewTable.TabStop = false;
            this.grbGCD_ReviewTable.Text = "GCD Review Table";
            // 
            // cmsGCD_Visit
            // 
            this.cmsGCD_Visit.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem,
            this.exploreSiteLevelUSGSStreamGageDataToolStripMenuItem});
            this.cmsGCD_Visit.Name = "cmsGCD";
            this.cmsGCD_Visit.Size = new System.Drawing.Size(318, 48);
            // 
            // downloadTopoAndHydroDataFromCmorgToolStripMenuItem
            // 
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.download;
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem.Name = "downloadTopoAndHydroDataFromCmorgToolStripMenuItem";
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem.Size = new System.Drawing.Size(317, 22);
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem.Text = "Download Topo and Hydro Data From cm.org";
            this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem.Click += new System.EventHandler(this.downloadTopoAndHydroDataFromCmorgToolStripMenuItem_Click);
            // 
            // exploreSiteLevelUSGSStreamGageDataToolStripMenuItem
            // 
            this.exploreSiteLevelUSGSStreamGageDataToolStripMenuItem.Name = "exploreSiteLevelUSGSStreamGageDataToolStripMenuItem";
            this.exploreSiteLevelUSGSStreamGageDataToolStripMenuItem.Size = new System.Drawing.Size(317, 22);
            this.exploreSiteLevelUSGSStreamGageDataToolStripMenuItem.Text = "Explore Site Level USGS Stream Gage Data";
            this.exploreSiteLevelUSGSStreamGageDataToolStripMenuItem.Click += new System.EventHandler(this.exploreSiteLevelUSGSStreamGageDataToolStripMenuItem_Click);
            // 
            // frmEnterPostGCD_QAQC_Record
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1159, 583);
            this.Controls.Add(this.grbGCD_ReviewTable);
            this.Controls.Add(this.cmdOutputToJSON);
            this.Controls.Add(this.cmdSubmit);
            this.Controls.Add(this.grbQAQC_Info);
            this.Controls.Add(this.grbBasicGCDInfo);
            this.MinimumSize = new System.Drawing.Size(1011, 535);
            this.Name = "frmEnterPostGCD_QAQC_Record";
            this.ShowIcon = false;
            this.Text = "Enter Post GCD QA/QC Record";
            this.Load += new System.EventHandler(this.frmEnterPostGCD_QAQC_Record_Load);
            this.grbResultsValid.ResumeLayout(false);
            this.grbResultsValid.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.valNewVisitID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valOldVisitID)).EndInit();
            this.grbBasicGCDInfo.ResumeLayout(false);
            this.grbBasicGCDInfo.PerformLayout();
            this.grbQAQC_Info.ResumeLayout(false);
            this.grbQAQC_Info.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvGCD_Review)).EndInit();
            this.grbGCD_ReviewTable.ResumeLayout(false);
            this.cmsGCD_Visit.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lblNewVisitID;
        private System.Windows.Forms.Label lblOldVisitID;
        private System.Windows.Forms.Label lblErrorType;
        private System.Windows.Forms.Label lblVisitSourceOfError;
        private System.Windows.Forms.Label lblEnteredBy;
        private System.Windows.Forms.Label lblComment;
        private System.Windows.Forms.TextBox txtEnteredBy;
        private System.Windows.Forms.RadioButton rdoResultsValidTrue;
        private System.Windows.Forms.GroupBox grbResultsValid;
        private System.Windows.Forms.RadioButton rdoResultsValidFalse;
        private System.Windows.Forms.ComboBox cboErrorType;
        private System.Windows.Forms.ComboBox cboErrorDEM;
        private System.Windows.Forms.NumericUpDown valNewVisitID;
        private System.Windows.Forms.NumericUpDown valOldVisitID;
        private System.Windows.Forms.TextBox txtComments;
        private System.Windows.Forms.GroupBox grbBasicGCDInfo;
        private System.Windows.Forms.GroupBox grbQAQC_Info;
        private System.Windows.Forms.DataGridView dgvGCD_Review;
        private System.Windows.Forms.Button cmdSubmit;
        private System.Windows.Forms.Button cmdOutputToJSON;
        private System.Windows.Forms.Label lblReasonForFlagging;
        private System.Windows.Forms.GroupBox grbGCD_ReviewTable;
        private System.Windows.Forms.TextBox txtReasonForFlag;
        private System.Windows.Forms.ContextMenuStrip cmsGCD_Visit;
        private System.Windows.Forms.ToolStripMenuItem downloadTopoAndHydroDataFromCmorgToolStripMenuItem;
        private System.Windows.Forms.TextBox txtWatershed;
        private System.Windows.Forms.TextBox txtSite;
        private System.Windows.Forms.Label lblWatershed;
        private System.Windows.Forms.Label lblSite;
        private System.Windows.Forms.TextBox txtOldVisitDate;
        private System.Windows.Forms.TextBox txtNewVisitDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNewVisitDate;
        private System.Windows.Forms.ToolStripMenuItem exploreSiteLevelUSGSStreamGageDataToolStripMenuItem;
        private System.Windows.Forms.TextBox txtMask;
        private System.Windows.Forms.Label lblMask;
        private System.Windows.Forms.CheckBox chkProcessed;
    }
}