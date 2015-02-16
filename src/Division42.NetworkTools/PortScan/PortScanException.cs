using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Division42.NetworkTools.PortScan
{
    /// <summary>
    /// Exception class for the Division42.NetworkTools.PortScan namespace.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class PortScanException : NetworkToolsException
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public PortScanException()
        {
        }

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="message">The message to include with this exception.</param>
        public PortScanException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="message">The message to include with this exception.</param>
        /// <param name="inner"></param>
        public PortScanException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
