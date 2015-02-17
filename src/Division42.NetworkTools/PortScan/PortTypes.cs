namespace Division42.NetworkTools.PortScan
{
    /// <summary>
    /// Enum for possible TCP/IP port types.
    /// </summary>
    public enum PortTypes
    {
        /// <summary>
        /// Unknown or not set.
        /// </summary>
        Unknown,
        /// <summary>
        /// Unigram Data Protocol (UDP).
        /// </summary>
        Udp,
        /// <summary>
        /// Transmission Control Protocol (TCP).
        /// </summary>
        Tcp,
    }
}