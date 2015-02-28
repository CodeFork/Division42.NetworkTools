using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Division42.NetworkTools.Whois
{
    /// <summary>
    /// Class for executing Whois requests.
    /// </summary>
    public class WhoisManager : IWhoisManager
    {
        /// <summary>
        /// Executes whois for a domain.
        /// </summary>
        /// <param name="domain">The domain to lookup.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>A text string with the whois information.</returns>
        public String ExecuteWhoisForDomain(String domain)
        {
            if (String.IsNullOrWhiteSpace(domain))
                throw new ArgumentException("Argument \"domain\" cannot be null or empty.", "domain");

            String[] domainParts = domain.Split('.');
            String topLevelDomain = domainParts[domainParts.Length - 1];
            String lookupServer = String.Format(DefaultWhoisLookupFormat, topLevelDomain);

            return ExecuteWhoisForDomain(domain, lookupServer);
        }

        /// <summary>
        /// Executes whois for a domain.
        /// </summary>
        /// <param name="domain">The domain to lookup.</param>
        /// <param name="whoisServer">The whois server to use.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>A text string with the whois information.</returns>
        public String ExecuteWhoisForDomain(String domain, String whoisServer)
        {
            if (String.IsNullOrWhiteSpace(domain))
                throw new ArgumentException("Argument \"domain\" cannot be null or empty.", "domain");
            if (String.IsNullOrWhiteSpace(whoisServer))
                throw new ArgumentException("Argument \"whoisServer\" cannot be null or empty.", "whoisServer");

            IPAddress whoisServerIPAddress = Dns.GetHostEntry(whoisServer).AddressList[0];
            Int32 whoisPort = 43;
            IPEndPoint lookupServerEndPoint = new IPEndPoint(whoisServerIPAddress, whoisPort);

            StringBuilder output = new StringBuilder();

            try
            {
                using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
                {
                    socket.Connect(lookupServerEndPoint);

                    byte[] requestBytes = ASCIIEncoding.ASCII.GetBytes(string.Format("{0}\r\n", domain));

                    socket.Send(requestBytes);

                    byte[] response = new byte[1024];
                    int bytesRecievedCount = socket.Receive(response);
                    while (bytesRecievedCount > 0)
                    {
                        output.Append(ASCIIEncoding.ASCII.GetString(response, 0, bytesRecievedCount).Replace("\n", Environment.NewLine));
                        bytesRecievedCount = socket.Receive(response);
                    }
                    socket.Shutdown(SocketShutdown.Both);
                }
            }
            catch (SocketException)
            {
                //TODO: Ugh, what shoudl we do in this case?
            }

            return output.ToString();

        }

        /// <summary>
        /// Attempts to find the authority whois server, in another whois server output.
        /// </summary>
        /// <param name="whoisOutput">The whois output to search.</param>
        /// <exception cref="ArgumentException"></exception>
        /// <returns>A list of all of the found whois servers, or null.</returns>
        public IEnumerable<String> FindWhoisServerInOutput(String whoisOutput)
        {
            if (String.IsNullOrWhiteSpace(whoisOutput))
                throw new ArgumentException("Argument \"whoisOutput\" cannot be null or empty.", "whoisOutput");

            List<String> output = new List<String>();

            if (whoisOutput.Contains("Whois Server: "))
            {
                Int32 lastPosition = 0;
                while (true)
                {
                    Int32 startPosition = whoisOutput.IndexOf("Whois Server:", lastPosition);

                    if (startPosition > 0)
                    {

                        Int32 endPosition = whoisOutput.IndexOf('\n', startPosition + 1);

                        String line = whoisOutput.Substring(startPosition, endPosition - startPosition);

                        String[] lineParts = line.Split(':');

                        if (lineParts.GetUpperBound(0) > 0)
                        {
                            output.Add(lineParts[1].Trim());
                        }

                        lastPosition = endPosition + 1;
                    }
                    else
                    {
                        break;
                    }
                }
            }
            return output;
        }

        private const String DefaultWhoisLookupFormat = "{0}.whois-servers.net";
    }
}
