using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Division42.NetworkTools.PortScan;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Division42.NetworkTools.Tests.PortScan
{
    [TestClass]
    public class PortScanManagerTests
    {
        [TestMethod]
        public void ConstructorWithValidArgument_ReturnsInstance()
        {
            String endPointToScan = "localhost";
            PortScanManager instance = new PortScanManager(endPointToScan);

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithEmptyArgument_ReturnsInstance()
        {
            String endPointToScan = String.Empty;
            PortScanManager instance = new PortScanManager(endPointToScan);

            Assert.Fail("Should have thrown ArgumentException.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithNullArgument_ReturnsInstance()
        {
            String endPointToScan = null;
            PortScanManager instance = new PortScanManager(endPointToScan);

            Assert.Fail("Should have thrown ArgumentException.");
        }

        [TestMethod]
        public void StartWithValidTcpSettings_ShouldFindSomething()
        {
            Boolean wasFound = false;
            String endPointToScan = "localhost";
            PortScanManager instance = new PortScanManager(endPointToScan);
            instance.PortScanResult += (sender, e) =>
            {
                Debug.WriteLine("Port scan found: {0} ({1})", e.Port, e.PortType);
                wasFound = true;
            };

            instance.Start(120, 150, PortTypes.Tcp);

            Task.WaitAll(instance.Tasks.ToArray());

            Assert.IsTrue(wasFound);
        }

        [TestMethod]
        public void StartWithValidUdpSettings_ShouldFindSomething()
        {
            Boolean wasFound = false;
            String endPointToScan = "localhost";
            PortScanManager instance = new PortScanManager(endPointToScan);
            instance.PortScanResult += (sender, e) =>
            {
                Debug.WriteLine("Port scan found: {0} ({1})", e.Port, e.PortType);
                wasFound = true;
            };

            instance.Start(120, 150, PortTypes.Udp);

            Task.WaitAll(instance.Tasks.ToArray());

            Assert.IsTrue(wasFound);
        }

        [TestMethod]
        public void StopAfterStart_ShouldCancelProperly()
        {
            Boolean wasFound = false;
            String endPointToScan = "localhost";
            PortScanManager instance = new PortScanManager(endPointToScan);
            instance.PortScanResult += (sender, e) =>
            {
                Debug.WriteLine("Port scan found: {0} ({1})", e.Port, e.PortType);
                wasFound = true;
            };

            instance.Start();
            instance.Stop();

            try
            {
                Task.WaitAll(instance.Tasks.ToArray());
            }
            catch (AggregateException exception)
            {
                if (exception.InnerException is TaskCanceledException)
                {
                }
                else
                {
                    Assert.Fail("TaskCanceledException should have been thrown.");
                }
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartWithLowStartArguments_ThrowsException()
        {
            Boolean wasFound = false;
            String endPointToScan = "localhost";
            PortScanManager instance = new PortScanManager(endPointToScan);
            instance.PortScanResult += (sender, e) =>
            {
                Debug.WriteLine("Port scan found: {0} ({1})", e.Port, e.PortType);
                wasFound = true;
            };

            instance.Start(-5,159, PortTypes.Tcp);

            Assert.Fail("Should have thrown ArgumentException.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void StartWithHighStopArguments_ThrowsException()
        {
            Boolean wasFound = false;
            String endPointToScan = "localhost";
            PortScanManager instance = new PortScanManager(endPointToScan);
            instance.PortScanResult += (sender, e) =>
            {
                Debug.WriteLine("Port scan found: {0} ({1})", e.Port, e.PortType);
                wasFound = true;
            };

            instance.Start(120, UInt16.MaxValue+1, PortTypes.Tcp);

            Assert.Fail("Should have thrown ArgumentException.");
        }
    }
}
