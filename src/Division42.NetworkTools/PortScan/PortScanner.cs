using System;
using System.Net.Sockets;

namespace Division42.NetworkTools.PortScan
{
    public class PortScanner
    {
        public PortScanner(string endPointName, int port)
        {
            EndPointName = endPointName;
            Port = port;
        }



        public int Port { get; set; }
        public string EndPointName { get; set; }

        public void AttemptConnectionToPort(object state)
        {
            try
            {
                try
                {
                    TcpClient client = new TcpClient();
                    client.ExclusiveAddressUse = false;
                    client.Connect(EndPointName, Port);
                    client.Close();

                    if (PortScanResult != null)
                    {
                        PortScanResult(this, new PortScanResultEventArgs(EndPointName, Port, PortScanResultEventArgs.PortTypes.Tcp));
                    }
                }
                catch (SocketException)
                {
                }

                //try
                //{
                //    UdpClient client = new UdpClient();
                //    client.ExclusiveAddressUse = false;
                //    client.Connect(EndPointName, Port);
                //    client.
                //    client.Close();

                //    if (PortScanResult != null)
                //    {
                //        PortScanResult(this, new PortScanResultEventArgs(EndPointName, Port, PortScanResultEventArgs.PortTypes.Udp));
                //    }
                //}
                //catch (SocketException)
                //{
                //}
            }
            finally
            {
//                System.Threading.Interlocked.Decrement(ref ThreadCount);
            }
        }

        public event EventHandler<PortScanResultEventArgs> PortScanResult;

    }
}