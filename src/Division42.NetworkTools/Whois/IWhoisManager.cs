using System;
using System.Collections.Generic;

namespace Division42.NetworkTools.Whois
{
    /// <summary>
    /// Interface for classes who execute "whois" requests.
    /// </summary>
    public interface IWhoisManager : INetworkManager
    {
        /// <summary>
        /// Executes whois for a domain.
        /// </summary>
        /// <param name="domain">The domain to lookup.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>A text string with the whois information.</returns>
        String ExecuteWhoisForDomain(String domain);

        /// <summary>
        /// Executes whois for a domain.
        /// </summary>
        /// <param name="domain">The domain to lookup.</param>
        /// <param name="whoisServer">The whois server to use.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>A text string with the whois information.</returns>
        String ExecuteWhoisForDomain(String domain, String whoisServer);

        /// <summary>
        /// Attempts to find the authority whois server, in another whois server output.
        /// </summary>
        /// <param name="whoisOutput">The whois output to search.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>A list of all of the found whois servers, or null.</returns>
        IEnumerable<String> FindWhoisServerInOutput(String whoisOutput);
    }
}