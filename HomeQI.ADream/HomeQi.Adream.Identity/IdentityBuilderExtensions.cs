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
            //��appsettings.json�е�JwtSettings�����ļ���ȡ��JwtSettings�У����Ǹ������ط��õ�
            services.Configure<JwtSettings>(configuration.GetSection("JwtSettings"));

            //���ڳ�ʼ����ʱ�����Ǿ���Ҫ�ã�����ʹ��Bind�ķ�ʽ��ȡ����
            //�����ð󶨵�JwtSettingsʵ����
            var jwtSettings = new JwtSettings();
            configuration.Bind("JwtSettings", jwtSettings);
            services.AddAuthentication(options =>
            {
                //��֤middleware����
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(o =>
           {
               ////TokenValidated����Token��֤ͨ������á�

               // AuthenticationFailed: ��֤ʧ��ʱ���á�

               ////Challenge: δ��Ȩʱ���á�
               o.Events = new JwtBearerEvents()
               {
                   OnMessageReceived = context =>
                   {
                       context.Token = context.Request.Query["access_token"];
                       return Task.CompletedTask;
                   }
               };
               //��Ҫ��jwt  token��������
               o.TokenValidationParameters = new TokenValidationParameters
               {
                   //Token�䷢����
                   ValidIssuer = jwtSettings.Iss,
                   //�䷢��˭
                   ValidAudience = jwtSettings.Aud,
                   //�����keyҪ���м��ܣ���Ҫ����Microsoft.IdentityModel.Tokens
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret))
                   //ValidateIssuerSigningKey=true,
                   ////�Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                   //ValidateLifetime=true,
                   ////����ķ�����ʱ��ƫ����
                   //ClockSkew=TimeSpan.Zero
                   //    /***********************************TokenValidationParameters�Ĳ���Ĭ��ֵ***********************************/
                   //    // RequireSignedTokens = true,
                   //    SaveSigninToken = true,
                   //    // ValidateActor = false,
                   //    // ������������������Ϊfalse�����Բ���֤Issuer��Audience�����ǲ�������������
                   //    ValidateAudience = false,
                   //    ValidateIssuer = false,
                   //    // ValidateIssuerSigningKey = false,
                   //    // �Ƿ�Ҫ��Token��Claims�б������Expires
                   //    // RequireExpirationTime = true,
                   //    // ����ķ�����ʱ��ƫ����
                   //    // ClockSkew = TimeSpan.FromSeconds(300),
                   //    // �Ƿ���֤Token��Ч�ڣ�ʹ�õ�ǰʱ����Token��Claims�е�NotBefore��Expires�Ա�
                   //    // ValidateLifetime = true
                   //};
               };
           }).AddCookie();
            return services;
        }
    }
}
