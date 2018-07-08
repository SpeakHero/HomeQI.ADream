using HomeQI.Adream.Identity.EntityFrameworkCore;
using HomeQI.ADream.Identity.Stores;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NLog.Extensions.Logging;
using System;

namespace HomeQI.Adream.Identity.Test
{
    public class TestBase
    {
        public IServiceProvider ServiceProvider { get; }
        public IServiceCollection ServiceDescriptors { get; }
        public TestBase()
        {
            ServiceDescriptors = new ServiceCollection();

            ServiceDescriptors.AddIdentity().AddEntityFrameworkStores();
            ServiceDescriptors.AddLocalization();
            ServiceDescriptors.AddMvc();
            ServiceDescriptors.AddLogging();
            ServiceDescriptors.AddAuthentication();
            ServiceDescriptors.AddAuthorization();
            ServiceDescriptors.AddHttpClient();
            ServiceDescriptors.AddNodeServices();
            ServiceDescriptors.AddSingleton<ILoggerFactory, LoggerFactory>();
            ServiceDescriptors.AddSingleton(typeof(ILogger<>), typeof(Logger<>));
            ServiceDescriptors.AddDistributedMemoryCache();
            ServiceDescriptors.AddMemoryCache();

            ServiceDescriptors
            .AddDbContext<IdentityDbContext>(); ServiceDescriptors.AddEntityFrameworkInMemoryDatabase();
            var loggerFactory = GetRequiredService<ILoggerFactory>();
            //configure NLog
            var loggerFactory2 = loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            loggerFactory2.AddConsole();
            loggerFactory2.AddEventSourceLogger();
            loggerFactory2.AddDebug();
            ServiceDescriptors.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        public T GetService<T>() => ServiceDescriptors.BuildServiceProvider().GetService<T>();

        public T GetRequiredService<T>() => ServiceDescriptors.BuildServiceProvider().GetRequiredService<T>();
    }
}
