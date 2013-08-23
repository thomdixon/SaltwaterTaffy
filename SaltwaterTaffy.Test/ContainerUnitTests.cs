using System;
using System.Net;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltwaterTaffy.Container;

namespace SaltwaterTaffy.Test
{
    [TestClass]
    public class TargetUnitTests
    {
        [TestMethod]
        public void when_Target_constructed_with_strings_then_ToString_should_yield_space_separated_target()
        {
            var t = new Target(new[] {"this", "is", "a", "test"});
            Assert.AreEqual("this is a test", t.ToString());
        }

        [TestMethod]
        public void when_Target_constructed_with_string_then_ToString_should_yield_target()
        {
            var t = new Target("this is a test");
            Assert.AreEqual("this is a test", t.ToString());
        }

        [TestMethod]
        public void when_Target_constructed_with_IPAddress_then_ToString_should_yield_target()
        {
            var t = new Target(IPAddress.Parse("127.0.0.1"));
            Assert.AreEqual("127.0.0.1", t.ToString());
        }

        [TestMethod]
        public void when_Target_constructed_with_IPAddresses_then_ToString_should_yield_space_separated_target()
        {
            var t = new Target(new[] { IPAddress.Parse("127.0.0.1"), IPAddress.Parse("127.0.0.2"), IPAddress.Parse("127.0.0.3") });
            Assert.AreEqual("127.0.0.1 127.0.0.2 127.0.0.3", t.ToString());
        }
    }
}
