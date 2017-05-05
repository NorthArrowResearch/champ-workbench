namespace CHaMPWorkbench.UserQueries
{
    partial class frmManageQueries
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmManageQueries));
            this.grdData = new System.Windows.Forms.DataGridView();
            this.colQueryID = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colTitle = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colQueryText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDateCreated = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cmdDelete = new System.Windows.Forms.Button();
            this.cmdProperties = new System.Windows.Forms.Button();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.cmdOK = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).BeginInit();
            this.SuspendLayout();
            // 
            // grdData
            // 
            this.grdData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grdData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grdData.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colQueryID,
            this.colTitle,
            this.colQueryText,
            this.colDateCreated});
            this.grdData.Location = new System.Drawing.Point(12, 36);
            this.grdData.Name = "grdData";
            this.grdData.Size = new System.Drawing.Size(778, 168);
            this.grdData.TabIndex = 9;
            this.grdData.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.grdData_CellDoubleClick);
            this.grdData.SelectionChanged += new System.EventHandler(this.grdData_SelectionChanged);
            // 
            // colQueryID
            // 
            this.colQueryID.DataPropertyName = "QueryID";
            this.colQueryID.HeaderText = "colQueryID";
            this.colQueryID.Name = "colQueryID";
            this.colQueryID.ReadOnly = true;
            this.colQueryID.Visible = false;
            // 
            // colTitle
            // 
            this.colTitle.DataPropertyName = "Title";
            this.colTitle.HeaderText = "Title";
            this.colTitle.Name = "colTitle";
            this.colTitle.ReadOnly = true;
            // 
            // colQueryText
            // 
            this.colQueryText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.colQueryText.DataPropertyName = "QueryText";
            this.colQueryText.HeaderText = "SQL Query";
            this.colQueryText.Name = "colQueryText";
            this.colQueryText.ReadOnly = true;
            this.colQueryText.Width = 84;
            // 
            // colDateCreated
            // 
            this.colDateCreated.DataPropertyName = "CreatedOn";
            dataGridViewCellStyle1.Format = "d MMM yyy";
            dataGridViewCellStyle1.NullValue = null;
            this.colDateCreated.DefaultCellStyle = dataGridViewCellStyle1;
            this.colDateCreated.HeaderText = "Created";
            this.colDateCreated.Name = "colDateCreated";
            this.colDateCreated.ReadOnly = true;
            // 
            // cmdDelete
            // 
            this.cmdDelete.Image = global::CHaMPWorkbench.Properties.Resources.Delete;
            this.cmdDelete.Location = new System.Drawing.Point(64, 7);
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Size = new System.Drawing.Size(23, 23);
            this.cmdDelete.TabIndex = 8;
            this.cmdDelete.UseVisualStyleBackColor = true;
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // cmdProperties
            // 
            this.cmdProperties.Image = global::CHaMPWorkbench.Properties.Resources.Settings;
            this.cmdProperties.Location = new System.Drawing.Point(38, 7);
            this.cmdProperties.Name = "cmdProperties";
            this.cmdProperties.Size = new System.Drawing.Size(23, 23);
            this.cmdProperties.TabIndex = 7;
            this.cmdProperties.UseVisualStyleBackColor = true;
            this.cmdProperties.Click += new System.EventHandler(this.cmdProperties_Click);
            // 
            // cmdAdd
            // 
            this.cmdAdd.Image = global::CHaMPWorkbench.Properties.Resources.Add;
            this.cmdAdd.Location = new System.Drawing.Point(12, 7);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(23, 23);
            this.cmdAdd.TabIndex = 6;
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // cmdOK
            // 
            this.cmdOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.cmdOK.Location = new System.Drawing.Point(715, 210);
            this.cmdOK.Name = "cmdOK";
            this.cmdOK.Size = new System.Drawing.Size(75, 23);
            this.cmdOK.TabIndex = 10;
            this.cmdOK.Text = "Close";
            this.cmdOK.UseVisualStyleBackColor = true;
            // 
            // frmManageQueries
            // 
            this.AcceptButton = this.cmdOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.cmdOK;
            this.ClientSize = new System.Drawing.Size(802, 245);
            this.Controls.Add(this.cmdOK);
            this.Controls.Add(this.grdData);
            this.Controls.Add(this.cmdDelete);
            this.Controls.Add(this.cmdProperties);
            this.Controls.Add(this.cmdAdd);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "frmManageQueries";
            this.Text = "User Queries";
            this.Load += new System.EventHandler(this.frmManageQueries_Load);
            ((System.ComponentModel.ISupportInitialize)(this.grdData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grdData;
        private System.Windows.Forms.Button cmdDelete;
        private System.Windows.Forms.Button cmdProperties;
        private System.Windows.Forms.Button cmdAdd;
        private System.Windows.Forms.Button cmdOK;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueryID;
        private System.Windows.Forms.DataGridViewTextBoxColumn colTitle;
        private System.Windows.Forms.DataGridViewTextBoxColumn colQueryText;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDateCreated;
    }
}