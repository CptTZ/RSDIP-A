using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;

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

            _loading.Start();
            _dock.AddDocWpf(new RS_Diag.GLCM(), "灰度共生矩阵: " + _image[_fChoose.ChoosedFile].FileName);
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

            // TODO:界面完善


        }

        /// <summary>
        /// 联合直方图
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonUniHistogram_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;
            if (_image[_fChoose.ChoosedFile].BandsCount < 2)
            {
                MessageBox.Show("图像波段太少！", "错误",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            // TODO:界面完善

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
    }
}
