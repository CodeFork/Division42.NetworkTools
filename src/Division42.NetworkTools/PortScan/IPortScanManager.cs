using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Division42.NetworkTools.PortScan
{
    /// <summary>
    /// Interface for classes who perform a port scan.
    /// </summary>
    public interface IPortScanManager : INetworkManager
    {
        /// <summary>
        /// Gets the current endpoint to be scanned.
        /// </summary>
        String EndPoint { get; }

        /// <summary>
        /// Gets the current cancellation token source.
        /// </summary>
        CancellationTokenSource CurrentCancellationTokenSource { get; }

        /// <summary>
        /// Gets or sets the currently running tasks.
        /// </summary>
        IEnumerable<Task> Tasks { get; }

        /// <summary>
        /// Event for when there is a result from the port scan.
        /// </summary>
        event EventHandler<PortScanResultEventArgs> PortScanResult;

        /// <summary>
        /// Starts the port scan. Scans ports 1 through 65535.
        /// </summary>
        void Start();

        /// <summary>
        /// Starts the port scan.
        /// </summary>
        /// <param name="startingPortNumber">The port number to inclusively start scanning. (e.g. 1)</param>
        /// <param name="endingPortNumber">The port number to inclusively stop scanning. (e.g. 65535)</param>
        /// <param name="typesToScan">The protocols to scan.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        void Start(Int32 startingPortNumber, Int32 endingPortNumber, PortTypes typesToScan);

        /// <summary>
        /// Stops the port scan.
        /// </summary>
        void Stop();
    }
}