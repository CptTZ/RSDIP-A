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
    public partial class HSI : UserControl
    {
        private System.Drawing.Bitmap _bit;
        private readonly byte[,,] _img;
        private readonly ShowLoading _loading = new ShowLoading();

        /// <summary>
        /// 显示图像-RGB
        /// </summary>
        /// <param name="ig">图像</param>
        public HSI(RsImage ig)
        {
            InitializeComponent();

            InitMousePan();
            InitComboBox(ig.BandsCount);

            this._img = ig.GetPicData();
        }

        #region 基本操作逻辑

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

        private void InitComboBox(int n)
        {
            String[] str = new String[n];

            for (int i = 0; i < n; i++)
            {
                str[i] = "Band " + (i + 1);
            }

            red.ItemsSource = str;
            blue.ItemsSource = str;
            green.ItemsSource = str;

            red.SelectedIndex = 0;
            blue.SelectedIndex = 0;
            green.SelectedIndex = 0;
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
        private void MakeImage(byte[,,] img)
        {
            this._bit = new System.Drawing.Bitmap(_img.GetLength(1), _img.GetLength(2));

            for (int i = 0; i < _img.GetLength(1); i++)
            {
                for (int j = 0; j < _img.GetLength(2); j++)
                {
                    System.Drawing.Color c = System.Drawing.Color.FromArgb(
                        img[0, i, j],
                        img[1, i, j],
                        img[2, i, j]);

                    _bit.SetPixel(i, j, c);
                }
            }

            RsImage.Source = ConvertBitmapToSource(_bit);
        }

        /// <summary>
        /// 确定按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            byte[] meg = {(byte) red.SelectedIndex, (byte) green.SelectedIndex, (byte) blue.SelectedIndex};
            _loading.Start();

            RGBHSI hsi = new RGBHSI(_img, meg);
            MakeImage(hsi.GetLinearStretch());

            _loading.Abort();
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

    }
}
