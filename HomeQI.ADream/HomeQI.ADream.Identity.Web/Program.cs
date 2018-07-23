using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Net;

namespace HomeQI.ADream.Identity.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Program
    {
        /// <summary>
        /// 
        /// </summary>
        public static string IP;
        /// <summary>
        /// 
        /// </summary>
        public static int Port;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        public static IWebHost BuildWebHost(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddCommandLine(args)
                .Build();

            IP = config["ip"];
            Port = Convert.ToInt32(config["port"]);

            if (string.IsNullOrEmpty(IP))
            {
                IP = NetworkHelper.LocalIPAddress;
            }

            if (Port == 0)
            {
                Port = NetworkHelper.GetRandomAvaliablePort();
            }

            return WebHost.CreateDefaultBuilder(args)
                            .UseStartup<Startup>()
                            .UseUrls($"http://{IP}:{Port}")
                            .ConfigureAppConfiguration((hostingContext, builder) =>
                            {
                                var basepath = hostingContext.HostingEnvironment.ContentRootPath;
                                foreach (var item in Directory.GetFiles(basepath + "\\config"))
                                {
                                    if (Path.GetExtension(item).ToUpper().Equals(".JSON"))
                                    {
                                        builder.AddJsonFile(item, true, true);
                                    }
                                }
                                builder.SetBasePath(basepath);
                            })
                            .Build();
        }
    }
}
