namespace CHaMPWorkbench.RBT.Batches
{
    partial class frmRandomBatch
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
            this.cboBatch = new System.Windows.Forms.ComboBox();
            this.valSize = new System.Windows.Forms.NumericUpDown();
            this.rdoOnlyBatch = new System.Windows.Forms.RadioButton();
            this.rdoLeaveOtherBatches = new System.Windows.Forms.RadioButton();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.valSize)).BeginInit();
            this.SuspendLayout();
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(436, 123);
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
            this.cmdOK.Location = new System.Drawing.Point(355, 123);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 1;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // cboBatch
            // 
            this.cboBatch.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboBatch.FormattingEnabled = true;
            this.cboBatch.Location = new System.Drawing.Point(130, 13);
            this.cboBatch.Name = "cboBatch";
            this.cboBatch.Size = new System.Drawing.Size(381, 21);
            this.cboBatch.TabIndex = 2;
            this.cboBatch.SelectedIndexChanged += new System.EventHandler(this.cboBatch_SelectedIndexChanged);
            // 
            // valSize
            // 
            this.valSize.Location = new System.Drawing.Point(130, 92);
            this.valSize.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.valSize.Name = "valSize";
            this.valSize.Size = new System.Drawing.Size(120, 20);
            this.valSize.TabIndex = 3;
            this.valSize.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // rdoOnlyBatch
            // 
            this.rdoOnlyBatch.AutoSize = true;
            this.rdoOnlyBatch.Checked = true;
            this.rdoOnlyBatch.Location = new System.Drawing.Point(130, 40);
            this.rdoOnlyBatch.Name = "rdoOnlyBatch";
            this.rdoOnlyBatch.Size = new System.Drawing.Size(174, 17);
            this.rdoOnlyBatch.TabIndex = 4;
            this.rdoOnlyBatch.TabStop = true;
            this.rdoOnlyBatch.Text = "Set all other batches to inactive";
            this.rdoOnlyBatch.UseVisualStyleBackColor = true;
            // 
            // rdoLeaveOtherBatches
            // 
            this.rdoLeaveOtherBatches.AutoSize = true;
            this.rdoLeaveOtherBatches.Location = new System.Drawing.Point(130, 64);
            this.rdoLeaveOtherBatches.Name = "rdoLeaveOtherBatches";
            this.rdoLeaveOtherBatches.Size = new System.Drawing.Size(227, 17);
            this.rdoLeaveOtherBatches.TabIndex = 5;
            this.rdoLeaveOtherBatches.TabStop = true;
            this.rdoLeaveOtherBatches.Text = "Leave all other batches with existing status";
            this.rdoLeaveOtherBatches.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(69, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(46, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Batches";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(15, 92);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 38);
            this.label2.TabIndex = 7;
            this.label2.Text = "Random number of runs to make active";
            // 
            // frmRandomBatch
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(523, 158);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.rdoLeaveOtherBatches);
            this.Controls.Add(this.rdoOnlyBatch);
            this.Controls.Add(this.valSize);
            this.Controls.Add(this.cboBatch);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmRandomBatch";
            this.Text = "Queue Random Number of Batch Runs";
            this.Load += new System.EventHandler(this.frmRandomBatch_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valSize)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.ComboBox cboBatch;
        private System.Windows.Forms.NumericUpDown valSize;
        private System.Windows.Forms.RadioButton rdoOnlyBatch;
        private System.Windows.Forms.RadioButton rdoLeaveOtherBatches;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
    }
}