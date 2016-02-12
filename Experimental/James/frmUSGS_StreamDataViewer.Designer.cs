﻿namespace CHaMPWorkbench.Experimental.James
{
    partial class frmUSGS_StreamDataViewer
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
            this.zedGraphControl = new ZedGraph.ZedGraphControl();
            this.cmdGetData = new System.Windows.Forms.Button();
            this.grbFigure = new System.Windows.Forms.GroupBox();
            this.lblUSGS_StreamGageNumber = new System.Windows.Forms.Label();
            this.cmdOutputDirectory = new System.Windows.Forms.Button();
            this.txtOutputDirectory = new System.Windows.Forms.TextBox();
            this.grbFilters = new System.Windows.Forms.GroupBox();
            this.lblSite = new System.Windows.Forms.Label();
            this.cmbCHaMPSite = new System.Windows.Forms.ComboBox();
            this.lblWatershed = new System.Windows.Forms.Label();
            this.cmbWatershed = new System.Windows.Forms.ComboBox();
            this.grbUSGS_Gage = new System.Windows.Forms.GroupBox();
            this.txtUSGS_SiteNumber = new System.Windows.Forms.TextBox();
            this.lblManualUSGS_SiteNumber = new System.Windows.Forms.Label();
            this.lblOutputDirectory = new System.Windows.Forms.Label();
            this.cmbUSGS_Gage = new System.Windows.Forms.ComboBox();
            this.grbFigure.SuspendLayout();
            this.grbFilters.SuspendLayout();
            this.grbUSGS_Gage.SuspendLayout();
            this.SuspendLayout();
            // 
            // zedGraphControl
            // 
            this.zedGraphControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl.Location = new System.Drawing.Point(3, 16);
            this.zedGraphControl.Name = "zedGraphControl";
            this.zedGraphControl.ScrollGrace = 0D;
            this.zedGraphControl.ScrollMaxX = 0D;
            this.zedGraphControl.ScrollMaxY = 0D;
            this.zedGraphControl.ScrollMaxY2 = 0D;
            this.zedGraphControl.ScrollMinX = 0D;
            this.zedGraphControl.ScrollMinY = 0D;
            this.zedGraphControl.ScrollMinY2 = 0D;
            this.zedGraphControl.Size = new System.Drawing.Size(746, 315);
            this.zedGraphControl.TabIndex = 0;
            // 
            // cmdGetData
            // 
            this.cmdGetData.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdGetData.Location = new System.Drawing.Point(686, 164);
            this.cmdGetData.Name = "cmdGetData";
            this.cmdGetData.Size = new System.Drawing.Size(75, 23);
            this.cmdGetData.TabIndex = 1;
            this.cmdGetData.Text = "Get Data";
            this.cmdGetData.UseVisualStyleBackColor = true;
            this.cmdGetData.Click += new System.EventHandler(this.cmdGetData_Click);
            // 
            // grbFigure
            // 
            this.grbFigure.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbFigure.Controls.Add(this.zedGraphControl);
            this.grbFigure.Location = new System.Drawing.Point(12, 193);
            this.grbFigure.Name = "grbFigure";
            this.grbFigure.Size = new System.Drawing.Size(752, 334);
            this.grbFigure.TabIndex = 2;
            this.grbFigure.TabStop = false;
            this.grbFigure.Text = "Figure";
            // 
            // lblUSGS_StreamGageNumber
            // 
            this.lblUSGS_StreamGageNumber.AutoSize = true;
            this.lblUSGS_StreamGageNumber.Location = new System.Drawing.Point(15, 85);
            this.lblUSGS_StreamGageNumber.Name = "lblUSGS_StreamGageNumber";
            this.lblUSGS_StreamGageNumber.Size = new System.Drawing.Size(65, 13);
            this.lblUSGS_StreamGageNumber.TabIndex = 4;
            this.lblUSGS_StreamGageNumber.Text = "Site Number";
            // 
            // cmdOutputDirectory
            // 
            this.cmdOutputDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmdOutputDirectory.Image = global::CHaMPWorkbench.Properties.Resources.BrowseFolder;
            this.cmdOutputDirectory.Location = new System.Drawing.Point(715, 109);
            this.cmdOutputDirectory.Name = "cmdOutputDirectory";
            this.cmdOutputDirectory.Size = new System.Drawing.Size(25, 25);
            this.cmdOutputDirectory.TabIndex = 5;
            this.cmdOutputDirectory.UseVisualStyleBackColor = true;
            this.cmdOutputDirectory.Click += new System.EventHandler(this.cmdOutputDirectory_Click);
            // 
            // txtOutputDirectory
            // 
            this.txtOutputDirectory.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtOutputDirectory.Location = new System.Drawing.Point(107, 114);
            this.txtOutputDirectory.Name = "txtOutputDirectory";
            this.txtOutputDirectory.Size = new System.Drawing.Size(602, 20);
            this.txtOutputDirectory.TabIndex = 6;
            // 
            // grbFilters
            // 
            this.grbFilters.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbFilters.Controls.Add(this.lblSite);
            this.grbFilters.Controls.Add(this.cmbCHaMPSite);
            this.grbFilters.Controls.Add(this.lblWatershed);
            this.grbFilters.Controls.Add(this.cmbWatershed);
            this.grbFilters.Location = new System.Drawing.Point(9, 19);
            this.grbFilters.Name = "grbFilters";
            this.grbFilters.Size = new System.Drawing.Size(731, 57);
            this.grbFilters.TabIndex = 8;
            this.grbFilters.TabStop = false;
            this.grbFilters.Text = "CHaMP Filters";
            // 
            // lblSite
            // 
            this.lblSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSite.AutoSize = true;
            this.lblSite.Location = new System.Drawing.Point(382, 22);
            this.lblSite.Name = "lblSite";
            this.lblSite.Size = new System.Drawing.Size(25, 13);
            this.lblSite.TabIndex = 3;
            this.lblSite.Text = "Site";
            // 
            // cmbCHaMPSite
            // 
            this.cmbCHaMPSite.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbCHaMPSite.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCHaMPSite.FormattingEnabled = true;
            this.cmbCHaMPSite.Location = new System.Drawing.Point(424, 19);
            this.cmbCHaMPSite.Name = "cmbCHaMPSite";
            this.cmbCHaMPSite.Size = new System.Drawing.Size(301, 21);
            this.cmbCHaMPSite.TabIndex = 2;
            this.cmbCHaMPSite.SelectedIndexChanged += new System.EventHandler(this.cmbCHaMPSite_SelectedIndexChanged);
            // 
            // lblWatershed
            // 
            this.lblWatershed.AutoSize = true;
            this.lblWatershed.Location = new System.Drawing.Point(6, 22);
            this.lblWatershed.Name = "lblWatershed";
            this.lblWatershed.Size = new System.Drawing.Size(59, 13);
            this.lblWatershed.TabIndex = 1;
            this.lblWatershed.Text = "Watershed";
            // 
            // cmbWatershed
            // 
            this.cmbWatershed.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbWatershed.FormattingEnabled = true;
            this.cmbWatershed.Location = new System.Drawing.Point(98, 19);
            this.cmbWatershed.Name = "cmbWatershed";
            this.cmbWatershed.Size = new System.Drawing.Size(247, 21);
            this.cmbWatershed.TabIndex = 0;
            // 
            // grbUSGS_Gage
            // 
            this.grbUSGS_Gage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grbUSGS_Gage.Controls.Add(this.txtUSGS_SiteNumber);
            this.grbUSGS_Gage.Controls.Add(this.lblManualUSGS_SiteNumber);
            this.grbUSGS_Gage.Controls.Add(this.lblOutputDirectory);
            this.grbUSGS_Gage.Controls.Add(this.grbFilters);
            this.grbUSGS_Gage.Controls.Add(this.cmbUSGS_Gage);
            this.grbUSGS_Gage.Controls.Add(this.lblUSGS_StreamGageNumber);
            this.grbUSGS_Gage.Controls.Add(this.cmdOutputDirectory);
            this.grbUSGS_Gage.Controls.Add(this.txtOutputDirectory);
            this.grbUSGS_Gage.Location = new System.Drawing.Point(15, 12);
            this.grbUSGS_Gage.Name = "grbUSGS_Gage";
            this.grbUSGS_Gage.Size = new System.Drawing.Size(746, 146);
            this.grbUSGS_Gage.TabIndex = 9;
            this.grbUSGS_Gage.TabStop = false;
            this.grbUSGS_Gage.Text = "USGS Stream Gage";
            // 
            // txtUSGS_SiteNumber
            // 
            this.txtUSGS_SiteNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtUSGS_SiteNumber.Location = new System.Drawing.Point(581, 82);
            this.txtUSGS_SiteNumber.Name = "txtUSGS_SiteNumber";
            this.txtUSGS_SiteNumber.Size = new System.Drawing.Size(128, 20);
            this.txtUSGS_SiteNumber.TabIndex = 11;
            // 
            // lblManualUSGS_SiteNumber
            // 
            this.lblManualUSGS_SiteNumber.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblManualUSGS_SiteNumber.AutoSize = true;
            this.lblManualUSGS_SiteNumber.Location = new System.Drawing.Point(472, 86);
            this.lblManualUSGS_SiteNumber.Name = "lblManualUSGS_SiteNumber";
            this.lblManualUSGS_SiteNumber.Size = new System.Drawing.Size(103, 13);
            this.lblManualUSGS_SiteNumber.TabIndex = 10;
            this.lblManualUSGS_SiteNumber.Text = "Manual Site Number";
            // 
            // lblOutputDirectory
            // 
            this.lblOutputDirectory.AutoSize = true;
            this.lblOutputDirectory.Location = new System.Drawing.Point(15, 117);
            this.lblOutputDirectory.Name = "lblOutputDirectory";
            this.lblOutputDirectory.Size = new System.Drawing.Size(84, 13);
            this.lblOutputDirectory.TabIndex = 9;
            this.lblOutputDirectory.Text = "Output Directory";
            // 
            // cmbUSGS_Gage
            // 
            this.cmbUSGS_Gage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbUSGS_Gage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbUSGS_Gage.FormattingEnabled = true;
            this.cmbUSGS_Gage.Location = new System.Drawing.Point(107, 82);
            this.cmbUSGS_Gage.Name = "cmbUSGS_Gage";
            this.cmbUSGS_Gage.Size = new System.Drawing.Size(338, 21);
            this.cmbUSGS_Gage.TabIndex = 8;
            this.cmbUSGS_Gage.SelectedIndexChanged += new System.EventHandler(this.cmbUSGS_Gage_SelectedIndexChanged);
            // 
            // frmUSGS_StreamDataViewer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(776, 539);
            this.Controls.Add(this.grbUSGS_Gage);
            this.Controls.Add(this.grbFigure);
            this.Controls.Add(this.cmdGetData);
            this.MinimumSize = new System.Drawing.Size(792, 577);
            this.Name = "frmUSGS_StreamDataViewer";
            this.ShowIcon = false;
            this.Text = "USGS Stream Gage Data Viewer";
            this.grbFigure.ResumeLayout(false);
            this.grbFilters.ResumeLayout(false);
            this.grbFilters.PerformLayout();
            this.grbUSGS_Gage.ResumeLayout(false);
            this.grbUSGS_Gage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private ZedGraph.ZedGraphControl zedGraphControl;
        private System.Windows.Forms.Button cmdGetData;
        private System.Windows.Forms.GroupBox grbFigure;
        private System.Windows.Forms.Label lblUSGS_StreamGageNumber;
        private System.Windows.Forms.Button cmdOutputDirectory;
        private System.Windows.Forms.TextBox txtOutputDirectory;
        private System.Windows.Forms.GroupBox grbFilters;
        private System.Windows.Forms.Label lblSite;
        private System.Windows.Forms.ComboBox cmbCHaMPSite;
        private System.Windows.Forms.Label lblWatershed;
        private System.Windows.Forms.ComboBox cmbWatershed;
        private System.Windows.Forms.GroupBox grbUSGS_Gage;
        private System.Windows.Forms.ComboBox cmbUSGS_Gage;
        private System.Windows.Forms.Label lblOutputDirectory;
        private System.Windows.Forms.TextBox txtUSGS_SiteNumber;
        private System.Windows.Forms.Label lblManualUSGS_SiteNumber;
    }
}