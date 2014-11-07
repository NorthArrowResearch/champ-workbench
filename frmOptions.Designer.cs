namespace CHaMPWorkbench
{
    partial class frmOptions
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmOptions));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtOptions = new System.Windows.Forms.TextBox();
            this.cmdBrowseRBT = new System.Windows.Forms.Button();
            this.cmdBrowse7Zip = new System.Windows.Forms.Button();
            this.txt7Zip = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmdBrowseTextEditor = new System.Windows.Forms.Button();
            this.txtTextEditor = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.dlgBrowseExecutable = new System.Windows.Forms.OpenFileDialog();
            this.tTip = new System.Windows.Forms.ToolTip(this.components);
            this.cmdBrowseMonitoring = new System.Windows.Forms.Button();
            this.txtMonitoring = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cmdBrowseOutput = new System.Windows.Forms.Button();
            this.txtOutput = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(489, 202);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 11;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(408, 202);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 10;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(32, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "RBT Console:";
            // 
            // txtOptions
            // 
            this.txtOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOptions.Location = new System.Drawing.Point(115, 19);
            this.txtOptions.Name = "txtOptions";
            this.txtOptions.Size = new System.Drawing.Size(368, 20);
            this.txtOptions.TabIndex = 1;
            // 
            // cmdBrowseRBT
            // 
            this.cmdBrowseRBT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseRBT.Location = new System.Drawing.Point(489, 18);
            this.cmdBrowseRBT.Name = "cmdBrowseRBT";
            this.cmdBrowseRBT.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseRBT.TabIndex = 2;
            this.cmdBrowseRBT.Text = "Browse";
            this.cmdBrowseRBT.UseVisualStyleBackColor = true;
            this.cmdBrowseRBT.Click += new System.EventHandler(this.cmdBrowseRBT_Click);
            // 
            // cmdBrowse7Zip
            // 
            this.cmdBrowse7Zip.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowse7Zip.Location = new System.Drawing.Point(489, 44);
            this.cmdBrowse7Zip.Name = "cmdBrowse7Zip";
            this.cmdBrowse7Zip.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowse7Zip.TabIndex = 5;
            this.cmdBrowse7Zip.Text = "Browse";
            this.cmdBrowse7Zip.UseVisualStyleBackColor = true;
            this.cmdBrowse7Zip.Click += new System.EventHandler(this.cmdBrowse7Zip_Click);
            // 
            // txt7Zip
            // 
            this.txt7Zip.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt7Zip.Location = new System.Drawing.Point(115, 45);
            this.txt7Zip.Name = "txt7Zip";
            this.txt7Zip.Size = new System.Drawing.Size(368, 20);
            this.txt7Zip.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "7 Zip software:";
            // 
            // cmdBrowseTextEditor
            // 
            this.cmdBrowseTextEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseTextEditor.Location = new System.Drawing.Point(489, 70);
            this.cmdBrowseTextEditor.Name = "cmdBrowseTextEditor";
            this.cmdBrowseTextEditor.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseTextEditor.TabIndex = 8;
            this.cmdBrowseTextEditor.Text = "Browse";
            this.cmdBrowseTextEditor.UseVisualStyleBackColor = true;
            this.cmdBrowseTextEditor.Click += new System.EventHandler(this.cmdBrowseTextEditor_Click);
            // 
            // txtTextEditor
            // 
            this.txtTextEditor.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtTextEditor.Location = new System.Drawing.Point(115, 71);
            this.txtTextEditor.Name = "txtTextEditor";
            this.txtTextEditor.Size = new System.Drawing.Size(368, 20);
            this.txtTextEditor.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Text editor:";
            // 
            // dlgBrowseExecutable
            // 
            this.dlgBrowseExecutable.Filter = "Executable Files (*.exe)|*.exe";
            // 
            // cmdBrowseMonitoring
            // 
            this.cmdBrowseMonitoring.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseMonitoring.Location = new System.Drawing.Point(463, 20);
            this.cmdBrowseMonitoring.Name = "cmdBrowseMonitoring";
            this.cmdBrowseMonitoring.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseMonitoring.TabIndex = 2;
            this.cmdBrowseMonitoring.Text = "Browse";
            this.cmdBrowseMonitoring.UseVisualStyleBackColor = true;
            this.cmdBrowseMonitoring.Click += new System.EventHandler(this.cmdBrowseMonitoring_Click);
            // 
            // txtMonitoring
            // 
            this.txtMonitoring.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtMonitoring.Location = new System.Drawing.Point(102, 22);
            this.txtMonitoring.Name = "txtMonitoring";
            this.txtMonitoring.Size = new System.Drawing.Size(355, 20);
            this.txtMonitoring.TabIndex = 1;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 26);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(83, 13);
            this.label4.TabIndex = 0;
            this.label4.Text = "Monitoring data:";
            // 
            // cmdBrowseOutput
            // 
            this.cmdBrowseOutput.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseOutput.Location = new System.Drawing.Point(463, 50);
            this.cmdBrowseOutput.Name = "cmdBrowseOutput";
            this.cmdBrowseOutput.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseOutput.TabIndex = 5;
            this.cmdBrowseOutput.Text = "Browse";
            this.cmdBrowseOutput.UseVisualStyleBackColor = true;
            this.cmdBrowseOutput.Click += new System.EventHandler(this.cmdBrowseOutput_Click);
            // 
            // txtOutput
            // 
            this.txtOutput.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutput.Location = new System.Drawing.Point(102, 51);
            this.txtOutput.Name = "txtOutput";
            this.txtOutput.Size = new System.Drawing.Size(355, 20);
            this.txtOutput.TabIndex = 4;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 55);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Input output foler:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtOutput);
            this.groupBox1.Controls.Add(this.cmdBrowseOutput);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtMonitoring);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.cmdBrowseMonitoring);
            this.groupBox1.Location = new System.Drawing.Point(13, 100);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(551, 87);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Folders";
            // 
            // frmOptions
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(576, 237);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.cmdBrowseTextEditor);
            this.Controls.Add(this.txtTextEditor);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmdBrowse7Zip);
            this.Controls.Add(this.txt7Zip);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmdBrowseRBT);
            this.Controls.Add(this.txtOptions);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1024, 768);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(317, 176);
            this.Name = "frmOptions";
            this.Text = "Options";
            this.Load += new System.EventHandler(this.frmOptions_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtOptions;
        private System.Windows.Forms.Button cmdBrowseRBT;
        private System.Windows.Forms.Button cmdBrowse7Zip;
        private System.Windows.Forms.TextBox txt7Zip;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdBrowseTextEditor;
        private System.Windows.Forms.TextBox txtTextEditor;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog dlgBrowseExecutable;
        private System.Windows.Forms.ToolTip tTip;
        private System.Windows.Forms.Button cmdBrowseMonitoring;
        private System.Windows.Forms.TextBox txtMonitoring;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button cmdBrowseOutput;
        private System.Windows.Forms.TextBox txtOutput;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.GroupBox groupBox1;
    }
}