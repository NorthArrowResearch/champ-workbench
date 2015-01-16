namespace CHaMPWorkbench
{
    partial class frmScavengeVisitTopoInfo
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
            this.cmdOK = new System.Windows.Forms.Button();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtMonitoringDataFolder = new System.Windows.Forms.TextBox();
            this.cmdBrowseFolder = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.chkSetNull = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(577, 390);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 0;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.button1_Click);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(11, 133);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(135, 13);
            this.Label2.TabIndex = 5;
            this.Label2.Text = "Parent folder (\"Monitoring\")";
            // 
            // txtMonitoringDataFolder
            // 
            this.txtMonitoringDataFolder.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMonitoringDataFolder.Location = new System.Drawing.Point(155, 129);
            this.txtMonitoringDataFolder.Name = "txtMonitoringDataFolder";
            this.txtMonitoringDataFolder.Size = new System.Drawing.Size(497, 20);
            this.txtMonitoringDataFolder.TabIndex = 6;
            this.txtMonitoringDataFolder.Text = "E:\\Local Cloud\\Shared\\CHaMP\\MonitoringData";
            // 
            // cmdBrowseFolder
            // 
            this.cmdBrowseFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseFolder.Location = new System.Drawing.Point(658, 128);
            this.cmdBrowseFolder.Name = "cmdBrowseFolder";
            this.cmdBrowseFolder.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseFolder.TabIndex = 7;
            this.cmdBrowseFolder.Text = "Browse";
            this.cmdBrowseFolder.UseVisualStyleBackColor = true;
            this.cmdBrowseFolder.Click += new System.EventHandler(this.cmdBrowseFolder_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(658, 390);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 8;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // chkSetNull
            // 
            this.chkSetNull.AutoSize = true;
            this.chkSetNull.Location = new System.Drawing.Point(155, 164);
            this.chkSetNull.Name = "chkSetNull";
            this.chkSetNull.Size = new System.Drawing.Size(356, 17);
            this.chkSetNull.TabIndex = 9;
            this.chkSetNull.Text = "Set topo data fields to NULL for visits where the data cannot be found";
            this.chkSetNull.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 10;
            this.label1.Text = "label1";
            // 
            // frmScavengeVisitTopoInfo
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(745, 425);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chkSetNull);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.Label2);
            this.Controls.Add(this.txtMonitoringDataFolder);
            this.Controls.Add(this.cmdBrowseFolder);
            this.Controls.Add(this.cmdOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmScavengeVisitTopoInfo";
            this.Text = "Update Topo and Hydro Paths In Workbench Database";
            this.Load += new System.EventHandler(this.frmScavengeVisitInfo_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtMonitoringDataFolder;
        internal System.Windows.Forms.Button cmdBrowseFolder;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.CheckBox chkSetNull;
        private System.Windows.Forms.Label label1;
    }
}