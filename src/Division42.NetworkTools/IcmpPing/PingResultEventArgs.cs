using System;
using System.Net.NetworkInformation;

namespace Division42.NetworkTools.IcmpPing
{
    public class PingResultEventArgs : EventArgs
    {
        public PingResultEventArgs(PingReply reply, Exception exception)
        {
            Reply = reply;

            Success = (reply != null);
            LastException = exception;
        }

        public PingReply Reply { get; set; }

        public bool Success { get; set; }

        public Exception LastException { get; set; }
    }
}