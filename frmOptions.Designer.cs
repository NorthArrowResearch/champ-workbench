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
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(489, 104);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 0;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(408, 104);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
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
            this.label1.TabIndex = 2;
            this.label1.Text = "RBT Console:";
            // 
            // txtOptions
            // 
            this.txtOptions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOptions.Location = new System.Drawing.Point(115, 19);
            this.txtOptions.Name = "txtOptions";
            this.txtOptions.Size = new System.Drawing.Size(368, 20);
            this.txtOptions.TabIndex = 3;
            // 
            // cmdBrowseRBT
            // 
            this.cmdBrowseRBT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseRBT.Location = new System.Drawing.Point(489, 18);
            this.cmdBrowseRBT.Name = "cmdBrowseRBT";
            this.cmdBrowseRBT.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseRBT.TabIndex = 4;
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
            this.cmdBrowse7Zip.TabIndex = 7;
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
            this.txt7Zip.TabIndex = 6;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "7 Zip software:";
            // 
            // cmdBrowseTextEditor
            // 
            this.cmdBrowseTextEditor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseTextEditor.Location = new System.Drawing.Point(489, 70);
            this.cmdBrowseTextEditor.Name = "cmdBrowseTextEditor";
            this.cmdBrowseTextEditor.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseTextEditor.TabIndex = 10;
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
            this.txtTextEditor.TabIndex = 9;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(45, 75);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(60, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Text editor:";
            // 
            // dlgBrowseExecutable
            // 
            this.dlgBrowseExecutable.Filter = "Executable Files (*.exe)|*.exe";
            // 
            // frmOptions
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(576, 139);
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
    }
}