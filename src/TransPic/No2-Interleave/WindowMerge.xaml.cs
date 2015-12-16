using System;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.Win32;
using System.IO;
using RS_Lib;

namespace RS_Diag
{
    /// <summary>
    /// WindowMerge.xaml 的交互逻辑
    /// </summary>
    public partial class WindowMerge : Window
    {
        public WindowMerge()
        {
            InitializeComponent();
        }

        private MergeBSQPic _mg;

        private void MergeMethod(String[] fn)
        {
            _mg = new MergeBSQPic(fn);

            if (_mg.GetAllPics() != null)
            {
                labelNumOfFiles.Content = _mg.GetNumOfFiles();
                buttonSaveMerge.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("文件打开失败！", "Gp.A FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void buttonChoose_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog { Multiselect = true, Filter = "ENVI遥感数据头文件(*.HDR)|*.HDR" };
            var s = ofd.ShowDialog();
            if (s != true)  return;

            MergeMethod(ofd.FileNames);
        }

        private void buttonSaveMerge_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "遥感图像文件(无扩展名)|*" };
            var s = sfd.ShowDialog();
            if (s != true) return;

            _mg.MergePic();

            if (MainWindow.WriteData(sfd.FileName, _mg.GetFinalPic()) |
                MainWindow.WriteData(sfd.FileName + ".HDR", Encoding.Default.GetBytes(_mg.BuildMetaData())))
            {
                MessageBox.Show("合并成功！", "OK", MessageBoxButton.OK, MessageBoxImage.Asterisk);
            }
            else
            {
                MessageBox.Show("合并失败！", "FAIL", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private void buttonChoose_Drop(object sender, DragEventArgs e)
        {
            String[] pPath = (String[])e.Data.GetData(DataFormats.FileDrop);
            if (pPath.Any(path => !File.Exists(path) || (path.Substring(path.LastIndexOf(".", StringComparison.Ordinal) + 1).ToUpper() != "HDR")))
            {
                MessageBox.Show("只接受HDR头文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            MergeMethod(pPath);
        }
    }
}
