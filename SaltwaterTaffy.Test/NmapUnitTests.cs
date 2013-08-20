using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltwaterTaffy.Container;
using Simple.DotNMap;

namespace SaltwaterTaffy.Test
{
    [TestClass]
    public class NmapUnitTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var c = new NmapContext();
            c.Options.Add(NmapFlag.AckScan);
            c.Target = "192.168.2.4";
            var foo = c.Run();
            var sr = new ScanResult(foo);
            Console.WriteLine();
        }


        [TestMethod]
        public void TestMethod2()
        {
            var s = new Scanner(new Target("192.168.2.6"));
            var r = s.PortScan(ScanType.Syn);
            var q = s.FirewallProtected();
            Console.WriteLine();
        }
    }
}
