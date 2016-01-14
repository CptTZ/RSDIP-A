using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using RS_Lib;

namespace RsNoAMain
{
    public partial class MainWindow
    {
        /// <summary>
        /// 显示直方图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonHistogram_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            _dock.AddDocForm(new RS_Diag.Histogram(_image[_fChoose.ChoosedFile], 1), "直方图: " + _image[_fChoose.ChoosedFile].FileName);
        }

        /// <summary>
        /// 累计直方图计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonAccHistogram_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            _dock.AddDocForm(new RS_Diag.Histogram(_image[_fChoose.ChoosedFile], 2), "累计直方图: " + _image[_fChoose.ChoosedFile].FileName);
        }

        /// <summary>
        /// 对比度计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonContrast_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            double[] cst = new double[_image[_fChoose.ChoosedFile].BandsCount];
            for (int i = 1; i <= cst.Length; i++)
            {
                cst[i - 1] = new RS_Lib.Contrast(_image[_fChoose.ChoosedFile].GetPicData(i)).GetImageContrast();
            }

            _dock.AddDocForm(new RS_Diag.tmpContrast(cst), "对比度: " + _image[_fChoose.ChoosedFile].FileName);

        }

        /// <summary>
        /// 基本统计数据-平均，标准差，相关系数等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonBasicStats_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;
            _loading.Start();

            _dock.AddDocWpf(new RS_Diag.BasicStats(_image[_fChoose.ChoosedFile]), "基本统计量: " + _image[_fChoose.ChoosedFile].FileName);
            _loading.Abort();
        }

        /// <summary>
        /// 灰度共生矩阵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonGLCM_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            var cho = _image[_fChoose.ChoosedFile];

            _loading.Start();
            _dock.AddDocForm(new RS_Diag.GLCM(cho), "灰度共生矩阵：" + cho.FileName);
            _loading.Abort();
        }

        /// <summary>
        /// 极差纹理矩阵
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonRangeFilt_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            _loading.Start();
            _dock.AddDocWpf(new RS_Diag.RangeFilt(_image[_fChoose.ChoosedFile]), "极差纹理矩阵: " + _image[_fChoose.ChoosedFile].FileName);
            _loading.Abort();
        }

        private void HistoMatch(RsImage a, RsImage b)
        {
            var w = new RS_Diag.HistoMatch(a, b);
            if (w.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            var data = w.MatchedData;
            byte[,,] res = new byte[1, data.GetLength(0), data.GetLength(1)];

            for (int i = 0; i < data.GetLength(0); i++)
            {
                for (int j = 0; j < data.GetLength(1); j++)
                {
                    res[0, i, j] = data[i, j];
                }
            }

            AddNewPic(res, a.FileName + "-直方图规定化结果", false);
        }

        /// <summary>
        /// 直方图规定化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonUniHistogram_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            var cho = _image[_fChoose.ChoosedFile];

            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "选择目标直方图",
                Filter = "ENVI遥感数据头文件(*.HDR)|*.HDR;*.hdr"
            };
            if (ofd.ShowDialog() != true) return;
            
            try
            {
                RS_Lib.RsImage img = new RsImage(ofd.FileName);
                HistoMatch(cho, img);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "TonyZ", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// OIF计算
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonOif_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;
            if (_image[_fChoose.ChoosedFile].BandsCount < 4)
            {
                MessageBox.Show("图像波段太少，计算合成OIF无意义！", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            _loading.Start();
            _dock.AddDocWpf(new RS_Diag.OIF(_image[_fChoose.ChoosedFile]),
                "OIF情况: " + _image[_fChoose.ChoosedFile].FileName);
            _loading.Abort();
        }

        /// <summary>
        /// 直方图均衡化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            var cho = _image[_fChoose.ChoosedFile];

            HistoEqualization[] eq = new HistoEqualization[cho.BandsCount];

            _loading.Start();

            for (int i = 0; i < cho.BandsCount; i++)
            {
                eq[i] = new HistoEqualization(cho, i + 1);
            }

            byte[,,] res = new byte[cho.BandsCount, cho.Lines, cho.Samples];
            for (int i = 0; i < eq.Length; i++)
            {
                for (int j = 0; j < res.GetLength(1); j++)
                {
                    for (int k = 0; k < res.GetLength(2); k++)
                    {
                        res[i, j, k] = eq[i].EqualedData[j, k];
                    }
                }
            }

            AddNewPic(res, cho.FileName + "-直方图均衡化结果", false);

            _loading.Abort();
        }

        /// <summary>
        /// 对比度拉伸
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonConSt_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            var cho = _image[_fChoose.ChoosedFile];
            var a = new RS_Diag.ContrastStr(cho);
            if (a.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            double lV = a.LeftValue,
                rV = a.RightValue;
            var ind = a.BandList;
            RS_Lib.ContrastStretch[] st = new ContrastStretch[ind.Length];

            for (int i = 0; i < ind.Length; i++)
            {
                st[i] = new ContrastStretch(cho, ind[i] + 1, lV, rV);
            }

            byte[,,] res = new byte[ind.Length, cho.Lines, cho.Samples];

            for (int i = 0; i < ind.Length; i++)
            {
                for (int j = 0; j < cho.Lines; j++)
                {
                    for (int k = 0; k < cho.Samples; k++)
                    {
                        res[i, j, k] = st[i].StretchedImg[j, k];
                    }
                }
            }

            AddNewPic(res, cho.FileName + "-对比度拉伸结果", false);
        }

    }
}
