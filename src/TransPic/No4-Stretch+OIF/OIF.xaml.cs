using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace RS_Diag
{
    class CalcOIF
    {
        public string BandGroup { get; private set; }
        public double OIFValue { get; private set; }

        public CalcOIF(string s,double k)
        {
            BandGroup = s;
            OIFValue = k;
        }
    }

    /// <summary>
    /// OIF.xaml 的交互逻辑
    /// </summary>
    public partial class OIF : UserControl
    {

        private readonly RS_Lib.CalcOIF _oif;

        private readonly ObservableCollection<CalcOIF> _calcOif =
            new ObservableCollection<CalcOIF>();


        public OIF(RS_Lib.RsImage l)
        {
            InitializeComponent();
            _oif = new RS_Lib.CalcOIF(l);

            oif.ItemsSource = _calcOif;
            InitData();
        }

        private void InitData()
        {
            var data = _oif.ResultOIF;
            foreach (string[] t in data)
            {
                _calcOif.Add(new CalcOIF(t[0], double.Parse(t[1])));
            }
        }
    }
}
