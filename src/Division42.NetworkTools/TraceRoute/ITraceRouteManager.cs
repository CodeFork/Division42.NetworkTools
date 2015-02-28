using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Division42.NetworkTools.TraceRoute
{
    /// <summary>
    /// Interface for classes who perform a trace route.
    /// </summary>
    public interface ITraceRouteManager : INetworkManager
    {
        /// <summary>
        /// Event for when a trace route node is found.
        /// </summary>
        event EventHandler<TraceRouteNodeFoundEventArgs> TraceRouteNodeFound;

        /// <summary>
        /// Event for when the trace route is complete.
        /// </summary>
        event EventHandler<TraceRouteCompleteEventArgs> TraceRouteComplete;

        /// <summary>
        /// Gets the current cancellation token source.
        /// </summary>
        CancellationTokenSource CurrentCancellationTokenSource { get; }

        /// <summary>
        /// Executes the trace route.
        /// </summary>
        /// <param name="host">The destination host to which you wish to find the route.</param>
        /// <returns>All of the hops between the current computers and the 
        /// <paramref name="host"/> computer.</returns>
        /// <exception cref="ArgumentException"></exception>
        Task<IEnumerable<TraceRouteHopDetail>> ExecuteTraceRoute(String host);

        /// <summary>
        /// Gets the response time to the specified <paramref name="host"/>.
        /// </summary>
        /// <param name="host">The host to ping.</param>
        /// <exception cref="ArgumentException"></exception>
        TimeSpan GetResponseTime(String host);

        /// <summary>
        /// Cancels the current execution.
        /// </summary>
        void Cancel();
    }
}