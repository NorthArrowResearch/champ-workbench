namespace CHaMPWorkbench.Data
{
    partial class ucUserFeedbackGrid
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
            this.components = new System.ComponentModel.Container();
            this.grdData = new System.Windows.Forms.DataGridView();
            this.cmsUserFeedback = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.addUserFeedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.editSelectedUserFeedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteSelectedUserFeedbackToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.cmsUserFeedback.SuspendLayout();
            this.SuspendLayout();
            // 
            // grdData
            // 
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Location = new System.Drawing.Point(0, 68);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(482, 308);
            this.grdData.TabIndex = 0;
            this.grdData.SelectionChanged += new System.EventHandler(this.grdData_SelectionChanged);
            this.grdData.MouseDown += new System.Windows.Forms.MouseEventHandler(this.grdData_MouseDown);
            // 
            // cmsUserFeedback
            // 
            this.cmsUserFeedback.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addUserFeedbackToolStripMenuItem,
            this.editSelectedUserFeedbackToolStripMenuItem,
            this.deleteSelectedUserFeedbackToolStripMenuItem});
            this.cmsUserFeedback.Name = "cmsUserFeedback";
            this.cmsUserFeedback.Size = new System.Drawing.Size(243, 70);
            // 
            // addUserFeedbackToolStripMenuItem
            // 
            this.addUserFeedbackToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.Add;
            this.addUserFeedbackToolStripMenuItem.Name = "addUserFeedbackToolStripMenuItem";
            this.addUserFeedbackToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.addUserFeedbackToolStripMenuItem.Text = "Add User Feedback...";
            this.addUserFeedbackToolStripMenuItem.Click += new System.EventHandler(this.addUserFeedbackToolStripMenuItem_Click);
            // 
            // editSelectedUserFeedbackToolStripMenuItem
            // 
            this.editSelectedUserFeedbackToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.edit;
            this.editSelectedUserFeedbackToolStripMenuItem.Name = "editSelectedUserFeedbackToolStripMenuItem";
            this.editSelectedUserFeedbackToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.editSelectedUserFeedbackToolStripMenuItem.Text = "Edit Selected User Feedback...";
            this.editSelectedUserFeedbackToolStripMenuItem.Click += new System.EventHandler(this.editSelectedUserFeedbackToolStripMenuItem_Click);
            // 
            // deleteSelectedUserFeedbackToolStripMenuItem
            // 
            this.deleteSelectedUserFeedbackToolStripMenuItem.Image = global::CHaMPWorkbench.Properties.Resources.Delete;
            this.deleteSelectedUserFeedbackToolStripMenuItem.Name = "deleteSelectedUserFeedbackToolStripMenuItem";
            this.deleteSelectedUserFeedbackToolStripMenuItem.Size = new System.Drawing.Size(242, 22);
            this.deleteSelectedUserFeedbackToolStripMenuItem.Text = "Delete Selected User Feedback...";
            this.deleteSelectedUserFeedbackToolStripMenuItem.Click += new System.EventHandler(this.deleteSelectedUserFeedbackToolStripMenuItem_Click);
            // 
            // ucUserFeedbackGrid
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grdData);
            this.Name = "ucUserFeedbackGrid";
            this.Size = new System.Drawing.Size(541, 376);
            this.Load += new System.EventHandler(this.ucUserFeedbackGrid_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.cmsUserFeedback.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.ContextMenuStrip cmsUserFeedback;
        private System.Windows.Forms.ToolStripMenuItem addUserFeedbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem editSelectedUserFeedbackToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteSelectedUserFeedbackToolStripMenuItem;
    }
}
