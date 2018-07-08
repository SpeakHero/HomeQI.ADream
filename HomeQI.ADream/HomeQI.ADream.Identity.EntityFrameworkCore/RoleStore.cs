// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HomeQI.ADream.EntityFrameworkCore;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeQI.Adream.Identity.EntityFrameworkCore
{

    /// <summary>
    /// 
    /// </summary>
    public class RoleStore : RoleStoreBase<IdentityDbContext>,
        IQueryableRoleStore<IdentityRole>,
        IRoleClaimStore<IdentityRole>
    {

        public RoleStore(IdentityDbContext context, IdentityErrorDescriber errorDescriber, ILoggerFactory loggerFactory) : base(context, errorDescriber, loggerFactory)
        {
        }

        /// <summary>
        /// Gets the ID for a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
        public override Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new InvalidOperationEx(nameof(role));
            }
            return Task.FromResult(role.Id);
        }

        /// <summary>
        /// Gets the name of a role from the store as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose name should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the name of the role.</returns>
        public override Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (role == null)
            {
                throw new InvalidOperationEx(nameof(role));
            }
            return Task.FromResult(role.Name);
        }

        /// <summary>
        /// Finds the role who has the specified normalized name as an asynchronous operation.
        /// </summary>
        /// <param name="normalizedName">The normalized role name to look for.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that result of the look up.</returns>
        public override Task<IdentityRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Roles.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, cancellationToken);
        }



        /// <summary>
        /// Get the claims associated with the specified <paramref name="role"/> as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose claims should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the claims granted to a role.</returns>
        public async override Task<IList<Claim>> GetClaimsAsync(IdentityRole role, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new InvalidOperationEx(nameof(role));
            }

            return await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Adds the <paramref name="claim"/> given to the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to add the claim to.</param>
        /// <param name="claim">The claim to add to the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public override Task AddClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new InvalidOperationEx(nameof(role));
            }
            if (claim == null)
            {
                throw new InvalidOperationEx(nameof(claim));
            }

            RoleClaims.Add(CreateRoleClaim(role, claim));
            return Task.FromResult(false);
        }

        /// <summary>
        /// Removes the <paramref name="claim"/> given from the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove from the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async override Task RemoveClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new InvalidOperationEx(nameof(role));
            }
            if (claim == null)
            {
                throw new InvalidOperationEx(nameof(claim));
            }
            var claims = await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id) && rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type).ToListAsync(cancellationToken);
            foreach (var c in claims)
            {
                RoleClaims.Remove(c);
            }
        }


        private DbSet<IdentityRoleClaim> RoleClaims { get { return Context.Set<IdentityRoleClaim>(); } }

        public override IQueryable<IdentityRole> Roles => Context.Set<IdentityRole>();



    }
}
