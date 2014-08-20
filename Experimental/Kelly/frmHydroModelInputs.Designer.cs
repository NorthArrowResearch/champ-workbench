namespace CHaMPWorkbench.Experimental.Kelly
{
    partial class frmHydroModelInputs
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
            this.gbxInputParams = new System.Windows.Forms.GroupBox();
            this.gbxInputData = new System.Windows.Forms.GroupBox();
            this.cHAMP_VisitsDataGridView = new System.Windows.Forms.DataGridView();
            this.cHAMP_VisitsBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.rBTWorkbenchDataSet = new CHaMPWorkbench.RBTWorkbenchDataSet();
            this.optBatches = new System.Windows.Forms.RadioButton();
            this.optSelectVisits = new System.Windows.Forms.RadioButton();
            this.optAllVisits = new System.Windows.Forms.RadioButton();
            this.gbxOutput = new System.Windows.Forms.GroupBox();
            this.txtOutputWorkspace = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.cHAMP_VisitsTableAdapter = new CHaMPWorkbench.RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter();
            this.tableAdapterManager = new CHaMPWorkbench.RBTWorkbenchDataSetTableAdapters.TableAdapterManager();
            this.Run = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn8 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewCheckBoxColumn1 = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.dataGridViewTextBoxColumn1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.dataGridViewTextBoxColumn19 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.chkSaveNewBatch = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.gbxInputParams.SuspendLayout();
            this.gbxInputData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cHAMP_VisitsDataGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.cHAMP_VisitsBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rBTWorkbenchDataSet)).BeginInit();
            this.gbxOutput.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbxInputParams
            // 
            this.gbxInputParams.Controls.Add(this.label2);
            this.gbxInputParams.Location = new System.Drawing.Point(12, 12);
            this.gbxInputParams.Name = "gbxInputParams";
            this.gbxInputParams.Size = new System.Drawing.Size(886, 118);
            this.gbxInputParams.TabIndex = 0;
            this.gbxInputParams.TabStop = false;
            this.gbxInputParams.Text = "Input Parameters";
            // 
            // gbxInputData
            // 
            this.gbxInputData.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxInputData.Controls.Add(this.textBox1);
            this.gbxInputData.Controls.Add(this.chkSaveNewBatch);
            this.gbxInputData.Controls.Add(this.cHAMP_VisitsDataGridView);
            this.gbxInputData.Controls.Add(this.optBatches);
            this.gbxInputData.Controls.Add(this.optSelectVisits);
            this.gbxInputData.Controls.Add(this.optAllVisits);
            this.gbxInputData.Location = new System.Drawing.Point(12, 136);
            this.gbxInputData.Name = "gbxInputData";
            this.gbxInputData.Size = new System.Drawing.Size(886, 448);
            this.gbxInputData.TabIndex = 1;
            this.gbxInputData.TabStop = false;
            this.gbxInputData.Text = "Input Datasets";
            // 
            // cHAMP_VisitsDataGridView
            // 
            this.cHAMP_VisitsDataGridView.AllowUserToAddRows = false;
            this.cHAMP_VisitsDataGridView.AllowUserToDeleteRows = false;
            this.cHAMP_VisitsDataGridView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.cHAMP_VisitsDataGridView.AutoGenerateColumns = false;
            this.cHAMP_VisitsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.cHAMP_VisitsDataGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Run,
            this.dataGridViewTextBoxColumn4,
            this.dataGridViewTextBoxColumn2,
            this.dataGridViewTextBoxColumn8,
            this.dataGridViewTextBoxColumn5,
            this.dataGridViewTextBoxColumn6,
            this.dataGridViewTextBoxColumn7,
            this.dataGridViewCheckBoxColumn1,
            this.dataGridViewTextBoxColumn1,
            this.dataGridViewTextBoxColumn19});
            this.cHAMP_VisitsDataGridView.DataSource = this.cHAMP_VisitsBindingSource;
            this.cHAMP_VisitsDataGridView.Location = new System.Drawing.Point(11, 86);
            this.cHAMP_VisitsDataGridView.Name = "cHAMP_VisitsDataGridView";
            this.cHAMP_VisitsDataGridView.RowTemplate.Height = 28;
            this.cHAMP_VisitsDataGridView.Size = new System.Drawing.Size(868, 277);
            this.cHAMP_VisitsDataGridView.TabIndex = 3;
            // 
            // cHAMP_VisitsBindingSource
            // 
            this.cHAMP_VisitsBindingSource.DataMember = "CHAMP_Visits";
            this.cHAMP_VisitsBindingSource.DataSource = this.rBTWorkbenchDataSet;
            // 
            // rBTWorkbenchDataSet
            // 
            this.rBTWorkbenchDataSet.DataSetName = "RBTWorkbenchDataSet";
            this.rBTWorkbenchDataSet.EnforceConstraints = false;
            this.rBTWorkbenchDataSet.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // optBatches
            // 
            this.optBatches.AutoSize = true;
            this.optBatches.Enabled = false;
            this.optBatches.Location = new System.Drawing.Point(6, 418);
            this.optBatches.Name = "optBatches";
            this.optBatches.Size = new System.Drawing.Size(142, 24);
            this.optBatches.TabIndex = 1;
            this.optBatches.TabStop = true;
            this.optBatches.Text = "Saved Batches";
            this.optBatches.UseVisualStyleBackColor = true;
            // 
            // optSelectVisits
            // 
            this.optSelectVisits.AutoSize = true;
            this.optSelectVisits.Location = new System.Drawing.Point(6, 56);
            this.optSelectVisits.Name = "optSelectVisits";
            this.optSelectVisits.Size = new System.Drawing.Size(131, 24);
            this.optSelectVisits.TabIndex = 2;
            this.optSelectVisits.TabStop = true;
            this.optSelectVisits.Text = "Select Visit(s)";
            this.optSelectVisits.UseVisualStyleBackColor = true;
            // 
            // optAllVisits
            // 
            this.optAllVisits.AutoSize = true;
            this.optAllVisits.Location = new System.Drawing.Point(7, 26);
            this.optAllVisits.Name = "optAllVisits";
            this.optAllVisits.Size = new System.Drawing.Size(93, 24);
            this.optAllVisits.TabIndex = 0;
            this.optAllVisits.TabStop = true;
            this.optAllVisits.Text = "All Visits";
            this.optAllVisits.UseVisualStyleBackColor = true;
            // 
            // gbxOutput
            // 
            this.gbxOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.gbxOutput.Controls.Add(this.txtOutputWorkspace);
            this.gbxOutput.Controls.Add(this.label1);
            this.gbxOutput.Location = new System.Drawing.Point(12, 590);
            this.gbxOutput.Name = "gbxOutput";
            this.gbxOutput.Size = new System.Drawing.Size(886, 86);
            this.gbxOutput.TabIndex = 2;
            this.gbxOutput.TabStop = false;
            this.gbxOutput.Text = "Outputs";
            // 
            // txtOutputWorkspace
            // 
            this.txtOutputWorkspace.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputWorkspace.Location = new System.Drawing.Point(102, 38);
            this.txtOutputWorkspace.Name = "txtOutputWorkspace";
            this.txtOutputWorkspace.Size = new System.Drawing.Size(777, 26);
            this.txtOutputWorkspace.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Workspace";
            // 
            // button1
            // 
            this.button1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button1.Location = new System.Drawing.Point(831, 689);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 3;
            this.button1.Text = "Cancel";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.button2.Location = new System.Drawing.Point(750, 689);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 4;
            this.button2.Text = "OK";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // cHAMP_VisitsTableAdapter
            // 
            this.cHAMP_VisitsTableAdapter.ClearBeforeFill = true;
            // 
            // tableAdapterManager
            // 
            this.tableAdapterManager.BackupDataSetBeforeUpdate = false;
            this.tableAdapterManager.CHAMP_ChannelUnitsTableAdapter = null;
            this.tableAdapterManager.CHaMP_SegmentsTableAdapter = null;
            this.tableAdapterManager.CHAMP_SitesTableAdapter = null;
            this.tableAdapterManager.CHAMP_VisitsTableAdapter = this.cHAMP_VisitsTableAdapter;
            this.tableAdapterManager.CHAMP_WatershedsTableAdapter = null;
            this.tableAdapterManager.UpdateOrder = CHaMPWorkbench.RBTWorkbenchDataSetTableAdapters.TableAdapterManager.UpdateOrderOption.InsertUpdateDelete;
            // 
            // Run
            // 
            this.Run.Frozen = true;
            this.Run.HeaderText = "Run";
            this.Run.Name = "Run";
            this.Run.Width = 25;
            // 
            // dataGridViewTextBoxColumn4
            // 
            this.dataGridViewTextBoxColumn4.DataPropertyName = "VisitYear";
            this.dataGridViewTextBoxColumn4.HeaderText = "VisitYear";
            this.dataGridViewTextBoxColumn4.Name = "dataGridViewTextBoxColumn4";
            this.dataGridViewTextBoxColumn4.ReadOnly = true;
            this.dataGridViewTextBoxColumn4.Width = 75;
            // 
            // dataGridViewTextBoxColumn2
            // 
            this.dataGridViewTextBoxColumn2.DataPropertyName = "SiteID";
            this.dataGridViewTextBoxColumn2.HeaderText = "SiteID";
            this.dataGridViewTextBoxColumn2.Name = "dataGridViewTextBoxColumn2";
            this.dataGridViewTextBoxColumn2.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn8
            // 
            this.dataGridViewTextBoxColumn8.DataPropertyName = "SampleDate";
            this.dataGridViewTextBoxColumn8.HeaderText = "SampleDate";
            this.dataGridViewTextBoxColumn8.Name = "dataGridViewTextBoxColumn8";
            this.dataGridViewTextBoxColumn8.ReadOnly = true;
            this.dataGridViewTextBoxColumn8.Width = 75;
            // 
            // dataGridViewTextBoxColumn5
            // 
            this.dataGridViewTextBoxColumn5.DataPropertyName = "HitchID";
            this.dataGridViewTextBoxColumn5.HeaderText = "HitchID";
            this.dataGridViewTextBoxColumn5.Name = "dataGridViewTextBoxColumn5";
            this.dataGridViewTextBoxColumn5.ReadOnly = true;
            this.dataGridViewTextBoxColumn5.Width = 50;
            // 
            // dataGridViewTextBoxColumn6
            // 
            this.dataGridViewTextBoxColumn6.DataPropertyName = "HitchName";
            this.dataGridViewTextBoxColumn6.HeaderText = "HitchName";
            this.dataGridViewTextBoxColumn6.Name = "dataGridViewTextBoxColumn6";
            this.dataGridViewTextBoxColumn6.ReadOnly = true;
            // 
            // dataGridViewTextBoxColumn7
            // 
            this.dataGridViewTextBoxColumn7.DataPropertyName = "CrewName";
            this.dataGridViewTextBoxColumn7.HeaderText = "CrewName";
            this.dataGridViewTextBoxColumn7.Name = "dataGridViewTextBoxColumn7";
            this.dataGridViewTextBoxColumn7.ReadOnly = true;
            // 
            // dataGridViewCheckBoxColumn1
            // 
            this.dataGridViewCheckBoxColumn1.DataPropertyName = "IsPrimary";
            this.dataGridViewCheckBoxColumn1.HeaderText = "IsPrimary";
            this.dataGridViewCheckBoxColumn1.Name = "dataGridViewCheckBoxColumn1";
            this.dataGridViewCheckBoxColumn1.ReadOnly = true;
            this.dataGridViewCheckBoxColumn1.Width = 25;
            // 
            // dataGridViewTextBoxColumn1
            // 
            this.dataGridViewTextBoxColumn1.DataPropertyName = "VisitID";
            this.dataGridViewTextBoxColumn1.HeaderText = "VisitID";
            this.dataGridViewTextBoxColumn1.Name = "dataGridViewTextBoxColumn1";
            this.dataGridViewTextBoxColumn1.ReadOnly = true;
            this.dataGridViewTextBoxColumn1.Width = 75;
            // 
            // dataGridViewTextBoxColumn19
            // 
            this.dataGridViewTextBoxColumn19.DataPropertyName = "DisplayName";
            this.dataGridViewTextBoxColumn19.HeaderText = "DisplayName";
            this.dataGridViewTextBoxColumn19.Name = "dataGridViewTextBoxColumn19";
            this.dataGridViewTextBoxColumn19.ReadOnly = true;
            // 
            // chkSaveNewBatch
            // 
            this.chkSaveNewBatch.AutoSize = true;
            this.chkSaveNewBatch.Enabled = false;
            this.chkSaveNewBatch.Location = new System.Drawing.Point(11, 370);
            this.chkSaveNewBatch.Name = "chkSaveNewBatch";
            this.chkSaveNewBatch.Size = new System.Drawing.Size(177, 24);
            this.chkSaveNewBatch.TabIndex = 4;
            this.chkSaveNewBatch.Text = "Save as New Batch:";
            this.chkSaveNewBatch.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(194, 370);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(338, 26);
            this.textBox1.TabIndex = 5;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(325, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(193, 20);
            this.label2.TabIndex = 0;
            this.label2.Text = "No Parameters to Change";
            // 
            // frmHydroModelInputs
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(910, 724);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.gbxOutput);
            this.Controls.Add(this.gbxInputData);
            this.Controls.Add(this.gbxInputParams);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(781, 780);
            this.Name = "frmHydroModelInputs";
            this.ShowIcon = false;
            this.Text = "Hydro Model Input Generator";
            this.Load += new System.EventHandler(this.frmHydroModelInputs_Load);
            this.gbxInputParams.ResumeLayout(false);
            this.gbxInputParams.PerformLayout();
            this.gbxInputData.ResumeLayout(false);
            this.gbxInputData.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.cHAMP_VisitsDataGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.cHAMP_VisitsBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rBTWorkbenchDataSet)).EndInit();
            this.gbxOutput.ResumeLayout(false);
            this.gbxOutput.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbxInputParams;
        private System.Windows.Forms.GroupBox gbxInputData;
        private System.Windows.Forms.RadioButton optSelectVisits;
        private System.Windows.Forms.RadioButton optBatches;
        private System.Windows.Forms.RadioButton optAllVisits;
        private System.Windows.Forms.GroupBox gbxOutput;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox txtOutputWorkspace;
        private System.Windows.Forms.Label label1;
        private RBTWorkbenchDataSet rBTWorkbenchDataSet;
        private System.Windows.Forms.BindingSource cHAMP_VisitsBindingSource;
        private RBTWorkbenchDataSetTableAdapters.CHAMP_VisitsTableAdapter cHAMP_VisitsTableAdapter;
        private RBTWorkbenchDataSetTableAdapters.TableAdapterManager tableAdapterManager;
        private System.Windows.Forms.DataGridView cHAMP_VisitsDataGridView;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Run;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn4;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn8;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn5;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn6;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn7;
        private System.Windows.Forms.DataGridViewCheckBoxColumn dataGridViewCheckBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn1;
        private System.Windows.Forms.DataGridViewTextBoxColumn dataGridViewTextBoxColumn19;
        private System.Windows.Forms.CheckBox chkSaveNewBatch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
    }
}