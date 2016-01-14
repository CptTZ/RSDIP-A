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

        private void BandMath_Click(object sender, RoutedEventArgs e)
        {
            var a = new RS_Diag.BandMath(_image);
            if (a.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            try
            {
                _loading.Start();
                var d = new RS_Lib.BandMath(a.StateMent, a.CtrlStr.ToArray(), a.Data.ToArray()).CalculatedResult;

                byte[,,] tmp = new byte[1, d.GetLength(0), d.GetLength(1)];

                for (int i = 0; i < d.GetLength(0); i++)
                {
                    for (int j = 0; j < d.GetLength(1); j++)
                    {
                        tmp[0, i, j] = d[i, j];
                    }
                }

                AddNewPic(tmp, "Band Math结果", false);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TonyZ", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _loading.Abort();
            }

        }

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

            AddNewPic(tmp, cho.FileName + "(自定义卷积核)", false);
            _loading.Abort();
        }

        private void ProcessSmooth(int h, int l, int o, int m)
        {
            var cho = _image[_fChoose.ChoosedFile];

            byte[,,] res = null;
            string type = "";
            switch (m)
            {
                case 0:
                    res = new RS_Lib.Mean(cho).MeanFilter;
                    type = "-均值滤波";
                    break;

                case 1:
                    res = new RS_Lib.Median(cho, h, l).MedianData;
                    type = "-中位数滤波";
                    break;

                case 2:
                    res = new RS_Lib.GaussLow(cho, h, l, o).GaussLowData;
                    type = "-高斯低通滤波";
                    break;

                case 3:
                    res = new RS_Lib.Grad(cho).GradData;
                    type = "-梯度倒数加权滤波";
                    break;
            }
            
            AddNewPic(res, cho.FileName + type, false);
        }

        private void ButtonSmooth_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            var a = new RS_Diag.Smooth();
            if (a.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            _loading.Start();

            ProcessSmooth(a.Hang, a.Lie, a.O, a.Method);

            _loading.Abort();
        }

        private void ProcessSharp(int m)
        {
            var cho = _image[_fChoose.ChoosedFile];

            byte[,,] res = null;
            string type = "";

            switch (m)
            {
                case 0:
                    res = new Robert(cho).RobertData;
                    type = "-罗伯特锐化最终图";
                    break;

                case 1:
                    res = new Sobel(cho).SobelData;
                    type = "-Sobel锐化最终图";
                    break;

                case 2:
                    res = new Lap(cho).Lapla;
                    type = "-拉普拉斯锐化最终图";
                    break;
            }

            AddNewPic(res, cho.FileName + type, false);
        }

        private void ButtonSharp_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;
            
            var a = new RS_Diag.Sharp();
            if (a.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            _loading.Start();

            ProcessSharp(a.Method);

            _loading.Abort();
        }

    }
}
