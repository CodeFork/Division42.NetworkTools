using System;
using System.Collections.Generic;

namespace Division42.NetworkTools.TraceRoute
{
    public class TraceRouteCompleteEventArgs : EventArgs
    {
        public TraceRouteCompleteEventArgs()
        {
        }

        public TraceRouteCompleteEventArgs(List<TraceRouteHopDetail> traceRouteHops)
            : this()
        {
            TraceRouteHops = traceRouteHops;
        }

        public List<TraceRouteHopDetail> TraceRouteHops { get; set; }
    }
}