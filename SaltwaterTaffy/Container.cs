// This file is part of SaltwaterTaffy, an nmap wrapper library for .NET
// Copyright (C) 2013 Thom Dixon <thom@thomdixon.org>
// Released under the GNU GPLv2 or any later version
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

namespace SaltwaterTaffy.Container
{
    /// <summary>
    ///     Struct representing a service (e.g., ssh, httpd) running on a host
    /// </summary>
    public struct Service
    {
        public string Name { get; set; }
        public string Product { get; set; }
        public string Os { get; set; }
        public string Version { get; set; }
    }

    /// <summary>
    ///     Struct representing a port on a host
    /// </summary>
    public struct Port
    {
        public int PortNumber { get; set; }
        public ProtocolType Protocol { get; set; }
        public bool Filtered { get; set; }
        public string State { get; set; }
        public Service Service { get; set; }
    }

    /// <summary>
    ///     Struct which represents the "extraports" information produced by nmap
    /// </summary>
    public struct ExtraPorts
    {
        public int Count { get; set; }
        public string State { get; set; }
    }

    /// <summary>
    ///     Struct which represents an operating system
    /// </summary>
    public struct Os
    {
        public int Certainty { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Generation { get; set; }
    }

    /// <summary>
    ///     Struct which represents a scanned host
    /// </summary>
    public struct Host
    {
        public IPAddress Address { get; set; }
        public IEnumerable<string> Hostnames { get; set; }
        public IEnumerable<Port> Ports { get; set; }
        public IEnumerable<ExtraPorts> ExtraPorts { get; set; }
        public IEnumerable<Os> OsMatches { get; set; }
    }

    /// <summary>
    ///     Object representing the target(s) of an nmap scan
    /// </summary>
    public struct Target
    {
        private readonly string _target;

        public Target(string target)
        {
            _target = target;
        }

        public Target(IPAddress target)
        {
            _target = target.ToString();
        }

        public Target(IEnumerable<IPAddress> target)
        {
            _target = string.Join(" ", target);
        }

        public Target(IEnumerable<string> targets)
        {
            _target = string.Join(" ", targets);
        }

        public override string ToString()
        {
            return _target;
        }
    }
}