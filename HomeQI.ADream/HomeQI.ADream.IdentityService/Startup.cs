using HomeQi.Adream.Identity;
using HomeQI.Adream.Identity.EntityFrameworkCore;
using HomeQI.ADream.Identity.Models;
using HomeQI.ADream.IdentityService.Models;
using IdentityModel;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Microsoft.IdentityModel.Tokens;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

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
            //注册Swagger生成器，定义一个和多个Swagger 文档
            services.AddSwaggerGen(c =>
            {


                //添加header验证信息
                //c.OperationFilter<SwaggerHeader>();
                var security = new Dictionary<string, IEnumerable<string>> { { "Bearer", new string[] { } }, };
                c.AddSecurityRequirement(security);//添加一个必须的全局安全信息，和AddSecurityDefinition方法指定的方案名称要一致，这里是Bearer。
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT授权(数据将在请求头中进行传输) 参数结构: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",//jwt默认的参数名称
                    In = "header",//jwt默认存放Authorization信息的位置(请求头中)
                    Type = "apiKey"
                });
                c.SwaggerDoc("v1", new Info { Contact = new Contact { Name = "SpeakHero", Email = "491960352@qq.com" }, Description = "身份认证接口", Title = "Identity API", Version = "v1" });
            });
            services.AddMemoryCache(); //MemoryCache缓存注入
            services.AddDistributedMemoryCache();
            services.AddIdentity().AddEntityFrameworkStores();
            services.AddDbContext<IdentityDbContext>();
            services.AddEntityFrameworkInMemoryDatabase();
            var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
            //configure NLog
            var loggerFactory2 = loggerFactory.AddNLog(new NLogProviderOptions { CaptureMessageTemplates = true, CaptureMessageProperties = true });
            loggerFactory2.AddConsole();
            loggerFactory2.AddEventSourceLogger();
            loggerFactory2.AddDebug();
            ///////////////////jwt//////////////////////
            services.Configure<Audience>(Configuration.GetSection("Audience"));
            var audienceConfig = Configuration.GetSection("Audience");
            var symmetricKeyAsBase64 = audienceConfig["Secret"];
            var keyByteArray = Encoding.UTF8.GetBytes(symmetricKeyAsBase64);
            var signingKey = new SymmetricSecurityKey(keyByteArray);

            var tokenValidationParameters = new TokenValidationParameters
            {

                // The signing key must match!
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = signingKey,

                // Validate the JWT Issuer (iss) claim
                ValidateIssuer = true,
                ValidIssuer = audienceConfig["Issuer"],

                // Validate the JWT Audience (aud) claim
                ValidateAudience = true,
                ValidAudience = audienceConfig["Audience"],
                NameClaimType = JwtClaimTypes.Name,
                RoleClaimType = JwtClaimTypes.Role,
                // Validate the token expiry
                ValidateLifetime = true,

                ClockSkew = TimeSpan.FromMinutes(30) //有效期30分钟
            };
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                //不使用https
                //o.RequireHttpsMetadata = false;
                o.TokenValidationParameters = tokenValidationParameters;
                o.Events = new JwtBearerEvents()
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Query["access_token"];
                        return Task.CompletedTask;
                    }
                };
                o.Authority = audienceConfig["Issuer"]; ///OIDC服务的地址
                o.Audience = audienceConfig["Audience"];
            })
            .AddCookie(); ;
            services.AddSingleton<IDistributedCache>(
     serviceProvider =>
         new RedisCache(new RedisCacheOptions
         {
             Configuration = "127.0.0.1:6379",
             InstanceName = "Identity:"
         }));
            services.AddSession();

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

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
            app.UseIdentityServer();
            app.UseMvcWithDefaultRoute();
        }
    }
}
