# SaltwaterTaffy

SaltwaterTaffy is an [nmap](http://www.nmap.org) wrapper library for .NET, which
aims to provide a simple interface for host discovery, firewall detection, and
port scanning. This library was partially inspired by
[nmap4j](https://github.com/narkisr/nmap4j).

## Dependencies

SaltwaterTaffy utilizes [DotNMap](http://dotnmap.codeplex.com) to parse nmap's XML output format. A DLL is provided in `lib`.

## Usage & Examples

Using the library is (hopefully) fairly simple. A demo project is included in `SaltwaterTaffy.Demo`, and some examples are provided below.

### Host discovery

Host discovery will yield a collection of `Host` objects, each containing information about the discovered host.

```C#
    using SaltwaterTaffy;
    using SaltwaterTaffy.Container;

    class Program
    {
        public static void Main(string[] args)
        {
            var target = new Target("192.168.0.0/24");
            var result = new Scanner(target).HostDiscovery();
            // do something with the result
        }
    }
```

### Port scan (TCP SYN scan)

```C#
    using SaltwaterTaffy;
    using SaltwaterTaffy.Container;

    class Program
    {
        public static void Main(string[] args)
        {
            var target = new Target("192.168.1.101");
            var result = new Scanner(target).PortScan(ScanType.Syn);
            // do something with the result
        }
    }
```

### A more advanced scan

```C#
    using SaltwaterTaffy;
    using SaltwaterTaffy.Container;

    class Program
    {
        public static void Main(string[] args)
        {
            // target can be a string, an IPAddress or an IEnumerable of either
            var target = new Target("192.168.1.0/24");
            var scanner = new Scanner(target);

            // multiple calls to scanner will always exclude this host
            scanner.PersistentOptions = new NmapOptions {
                {NmapFlag.ExcludeHosts, "192.168.0.12"}
            };
            var result = scanner.HostDiscovery();
            // do something with the result
        }
    }
```