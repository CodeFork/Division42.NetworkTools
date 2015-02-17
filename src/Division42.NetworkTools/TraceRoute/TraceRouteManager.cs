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
    /// <summary>
    /// Class for managing a network trace route.
    /// </summary>
    public class TraceRouteManager
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public TraceRouteManager()
        {
            CurrentCancellationTokenSource = new CancellationTokenSource();
        }

        public event EventHandler<TraceRouteNodeFoundEventArgs> TraceRouteNodeFound;
        public event EventHandler<TraceRouteCompleteEventArgs> TraceRouteComplete;

        /// <summary>
        /// Gets the current cancellation token source.
        /// </summary>
        public CancellationTokenSource CurrentCancellationTokenSource { get; private set; }

        public Task<IEnumerable<TraceRouteHopDetail>> ExecuteTraceRoute(String host)
        {
            if (String.IsNullOrWhiteSpace(host))
                throw new ArgumentException("Argument \"host\" cannot be null or empty.", "host");

            return Task<IEnumerable<TraceRouteHopDetail>>.Factory.StartNew(() =>
            {
                List<TraceRouteHopDetail> output = new List<TraceRouteHopDetail>();

                using (Ping ping = new Ping())
                {
                    PingOptions options = new PingOptions(1, true);
                    Byte[] buffer = new Byte[32];

                    PingReply reply = ping.Send(host, 5000, buffer, options);

                    while (true)
                    {
                        if (CurrentCancellationTokenSource.IsCancellationRequested)
                            break;

                        if (reply.Address == null)
                        {
                            TraceRouteHopDetail detail = new TraceRouteHopDetail(options.Ttl, "*",
                                "***Request Timed Out***", new TimeSpan());
                            output.Add(detail);

                            if (TraceRouteNodeFound != null)
                                TraceRouteNodeFound(this, new TraceRouteNodeFoundEventArgs(detail));
                        }
                        else
                        {
                            String hostName = reply.Address.ToString();

                            try
                            {
                                hostName = Dns.GetHostEntry(reply.Address).HostName;
                            }
                            catch (SocketException)
                            {
                            }

                            TimeSpan responseTime = GetResponseTime(reply.Address.ToString());

                            TraceRouteHopDetail detail = new TraceRouteHopDetail(options.Ttl, reply.Address.ToString(),
                                hostName, responseTime);
                            output.Add(detail);

                            if (TraceRouteNodeFound != null)
                                TraceRouteNodeFound(this, new TraceRouteNodeFoundEventArgs(detail));

                            if (reply.Address.ToString().Equals(host))
                                break;


                        }
                        if (options.Ttl >= 30)
                            break;

                        options.Ttl += 1;
                        reply = ping.Send(host, 5000, buffer, options);
                    }

                    if (TraceRouteComplete != null)
                        TraceRouteComplete(this, new TraceRouteCompleteEventArgs(output));

                    return output;
                }
            }, CurrentCancellationTokenSource.Token);
        }

        /// <summary>
        /// Gets the response time to the specified <paramref name="host"/>.
        /// </summary>
        /// <param name="host">The host to ping.</param>
        /// <exception cref="ArgumentException"></exception>
        public TimeSpan GetResponseTime(String host)
        {
            if (String.IsNullOrWhiteSpace(host))
                throw new ArgumentException("Argument \"ipAddress\" cannot be null or empty.", "host");

            using (Ping ping = new Ping())
            {
                try
                {
                    PingReply reply = ping.Send(host);
                    Int32 totalMilliseconds = (Int32)reply.RoundtripTime;

                    return new TimeSpan(0, 0, 0, 0, totalMilliseconds);
                }
                catch (PingException)
                {
                }
            }

            return TimeSpan.MaxValue;
        }

        public void Cancel()
        {
            CurrentCancellationTokenSource.Cancel();
        }
    }
}
