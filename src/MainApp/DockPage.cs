using System;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using Xceed.Wpf.AvalonDock;
using Xceed.Wpf.AvalonDock.Layout;

namespace RsNoAMain
{
    /// <summary>
    /// Dock控制类
    /// </summary>
    class DockPage
    {
        private readonly DockingManager _dManager;

        public static bool dockManagerHasInstanced { get; private set; }

        public DockPage(DockingManager t)
        {
            if (dockManagerHasInstanced == true)
            {
                throw new ApplicationException("Dock管理应为单例模式！");
            }

            dockManagerHasInstanced = true;
            _dManager = t;
        }

        public void CloseAllDocument()
        {
            var docPane = _dManager.Layout.Descendents().OfType<LayoutDocumentPane>().First();

            docPane.Children.Clear();
        }

        /// <summary>
        /// 左侧载入新的WinForm
        /// </summary>
        public void AddFormLeft(UserControl uc, String title, String to = "图像加载信息")
        {
            var leftPane = _dManager.Layout.Descendents().OfType<LayoutAnchorablePane>().First();
            if (leftPane == null) return;

            var doc = new LayoutAnchorable
            {
                CanClose = false,
                CanHide = false,
                Title = title,
                ToolTip = to,
                Content = new WindowsFormsHost {Child = uc},
                IsSelected = true,
                CanAutoHide = true
            };

            leftPane.Children.Add(doc);
        }

        /// <summary>
        /// 加入新的WinForm到文档口(WPF Only)
        /// </summary>
        public void AddDocForm(UserControl uc, String title)
        {
            var docPane = _dManager.Layout.Descendents().OfType<LayoutDocumentPane>().First();

            var doc = new LayoutAnchorable
            {
                Title = title,
                Content = new WindowsFormsHost { Child = uc },
                IsSelected = true,
                CanAutoHide = true
            };

            docPane.Children.Add(doc);
        }

        /// <summary>
        /// 接入新的Window到侧边栏(WPF Only)
        /// </summary>
        /// <param name="wd">WPF窗口</param>
        /// <param name="title">标题</param>
        public void AddSidebarWpf(Object wd, String title)
        {
            var rightAnchorGp = _dManager.Layout.RightSide.Children.FirstOrDefault();
            if (rightAnchorGp == null)
            {
                rightAnchorGp = new LayoutAnchorGroup();
                _dManager.Layout.RightSide.Children.Add(rightAnchorGp);
            }

            var layAnc = new LayoutAnchorable
            {
                Title = title,
                Content = wd,
                CanAutoHide = true
            };

            rightAnchorGp.Children.Add(layAnc);
        }

        /// <summary>
        /// 加入新的Window到文档口(WPF Only)
        /// </summary>
        /// <param name="wd">WPF窗口</param>
        /// <param name="title">标题</param>
        public void AddDocWpf(Object wd, String title)
        {
            var docPane = _dManager.Layout.Descendents().OfType<LayoutDocumentPane>().First();
            var doc = new LayoutAnchorable
            {
                Title = title,
                Content = wd,
                IsSelected = true,
                CanAutoHide = true
            };

            if (docPane != null) docPane.Children.Add(doc);
        }
    }
}
