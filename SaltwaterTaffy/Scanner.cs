using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using SaltwaterTaffy;
using SaltwaterTaffy.Container;
using SaltwaterTaffy.Utility;
using Simple.DotNMap;

namespace SaltwaterTaffy
{
    /// <summary>
    /// Represents the result of an nmap run
    /// </summary>
    public class ScanResult
    {
        public int Total { get; set; }
        public int Up { get; set; }
        public int Down { get; set; }
        public IEnumerable<Host> Hosts { get; set; }

        /// <summary>
        /// Process the "ports" section of the XML document
        /// </summary>
        /// <param name="portsSection">Object representing the ports section</param>
        /// <returns>A collection of Port objects containing information about each individual port</returns>
        private static IEnumerable<Port> PortsSection(ports portsSection)
        {
            return portsSection != null && portsSection.port != null
                       ? portsSection.port.Select(
                           x => new Port
                               {
                                   PortNumber = int.Parse(x.portid),
                                   Protocol =
                                       (ProtocolType)
                                       Enum.Parse(typeof (ProtocolType),
                                                  x.protocol.ToString().Capitalize()),
                                   Filtered = x.state.state1 == "filtered",
                                   Service = x.service != null
                                                 ? new Service
                                                     {
                                                         Name = x.service.name,
                                                         Product = x.service.product,
                                                         Os = x.service.ostype,
                                                         Version = x.service.version
                                                     }
                                                 : default(Service)
                               }
                             )
                       : Enumerable.Empty<Port>();
        }

        /// <summary>
        /// Process the "extraports" section of the XML document (contains large numbers of ports in the same state)
        /// </summary>
        /// <param name="portsSection">Object representing the ports section</param>
        /// <returns>A collection of ExtraPorts objects if the extraports section exists, empty otherwise</returns>
        private static IEnumerable<ExtraPorts> ExtraPortsSection(ports portsSection)
        {
            return portsSection != null && portsSection.extraports != null
                       ? portsSection.extraports.Select(
                           x => new ExtraPorts
                               {
                                   Count = int.Parse(x.count),
                                   State = x.state
                               })
                       : Enumerable.Empty<ExtraPorts>();
        }

        /// <summary>
        /// Process the "hostnames" section of the XML document
        /// </summary>
        /// <param name="names">Object representing the hostnames section</param>
        /// <returns>A collection of hostnames as strings if the hostname exists, empty otherwise</returns>
        private static IEnumerable<string> HostnamesSection(hostnames names)
        {
            return names != null && names.hostname != null
                    ? names.hostname.Select(x => x.name)
                    : Enumerable.Empty<string>();
        }

        /// <summary>
        /// Process the "os" section of the XML document
        /// </summary>
        /// <param name="osSection">Object representing the hos section</param>
        /// <returns>A collection of Os objects if osmatch is not null, empty otherwise</returns>
        private static IEnumerable<Os> OsMatchesSection(os osSection)
        {
            return osSection != null && osSection.osmatch != null
                       ? osSection.osmatch.Select(
                           x => new Os
                               {
                                   Certainty = int.Parse(x.accuracy),
                                   Name = x.name,
                                   Family = x.osclass[0].osfamily,
                                   Generation = x.osclass[0].osgen
                               })
                       : Enumerable.Empty<Os>();

        }

        /// <summary>
        /// Use the provided raw nmaprun object to construct a more sane ScanResult object which contains information about the nmap run
        /// </summary>
        /// <param name="result">The result of parsing an nmaprun </param>
        public ScanResult(nmaprun result)
        {
            Total = int.Parse(result.runstats.hosts.total);
            Up = int.Parse(result.runstats.hosts.up);
            Down = int.Parse(result.runstats.hosts.down);
            Hosts = result.Items != null
                        ? result.Items.OfType<host>().Select(
                            x => new Host
                                {
                                    Address = IPAddress.Parse(x.address.addr),
                                    Ports =
                                        PortsSection(
                                            x.Items.OfType<ports>().DefaultIfEmpty(null).FirstOrDefault()),
                                    ExtraPorts =
                                        ExtraPortsSection(
                                            x.Items.OfType<ports>().DefaultIfEmpty(null).FirstOrDefault()),
                                    Hostnames =
                                        HostnamesSection(
                                            x.Items.OfType<hostnames>().DefaultIfEmpty(null).FirstOrDefault()),
                                    OsMatches = OsMatchesSection(
                                        x.Items.OfType<os>().DefaultIfEmpty(null).FirstOrDefault())
                                })
                        : Enumerable.Empty<Host>();
        }
    }

    /// <summary>
    /// Enumeration of potential scan types, including TCP, UDP, and SCTP scans
    /// </summary>
    public enum ScanType
    {
        // Perform a scan of our choice
        Default,

        // TCP scans
        Null, Fin, Xmas, Syn, Connect, Ack, Window, Maimon,

        // SCTP scans
        SctpInit, SctpCookieEcho,

        // UDP-only scan
        Udp
    }

    /// <summary>
    /// High-level scanner object for performing network reconnaissance using nmap
    /// </summary>
    public class Scanner
    {
        private readonly Dictionary<ScanType, NmapFlag> _scanTypeToNmapFlag = new Dictionary<ScanType, NmapFlag>
            {
                {ScanType.Null, NmapFlag.TcpNullScan},
                {ScanType.Fin, NmapFlag.FinScan},
                {ScanType.Xmas, NmapFlag.XmasScan},
                {ScanType.Syn, NmapFlag.TcpSynScan},
                {ScanType.Connect, NmapFlag.ConnectScan},
                {ScanType.Ack, NmapFlag.AckScan},
                {ScanType.Window, NmapFlag.WindowScan},
                {ScanType.Maimon, NmapFlag.MaimonScan},
                {ScanType.SctpInit, NmapFlag.SctpInitScan},
                {ScanType.SctpCookieEcho, NmapFlag.CookieEchoScan}
            };

        /// <summary>
        /// Intended target.
        /// </summary>
        public Target Target { get; set; }

        /// <summary>
        /// NmapOptions that should persist between runs (e.g., --exclude foobar)
        /// </summary>
        public NmapOptions PersistentOptions { get; set; }

        /// <summary>
        /// Create a new scanner with an intended target
        /// </summary>
        /// <param name="target">Intended target</param>
        public Scanner(Target target)
        {
            Target = target;
        }

        /// <summary>
        /// Create a new NmapContext with the intended target and our persistent options
        /// </summary>
        /// <returns>NmapContext with the intended target and our persistent options</returns>
        private NmapContext GetContext()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                throw new ApplicationException("No network reachable");
            }

            var ctx = new NmapContext
            {
                Target = Target.ToString()
            };

            if (PersistentOptions != null)
            {
                ctx.Options.AddAll(PersistentOptions);
            }

            return ctx;
        }

        /// <summary>
        /// Perform host discovery and OS detection on the intended target (preferably a subnet or IP range)
        /// </summary>
        /// <returns>A collection of Hosts detailing the results of the discovery</returns>
        public IEnumerable<Host> HostDiscovery()
        {
            var ctx = GetContext();
            ctx.Options.AddAll(new[]
                {
                    NmapFlag.PingScan,
                    NmapFlag.OsDetection
                });
            ctx.Options.Add(NmapFlag.OsDetection);
             
            return new ScanResult(ctx.Run()).Hosts;
        }

        /// <summary>
        /// Determine whether the intended target is firewalled.
        /// </summary>
        /// <returns>Returns true if the intended targer is firewalled and false otherwise. If used on a subnet or IP range, this determines if any host has a firewall.</returns>
        public bool FirewallProtected()
        {
            var ctx = GetContext();
            ctx.Options.AddAll(new [] {
                NmapFlag.AckScan,
                NmapFlag.FragmentPackets
            });

            var sr = new ScanResult(ctx.Run());

            return
                sr.Hosts.Any(
                    x => (x.ExtraPorts.First().Count > 0 && x.ExtraPorts.First().State == "filtered") || x.Ports.Any(y => y.Filtered));
        }

        /// <summary>
        /// Build an nmap context with the specified options
        /// </summary>
        /// <param name="scanType">The desired type of scan to perform</param>
        /// <param name="ports">The ports to scan (null of empty for default ports)</param>
        /// <returns>An nmap context for performing the desired scan</returns>
        private NmapContext _portScanCommon(ScanType scanType, string ports)
        {
            var ctx = GetContext();

            // We always try to detect the OS and the service versions
            ctx.Options.AddAll(new[]
                {
                    NmapFlag.ServiceVersion,
                    NmapFlag.OsDetection
                });

            // Add the appropriate flag if we're not performing a default scan
            if (scanType != ScanType.Default)
            {
                ctx.Options.Add(_scanTypeToNmapFlag[scanType]);
            }

            // Unless we specify UDP only, then perform a UDP scan in the same run
            if (scanType != ScanType.Default && scanType != ScanType.Udp)
            {
                ctx.Options.Add(NmapFlag.UdpScan);
            }

            // If we have a port specification, then use it
            if (ports != null && !string.IsNullOrEmpty(ports))
            {
                ctx.Options.Add(NmapFlag.PortSpecification, ports);
            }

            return ctx;
        }

        /// <summary>
        /// Perform a TCP port scan with service detection and OS detection.
        /// </summary>
        /// <returns>A ScanResult object detailing the results of the port scan</returns>
        public ScanResult PortScan()
        {
            var ctx = _portScanCommon(ScanType.Default, null);
            return new ScanResult(ctx.Run());
        }

        /// <summary>
        /// Perform the desired scan with service detection and OS detection.
        /// </summary>
        /// <returns>A ScanResult object detailing the results of the port scan</returns>
        public ScanResult PortScan(ScanType scanType)
        {
            var ctx = _portScanCommon(scanType, null);
            return new ScanResult(ctx.Run());
        }

        /// <summary>
        /// Perform a TCP port scan on the specified ports with service detection and OS detection.
        /// </summary>
        /// <param name="scanType">The type of scan to perform</param>
        /// <param name="ports">A list of ports to scan</param>
        /// <returns>A ScanResult object detailing the results of the port scan</returns>
        public ScanResult PortScan(ScanType scanType, IEnumerable<int> ports)
        {
            var ctx = _portScanCommon(scanType, string.Join(",", ports.Select(x => x.ToString(CultureInfo.InvariantCulture))));
            return new ScanResult(ctx.Run());
        }

        /// <summary>
        /// Perform a TCP port scan on the specified ports with service detection and OS detection.
        /// </summary>
        /// <param name="scanType">The type of scan to perform</param>
        /// <param name="ports">A string detailing which ports to scan (e.g., "10-20,33")</param>
        /// <returns>A ScanResult object detailing the results of the port scan</returns>
        public ScanResult PortScan(ScanType scanType, string ports)
        {
            var ctx = _portScanCommon(scanType, ports);
            return new ScanResult(ctx.Run());
        }

        /// <summary>
        /// Yield a list of our own network interfaces (first half of nmap --iflist)
        /// </summary>
        /// <returns>A list of our network interfaces</returns>
        public NetworkInterface[] GetAllHostNetworkInterfaces()
        {
            return NetworkInterface.GetAllNetworkInterfaces();
        }
    }
}
