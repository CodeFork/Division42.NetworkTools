using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Division42.NetworkTools.TraceRoute;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Division42.NetworkTools.Tests.TraceRoute
{
    [TestClass]
    public class TraceRouteManagerTests
    {
        [TestMethod]
        public void ConstructorWithNoArguments_ReturnsInstance()
        {
            TraceRouteManager instance = new TraceRouteManager();

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void ExecuteTraceRouteWithValidArguments_ReturnsExpected()
        {
            TraceRouteManager instance = new TraceRouteManager();
            Int32 expected = 0;
            Int32 actual = -1;

            Debug.WriteLine(TraceRouteHopDetail.FormattedTextHeader);
            instance.TraceRouteNodeFound += (sender, e) =>
            {
                Debug.WriteLine(e.Detail);
            };
            instance.TraceRouteComplete += (sender, e) =>
            {
                Debug.WriteLine("Trace complete.");
            };

            String host = "www.microsoft.com";
            Task<IEnumerable<TraceRouteHopDetail>> results = instance.ExecuteTraceRoute(host);

            results.Wait();

            actual = results.Result.Count();

            Assert.IsTrue(actual > expected);
        }
    }
}
