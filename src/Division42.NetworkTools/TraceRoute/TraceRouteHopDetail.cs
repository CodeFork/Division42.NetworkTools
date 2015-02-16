using System;

namespace Division42.NetworkTools.TraceRoute
{
    public class TraceRouteHopDetail
    {
        public TraceRouteHopDetail()
        {
        }

        public TraceRouteHopDetail(int hopNumber, string ipAddress, string hostName, TimeSpan responseTime)
            : this()
        {
            HopNumber = hopNumber;
            IPAddress = ipAddress;
            HostName = hostName;
            ResponseTime = responseTime;
        }

        public int HopNumber { get; set; }
        public string IPAddress { get; set; }
        public string HostName { get; set; }
        public TimeSpan ResponseTime { get; set; }

        public static string FormattedTextHeader
        {
            get
            {
                string output = "Hop IP Address       Response Host" + Environment.NewLine;
                output += "--- ---------------- -------- ---------------------------------------" + Environment.NewLine;

                return output;
            }
        }

        public override string ToString()
        {
            return string.Format("{0,3} {1,-16} {2,6:N0}ms {3}", HopNumber, IPAddress, ResponseTime.Milliseconds, HostName);
        }
    }
}