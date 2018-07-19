// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using HomeQI.Adream.Identity;
using HomeQI.ADream.Entities.Framework;

namespace Microsoft.Extensions.DependencyInjection.Extensions
{
    /// <summary>
    /// Contains extension methods to <see cref="IServiceCollection"/> for configuring identity services.
    /// </summary>
    public static class IdentityServiceCollectionExtensions
    {
        /// <summary>
        /// 添加和配置指定用户和角色类型的标识系统。
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static IdentityBuilder AddIdentityCore<TUser>(this IServiceCollection services) where TUser : EntityBase<string>
            => services.AddIdentityCore<TUser>(o => { });

        /// <summary>
        ///添加和配置指定用户和角色类型的标识系统。
        /// </summary>
        /// <typeparam name="TUser">The type representing a User in the system.</typeparam>
        /// <param name="services">The services available in the application.</param>
        /// <param name="setupAction">An action to configure the <see cref="IdentityOptions"/>.</param>
        /// <returns>An <see cref="IdentityBuilder"/> for creating and configuring the identity system.</returns>
        public static IdentityBuilder AddIdentityCore<TUser>(this IServiceCollection services, Action<IdentityOptions> setupAction)
            where TUser : EntityBase<string>
        {
            // Services identity depends on
            services.AddOptions().AddLogging();

            // Services used by identity
            // services.TryAddScoped<IUserValidator<TUser>, UserValidator<TUser>>();
            services.TryAddScoped<IPasswordValidator<TUser>, PasswordValidator<TUser>>();
            services.TryAddScoped<IPasswordHasher<TUser>, PasswordHasher<TUser>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            // No interface for the error describer so we can add errors without rev'ing the interface
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<TUser>, UserClaimsPrincipalFactory<TUser>>();
            services.TryAddScoped<UserManager<TUser>>();
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new IdentityBuilder(typeof(TUser), services);
        }
    }
}