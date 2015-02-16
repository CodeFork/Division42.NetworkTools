using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Division42.NetworkTools.PortScan
{
    public class PortScanManager
    {
        public PortScanManager(string endPointToScan)
        {
            EndPoint = endPointToScan;
        }

        public string EndPoint { get; set; }

        public event EventHandler<PortScanResultEventArgs> PortScanResult;

        public void Start()
        {
            for (int index = 1; index < 100; index++)
            {
                if (_shouldCancel)
                    return;

                PortScanner scanner = new PortScanner(EndPoint, index);
                scanner.PortScanResult += new EventHandler<PortScanResultEventArgs>(scanner_PortScanResult);

                System.Threading.Interlocked.Increment(ref PortScanManager.ThreadCount);
                ThreadPool.QueueUserWorkItem(scanner.AttemptConnectionToPort);
            }
        }

        private void scanner_PortScanResult(object sender, PortScanResultEventArgs e)
        {
            if (PortScanResult != null)
            {
                PortScanResult(this, e);
            }
            System.Threading.Interlocked.Decrement(ref ThreadCount);
        }

        public void Stop()
        {
            _shouldCancel = true;
        }

        private bool _shouldCancel = false;


        public static int ThreadCount = 0;
    }
}
