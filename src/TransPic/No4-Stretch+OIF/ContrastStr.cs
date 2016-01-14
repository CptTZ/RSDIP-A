using System;
using System.Windows.Forms;

namespace RS_Diag
{
    public partial class ContrastStr : Form
    {
        public double LeftValue { get; private set; }

        public double RightValue { get; private set; }

        public int[] BandList { get; private set; }

        private readonly RS_Lib.RsImage _img;

        private void InitCheck()
        {
            for (int i = 0; i < _img.BandsCount; i++)
            {
                checkedListBox1.Items.Add("Band " + (i + 1).ToString(), false);
            }
        }
        
        public ContrastStr(RS_Lib.RsImage r)
        {
            this._img = r;
            InitializeComponent();
            InitCheck();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;

            try
            {
                LeftValue = double.Parse(textBox1.Text);
                RightValue = double.Parse(textBox2.Text);
                BandList = new int[checkedListBox1.CheckedItems.Count];

                for (int i = 0; i < BandList.Length; i++)
                {
                    BandList[i] = checkedListBox1.CheckedIndices[i];
                }
            }
            catch (Exception)
            {
                MessageBox.Show("数值输入错误", "TonyZ", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.DialogResult = DialogResult.Abort;
                return;
            }

            this.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
