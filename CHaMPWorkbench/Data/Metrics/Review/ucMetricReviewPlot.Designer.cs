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
            this.cboMetricSchemas = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.chtData)).BeginInit();
            this.SuspendLayout();
            // 
            // chtData
            // 
            this.chtData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chtData.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chtData.Legends.Add(legend1);
            this.chtData.Location = new System.Drawing.Point(0, 102);
            this.chtData.Name = "chtData";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chtData.Series.Add(series1);
            this.chtData.Size = new System.Drawing.Size(576, 485);
            this.chtData.TabIndex = 0;
            this.chtData.Text = "chart1";
            // 
            // cboYAxis
            // 
            this.cboYAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboYAxis.FormattingEnabled = true;
            this.cboYAxis.Location = new System.Drawing.Point(345, 68);
            this.cboYAxis.Name = "cboYAxis";
            this.cboYAxis.Size = new System.Drawing.Size(220, 21);
            this.cboYAxis.TabIndex = 15;
            this.cboYAxis.SelectedIndexChanged += new System.EventHandler(this.MetricCombo_SelectedIndexChanged);
            // 
            // cboXAxis
            // 
            this.cboXAxis.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboXAxis.FormattingEnabled = true;
            this.cboXAxis.Location = new System.Drawing.Point(97, 68);
            this.cboXAxis.Name = "cboXAxis";
            this.cboXAxis.Size = new System.Drawing.Size(201, 21);
            this.cboXAxis.TabIndex = 14;
            this.cboXAxis.SelectedIndexChanged += new System.EventHandler(this.MetricCombo_SelectedIndexChanged);
            // 
            // lblYAxis
            // 
            this.lblYAxis.AutoSize = true;
            this.lblYAxis.Location = new System.Drawing.Point(309, 72);
            this.lblYAxis.Name = "lblYAxis";
            this.lblYAxis.Size = new System.Drawing.Size(35, 13);
            this.lblYAxis.TabIndex = 13;
            this.lblYAxis.Text = "Y axis";
            // 
            // lblXAxis
            // 
            this.lblXAxis.AutoSize = true;
            this.lblXAxis.Location = new System.Drawing.Point(55, 72);
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
            this.cboPlotTypes.Location = new System.Drawing.Point(97, 37);
            this.cboPlotTypes.Name = "cboPlotTypes";
            this.cboPlotTypes.Size = new System.Drawing.Size(468, 21);
            this.cboPlotTypes.TabIndex = 11;
            this.cboPlotTypes.SelectedIndexChanged += new System.EventHandler(this.cboPlotTypes_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 40);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Plot types";
            // 
            // cboMetricSchemas
            // 
            this.cboMetricSchemas.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.cboMetricSchemas.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboMetricSchemas.FormattingEnabled = true;
            this.cboMetricSchemas.Location = new System.Drawing.Point(97, 3);
            this.cboMetricSchemas.Name = "cboMetricSchemas";
            this.cboMetricSchemas.Size = new System.Drawing.Size(468, 21);
            this.cboMetricSchemas.TabIndex = 17;
            this.cboMetricSchemas.SelectedIndexChanged += new System.EventHandler(this.cboSchemaType_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 6);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(81, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Metric schemas";
            // 
            // ucMetricReviewPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.cboMetricSchemas);
            this.Controls.Add(this.label1);
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
        private System.Windows.Forms.ComboBox cboMetricSchemas;
        private System.Windows.Forms.Label label1;
    }
}
