namespace CHaMPWorkbench.Data.Metrics
{
    partial class ucBatchPicker
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
            this.label1 = new System.Windows.Forms.Label();
            this.cboProgram = new System.Windows.Forms.ComboBox();
            this.grpSource = new System.Windows.Forms.GroupBox();
            this.grdInfo = new System.Windows.Forms.DataGridView();
            this.colCopy = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.colProgram = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colScavengeType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSchema = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colVisits = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colInstances = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.grpSource.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(1, 3);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 9;
            this.label1.Text = "Program";
            // 
            // cboProgram
            // 
            this.cboProgram.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProgram.FormattingEnabled = true;
            this.cboProgram.Location = new System.Drawing.Point(72, 3);
            this.cboProgram.Name = "cboProgram";
            this.cboProgram.Size = new System.Drawing.Size(298, 21);
            this.cboProgram.TabIndex = 8;
            this.cboProgram.SelectedIndexChanged += new System.EventHandler(this.cboProgram_SelectedIndexChanged);
            // 
            // grpSource
            // 
            this.grpSource.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpSource.Controls.Add(this.grdInfo);
            this.grpSource.Location = new System.Drawing.Point(1, 32);
            this.grpSource.Name = "grpSource";
            this.grpSource.Size = new System.Drawing.Size(666, 257);
            this.grpSource.TabIndex = 7;
            this.grpSource.TabStop = false;
            this.grpSource.Text = "Source Metrics";
            // 
            // grdInfo
            // 
            this.grdInfo.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdInfo.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdInfo.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colCopy,
            this.colProgram,
            this.colScavengeType,
            this.colSchema,
            this.colVisits,
            this.colInstances});
            this.grdInfo.Location = new System.Drawing.Point(6, 19);
            this.grdInfo.Name = "grdInfo";
            this.grdInfo.Size = new System.Drawing.Size(654, 232);
            this.grdInfo.TabIndex = 3;
            // 
            // colCopy
            // 
            this.colCopy.DataPropertyName = "Copy";
            this.colCopy.HeaderText = "";
            this.colCopy.Name = "colCopy";
            this.colCopy.Width = 40;
            // 
            // colProgram
            // 
            this.colProgram.DataPropertyName = "Program";
            this.colProgram.HeaderText = "Program";
            this.colProgram.Name = "colProgram";
            this.colProgram.ReadOnly = true;
            // 
            // colScavengeType
            // 
            this.colScavengeType.DataPropertyName = "ScavengeType";
            this.colScavengeType.HeaderText = "Scavenge Type";
            this.colScavengeType.Name = "colScavengeType";
            this.colScavengeType.ReadOnly = true;
            this.colScavengeType.Width = 200;
            // 
            // colSchema
            // 
            this.colSchema.DataPropertyName = "Schema";
            this.colSchema.HeaderText = "Schema";
            this.colSchema.Name = "colSchema";
            this.colSchema.ReadOnly = true;
            this.colSchema.Width = 200;
            // 
            // colVisits
            // 
            this.colVisits.DataPropertyName = "Visits";
            this.colVisits.HeaderText = "Visits";
            this.colVisits.Name = "colVisits";
            this.colVisits.ReadOnly = true;
            this.colVisits.Width = 60;
            // 
            // colInstances
            // 
            this.colInstances.DataPropertyName = "Instances";
            this.colInstances.HeaderText = "Instances";
            this.colInstances.Name = "colInstances";
            this.colInstances.ReadOnly = true;
            this.colInstances.Visible = false;
            this.colInstances.Width = 60;
            // 
            // ucBatchPicker
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cboProgram);
            this.Controls.Add(this.grpSource);
            this.Name = "ucBatchPicker";
            this.Size = new System.Drawing.Size(667, 289);
            this.Load += new System.EventHandler(this.ucBatchPicker_Load);
            this.grpSource.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdInfo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboProgram;
        private System.Windows.Forms.GroupBox grpSource;
        private System.Windows.Forms.DataGridView grdInfo;
        private System.Windows.Forms.DataGridViewCheckBoxColumn colCopy;
        private System.Windows.Forms.DataGridViewTextBoxColumn colProgram;
        private System.Windows.Forms.DataGridViewTextBoxColumn colScavengeType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSchema;
        private System.Windows.Forms.DataGridViewTextBoxColumn colVisits;
        private System.Windows.Forms.DataGridViewTextBoxColumn colInstances;
    }
}
