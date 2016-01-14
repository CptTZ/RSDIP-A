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
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("图像元信息如下");
            sb.AppendLine();
            sb.AppendLine("图像文件地址：" + p.GetFilePath());
            sb.AppendLine("波段数：" + p.BandsCount);
            sb.AppendLine("图像行数：" + p.Lines);
            sb.AppendLine("图像列数：" + p.Samples);
            sb.AppendLine("图像格式：" + p.Interleave);
            sb.AppendLine();
            sb.AppendLine("图像说明：" + p.Description);
            
            textBlock1.Text = sb.ToString();
        }

    }
}
