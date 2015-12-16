using System;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using RS_Lib;
using DataFormats = System.Windows.DataFormats;
using DragDropEffects = System.Windows.DragDropEffects;
using DragEventArgs = System.Windows.DragEventArgs;
using MessageBox = System.Windows.MessageBox;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;

namespace RS_Diag
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : UserControl
    {
        // 遥感图像类
        private readonly RsImage _image = null;

        public MainWindow(RsImage p)
        {
            InitializeComponent();
            this._image = p;
            this.textOpenLoc.Text = p.GetFilePath();
            SetRadioVisibility(p.Interleave);
        }

        // 二进制数据写入到文件
        public static bool WriteData(string path, byte[] data)
        {
            FileStream fs = null;
            try
            {
                fs = new FileStream(path, FileMode.Create);
                fs.Write(data, 0, data.Length);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message, e.Source, MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            finally
            {
                fs?.Close();
            }
            return true;
        }

        private void buttonSaveAs_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog { Filter = "遥感图像文件(无扩展名)|*" };
            var s = sfd.ShowDialog();
            if (s!=true) return;

            textSaveLoc.IsReadOnly = true;
            textSaveLoc.Text = sfd.FileName;
            buttonTrans.IsEnabled = true;
        }

        private void buttonTrans_Click(object sender, RoutedEventArgs e)
        {
            int cho = GetRadioChoice();
            if (String.IsNullOrWhiteSpace(textSaveLoc.Text))
            {
                MessageBox.Show("请先输入输出路径！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                textSaveLoc.Focus();
                return;
            }
            if (cho == 0)
            {
                MessageBox.Show("请选择转换格式", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            byte[] converData = null;
            if (_image.GetPicData() == null) 
            {
                MessageBox.Show("遥感图像读取错误！请检查文件情况！", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            PicConvert dc = new PicConvert(_image, cho);
            if ((converData = dc.GetConvertedData()) != null)
            {
                byte[] converHead = Encoding.Default.GetBytes(_image.BuildMetaData(dc.GetTargetFormat()));
                WriteData(textSaveLoc.Text.Trim(), converData);
                WriteData(textSaveLoc.Text.Trim() + ".HDR", converHead);

                MessageBox.Show("数据转换完成！", "OK", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("数据转换失败！", "OK", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }

        private int GetRadioChoice()
        {
            if (radioButtonBIP.IsChecked != null && radioButtonBIP.IsChecked.Value)
            {
                return 2;
            }

            if (radioButtonBIL.IsChecked != null && radioButtonBIL.IsChecked.Value)
            {
                return 3;
            }

            if (radioButtonBSQ.IsChecked != null && radioButtonBSQ.IsChecked.Value)
            {
                return 1;
            }

            return 0;
        }

        private void SetRadioVisibility(string datatype)
        {
            radioButtonBIL.Visibility = Visibility.Visible;
            radioButtonBSQ.Visibility = Visibility.Visible;
            radioButtonBIP.Visibility = Visibility.Visible;
            if (datatype == "bil")
            {
                radioButtonBIL.Visibility = Visibility.Hidden;
                radioButtonBIL.IsChecked = false;
            }
            if (datatype == "bip")
            {
                radioButtonBIP.Visibility = Visibility.Hidden;
                radioButtonBIP.IsChecked = false;
            }
            if (datatype == "bsq")
            {
                radioButtonBSQ.Visibility = Visibility.Hidden;
                radioButtonBSQ.IsChecked = false;
            }
        }

        private void textOpenLoc_PreviewDragOver(object sender, DragEventArgs e)
        {
            e.Effects = DragDropEffects.Link;
            e.Handled = true;
        }

        private void textOpenLoc_PreviewDrop(object sender, DragEventArgs e)
        {
            String[] path = (String[])e.Data.GetData(DataFormats.FileDrop);
            if(path.Length>1)
            {
                MessageBox.Show("一次只可以拖放一个文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }
            // 防止拖文件夹过来
            if (!File.Exists(path[0]) || (path[0].Substring(path[0].LastIndexOf(".") + 1).ToUpper() != "HDR")) 
            {
                MessageBox.Show("只接受HDR头文件！", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            textOpenLoc.Text = path[0];
        }

    }
}
