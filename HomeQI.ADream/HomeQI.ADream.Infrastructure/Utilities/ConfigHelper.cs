using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace System
{

    /// <summary>
    /// 读取xml和json的帮助
    /// </summary>
    public static class ConfigHelper
    {
        /// <summary>
        /// xml配置文件读取
        /// </summary>
        /// <param name="configFileName"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static IConfigurationRoot GetXmlConfig(
           string configFileName = "appsettings.xml",
           string basePath = "")
        {
            basePath = string.IsNullOrWhiteSpace(basePath) ? Directory.GetCurrentDirectory() : basePath;
            var builder = new ConfigurationBuilder().
             SetBasePath(basePath).
             AddXmlFile(b =>
             {
                 b.Path = configFileName;
                 b.FileProvider = new PhysicalFileProvider(basePath);
             });
            return builder.Build();
        }
        public static string GetConnectionString(string connection = "")
        {
            return GetJsonConfig().GetConnectionString(connection ?? "DefaultConnection");
        }
        /// <summary>
        /// json配置文件读取
        /// </summary>
        /// <param name="configFileName"></param>
        /// <param name="basePath"></param>
        /// <returns></returns>
        public static IConfigurationRoot GetJsonConfig(
           string configFileName = "appsettings.json",
           string basePath = "")
        {
            basePath = string.IsNullOrWhiteSpace(basePath) ? Directory.GetCurrentDirectory() : basePath;
            var builder = new ConfigurationBuilder().
              SetBasePath(basePath).
              AddJsonFile(configFileName);
            return builder.Build();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationRoot"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string GetValue(this IConfigurationRoot configurationRoot, string input)
        {
            return configurationRoot.GetSection(input).Value;
        }
    }
}
