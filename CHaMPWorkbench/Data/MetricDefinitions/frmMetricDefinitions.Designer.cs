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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grdData = new System.Windows.Forms.DataGridView();
            this.colMetricID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDisplayNameShort = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colModel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSchema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDataType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colThreshold = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colPrecision = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMinValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMaxValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colXPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colIsActive = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.grdData);
            this.splitContainer1.Size = new System.Drawing.Size(535, 423);
            this.splitContainer1.SplitterDistance = 178;
            this.splitContainer1.TabIndex = 0;
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
            this.colThreshold,
            this.colPrecision,
            this.colMinValue,
            this.colMaxValue,
            this.colXPath,
            this.colIsActive});
            this.grdData.Location = new System.Drawing.Point(67, 155);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(240, 150);
            this.grdData.TabIndex = 0;
            // 
            // colMetricID
            // 
            this.colMetricID.DataPropertyName = "ID";
            this.colMetricID.HeaderText = "MetricID";
            this.colMetricID.Name = "colMetricID";
            this.colMetricID.ReadOnly = true;
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
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
            // colXPath
            // 
            this.colXPath.DataPropertyName = "XPath";
            this.colXPath.HeaderText = "XPath";
            this.colXPath.Name = "colXPath";
            this.colXPath.ReadOnly = true;
            // 
            // colIsActive
            // 
            this.colIsActive.DataPropertyName = "IsActive";
            this.colIsActive.HeaderText = "Active";
            this.colIsActive.Name = "colIsActive";
            this.colIsActive.ReadOnly = true;
            // 
            // frmMetricDefinitions
            // 
            this.ClientSize = new System.Drawing.Size(535, 423);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmMetricDefinitions";
            this.Load += new System.EventHandler(this.frmMetricDefinitions_Load);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMetricID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDisplayNameShort;
        private System.Windows.Forms.DataGridViewTextBoxColumn colModel;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchema;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDataType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colThreshold;
        private System.Windows.Forms.DataGridViewTextBoxColumn colPrecision;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMinValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMaxValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colXPath;
        private System.Windows.Forms.DataGridViewTextBoxColumn colIsActive;
    }
}