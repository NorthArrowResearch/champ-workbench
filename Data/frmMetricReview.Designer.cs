namespace CHaMPWorkbench.Data
{
    partial class frmMetricReview
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.ucUserFeedback1 = new CHaMPWorkbench.Data.ucUserFeedback();
            this.ucMetricGrid1 = new CHaMPWorkbench.Data.ucMetricGrid();
            this.ucMetricPlot1 = new CHaMPWorkbench.Data.ucMetricReviewPlot();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.ucMetricGrid1);
            this.splitContainer1.Size = new System.Drawing.Size(892, 555);
            this.splitContainer1.SplitterDistance = 297;
            this.splitContainer1.TabIndex = 0;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ucMetricPlot1);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ucUserFeedback1);
            this.splitContainer2.Panel2MinSize = 295;
            this.splitContainer2.Size = new System.Drawing.Size(892, 297);
            this.splitContainer2.SplitterDistance = 297;
            this.splitContainer2.TabIndex = 0;
            // 
            // ucUserFeedback1
            // 
            this.ucUserFeedback1.DBCon = null;
            this.ucUserFeedback1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucUserFeedback1.ItemReviewed = "";
            this.ucUserFeedback1.Location = new System.Drawing.Point(0, 0);
            this.ucUserFeedback1.LogID = 0;
            this.ucUserFeedback1.MinimumSize = new System.Drawing.Size(295, 300);
            this.ucUserFeedback1.Name = "ucUserFeedback1";
            this.ucUserFeedback1.Size = new System.Drawing.Size(591, 300);
            this.ucUserFeedback1.TabIndex = 0;
            // 
            // ucMetricGrid1
            // 
            this.ucMetricGrid1.DBCon = null;
            this.ucMetricGrid1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMetricGrid1.Location = new System.Drawing.Point(0, 0);
            this.ucMetricGrid1.Name = "ucMetricGrid1";
            this.ucMetricGrid1.ProgramID = 0;
            this.ucMetricGrid1.Size = new System.Drawing.Size(892, 254);
            this.ucMetricGrid1.TabIndex = 0;
            this.ucMetricGrid1.VisitIDs = null;
            // 
            // ucMetricPlot1
            // 
            this.ucMetricPlot1.DBCon = null;
            this.ucMetricPlot1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ucMetricPlot1.HighlightedVisitID = 0;
            this.ucMetricPlot1.Location = new System.Drawing.Point(0, 0);
            this.ucMetricPlot1.Name = "ucMetricPlot1";
            this.ucMetricPlot1.Program = null;
            this.ucMetricPlot1.Size = new System.Drawing.Size(297, 297);
            this.ucMetricPlot1.TabIndex = 0;
            this.ucMetricPlot1.VisitIDs = null;
            // 
            // frmMetricReview
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(892, 555);
            this.Controls.Add(this.splitContainer1);
            this.Name = "frmMetricReview";
            this.Text = "frmMetricReview";
            this.Load += new System.EventHandler(this.frmMetricReview_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private ucMetricGrid ucMetricGrid1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private ucUserFeedback ucUserFeedback1;
        private ucMetricReviewPlot ucMetricPlot1;
    }
}