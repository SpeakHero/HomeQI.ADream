using HomeQI.Adream.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Net.NetworkInformation;

namespace ConsoleTest
{
    class Program
    {

        // args[0] can be an IPaddress or host name.
        public static void Main(string[] args)
        {
            IdentityOptions identityOptions = new IdentityOptions();
            var json = identityOptions.ToJson();
            var baseDirectory = AppContext.BaseDirectory;
            var jsonfile = $"{baseDirectory}\\identity.json";
            File.AppendAllTextAsync(jsonfile, json);
            IServiceCollection serviceDescriptors = new ServiceCollection();
            IConfiguration configuration = ConfigHelper.GetJsonConfig("identity.json", baseDirectory);
            identityOptions = configuration.Get<IdentityOptions>();
            Console.ReadKey();
        }
        public static void DisplayDnsConfiguration()
        {
            NetworkInterface[] adapters = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in adapters)
            {
                IPInterfaceProperties properties = adapter.GetIPProperties();
                Console.WriteLine(adapter.Description);
                Console.WriteLine("  DNS suffix .............................. : {0}",
                    properties.DnsSuffix);
                Console.WriteLine("  DNS enabled ............................. : {0}",
                    properties.IsDnsEnabled);
                Console.WriteLine("  Dynamically configured DNS .............. : {0}",
                    properties.IsDynamicDnsEnabled);
            }
            Console.WriteLine();
        }
        public static void DisplayIPv4NetworkInterfaces()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            IPGlobalProperties properties = IPGlobalProperties.GetIPGlobalProperties();
            Console.WriteLine("IPv4 interface information for {0}.{1}",
               properties.HostName, properties.DomainName);
            Console.WriteLine();

            foreach (NetworkInterface adapter in nics)
            {
                // Only display informatin for interfaces that support IPv4.
                if (adapter.Supports(NetworkInterfaceComponent.IPv4) == false)
                {
                    continue;
                }
                Console.WriteLine(adapter.Description);
                // Underline the description.
                Console.WriteLine(String.Empty.PadLeft(adapter.Description.Length, '='));
                IPInterfaceProperties adapterProperties = adapter.GetIPProperties();
                // Try to get the IPv4 interface properties.
                IPv4InterfaceProperties p = adapterProperties.GetIPv4Properties();

                if (p == null)
                {
                    Console.WriteLine("No IPv4 information is available for this interface.");
                    Console.WriteLine();
                    continue;
                }
                // Display the IPv4 specific data.
                Console.WriteLine("  Index ............................. : {0}", p.Index);
                Console.WriteLine("  MTU ............................... : {0}", p.Mtu);
                Console.WriteLine("  APIPA active....................... : {0}",
                    p.IsAutomaticPrivateAddressingActive);
                Console.WriteLine("  APIPA enabled...................... : {0}",
                    p.IsAutomaticPrivateAddressingEnabled);
                Console.WriteLine("  Forwarding enabled................. : {0}",
                    p.IsForwardingEnabled);
                Console.WriteLine("  Uses WINS ......................... : {0}",
                    p.UsesWins);
                Console.WriteLine();
            }
        }
    }
}
