using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Division42.NetworkTools.IcmpPing
{
    /// <summary>
    /// Exception class for the Division42.NetworkTools.IcmpPing namespace.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class IcmpPingException : NetworkToolsException
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public IcmpPingException()
        {
        }

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="message">The message to include with this exception.</param>
        public IcmpPingException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="message">The message to include with this exception.</param>
        /// <param name="inner"></param>
        public IcmpPingException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
