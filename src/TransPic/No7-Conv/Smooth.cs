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
    public partial class Smooth : Form
    {
        public int Hang { get; private set; }
        public int Lie { get; private set; }
        public int Method { get; private set; }
        public int O { get; private set; }

        public Smooth()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            this.Method = 0;
            this.Hang = 3;
            this.Lie = 3;
        }

        private void numHang_ValueChanged(object sender, EventArgs e)
        {
            if (int.Parse(numHang.Value.ToString()) < 3)
                numHang.Value = 3;
            this.Hang = int.Parse(numHang.Value.ToString());
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
            if (comboBox1.SelectedIndex == 2)
            {
                textBox2.Visible = true;
                MessageBox.Show("请输入方差，用于控制平滑度", "TonyZ", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                textBox2.Visible = false;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                case 2:
                    try
                    {
                        this.O = int.Parse(textBox2.Text);
                        textBox1.Text = RS_Lib.GaussLow.ShowKernel(Hang, Lie, this.O);
                    }
                    catch (Exception)
                    {
                        MessageBox.Show("输入错误", "Tony", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        return;
                    }
                    
                    break;

                case 0:
                    textBox1.Text = "0.125, 0.125, 0.125\r\n0.125, 0, 0.125\r\n0.125, 0.125, 0.125\r\n";
                    break;

                case 1:
                case 3:
                    textBox1.Text = "此方法无可编辑卷积核";
                    break;

                default:
                    MessageBox.Show("选择错误！", "TonyZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
            }
        }

        private void numLie_ValueChanged(object sender, EventArgs e)
        {
            if (int.Parse(numLie.Value.ToString()) < 3)
                numLie.Value = 3;
            this.Lie = int.Parse(numLie.Value.ToString());
        }
    }
}
