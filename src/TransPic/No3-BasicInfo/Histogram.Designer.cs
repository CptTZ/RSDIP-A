namespace RS_Diag
{
    partial class Histogram
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.BandChoser = new System.Windows.Forms.GroupBox();
            this.bandList = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            this.BandChoser.SuspendLayout();
            this.SuspendLayout();
            // 
            // chart1
            // 
            this.chart1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            this.chart1.Location = new System.Drawing.Point(0, 0);
            this.chart1.Margin = new System.Windows.Forms.Padding(0);
            this.chart1.Name = "chart1";
            this.chart1.Size = new System.Drawing.Size(552, 458);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // BandChoser
            // 
            this.BandChoser.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.BandChoser.Controls.Add(this.bandList);
            this.BandChoser.Location = new System.Drawing.Point(555, 36);
            this.BandChoser.Name = "BandChoser";
            this.BandChoser.Size = new System.Drawing.Size(134, 320);
            this.BandChoser.TabIndex = 1;
            this.BandChoser.TabStop = false;
            this.BandChoser.Text = "选择波段";
            // 
            // bandList
            // 
            this.bandList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.bandList.FormattingEnabled = true;
            this.bandList.Location = new System.Drawing.Point(8, 54);
            this.bandList.Margin = new System.Windows.Forms.Padding(5);
            this.bandList.Name = "bandList";
            this.bandList.Size = new System.Drawing.Size(118, 32);
            this.bandList.TabIndex = 0;
            this.bandList.SelectedIndexChanged += new System.EventHandler(this.bandList_SelectedIndexChanged);
            // 
            // Histogram
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 24F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.BandChoser);
            this.Controls.Add(this.chart1);
            this.MinimumSize = new System.Drawing.Size(700, 400);
            this.Name = "Histogram";
            this.Size = new System.Drawing.Size(844, 618);
            this.Load += new System.EventHandler(this.Histogram_Load);
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            this.BandChoser.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.GroupBox BandChoser;
        private System.Windows.Forms.ComboBox bandList;
    }
}
