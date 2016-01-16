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
            Window lo = new Loading();
            lo.Show();
            Dispatcher.Run();
        }

        public void Start()
        {
            switch (_thread.ThreadState)
            {
                case ThreadState.Unstarted:
                    _thread.Start();
                    break;

                case ThreadState.Aborted:
                case ThreadState.Stopped:
                    NewThread();
                    _thread.Start();
                    break;
            }
        }

        public void Abort()
        {
            Dispatcher.FromThread(_thread)?.InvokeShutdown();
        }

    }
}
