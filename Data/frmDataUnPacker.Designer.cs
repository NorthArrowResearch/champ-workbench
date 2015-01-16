namespace CHaMPWorkbench.Data
{
    partial class frmDataUnPacker
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmDataUnPacker));
            this.txtSurvey3 = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.txtWSTIN = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.txtTopoTIN = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtSurvey2 = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtSurvey1 = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdBrowse = new System.Windows.Forms.Button();
            this.txtFolder = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSurvey3
            // 
            this.txtSurvey3.Location = new System.Drawing.Point(86, 71);
            this.txtSurvey3.Name = "txtSurvey3";
            this.txtSurvey3.Size = new System.Drawing.Size(408, 20);
            this.txtSurvey3.TabIndex = 5;
            this.txtSurvey3.Text = "*gdb.zip";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(33, 75);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(47, 13);
            this.Label6.TabIndex = 4;
            this.Label6.Text = "Option 3";
            // 
            // txtWSTIN
            // 
            this.txtWSTIN.Location = new System.Drawing.Point(101, 150);
            this.txtWSTIN.Name = "txtWSTIN";
            this.txtWSTIN.Size = new System.Drawing.Size(408, 20);
            this.txtWSTIN.TabIndex = 8;
            this.txtWSTIN.Text = "wsetin*.zip";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(17, 154);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(74, 13);
            this.Label5.TabIndex = 7;
            this.Label5.Text = "WS TIN mask";
            // 
            // txtTopoTIN
            // 
            this.txtTopoTIN.Location = new System.Drawing.Point(101, 124);
            this.txtTopoTIN.Name = "txtTopoTIN";
            this.txtTopoTIN.Size = new System.Drawing.Size(408, 20);
            this.txtTopoTIN.TabIndex = 6;
            this.txtTopoTIN.Text = "tin*.zip";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(10, 128);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(81, 13);
            this.Label4.TabIndex = 5;
            this.Label4.Text = "Topo TIN mask";
            // 
            // txtSurvey2
            // 
            this.txtSurvey2.Location = new System.Drawing.Point(86, 45);
            this.txtSurvey2.Name = "txtSurvey2";
            this.txtSurvey2.Size = new System.Drawing.Size(408, 20);
            this.txtSurvey2.TabIndex = 3;
            this.txtSurvey2.Text = "SurveyGDB.zip";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(33, 49);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(47, 13);
            this.Label3.TabIndex = 2;
            this.Label3.Text = "Option 2";
            // 
            // txtSurvey1
            // 
            this.txtSurvey1.Location = new System.Drawing.Point(86, 19);
            this.txtSurvey1.Name = "txtSurvey1";
            this.txtSurvey1.Size = new System.Drawing.Size(408, 20);
            this.txtSurvey1.TabIndex = 1;
            this.txtSurvey1.Text = "Orthogonal*.zip";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(33, 23);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(47, 13);
            this.Label2.TabIndex = 0;
            this.Label2.Text = "Option 1";
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(438, 294);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 10;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(519, 294);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 11;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Location = new System.Drawing.Point(515, 95);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowse.TabIndex = 4;
            this.cmdBrowse.Text = "Browse";
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(101, 96);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(408, 20);
            this.txtFolder.TabIndex = 3;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(11, 100);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(80, 13);
            this.Label1.TabIndex = 2;
            this.Label1.Text = "Top level folder";
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(15, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(578, 35);
            this.label7.TabIndex = 0;
            this.label7.Text = resources.GetString("label7.Text");
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtSurvey1);
            this.groupBox1.Controls.Add(this.Label2);
            this.groupBox1.Controls.Add(this.txtSurvey3);
            this.groupBox1.Controls.Add(this.Label3);
            this.groupBox1.Controls.Add(this.Label6);
            this.groupBox1.Controls.Add(this.txtSurvey2);
            this.groupBox1.Location = new System.Drawing.Point(15, 175);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(507, 104);
            this.groupBox1.TabIndex = 9;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Survey GDB";
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(15, 44);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(578, 35);
            this.label8.TabIndex = 1;
            this.label8.Text = resources.GetString("label8.Text");
            // 
            // frmDataUnPacker
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(607, 330);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.txtWSTIN);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.txtTopoTIN);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdBrowse);
            this.Controls.Add(this.txtFolder);
            this.Controls.Add(this.Label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmDataUnPacker";
            this.Text = "CHaMP Monitoring Data Unpacker";
            this.Load += new System.EventHandler(this.frmDataUnPacker_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        internal System.Windows.Forms.TextBox txtSurvey3;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.TextBox txtWSTIN;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.TextBox txtTopoTIN;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox txtSurvey2;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.TextBox txtSurvey1;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.Button cmdOK;
        internal System.Windows.Forms.Button cmdCancel;
        internal System.Windows.Forms.Button cmdBrowse;
        internal System.Windows.Forms.TextBox txtFolder;
        internal System.Windows.Forms.Label Label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
    }
}