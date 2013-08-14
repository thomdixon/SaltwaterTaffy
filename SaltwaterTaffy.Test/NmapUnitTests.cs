using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            c.Options.Add(NmapFlag.FastScanMode);
            c.Targets = new List<string> {"scanme.nmap.org"};
            var foo = c.Run();
            var sr = new ScanResult(foo);
            Console.WriteLine();
        }
    }
}
