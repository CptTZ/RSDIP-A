using System.Threading;
using System.Windows;
using System.Windows.Threading;

namespace RS_Diag
{
    public class ShowLoading
    {
        private Thread _thread;

        public ShowLoading()
        {
            NewThread();
        }

        private void NewThread()
        {
            _thread = new Thread(InitWindow) {IsBackground = false};
            _thread.SetApartmentState(ApartmentState.STA);
        }

        private void InitWindow()
        {
            Window _load = new Loading();
            _load.Show();
            System.Windows.Threading.Dispatcher.Run();
        }

        public void Start()
        {
            if (_thread.ThreadState == ThreadState.Unstarted)
            {
                _thread.Start();
                return;
            }
            if (_thread.ThreadState == ThreadState.Aborted)
            {
                NewThread();
                _thread.Start();
                return;
            }
        }

        public void Abort()
        {
            if (_thread.ThreadState != ThreadState.Running) return;

            // 避免计算过快，导致窗口未显示即关闭显示线程
            Thread.Sleep(50);
            _thread.IsBackground = true;
            _thread.Abort();
        }

    }
}
