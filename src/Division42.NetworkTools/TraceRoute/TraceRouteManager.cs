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
    public class TraceRouteManager : ITraceRouteManager
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public TraceRouteManager()
        {
            CurrentCancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Event for when a trace route node is found.
        /// </summary>
        public event EventHandler<TraceRouteNodeFoundEventArgs> TraceRouteNodeFound;

        /// <summary>
        /// Event for when the trace route is complete.
        /// </summary>
        public event EventHandler<TraceRouteCompleteEventArgs> TraceRouteComplete;

        /// <summary>
        /// Gets the current cancellation token source.
        /// </summary>
        public CancellationTokenSource CurrentCancellationTokenSource { get; private set; }

        /// <summary>
        /// Executes the trace route.
        /// </summary>
        /// <param name="host">The destination host to which you wish to find the route.</param>
        /// <returns>All of the hops between the current computers and the 
        /// <paramref name="host"/> computer.</returns>
        /// <exception cref="ArgumentException"></exception>
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

                            // If we hit the last address or found the host, then stop.
                            if (reply.Address.Equals(lastReplyAddress) || reply.Address.ToString().Equals(host))
                                break;
                         
                            TraceRouteHopDetail detail = new TraceRouteHopDetail(options.Ttl, reply.Address.ToString(),
                                hostName, responseTime);
                            
                            output.Add(detail);

                            if (TraceRouteNodeFound != null)
                                TraceRouteNodeFound(this, new TraceRouteNodeFoundEventArgs(detail));
                        }
                        if (options.Ttl >= 30)
                            break;

                        lastReplyAddress = reply.Address;

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

        /// <summary>
        /// Cancels the current execution.
        /// </summary>
        public void Cancel()
        {
            CurrentCancellationTokenSource.Cancel();
        }

        private IPAddress lastReplyAddress;
    }
}
