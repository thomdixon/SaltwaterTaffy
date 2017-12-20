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
        public void diff_two_nmap_text_output_files_good_files()
        {
            var file1 = Path.Combine(TestFileDir, "scanme.nmap.org.xml");
            var file2 = Path.Combine(TestFileDir, "scanme.nmap.org_modified.xml");

            var options = new NdiffOptions();
            options.Add(NdiffFlag.Text, string.Empty);

            var ndiff = new NdiffContext()
            {
                File1 = file1,
                File2 = file2,
                Options = options
            };

            var output = ndiff.Run();


            Assert.AreEqual(output, "\r\n scanme.nmap.org (45.33.32.156):\r\n PORT   STATE SERVICE VERSION\r\n-22/tcp open  ssh     OpenSSH 6.6.1p1 Ubuntu 2ubuntu2.8 (Ubuntu Linux; protocol 2.0)\r\n");
        }

        [TestMethod]
        public void diff_two_nmap_text_verbose_output_files_good_files()
        {
            var file1 = Path.Combine(TestFileDir, "scanme.nmap.org.xml");
            var file2 = Path.Combine(TestFileDir, "scanme.nmap.org_modified.xml");

            var options = new NdiffOptions();
            options.Add(NdiffFlag.Verbose, string.Empty);
            options.Add(NdiffFlag.Text, string.Empty);

            var ndiff = new NdiffContext()
            {
                File1 = file1,
                File2 = file2,
                Options = options
            };

            var output = ndiff.Run();


            Assert.AreEqual(output, " Nmap 7.60 scan initiated Tue Dec 05 10:06:28 2017 as: nmap -T4 -A -v scanme.nmap.org\r\n\r\n scanme.nmap.org (45.33.32.156):\r\n Host is up.\r\n Not shown: 994 closed ports\r\n PORT     STATE    SERVICE      VERSION\r\n-22/tcp   open     ssh          OpenSSH 6.6.1p1 Ubuntu 2ubuntu2.8 (Ubuntu Linux; protocol 2.0)\r\n 25/tcp   filtered smtp\r\n 80/tcp   open     http         Apache httpd 2.4.7 ((Ubuntu))\r\n 110/tcp  filtered pop3\r\n 445/tcp  filtered microsoft-ds\r\n 9929/tcp open     nping-echo   Nping echo\r\n");
        }

        [TestMethod]
        public void diff_two_nmap_xml_output_files_good_files()
        {
            var file1 = Path.Combine(TestFileDir, "scanme.nmap.org.xml");
            var file2 = Path.Combine(TestFileDir, "scanme.nmap.org_modified.xml");

            var options = new NdiffOptions();
            options.Add(NdiffFlag.Xml, string.Empty);

            var ndiff = new NdiffContext()
            {
                File1 = file1,
                File2 = file2,
                Options = options
            };

            var output = ndiff.Run();


            Assert.AreEqual(output, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<nmapdiff version=\"1\"><scandiff><hostdiff>\r\n<host>\r\n<address addr=\"45.33.32.156\" addrtype=\"ipv4\"/>\r\n<hostnames>\r\n<hostname name=\"scanme.nmap.org\"/>\r\n</hostnames>\r\n<ports>\r\n<extraports count=\"994\" state=\"closed\"/>\r\n<portdiff>\r\n<a>\r\n<port portid=\"22\" protocol=\"tcp\">\r\n<state state=\"open\"/>\r\n<service extrainfo=\"Ubuntu Linux; protocol 2.0\" name=\"ssh\" product=\"OpenSSH\" version=\"6.6.1p1 Ubuntu 2ubuntu2.8\"/>\r\n</port>\r\n</a>\r\n<b>\r\n<port portid=\"22\" protocol=\"tcp\">\r\n<state state=\"closed\"/>\r\n<service extrainfo=\"Ubuntu Linux; protocol 2.0\" name=\"ssh\" product=\"OpenSSH\" version=\"6.6.1p1 Ubuntu 2ubuntu2.8\"/>\r\n</port>\r\n</b>\r\n</portdiff>\r\n</ports>\r\n</host>\r\n</hostdiff>\r\n</scandiff></nmapdiff>");
        }

        [TestMethod]
        public void diff_two_nmap_xml_verbose_output_files_good_files()
        {
            var file1 = Path.Combine(TestFileDir, "scanme.nmap.org.xml");
            var file2 = Path.Combine(TestFileDir, "scanme.nmap.org_modified.xml");

            var options = new NdiffOptions();
            options.Add(NdiffFlag.Verbose, string.Empty);
            options.Add(NdiffFlag.Xml, string.Empty);

            var ndiff = new NdiffContext()
            {
                File1 = file1,
                File2 = file2,
                Options = options
            };

            var output = ndiff.Run();


            Assert.AreEqual(output, "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<nmapdiff version=\"1\"><scandiff><nmaprun args=\"nmap -T4 -A -v scanme.nmap.org\" scanner=\"nmap\" start=\"1512489988\" startstr=\"Tue Dec 05 10:06:28 2017\" version=\"7.60\"/>\r\n<hostdiff>\r\n<host>\r\n<status state=\"up\"/>\r\n<address addr=\"45.33.32.156\" addrtype=\"ipv4\"/>\r\n<hostnames>\r\n<hostname name=\"scanme.nmap.org\"/>\r\n</hostnames>\r\n<ports>\r\n<extraports count=\"994\" state=\"closed\"/>\r\n<portdiff>\r\n<a>\r\n<port portid=\"22\" protocol=\"tcp\">\r\n<state state=\"open\"/>\r\n<service extrainfo=\"Ubuntu Linux; protocol 2.0\" name=\"ssh\" product=\"OpenSSH\" version=\"6.6.1p1 Ubuntu 2ubuntu2.8\"/>\r\n</port>\r\n</a>\r\n<b>\r\n<port portid=\"22\" protocol=\"tcp\">\r\n<state state=\"closed\"/>\r\n<service extrainfo=\"Ubuntu Linux; protocol 2.0\" name=\"ssh\" product=\"OpenSSH\" version=\"6.6.1p1 Ubuntu 2ubuntu2.8\"/>\r\n</port>\r\n</b>\r\n</portdiff>\r\n<port portid=\"25\" protocol=\"tcp\">\r\n<state state=\"filtered\"/>\r\n<service name=\"smtp\"/>\r\n</port>\r\n<port portid=\"80\" protocol=\"tcp\">\r\n<state state=\"open\"/>\r\n<service extrainfo=\"(Ubuntu)\" name=\"http\" product=\"Apache httpd\" version=\"2.4.7\"/>\r\n</port>\r\n<port portid=\"110\" protocol=\"tcp\">\r\n<state state=\"filtered\"/>\r\n<service name=\"pop3\"/>\r\n</port>\r\n<port portid=\"445\" protocol=\"tcp\">\r\n<state state=\"filtered\"/>\r\n<service name=\"microsoft-ds\"/>\r\n</port>\r\n<port portid=\"9929\" protocol=\"tcp\">\r\n<state state=\"open\"/>\r\n<service name=\"nping-echo\" product=\"Nping echo\"/>\r\n</port>\r\n</ports>\r\n</host>\r\n</hostdiff>\r\n</scandiff></nmapdiff>");
        }

        [TestMethod]
        public void diff_two_nmap_output_files_non_existent_files()
        {
            var file1 = Path.Combine(TestFileDir, "im_not_here.xml");
            var file2 = Path.Combine(TestFileDir, "me_neither.xml");
            
            var options = new NdiffOptions();
            options.Add(NdiffFlag.Verbose, string.Empty);
            options.Add(NdiffFlag.Xml, string.Empty);

            var ndiff = new NdiffContext()
            {
                File1 = file1,
                File2 = file2,
                Options = options
            };

            var output = ndiff.Run();

            var fail = output.Contains("Can't open file: ") ? true : false;

            Assert.IsTrue(fail);
        }
    }
}
