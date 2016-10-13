namespace CHaMPWorkbench.Data
{
    partial class ucMetricReviewPlot
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chtData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cboYAxis = new System.Windows.Forms.ComboBox();
            this.cboXAxis = new System.Windows.Forms.ComboBox();
            this.lblYAxis = new System.Windows.Forms.Label();
            this.lblXAxis = new System.Windows.Forms.Label();
            this.cboPlotTypes = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chtData)).BeginInit();
            this.SuspendLayout();
            // 
            // chtData
            // 
            chartArea1.Name = "ChartArea1";
            this.chtData.ChartAreas.Add(chartArea1);
            this.chtData.Dock = System.Windows.Forms.DockStyle.Bottom;
            legend1.Name = "Legend1";
            this.chtData.Legends.Add(legend1);
            this.chtData.Location = new System.Drawing.Point(0, 73);
            this.chtData.Name = "chtData";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chtData.Series.Add(series1);
            this.chtData.Size = new System.Drawing.Size(576, 514);
            this.chtData.TabIndex = 0;
            this.chtData.Text = "chart1";
            // 
            // cboYAxis
            // 
            this.cboYAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYAxis.FormattingEnabled = true;
            this.cboYAxis.Location = new System.Drawing.Point(345, 40);
            this.cboYAxis.Name = "cboYAxis";
            this.cboYAxis.Size = new System.Drawing.Size(220, 21);
            this.cboYAxis.TabIndex = 15;
            // 
            // cboXAxis
            // 
            this.cboXAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboXAxis.FormattingEnabled = true;
            this.cboXAxis.Location = new System.Drawing.Point(78, 40);
            this.cboXAxis.Name = "cboXAxis";
            this.cboXAxis.Size = new System.Drawing.Size(220, 21);
            this.cboXAxis.TabIndex = 14;
            // 
            // lblYAxis
            // 
            this.lblYAxis.AutoSize = true;
            this.lblYAxis.Location = new System.Drawing.Point(309, 44);
            this.lblYAxis.Name = "lblYAxis";
            this.lblYAxis.Size = new System.Drawing.Size(35, 13);
            this.lblYAxis.TabIndex = 13;
            this.lblYAxis.Text = "Y axis";
            // 
            // lblXAxis
            // 
            this.lblXAxis.AutoSize = true;
            this.lblXAxis.Location = new System.Drawing.Point(42, 44);
            this.lblXAxis.Name = "lblXAxis";
            this.lblXAxis.Size = new System.Drawing.Size(35, 13);
            this.lblXAxis.TabIndex = 12;
            this.lblXAxis.Text = "X axis";
            // 
            // cboPlotTypes
            // 
            this.cboPlotTypes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboPlotTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlotTypes.FormattingEnabled = true;
            this.cboPlotTypes.Location = new System.Drawing.Point(78, 9);
            this.cboPlotTypes.Name = "cboPlotTypes";
            this.cboPlotTypes.Size = new System.Drawing.Size(487, 21);
            this.cboPlotTypes.TabIndex = 11;
            this.cboPlotTypes.SelectedIndexChanged += new System.EventHandler(this.cboPlotTypes_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(14, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Plot types";
            // 
            // ucMetricReviewPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboYAxis);
            this.Controls.Add(this.cboXAxis);
            this.Controls.Add(this.lblYAxis);
            this.Controls.Add(this.lblXAxis);
            this.Controls.Add(this.cboPlotTypes);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.chtData);
            this.Name = "ucMetricReviewPlot";
            this.Size = new System.Drawing.Size(576, 587);
            this.Load += new System.EventHandler(this.ucMetricReviewPlot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chtData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chtData;
        private System.Windows.Forms.ComboBox cboYAxis;
        private System.Windows.Forms.ComboBox cboXAxis;
        private System.Windows.Forms.Label lblYAxis;
        private System.Windows.Forms.Label lblXAxis;
        private System.Windows.Forms.ComboBox cboPlotTypes;
        private System.Windows.Forms.Label label2;
    }
}
