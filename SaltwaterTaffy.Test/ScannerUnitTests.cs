using System;
using System.Net.Sockets;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SaltwaterTaffy.Container;
using Simple.DotNMap;

namespace SaltwaterTaffy.Test
{
    [TestClass]
    public class ScanResultUnitTests
    {
        [TestMethod]
        public void when_ScanResult_constructed_with_empty_nmaprun_then_Hosts_should_be_empty()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.IsFalse(sr.Hosts.Any());
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_Total_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "5643",
                                    down = "0",
                                    up = "0"
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual(5643, sr.Total);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_Down_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "5643",
                                    up = "0"
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual(5643, sr.Down);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_Up_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "5643"
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual(5643, sr.Up);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_Address_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new hostnames
                                                {
                                                    hostname = new[]
                                                        {
                                                            new hostname()
                                                        }
                                                }
                                        }

                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual("127.0.0.1", sr.Hosts.First().Address.ToString());
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_Hostname_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new hostnames
                                                {
                                                    hostname = new[]
                                                        {
                                                            new hostname
                                                                {
                                                                    name = "example.com"
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual("example.com", sr.Hosts.First().Hostnames.First());
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_ExtraPorts_Count_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    extraports = new[]
                                                        {
                                                            new extraports
                                                                {
                                                                    count = "5643",
                                                                    state = string.Empty
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual(5643, sr.Hosts.First().ExtraPorts.First().Count);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_and_no_extraports_then_ExtraPorts_should_be_empty()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[] {}
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.IsFalse(sr.Hosts.First().ExtraPorts.Any());
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_ExtraPorts_State_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    extraports = new[]
                                                        {
                                                            new extraports
                                                                {
                                                                    count = "5643",
                                                                    state = "parsed"
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual("parsed", sr.Hosts.First().ExtraPorts.First().State);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_Port_PortNumber_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.tcp,
                                                                    state = new state
                                                                        {
                                                                            state1 = "parsed"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual(5643, sr.Hosts.First().Ports.First().PortNumber);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_with_tcp_as_protocol_then_Port_Protocol_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.tcp,
                                                                    state = new state
                                                                        {
                                                                            state1 = "parsed"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual(ProtocolType.Tcp, sr.Hosts.First().Ports.First().Protocol);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_with_udp_as_protocol_then_Port_Protocol_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.udp,
                                                                    state = new state
                                                                        {
                                                                            state1 = "parsed"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual(ProtocolType.Udp, sr.Hosts.First().Ports.First().Protocol);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_with_sctp_as_protocol_then_Port_Protocol_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.sctp,
                                                                    state = new state
                                                                        {
                                                                            state1 = "parsed"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            // SCTP isn't supported by Windows by default, so Unknown seems the most appropriate protocol to return
            Assert.AreEqual(ProtocolType.Unknown, sr.Hosts.First().Ports.First().Protocol);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_with_ip_as_protocol_then_Port_Protocol_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.ip,
                                                                    state = new state
                                                                        {
                                                                            state1 = "parsed"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual(ProtocolType.IP, sr.Hosts.First().Ports.First().Protocol);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_and_port_is_filtered_then_Port_Filtered_should_be_true()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.ip,
                                                                    state = new state
                                                                        {
                                                                            state1 = "filtered"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.IsTrue(sr.Hosts.First().Ports.First().Filtered);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_and_port_is_not_filtered_then_Port_Filtered_should_be_false
            ()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.ip,
                                                                    state = new state
                                                                        {
                                                                            state1 = "notfiltered"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.IsFalse(sr.Hosts.First().Ports.First().Filtered);
        }

        [TestMethod]
        public void
            when_ScanResult_constructed_with_nmaprun_and_service_is_not_present_then_Port_Service_should_be_default()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.ip,
                                                                    state = new state
                                                                        {
                                                                            state1 = "herpderp"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual(default(Service), sr.Hosts.First().Ports.First().Service);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_and_service_is_present_then_Service_Name_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.ip,
                                                                    state = new state
                                                                        {
                                                                            state1 = "herpderp"
                                                                        },
                                                                    service = new service
                                                                        {
                                                                            name = "Foobar",
                                                                            product = "Bizbaz",
                                                                            ostype = "DragonFly BSD",
                                                                            version = "2.718281828"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual("Foobar", sr.Hosts.First().Ports.First().Service.Name);
        }

        [TestMethod]
        public void
            when_ScanResult_constructed_with_nmaprun_and_service_is_present_then_Service_Product_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.ip,
                                                                    state = new state
                                                                        {
                                                                            state1 = "herpderp"
                                                                        },
                                                                    service = new service
                                                                        {
                                                                            name = "Foobar",
                                                                            product = "Bizbaz",
                                                                            ostype = "DragonFly BSD",
                                                                            version = "2.718281828"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual("Bizbaz", sr.Hosts.First().Ports.First().Service.Product);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_and_service_is_present_then_Service_Os_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.ip,
                                                                    state = new state
                                                                        {
                                                                            state1 = "herpderp"
                                                                        },
                                                                    service = new service
                                                                        {
                                                                            name = "Foobar",
                                                                            product = "Bizbaz",
                                                                            ostype = "DragonFly BSD",
                                                                            version = "2.718281828"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual("DragonFly BSD", sr.Hosts.First().Ports.First().Service.Os);
        }

        [TestMethod]
        public void
            when_ScanResult_constructed_with_nmaprun_and_service_is_present_then_Service_Version_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new ports
                                                {
                                                    port = new[]
                                                        {
                                                            new port
                                                                {
                                                                    portid = "5643",
                                                                    protocol = portProtocol.ip,
                                                                    state = new state
                                                                        {
                                                                            state1 = "herpderp"
                                                                        },
                                                                    service = new service
                                                                        {
                                                                            name = "Foobar",
                                                                            product = "Bizbaz",
                                                                            ostype = "DragonFly BSD",
                                                                            version = "2.718281828"
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual("2.718281828", sr.Hosts.First().Ports.First().Service.Version);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_and_no_ports_then_Ports_should_be_empty()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[] {}
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.IsFalse(sr.Hosts.First().Ports.Any());
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_and_no_hostnames_then_Hostnames_should_be_empty()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[] {}
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.IsFalse(sr.Hosts.First().Hostnames.Any());
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_and_no_osmatches_then_OsMatches_should_be_empty()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[] {}
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.IsFalse(sr.Hosts.First().OsMatches.Any());
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_OsMatches_Certainty_should_be_correct()
        {
            var nr = new nmaprun
                {
                    runstats = new runstats
                        {
                            hosts = new hosts
                                {
                                    total = "0",
                                    down = "0",
                                    up = "0"
                                }
                        },
                    Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new os
                                                {
                                                    osmatch = new []
                                                        {
                                                            new osmatch
                                                                {
                                                                    accuracy = "100",
                                                                    name = "Temple OS (www.templeos.org)",
                                                                    osclass = new []
                                                                        {
                                                                            new osclass
                                                                                {
                                                                                    accuracy = "100",
                                                                                    osfamily = "Temple",
                                                                                    osgen = "apocalypse",
                                                                                    vendor = "Terry A. Davis"
                                                                                }
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
                };
            var sr = new ScanResult(nr);

            Assert.AreEqual(100, sr.Hosts.First().OsMatches.First().Certainty);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_OsMatches_Name_should_be_correct()
        {
            var nr = new nmaprun
            {
                runstats = new runstats
                {
                    hosts = new hosts
                    {
                        total = "0",
                        down = "0",
                        up = "0"
                    }
                },
                Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new os
                                                {
                                                    osmatch = new []
                                                        {
                                                            new osmatch
                                                                {
                                                                    accuracy = "100",
                                                                    name = "Temple OS (www.templeos.org)",
                                                                    osclass = new []
                                                                        {
                                                                            new osclass
                                                                                {
                                                                                    accuracy = "100",
                                                                                    osfamily = "Temple",
                                                                                    osgen = "apocalypse",
                                                                                    vendor = "Terry A. Davis"
                                                                                }
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
            };
            var sr = new ScanResult(nr);

            Assert.AreEqual("Temple OS (www.templeos.org)", sr.Hosts.First().OsMatches.First().Name);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_OsMatches_Family_should_be_correct()
        {
            var nr = new nmaprun
            {
                runstats = new runstats
                {
                    hosts = new hosts
                    {
                        total = "0",
                        down = "0",
                        up = "0"
                    }
                },
                Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new os
                                                {
                                                    osmatch = new []
                                                        {
                                                            new osmatch
                                                                {
                                                                    accuracy = "100",
                                                                    name = "Temple OS (www.templeos.org)",
                                                                    osclass = new []
                                                                        {
                                                                            new osclass
                                                                                {
                                                                                    accuracy = "100",
                                                                                    osfamily = "Temple",
                                                                                    osgen = "apocalypse",
                                                                                    vendor = "Terry A. Davis"
                                                                                }
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
            };
            var sr = new ScanResult(nr);

            Assert.AreEqual("Temple", sr.Hosts.First().OsMatches.First().Family);
        }

        [TestMethod]
        public void when_ScanResult_constructed_with_nmaprun_then_OsMatches__should_be_correct()
        {
            var nr = new nmaprun
            {
                runstats = new runstats
                {
                    hosts = new hosts
                    {
                        total = "0",
                        down = "0",
                        up = "0"
                    }
                },
                Items = new object[]
                        {
                            new host
                                {
                                    address = new address
                                        {
                                            addr = "127.0.0.1"
                                        },
                                    Items = new object[]
                                        {
                                            new os
                                                {
                                                    osmatch = new []
                                                        {
                                                            new osmatch
                                                                {
                                                                    accuracy = "100",
                                                                    name = "Temple OS (www.templeos.org)",
                                                                    osclass = new []
                                                                        {
                                                                            new osclass
                                                                                {
                                                                                    accuracy = "100",
                                                                                    osfamily = "Temple",
                                                                                    osgen = "apocalypse",
                                                                                    vendor = "Terry A. Davis"
                                                                                }
                                                                        }
                                                                }
                                                        }
                                                }
                                        }
                                }
                        }
            };
            var sr = new ScanResult(nr);

            Assert.AreEqual("apocalypse", sr.Hosts.First().OsMatches.First().Generation);
        }
    }
}
