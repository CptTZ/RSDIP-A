using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace RsNoAMain
{
    public partial class MainWindow
    {
        private void HSI_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            _dock.AddDocWpf(new RS_Diag.HSI(_image[_fChoose.ChoosedFile]),
                "图片HSI显示: " + _image[_fChoose.ChoosedFile].FileName);
        }
    }
}
