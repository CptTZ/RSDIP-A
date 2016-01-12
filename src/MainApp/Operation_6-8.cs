using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using RS_Diag.Basic;
using RS_Lib;

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

        private void ButtonCustom_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            var a = new RS_Diag.UserDef();
            if (a.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            _loading.Start();

            // 以下很慢……
            var cho = _image[_fChoose.ChoosedFile];
            var knl = a.Kernel;
            RS_Lib.Conv[] c = new Conv[cho.BandsCount];
            for (int i = 0; i < c.Length; i++)
            {
                c[i] = new Conv(cho.GetPicData(i + 1), knl);
            }

            byte[,,] tmp = new byte[cho.BandsCount, cho.Lines, cho.Samples];
            for (int i = 0; i < tmp.GetLength(0); i++)
            {
                var tt = c[i].GetLinearStretch();
                for (int j = 0; j < tmp.GetLength(1); j++)
                {
                    for (int k = 0; k < tmp.GetLength(2); k++)
                    {
                        tmp[i, j, k] = tt[j, k];
                    }
                }
            }

            AddNewPic(tmp, cho.FileName + "(自定义卷积核)");
            _loading.Abort();
        }

        private void ButtonSmooth_Click(object sender, RoutedEventArgs e)
        {
            var a = new RS_Diag.Smooth();
            if (a.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;


        }

    }
}
