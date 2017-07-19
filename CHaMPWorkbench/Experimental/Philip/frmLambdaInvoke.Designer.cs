namespace CHaMPWorkbench.Experimental.Philip
{
    partial class frmLambdaInvoke
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLambdaInvoke));
            this.label1 = new System.Windows.Forms.Label();
            this.txtSelectedVisits = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cboTool = new System.Windows.Forms.ComboBox();
            this.cboQueue = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.cboFunction = new System.Windows.Forms.ComboBox();
            this.cboBucket = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.lblProgress = new System.Windows.Forms.Label();
            this.pgr = new System.Windows.Forms.ProgressBar();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(75, 13);
            this.label1.TabIndex = 12;
            this.label1.Text = "Selected visits";
            // 
            // txtSelectedVisits
            // 
            this.txtSelectedVisits.Location = new System.Drawing.Point(89, 10);
            this.txtSelectedVisits.Name = "txtSelectedVisits";
            this.txtSelectedVisits.ReadOnly = true;
            this.txtSelectedVisits.Size = new System.Drawing.Size(100, 20);
            this.txtSelectedVisits.TabIndex = 13;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(57, 45);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(28, 13);
            this.label2.TabIndex = 0;
            this.label2.Text = "Tool";
            // 
            // cboTool
            // 
            this.cboTool.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboTool.FormattingEnabled = true;
            this.cboTool.Location = new System.Drawing.Point(89, 41);
            this.cboTool.Name = "cboTool";
            this.cboTool.Size = new System.Drawing.Size(266, 21);
            this.cboTool.TabIndex = 1;
            // 
            // cboQueue
            // 
            this.cboQueue.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboQueue.FormattingEnabled = true;
            this.cboQueue.Location = new System.Drawing.Point(89, 73);
            this.cboQueue.Name = "cboQueue";
            this.cboQueue.Size = new System.Drawing.Size(266, 21);
            this.cboQueue.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(46, 77);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(39, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Queue";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(37, 109);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(48, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Function";
            // 
            // cboFunction
            // 
            this.cboFunction.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboFunction.Enabled = false;
            this.cboFunction.FormattingEnabled = true;
            this.cboFunction.Location = new System.Drawing.Point(89, 105);
            this.cboFunction.Name = "cboFunction";
            this.cboFunction.Size = new System.Drawing.Size(266, 21);
            this.cboFunction.TabIndex = 5;
            // 
            // cboBucket
            // 
            this.cboBucket.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBucket.Enabled = false;
            this.cboBucket.FormattingEnabled = true;
            this.cboBucket.Location = new System.Drawing.Point(89, 137);
            this.cboBucket.Name = "cboBucket";
            this.cboBucket.Size = new System.Drawing.Size(266, 21);
            this.cboBucket.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(44, 141);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Bucket";
            // 
            // lblProgress
            // 
            this.lblProgress.AutoSize = true;
            this.lblProgress.Location = new System.Drawing.Point(37, 169);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(48, 13);
            this.lblProgress.TabIndex = 8;
            this.lblProgress.Text = "Progress";
            this.lblProgress.Visible = false;
            // 
            // pgr
            // 
            this.pgr.Location = new System.Drawing.Point(89, 169);
            this.pgr.Name = "pgr";
            this.pgr.Size = new System.Drawing.Size(266, 13);
            this.pgr.TabIndex = 9;
            this.pgr.Visible = false;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.Location = new System.Drawing.Point(284, 203);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 11;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            this.cmdCancel.Click += new System.EventHandler(this.cmdCancel_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(203, 203);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 10;
            this.cmdOK.Text = "Run";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // bgWorker
            // 
            this.bgWorker.WorkerReportsProgress = true;
            this.bgWorker.WorkerSupportsCancellation = true;
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            this.bgWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgWorker_ProgressChanged);
            this.bgWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgWorker_RunWorkerCompleted);
            // 
            // frmLambdaInvoke
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(371, 238);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.pgr);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.cboBucket);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.cboFunction);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cboQueue);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboTool);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtSelectedVisits);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmLambdaInvoke";
            this.Text = "Run AWS Automation Worker";
            this.Load += new System.EventHandler(this.frmLambdaInvoke_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSelectedVisits;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboTool;
        private System.Windows.Forms.ComboBox cboQueue;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox cboFunction;
        private System.Windows.Forms.ComboBox cboBucket;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblProgress;
        private System.Windows.Forms.ProgressBar pgr;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.ComponentModel.BackgroundWorker bgWorker;
    }
}