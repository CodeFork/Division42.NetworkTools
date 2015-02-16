using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Division42.NetworkTools.Whois
{
    public class WhoisManager
    {
        private const string DefaultWhoisLookupFormat = "{0}.whois-servers.net";

        public static string ExecuteWhoisForDomain(string domain)
        {
            string[] domainParts = domain.Split('.');
            string topLevelDomain = domainParts[domainParts.Length - 1];
            string lookupServer = string.Format(DefaultWhoisLookupFormat, topLevelDomain);

            return ExecuteWhoisForDomain(domain, lookupServer);
        }

        public static string ExecuteWhoisForDomain(string domain, string whoisServer)
        {
            IPAddress whoisServerIPAddress = Dns.GetHostEntry(whoisServer).AddressList[0];
            int whoisPort = 43;
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
                        //output.Append(ASCIIEncoding.ASCII.GetString(response, 0, bytesRecievedCount));
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

        public static List<string> FindWhoisServerInOutput(string whoisOutput)
        {
            List<string> output = new List<string>();

            if (whoisOutput.Contains("Whois Server: "))
            {
                int lastPosition = 0;
                while (true)
                {
                    int startPosition = whoisOutput.IndexOf("Whois Server:", lastPosition);

                    if (startPosition > 0)
                    {

                        int endPosition = whoisOutput.IndexOf('\n', startPosition + 1);

                        string line = whoisOutput.Substring(startPosition, endPosition - startPosition);

                        string[] lineParts = line.Split(':');

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
    }
}
