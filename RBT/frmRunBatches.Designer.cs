namespace CHaMPWorkbench.RBT
{
    partial class frmRunBatches
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.lblMessage = new System.Windows.Forms.Label();
            this.chkScavengeResults = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.chkScavengeLog = new System.Windows.Forms.CheckBox();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(487, 134);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(406, 134);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 23);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // lblMessage
            // 
            this.lblMessage.Location = new System.Drawing.Point(12, 9);
            this.lblMessage.Name = "lblMessage";
            this.lblMessage.Size = new System.Drawing.Size(550, 50);
            this.lblMessage.TabIndex = 2;
            this.lblMessage.Text = "label1";
            // 
            // chkScavengeResults
            // 
            this.chkScavengeResults.AutoSize = true;
            this.chkScavengeResults.Checked = true;
            this.chkScavengeResults.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScavengeResults.Location = new System.Drawing.Point(15, 19);
            this.chkScavengeResults.Name = "chkScavengeResults";
            this.chkScavengeResults.Size = new System.Drawing.Size(88, 17);
            this.chkScavengeResults.TabIndex = 3;
            this.chkScavengeResults.Text = "Metric results";
            this.chkScavengeResults.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.chkScavengeLog);
            this.groupBox1.Controls.Add(this.chkScavengeResults);
            this.groupBox1.Location = new System.Drawing.Point(12, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(550, 65);
            this.groupBox1.TabIndex = 4;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Scavenge Each Run During Processing";
            // 
            // chkScavengeLog
            // 
            this.chkScavengeLog.AutoSize = true;
            this.chkScavengeLog.Checked = true;
            this.chkScavengeLog.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkScavengeLog.Location = new System.Drawing.Point(15, 42);
            this.chkScavengeLog.Name = "chkScavengeLog";
            this.chkScavengeLog.Size = new System.Drawing.Size(65, 17);
            this.chkScavengeLog.TabIndex = 4;
            this.chkScavengeLog.Text = "Log files";
            this.chkScavengeLog.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 101);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(550, 23);
            this.progressBar1.TabIndex = 5;
            this.progressBar1.Visible = false;
            // 
            // frmRunBatches
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 169);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lblMessage);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRunBatches";
            this.Text = "Run Selected RBT Batches";
            this.Load += new System.EventHandler(this.frmRunBatches_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblMessage;
        private System.Windows.Forms.CheckBox chkScavengeResults;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox chkScavengeLog;
        private System.Windows.Forms.ProgressBar progressBar1;
    }
}