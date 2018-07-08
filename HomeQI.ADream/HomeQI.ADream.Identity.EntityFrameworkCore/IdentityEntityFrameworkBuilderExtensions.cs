// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Reflection;
using HomeQI.Adream.Identity;
using HomeQI.Adream.Identity.EntityFrameworkCore;
using HomeQI.ADream.Identity.Stores;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// Contains extension methods to <see cref="IdentityBuilder"/> for adding entity framework stores.
    /// </summary>
    public static class IdentityEntityFrameworkBuilderExtensions
    {
        /// <summary>
        /// Adds an Entity Framework implementation of identity information stores.
        /// </summary>
        /// <typeparam name="TContext">The Entity Framework database context to use.</typeparam>
        /// <param name="builder">The <see cref="IdentityBuilder"/> instance this method extends.</param>
        /// <returns>The <see cref="IdentityBuilder"/> instance this method extends.</returns>
        public static IdentityBuilder AddEntityFrameworkStores(this IdentityBuilder builder)

        {
            AddStores(builder.Services);
            return builder;
        }

        private static void AddStores(IServiceCollection services)
        {
            services.TryAddScoped<IUserStore<IdentityUser>, UserStore>();
            services.TryAddScoped<IRoleStore<IdentityRole>, RoleStore>();
            services.TryAddScoped<IUserValidator<IdentityUser>, UserValidator>();
        }

    }
}