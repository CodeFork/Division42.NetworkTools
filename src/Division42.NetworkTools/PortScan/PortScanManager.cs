using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Division42.NetworkTools.Extensions;

namespace Division42.NetworkTools.PortScan
{
    /// <summary>
    /// Class for managing a port scan.
    /// </summary>
    public class PortScanManager : IPortScanManager
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="endPointToScan">The hostname or IP address to scan.</param>
        public PortScanManager(String endPointToScan)
        {
            if (String.IsNullOrWhiteSpace(endPointToScan))
                throw new ArgumentException("Argument \"endPointToScan\" cannot be null or empty.", "endPointToScan");

            EndPoint = endPointToScan;
            CurrentCancellationTokenSource = new CancellationTokenSource();
        }

        /// <summary>
        /// Gets the current endpoint to be scanned.
        /// </summary>
        public String EndPoint { get; private set; }

        /// <summary>
        /// Gets the current cancellation token source.
        /// </summary>
        public CancellationTokenSource CurrentCancellationTokenSource { get; private set; }

        /// <summary>
        /// Event for when there is a result from the port scan.
        /// </summary>
        public event EventHandler<PortScanResultEventArgs> PortScanResult;

        /// <summary>
        /// Starts the port scan. Scans ports 1 through 65535.
        /// </summary>
        public void Start()
        {
            Start(1, UInt16.MaxValue, PortTypes.Tcp);
        }

        /// <summary>
        /// Starts the port scan.
        /// </summary>
        /// <param name="startingPortNumber">The port number to inclusively start scanning. (e.g. 1)</param>
        /// <param name="endingPortNumber">The port number to inclusively stop scanning. (e.g. 65535)</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void Start(Int32 startingPortNumber, Int32 endingPortNumber, PortTypes typesToScan)
        {
            if (startingPortNumber < 1)
                throw new ArgumentOutOfRangeException("startingPortNumber",
                    "Argument \"startingPortNumber\" must be greater than zero.");
            if (endingPortNumber > 65535)
                throw new ArgumentOutOfRangeException("endingPortNumber",
                    "Argument \"endingPortNumber\" must be less than 65535.");

            for (int index = startingPortNumber; index <= endingPortNumber; index++)
            {
                if (CurrentCancellationTokenSource.IsCancellationRequested)
                    return;

                PortScanner scanner = new PortScanner(EndPoint, index);
                scanner.PortScanResult += new EventHandler<PortScanResultEventArgs>(scanner_PortScanResult);

                switch (typesToScan)
                {
                    case PortTypes.Tcp:
                        {
                            _tasks.Add(Task.Factory.StartNew(() => scanner.AttemptTcpConnectionToPort(),
                                CurrentCancellationTokenSource.Token));
                            break;
                        }
                    case PortTypes.Udp:
                        {
                            _tasks.Add(Task.Factory.StartNew(() => scanner.AttemptUdpConnectionToPort(),
                                CurrentCancellationTokenSource.Token));
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// Stops the port scan.
        /// </summary>
        public void Stop()
        {
            CurrentCancellationTokenSource.Cancel();
        }

        /// <summary>
        /// Gets or sets the currently running tasks.
        /// </summary>
        public IEnumerable<Task> Tasks
        {
            get { return _tasks; }
        } private readonly List<Task> _tasks = new List<Task>();


        private void scanner_PortScanResult(object sender, PortScanResultEventArgs e)
        {
            if (PortScanResult != null)
            {
                PortScanResult(this, e);
            }
        }
    }
}
