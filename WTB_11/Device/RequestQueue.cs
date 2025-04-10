using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static System.Windows.Forms.AxHost;

namespace WPB_11.Device
{
    public class RequestQueue
    {
        private Queue<int> _requestQueue = new Queue<int>();
        private object _lockObject = new object();
        private bool _isProcessing = false;

        public void Enqueue(int request)
        {
            lock (_lockObject)
            {
                _requestQueue.Enqueue(request);
                if (!_isProcessing)
                {
                    _isProcessing = true;
                    ProcessQueue();
                }
            }
        }

        private void ProcessQueue()
        {
            ThreadPool.QueueUserWorkItem(state =>
        {
                while (true)
                {
                    int request;
                    lock (_lockObject)
                    {
                        if (_requestQueue.Count == 0)
                        {
                            _isProcessing = false;
                            break;
                        }
                        request = _requestQueue.Dequeue();
                    }
                    DeviceConnector.Instance().Request(DeviceCommands.CreateRequestTPCHR(request));
                }
            });
        }
    }
}
