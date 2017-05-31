namespace CHaMPWorkbench.Data
{
    partial class frmVisitDetails
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
            this.cmdClose = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtVisitID = new System.Windows.Forms.TextBox();
            this.txtSite = new System.Windows.Forms.TextBox();
            this.txtFieldSeason = new System.Windows.Forms.TextBox();
            this.txtWatershed = new System.Windows.Forms.TextBox();
            this.txtPanel = new System.Windows.Forms.TextBox();
            this.txtOrganization = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.grdVisitDetails = new System.Windows.Forms.DataGridView();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.grdChannelUnits = new System.Windows.Forms.DataGridView();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.grdLogMessages = new System.Windows.Forms.DataGridView();
            this.colLogMessageID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTyp = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSeverity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colMessage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.label8 = new System.Windows.Forms.Label();
            this.cboLogMessageTypes = new System.Windows.Forms.ComboBox();
            this.cboLogResults = new System.Windows.Forms.ComboBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage6 = new System.Windows.Forms.TabPage();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdVisitDetails)).BeginInit();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdChannelUnits)).BeginInit();
            this.tabPage4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLogMessages)).BeginInit();
            this.tabPage6.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmdClose
            // 
            this.cmdClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdClose.Location = new System.Drawing.Point(629, 430);
            this.cmdClose.Name = "cmdClose";
            this.cmdClose.Size = new System.Drawing.Size(75, 23);
            this.cmdClose.TabIndex = 0;
            this.cmdClose.Text = "Close";
            this.cmdClose.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Visit ID";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(28, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(25, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Site";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(235, 17);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Watershed";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(228, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 4;
            this.label4.Text = "Field season";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(468, 17);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 5;
            this.label5.Text = "Organization";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(500, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 6;
            this.label6.Text = "Panel";
            // 
            // txtVisitID
            // 
            this.txtVisitID.Location = new System.Drawing.Point(57, 13);
            this.txtVisitID.Name = "txtVisitID";
            this.txtVisitID.ReadOnly = true;
            this.txtVisitID.Size = new System.Drawing.Size(164, 20);
            this.txtVisitID.TabIndex = 7;
            // 
            // txtSite
            // 
            this.txtSite.Location = new System.Drawing.Point(57, 42);
            this.txtSite.Name = "txtSite";
            this.txtSite.ReadOnly = true;
            this.txtSite.Size = new System.Drawing.Size(164, 20);
            this.txtSite.TabIndex = 8;
            // 
            // txtFieldSeason
            // 
            this.txtFieldSeason.Location = new System.Drawing.Point(297, 42);
            this.txtFieldSeason.Name = "txtFieldSeason";
            this.txtFieldSeason.ReadOnly = true;
            this.txtFieldSeason.Size = new System.Drawing.Size(164, 20);
            this.txtFieldSeason.TabIndex = 10;
            // 
            // txtWatershed
            // 
            this.txtWatershed.Location = new System.Drawing.Point(297, 13);
            this.txtWatershed.Name = "txtWatershed";
            this.txtWatershed.ReadOnly = true;
            this.txtWatershed.Size = new System.Drawing.Size(164, 20);
            this.txtWatershed.TabIndex = 9;
            // 
            // txtPanel
            // 
            this.txtPanel.Location = new System.Drawing.Point(538, 42);
            this.txtPanel.Name = "txtPanel";
            this.txtPanel.ReadOnly = true;
            this.txtPanel.Size = new System.Drawing.Size(164, 20);
            this.txtPanel.TabIndex = 12;
            // 
            // txtOrganization
            // 
            this.txtOrganization.Location = new System.Drawing.Point(537, 13);
            this.txtOrganization.Name = "txtOrganization";
            this.txtOrganization.ReadOnly = true;
            this.txtOrganization.Size = new System.Drawing.Size(164, 20);
            this.txtOrganization.TabIndex = 11;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage4);
            this.tabControl1.Controls.Add(this.tabPage6);
            this.tabControl1.Location = new System.Drawing.Point(12, 77);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(692, 347);
            this.tabControl1.TabIndex = 13;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.grdVisitDetails);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(684, 321);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Visit Details";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // grdVisitDetails
            // 
            this.grdVisitDetails.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdVisitDetails.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdVisitDetails.Location = new System.Drawing.Point(3, 3);
            this.grdVisitDetails.Name = "grdVisitDetails";
            this.grdVisitDetails.Size = new System.Drawing.Size(678, 315);
            this.grdVisitDetails.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.grdChannelUnits);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(684, 321);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Channel Units";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // grdChannelUnits
            // 
            this.grdChannelUnits.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdChannelUnits.Dock = System.Windows.Forms.DockStyle.Fill;
            this.grdChannelUnits.Location = new System.Drawing.Point(3, 3);
            this.grdChannelUnits.Name = "grdChannelUnits";
            this.grdChannelUnits.Size = new System.Drawing.Size(678, 315);
            this.grdChannelUnits.TabIndex = 0;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.grdLogMessages);
            this.tabPage4.Controls.Add(this.label8);
            this.tabPage4.Controls.Add(this.cboLogMessageTypes);
            this.tabPage4.Controls.Add(this.cboLogResults);
            this.tabPage4.Controls.Add(this.label7);
            this.tabPage4.Location = new System.Drawing.Point(4, 22);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage4.Size = new System.Drawing.Size(684, 321);
            this.tabPage4.TabIndex = 5;
            this.tabPage4.Text = "Log Messages";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // grdLogMessages
            // 
            this.grdLogMessages.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdLogMessages.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdLogMessages.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colLogMessageID,
            this.colTyp,
            this.colSeverity,
            this.colMessage});
            this.grdLogMessages.Location = new System.Drawing.Point(6, 75);
            this.grdLogMessages.Name = "grdLogMessages";
            this.grdLogMessages.Size = new System.Drawing.Size(672, 240);
            this.grdLogMessages.TabIndex = 4;
            // 
            // colLogMessageID
            // 
            this.colLogMessageID.DataPropertyName = "LogMessageID";
            this.colLogMessageID.HeaderText = "LogMessageID";
            this.colLogMessageID.Name = "colLogMessageID";
            this.colLogMessageID.ReadOnly = true;
            this.colLogMessageID.Visible = false;
            // 
            // colTyp
            // 
            this.colTyp.DataPropertyName = "MessageType";
            this.colTyp.HeaderText = "Type";
            this.colTyp.Name = "colTyp";
            this.colTyp.ReadOnly = true;
            // 
            // colSeverity
            // 
            this.colSeverity.DataPropertyName = "LogSeverity";
            this.colSeverity.HeaderText = "Severity";
            this.colSeverity.Name = "colSeverity";
            this.colSeverity.ReadOnly = true;
            // 
            // colMessage
            // 
            this.colMessage.DataPropertyName = "LogMessage";
            this.colMessage.HeaderText = "Message";
            this.colMessage.Name = "colMessage";
            this.colMessage.ReadOnly = true;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(15, 51);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(73, 13);
            this.label8.TabIndex = 3;
            this.label8.Text = "Message type";
            // 
            // cboLogMessageTypes
            // 
            this.cboLogMessageTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLogMessageTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLogMessageTypes.FormattingEnabled = true;
            this.cboLogMessageTypes.Location = new System.Drawing.Point(94, 48);
            this.cboLogMessageTypes.Name = "cboLogMessageTypes";
            this.cboLogMessageTypes.Size = new System.Drawing.Size(584, 21);
            this.cboLogMessageTypes.TabIndex = 2;
            // 
            // cboLogResults
            // 
            this.cboLogResults.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboLogResults.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboLogResults.FormattingEnabled = true;
            this.cboLogResults.Location = new System.Drawing.Point(94, 15);
            this.cboLogResults.Name = "cboLogResults";
            this.cboLogResults.Size = new System.Drawing.Size(584, 21);
            this.cboLogResults.TabIndex = 1;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(24, 19);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(64, 13);
            this.label7.TabIndex = 0;
            this.label7.Text = "Model result";
            // 
            // tabPage6
            // 
            this.tabPage6.Controls.Add(this.txtNotes);
            this.tabPage6.Location = new System.Drawing.Point(4, 22);
            this.tabPage6.Name = "tabPage6";
            this.tabPage6.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage6.Size = new System.Drawing.Size(684, 321);
            this.tabPage6.TabIndex = 6;
            this.tabPage6.Text = "Notes";
            this.tabPage6.UseVisualStyleBackColor = true;
            // 
            // txtNotes
            // 
            this.txtNotes.BackColor = System.Drawing.Color.White;
            this.txtNotes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txtNotes.Location = new System.Drawing.Point(3, 3);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.ReadOnly = true;
            this.txtNotes.Size = new System.Drawing.Size(678, 315);
            this.txtNotes.TabIndex = 0;
            // 
            // frmVisitDetails
            // 
            this.AcceptButton = this.cmdClose;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdClose;
            this.ClientSize = new System.Drawing.Size(716, 465);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtPanel);
            this.Controls.Add(this.txtOrganization);
            this.Controls.Add(this.txtFieldSeason);
            this.Controls.Add(this.txtWatershed);
            this.Controls.Add(this.txtSite);
            this.Controls.Add(this.txtVisitID);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmdClose);
            this.MinimumSize = new System.Drawing.Size(732, 251);
            this.Name = "frmVisitDetails";
            this.ShowIcon = false;
            this.Text = "Visit Details";
            this.Load += new System.EventHandler(this.frmVisitDetails_Load);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdVisitDetails)).EndInit();
            this.tabPage2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grdChannelUnits)).EndInit();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grdLogMessages)).EndInit();
            this.tabPage6.ResumeLayout(false);
            this.tabPage6.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button cmdClose;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtVisitID;
        private System.Windows.Forms.TextBox txtSite;
        private System.Windows.Forms.TextBox txtFieldSeason;
        private System.Windows.Forms.TextBox txtWatershed;
        private System.Windows.Forms.TextBox txtPanel;
        private System.Windows.Forms.TextBox txtOrganization;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.DataGridView grdVisitDetails;
        private System.Windows.Forms.DataGridView grdChannelUnits;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.DataGridView grdLogMessages;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cboLogMessageTypes;
        private System.Windows.Forms.ComboBox cboLogResults;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLogMessageID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTyp;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSeverity;
        private System.Windows.Forms.DataGridViewTextBoxColumn colMessage;
        private System.Windows.Forms.TabPage tabPage6;
        private System.Windows.Forms.TextBox txtNotes;
    }
}