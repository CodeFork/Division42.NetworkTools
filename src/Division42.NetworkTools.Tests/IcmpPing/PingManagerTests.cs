using System;
using System.Threading;
using Division42.NetworkTools.IcmpPing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Division42.NetworkTools.Tests.IcmpPing
{
    [TestClass]
    public class PingManagerTests
    {
        [TestMethod]
        public void ConstructorWithValidArguments_ShouldReturnInstance()
        {
            String hostName = "localhost";
            TimeSpan timeBetweenPings = new TimeSpan(0, 0, 1);
            IPingManager instance = new PingManager(hostName, timeBetweenPings);

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void PingWithValidSettings_ShouldFirePingResultEventWithSuccess()
        {
            String hostName = "localhost";
            TimeSpan timeBetweenPings = new TimeSpan(0, 0, 1);
            IPingManager instance = new PingManager(hostName, timeBetweenPings);
            Boolean expected = true;
            Boolean actual = false;

            instance.PingResult += (sender, e) =>
            {
                actual = e.Success; // Only set if the event fired.
            };

            instance.Start();
            Thread.Sleep(2000);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void PingWithInvalidSettings_ShouldFirePingResultEventWithFailure()
        {
            String hostName = "invalidhostname";
            TimeSpan timeBetweenPings = new TimeSpan(0, 0, 1);
            IPingManager instance = new PingManager(hostName, timeBetweenPings);
            Boolean expected = true;
            Boolean actual = false;

            instance.PingResult += (sender, e) =>
            {
                actual = !e.Success; // Only set if the event fired.
            };

            instance.Start();
            Thread.Sleep(5000);

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void StartAgainWhileAlreadyRunning_ShouldIgnoreAndReturn()
        {
            String hostName = "localhost";
            TimeSpan timeBetweenPings = new TimeSpan(0, 0, 1);
            IPingManager instance = new PingManager(hostName, timeBetweenPings);
            Boolean expected = true;
            Boolean actual = false;

            instance.PingResult += (sender, e) =>
            {
                actual = e.Success; // Only set if the event fired.
            };

            instance.Start();
            instance.Start();

            // No exception should be thrown.
        }

        [TestMethod]
        public void StopAfterRunning_ShouldFireEvent()
        {
            String hostName = "localhost";
            TimeSpan timeBetweenPings = new TimeSpan(0, 0, 1);
            IPingManager instance = new PingManager(hostName, timeBetweenPings);
            Boolean expected = true;
            Boolean actual = false;

            instance.PingManagerStateChanged += (sender, e) =>
            {
                actual = (e.NewState == PingManagerStates.Stopped);
            };

            instance.Start();
            instance.Stop ();

            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithNullHost_ShouldThrowException()
        {
            String hostName = null;
            TimeSpan timeBetweenPings = new TimeSpan(0, 0, 1);

            IPingManager instance = new PingManager(hostName, timeBetweenPings);

            Assert.Fail("Should have thrown ArgumentException.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ConstructorWithEmptyHost_ShouldThrowException()
        {
            String hostName = String.Empty;
            TimeSpan timeBetweenPings = new TimeSpan(0, 0, 1);

            IPingManager instance = new PingManager(hostName, timeBetweenPings);

            Assert.Fail("Should have thrown ArgumentException.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructorWithInvalidTimeSpan_ShouldThrowException()
        {
            String hostName = "localhost";
            TimeSpan timeBetweenPings = new TimeSpan(0, 0, 0);

            IPingManager instance = new PingManager(hostName, timeBetweenPings);

            Assert.Fail("Should have thrown ArgumentOutOfRangeException.");
        }
    }
}
