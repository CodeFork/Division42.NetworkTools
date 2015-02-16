using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Division42.NetworkTools.IcmpPing
{
    /// <summary>
    /// Class who can perform an ICMP ping to a network host.
    /// </summary>
    public class PingManager : IPingManager
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="hostName">The hostname to ping.</param>
        /// <param name="timeBetweenPings">The time span in between ping attempts.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <exception cref="ArgumentException"></exception>
        public PingManager(String hostName, TimeSpan timeBetweenPings)
        {
            if (String.IsNullOrWhiteSpace(hostName))
                throw new ArgumentException("Argument \"HostName\" cannot be null or empty.", "hostName");
            if (timeBetweenPings.TotalSeconds < 1)
                throw new ArgumentOutOfRangeException("timeBetweenPings", "Argument \"timeBetweenPings\" cannot be less than 1 second.");

            HostName = hostName;
            TimeBetweenPings = timeBetweenPings;
        }

        /// <summary>
        /// Gets the current hostname who will be pinged.
        /// </summary>
        public String HostName { get; protected set; }

        /// <summary>
        /// Gets the time span in between ping attempts.
        /// </summary>
        public TimeSpan TimeBetweenPings { get; protected set; }


        /// <summary>
        /// Event for when the state of this ping manager changes. (e.g. starting, stopping, etc)
        /// </summary>
        public event EventHandler<PingManagerStateChangedEventArgs> PingManagerStateChanged;

        /// <summary>
        /// Event for when the ping process determines a result, including no response.
        /// </summary>
        public event EventHandler<PingResultEventArgs> PingResult;

        /// <summary>
        /// Starts the pinging process. Check the <see cref="PingResult"/> event for the async results of the ping.
        /// </summary>
        public virtual void Start()
        {
            if (pingTask != null && pingTask.Status == TaskStatus.Running)
            {
                return; // Already running
            }

            pingCancellationToken = new CancellationTokenSource();
            pingTask = Task.Factory.StartNew(ExecutePing, pingCancellationToken.Token);
        }

        /// <summary>
        /// Triggers the stop of the pinging process.
        /// </summary>
        public virtual void Stop()
        {
            if (pingTask != null && pingTask.Status == TaskStatus.Running && pingCancellationToken != null)
            {
                pingCancellationToken.Cancel();
            }

            OnStateChangedEvent(PingManagerStates.Stopped);
        }

        /// <summary>
        /// Actually execute the ping.
        /// </summary>
        protected virtual void ExecutePing()
        {
            while (!pingCancellationToken.Token.IsCancellationRequested)
            {
                OnStateChangedEvent(PingManagerStates.Pinging);
                using (Ping ping = new Ping())
                {
                    try
                    {
                        PingReply reply = ping.Send(HostName, 5000);

                        OnPingResult(reply, null);
                    }
                    catch (PingException exception)
                    {
                        OnPingResult(null, exception);
                    }
                    catch (SocketException exception)
                    {
                        OnPingResult(null, exception);
                    }
                }

                OnStateChangedEvent(PingManagerStates.Idle);
                Thread.Sleep(TimeBetweenPings);
            }
        }

        /// <summary>
        /// Event handler for when the ping manager state is about to change.
        /// </summary>
        /// <param name="newState">The new state of the ping manager.</param>
        protected virtual void OnStateChangedEvent(PingManagerStates newState)
        {
            if (PingManagerStateChanged != null)
            {
                PingManagerStateChanged(this, new PingManagerStateChangedEventArgs(newState));
            }
        }

        /// <summary>
        /// Event handler for when the ping manager state is about to change.
        /// </summary>
        /// <param name="reply">The reply from the ping request, or null.</param>
        /// <param name="exception">The exception that was thrown, if any, or null.</param>
        protected virtual void OnPingResult(PingReply reply,Exception exception)
        {
            if (PingResult != null)
            {
                PingResult(this, new PingResultEventArgs(reply,exception));
            }
        }

        private Task pingTask;
        private CancellationTokenSource pingCancellationToken = new CancellationTokenSource();
    }
}
