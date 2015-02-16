using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Division42.NetworkTools.TraceRoute
{
    public class TraceRouteManager
    {
        public event EventHandler<TraceRouteNodeFoundEventArgs> TraceRouteNodeFound;
        public event EventHandler<TraceRouteCompleteEventArgs> TraceRouteComplete;

        public void ExecuteTraceRouteAsync(IPAddress destinationIPAddress)
        {
            ExecuteTraceRouteAsync(destinationIPAddress.ToString());
        }

        public void ExecuteTraceRouteAsync(string destinationIPAddress)
        {
            ParameterizedThreadStart threadStart = new ParameterizedThreadStart(ExecuteTraceRoute);
            Thread thread = new Thread(threadStart);

            thread.Start(destinationIPAddress);
        }

        private void ExecuteTraceRoute(object destinationIPAddress)
        {
            ExecuteTraceRoute(destinationIPAddress.ToString());
        }

        public List<TraceRouteHopDetail> ExecuteTraceRoute(IPAddress destinationIPAddress)
        {
            return ExecuteTraceRoute(destinationIPAddress.ToString());
        }

        public List<TraceRouteHopDetail> ExecuteTraceRoute(string destinationIPAddress)
        {
            List<TraceRouteHopDetail> output = new List<TraceRouteHopDetail>();

            using (Ping ping = new Ping())
            {
                PingOptions options = new PingOptions(1, true);
                byte[] buffer = new byte[32];
                PingReply reply = null;

                reply = ping.Send(destinationIPAddress, 5000, buffer, options);

                while (true)
                {
                    if (shouldCancel)
                        break;

                    if (reply.Address == null)
                    {
                        TraceRouteHopDetail detail = new TraceRouteHopDetail(options.Ttl, "*", "***Request Timed Out***", new TimeSpan());
                        output.Add(detail);

                        if (TraceRouteNodeFound != null)
                            TraceRouteNodeFound(this, new TraceRouteNodeFoundEventArgs(detail));
                    }
                    else
                    {
                        string hostName = reply.Address.ToString();

                        try
                        {
                            hostName = Dns.GetHostEntry(reply.Address).HostName;
                        }
                        catch (SocketException)
                        {
                        }

                        TimeSpan responseTime = GetResponseTime(reply.Address);

                        TraceRouteHopDetail detail = new TraceRouteHopDetail(options.Ttl, reply.Address.ToString(), hostName, responseTime);
                        output.Add(detail);

                        if (TraceRouteNodeFound != null)
                            TraceRouteNodeFound(this, new TraceRouteNodeFoundEventArgs(detail));

                        if (reply.Address.ToString().Equals(destinationIPAddress))
                            break;


                    }
                    if (options.Ttl >= 30)
                        break;

                    options.Ttl += 1;
                    reply = ping.Send(destinationIPAddress, 5000, buffer, options);
                }

                if (TraceRouteComplete != null)
                    TraceRouteComplete(this, new TraceRouteCompleteEventArgs(output));

                return output;
            }
        }

        private TimeSpan GetResponseTime(IPAddress ipAddress)
        {
            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = ping.Send(ipAddress);

                    return new TimeSpan(0, 0, 0, 0, (int)reply.RoundtripTime);
                }
                catch (PingException)
                {
                }
            }

            return new TimeSpan();
        }

        public void Cancel()
        {
            shouldCancel = true;
        } bool shouldCancel = false;
    }
}
