namespace CHaMPWorkbench.RBT.InputFiles
{
    partial class ucRBTChangeDetection
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.valThreshold = new System.Windows.Forms.NumericUpDown();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.lstSegregations = new System.Windows.Forms.CheckedListBox();
            ((System.ComponentModel.ISupportInitialize)(this.valThreshold)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(129, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Probabilistic threshold (%):";
            // 
            // valThreshold
            // 
            this.valThreshold.DecimalPlaces = 2;
            this.valThreshold.Location = new System.Drawing.Point(147, 19);
            this.valThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valThreshold.Name = "valThreshold";
            this.valThreshold.Size = new System.Drawing.Size(77, 20);
            this.valThreshold.TabIndex = 2;
            this.valThreshold.Value = new decimal(new int[] {
            80,
            0,
            0,
            0});
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.lstSegregations);
            this.groupBox1.Location = new System.Drawing.Point(15, 45);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(296, 185);
            this.groupBox1.TabIndex = 3;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Budget Segregations";
            // 
            // lstSegregations
            // 
            this.lstSegregations.FormattingEnabled = true;
            this.lstSegregations.Location = new System.Drawing.Point(6, 19);
            this.lstSegregations.Name = "lstSegregations";
            this.lstSegregations.Size = new System.Drawing.Size(279, 154);
            this.lstSegregations.TabIndex = 0;
            // 
            // ucRBTChangeDetection
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.valThreshold);
            this.Controls.Add(this.label1);
            this.Name = "ucRBTChangeDetection";
            this.Size = new System.Drawing.Size(322, 244);
            this.Load += new System.EventHandler(this.ucRBTChangeDetection_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valThreshold)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown valThreshold;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckedListBox lstSegregations;
    }
}
