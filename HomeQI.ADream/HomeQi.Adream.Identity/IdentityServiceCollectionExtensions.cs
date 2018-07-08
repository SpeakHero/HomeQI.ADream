// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using HomeQI.Adream.Identity;
using Microsoft.Extensions.DependencyInjection.Extensions;
using HomeQi.Adream.Identity;
using IdentityServer4.Validation;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
    /// </summary>
    public static class IdentityServiceCollectionExt
    {

        public static IdentityBuilder AddIdentity(
            this IServiceCollection services)
            => services.AddIdentity(setupAction: null);

        /// <summary>
        /// Adds and configures the identity system for the specified User and Role types.
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <typeparam name="TRole">The type representing a Role in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="IdentityOptions"/>.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static IdentityBuilder AddIdentity(
            this IServiceCollection services,
            Action<IdentityOptions> setupAction)
        {
            // Services used by identity
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddCookie(IdentityConstants.ApplicationScheme, o =>
            {
                o.LoginPath = new PathString("/Account/Login");
                o.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                };
            })
            .AddCookie(IdentityConstants.ExternalScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.ExternalScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
                o.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
                };
            })
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });

            // Hosting doesn't add IHttpContextAccessor by default
            services.AddHttpContextAccessor();
            // Identity services
            //services.TryAddScoped<IUserValidator<IdentityUser>, UserValidator<IdentityUser>>();
            services.TryAddScoped<IPasswordValidator<IdentityUser>, PasswordValidator<IdentityUser>>();
            services.TryAddScoped<IPasswordHasher<IdentityUser>, PasswordHasher<IdentityUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<IdentityRole>, RoleValidator<IdentityRole>>();
            // 没有错误描述器的接口，所以我们可以在不修改接口的情况下添加错误。
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<IdentityUser>>();
            services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator<IdentityUser>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<IdentityUser>, UserClaimsPrincipalFactory<IdentityUser, IdentityRole>>();
            services.TryAddScoped<UserManager<IdentityUser>>();
            services.TryAddScoped<SignInManager<IdentityUser>>();
            services.TryAddScoped<RoleManager<IdentityRole>>();
            services.TryAddScoped<AspNetRoleManager<IdentityRole>>();
            services.TryAddScoped<AspNetUserManager<IdentityUser>>();
            services.TryAddScoped<UserManager>();
            services.TryAddScoped<RoleManager>();
            services.TryAddScoped<SignInManager>();
            services.TryAddScoped<IResourceOwnerPasswordValidator, ResourceOwnerPasswordValidator>();

            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new IdentityBuilder(typeof(IdentityUser), typeof(IdentityRole), services);
        }

        /// <summary>
        /// 配置应用程序Cookie。
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configure">An action to configure the <see cref="CookieAuthenticationOptions"/>.</param>
        /// <returns>The services.</returns>
        public static IServiceCollection ConfigureApplicationCookie(this IServiceCollection services, Action<CookieAuthenticationOptions> configure)
            => services.Configure(IdentityConstants.ApplicationScheme, configure);

        /// <summary>
        /// 配置外部Cookie。
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configure">An action to configure the <see cref="CookieAuthenticationOptions"/>.</param>
        /// <returns>The services.</returns>
        public static IServiceCollection ConfigureExternalCookie(this IServiceCollection services, Action<CookieAuthenticationOptions> configure)
            => services.Configure(IdentityConstants.ExternalScheme, configure);

    }
}
