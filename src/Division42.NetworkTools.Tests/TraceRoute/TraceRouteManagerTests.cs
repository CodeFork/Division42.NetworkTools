using System;
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

        //[TestMethod]
        //public void ConstructorWithNoArguments_ReturnsInstance()
        //{
        //    TraceRouteManager instance = new TraceRouteManager();

        //    //instance.ExecuteTraceRoute(destination)

        //    Assert.IsNotNull(instance);
        //}
    }
}
