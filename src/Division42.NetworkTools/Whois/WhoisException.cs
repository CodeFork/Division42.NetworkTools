using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Division42.NetworkTools.Whois
{
    /// <summary>
    /// Exception class for the Division42.NetworkTools.Whois namespace.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class WhoisException : NetworkToolsException
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public WhoisException()
        {
        }

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="message">The message to include with this exception.</param>
        public WhoisException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="message">The message to include with this exception.</param>
        /// <param name="inner"></param>
        public WhoisException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
