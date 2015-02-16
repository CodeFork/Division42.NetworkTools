using System;
using Division42.NetworkTools.PortScan;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Division42.NetworkTools.Tests.PortScan
{
    [TestClass]
    public class PortScannerTests
    {
        [TestMethod]
        public void ConstructorWithValidArguments_ReturnsInstance()
        {
            String endPointName = "localhost";
            Int32 portNumber = 135;
            PortScanner instance = new PortScanner(endPointName, portNumber);

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithEmptyEndpointArgument_ThrowsException()
        {
            String endPointName = String.Empty;
            Int32 portNumber = 135;
            PortScanner instance = new PortScanner(endPointName, portNumber);

            Assert.Fail("ArgumentException should have been thrown.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithNullEndpointArgument_ThrowsException()
        {
            String endPointName = null;
            Int32 portNumber = 135;
            PortScanner instance = new PortScanner(endPointName, portNumber);

            Assert.Fail("ArgumentException should have been thrown.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorWithLowPortArgument_ThrowsException()
        {
            String endPointName = "localhost";
            Int32 portNumber = 0;
            PortScanner instance = new PortScanner(endPointName, portNumber);

            Assert.Fail("ArgumentOutOfRangeException should have been thrown.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorWithHighPortArgument_ThrowsException()
        {
            String endPointName = "localhost";
            Int32 portNumber = UInt16.MaxValue + 1;
            PortScanner instance = new PortScanner(endPointName, portNumber);

            Assert.Fail("ArgumentOutOfRangeException should have been thrown.");
        }


        [TestMethod]
        public void AttemptTcpConnectionToPortWithValidSettings_ReturnsExpected()
        {
            Boolean hadSuccess = false;
            String endPointName = "localhost";
            Int32 portNumber = 135; // DCE (see: https://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers)
            PortScanner instance = new PortScanner(endPointName, portNumber);

            instance.PortScanResult += (sender, e) =>
            {
                hadSuccess = true;
            };

            instance.AttemptTcpConnectionToPort();

            Assert.IsTrue(hadSuccess);
        }

        [TestMethod]
        public void AttemptTcpConnectionToPortWithBadEndpoint_NoResponse()
        {
            Boolean hadSuccess = false;
            String endPointName = "localhostNotValid";
            Int32 portNumber = 135; // DCE (see: https://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers)
            PortScanner instance = new PortScanner(endPointName, portNumber);

            instance.PortScanResult += (sender, e) =>
            {
                hadSuccess = true;
            };

            instance.AttemptTcpConnectionToPort();

            Assert.IsFalse(hadSuccess);
        }

        [TestMethod]
        public void AttemptUdpConnectionToPortWithValidSettings_ReturnsExpected()
        {
            Boolean hadSuccess = false;
            String endPointName = "localhost";
            Int32 portNumber = 135; // DCE (see: https://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers)
            PortScanner instance = new PortScanner(endPointName, portNumber);

            instance.PortScanResult += (sender, e) =>
            {
                hadSuccess = true;
            };

            instance.AttemptUdpConnectionToPort();

            Assert.IsTrue(hadSuccess);
        }

        [TestMethod]
        public void AttemptUdpConnectionToPortWithBadEndpoint_NoResponse()
        {
            Boolean hadSuccess = false;
            String endPointName = "localhostNotValid";
            Int32 portNumber = 135; // DCE (see: https://en.wikipedia.org/wiki/List_of_TCP_and_UDP_port_numbers)
            PortScanner instance = new PortScanner(endPointName, portNumber);

            instance.PortScanResult += (sender, e) =>
            {
                hadSuccess = true;
            };

            instance.AttemptUdpConnectionToPort();

            Assert.IsFalse(hadSuccess);
        }
    }
}
