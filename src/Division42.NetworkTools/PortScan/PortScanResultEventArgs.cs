using System;

namespace Division42.NetworkTools.PortScan
{
    public class PortScanResultEventArgs : EventArgs
    {
        public PortScanResultEventArgs()
        {
        }

        public PortScanResultEventArgs(string endPoint, int port, PortTypes portType)
            : this()
        {
            EndPoint = endPoint;
            Port = port;
            PortType = portType;
        }

        public Int32 Port { get; set; }
        public String EndPoint { get; set; }
        public PortTypes PortType { get; set; }

        public override string ToString()
        {
            return String.Format("{0} port {1} found on host {2}.", PortType, Port, EndPoint);
        }
    }
}