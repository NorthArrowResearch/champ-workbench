namespace CHaMPWorkbench.Data
{
    partial class frmFindVisitByID
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
            this.label1 = new System.Windows.Forms.Label();
            this.valVisitID = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmdInputXML = new System.Windows.Forms.Button();
            this.cmdRBTBatch = new System.Windows.Forms.Button();
            this.cmdCopyOutput = new System.Windows.Forms.Button();
            this.cmdExplorerOutput = new System.Windows.Forms.Button();
            this.txtOutputPath = new System.Windows.Forms.TextBox();
            this.cmdCopySurveyData = new System.Windows.Forms.Button();
            this.cmdExploreSurveyData = new System.Windows.Forms.Button();
            this.txtSurveyPath = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.cmdClose = new System.Windows.Forms.Button();
            this.tTip = new System.Windows.Forms.ToolTip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.valVisitID)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 2;
            this.label1.Text = "Visit ID";
            // 
            // valVisitID
            // 
            this.valVisitID.Location = new System.Drawing.Point(65, 14);
            this.valVisitID.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.valVisitID.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valVisitID.Name = "valVisitID";
            this.valVisitID.Size = new System.Drawing.Size(120, 20);
            this.valVisitID.TabIndex = 1;
            this.valVisitID.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valVisitID.ValueChanged += new System.EventHandler(this.valVisitID_ValueChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmdInputXML);
            this.groupBox1.Controls.Add(this.cmdRBTBatch);
            this.groupBox1.Controls.Add(this.cmdCopyOutput);
            this.groupBox1.Controls.Add(this.cmdExplorerOutput);
            this.groupBox1.Controls.Add(this.txtOutputPath);
            this.groupBox1.Controls.Add(this.cmdCopySurveyData);
            this.groupBox1.Controls.Add(this.cmdExploreSurveyData);
            this.groupBox1.Controls.Add(this.txtSurveyPath);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.txtResult);
            this.groupBox1.Location = new System.Drawing.Point(16, 42);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(507, 192);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Visit Info";
            // 
            // cmdInputXML
            // 
            this.cmdInputXML.Image = global::CHaMPWorkbench.Properties.Resources.icon_xml;
            this.cmdInputXML.Location = new System.Drawing.Point(475, 139);
            this.cmdInputXML.Name = "cmdInputXML";
            this.cmdInputXML.Size = new System.Drawing.Size(23, 23);
            this.cmdInputXML.TabIndex = 10;
            this.cmdInputXML.UseVisualStyleBackColor = true;
            this.cmdInputXML.Click += new System.EventHandler(this.cmdInputXML_Click);
            // 
            // cmdRBTBatch
            // 
            this.cmdRBTBatch.Image = global::CHaMPWorkbench.Properties.Resources.rbt_16x16;
            this.cmdRBTBatch.Location = new System.Drawing.Point(475, 163);
            this.cmdRBTBatch.Name = "cmdRBTBatch";
            this.cmdRBTBatch.Size = new System.Drawing.Size(23, 23);
            this.cmdRBTBatch.TabIndex = 9;
            this.cmdRBTBatch.UseVisualStyleBackColor = true;
            this.cmdRBTBatch.Click += new System.EventHandler(this.button1_Click);
            // 
            // cmdCopyOutput
            // 
            this.cmdCopyOutput.Image = global::CHaMPWorkbench.Properties.Resources.Copy;
            this.cmdCopyOutput.Location = new System.Drawing.Point(421, 164);
            this.cmdCopyOutput.Name = "cmdCopyOutput";
            this.cmdCopyOutput.Size = new System.Drawing.Size(23, 23);
            this.cmdCopyOutput.TabIndex = 8;
            this.cmdCopyOutput.UseVisualStyleBackColor = true;
            this.cmdCopyOutput.Click += new System.EventHandler(this.cmdCopyOutput_Click);
            // 
            // cmdExplorerOutput
            // 
            this.cmdExplorerOutput.Image = global::CHaMPWorkbench.Properties.Resources.BrowseFolder;
            this.cmdExplorerOutput.Location = new System.Drawing.Point(448, 164);
            this.cmdExplorerOutput.Name = "cmdExplorerOutput";
            this.cmdExplorerOutput.Size = new System.Drawing.Size(23, 23);
            this.cmdExplorerOutput.TabIndex = 7;
            this.cmdExplorerOutput.UseVisualStyleBackColor = true;
            this.cmdExplorerOutput.Click += new System.EventHandler(this.cmdExplorerOutput_Click);
            // 
            // txtOutputPath
            // 
            this.txtOutputPath.Location = new System.Drawing.Point(74, 165);
            this.txtOutputPath.Name = "txtOutputPath";
            this.txtOutputPath.ReadOnly = true;
            this.txtOutputPath.Size = new System.Drawing.Size(341, 20);
            this.txtOutputPath.TabIndex = 6;
            this.txtOutputPath.TextChanged += new System.EventHandler(this.txtOutputPath_TextChanged);
            // 
            // cmdCopySurveyData
            // 
            this.cmdCopySurveyData.Image = global::CHaMPWorkbench.Properties.Resources.Copy;
            this.cmdCopySurveyData.Location = new System.Drawing.Point(421, 138);
            this.cmdCopySurveyData.Name = "cmdCopySurveyData";
            this.cmdCopySurveyData.Size = new System.Drawing.Size(23, 23);
            this.cmdCopySurveyData.TabIndex = 5;
            this.cmdCopySurveyData.UseVisualStyleBackColor = true;
            this.cmdCopySurveyData.Click += new System.EventHandler(this.cmdCopySurveyData_Click);
            // 
            // cmdExploreSurveyData
            // 
            this.cmdExploreSurveyData.Image = global::CHaMPWorkbench.Properties.Resources.BrowseFolder;
            this.cmdExploreSurveyData.Location = new System.Drawing.Point(448, 138);
            this.cmdExploreSurveyData.Name = "cmdExploreSurveyData";
            this.cmdExploreSurveyData.Size = new System.Drawing.Size(23, 23);
            this.cmdExploreSurveyData.TabIndex = 4;
            this.cmdExploreSurveyData.UseVisualStyleBackColor = true;
            this.cmdExploreSurveyData.Click += new System.EventHandler(this.cmdExploreSurveyData_Click);
            // 
            // txtSurveyPath
            // 
            this.txtSurveyPath.Location = new System.Drawing.Point(74, 139);
            this.txtSurveyPath.Name = "txtSurveyPath";
            this.txtSurveyPath.ReadOnly = true;
            this.txtSurveyPath.Size = new System.Drawing.Size(341, 20);
            this.txtSurveyPath.TabIndex = 3;
            this.txtSurveyPath.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 169);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 2;
            this.label3.Text = "Outputs";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 143);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Survey data";
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(7, 20);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.ReadOnly = true;
            this.txtResult.Size = new System.Drawing.Size(492, 112);
            this.txtResult.TabIndex = 0;
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdClose.Location = new System.Drawing.Point(448, 240);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 4;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            // 
            // frmFindVisitByID
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(535, 275);
            this.Controls.Add(this.cmdClose);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.valVisitID);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmFindVisitByID";
            this.Text = "Find Visit By ID";
            this.Load += new System.EventHandler(this.frmFindVisitByID_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valVisitID)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown valVisitID;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Button cmdExploreSurveyData;
        private System.Windows.Forms.TextBox txtSurveyPath;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button cmdCopySurveyData;
        private System.Windows.Forms.Button cmdCopyOutput;
        private System.Windows.Forms.Button cmdExplorerOutput;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button cmdRBTBatch;
        private System.Windows.Forms.Button cmdInputXML;
        private System.Windows.Forms.ToolTip tTip;
    }
}