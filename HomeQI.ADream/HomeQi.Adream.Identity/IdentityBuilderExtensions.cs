// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using HomeQI.ADream.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// Helper functions for configuring identity services.
    /// </summary>
    public static class IdentityBuilderExtensions
    {
        /// <summary>
        /// Adds the default token providers used to generate tokens for reset passwords, change email
        /// and change telephone number operations, and for two factor authentication token generation.
        /// </summary>
        /// <param name="builder">The current <see cref="IdentityBuilder"/> instance.</param>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public static IdentityBuilder AddDefaultTokenProviders(this IdentityBuilder builder)
        {
            var userType = builder.UserType;
            var dataProtectionProviderType = typeof(DataProtectorTokenProvider<>).MakeGenericType(userType);
            var phoneNumberProviderType = typeof(PhoneNumberTokenProvider<>).MakeGenericType(userType);
            var emailTokenProviderType = typeof(EmailTokenProvider<>).MakeGenericType(userType);
            var authenticatorProviderType = typeof(AuthenticatorTokenProvider<>).MakeGenericType(userType);
            return builder.AddTokenProvider(TokenOptions.DefaultProvider, dataProtectionProviderType)
                .AddTokenProvider(TokenOptions.DefaultEmailProvider, emailTokenProviderType)
                .AddTokenProvider(TokenOptions.DefaultPhoneProvider, phoneNumberProviderType)
                .AddTokenProvider(TokenOptions.DefaultAuthenticatorProvider, authenticatorProviderType);
        }

        private static void AddSignInManagerDeps(this IdentityBuilder builder)
        {
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped(typeof(ISecurityStampValidator), typeof(SecurityStampValidator<>).MakeGenericType(builder.UserType));
            builder.Services.AddScoped(typeof(ITwoFactorSecurityStampValidator), typeof(TwoFactorSecurityStampValidator<>).MakeGenericType(builder.UserType));
        }

        /// <summary>
        /// Adds a <see cref="SignInManager{TUser}"/> for the <seealso cref="IdentityBuilder.UserType"/>.
        /// </summary>
        /// <param name="builder">The current <see cref="IdentityBuilder"/> instance.</param>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public static IdentityBuilder AddSignInManager(this IdentityBuilder builder)
        {
            builder.AddSignInManagerDeps();
            var managerType = typeof(SignInManager<>).MakeGenericType(builder.UserType);
            builder.Services.AddScoped(managerType);
            return builder;
        }

        /// <summary>
        /// Adds a <see cref="SignInManager{TUser}"/> for the <seealso cref="IdentityBuilder.UserType"/>.
        /// </summary>
        /// <typeparam name="TSignInManager">The type of the sign in manager to add.</typeparam>
        /// <param name="builder">The current <see cref="IdentityBuilder"/> instance.</param>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public static IdentityBuilder AddSignInManager<TSignInManager>(this IdentityBuilder builder) where TSignInManager : class
        {
            builder.AddSignInManagerDeps();
            var managerType = typeof(SignInManager<>).MakeGenericType(builder.UserType);
            var customType = typeof(TSignInManager);
            if (!managerType.GetTypeInfo().IsAssignableFrom(customType.GetTypeInfo()))
            {
                throw new InvalidOperationEx(Resources.FormatInvalidManagerType(customType.Name, "SignInManager", builder.UserType.Name));
            }
            if (managerType != customType)
            {
                builder.Services.AddScoped(typeof(TSignInManager), services => services.GetRequiredService(managerType));
            }
            builder.Services.AddScoped(managerType, typeof(TSignInManager));
            return builder;
        }

        public static IServiceCollection AddIdentityJwtBear(this IServiceCollection services)
        {
            var configuration = services.BuildServiceProvider().GetRequiredService<IConfiguration>();
            ///////////////////jwt//////////////////////
            //将appsettings.json中的JwtSettings部分文件读取到JwtSettings中，这是给其他地方用的
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            //由于初始化的时候我们就需要用，所以使用Bind的方式读取配置
            //将配置绑定到JwtSettings实例中
            var jwtSettings = new JwtSettings();
            configuration.Bind("JwtSettings", jwtSettings);
            services.AddAuthentication(options =>
            {
                //认证middleware配置
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(o =>
           {
               ////TokenValidated：在Token验证通过后调用。

               // AuthenticationFailed: 认证失败时调用。

               ////Challenge: 未授权时调用。
               o.Events = new JwtBearerEvents()
               {
                   OnMessageReceived = context =>
                   {
                       context.Token = context.Request.Query["access_token"];
                       return Task.CompletedTask;
                   }
               };
               //主要是jwt  token参数设置
               o.TokenValidationParameters = new TokenValidationParameters
               {
                   //Token颁发机构
                   ValidIssuer = jwtSettings.Iss,
                   //颁发给谁
                   ValidAudience = jwtSettings.Aud,
                   //这里的key要进行加密，需要引用Microsoft.IdentityModel.Tokens
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                   //ValidateIssuerSigningKey=true,
                   ////是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                   //ValidateLifetime=true,
                   ////允许的服务器时间偏移量
                   //ClockSkew=TimeSpan.Zero
                   //    /***********************************TokenValidationParameters的参数默认值***********************************/
                   //    // RequireSignedTokens = true,
                   //    SaveSigninToken = true,
                   //    // ValidateActor = false,
                   //    // 将下面两个参数设置为false，可以不验证Issuer和Audience，但是不建议这样做。
                   //    ValidateAudience = false,
                   //    ValidateIssuer = false,
                   //    // ValidateIssuerSigningKey = false,
                   //    // 是否要求Token的Claims中必须包含Expires
                   //    // RequireExpirationTime = true,
                   //    // 允许的服务器时间偏移量
                   //    // ClockSkew = TimeSpan.FromSeconds(300),
                   //    // 是否验证Token有效期，使用当前时间与Token的Claims中的NotBefore和Expires对比
                   //    // ValidateLifetime = true
                   //};
               };
           }).AddCookie();
            return services;
        }
    }
}
