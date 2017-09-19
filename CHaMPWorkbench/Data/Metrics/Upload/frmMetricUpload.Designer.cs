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
            this.label1 = new System.Windows.Forms.Label();
            this.cboBatch = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // cboCancel
            // 
            this.cboCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cboCancel.Location = new System.Drawing.Point(494, 202);
            this.cboCancel.Name = "cboCancel";
            this.cboCancel.Size = new System.Drawing.Size(75, 23);
            this.cboCancel.TabIndex = 0;
            this.cboCancel.Text = "Cancel";
            this.cboCancel.UseVisualStyleBackColor = true;
            // 
            // cboOK
            // 
            this.cboOK.Location = new System.Drawing.Point(413, 202);
            this.cboOK.Name = "cboOK";
            this.cboOK.Size = new System.Drawing.Size(75, 23);
            this.cboOK.TabIndex = 1;
            this.cboOK.Text = "Run";
            this.cboOK.UseVisualStyleBackColor = true;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Location = new System.Drawing.Point(12, 202);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 2;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // cboBatch
            // 
            this.cboBatch.FormattingEnabled = true;
            this.cboBatch.Location = new System.Drawing.Point(95, 10);
            this.cboBatch.Name = "cboBatch";
            this.cboBatch.Size = new System.Drawing.Size(354, 21);
            this.cboBatch.TabIndex = 4;
            // 
            // frmMetricUpload
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(581, 237);
            this.Controls.Add(this.cboBatch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cboOK);
            this.Controls.Add(this.cboCancel);
            this.Name = "frmMetricUpload";
            this.Text = "Metric Upload";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cboCancel;
        private System.Windows.Forms.Button cboOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cboBatch;
    }
}