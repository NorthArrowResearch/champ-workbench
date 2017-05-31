namespace CHaMPWorkbench.Data.MetricDefinitions
{
    partial class frmMetricDefinitions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMetricDefinitions));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.chkXPath = new System.Windows.Forms.CheckBox();
            this.chkActive = new System.Windows.Forms.CheckBox();
            this.grpTitle = new System.Windows.Forms.GroupBox();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.grpSchema = new System.Windows.Forms.GroupBox();
            this.chkSchema = new System.Windows.Forms.CheckedListBox();
            this.grpModel = new System.Windows.Forms.GroupBox();
            this.chkModel = new System.Windows.Forms.CheckedListBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.dataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.exportSchemaXMLToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.createNewMetricToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.grdData = new System.Windows.Forms.DataGridView();
            this.colMetricID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisplayNameShort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSchema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colXPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMinValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colUpdated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.grpTitle.SuspendLayout();
            this.grpSchema.SuspendLayout();
            this.grpModel.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
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
            this.splitContainer1.Panel1.Controls.Add(this.chkXPath);
            this.splitContainer1.Panel1.Controls.Add(this.chkActive);
            this.splitContainer1.Panel1.Controls.Add(this.grpTitle);
            this.splitContainer1.Panel1.Controls.Add(this.grpSchema);
            this.splitContainer1.Panel1.Controls.Add(this.grpModel);
            this.splitContainer1.Panel1.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdData);
            this.splitContainer1.Size = new System.Drawing.Size(788, 519);
            this.splitContainer1.SplitterDistance = 193;
            this.splitContainer1.TabIndex = 0;
            // 
            // chkXPath
            // 
            this.chkXPath.AutoSize = true;
            this.chkXPath.Checked = true;
            this.chkXPath.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkXPath.Location = new System.Drawing.Point(12, 386);
            this.chkXPath.Name = "chkXPath";
            this.chkXPath.Size = new System.Drawing.Size(123, 17);
            this.chkXPath.TabIndex = 0;
            this.chkXPath.Text = "Possesses an XPath";
            this.chkXPath.UseVisualStyleBackColor = true;
            // 
            // chkActive
            // 
            this.chkActive.AutoSize = true;
            this.chkActive.Checked = true;
            this.chkActive.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkActive.Location = new System.Drawing.Point(12, 363);
            this.chkActive.Name = "chkActive";
            this.chkActive.Size = new System.Drawing.Size(80, 17);
            this.chkActive.TabIndex = 4;
            this.chkActive.Text = "Active Only";
            this.chkActive.UseVisualStyleBackColor = true;
            // 
            // grpTitle
            // 
            this.grpTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpTitle.Controls.Add(this.txtTitle);
            this.grpTitle.Location = new System.Drawing.Point(12, 308);
            this.grpTitle.Name = "grpTitle";
            this.grpTitle.Size = new System.Drawing.Size(178, 48);
            this.grpTitle.TabIndex = 3;
            this.grpTitle.TabStop = false;
            this.grpTitle.Text = "Title, Short Name or XPath";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(6, 19);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(166, 20);
            this.txtTitle.TabIndex = 0;
            // 
            // grpSchema
            // 
            this.grpSchema.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSchema.Controls.Add(this.chkSchema);
            this.grpSchema.Location = new System.Drawing.Point(12, 151);
            this.grpSchema.Name = "grpSchema";
            this.grpSchema.Size = new System.Drawing.Size(178, 149);
            this.grpSchema.TabIndex = 2;
            this.grpSchema.TabStop = false;
            this.grpSchema.Text = "Schema";
            // 
            // chkSchema
            // 
            this.chkSchema.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkSchema.CheckOnClick = true;
            this.chkSchema.FormattingEnabled = true;
            this.chkSchema.Location = new System.Drawing.Point(6, 19);
            this.chkSchema.Name = "chkSchema";
            this.chkSchema.Size = new System.Drawing.Size(166, 124);
            this.chkSchema.TabIndex = 0;
            // 
            // grpModel
            // 
            this.grpModel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpModel.Controls.Add(this.chkModel);
            this.grpModel.Location = new System.Drawing.Point(12, 27);
            this.grpModel.Name = "grpModel";
            this.grpModel.Size = new System.Drawing.Size(178, 118);
            this.grpModel.TabIndex = 1;
            this.grpModel.TabStop = false;
            this.grpModel.Text = "Model";
            // 
            // chkModel
            // 
            this.chkModel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chkModel.CheckOnClick = true;
            this.chkModel.FormattingEnabled = true;
            this.chkModel.Location = new System.Drawing.Point(6, 19);
            this.chkModel.Name = "chkModel";
            this.chkModel.Size = new System.Drawing.Size(166, 94);
            this.chkModel.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.dataToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(193, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // dataToolStripMenuItem
            // 
            this.dataToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.exportSchemaXMLToolStripMenuItem,
            this.createNewMetricToolStripMenuItem});
            this.dataToolStripMenuItem.Name = "dataToolStripMenuItem";
            this.dataToolStripMenuItem.Size = new System.Drawing.Size(43, 20);
            this.dataToolStripMenuItem.Text = "Data";
            // 
            // exportSchemaXMLToolStripMenuItem
            // 
            this.exportSchemaXMLToolStripMenuItem.Name = "exportSchemaXMLToolStripMenuItem";
            this.exportSchemaXMLToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.exportSchemaXMLToolStripMenuItem.Text = "Export Schema XML...";
            this.exportSchemaXMLToolStripMenuItem.Click += new System.EventHandler(this.exportSchemaXMLToolStripMenuItem_Click);
            // 
            // createNewMetricToolStripMenuItem
            // 
            this.createNewMetricToolStripMenuItem.Name = "createNewMetricToolStripMenuItem";
            this.createNewMetricToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.createNewMetricToolStripMenuItem.Text = "Create New Metric...";
            this.createNewMetricToolStripMenuItem.Click += new System.EventHandler(this.createNewMetricToolStripMenuItem_Click);
            // 
            // grdData
            // 
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colMetricID,
            this.colTitle,
            this.colDisplayNameShort,
            this.colModel,
            this.colSchema,
            this.colDataType,
            this.colIsActive,
            this.colXPath,
            this.colThreshold,
            this.colPrecision,
            this.colMinValue,
            this.colMaxValue,
            this.colUpdated});
            this.grdData.Location = new System.Drawing.Point(67, 155);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(240, 150);
            this.grdData.TabIndex = 0;
            this.grdData.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdData_CellDoubleClick);
            // 
            // colMetricID
            // 
            this.colMetricID.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colMetricID.DataPropertyName = "ID";
            this.colMetricID.Frozen = true;
            this.colMetricID.HeaderText = "ID";
            this.colMetricID.Name = "colMetricID";
            this.colMetricID.ReadOnly = true;
            this.colMetricID.Width = 43;
            // 
            // colTitle
            // 
            this.colTitle.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colTitle.DataPropertyName = "Name";
            this.colTitle.Frozen = true;
            this.colTitle.HeaderText = "Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            this.colTitle.Width = 52;
            // 
            // colDisplayNameShort
            // 
            this.colDisplayNameShort.DataPropertyName = "DisplayNameShort";
            this.colDisplayNameShort.HeaderText = "Short Name";
            this.colDisplayNameShort.Name = "colDisplayNameShort";
            this.colDisplayNameShort.ReadOnly = true;
            // 
            // colModel
            // 
            this.colModel.DataPropertyName = "ModelName";
            this.colModel.HeaderText = "Model";
            this.colModel.Name = "colModel";
            this.colModel.ReadOnly = true;
            // 
            // colSchema
            // 
            this.colSchema.DataPropertyName = "SchemaName";
            this.colSchema.HeaderText = "Schema";
            this.colSchema.Name = "colSchema";
            this.colSchema.ReadOnly = true;
            // 
            // colDataType
            // 
            this.colDataType.DataPropertyName = "DataTypeName";
            this.colDataType.HeaderText = "Data Type";
            this.colDataType.Name = "colDataType";
            this.colDataType.ReadOnly = true;
            // 
            // colIsActive
            // 
            this.colIsActive.DataPropertyName = "IsActive";
            this.colIsActive.HeaderText = "Active";
            this.colIsActive.Name = "colIsActive";
            this.colIsActive.ReadOnly = true;
            // 
            // colXPath
            // 
            this.colXPath.DataPropertyName = "XPath";
            this.colXPath.HeaderText = "XPath";
            this.colXPath.Name = "colXPath";
            this.colXPath.ReadOnly = true;
            // 
            // colThreshold
            // 
            this.colThreshold.DataPropertyName = "Threshold";
            this.colThreshold.HeaderText = "Threshold";
            this.colThreshold.Name = "colThreshold";
            this.colThreshold.ReadOnly = true;
            // 
            // colPrecision
            // 
            this.colPrecision.DataPropertyName = "Precision";
            this.colPrecision.HeaderText = "Precision";
            this.colPrecision.Name = "colPrecision";
            this.colPrecision.ReadOnly = true;
            // 
            // colMinValue
            // 
            this.colMinValue.DataPropertyName = "MinValue";
            this.colMinValue.HeaderText = "Minimum";
            this.colMinValue.Name = "colMinValue";
            this.colMinValue.ReadOnly = true;
            // 
            // colMaxValue
            // 
            this.colMaxValue.DataPropertyName = "MaxValue";
            this.colMaxValue.HeaderText = "Maximum";
            this.colMaxValue.Name = "colMaxValue";
            this.colMaxValue.ReadOnly = true;
            // 
            // colUpdated
            // 
            this.colUpdated.DataPropertyName = "UpdatedOn";
            this.colUpdated.HeaderText = "Updated";
            this.colUpdated.Name = "colUpdated";
            this.colUpdated.ReadOnly = true;
            // 
            // frmMetricDefinitions
            // 
            this.ClientSize = new System.Drawing.Size(788, 519);
            this.Controls.Add(this.splitContainer1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "frmMetricDefinitions";
            this.Text = "Metric Definitions";
            this.Load += new System.EventHandler(this.frmMetricDefinitions_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.grpTitle.ResumeLayout(false);
            this.grpTitle.PerformLayout();
            this.grpSchema.ResumeLayout(false);
            this.grpModel.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.CheckBox chkActive;
        private System.Windows.Forms.GroupBox grpTitle;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.GroupBox grpSchema;
        private System.Windows.Forms.CheckedListBox chkSchema;
        private System.Windows.Forms.GroupBox grpModel;
        private System.Windows.Forms.CheckedListBox chkModel;
        private System.Windows.Forms.CheckBox chkXPath;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem dataToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem exportSchemaXMLToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem createNewMetricToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMetricID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisplayNameShort;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchema;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
        private System.Windows.Forms.DataGridViewTextBoxColumn colXPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecision;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMinValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaxValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUpdated;
    }
}