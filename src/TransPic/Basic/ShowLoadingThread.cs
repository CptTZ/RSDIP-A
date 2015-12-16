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
            _thread = new Thread(InitWindow);
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
            if (_thread.ThreadState == ThreadState.Running)
            {
                _thread.IsBackground = true;
                _thread.Abort();
            }
        }

    }
}
