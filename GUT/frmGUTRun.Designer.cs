namespace CHaMPWorkbench.GUT
{
    partial class frmGUTRun
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmGUTRun));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtPyGUT = new System.Windows.Forms.TextBox();
            this.txtPython = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboModes = new System.Windows.Forms.ComboBox();
            this.cmdBrowsePyGUT = new System.Windows.Forms.Button();
            this.cmdBrowsePython = new System.Windows.Forms.Button();
            this.dlgBrowseExecutable = new System.Windows.Forms.OpenFileDialog();
            this.cboWindowStyle = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(598, 150);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(517, 150);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 8;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 150);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 10;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(18, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "PyGUT path";
            // 
            // txtPyGUT
            // 
            this.txtPyGUT.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPyGUT.Location = new System.Drawing.Point(96, 16);
            this.txtPyGUT.Name = "txtPyGUT";
            this.txtPyGUT.ReadOnly = true;
            this.txtPyGUT.Size = new System.Drawing.Size(496, 20);
            this.txtPyGUT.TabIndex = 1;
            // 
            // txtPython
            // 
            this.txtPython.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtPython.Location = new System.Drawing.Point(96, 47);
            this.txtPython.Name = "txtPython";
            this.txtPython.ReadOnly = true;
            this.txtPython.Size = new System.Drawing.Size(496, 20);
            this.txtPython.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(20, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Python path";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(24, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "GUT mode";
            // 
            // cboModes
            // 
            this.cboModes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModes.FormattingEnabled = true;
            this.cboModes.Location = new System.Drawing.Point(96, 78);
            this.cboModes.Name = "cboModes";
            this.cboModes.Size = new System.Drawing.Size(212, 21);
            this.cboModes.TabIndex = 7;
            // 
            // cmdBrowsePyGUT
            // 
            this.cmdBrowsePyGUT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowsePyGUT.Location = new System.Drawing.Point(598, 15);
            this.cmdBrowsePyGUT.Name = "cmdBrowsePyGUT";
            this.cmdBrowsePyGUT.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowsePyGUT.TabIndex = 2;
            this.cmdBrowsePyGUT.Text = "Browse";
            this.cmdBrowsePyGUT.UseVisualStyleBackColor = true;
            this.cmdBrowsePyGUT.Click += new System.EventHandler(this.cmdBrowsePyGUT_Click);
            // 
            // cmdBrowsePython
            // 
            this.cmdBrowsePython.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowsePython.Location = new System.Drawing.Point(598, 46);
            this.cmdBrowsePython.Name = "cmdBrowsePython";
            this.cmdBrowsePython.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowsePython.TabIndex = 5;
            this.cmdBrowsePython.Text = "Browse";
            this.cmdBrowsePython.UseVisualStyleBackColor = true;
            this.cmdBrowsePython.Click += new System.EventHandler(this.cmdBrowsePython_Click);
            // 
            // cboWindowStyle
            // 
            this.cboWindowStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWindowStyle.FormattingEnabled = true;
            this.cboWindowStyle.Location = new System.Drawing.Point(96, 111);
            this.cboWindowStyle.Name = "cboWindowStyle";
            this.cboWindowStyle.Size = new System.Drawing.Size(212, 21);
            this.cboWindowStyle.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 115);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Window style";
            // 
            // frmGUTRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 185);
            this.Controls.Add(this.cboWindowStyle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdBrowsePython);
            this.Controls.Add(this.cmdBrowsePyGUT);
            this.Controls.Add(this.cboModes);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtPython);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtPyGUT);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmGUTRun";
            this.Text = "Run GUT";
            this.Load += new System.EventHandler(this.frmGUTRun_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtPyGUT;
        private System.Windows.Forms.TextBox txtPython;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboModes;
        private System.Windows.Forms.Button cmdBrowsePyGUT;
        private System.Windows.Forms.Button cmdBrowsePython;
        private System.Windows.Forms.OpenFileDialog dlgBrowseExecutable;
        private System.Windows.Forms.ComboBox cboWindowStyle;
        private System.Windows.Forms.Label label4;
    }
}