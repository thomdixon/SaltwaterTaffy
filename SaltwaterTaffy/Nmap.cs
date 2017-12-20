// This file is part of SaltwaterTaffy, an nmap wrapper library for .NET
// Copyright (C) 2013 Thom Dixon <thom@thomdixon.org>
// Released under the GNU GPLv2 or any later version
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Simple.DotNMap;
using Simple.DotNMap.Extensions;

namespace SaltwaterTaffy
{
    public enum NmapFlag
    {
        InputFilename,
        RandomTargets,
        ExcludeHosts,
        ExcludeFile,
        ListScan,
        PingScan,
        TreatHostsAsOnline,
        TcpSynDiscovery,
        AckDiscovery,
        UdpDiscovery,
        SctpDiscovery,
        IcmpEchoDiscovery,
        IcmpTimestampDiscovery,
        IcmpNetmaskDiscovery,
        ProtocolPing,
        NeverDnsResolve,
        DnsServers,
        SystemDns,
        Traceroute,
        HostScan,
        TcpSynScan,
        ConnectScan,
        AckScan,
        WindowScan,
        MaimonScan,
        UdpScan,
        TcpNullScan,
        FinScan,
        XmasScan,
        ScanFlags,
        IdleScan,
        SctpInitScan,
        CookieEchoScan,
        IpProtocolScan,
        FtpBounceScan,
        PortSpecification,
        FastScanMode,
        ScanPortsConsecutively,
        TopPorts,
        PortRatio,
        ServiceVersion,
        VersionIntensity,
        VersionLight,
        VersionAll,
        VersionTrace,
        DefaultScriptScan,
        Script,
        ScriptArgs,
        ScriptTrace,
        ScriptUpdateDb,
        OsDetection,
        OsScanLimit,
        OsScanGuess,
        ParanoidTiming,
        SneakyTiming,
        PoliteTiming,
        NormalTiming,
        AggressiveTiming,
        InsaneTiming,
        ParallelMinHostGroupSize,
        ParallelMaxHostGroupSize,
        MinProbeParallelization,
        MaxProbeParallelization,
        MinRttTimeout,
        MaxRttTimeout,
        InitialRttTimeout,
        MaxRetries,
        HostTimeout,
        ScanDelay,
        MaxScanDelay,
        MinRate,
        MaxRate,
        FragmentPackets,
        Mtu,
        Decoy,
        SpoofSourceAddress,
        Interface,
        SourcePortG,
        SourcePort,
        DataLength,
        IpOptions,
        TimeToLive,
        SpoofMacAddress,
        BadSum,
        Adler32,
        NormalOutput,
        XmlOutput,
        ScriptKiddieOutput,
        GreppableOutput,
        AllThreeOutput,
        Verbose,
        DebugLevel,
        Reason,
        Open,
        PacketTrace,
        PrintHostInterfaceList,
        LogErrors,
        AppendOutput,
        Resume,
        Stylesheet,
        WebXml,
        NoStylesheet,
        Ipv6,
        A,
        DataDir,
        SendEth,
        SendIp,
        Privileged,
        Unprivileged,
        Version,
        Help,
    }

    /// <summary>
    ///     Class which represents command-line options for nmap
    /// </summary>
    public class NmapOptions : IDictionary<NmapFlag, string>
    {
        #region NmapFlagToOption

        private readonly Dictionary<NmapFlag, string> _nmapFlagToOption = new Dictionary<NmapFlag, string>
            {
                {NmapFlag.InputFilename, "-iL"},
                {NmapFlag.RandomTargets, "-iR"},
                {NmapFlag.ExcludeHosts, "--exclude"},
                {NmapFlag.ExcludeFile, "--excludefile"},
                {NmapFlag.ListScan, "-sL"},
                {NmapFlag.PingScan, "-sP"},
                {NmapFlag.TreatHostsAsOnline, "-PN"},
                {NmapFlag.TcpSynDiscovery, "-PS"},
                {NmapFlag.AckDiscovery, "-PA"},
                {NmapFlag.UdpDiscovery, "-PU"},
                {NmapFlag.SctpDiscovery, "-PY"},
                {NmapFlag.IcmpEchoDiscovery, "-PE"},
                {NmapFlag.IcmpTimestampDiscovery, "-PP"},
                {NmapFlag.IcmpNetmaskDiscovery, "-PM"},
                {NmapFlag.ProtocolPing, "-PO"},
                {NmapFlag.NeverDnsResolve, "-n"},
                {NmapFlag.DnsServers, "--dns-servers"},
                {NmapFlag.SystemDns, "--system-dns"},
                {NmapFlag.Traceroute, "--traceroute"},
                {NmapFlag.HostScan, "-sn"},
                {NmapFlag.TcpSynScan, "-sS"},
                {NmapFlag.ConnectScan, "-sT"},
                {NmapFlag.AckScan, "-sA"},
                {NmapFlag.WindowScan, "-sW"},
                {NmapFlag.MaimonScan, "-sM"},
                {NmapFlag.UdpScan, "-sU"},
                {NmapFlag.TcpNullScan, "-sN"},
                {NmapFlag.FinScan, "-sF"},
                {NmapFlag.XmasScan, "-sX"},
                {NmapFlag.ScanFlags, "--scanflags"},
                {NmapFlag.IdleScan, "-sI"},
                {NmapFlag.SctpInitScan, "-sY"},
                {NmapFlag.CookieEchoScan, "-sZ"},
                {NmapFlag.IpProtocolScan, "-sO"},
                {NmapFlag.FtpBounceScan, "-b"},
                {NmapFlag.PortSpecification, "-p"},
                {NmapFlag.FastScanMode, "-F"},
                {NmapFlag.ScanPortsConsecutively, "-r"},
                {NmapFlag.TopPorts, "--top-ports"},
                {NmapFlag.PortRatio, "--port-ratio"},
                {NmapFlag.ServiceVersion, "-sV"},
                {NmapFlag.VersionIntensity, "--version-intensity"},
                {NmapFlag.VersionLight, "--version-light"},
                {NmapFlag.VersionAll, "--version-all"},
                {NmapFlag.VersionTrace, "--version-trace"},
                {NmapFlag.DefaultScriptScan, "-sC"},
                {NmapFlag.Script, "--script"},
                {NmapFlag.ScriptArgs, "--script-args"},
                {NmapFlag.ScriptTrace, "--script-trace"},
                {NmapFlag.ScriptUpdateDb, "--script-updatedb"},
                {NmapFlag.OsDetection, "-O"},
                {NmapFlag.OsScanLimit, "--osscan-limit"},
                {NmapFlag.OsScanGuess, "--osscan-guess"},
                {NmapFlag.ParanoidTiming, "-T0"},
                {NmapFlag.SneakyTiming, "-T1"},
                {NmapFlag.PoliteTiming, "-T2"},
                {NmapFlag.NormalTiming, "-T3"},
                {NmapFlag.AggressiveTiming, "-T4"},
                {NmapFlag.InsaneTiming, "-T5"},
                {NmapFlag.ParallelMinHostGroupSize, "--min-hostgroup"},
                {NmapFlag.ParallelMaxHostGroupSize, "--max-hostgroup"},
                {NmapFlag.MinProbeParallelization, "--min-parallelism"},
                {NmapFlag.MaxProbeParallelization, "--max-parallelism"},
                {NmapFlag.MinRttTimeout, "--min-rtt-timeout"},
                {NmapFlag.MaxRttTimeout, "--max-rtt-timeout"},
                {NmapFlag.InitialRttTimeout, "--initial-rtt-timeout"},
                {NmapFlag.MaxRetries, "--max-retries"},
                {NmapFlag.HostTimeout, "--host-timeout"},
                {NmapFlag.ScanDelay, "--scan-delay"},
                {NmapFlag.MaxScanDelay, "--max-scan-delay"},
                {NmapFlag.MinRate, "--min-rate"},
                {NmapFlag.MaxRate, "--max-rate"},
                {NmapFlag.FragmentPackets, "-f"},
                {NmapFlag.Mtu, "--mtu"},
                {NmapFlag.Decoy, "-D"},
                {NmapFlag.SpoofSourceAddress, "-S"},
                {NmapFlag.Interface, "-e"},
                {NmapFlag.SourcePortG, "-g"},
                {NmapFlag.SourcePort, "--source-port"},
                {NmapFlag.DataLength, "--data-length"},
                {NmapFlag.IpOptions, "--ip-options"},
                {NmapFlag.TimeToLive, "--ttl"},
                {NmapFlag.SpoofMacAddress, "--spoof-mac"},
                {NmapFlag.BadSum, "--badsum"},
                {NmapFlag.Adler32, "--adler32"},
                {NmapFlag.NormalOutput, "-oN"},
                {NmapFlag.XmlOutput, "-oX"},
                {NmapFlag.ScriptKiddieOutput, "-oS"},
                {NmapFlag.GreppableOutput, "-oG"},
                {NmapFlag.AllThreeOutput, "-oA"},
                {NmapFlag.Verbose, "-v"},
                {NmapFlag.DebugLevel, "-d"},
                {NmapFlag.Reason, "--reason"},
                {NmapFlag.Open, "--open"},
                {NmapFlag.PacketTrace, "--packet-trace"},
                {NmapFlag.PrintHostInterfaceList, "--iflist"},
                {NmapFlag.LogErrors, "--log-errors"},
                {NmapFlag.AppendOutput, "--append-output"},
                {NmapFlag.Resume, "--resume"},
                {NmapFlag.Stylesheet, "--stylesheet"},
                {NmapFlag.WebXml, "--webxml"},
                {NmapFlag.NoStylesheet, "--no-stylesheet"},
                {NmapFlag.Ipv6, "-6"},
                {NmapFlag.A, "-A"},
                {NmapFlag.DataDir, "--datadir"},
                {NmapFlag.SendEth, "--send-eth"},
                {NmapFlag.SendIp, "--send-ip"},
                {NmapFlag.Privileged, "--privileged"},
                {NmapFlag.Unprivileged, "--unprivileged"},
                {NmapFlag.Version, "-V"},
                {NmapFlag.Help, "-h"}
            };

        #endregion

        #region NmapOptionToFlag

        private readonly Dictionary<string, NmapFlag> _nmapOptionToFlag = new Dictionary<string, NmapFlag>
            {
                {"-iL", NmapFlag.InputFilename},
                {"-iR", NmapFlag.RandomTargets},
                {"--exclude", NmapFlag.ExcludeHosts},
                {"--excludefile", NmapFlag.ExcludeFile},
                {"-sL", NmapFlag.ListScan},
                {"-sP", NmapFlag.PingScan},
                {"-PN", NmapFlag.TreatHostsAsOnline},
                {"-PS", NmapFlag.TcpSynDiscovery},
                {"-PA", NmapFlag.AckDiscovery},
                {"-PU", NmapFlag.UdpDiscovery},
                {"-PY", NmapFlag.SctpDiscovery},
                {"-PE", NmapFlag.IcmpEchoDiscovery},
                {"-PP", NmapFlag.IcmpTimestampDiscovery},
                {"-PM", NmapFlag.IcmpNetmaskDiscovery},
                {"-PO", NmapFlag.ProtocolPing},
                {"-n", NmapFlag.NeverDnsResolve},
                {"--dns-servers", NmapFlag.DnsServers},
                {"--system-dns", NmapFlag.SystemDns},
                {"--traceroute", NmapFlag.Traceroute},
                {"-sn", NmapFlag.HostScan},
                {"-sS", NmapFlag.TcpSynScan},
                {"-sT", NmapFlag.ConnectScan},
                {"-sA", NmapFlag.AckScan},
                {"-sW", NmapFlag.WindowScan},
                {"-sM", NmapFlag.MaimonScan},
                {"-sU", NmapFlag.UdpScan},
                {"-sN", NmapFlag.TcpNullScan},
                {"-sF", NmapFlag.FinScan},
                {"-sX", NmapFlag.XmasScan},
                {"--scanflags", NmapFlag.ScanFlags},
                {"-sI", NmapFlag.IdleScan},
                {"-sY", NmapFlag.SctpInitScan},
                {"-sZ", NmapFlag.CookieEchoScan},
                {"-sO", NmapFlag.IpProtocolScan},
                {"-b", NmapFlag.FtpBounceScan},
                {"-p", NmapFlag.PortSpecification},
                {"-F", NmapFlag.FastScanMode},
                {"-r", NmapFlag.ScanPortsConsecutively},
                {"--top-ports", NmapFlag.TopPorts},
                {"--port-ratio", NmapFlag.PortRatio},
                {"-sV", NmapFlag.ServiceVersion},
                {"--version-intensity", NmapFlag.VersionIntensity},
                {"--version-light", NmapFlag.VersionLight},
                {"--version-all", NmapFlag.VersionAll},
                {"--version-trace", NmapFlag.VersionTrace},
                {"-sC", NmapFlag.DefaultScriptScan},
                {"--script", NmapFlag.Script},
                {"--script-args", NmapFlag.ScriptArgs},
                {"--script-trace", NmapFlag.ScriptTrace},
                {"--script-updatedb", NmapFlag.ScriptUpdateDb},
                {"-O", NmapFlag.OsDetection},
                {"--osscan-limit", NmapFlag.OsScanLimit},
                {"--osscan-guess", NmapFlag.OsScanGuess},
                {"-T0", NmapFlag.ParanoidTiming},
                {"-T1", NmapFlag.SneakyTiming},
                {"-T2", NmapFlag.PoliteTiming},
                {"-T3", NmapFlag.NormalTiming},
                {"-T4", NmapFlag.AggressiveTiming},
                {"-T5", NmapFlag.InsaneTiming},
                {"--min-hostgroup", NmapFlag.ParallelMinHostGroupSize},
                {"--max-hostgroup", NmapFlag.ParallelMaxHostGroupSize},
                {"--min-parallelism", NmapFlag.MinProbeParallelization},
                {"--max-parallelism", NmapFlag.MaxProbeParallelization},
                {"--min-rtt-timeout", NmapFlag.MinRttTimeout},
                {"--max-rtt-timeout", NmapFlag.MaxRttTimeout},
                {"--initial-rtt-timeout", NmapFlag.InitialRttTimeout},
                {"--max-retries", NmapFlag.MaxRetries},
                {"--host-timeout", NmapFlag.HostTimeout},
                {"--scan-delay", NmapFlag.ScanDelay},
                {"--max-scan-delay", NmapFlag.MaxScanDelay},
                {"--min-rate", NmapFlag.MinRate},
                {"--max-rate", NmapFlag.MaxRate},
                {"-f", NmapFlag.FragmentPackets},
                {"--mtu", NmapFlag.Mtu},
                {"-D", NmapFlag.Decoy},
                {"-S", NmapFlag.SpoofSourceAddress},
                {"-e", NmapFlag.Interface},
                {"-g", NmapFlag.SourcePortG},
                {"--source-port", NmapFlag.SourcePort},
                {"--data-length", NmapFlag.DataLength},
                {"--ip-options", NmapFlag.IpOptions},
                {"--ttl", NmapFlag.TimeToLive},
                {"--spoof-mac", NmapFlag.SpoofMacAddress},
                {"--badsum", NmapFlag.BadSum},
                {"--adler32", NmapFlag.Adler32},
                {"-oN", NmapFlag.NormalOutput},
                {"-oX", NmapFlag.XmlOutput},
                {"-oS", NmapFlag.ScriptKiddieOutput},
                {"-oG", NmapFlag.GreppableOutput},
                {"-oA", NmapFlag.AllThreeOutput},
                {"-v", NmapFlag.Verbose},
                {"-d", NmapFlag.DebugLevel},
                {"--reason", NmapFlag.Reason},
                {"--open", NmapFlag.Open},
                {"--packet-trace", NmapFlag.PacketTrace},
                {"--iflist", NmapFlag.PrintHostInterfaceList},
                {"--log-errors", NmapFlag.LogErrors},
                {"--append-output", NmapFlag.AppendOutput},
                {"--resume", NmapFlag.Resume},
                {"--stylesheet", NmapFlag.Stylesheet},
                {"--webxml", NmapFlag.WebXml},
                {"--no-stylesheet", NmapFlag.NoStylesheet},
                {"-6", NmapFlag.Ipv6},
                {"-A", NmapFlag.A},
                {"--datadir", NmapFlag.DataDir},
                {"--send-eth", NmapFlag.SendEth},
                {"--send-ip", NmapFlag.SendIp},
                {"--privileged", NmapFlag.Privileged},
                {"--unprivileged", NmapFlag.Unprivileged},
                {"-V", NmapFlag.Version},
                {"-h", NmapFlag.Help}
            };

        #endregion

        private readonly Dictionary<string, string> _nmapOptions;

        public NmapOptions()
        {
            _nmapOptions = new Dictionary<string, string>();
        }

        public void Add(KeyValuePair<NmapFlag, string> kvp)
        {
            Add(kvp.Key, kvp.Value);
        }

        public void Clear()
        {
            _nmapOptions.Clear();
        }

        public bool Contains(KeyValuePair<NmapFlag, string> item)
        {
            return _nmapOptions.Contains(new KeyValuePair<string, string>(_nmapFlagToOption[item.Key], item.Value));
        }

        public void CopyTo(KeyValuePair<NmapFlag, string>[] array, int arrayIndex)
        {
            _nmapOptions.Select(x => new KeyValuePair<NmapFlag, string>(_nmapOptionToFlag[x.Key], x.Value))
                        .ToArray()
                        .CopyTo(array, arrayIndex);
        }

        public bool Remove(KeyValuePair<NmapFlag, string> item)
        {
            return _nmapOptions.Remove(_nmapFlagToOption[item.Key]);
        }

        public int Count
        {
            get { return _nmapOptions.Count; }
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public bool ContainsKey(NmapFlag key)
        {
            return _nmapOptions.ContainsKey(_nmapFlagToOption[key]);
        }

        public void Add(NmapFlag flag, string argument)
        {
            string option = _nmapFlagToOption[flag];

            if (_nmapOptions.ContainsKey(option))
            {
                _nmapOptions[option] = string.Format("{0},{1}", _nmapOptions[option], argument);
            }
            else
            {
                _nmapOptions.Add(option, argument);
            }
        }

        public bool Remove(NmapFlag key)
        {
            return _nmapOptions.Remove(_nmapFlagToOption[key]);
        }

        public bool TryGetValue(NmapFlag key, out string value)
        {
            return _nmapOptions.TryGetValue(_nmapFlagToOption[key], out value);
        }

        public string this[NmapFlag key]
        {
            get { return _nmapOptions[_nmapFlagToOption[key]]; }
            set { _nmapOptions[_nmapFlagToOption[key]] = value; }
        }

        public ICollection<NmapFlag> Keys
        {
            get { return _nmapOptions.Select(x => _nmapOptionToFlag[x.Key]).ToArray(); }
        }

        public ICollection<string> Values
        {
            get { return _nmapOptions.Values; }
        }


        public IEnumerator<KeyValuePair<NmapFlag, string>> GetEnumerator()
        {
            return
                _nmapOptions.Select(x => new KeyValuePair<NmapFlag, string>(_nmapOptionToFlag[x.Key], x.Value))
                            .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(NmapFlag flag)
        {
            Add(flag, string.Empty);
        }

        public void AddAll(IEnumerable<NmapFlag> flags)
        {
            foreach (NmapFlag flag in flags)
            {
                Add(flag);
            }
        }

        public void AddAll(IEnumerable<KeyValuePair<NmapFlag, string>> kvps)
        {
            foreach (var kvp in kvps)
            {
                Add(kvp.Key, kvp.Value);
            }
        }

        public override string ToString()
        {
            return
                _nmapOptions.Aggregate(new StringBuilder(), (sb, kvp) => sb.AppendFormat("{0} {1} ", kvp.Key, kvp.Value),
                                       sb => sb.ToString()).Trim();
        }
    }

    public class NmapException : ApplicationException
    {
        public NmapException(string ex) : base(ex)
        {
        }
    }

    /// <summary>
    ///     A class that represents an nmap run
    /// </summary>
    public class NmapContext
    {
        /// <summary>
        ///     By default we try to find the path to the nmap executable by searching the path, the output XML file is a temporary file, and the nmap options are empty.
        ///     
        ///     Nmap ProcessWindowStyle is Hidden by default.
        /// </summary>
        public NmapContext(ProcessWindowStyle windowStyle = ProcessWindowStyle.Hidden)
        {
            Path = GetPathToNmap();
            OutputPath = System.IO.Path.GetTempFileName();
            Options = new NmapOptions();
            WindowStyle = windowStyle;
        }

        /// <summary>
        ///     The path to the nmap executable
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        ///     The output path for the nmap XML file
        /// </summary>
        public string OutputPath { get; set; }

        /// <summary>
        ///     The specified nmap options
        /// </summary>
        public NmapOptions Options { get; set; }

        /// <summary>
        ///     The intended target
        /// </summary>
        public string Target { get; set; }

        public ProcessWindowStyle WindowStyle { get; set; }

        /// <summary>
        ///     This searches our PATH environment variable for a particular file
        /// </summary>
        /// <param name="filename">The file to search for</param>
        /// <returns>The path to the file if it is found, the empty string otherwise</returns>
        private static string LocateExecutable(string filename)
        {
            string path = Environment.GetEnvironmentVariable("path");
            string[] folders = path.Split(';');

            foreach (string folder in folders)
            {
                string combined = System.IO.Path.Combine(folder, filename);
                if (File.Exists(combined))
                {
                    return combined;
                }
            }

            return string.Empty;
        }

        /// <summary>
        ///     This searches our PATH for the nmap executable
        /// </summary>
        /// <returns>The path to the nmap executable or the empty string if it cannot be located</returns>
        public string GetPathToNmap()
        {
            return LocateExecutable("nmap.exe");
        }

        /// <summary>
        ///     This searches our PATH for the ndiff executable
        /// </summary>
        /// <returns>The path to the ndiff executable or the empty string if it cannot be located</returns>
        public string GetPathToNdiff()
        {
            return LocateExecutable("ndiff.exe");
        }

        /// <summary>
        ///     Execute an nmap run with the specified options on the intended target, writing the resultant XML to the specified file
        /// </summary>
        /// <returns>An nmaprun object representing the result of an nmap run</returns>
        public virtual nmaprun Run()
        {
            if (string.IsNullOrEmpty(OutputPath))
            {
                throw new ApplicationException("Nmap output file path is null or empty");
            }

            if (string.IsNullOrEmpty(Path) || !File.Exists(Path))
            {
                throw new ApplicationException("Path to nmap is invalid");
            }

            if (string.IsNullOrEmpty(Target))
            {
                throw new ApplicationException("Attempted run on empty target");
            }

            if (Options == null)
            {
                throw new ApplicationException("Nmap options null");
            }

            Options[NmapFlag.XmlOutput] = OutputPath;

            using (var process = new Process())
            {
                process.StartInfo.FileName = Path;
                process.StartInfo.Arguments = string.Format("{0} {1}", Options, Target);
                process.StartInfo.WindowStyle = WindowStyle;
                process.Start();
                process.WaitForExit();

                if (!File.Exists(OutputPath))
                {
                    throw new NmapException(process.StartInfo.Arguments);
                }
            }

            return Serialization.DeserializeFromFile<nmaprun>(OutputPath);
        }
    }
}