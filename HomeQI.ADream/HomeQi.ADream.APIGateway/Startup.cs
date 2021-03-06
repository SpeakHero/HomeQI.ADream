﻿using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Linq;

namespace HomeQI.ADream.APIGateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // IdentityServer
            #region IdentityServerAuthenticationOptions => need to refactor
            Action<IdentityServerAuthenticationOptions> isaOptClient = option =>
            {
                option.Authority = Configuration["IdentityService:Uri"];
                option.ApiName = "clientservice";
                option.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]);
                option.SupportedTokens = SupportedTokens.Both;
                option.ApiSecret = Configuration["IdentityService:ApiSecrets:clientservice"];
            };

            void isaOptProduct(IdentityServerAuthenticationOptions option)
            {
                option.Authority = Configuration["IdentityService:Uri"];
                option.ApiName = "productservice";
                option.RequireHttpsMetadata = Convert.ToBoolean(Configuration["IdentityService:UseHttps"]);
                option.SupportedTokens = SupportedTokens.Both;
                option.ApiSecret = Configuration["IdentityService:ApiSecrets:productservice"];
            }
            #endregion

            services.AddAuthentication()
                .AddIdentityServerAuthentication("ClientServiceKey", isaOptClient)
                .AddIdentityServerAuthentication("ProductServiceKey", isaOptProduct);
            // Ocelot

            //.AddOpenTracing(option =>
            //{
            //    option.CollectorUrl = Configuration["TracingCenter:Uri"];
            //    option.Service = Configuration["TracingCenter:Name"];
            //});
            // Swagger
            services.AddMvc();
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc($"{Configuration["Swagger:DocName"]}", new Info
                {
                    Title = Configuration["Swagger:Title"],
                    Version = Configuration["Swagger:Version"]
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            // -->> get from service discovery later
            var apiList = Configuration["Swagger:ServiceDocNames"].Split(',').ToList();
            app.UseMvc()
                .UseSwagger()
                .UseSwaggerUI(options =>
                {
                    apiList.ForEach(apiItem =>
                    {
                        options.SwaggerEndpoint($"/doc/{apiItem}/swagger.json", apiItem);
                    });
                });

            // Ocelot
        }
    }
}
