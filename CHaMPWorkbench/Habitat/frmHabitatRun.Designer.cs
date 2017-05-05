namespace CHaMPWorkbench.Habitat
{
    partial class frmHabitatRun
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmHabitatRun));
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHabitatProjectXML = new System.Windows.Forms.TextBox();
            this.cmdBrowseProjectXML = new System.Windows.Forms.Button();
            this.dlgBrowseExecutable = new System.Windows.Forms.OpenFileDialog();
            this.cboWindowStyle = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.HabitatOutput = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(598, 492);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 9;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.Location = new System.Drawing.Point(517, 492);
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
            this.cmdHelp.Location = new System.Drawing.Point(12, 492);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 10;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project XML";
            // 
            // txtHabitatProjectXML
            // 
            this.txtHabitatProjectXML.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHabitatProjectXML.Location = new System.Drawing.Point(96, 16);
            this.txtHabitatProjectXML.Name = "txtHabitatProjectXML";
            this.txtHabitatProjectXML.ReadOnly = true;
            this.txtHabitatProjectXML.Size = new System.Drawing.Size(496, 20);
            this.txtHabitatProjectXML.TabIndex = 1;
            // 
            // cmdBrowseProjectXML
            // 
            this.cmdBrowseProjectXML.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseProjectXML.Location = new System.Drawing.Point(598, 15);
            this.cmdBrowseProjectXML.Name = "cmdBrowseProjectXML";
            this.cmdBrowseProjectXML.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseProjectXML.TabIndex = 2;
            this.cmdBrowseProjectXML.Text = "Browse";
            this.cmdBrowseProjectXML.UseVisualStyleBackColor = true;
            this.cmdBrowseProjectXML.Click += new System.EventHandler(this.cmdBrowseProjectXML_Click);
            // 
            // cboWindowStyle
            // 
            this.cboWindowStyle.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboWindowStyle.FormattingEnabled = true;
            this.cboWindowStyle.Location = new System.Drawing.Point(96, 43);
            this.cboWindowStyle.Name = "cboWindowStyle";
            this.cboWindowStyle.Size = new System.Drawing.Size(212, 21);
            this.cboWindowStyle.TabIndex = 12;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(24, 47);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(70, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Window style";
            // 
            // HabitatOutput
            // 
            this.HabitatOutput.BackColor = System.Drawing.Color.White;
            this.HabitatOutput.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.HabitatOutput.Location = new System.Drawing.Point(13, 70);
            this.HabitatOutput.Multiline = true;
            this.HabitatOutput.Name = "HabitatOutput";
            this.HabitatOutput.ReadOnly = true;
            this.HabitatOutput.Size = new System.Drawing.Size(660, 416);
            this.HabitatOutput.TabIndex = 13;
            // 
            // frmHabitatRun
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(685, 527);
            this.Controls.Add(this.HabitatOutput);
            this.Controls.Add(this.cboWindowStyle);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmdBrowseProjectXML);
            this.Controls.Add(this.txtHabitatProjectXML);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmHabitatRun";
            this.Text = "Run Habitat";
            this.Load += new System.EventHandler(this.frmGUTRun_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHabitatProjectXML;
        private System.Windows.Forms.Button cmdBrowseProjectXML;
        private System.Windows.Forms.OpenFileDialog dlgBrowseExecutable;
        private System.Windows.Forms.ComboBox cboWindowStyle;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox HabitatOutput;
    }
}