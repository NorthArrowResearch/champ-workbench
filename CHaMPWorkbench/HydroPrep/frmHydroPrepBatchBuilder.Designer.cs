namespace CHaMPWorkbench.HydroPrep
{
    partial class frmHydroPrepBatchBuilder
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHydroPrepBatchBuilder));
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.lstVisits = new System.Windows.Forms.ListBox();
            this.txtInputFile = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.txtBatch = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblBatchName = new System.Windows.Forms.Label();
            this.txtOutputFolder = new System.Windows.Forms.TextBox();
            this.chkClearOtherBatches = new System.Windows.Forms.CheckBox();
            this.cmdBrowseInputOutput = new System.Windows.Forms.Button();
            this.cmdBrowseMonitoring = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtMonitoringDataFolder = new System.Windows.Forms.TextBox();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.txtTemp = new System.Windows.Forms.TextBox();
            this.cmdBrowseTemp = new System.Windows.Forms.Button();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.lstVisits);
            this.groupBox4.Location = new System.Drawing.Point(7, 156);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(587, 222);
            this.groupBox4.TabIndex = 14;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Visits";
            // 
            // lstVisits
            // 
            this.lstVisits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lstVisits.FormattingEnabled = true;
            this.lstVisits.Location = new System.Drawing.Point(8, 19);
            this.lstVisits.Name = "lstVisits";
            this.lstVisits.Size = new System.Drawing.Size(573, 186);
            this.lstVisits.TabIndex = 0;
            // 
            // txtInputFile
            // 
            this.txtInputFile.Location = new System.Drawing.Point(187, 130);
            this.txtInputFile.MaxLength = 255;
            this.txtInputFile.Name = "txtInputFile";
            this.txtInputFile.Size = new System.Drawing.Size(192, 20);
            this.txtInputFile.TabIndex = 13;
            this.txtInputFile.Text = "hydro_prep_input.xml";
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(106, 134);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(76, 13);
            this.label14.TabIndex = 12;
            this.label14.Text = "Input file name";
            // 
            // txtBatch
            // 
            this.txtBatch.Location = new System.Drawing.Point(187, 12);
            this.txtBatch.MaxLength = 255;
            this.txtBatch.Name = "txtBatch";
            this.txtBatch.Size = new System.Drawing.Size(192, 20);
            this.txtBatch.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(164, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Output folder (\"InputOutputFiles\")";
            // 
            // lblBatchName
            // 
            this.lblBatchName.AutoSize = true;
            this.lblBatchName.Location = new System.Drawing.Point(118, 16);
            this.lblBatchName.Name = "lblBatchName";
            this.lblBatchName.Size = new System.Drawing.Size(64, 13);
            this.lblBatchName.TabIndex = 0;
            this.lblBatchName.Text = "Batch name";
            // 
            // txtOutputFolder
            // 
            this.txtOutputFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputFolder.Location = new System.Drawing.Point(187, 75);
            this.txtOutputFolder.Name = "txtOutputFolder";
            this.txtOutputFolder.Size = new System.Drawing.Size(326, 20);
            this.txtOutputFolder.TabIndex = 7;
            this.txtOutputFolder.Text = "C:\\CHaMP\\RBTInputOutputFiles";
            // 
            // chkClearOtherBatches
            // 
            this.chkClearOtherBatches.AutoSize = true;
            this.chkClearOtherBatches.Location = new System.Drawing.Point(386, 14);
            this.chkClearOtherBatches.Name = "chkClearOtherBatches";
            this.chkClearOtherBatches.Size = new System.Drawing.Size(214, 17);
            this.chkClearOtherBatches.TabIndex = 2;
            this.chkClearOtherBatches.Text = "Set this as the only batch queued to run";
            this.chkClearOtherBatches.UseVisualStyleBackColor = true;
            // 
            // cmdBrowseInputOutput
            // 
            this.cmdBrowseInputOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseInputOutput.Location = new System.Drawing.Point(519, 74);
            this.cmdBrowseInputOutput.Name = "cmdBrowseInputOutput";
            this.cmdBrowseInputOutput.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseInputOutput.TabIndex = 8;
            this.cmdBrowseInputOutput.Text = "Browse";
            this.cmdBrowseInputOutput.UseVisualStyleBackColor = true;
            this.cmdBrowseInputOutput.Click += new System.EventHandler(this.cmdBrowseInputOutput_Click);
            // 
            // cmdBrowseMonitoring
            // 
            this.cmdBrowseMonitoring.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseMonitoring.Location = new System.Drawing.Point(519, 41);
            this.cmdBrowseMonitoring.Name = "cmdBrowseMonitoring";
            this.cmdBrowseMonitoring.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseMonitoring.TabIndex = 5;
            this.cmdBrowseMonitoring.Text = "Browse";
            this.cmdBrowseMonitoring.UseVisualStyleBackColor = true;
            this.cmdBrowseMonitoring.Click += new System.EventHandler(this.cmdBrowseMonitoring_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(47, 46);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(135, 13);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "Parent folder (\"Monitoring\")";
            // 
            // txtMonitoringDataFolder
            // 
            this.txtMonitoringDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMonitoringDataFolder.Location = new System.Drawing.Point(187, 42);
            this.txtMonitoringDataFolder.Name = "txtMonitoringDataFolder";
            this.txtMonitoringDataFolder.Size = new System.Drawing.Size(326, 20);
            this.txtMonitoringDataFolder.TabIndex = 4;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.CausesValidation = false;
            this.cmdHelp.Location = new System.Drawing.Point(12, 384);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 17;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(438, 384);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 15;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(519, 384);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 16;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(119, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "Temp folder";
            // 
            // txtTemp
            // 
            this.txtTemp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTemp.Location = new System.Drawing.Point(187, 104);
            this.txtTemp.Name = "txtTemp";
            this.txtTemp.Size = new System.Drawing.Size(326, 20);
            this.txtTemp.TabIndex = 10;
            // 
            // cmdBrowseTemp
            // 
            this.cmdBrowseTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseTemp.Location = new System.Drawing.Point(519, 103);
            this.cmdBrowseTemp.Name = "cmdBrowseTemp";
            this.cmdBrowseTemp.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseTemp.TabIndex = 11;
            this.cmdBrowseTemp.Text = "Browse";
            this.cmdBrowseTemp.UseVisualStyleBackColor = true;
            this.cmdBrowseTemp.Click += new System.EventHandler(this.cmdBrowseTemp_Click);
            // 
            // frmHydroPrepBatchBuilder
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(606, 419);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtTemp);
            this.Controls.Add(this.cmdBrowseTemp);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.txtInputFile);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.txtBatch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblBatchName);
            this.Controls.Add(this.txtOutputFolder);
            this.Controls.Add(this.chkClearOtherBatches);
            this.Controls.Add(this.cmdBrowseInputOutput);
            this.Controls.Add(this.cmdBrowseMonitoring);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtMonitoringDataFolder);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(622, 439);
            this.Name = "frmHydroPrepBatchBuilder";
            this.Text = "Hydro Prep Batch Builder";
            this.Load += new System.EventHandler(this.frmHydroPrepBatchBuilder_Load);
            this.groupBox4.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.ListBox lstVisits;
        internal System.Windows.Forms.TextBox txtInputFile;
        internal System.Windows.Forms.Label label14;
        internal System.Windows.Forms.TextBox txtBatch;
        internal System.Windows.Forms.Label label1;
        internal System.Windows.Forms.Label lblBatchName;
        internal System.Windows.Forms.TextBox txtOutputFolder;
        private System.Windows.Forms.CheckBox chkClearOtherBatches;
        internal System.Windows.Forms.Button cmdBrowseInputOutput;
        internal System.Windows.Forms.Button cmdBrowseMonitoring;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtMonitoringDataFolder;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.Label label3;
        internal System.Windows.Forms.TextBox txtTemp;
        internal System.Windows.Forms.Button cmdBrowseTemp;
    }
}