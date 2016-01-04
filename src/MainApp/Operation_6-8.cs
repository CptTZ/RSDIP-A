using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using RS_Diag.Basic;

namespace RsNoAMain
{
    public partial class MainWindow
    {

        private void HSI_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            int c = _fChoose.ChoosedFile;
            
            _dock.AddDocWpf(new RS_Diag.HSI(_image[c])
                , "图像HSI处理: " + _image[c].FileName);
        }

        private void KT_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;
            int c = _fChoose.ChoosedFile;

            try
            {
                _dock.AddDocWpf(new RS_Diag.KT(_image[c])
                , "图像缨帽变换: " + _image[c].FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "处理错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

        }

    }
}
