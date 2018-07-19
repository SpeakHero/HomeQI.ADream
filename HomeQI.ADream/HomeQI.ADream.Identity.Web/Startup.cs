using HomeQI.Adream.Identity;
using HomeQI.Adream.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Redis;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using NLog.Extensions.Logging;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
namespace HomeQI.ADream.Identity.Web
{
    /// <summary>
    /// 
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }
        /// <summary>
        /// 
        /// </summary>
        public IConfiguration Configuration { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="services"></param>
        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var basePath = PlatformServices.Default.Application.ApplicationBasePath; // 获取到应用程序的根路径
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
                c.IncludeXmlComments(basePath + "\\HomeQI.ADream.Identity.Web.xml");
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
            services.AddIdentityJwtBear();
            services.AddADeamServices();
            services.AddSingleton<IDistributedCache>(
     serviceProvider =>
         new RedisCache(new RedisCacheOptions
         {
             Configuration = "127.0.0.1:6379",
             InstanceName = "Identity:"
         }));
            services.AddSession(o =>
            {
                o.IdleTimeout = TimeSpan.FromHours(2);
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        // 此方法由运行时调用。使用此方法配置HTTP请求管道。
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHsts();
            }

            app.UseAuthentication();//注意添加这一句，启用验证
            //启用中间件服务生成Swagger作为JSON终结点
            app.UseSwagger();
            //启用中间件服务对swagger-ui，指定Swagger JSON终结点
            app.UseSwaggerUI(c =>
            {
                c.DocumentTitle = "HomeQI ADream Identity";
                c.OAuthClientId("Adream");
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Identity API V1");
            });
            app.UseHttpsRedirection();
            #region TokenAuth
            //app.UseMiddleware<TokenAuth>();
            #endregion
            app.UseSession(new SessionOptions() { IdleTimeout = TimeSpan.FromMinutes(30) });
            app.UseMvc();
        }
    }
}
