using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Data;

namespace RS_Diag
{
    class StatsCorr
    {
        public double CorrValue { get; set; }

        public StatsCorr(double a)
        {
            CorrValue = a;
        }
    }

    class StatsName
    {
        public String BandName { get; set; }

        public StatsName(String a)
        {
            BandName = a;
        }
    }

    class StatStdDev
    {
        public String BandName { get; set; }
        public Double DevValue { get; set; }

        public StatStdDev(String a, double b)
        {
            BandName = a;
            DevValue = b;
        }
    }

    class StatAve
    {
        public StatAve(String a, double b)
        {
            BandName = a;
            AveValue = b;
        }

        public String BandName { get; set; }
        public Double AveValue { get; set; }
    }


    /// <summary>
    /// BasicStats.xaml 的交互逻辑
    /// </summary>
    public partial class BasicStats : UserControl
    {
        /// <summary>
        /// 图像统计类
        /// </summary>
        private readonly RS_Lib.ImageStats _stats;

        private readonly List<String> _bandName = new List<String>();

        private readonly ObservableCollection<StatAve> _statAve = 
            new ObservableCollection<StatAve>();

        private readonly ObservableCollection<StatStdDev> _statDev =
            new ObservableCollection<StatStdDev>();

        private readonly ObservableCollection<StatsName> _statName =
            new ObservableCollection<StatsName>();

        // private ObservableCollection<StatsCorr>[] _statCorr;

        public BasicStats(RS_Lib.RsImage p)
        {
            InitializeComponent();
            _stats = new RS_Lib.ImageStats(p);
            TextInfo.Text += "\n\t图像数据位置：" + p.GetFilePath();
            TextInfo.Text += "\n\t波段数目：" + p.BandsCount;
            TextInfo.Text += "\n\t图像尺寸(高x宽)：" + p.Lines + " x " + p.Samples + "\n";

            for (int i = 0; i < p.BandsCount; i++)
            {
                _bandName.Add("波段 " + (i + 1));
            }

            DataAve.ItemsSource = _statAve;
            DataDev.ItemsSource = _statDev;

            Ave();
            StdDev();
            StdCorr();
            StdCov();
        }

        private void StdCov()
        {
            StringBuilder sb = new StringBuilder("协方差    ");
            foreach (var s in _bandName)
            {
                sb.AppendFormat("{0}\t", s);
            }
            sb.AppendLine();
            double[,] data = _stats.Covariance;
            for (int i = 0; i < _bandName.Count; i++)
            {
                sb.AppendFormat("{0}\t", _bandName[i]);
                for (int j = 0; j < _bandName.Count; j++)
                {
                    sb.AppendFormat("{0}\t", Math.Round(data[i, j], 3));
                }
                sb.AppendLine();
            }

            TextCov.Text = sb.ToString();
        }

        private void StdCorr()
        {
            StringBuilder sb = new StringBuilder("相关度    ");
            foreach (var s in _bandName)
            {
                sb.AppendFormat("{0}\t", s);
            }
            sb.AppendLine();
            double[,] data = _stats.Correlation;
            for (int i = 0; i < _bandName.Count; i++)
            {
                sb.AppendFormat("{0}\t", _bandName[i]);
                for (int j = 0; j < _bandName.Count; j++)
                {
                    sb.AppendFormat("{0}\t", Math.Round(data[i, j],3));
                }
                sb.AppendLine();
            }

            TextCorr.Text = sb.ToString();
        }

        private void StdDev()
        {
            for (int i = 0; i < _bandName.Count; i++)
            {
                _statDev.Add(new StatStdDev(_bandName[i], _stats.StdDev[i]));
            }
        }

        private void Ave()
        {
            for (int i = 0; i < _bandName.Count; i++)
            {
                _statAve.Add(new StatAve(_bandName[i], _stats.Mean[i]));
            }
        }
    }
}
