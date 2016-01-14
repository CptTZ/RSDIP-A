using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RS_Diag
{
    public partial class HistoMatch : Form
    {
        public byte[,] MatchedData { get; private set; }

        private readonly RS_Lib.RsImage _a, _b;

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Process();
            this.DialogResult = DialogResult.OK;

            this.Close();
        }

        public HistoMatch(RS_Lib.RsImage a, RS_Lib.RsImage b)
        {
            this._a = a;
            this._b = b;
            InitializeComponent();

            InitCombo();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
        }

        private void InitCombo()
        {
            string[] a = new string[_a.BandsCount];
            string[] b = new string[_b.BandsCount];

            for (int i = 0; i < a.Length; i++)
            {
                a[i] = "Band " + (i + 1).ToString();
            }

            for (int i = 0; i < b.Length; i++)
            {
                b[i] = "Band " + (i + 1).ToString();
            }

            comboBox1.DataSource = a;
            comboBox2.DataSource = b;
        }

        private void Process()
        {
            MatchedData = new RS_Lib.HistoMatch(_a.GetPicData(comboBox1.SelectedIndex + 1),
                _b.GetPicData(comboBox2.SelectedIndex + 1)).MatchedData;
        }

    }
}
