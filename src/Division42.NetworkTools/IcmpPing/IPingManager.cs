using System;

namespace Division42.NetworkTools.IcmpPing
{
    /// <summary>
    /// Interface for a class who can perform an ICMP ping to a network host.
    /// </summary>
    public interface IPingManager : INetworkManager
    {
        /// <summary>
        /// Gets the current hostname who will be pinged.
        /// </summary>
        String HostName { get; }

        /// <summary>
        /// Gets the time span in between ping attempts.
        /// </summary>
        TimeSpan TimeBetweenPings { get; }

        /// <summary>
        /// Event for when the state of this ping manager changes. (e.g. starting, stopping, etc)
        /// </summary>
        event EventHandler<PingManagerStateChangedEventArgs> PingManagerStateChanged;

        /// <summary>
        /// Event for when the ping process determines a result, including no response.
        /// </summary>
        event EventHandler<PingResultEventArgs> PingResult;

        /// <summary>
        /// Starts the pinging process.
        /// </summary>
        void Start();

        /// <summary>
        /// Stops the pinging process.
        /// </summary>
        void Stop();
    }
}