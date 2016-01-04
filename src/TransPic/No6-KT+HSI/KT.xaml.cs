using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Microsoft.Win32;
using RS_Lib;

namespace RS_Diag
{
    /// <summary>
    /// ShowImage.xaml 的交互逻辑
    /// </summary>
    public partial class KT : UserControl
    {
        private System.Drawing.Bitmap _bit;
        private readonly byte[,,] _img;
        private readonly ShowLoading _loading = new ShowLoading();

        private byte[,,] _final;

        /// <summary>
        /// 显示图像-RGB
        /// </summary>
        /// <param name="ig">图像</param>
        public KT(RsImage ig)
        {
            InitializeComponent();

            InitMousePan();
            InitCombo1();

            this._img = ig.GetPicData();
        }

        #region 基本操作逻辑

        private void InitCombo1()
        {
            string[] a =
            {
                "Landsat 4",
                "Landsat 5",
                "Landsat 7"
            };
            OriType.ItemsSource = a;
            OriType.SelectedIndex = 0;
        }

        /// <summary>
        /// 转换Bitmap到Image(WPF)
        /// </summary>
        public static BitmapImage ConvertBitmapToSource(System.Drawing.Bitmap b,
            System.Drawing.RotateFlipType t = System.Drawing.RotateFlipType.Rotate90FlipX)
        {
            b.RotateFlip(t);
            MemoryStream ms = new MemoryStream();
            b.Save(ms, System.Drawing.Imaging.ImageFormat.Bmp);

            BitmapImage bmi = new BitmapImage();
            bmi.BeginInit();
            bmi.StreamSource = new MemoryStream(ms.ToArray());
            bmi.EndInit();
            ms.Close();

            return bmi;
        }

        private Point _mouseOri;
        private Point _mouseStart;

        private void InitMousePan()
        {
            ImgWindow.MouseWheel += MainWindow_MouseWheel;

            RsImage.MouseLeftButtonDown += RsImageMouseLeftButtonDown;
            RsImage.MouseLeftButtonUp += RsImageMouseLeftButtonUp;
            RsImage.MouseMove += RsImageMouseMove;
        }

        private void RsImageMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            RsImage.ReleaseMouseCapture();
        }

        private void RsImageMouseMove(object sender, MouseEventArgs e)
        {
            if (!RsImage.IsMouseCaptured) return;
            Point p = e.MouseDevice.GetPosition(border);

            Matrix m = RsImage.RenderTransform.Value;
            m.OffsetX = _mouseOri.X + (p.X - _mouseStart.X);
            m.OffsetY = _mouseOri.Y + (p.Y - _mouseStart.Y);

            RsImage.RenderTransform = new MatrixTransform(m);
        }

        private void RsImageMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (RsImage.IsMouseCaptured) return;
            RsImage.CaptureMouse();

            _mouseStart = e.GetPosition(border);
            _mouseOri.X = RsImage.RenderTransform.Value.OffsetX;
            _mouseOri.Y = RsImage.RenderTransform.Value.OffsetY;
        }

        private void MainWindow_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            Point p = e.MouseDevice.GetPosition(RsImage);

            Matrix m = RsImage.RenderTransform.Value;
            if (e.Delta > 0)
                m.ScaleAtPrepend(1.1, 1.1, p.X, p.Y);
            else
                m.ScaleAtPrepend(1 / 1.1, 1 / 1.1, p.X, p.Y);

            RsImage.RenderTransform = new MatrixTransform(m);
        }
        #endregion

        /// <summary>
        /// 画图
        /// </summary>
        private void MakeImage(byte[,,] img, int b)
        {
            this._bit = new System.Drawing.Bitmap(_img.GetLength(1), _img.GetLength(2));

            for (int i = 0; i < _img.GetLength(1); i++)
            {
                for (int j = 0; j < _img.GetLength(2); j++)
                {
                    System.Drawing.Color c = System.Drawing.Color.FromArgb(
                        img[b, i, j],
                        img[b, i, j],
                        img[b, i, j]);

                    _bit.SetPixel(i, j, c);
                }
            }

            RsImage.Source = ConvertBitmapToSource(_bit);
        }

        /// <summary>
        /// K-T
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _loading.Start();
            
            string[] s = {"亮度", "绿度", "湿度", "霾"};
            byte c = 0;
            switch (OriType.SelectedIndex)
            {
                case 0:
                    c = 4;
                    break;
                case 1:
                    c = 5;
                    break;
                case 2:
                    c = 7;
                    break;
            }

            try
            {
                _final = new RS_Lib.KT(_img, c).GetLinearStretch();
                LayerChoice.ItemsSource = s;
                LayerChoice.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "处理错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            finally
            {
                _loading.Abort();
            }
            
        }

        /// <summary>
        /// 保存图像
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ButtonSav_Click(object sender, RoutedEventArgs e)
        {
            BitmapSource bs = (BitmapSource) RsImage.Source;
            if (bs == null)
            {
                MessageBox.Show("未加载图像", "ERR", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "选择保存路径",
                Filter = "PNG图像(*.png)|*.png"
            };
            var s = sfd.ShowDialog();
            if (s == false) return;

            PngBitmapEncoder pngE = new PngBitmapEncoder();
            pngE.Frames.Add(BitmapFrame.Create(bs));
            using (Stream st = File.Create(sfd.FileName))
            {
                pngE.Save(st);
            }
        }

        // 2%拉伸
        private void P2S_Click(object sender, RoutedEventArgs e)
        {
            if (_final == null) return;

            _loading.Start();
            byte[,,] img = new byte[1, _img.GetLength(1), _img.GetLength(2)];
            
            RS_Lib.Stretch sth = new RS_Lib.Stretch(_final, LayerChoice.SelectedIndex);

            for (int j = 0; j < img.GetLength(1); j++)
            {
                for (int k = 0; k < img.GetLength(2); k++)
                {
                    img[0, j, k] = sth.StretchedBandData[j, k];
                }
            }
            
            MakeImage(img, 0);

            _loading.Abort();
        }

        private void LayerChoice_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_final != null)
                MakeImage(_final, LayerChoice.SelectedIndex);
        }
    }
}
