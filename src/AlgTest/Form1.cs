using System;
using System.Windows.Forms;
using RS_Lib;

namespace AlgTest
{
    /// <summary>
    /// 验证算法正确性-发布时忽略
    /// </summary>
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            byte[,] data = { { 1, 2, 4 }, { 4, 3, 5 }, { 5, 3, 6 } };
            RS_Lib.Contrast ct = new Contrast(data);
            textBox1.Text = ct.GetImageContrast().ToString();

            byte[,] ttt = new byte[7, 7] { { 0, 1, 2, 3, 0, 1, 2 }, { 1, 2, 3, 0, 1, 2, 3 }, { 2, 3, 0, 1, 2, 3, 0 }, { 3, 0, 1, 2, 3, 0, 1 }, { 0, 1, 2, 3, 0, 1, 2 }, { 1, 2, 3, 0, 1, 2, 3 }, { 2, 3, 0, 1, 2, 3, 0 } };
            GLCM g = new GLCM(ttt, 1, 0);
            g.GetGLCM();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MessageBox.Show(Int32.MaxValue.ToString());
        }
    }
}
