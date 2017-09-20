namespace CHaMPWorkbench.Data.Metrics.Upload
{
    partial class frmMetricUpload
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
            this.cboCancel = new System.Windows.Forms.Button();
            this.cboOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.pgrProgress = new System.Windows.Forms.ProgressBar();
            this.txtMessages = new System.Windows.Forms.TextBox();
            this.ucBatch = new CHaMPWorkbench.Data.Metrics.ucBatchPicker();
            this.SuspendLayout();
            // 
            // cboCancel
            // 
            this.cboCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cboCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cboCancel.Location = new System.Drawing.Point(779, 446);
            this.cboCancel.Name = "cboCancel";
            this.cboCancel.Size = new System.Drawing.Size(75, 23);
            this.cboCancel.TabIndex = 0;
            this.cboCancel.Text = "Cancel";
            this.cboCancel.UseVisualStyleBackColor = true;
            // 
            // cboOK
            // 
            this.cboOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cboOK.Location = new System.Drawing.Point(698, 446);
            this.cboOK.Name = "cboOK";
            this.cboOK.Size = new System.Drawing.Size(75, 23);
            this.cboOK.TabIndex = 1;
            this.cboOK.Text = "Run";
            this.cboOK.UseVisualStyleBackColor = true;
            this.cboOK.Click += new System.EventHandler(this.cboOK_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 446);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 2;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // pgrProgress
            // 
            this.pgrProgress.Location = new System.Drawing.Point(12, 209);
            this.pgrProgress.Name = "pgrProgress";
            this.pgrProgress.Size = new System.Drawing.Size(842, 13);
            this.pgrProgress.TabIndex = 7;
            // 
            // txtMessages
            // 
            this.txtMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMessages.Location = new System.Drawing.Point(12, 228);
            this.txtMessages.Multiline = true;
            this.txtMessages.Name = "txtMessages";
            this.txtMessages.Size = new System.Drawing.Size(842, 209);
            this.txtMessages.TabIndex = 8;
            // 
            // ucBatch
            // 
            this.ucBatch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ucBatch.Location = new System.Drawing.Point(12, 12);
            this.ucBatch.Name = "ucBatch";
            this.ucBatch.Size = new System.Drawing.Size(842, 189);
            this.ucBatch.TabIndex = 9;
            // 
            // frmMetricUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(866, 481);
            this.Controls.Add(this.ucBatch);
            this.Controls.Add(this.txtMessages);
            this.Controls.Add(this.pgrProgress);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cboOK);
            this.Controls.Add(this.cboCancel);
            this.Name = "frmMetricUpload";
            this.Text = "Metric Upload";
            this.Load += new System.EventHandler(this.frmMetricUpload_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cboCancel;
        private System.Windows.Forms.Button cboOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.ProgressBar pgrProgress;
        private System.Windows.Forms.TextBox txtMessages;
        private ucBatchPicker ucBatch;
    }
}