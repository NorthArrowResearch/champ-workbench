namespace CHaMPWorkbench.Data
{
    partial class frmClearDatabase
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
            this.label1 = new System.Windows.Forms.Label();
            this.chkRBTBatches = new System.Windows.Forms.CheckBox();
            this.chkRBTLogs = new System.Windows.Forms.CheckBox();
            this.chkRBTMetrics = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkManulMetrics = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkWatersheds = new System.Windows.Forms.CheckBox();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(342, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Select which type of data that you want to DELETE from the database:";
            // 
            // chkRBTBatches
            // 
            this.chkRBTBatches.AutoSize = true;
            this.chkRBTBatches.Location = new System.Drawing.Point(11, 20);
            this.chkRBTBatches.Name = "chkRBTBatches";
            this.chkRBTBatches.Size = new System.Drawing.Size(291, 17);
            this.chkRBTBatches.TabIndex = 1;
            this.chkRBTBatches.Text = "RBT batches and batch runs (i.e. input XML file records)";
            this.chkRBTBatches.UseVisualStyleBackColor = true;
            // 
            // chkRBTLogs
            // 
            this.chkRBTLogs.AutoSize = true;
            this.chkRBTLogs.Location = new System.Drawing.Point(11, 44);
            this.chkRBTLogs.Name = "chkRBTLogs";
            this.chkRBTLogs.Size = new System.Drawing.Size(86, 17);
            this.chkRBTLogs.TabIndex = 2;
            this.chkRBTLogs.Text = "RBT log files";
            this.chkRBTLogs.UseVisualStyleBackColor = true;
            // 
            // chkRBTMetrics
            // 
            this.chkRBTMetrics.AutoSize = true;
            this.chkRBTMetrics.Location = new System.Drawing.Point(11, 68);
            this.chkRBTMetrics.Name = "chkRBTMetrics";
            this.chkRBTMetrics.Size = new System.Drawing.Size(112, 17);
            this.chkRBTMetrics.TabIndex = 3;
            this.chkRBTMetrics.Text = "RBT metric results";
            this.chkRBTMetrics.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkManulMetrics);
            this.groupBox1.Controls.Add(this.chkRBTBatches);
            this.groupBox1.Controls.Add(this.chkRBTMetrics);
            this.groupBox1.Controls.Add(this.chkRBTLogs);
            this.groupBox1.Location = new System.Drawing.Point(12, 38);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(369, 128);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "RBT Related";
            // 
            // chkManulMetrics
            // 
            this.chkManulMetrics.AutoSize = true;
            this.chkManulMetrics.Location = new System.Drawing.Point(11, 102);
            this.chkManulMetrics.Name = "chkManulMetrics";
            this.chkManulMetrics.Size = new System.Drawing.Size(176, 17);
            this.chkManulMetrics.TabIndex = 4;
            this.chkManulMetrics.Text = "Manual, validation metric results";
            this.chkManulMetrics.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkWatersheds);
            this.groupBox2.Location = new System.Drawing.Point(12, 172);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(369, 51);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "CHaMP";
            // 
            // chkWatersheds
            // 
            this.chkWatersheds.AutoSize = true;
            this.chkWatersheds.Location = new System.Drawing.Point(12, 21);
            this.chkWatersheds.Name = "chkWatersheds";
            this.chkWatersheds.Size = new System.Drawing.Size(302, 17);
            this.chkWatersheds.TabIndex = 0;
            this.chkWatersheds.Text = "Watershed, site, visit, segment and channel unit definitions";
            this.chkWatersheds.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(306, 243);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 6;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(225, 243);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 7;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // frmClearDatabase
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(398, 278);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmClearDatabase";
            this.Text = "Manage Workbench Database Contents";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkRBTBatches;
        private System.Windows.Forms.CheckBox chkRBTLogs;
        private System.Windows.Forms.CheckBox chkRBTMetrics;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox chkWatersheds;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.CheckBox chkManulMetrics;
    }
}