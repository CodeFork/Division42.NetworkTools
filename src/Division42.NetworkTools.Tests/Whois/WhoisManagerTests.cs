using System;
using System.Collections.Generic;
using System.Linq;
using Division42.NetworkTools.Whois;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Division42.NetworkTools.Tests.Whois
{
    [TestClass]
    public class WhoisManagerTests
    {
        [TestMethod]
        public void ConstructorWithNoArguments_ReturnsInstance()
        {
            WhoisManager instance = new WhoisManager();

            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void ExecuteWhoisForDomainWithValidArguments_ReturnsExpected()
        {
            WhoisManager instance = new WhoisManager();

            String results = instance.ExecuteWhoisForDomain("division42.com");

            Assert.IsNotNull(results);
        }

        [TestMethod]
        public void FindWhoisServerInOutputWithValidArguments_ReturnsExpected()
        {
            WhoisManager instance = new WhoisManager();

            String results = instance.ExecuteWhoisForDomain("division42.com");

            IEnumerable<String> whoisServers = instance.FindWhoisServerInOutput(results);



            Assert.IsNotNull(whoisServers);
        }

        [TestMethod]
        public void EndToEndTestWithValidArguments_ReturnsExpected()
        {
            WhoisManager instance = new WhoisManager();

            String results = instance.ExecuteWhoisForDomain("division42.com");

            IEnumerable<String> whoisServers = instance.FindWhoisServerInOutput(results);

            String actualResults = instance.ExecuteWhoisForDomain("division42.com", whoisServers.First());

            Assert.IsNotNull(actualResults);
        }
    }
}
