using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using SaltwaterTaffy.Container;
using SaltwaterTaffy.Utility;
using Simple.DotNMap;

namespace SaltwaterTaffy
{
    delegate void ScanAction<T1, T2>(T1 a, out T2 b);

    public class ScanResult
    {
        public ISet<Host> Hosts { get; set; }

        private readonly Dictionary<Type, Action<object, Host>> _dispatch = new Dictionary<Type, Action<object, Host>>
            {
                {typeof(hostnames), (o, h) =>
                    {
                        var names = (hostnames)o;
                        if (names.hostname != null)
                            names.hostname.ToList().ForEach(
                                (x) => h.Hostnames.Add(x.name));
                    }},
                {typeof(ports), (o, h) =>
                    {
                        var portsSection = (ports) o;
                        if (portsSection.port != null)
                        {
                            portsSection.port.ToList().ForEach(
                                (x) =>
                                    {
                                        var port = new Port
                                            {
                                                PortNumber = int.Parse(x.portid),
                                                Protocol =
                                                    (ProtocolType)
                                                    Enum.Parse(typeof (ProtocolType),
                                                               x.protocol.ToString().Capitalize()),
                                                Filtered = x.state.state1 == "filtered"
                                            };
                                        if (x.service != null)
                                        {
                                            port.Service = new Service
                                                {
                                                    Name = x.service.name,
                                                    Product = x.service.product,
                                                    Os = x.service.ostype,
                                                    Version = x.service.version
                                                };
                                        }
                                        h.Ports.Add(port);
                                    });
                        }

                        if (portsSection.extraports != null)
                        {
                            var extra = portsSection.extraports.First();
                            h.ExtraPorts.Add(
                                new ExtraPorts
                                    {
                                        Count = int.Parse(extra.count),
                                        State = extra.state
                                    });
                        }
                    }},
               {typeof(os), (o, h) =>
                   ((os)o).osmatch.ToList().ForEach(
                       (x) => h.OsMatches.Add(
                           new Os
                               {
                                   Certainty = int.Parse(x.accuracy),
                                   Name = x.name,
                                   Family = x.osclass[0].osfamily,
                                   Generation = x.osclass[0].osgen
                               }))}
            };
 
        public ScanResult(nmaprun result)
        {
            Hosts = new HashSet<Host>();
            foreach (host host in result.Items)
            {
                var newHost = new Host
                    {
                        Address = IPAddress.Parse(host.address.addr),
                        Ports = new List<Port>(),
                        OsMatches = new List<Os>(),
                        ExtraPorts = new List<ExtraPorts>(),
                        Hostnames = new HashSet<string>()
                    };
                foreach (var i in host.Items)
                {
                    var type = i.GetType();
                    if (_dispatch.ContainsKey(type))
                    {
                        _dispatch[i.GetType()](i, newHost);
                    }
                }

                Hosts.Add(newHost);
            }
        }
    }

    public class Scanner
    {
        public Target Target { get; set; }
        public NmapOptions PersistentOptions { get; set; } // e.g., --exclude foobar

        public Scanner(Target target)
        {
            Target = target;
        }

        private NmapContext GetContext()
        {
            var ctx = new NmapContext
            {
                Target = Target.ToString()
            };

            AddPersistentOptions(ctx.Options);

            return ctx;
        }

        private void AddPersistentOptions(NmapOptions opt)
        {
            if (PersistentOptions != null)
            {
                foreach (var kvp in PersistentOptions)
                {
                    opt.Add(kvp.Key, kvp.Value);
                }
            }
        }

        public ISet<Host> HostDiscovery()
        {
            var ctx = GetContext();
            ctx.Options.Add(NmapFlag.PingScan);
            ctx.Options.Add(NmapFlag.OsDetection);
             
            return new ScanResult(ctx.Run()).Hosts;
        }

        public bool FirewallProtected()
        {
            var ctx = GetContext();
            ctx.Options.Add(NmapFlag.AckScan);

            var sr = new ScanResult(ctx.Run());

            return
                sr.Hosts.Any(
                    x => (x.ExtraPorts.First().Count > 0 && x.ExtraPorts.First().State == "filtered") || x.Ports.Any(y => y.Filtered));
        }

        public ScanResult PortScan()
        {
            var ctx = GetContext();
            ctx.Options.Add(NmapFlag.ServiceVersion);
            ctx.Options.Add(NmapFlag.OsDetection);

            return new ScanResult(ctx.Run());
        } 
    }
}
