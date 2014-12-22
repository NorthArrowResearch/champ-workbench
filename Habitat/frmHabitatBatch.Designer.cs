namespace CHaMPWorkbench.Habitat
{
    partial class frmHabitatBatch
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.chkFieldSeasons = new System.Windows.Forms.CheckedListBox();
            this.chkWatersheds = new System.Windows.Forms.CheckedListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.chkVisitTypes = new System.Windows.Forms.CheckedListBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.chkPrimary = new System.Windows.Forms.CheckBox();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.tabControl2 = new System.Windows.Forms.TabControl();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.grdVisits = new System.Windows.Forms.DataGridView();
            this.colSelected = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colWatershed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFieldSeason = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSite = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colFolder = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.cmdHabitatModelDB = new System.Windows.Forms.Button();
            this.txtHabitatModelDB = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cboHabitatModel = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdBrowseMonitoringDataFolder = new System.Windows.Forms.Button();
            this.txtMonitoringFolder = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.bindingSourceSelectedVisits = new System.Windows.Forms.BindingSource(this.components);
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabControl2.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVisits)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.tabPage6.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSelectedVisits)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(607, 569);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(526, 569);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // chkFieldSeasons
            // 
            this.chkFieldSeasons.CheckOnClick = true;
            this.chkFieldSeasons.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkFieldSeasons.FormattingEnabled = true;
            this.chkFieldSeasons.Location = new System.Drawing.Point(3, 3);
            this.chkFieldSeasons.Name = "chkFieldSeasons";
            this.chkFieldSeasons.Size = new System.Drawing.Size(573, 167);
            this.chkFieldSeasons.TabIndex = 2;
            this.chkFieldSeasons.SelectedIndexChanged += new System.EventHandler(this.FilterVisits);
            // 
            // chkWatersheds
            // 
            this.chkWatersheds.CheckOnClick = true;
            this.chkWatersheds.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkWatersheds.FormattingEnabled = true;
            this.chkWatersheds.Location = new System.Drawing.Point(3, 3);
            this.chkWatersheds.Name = "chkWatersheds";
            this.chkWatersheds.Size = new System.Drawing.Size(573, 167);
            this.chkWatersheds.TabIndex = 5;
            this.chkWatersheds.SelectedIndexChanged += new System.EventHandler(this.FilterVisits);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Location = new System.Drawing.Point(6, 19);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(587, 199);
            this.tabControl1.TabIndex = 6;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.chkFieldSeasons);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(579, 173);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Field Seasons";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.chkWatersheds);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(579, 173);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Watersheds";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.chkVisitTypes);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage3.Size = new System.Drawing.Size(579, 173);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Visit Types";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // chkVisitTypes
            // 
            this.chkVisitTypes.CheckOnClick = true;
            this.chkVisitTypes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.chkVisitTypes.FormattingEnabled = true;
            this.chkVisitTypes.Location = new System.Drawing.Point(3, 3);
            this.chkVisitTypes.Name = "chkVisitTypes";
            this.chkVisitTypes.Size = new System.Drawing.Size(573, 167);
            this.chkVisitTypes.TabIndex = 0;
            this.chkVisitTypes.SelectedIndexChanged += new System.EventHandler(this.FilterVisits);
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.chkPrimary);
            this.tabPage4.Controls.Add(this.checkBox2);
            this.tabPage4.Controls.Add(this.checkBox1);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(579, 173);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Other Criteria";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // chkPrimary
            // 
            this.chkPrimary.AutoSize = true;
            this.chkPrimary.Checked = true;
            this.chkPrimary.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPrimary.Location = new System.Drawing.Point(7, 53);
            this.chkPrimary.Name = "chkPrimary";
            this.chkPrimary.Size = new System.Drawing.Size(130, 17);
            this.chkPrimary.TabIndex = 2;
            this.chkPrimary.Text = "Must be a primary visit";
            this.chkPrimary.UseVisualStyleBackColor = true;
            this.chkPrimary.CheckedChanged += new System.EventHandler(this.FilterVisits);
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Checked = true;
            this.checkBox2.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox2.Enabled = false;
            this.checkBox2.Location = new System.Drawing.Point(7, 30);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(247, 17);
            this.checkBox2.TabIndex = 1;
            this.checkBox2.Text = "Must have hydraulic model output CSV defined";
            this.checkBox2.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Checked = true;
            this.checkBox1.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox1.Enabled = false;
            this.checkBox1.Location = new System.Drawing.Point(7, 7);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(162, 17);
            this.checkBox1.TabIndex = 0;
            this.checkBox1.Text = "Must have topo data defined";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // tabControl2
            // 
            this.tabControl2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl2.Controls.Add(this.tabPage5);
            this.tabControl2.Controls.Add(this.tabPage6);
            this.tabControl2.Location = new System.Drawing.Point(12, 12);
            this.tabControl2.Name = "tabControl2";
            this.tabControl2.SelectedIndex = 0;
            this.tabControl2.Size = new System.Drawing.Size(672, 547);
            this.tabControl2.TabIndex = 7;
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.groupBox2);
            this.tabPage5.Controls.Add(this.groupBox1);
            this.tabPage5.Location = new System.Drawing.Point(4, 22);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage5.Size = new System.Drawing.Size(664, 521);
            this.tabPage5.TabIndex = 0;
            this.tabPage5.Text = "Visits";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.grdVisits);
            this.groupBox2.Location = new System.Drawing.Point(6, 236);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(599, 282);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Selected Visits";
            // 
            // grdVisits
            // 
            this.grdVisits.AllowUserToAddRows = false;
            this.grdVisits.AllowUserToDeleteRows = false;
            this.grdVisits.AllowUserToResizeRows = false;
            this.grdVisits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdVisits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colSelected,
            this.colWatershed,
            this.colFieldSeason,
            this.colSite,
            this.colFolder});
            this.grdVisits.Location = new System.Drawing.Point(6, 42);
            this.grdVisits.MultiSelect = false;
            this.grdVisits.Name = "grdVisits";
            this.grdVisits.RowHeadersVisible = false;
            this.grdVisits.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grdVisits.Size = new System.Drawing.Size(587, 237);
            this.grdVisits.TabIndex = 0;
            // 
            // colSelected
            // 
            this.colSelected.DataPropertyName = "Selected";
            this.colSelected.HeaderText = "";
            this.colSelected.Name = "colSelected";
            this.colSelected.Width = 30;
            // 
            // colWatershed
            // 
            this.colWatershed.DataPropertyName = "Watershed";
            this.colWatershed.HeaderText = "Watershed";
            this.colWatershed.Name = "colWatershed";
            this.colWatershed.ReadOnly = true;
            // 
            // colFieldSeason
            // 
            this.colFieldSeason.DataPropertyName = "FieldSeason";
            this.colFieldSeason.HeaderText = "Season";
            this.colFieldSeason.Name = "colFieldSeason";
            this.colFieldSeason.ReadOnly = true;
            this.colFieldSeason.Width = 50;
            // 
            // colSite
            // 
            this.colSite.DataPropertyName = "Site";
            this.colSite.HeaderText = "Site";
            this.colSite.Name = "colSite";
            this.colSite.ReadOnly = true;
            this.colSite.Width = 200;
            // 
            // colFolder
            // 
            this.colFolder.DataPropertyName = "TopoFolder";
            this.colFolder.HeaderText = "Folder";
            this.colFolder.Name = "colFolder";
            this.colFolder.ReadOnly = true;
            this.colFolder.Width = 300;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.tabControl1);
            this.groupBox1.Location = new System.Drawing.Point(6, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(599, 224);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Visit Filters";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.cmdHabitatModelDB);
            this.tabPage6.Controls.Add(this.txtHabitatModelDB);
            this.tabPage6.Controls.Add(this.label3);
            this.tabPage6.Controls.Add(this.cboHabitatModel);
            this.tabPage6.Controls.Add(this.label2);
            this.tabPage6.Controls.Add(this.cmdBrowseMonitoringDataFolder);
            this.tabPage6.Controls.Add(this.txtMonitoringFolder);
            this.tabPage6.Controls.Add(this.label1);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(664, 521);
            this.tabPage6.TabIndex = 1;
            this.tabPage6.Text = "Habitat Model";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // cmdHabitatModelDB
            // 
            this.cmdHabitatModelDB.Image = global::CHaMPWorkbench.Properties.Resources.BrowseFolder;
            this.cmdHabitatModelDB.Location = new System.Drawing.Point(626, 14);
            this.cmdHabitatModelDB.Name = "cmdHabitatModelDB";
            this.cmdHabitatModelDB.Size = new System.Drawing.Size(23, 23);
            this.cmdHabitatModelDB.TabIndex = 7;
            this.cmdHabitatModelDB.UseVisualStyleBackColor = true;
            this.cmdHabitatModelDB.Click += new System.EventHandler(this.cmdHabitatModelDB_Click);
            // 
            // txtHabitatModelDB
            // 
            this.txtHabitatModelDB.Location = new System.Drawing.Point(168, 15);
            this.txtHabitatModelDB.Name = "txtHabitatModelDB";
            this.txtHabitatModelDB.Size = new System.Drawing.Size(452, 20);
            this.txtHabitatModelDB.TabIndex = 6;
            this.txtHabitatModelDB.TextChanged += new System.EventHandler(this.txtHabitatModelDB_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(39, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(123, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Habitat project database";
            // 
            // cboHabitatModel
            // 
            this.cboHabitatModel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboHabitatModel.FormattingEnabled = true;
            this.cboHabitatModel.Location = new System.Drawing.Point(168, 85);
            this.cboHabitatModel.Name = "cboHabitatModel";
            this.cboHabitatModel.Size = new System.Drawing.Size(452, 21);
            this.cboHabitatModel.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(90, 89);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(72, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Habitat model";
            // 
            // cmdBrowseMonitoringDataFolder
            // 
            this.cmdBrowseMonitoringDataFolder.Image = global::CHaMPWorkbench.Properties.Resources.BrowseFolder;
            this.cmdBrowseMonitoringDataFolder.Location = new System.Drawing.Point(626, 49);
            this.cmdBrowseMonitoringDataFolder.Name = "cmdBrowseMonitoringDataFolder";
            this.cmdBrowseMonitoringDataFolder.Size = new System.Drawing.Size(23, 23);
            this.cmdBrowseMonitoringDataFolder.TabIndex = 2;
            this.cmdBrowseMonitoringDataFolder.UseVisualStyleBackColor = true;
            this.cmdBrowseMonitoringDataFolder.Click += new System.EventHandler(this.cmdBrowseMonitoringDataFolder_Click);
            // 
            // txtMonitoringFolder
            // 
            this.txtMonitoringFolder.Location = new System.Drawing.Point(168, 50);
            this.txtMonitoringFolder.Name = "txtMonitoringFolder";
            this.txtMonitoringFolder.Size = new System.Drawing.Size(452, 20);
            this.txtMonitoringFolder.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 54);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(155, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Top level monitoring data folder";
            // 
            // frmHabitatBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 604);
            this.Controls.Add(this.tabControl2);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Name = "frmHabitatBatch";
            this.Text = "frmHabitatBatch";
            this.Load += new System.EventHandler(this.frmHabitatBatch_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabControl2.ResumeLayout(false);
            this.tabPage5.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdVisits)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.bindingSourceSelectedVisits)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.CheckedListBox chkFieldSeasons;
        private System.Windows.Forms.CheckedListBox chkWatersheds;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckedListBox chkVisitTypes;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.CheckBox chkPrimary;
        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.TabControl tabControl2;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.DataGridView grdVisits;
        private System.Windows.Forms.BindingSource bindingSourceSelectedVisits;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colSelected;
        private System.Windows.Forms.DataGridViewTextBoxColumn colWatershed;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFieldSeason;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSite;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFolder;
        private System.Windows.Forms.Button cmdHabitatModelDB;
        private System.Windows.Forms.TextBox txtHabitatModelDB;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboHabitatModel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdBrowseMonitoringDataFolder;
        private System.Windows.Forms.TextBox txtMonitoringFolder;
        private System.Windows.Forms.Label label1;
    }
}