using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RS_Diag.Basic
{
    public partial class FileChoose : UserControl
    {
        /// <summary>
        /// 获取选择的ID
        /// </summary>
        public int ChoosedFile
        {
            get
            {
                if (files.Nodes.Count == 0) return -2;
                if (files.SelectedNode == null) return -1;
                return files.SelectedNode.Index;
            }
        }
        
        public void AddByMemoryFileName(string n)
        {
            files.Nodes.Add("Memory: " + n);
        }

        public void AddByFilePath(string n)
        {
            try
            {
                System.IO.FileInfo t = new System.IO.FileInfo(n);
                string info = t.Name + ": " + t.Directory;
                files.Nodes.Add(info);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Runtime Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void RemoveByID(int i)
        {
            files.Nodes.RemoveAt(i);   
        }

        public void RemoveAll()
        {
            files.Nodes.Clear();
        }

        public FileChoose()
        {
            InitializeComponent();
        }
    }
}
