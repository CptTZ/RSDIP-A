using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace RS_Diag
{
    /// <summary>
    /// ShowImage.xaml 的交互逻辑
    /// </summary>
    public partial class FileInfo : UserControl
    {
        private readonly RS_Lib.RsImage _img;

        public FileInfo(RS_Lib.RsImage p)
        {
            InitializeComponent();
            _img = p;
            textBlock1.Text += "\n图像文件地址：" + p.GetFilePath();
            textBlock1.Text += "\n波段数：" + p.BandsCount;
            textBlock1.Text += "\n图像说明：" + p.Description;
            textBlock1.Text += "\n图像格式：" + p.Interleave;

        }

    }
}
