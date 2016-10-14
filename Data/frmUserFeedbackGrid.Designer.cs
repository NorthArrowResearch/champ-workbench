namespace CHaMPWorkbench.Data
{
    partial class frmUserFeedbackGrid
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUserFeedbackGrid));
            this.ucUserFeedbackGrid1 = new CHaMPWorkbench.Data.ucUserFeedbackGrid();
            this.SuspendLayout();
            // 
            // ucUserFeedbackGrid1
            // 
            this.ucUserFeedbackGrid1.DBCon = null;
            this.ucUserFeedbackGrid1.Location = new System.Drawing.Point(75, 35);
            this.ucUserFeedbackGrid1.Name = "ucUserFeedbackGrid1";
            this.ucUserFeedbackGrid1.Size = new System.Drawing.Size(541, 376);
            this.ucUserFeedbackGrid1.TabIndex = 0;
            this.ucUserFeedbackGrid1.VisitIDs = null;
            // 
            // frmUserFeedbackGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(667, 445);
            this.Controls.Add(this.ucUserFeedbackGrid1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmUserFeedbackGrid";
            this.Text = "User Feedback Items";
            this.Load += new System.EventHandler(this.frmUserFeedbackGrid_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private ucUserFeedbackGrid ucUserFeedbackGrid1;
    }
}