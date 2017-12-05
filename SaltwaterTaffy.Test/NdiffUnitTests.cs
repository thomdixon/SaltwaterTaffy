using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace SaltwaterTaffy.Test
{
    [TestClass]
    public class NdiffUnitTests
    {

        public static string TestFileDir = Environment.CurrentDirectory + "\\TestFiles";

        [TestMethod]
        public void diff_two_nmap_output_files_good_files()
        {
            var file1 = Path.Combine(TestFileDir, "scanme.nmap.org.xml");
            var file2 = Path.Combine(TestFileDir, "scanme.nmap.org_modified.xml");

            var verbose = false;
            var xml = false;

            var output = Ndiff.RunDiff(verbose, xml, file1, file2);


            Assert.AreEqual(output, "\r\n scanme.nmap.org (45.33.32.156):\r\n PORT   STATE SERVICE VERSION\r\n-22/tcp open  ssh     OpenSSH 6.6.1p1 Ubuntu 2ubuntu2.8 (Ubuntu Linux; protocol 2.0)\r\n");
        }

        [TestMethod]
        public void diff_two_nmap_output_files_non_existent_files()
        {
            var file1 = Path.Combine(TestFileDir, "im_not_here.xml");
            var file2 = Path.Combine(TestFileDir, "me_neither.xml");

            var verbose = false;
            var xml = false;

            var output = Ndiff.RunDiff(verbose, xml, file1, file2);

            var fail = output.Contains("Can't open file: ") ? true : false;

            Assert.IsTrue(fail);
        }
    }
}
