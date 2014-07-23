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
            this.SuspendLayout();
            // 
            // txtSurvey3
            // 
            this.txtSurvey3.Location = new System.Drawing.Point(167, 100);
            this.txtSurvey3.Name = "txtSurvey3";
            this.txtSurvey3.Size = new System.Drawing.Size(342, 20);
            this.txtSurvey3.TabIndex = 23;
            this.txtSurvey3.Text = "*gdb.zip";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(11, 104);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(112, 13);
            this.Label6.TabIndex = 22;
            this.Label6.Text = "Survey GDB Option 3:";
            // 
            // txtWSTIN
            // 
            this.txtWSTIN.Location = new System.Drawing.Point(167, 152);
            this.txtWSTIN.Name = "txtWSTIN";
            this.txtWSTIN.Size = new System.Drawing.Size(342, 20);
            this.txtWSTIN.TabIndex = 27;
            this.txtWSTIN.Text = "wsetin*.zip";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(11, 156);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(77, 13);
            this.Label5.TabIndex = 26;
            this.Label5.Text = "WS TIN mask:";
            // 
            // txtTopoTIN
            // 
            this.txtTopoTIN.Location = new System.Drawing.Point(167, 126);
            this.txtTopoTIN.Name = "txtTopoTIN";
            this.txtTopoTIN.Size = new System.Drawing.Size(342, 20);
            this.txtTopoTIN.TabIndex = 25;
            this.txtTopoTIN.Text = "tin*.zip";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(11, 130);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(84, 13);
            this.Label4.TabIndex = 24;
            this.Label4.Text = "Topo TIN mask:";
            // 
            // txtSurvey2
            // 
            this.txtSurvey2.Location = new System.Drawing.Point(167, 74);
            this.txtSurvey2.Name = "txtSurvey2";
            this.txtSurvey2.Size = new System.Drawing.Size(342, 20);
            this.txtSurvey2.TabIndex = 21;
            this.txtSurvey2.Text = "SurveyGDB.zip";
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(11, 78);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(112, 13);
            this.Label3.TabIndex = 20;
            this.Label3.Text = "Survey GDB Option 2:";
            // 
            // txtSurvey1
            // 
            this.txtSurvey1.Location = new System.Drawing.Point(167, 48);
            this.txtSurvey1.Name = "txtSurvey1";
            this.txtSurvey1.Size = new System.Drawing.Size(342, 20);
            this.txtSurvey1.TabIndex = 19;
            this.txtSurvey1.Text = "Orthogonal*.zip";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(11, 52);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(112, 13);
            this.Label2.TabIndex = 18;
            this.Label2.Text = "Survey GDB Option 1:";
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(433, 188);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 28;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(514, 188);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 29;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdBrowse
            // 
            this.cmdBrowse.Location = new System.Drawing.Point(515, 11);
            this.cmdBrowse.Name = "cmdBrowse";
            this.cmdBrowse.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowse.TabIndex = 17;
            this.cmdBrowse.Text = "Browse";
            this.cmdBrowse.UseVisualStyleBackColor = true;
            this.cmdBrowse.Click += new System.EventHandler(this.cmdBrowse_Click);
            // 
            // txtFolder
            // 
            this.txtFolder.Location = new System.Drawing.Point(101, 12);
            this.txtFolder.Name = "txtFolder";
            this.txtFolder.Size = new System.Drawing.Size(408, 20);
            this.txtFolder.TabIndex = 16;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(11, 16);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(83, 13);
            this.Label1.TabIndex = 15;
            this.Label1.Text = "Top level folder:";
            // 
            // frmDataUnPacker
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(602, 224);
            this.Controls.Add(this.txtSurvey3);
            this.Controls.Add(this.Label6);
            this.Controls.Add(this.txtWSTIN);
            this.Controls.Add(this.Label5);
            this.Controls.Add(this.txtTopoTIN);
            this.Controls.Add(this.Label4);
            this.Controls.Add(this.txtSurvey2);
            this.Controls.Add(this.Label3);
            this.Controls.Add(this.txtSurvey1);
            this.Controls.Add(this.Label2);
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
    }
}