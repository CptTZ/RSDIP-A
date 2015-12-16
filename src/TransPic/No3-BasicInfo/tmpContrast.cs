using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace RS_Diag
{
    public partial class tmpContrast : UserControl
    {
        private readonly double[] _data;

        public tmpContrast(double[] data)
        {
            InitializeComponent();
            _data = data;
        }

        private void tmpContrast_Load(object sender, EventArgs e)
        {
            Series sr = new Series("对比度") {ChartType = SeriesChartType.Line};
            Title t = new Title("1-7波段对比度")
            {
                TextStyle = TextStyle.Shadow,
                Font = new Font(new FontFamily("微软雅黑"), 15)
            };

            for (int i = 0; i < _data.Length; i++)
            {
                sr.Points.AddXY(i + 1, _data[i]);
            }

            chart1.Series.Add(sr);
            chart1.Titles.Add(t);
            chart1.Refresh();
        }
    }
}
