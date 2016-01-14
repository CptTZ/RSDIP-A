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
    public partial class Sharp : Form
    {
        public int Method { get; private set; }

        public Sharp()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            this.Method = 0;
        }

        private void numHang_ValueChanged(object sender, EventArgs e)
        {
            numHang.Value = 3;
            UpdateKernel();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Method = comboBox1.SelectedIndex;
            UpdateKernel();
        }

        private void UpdateKernel()
        {
            switch (comboBox1.SelectedIndex)
            {
                case 2:
                    textBox1.Text = "0, -1, 0\r\n-1, 4, -1\r\n0, -1, 0\r\n";
                    break;

                case 0:
                case 1:
                    textBox1.Text = "此方法无可直接表达的卷积核，分别见书P168-169";
                    break;
            }
        }

        private void numLie_ValueChanged(object sender, EventArgs e)
        {
            numLie.Value = 3;
            UpdateKernel();
        }
        
    }
}
