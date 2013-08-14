using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using SaltwaterTaffy.Utility;
using Simple.DotNMap;

namespace SaltwaterTaffy
{
    public struct Port
    {
        public int PortNumber { get; set; }
        public ProtocolType Protocol { get; set; }
        public bool Filtered { get; set; }
    }

    public struct Host
    {
        public string Address { get; set; }
        public ISet<string> Hostnames { get; set; }
        public ISet<Port> Ports { get; set; }
    }

    public class ScanResult
    {
        public ISet<Host> Hosts { get; set; }

        private readonly Dictionary<Type, Func<object, IEither<hostnames, ports>>> _nmapTypeToEither = new Dictionary<Type, Func<object, IEither<hostnames, ports>>>
            {
                {typeof(hostnames), (o) => Either.Create<hostnames,ports>((hostnames)o)},
                {typeof(ports), (o) => Either.Create<hostnames,ports>((ports)o)}
            };
 
        public ScanResult(nmaprun result)
        {
            Hosts = new HashSet<Host>();
            foreach (host host in result.Items)
            {
                var newHost = new Host
                    {
                        Address = host.address.addr,
                        Ports = new HashSet<Port>(),
                        Hostnames = new HashSet<string>()
                    };
                foreach (var i in host.Items)
                {
                    var either = _nmapTypeToEither[i.GetType()](i);
                    either.Case(
                        (x) => x.hostname.ToList().ForEach(
                            (y) => newHost.Hostnames.Add(y.name)),
                        (x) => x.port.ToList().ForEach(
                            (y) => newHost.Ports.Add(
                                new Port
                                {
                                    PortNumber = int.Parse(y.portid),
                                    Protocol = (ProtocolType)Enum.Parse(typeof(ProtocolType), y.protocol.ToString().Capitalize()),
                                    Filtered = y.state.state1 == "filtered"
                                }
                            )
                        )
                    );
                }

                Hosts.Add(newHost);
            }
        }
    }

    public class Scanner
    {
    }
}
