namespace CHaMPWorkbench.Data
{
    partial class frmMetricGrid
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
            this.ucMetricGrid1 = new CHaMPWorkbench.Data.ucMetricGrid();
            this.SuspendLayout();
            // 
            // ucMetricGrid1
            // 
            this.ucMetricGrid1.DBCon = null;
            this.ucMetricGrid1.Location = new System.Drawing.Point(23, 28);
            this.ucMetricGrid1.Name = "ucMetricGrid1";
            this.ucMetricGrid1.Size = new System.Drawing.Size(183, 106);
            this.ucMetricGrid1.TabIndex = 0;
            this.ucMetricGrid1.VisitIDs = null;
            // 
            // frmMetricGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(383, 192);
            this.Controls.Add(this.ucMetricGrid1);
            this.Name = "frmMetricGrid";
            this.Text = "frmMetricGrid";
            this.ResumeLayout(false);

        }

        #endregion

        private ucMetricGrid ucMetricGrid1;
    }
}