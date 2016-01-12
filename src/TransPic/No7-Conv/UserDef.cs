using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using RS_Lib;

namespace RS_Diag
{
    public partial class UserDef : Form
    {
        private int _hang = 3, _lie = 3;
        public double[,] Kernel { get; private set; }

        public UserDef()
        {
            InitializeComponent();
            this.DialogResult = DialogResult.Ignore;
            UpdateData();
            Put1();
        }

        private void Put1()
        {
            for (int i = 0; i < _hang; i++)
            {
                for (int j = 0; j < _lie; j++)
                {
                    kernData.Rows[i].Cells[j].Value = "1.0";
                }
            }
        }

        private void UpdateData()
        {
            if (kernData.ColumnCount != _lie)
            {
                kernData.Columns.Clear();

                for (int i = 0; i < _lie; i++)
                {
                    DataGridViewColumn dgc = new DataGridViewTextBoxColumn
                    {
                        HeaderText = (i + 1).ToString(),
                        Width = 60
                    };
                    kernData.Columns.Add(dgc);
                }
            }

            if (kernData.RowCount != _hang)
            {
                kernData.Rows.Clear();

                for (int i = 0; i < _hang; i++)
                {
                    DataGridViewRow dgr = new DataGridViewRow
                    {
                        HeaderCell = {Value = (i + 1).ToString()}
                    };
                    kernData.Rows.Add(dgr);
                }
            }

            Put1();
        }

        private void numericUpDown2_ValueChanged(object sender, EventArgs e)
        {
            if (int.Parse(numLie.Value.ToString()) < 3)
                numLie.Value = 3;
            this._lie = int.Parse(numLie.Value.ToString());

            UpdateData();
        }

        private void numericUpDown1_ValueChanged(object sender, EventArgs e)
        {
            if (int.Parse(numHang.Value.ToString()) < 3)
                numHang.Value = 3;
            this._hang = int.Parse(numHang.Value.ToString());

            UpdateData();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        #region 只给数字
        private void kernData_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            TextBox tx = e.Control as TextBox;
            tx.KeyPress -= new KeyPressEventHandler(tx_KeyPress);
            tx.KeyPress += new KeyPressEventHandler(tx_KeyPress);
        }

        private void tx_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!(char.IsNumber(e.KeyChar) || e.KeyChar == '.' || e.KeyChar == '\b' || e.KeyChar == '-'))
                e.Handled = true;
        }
        #endregion

        private void button2_Click(object sender, EventArgs e)
        {
            if (MakeKernel() == false) return;

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private bool MakeKernel()
        {
            this.Kernel = new double[_hang, _lie];

            try
            {
                for (int i = 0; i < _hang; i++)
                {
                    for (int j = 0; j < _lie; j++)
                    {
                        var s = kernData.Rows[i].Cells[j].Value as string;
                        this.Kernel[i, j] = double.Parse(s);
                    }
                }
            }
            catch (Exception es)
            {
                MessageBox.Show(es.Message, "TonyZ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            return true;
        }

    }
}
