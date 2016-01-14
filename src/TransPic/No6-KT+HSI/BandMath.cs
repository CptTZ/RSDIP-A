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
    public partial class BandMath : Form
    {
        public string StateMent { get; private set; }
        private readonly int _tot;
        private readonly List<RS_Lib.RsImage> _img;
        private readonly List<byte[,]> _dat = new List<byte[,]>(); 
        public List<byte[,]> Data { get; private set; }
        public List<string> CtrlStr { get; private set; }

        public BandMath(List<RS_Lib.RsImage> img)
        {
            this._img = img;
            InitializeComponent();
            _tot = InitDGV();
        }

        private int InitDGV()
        {
            int count = 1;

            foreach (var tt in _img)
            {
                for (int i = 0; i < tt.BandsCount; i++)
                {
                    dataGridView1.Rows.Add(new DataGridViewRow());
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[0].Value = "b" + count++.ToString();
                    dataGridView1.Rows[dataGridView1.RowCount - 1].Cells[1].Value = tt.FileName + "-Band" +
                                                                                    (i + 1).ToString();
                    _dat.Add(tt.GetPicData(i + 1));
                }
            }

            return count;
        }

        private void Calc()
        {
            CtrlStr = new List<string>();
            Data = new List<byte[,]>(); 

            for (int i = 1; i <= _tot ; i++)
            {
                if (StateMent.Contains("b" + i.ToString()))
                {
                    CtrlStr.Add("b" + i.ToString());
                    Data.Add(_dat[i - 1]);
                }
            }

            if (CtrlStr.Count == 0) 
                throw new ArgumentException("算式输入错误！");

        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.StateMent = textBox1.Text;
            Calc();
            this.DialogResult = DialogResult.OK;
            this.Close();
        }
    }
}
