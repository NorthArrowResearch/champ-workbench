namespace CHaMPWorkbench.Habitat
{
    partial class frmScavengeHabitatResults
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
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            this.cmdBrowseProject = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtHabitatModelDB = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtCSVFile = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmdBrowseCSV = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.rdoDB = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.rdoCSV = new System.Windows.Forms.RadioButton();
            this.panelCSV = new System.Windows.Forms.Panel();
            this.groupBox1.SuspendLayout();
            this.panelCSV.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(399, 181);
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
            this.cmdOK.Location = new System.Drawing.Point(318, 181);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cmdBrowseProject
            // 
            this.cmdBrowseProject.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseProject.Location = new System.Drawing.Point(399, 45);
            this.cmdBrowseProject.Name = "cmdBrowseProject";
            this.cmdBrowseProject.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseProject.TabIndex = 2;
            this.cmdBrowseProject.Text = "Browse";
            this.cmdBrowseProject.UseVisualStyleBackColor = true;
            this.cmdBrowseProject.Click += new System.EventHandler(this.cmdBrowseProject_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(38, 50);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Habitat results xml";
            // 
            // txtHabitatModelDB
            // 
            this.txtHabitatModelDB.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHabitatModelDB.Location = new System.Drawing.Point(121, 46);
            this.txtHabitatModelDB.Name = "txtHabitatModelDB";
            this.txtHabitatModelDB.Size = new System.Drawing.Size(272, 20);
            this.txtHabitatModelDB.TabIndex = 4;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(13, 13);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(463, 30);
            this.label2.TabIndex = 5;
            this.label2.Text = "This tool reads all simulation result values from a habitat results file and writ" +
    "es them to either the Access DB or a CSV text file.";
            // 
            // txtCSVFile
            // 
            this.txtCSVFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCSVFile.Location = new System.Drawing.Point(102, 7);
            this.txtCSVFile.Name = "txtCSVFile";
            this.txtCSVFile.Size = new System.Drawing.Size(272, 20);
            this.txtCSVFile.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 11);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Output CSV file";
            // 
            // cmdBrowseCSV
            // 
            this.cmdBrowseCSV.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdBrowseCSV.Location = new System.Drawing.Point(380, 6);
            this.cmdBrowseCSV.Name = "cmdBrowseCSV";
            this.cmdBrowseCSV.Size = new System.Drawing.Size(75, 23);
            this.cmdBrowseCSV.TabIndex = 6;
            this.cmdBrowseCSV.Text = "Browse";
            this.cmdBrowseCSV.UseVisualStyleBackColor = true;
            this.cmdBrowseCSV.Click += new System.EventHandler(this.cmdBrowseCSV_Click);
            // 
            // button2
            // 
            this.button2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.button2.Location = new System.Drawing.Point(12, 181);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 9;
            this.button2.Text = "Help";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // rdoDB
            // 
            this.rdoDB.AutoSize = true;
            this.rdoDB.Checked = true;
            this.rdoDB.Location = new System.Drawing.Point(13, 19);
            this.rdoDB.Name = "rdoDB";
            this.rdoDB.Size = new System.Drawing.Size(114, 17);
            this.rdoDB.TabIndex = 10;
            this.rdoDB.TabStop = true;
            this.rdoDB.Text = "Worbench ACCDB";
            this.rdoDB.UseVisualStyleBackColor = true;
            this.rdoDB.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.rdoCSV);
            this.groupBox1.Controls.Add(this.rdoDB);
            this.groupBox1.Location = new System.Drawing.Point(16, 80);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(459, 42);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Destination";
            // 
            // rdoCSV
            // 
            this.rdoCSV.AutoSize = true;
            this.rdoCSV.Location = new System.Drawing.Point(140, 19);
            this.rdoCSV.Name = "rdoCSV";
            this.rdoCSV.Size = new System.Drawing.Size(65, 17);
            this.rdoCSV.TabIndex = 11;
            this.rdoCSV.Text = "CSV File";
            this.rdoCSV.UseVisualStyleBackColor = true;
            this.rdoCSV.CheckedChanged += new System.EventHandler(this.radioButton_CheckedChanged);
            // 
            // panelCSV
            // 
            this.panelCSV.Controls.Add(this.txtCSVFile);
            this.panelCSV.Controls.Add(this.cmdBrowseCSV);
            this.panelCSV.Controls.Add(this.label3);
            this.panelCSV.Location = new System.Drawing.Point(16, 126);
            this.panelCSV.Name = "panelCSV";
            this.panelCSV.Size = new System.Drawing.Size(458, 38);
            this.panelCSV.TabIndex = 12;
            // 
            // frmScavengeHabitatResults
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(486, 216);
            this.Controls.Add(this.panelCSV);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtHabitatModelDB);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdBrowseProject);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.MinimumSize = new System.Drawing.Size(502, 184);
            this.Name = "frmScavengeHabitatResults";
            this.Text = "Scavenge Habitat Results";
            this.Load += new System.EventHandler(this.frmScavengeHabitatResults_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.panelCSV.ResumeLayout(false);
            this.panelCSV.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.Button cmdBrowseProject;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtHabitatModelDB;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtCSVFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cmdBrowseCSV;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.RadioButton rdoDB;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton rdoCSV;
        private System.Windows.Forms.Panel panelCSV;
    }
}