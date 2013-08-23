using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SaltwaterTaffy.Container;

namespace SaltwaterTaffy.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter an IP or subnet: ");
            var target = new Target(Console.ReadLine().Trim());
            Console.WriteLine("Initializing scan of {0}", target);
            var result = new Scanner(target).PortScan();
            Console.WriteLine("Detected {0} host(s), {1} up and {2} down.", result.Total, result.Up, result.Down);
            foreach (var i in result.Hosts)
            {
                Console.WriteLine("Host: {0}", i.Address);
                foreach (var j in i.Ports)
                {
                    Console.Write("\tport {0}", j.PortNumber);
                    if (!string.IsNullOrEmpty(j.Service.Name))
                    {
                        Console.Write(" is running {0}", j.Service.Name);
                    }

                    if (j.Filtered)
                    {
                        Console.Write(" is filtered");
                    }

                    Console.WriteLine();
                }

                if (i.OsMatches.Any())
                {
                    Console.WriteLine("and is probably running {0}", i.OsMatches.First().Name);   
                }
            }

            Console.Read();
        }
    }
}
