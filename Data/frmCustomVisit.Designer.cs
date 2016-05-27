namespace CHaMPWorkbench.Data
{
    partial class frmCustomVisit
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.label1 = new System.Windows.Forms.Label();
            this.valVisitID = new System.Windows.Forms.NumericUpDown();
            this.cboWatershed = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.cboSite = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.valFieldSeason = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.cboProtocol = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtOrganization = new System.Windows.Forms.TextBox();
            this.grpChannelUnits = new System.Windows.Forms.GroupBox();
            this.button3 = new System.Windows.Forms.Button();
            this.grdChannelUnits = new System.Windows.Forms.DataGridView();
            this.colUnitNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSegmentNumber = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTier1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.colTier2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.button2 = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.cmdCancel = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.valVisitID)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.valFieldSeason)).BeginInit();
            this.grpChannelUnits.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdChannelUnits)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Visit ID";
            // 
            // valVisitID
            // 
            this.valVisitID.Location = new System.Drawing.Point(83, 15);
            this.valVisitID.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
            this.valVisitID.Minimum = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            this.valVisitID.Name = "valVisitID";
            this.valVisitID.Size = new System.Drawing.Size(72, 20);
            this.valVisitID.TabIndex = 1;
            this.valVisitID.Value = new decimal(new int[] {
            9000,
            0,
            0,
            0});
            // 
            // cboWatershed
            // 
            this.cboWatershed.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboWatershed.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboWatershed.FormattingEnabled = true;
            this.cboWatershed.Location = new System.Drawing.Point(83, 45);
            this.cboWatershed.Name = "cboWatershed";
            this.cboWatershed.Size = new System.Drawing.Size(174, 21);
            this.cboWatershed.TabIndex = 2;
            this.cboWatershed.SelectedIndexChanged += new System.EventHandler(this.cboWatershed_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Watershed";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(52, 80);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(25, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Site";
            // 
            // cboSite
            // 
            this.cboSite.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cboSite.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cboSite.FormattingEnabled = true;
            this.cboSite.Location = new System.Drawing.Point(83, 76);
            this.cboSite.Name = "cboSite";
            this.cboSite.Size = new System.Drawing.Size(174, 21);
            this.cboSite.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(272, 19);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "Field season";
            // 
            // valFieldSeason
            // 
            this.valFieldSeason.Location = new System.Drawing.Point(344, 15);
            this.valFieldSeason.Maximum = new decimal(new int[] {
            2020,
            0,
            0,
            0});
            this.valFieldSeason.Minimum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
            this.valFieldSeason.Name = "valFieldSeason";
            this.valFieldSeason.Size = new System.Drawing.Size(72, 20);
            this.valFieldSeason.TabIndex = 7;
            this.valFieldSeason.Value = new decimal(new int[] {
            2011,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(292, 53);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(46, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Protocol";
            // 
            // cboProtocol
            // 
            this.cboProtocol.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboProtocol.FormattingEnabled = true;
            this.cboProtocol.Location = new System.Drawing.Point(344, 49);
            this.cboProtocol.Name = "cboProtocol";
            this.cboProtocol.Size = new System.Drawing.Size(228, 21);
            this.cboProtocol.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(272, 84);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(66, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Organization";
            // 
            // txtOrganization
            // 
            this.txtOrganization.Location = new System.Drawing.Point(344, 80);
            this.txtOrganization.Name = "txtOrganization";
            this.txtOrganization.Size = new System.Drawing.Size(228, 20);
            this.txtOrganization.TabIndex = 11;
            // 
            // grpChannelUnits
            // 
            this.grpChannelUnits.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grpChannelUnits.Controls.Add(this.button3);
            this.grpChannelUnits.Controls.Add(this.grdChannelUnits);
            this.grpChannelUnits.Controls.Add(this.button2);
            this.grpChannelUnits.Controls.Add(this.button1);
            this.grpChannelUnits.Location = new System.Drawing.Point(13, 106);
            this.grpChannelUnits.Name = "grpChannelUnits";
            this.grpChannelUnits.Size = new System.Drawing.Size(567, 240);
            this.grpChannelUnits.TabIndex = 12;
            this.grpChannelUnits.TabStop = false;
            this.grpChannelUnits.Text = "Channel Units";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(170, 20);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(136, 23);
            this.button3.TabIndex = 3;
            this.button3.Text = "Load From Survey GDB";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // grdChannelUnits
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdChannelUnits.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.grdChannelUnits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdChannelUnits.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colUnitNumber,
            this.colSegmentNumber,
            this.colTier1,
            this.colTier2});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.grdChannelUnits.DefaultCellStyle = dataGridViewCellStyle2;
            this.grdChannelUnits.Location = new System.Drawing.Point(6, 49);
            this.grdChannelUnits.Name = "grdChannelUnits";
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.grdChannelUnits.RowHeadersDefaultCellStyle = dataGridViewCellStyle3;
            this.grdChannelUnits.Size = new System.Drawing.Size(548, 185);
            this.grdChannelUnits.TabIndex = 2;
            this.grdChannelUnits.RowValidating += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grdChannelUnits_RowValidating);
            // 
            // colUnitNumber
            // 
            this.colUnitNumber.DataPropertyName = "UnitNumber";
            this.colUnitNumber.HeaderText = "Unit Number";
            this.colUnitNumber.Name = "colUnitNumber";
            // 
            // colSegmentNumber
            // 
            this.colSegmentNumber.DataPropertyName = "SegmentNumber";
            this.colSegmentNumber.HeaderText = "Segment Number";
            this.colSegmentNumber.Name = "colSegmentNumber";
            // 
            // colTier1
            // 
            this.colTier1.DataPropertyName = "Tier1";
            this.colTier1.HeaderText = "Tier 1";
            this.colTier1.Name = "colTier1";
            // 
            // colTier2
            // 
            this.colTier2.DataPropertyName = "Tier2";
            this.colTier2.HeaderText = "Tier 2";
            this.colTier2.Name = "colTier2";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(312, 20);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(136, 23);
            this.button2.TabIndex = 1;
            this.button2.Text = "Load From Existing Visit";
            this.button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(454, 20);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(100, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Load From CSV";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // cmdCancel
            // 
            this.cmdCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cmdCancel.Location = new System.Drawing.Point(505, 352);
            this.cmdCancel.Name = "cmdCancel";
            this.cmdCancel.Size = new System.Drawing.Size(75, 23);
            this.cmdCancel.TabIndex = 13;
            this.cmdCancel.Text = "Cancel";
            this.cmdCancel.UseVisualStyleBackColor = true;
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(424, 352);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 14;
            this.cmdOK.Text = "OK";
            this.cmdOK.UseVisualStyleBackColor = true;
            this.cmdOK.Click += new System.EventHandler(this.cmdOK_Click);
            // 
            // frmCustomVisit
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdCancel;
            this.ClientSize = new System.Drawing.Size(592, 387);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.cmdCancel);
            this.Controls.Add(this.grpChannelUnits);
            this.Controls.Add(this.txtOrganization);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cboProtocol);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.valFieldSeason);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboSite);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cboWatershed);
            this.Controls.Add(this.valVisitID);
            this.Controls.Add(this.label1);
            this.Name = "frmCustomVisit";
            this.Text = "Custom Visit";
            this.Load += new System.EventHandler(this.frmCustomVisit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.valVisitID)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.valFieldSeason)).EndInit();
            this.grpChannelUnits.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdChannelUnits)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown valVisitID;
        private System.Windows.Forms.ComboBox cboWatershed;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cboSite;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown valFieldSeason;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cboProtocol;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtOrganization;
        private System.Windows.Forms.GroupBox grpChannelUnits;
        private System.Windows.Forms.Button cmdCancel;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.DataGridView grdChannelUnits;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.DataGridViewTextBoxColumn colUnitNumber;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSegmentNumber;
        private System.Windows.Forms.DataGridViewComboBoxColumn colTier1;
        private System.Windows.Forms.DataGridViewComboBoxColumn colTier2;
    }
}