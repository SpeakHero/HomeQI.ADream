// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Security.Claims;
using System.Threading.Tasks;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Manager;
using HomeQI.ADream.Identity.Options;
using Microsoft.Extensions.Options;

namespace HomeQI.ADream.Identity
{
    /// <summary>
    /// 提供用于为给定用户创建索赔主体的方法
    /// </summary>
    public class UserClaimsPrincipalFactory : IUserClaimsPrincipalFactory
    {
        
        public UserClaimsPrincipalFactory(
            IUserManager userManager,
            IOptions<IdentityOptions> optionsAccessor)
        {
            userManager.CheakArgument();
            if (optionsAccessor == null || optionsAccessor.Value == null)
            {
                throw new ArgumentNullException(nameof(optionsAccessor));
            }
            UserManager = userManager;
            Options = optionsAccessor.Value;
        }

        /// <summary>
        /// Gets the <see cref="UserManager"/> for this factory.
        /// </summary>
        /// <value>
        /// The current <see cref="UserManager"/> for this factory instance.
        /// </value>
        public IUserManager UserManager { get; private set; }

        /// <summary>
        /// Gets the <see cref="IdentityOptions"/> for this factory.
        /// </summary>
        /// <value>
        /// The current <see cref="IdentityOptions"/> for this factory instance.
        /// </value>
        public IdentityOptions Options { get; private set; }

        /// <summary>
        /// Creates a <see cref="ClaimsPrincipal"/> from an user asynchronously.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsPrincipal"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ClaimsPrincipal"/>.</returns>
        public virtual async Task<ClaimsPrincipal> CreateAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var id = await GenerateClaimsAsync(user);
            return new ClaimsPrincipal(id);
        }

        /// <summary>
        /// Generate the claims for a user.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsIdentity"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ClaimsIdentity"/>.</returns>
        protected virtual async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var userId = await UserManager.GetUserIdAsync(user);
            var userName = await UserManager.GetUserNameAsync(user);
            var id = new ClaimsIdentity("Identity.Application", // 回顾：用于匹配应用方案
                Options.ClaimsIdentity.UserNameClaimType,
                Options.ClaimsIdentity.RoleClaimType);
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserIdClaimType, userId));
            id.AddClaim(new Claim(Options.ClaimsIdentity.UserNameClaimType, userName));
            if (UserManager.SupportsUserSecurityStamp)
            {
                id.AddClaim(new Claim(Options.ClaimsIdentity.SecurityStampClaimType,
                    await UserManager.GetSecurityStampAsync(user)));
            }
            if (UserManager.SupportsUserClaim)
            {
                id.AddClaims(await UserManager.GetClaimsAsync(user));
            }
            return id;
        }
    }

    /// <summary>
    /// Provides methods to create a claims principal for a given user.
    /// </summary>
    /// <typeparam name="TUser">The type used to represent a user.</typeparam>
    /// <typeparam name="TRole">The type used to represent a role.</typeparam>
    public class UserClaimsPrincipalFactory<TUser, TRole> : UserClaimsPrincipalFactory
        where TUser : class
        where TRole : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UserClaimsPrincipalFactory{TUser, TRole}"/> class.
        /// </summary>
        /// <param name="userManager">The <see cref="UserManager"/> to retrieve user information from.</param>
        /// <param name="roleManager">The <see cref="RoleManager"/> to retrieve a user's roles from.</param>
        /// <param name="options">The configured <see cref="IdentityOptions"/>.</param>
        public UserClaimsPrincipalFactory(IUserManager userManager, IRoleManager roleManager, IOptions<IdentityOptions> options)
            : base(userManager, options)
        {
            roleManager.CheakArgument();
              RoleManager = roleManager;
        }

        /// <summary>
        /// Gets the <see cref="IRoleManager"/> for this factory.
        /// </summary>
        /// <value>
        /// The current <see cref="IRoleManager"/> for this factory instance.
        /// </value>
        public IRoleManager RoleManager { get; private set; }

        /// <summary>
        /// Generate the claims for a user.
        /// </summary>
        /// <param name="user">The user to create a <see cref="ClaimsIdentity"/> from.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous creation operation, containing the created <see cref="ClaimsIdentity"/>.</returns>
        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user)
        {
            var id = await base.GenerateClaimsAsync(user);
            if (UserManager.SupportsUserRole)
            {
                var roles = await UserManager.GetRolesAsync(user);
                foreach (var roleName in roles)
                {
                    id.AddClaim(new Claim(Options.ClaimsIdentity.RoleClaimType, roleName));
                    if (RoleManager.SupportsRoleClaims)
                    {
                        var role = await RoleManager.FindByNameAsync(roleName);
                        if (role != null)
                        {
                            id.AddClaims(await RoleManager.GetClaimsAsync(role));
                        }
                    }
                }
            }
            return id;
        }
    }
}