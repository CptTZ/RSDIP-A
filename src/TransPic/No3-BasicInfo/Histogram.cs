using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using RS_Lib;

namespace RS_Diag
{
    public partial class Histogram : UserControl
    {
        private readonly RsImage _image;

        /// <summary>
        /// 直方图种类：1、普通；2、累计
        /// </summary>
        private readonly byte _type;

        public Histogram(RsImage p,byte type)
        {
            InitializeComponent();
            _image = p;
            _type = type;
            var bandName = new string[p.BandsCount];
            for (int i = 0; i < p.BandsCount; i++)
            {
                bandName[i] = "Band " + (i + 1);
            }
            bandList.DataSource = bandName;
        }

        public void MadeChart(Chart chart, int band, int[] chartData)
        {
            Series series = new Series {ChartType = SeriesChartType.SplineArea};
            Title title;
            switch (_type)
            {
                case 2:
                    title = new Title
                    {
                        Text = "遥感图像累计直方图 · 波段：" + band,
                        TextStyle = TextStyle.Shadow,
                        Font = new Font(new FontFamily("微软雅黑"), 10)
                    };
                    break;

                case 1:
                default:
                    title = new Title
                    {
                        Text = "遥感图像直方图 · 波段：" + band,
                        TextStyle = TextStyle.Shadow,
                        Font = new Font(new FontFamily("微软雅黑"), 10)
                    };
                    break;
            }

            for (int i = 1; i < chartData.Length; i++)
            {
                series.Points.AddXY(i, chartData[i]);
            }

            chart.Series.Add(series);
            chart.Titles.Add(title);
            chart.Refresh();
        }

        private void Histogram_Load(object sender, EventArgs e)
        {
            DrawByBand(1);
        }

        /// <summary>
        /// 按照波段号画图
        /// </summary>
        /// <param name="band">波段号</param>
        private void DrawByBand(int band)
        {
            chart1.Series.Clear();
            chart1.Titles.Clear();
            HistoData histo = new HistoData(_image, band);

            if (_type==1)
            {
                chart1.ChartAreas[0].AxisY.Maximum = histo.GetHistogramData().Max() + 1000;
                MadeChart(chart1, band, histo.GetHistogramData());
            }
            else
            {
                chart1.ChartAreas[0].AxisY.Maximum = histo.GetAccHistogramData().Max() + 1000;
                MadeChart(chart1, band, histo.GetAccHistogramData());
            }
            
        }

        private void bandList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawByBand(bandList.SelectedIndex + 1);
        }

    }
}
