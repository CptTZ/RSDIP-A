using System;
using System.Collections.Generic;
using System.Windows;
using Fluent;
using Microsoft.Win32;
using RS_Diag;
using RS_Lib;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;

namespace RsNoAMain
{

    public partial class MainWindow : Fluent.RibbonWindow
    {
        /// <summary>
        /// 程序全局图像列表
        /// </summary>
        private readonly List<RS_Lib.RsImage> _image = new List<RS_Lib.RsImage>();

        /// <summary>
        /// Dock的各种操作
        /// </summary>
        private readonly DockPage _dock;

        private readonly RS_Diag.Basic.FileChoose _fChoose = new RS_Diag.Basic.FileChoose();
        private readonly ShowLoading _loading = new ShowLoading();

        public MainWindow()
        {
            InitializeComponent();
            _dock = new DockPage(this.DockingManager);

            _dock.AddFormLeft(_fChoose, "图像选择");
        }

        #region 不会动的区域

        private void CloseAllFile_Click(object sender, RoutedEventArgs e)
        {
            if (_image.Count == 0)
            {
                MessageBox.Show("您未打开任何图像！", "提示", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return;
            }

            var s = MessageBox.Show("确定是否要关闭所有已打开的文件？\n此选项包括存于内存中的文件！", "提示",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (s != MessageBoxResult.Yes) return;

            _dock.CloseAllDocument();
            _fChoose.RemoveAll();
            _image.Clear();
        }

        /// <summary>
        /// 检查是否可以进行图像处理
        /// </summary>
        /// <returns></returns>
        private bool CheckImage()
        {
            if (_fChoose.ChoosedFile == -1)
            {
                MessageBox.Show("请选择遥感图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }
            if (_image.Count == 0 || _fChoose.ChoosedFile == -2) 
            {
                MessageBox.Show("请至少打开一幅遥感图像！", "错误", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 退出程序
        /// </summary>
        private void ButtonExit_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            _image.Clear();
            _loading.Abort();

            System.Environment.Exit(0);
        }

        /// <summary>
        /// 关于信息
        /// </summary>
        private void ButtonAbout_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            MessageBox.Show("13级地信-A组编写\n本程序基于微软WPF及C#构建\n\n" +
                            "组长：张宜弛(10130423)\n副组长：顾赛华(10130430)\n\n组员（按学号排序）：\n" +
                            "王航宇\t王雪\n孙明\t李冬瑞\n夏子倩\t黄嘉翎\n龚俊杰\t方志鑫\n刘君妍"
                            , "关于本程序", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        /// <summary>
        /// 退出处理
        /// </summary>
        private void RibbonWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var res = MessageBox.Show("确定要退出程序吗？", "询问",
                MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (res == MessageBoxResult.No) 
            {
                e.Cancel = true;
            }
            else
            {
                _image.Clear();
            }
        }

        /// <summary>
        /// 合并BSQ图像
        /// </summary>
        private void MergeImage_Click(object sender, RoutedEventArgs e)
        {
            Window transWindow = new RS_Diag.WindowMerge();
            transWindow.Show();
        }

        #endregion

        /// <summary>
        /// 转换图像格式
        /// </summary>
        private void ButtonConvert_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;
            _dock.AddDocWpf(new RS_Diag.MainWindow(_image[_fChoose.ChoosedFile]),
                "图像转换: " + _image[_fChoose.ChoosedFile].FileName);
        }

        /// <summary>
        /// 打开遥感图像文件
        /// </summary>
        private void OpenHeaderFile_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Filter = "ENVI遥感数据头文件(*.HDR)|*.HDR;*.hdr|一般图像(*.jpg,*.bmp)|*.jpg;*.bmp"
            };

            if (ofd.ShowDialog() != true) return;
            _loading.Start();

            try
            {
                if (ofd.FilterIndex == 1)
                    _image.Add(new RS_Lib.RsImage(ofd.FileName));
                else if (ofd.FilterIndex == 2)
                    _image.Add(new RS_Lib.RsImage(ofd.FileName, " 来自图像文件"));
            }
            catch (Exception ex)
            {
                MessageBox.Show("打开失败！\n" + ex.Message, "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            finally { _loading.Abort(); }
            
            _fChoose.AddByFilePath(_image[_image.Count - 1].GetFilePath());
        }
        
        /// <summary>
        /// 图像合成
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowImg_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            _dock.AddDocWpf(new RS_Diag.ShowImage(_image[_fChoose.ChoosedFile]), 
                "图片合成: " + _image[_fChoose.ChoosedFile].FileName);
        }

        private void ShowImgInfo_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            _dock.AddDocWpf(new RS_Diag.FileInfo(_image[_fChoose.ChoosedFile]),
                    "图像信息: " + _image[_fChoose.ChoosedFile].FileName);
        }

        private void SaveFile_Click(object sender, RoutedEventArgs e)
        {
            if (!CheckImage()) return;

            var cho = _image[_fChoose.ChoosedFile];

            SaveFileDialog sfd = new SaveFileDialog
            {
                Filter = "图片名(*)|*.*"
            };
            if (sfd.ShowDialog() != true) return;

            try
            {
                cho.SavePic(sfd.FileName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("保存失败！\n" + ex.Message, "失败", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

    }
}
