using HomeQi.Adream.Identity;
using HomeQI.Adream.Identity.EntityFrameworkCore;
using HomeQI.ADream.IdentityService.Models;
using HomeQI.ADream.IdentityService.Repositories;
using HomeQI.ADream.IdentityService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.PlatformAbstractions;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace HomeQI.ADream.IdentityService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            // IoC - DbContext
            services.AddDbContext<IdentityDbContext>().AddCacheManager().AddHttpClient().AddResponseCaching().AddIdentity().AddEntityFrameworkStores();
            // IoC - Service & Repository
            // IdentityServer4
            string basePath = PlatformServices.Default.Application.ApplicationBasePath;
            InMemoryConfiguration.Configuration = Configuration;
            services.AddIdentityServer()
                .AddSigningCredential(new X509Certificate2(Path.Combine(basePath,
                    Configuration["Certificates:CerPath"]),
                    Configuration["Certificates:Password"]))
                //.AddTestUsers(InMemoryConfiguration.GetTestUsers().ToList())
                .AddInMemoryIdentityResources(InMemoryConfiguration.GetIdentityResources())
                .AddInMemoryApiResources(InMemoryConfiguration.GetApiResources())
                .AddInMemoryClients(InMemoryConfiguration.GetClients())
                .AddResourceOwnerValidator<ResourceOwnerPasswordValidator>()
                .AddProfileService<ProfileService>();
            // for QuickStart-UI
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseIdentityServer();
            // for QuickStart-UI
            app.UseStaticFiles();
            app.UseMvcWithDefaultRoute();
        }
    }
}
