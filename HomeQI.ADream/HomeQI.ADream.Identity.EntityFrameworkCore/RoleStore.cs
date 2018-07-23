// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.Adream.Identity.EntityFrameworkCore
{
    public class RoleStore : RoleStore<IdentityRole, IdentityRoleClaim>
    {
        public RoleStore(IdentityDbContext context, IdentityErrorDescriber errorDescriber, ILoggerFactory loggerFactory, IConfiguration configuration) : base(context, errorDescriber, loggerFactory, configuration)
        {
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class RoleStore<TRole, TRoleClaim> : RoleStoreBase<IdentityDbContext, TRole, TRoleClaim>,
        IQueryableRoleStore<TRole>,
        IRoleClaimStore<TRole> where TRole : IdentityRole<string>, new() where TRoleClaim : IdentityRoleClaim<string>, new()
    {

        public RoleStore(IdentityDbContext context, IdentityErrorDescriber errorDescriber, ILoggerFactory loggerFactory, IConfiguration configuration) : base(context, errorDescriber, loggerFactory, configuration)
        {
        }

        /// <summary>
        /// 从存储中获取角色的ID作为异步操作。
        /// </summary>
        /// <param name="role">The role whose ID should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>A <see cref="Task{TResult}"/> that contains the ID of the role.</returns>
        public override Task<string> GetRoleIdAsync(TRole role, CancellationToken cancellationToken = default)
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
        public override Task<string> GetRoleNameAsync(TRole role, CancellationToken cancellationToken = default)
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
        public override Task<TRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
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
        public async override Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default)
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
        public override async Task<IdentityResult> AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
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
            var f2 = await DbEntitySet.AnyAsync(d => d.Id.Equals(role.Id));
            if (f2)
            {

                var flag = await RoleClaims.AnyAsync(a => a.ClaimType.Equals(claim.Type)
                 && a.ClaimValue.Equals(claim.Value) && a.RoleId.Equals(role.Id));
                if (!flag)
                {
                    RoleClaims.Add(CreateRoleClaim(role, claim));
                    return await SaveChangesAsync(cancellationToken);
                }
            }
            return IdentityResult.Failed("已经存在该权限");
        }

        /// <summary>
        /// Removes the <paramref name="claim"/> given from the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove from the role.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        public async override Task<IdentityResult> RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default)
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
            var rolecasims = RoleClaims.Where(rc =>
             rc.RoleId.Equals(role.Id) &&
             rc.ClaimValue == claim.Value &&
             rc.ClaimType == claim.Type).Select(s => new TRoleClaim() { Id = s.Id, EditedTime = s.EditedTime });
            //  await rolecasims.ForEachAsync(f => f.IsDeleted = true);
            AutoSaveChanges = false;
            foreach (var item in rolecasims)
            {
                item.IsDeleted = true;
                var delte = await DeleteAsync(item, default);
            }
            AutoSaveChanges = true;
            return await SaveChangesAsync(cancellationToken);
        }


        private DbSet<TRoleClaim> RoleClaims
        {
            get
            {
                return Context.Set<TRoleClaim>();
            }
        }

        public override IQueryable<TRole> Roles
        {
            get
            {
                return Context.Set<TRole>();
            }
        }
    }
}
