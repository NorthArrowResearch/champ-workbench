namespace CHaMPWorkbench.Data
{
    partial class frmAPIUpload
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdBrowseProject = new System.Windows.Forms.Button();
            this.txtProjectFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.grdData = new System.Windows.Forms.DataGridView();
            this.cmdStart = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdHelp = new System.Windows.Forms.Button();
            this.grpProgress = new System.Windows.Forms.GroupBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.bgWorker = new System.ComponentModel.BackgroundWorker();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.grpProgress.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.grdData);
            this.groupBox1.Controls.Add(this.cmdBrowseProject);
            this.groupBox1.Controls.Add(this.txtProjectFile);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(545, 206);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Topo Survey Project";
            // 
            // cmdBrowseProject
            // 
            this.cmdBrowseProject.Image = global::CHaMPWorkbench.Properties.Resources.BrowseFolder;
            this.cmdBrowseProject.Location = new System.Drawing.Point(516, 22);
            this.cmdBrowseProject.Name = "cmdBrowseProject";
            this.cmdBrowseProject.Size = new System.Drawing.Size(23, 23);
            this.cmdBrowseProject.TabIndex = 2;
            this.cmdBrowseProject.UseVisualStyleBackColor = true;
            this.cmdBrowseProject.Click += new System.EventHandler(this.cmdBrowseProject_Click);
            // 
            // txtProjectFile
            // 
            this.txtProjectFile.Location = new System.Drawing.Point(105, 23);
            this.txtProjectFile.Name = "txtProjectFile";
            this.txtProjectFile.ReadOnly = true;
            this.txtProjectFile.Size = new System.Drawing.Size(405, 20);
            this.txtProjectFile.TabIndex = 1;
            this.txtProjectFile.TextChanged += new System.EventHandler(this.txtProjectFile_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(56, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Project file";
            // 
            // grdData
            // 
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Location = new System.Drawing.Point(6, 56);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(533, 144);
            this.grdData.TabIndex = 3;
            // 
            // cmdStart
            // 
            this.cmdStart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdStart.Location = new System.Drawing.Point(401, 416);
            this.cmdStart.Name = "cmdStart";
            this.cmdStart.Size = new System.Drawing.Size(75, 23);
            this.cmdStart.TabIndex = 1;
            this.cmdStart.Text = "Start";
            this.cmdStart.UseVisualStyleBackColor = true;
            this.cmdStart.Click += new System.EventHandler(this.cmdStart_Click);
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(482, 416);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 2;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdHelp
            // 
            this.cmdHelp.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmdHelp.Location = new System.Drawing.Point(12, 416);
            this.cmdHelp.Name = "cmdHelp";
            this.cmdHelp.Size = new System.Drawing.Size(75, 23);
            this.cmdHelp.TabIndex = 3;
            this.cmdHelp.Text = "Help";
            this.cmdHelp.UseVisualStyleBackColor = true;
            // 
            // grpProgress
            // 
            this.grpProgress.Controls.Add(this.textBox1);
            this.grpProgress.Location = new System.Drawing.Point(12, 224);
            this.grpProgress.Name = "grpProgress";
            this.grpProgress.Size = new System.Drawing.Size(545, 186);
            this.grpProgress.TabIndex = 4;
            this.grpProgress.TabStop = false;
            this.grpProgress.Text = "Progress";
            // 
            // textBox1
            // 
            this.textBox1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.textBox1.Location = new System.Drawing.Point(6, 19);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(533, 161);
            this.textBox1.TabIndex = 0;
            // 
            // bgWorker
            // 
            this.bgWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgWorker_DoWork);
            // 
            // frmAPIUpload
            // 
            this.AcceptButton = this.cmdStart;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(569, 451);
            this.Controls.Add(this.grpProgress);
            this.Controls.Add(this.cmdHelp);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.cmdStart);
            this.Controls.Add(this.groupBox1);
            this.Name = "frmAPIUpload";
            this.Text = "Upload Topo Survey Project";
            this.Load += new System.EventHandler(this.frmAPIUpload_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.grpProgress.ResumeLayout(false);
            this.grpProgress.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button cmdBrowseProject;
        private System.Windows.Forms.TextBox txtProjectFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.Button cmdStart;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdHelp;
        private System.Windows.Forms.GroupBox grpProgress;
        private System.Windows.Forms.TextBox textBox1;
        private System.ComponentModel.BackgroundWorker bgWorker;
    }
}