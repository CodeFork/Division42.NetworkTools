using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Division42.NetworkTools.TraceRoute
{
    /// <summary>
    /// Exception class for the Division42.NetworkTools.TraceRouteException namespace.
    /// </summary>
    [ExcludeFromCodeCoverage]
    public class TraceRouteExceptionException : NetworkToolsException
    {
        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        public TraceRouteExceptionException()
        {
        }

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="message">The message to include with this exception.</param>
        public TraceRouteExceptionException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Creates a new instance of this type.
        /// </summary>
        /// <param name="message">The message to include with this exception.</param>
        /// <param name="inner"></param>
        public TraceRouteExceptionException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
