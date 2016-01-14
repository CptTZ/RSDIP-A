using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RS_Diag
{
    public partial class GLCM : UserControl
    {

        private readonly RS_Lib.RsImage _img;

        private void InitCombo()
        {
            string[] str = new string[_img.BandsCount];

            for (int i = 0; i < str.Length; i++)
            {
                str[i] = "Band " + (i + 1).ToString();
            }

            comboBox1.DataSource = str;
        }

        public GLCM(RS_Lib.RsImage r)
        {
            InitializeComponent();
            this._img = r;
            InitCombo();
            comboBox1.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                int dx = int.Parse(textBox2.Text);
                int dy = int.Parse(textBox1.Text);

                if (dx > _img.Samples | dy > _img.Lines)
                    throw new ArgumentException("输入错误");

                RS_Lib.GLCM gl = new RS_Lib.GLCM(_img.GetPicData(comboBox1.SelectedIndex + 1), dx, dy);

                var data = gl.GLCMdata;

                Bitmap bit = new Bitmap(256, 256);

                for (int i = 0; i < 256; i++)
                {
                    for (int j = 0; j < 256; j++)
                    {
                        bit.SetPixel(i, j, Color.FromArgb(data[i, j], data[i, j], data[i, j]));
                    }
                }

                pictureBox1.Image = bit;

            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(ex.Message, "TonyZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            catch (Exception)
            {
                MessageBox.Show("运算错误！", "TonyZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
        }
    }
}
