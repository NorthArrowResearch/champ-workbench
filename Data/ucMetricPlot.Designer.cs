namespace CHaMPWorkbench.Data
{
    partial class ucMetricPlot
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
            this.label2 = new System.Windows.Forms.Label();
            this.cboPlotTypes = new System.Windows.Forms.ComboBox();
            this.cboModelResults = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.chtData = new System.Windows.Forms.DataVisualization.Charting.Chart();
            ((System.ComponentModel.ISupportInitialize)(this.chtData)).BeginInit();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(29, 16);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Plot types";
            // 
            // cboPlotTypes
            // 
            this.cboPlotTypes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboPlotTypes.FormattingEnabled = true;
            this.cboPlotTypes.Location = new System.Drawing.Point(93, 13);
            this.cboPlotTypes.Name = "cboPlotTypes";
            this.cboPlotTypes.Size = new System.Drawing.Size(381, 21);
            this.cboPlotTypes.TabIndex = 2;
            // 
            // cboModelResults
            // 
            this.cboModelResults.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cboModelResults.FormattingEnabled = true;
            this.cboModelResults.Location = new System.Drawing.Point(93, 40);
            this.cboModelResults.Name = "cboModelResults";
            this.cboModelResults.Size = new System.Drawing.Size(381, 21);
            this.cboModelResults.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(69, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Model results";
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
            this.chtData.Location = new System.Drawing.Point(0, 67);
            this.chtData.Name = "chtData";
            series1.ChartArea = "ChartArea1";
            series1.Legend = "Legend1";
            series1.Name = "Series1";
            this.chtData.Series.Add(series1);
            this.chtData.Size = new System.Drawing.Size(790, 413);
            this.chtData.TabIndex = 5;
            this.chtData.Text = "chart1";
            // 
            // ucMetricPlot
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.chtData);
            this.Controls.Add(this.cboModelResults);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cboPlotTypes);
            this.Controls.Add(this.label2);
            this.Name = "ucMetricPlot";
            this.Size = new System.Drawing.Size(790, 480);
            this.Load += new System.EventHandler(this.ucMetricPlot_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chtData)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cboPlotTypes;
        private System.Windows.Forms.ComboBox cboModelResults;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.DataVisualization.Charting.Chart chtData;
    }
}
