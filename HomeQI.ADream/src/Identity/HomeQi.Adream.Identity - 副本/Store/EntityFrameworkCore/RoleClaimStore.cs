using HomeQI.ADream.Identity.Entites;
using Microsoft.EntityFrameworkCore;
using System;
using HomeQI.ADream.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HomeQI.ADream.Infrastructure.Core;
using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Store;
using Microsoft.Extensions.Logging;

namespace HomeQI.ADream.Identity.EntityFrameworkCore
{
    public class RoleClaimStore : EntityStore<Role, IdentityResult, IdentityDbContext, IdentityError>, IRoleClaimStore
    {
        public RoleClaimStore(IdentityDbContext context, IdentityErrorDescriber errorDescriber, ILoggerFactory loggerFactory) : base(context, errorDescriber, loggerFactory)
        {
        }
        protected virtual RoleClaim CreateRoleClaim(Role role, Claim claim)
      => new RoleClaim { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
        public virtual Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            role.CheakArgument();
            claim.CheakArgument();
            Context.RoleClaims.Add(CreateRoleClaim(role, claim));
            return Task.CompletedTask;
        }
        public virtual Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.CheakArgument();
            return Task.FromResult(role.Id);
        }

        public virtual Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.CheakArgument();
            return Task.FromResult(role.Name);
        }

        public virtual Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.CheakArgument();
            role.Name = roleName;
            return Task.CompletedTask;
        }


        public virtual Task<Role> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return DbEntitySet.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, cancellationToken);
        }

        public virtual Task<string> GetNormalizedRoleNameAsync(Role role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.CheakArgument();
            return Task.FromResult(role.NormalizedName);
        }


        public virtual Task SetNormalizedRoleNameAsync(Role role, string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.CheakArgument();
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }
        public async virtual Task<IList<Claim>> GetClaimsAsync(Role role, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            role.CheakArgument();
            return await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToListAsync(cancellationToken);
        }

        public async virtual Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            role.CheakArgument();
            claim.CheakArgument();
            var claims = await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id) && rc.ClaimValue == claim.Value && rc.ClaimType == claim.Type).ToListAsync(cancellationToken);
            foreach (var c in claims)
            {
                RoleClaims.Remove(c);
            }
        }
        protected virtual DbSet<RoleClaim> RoleClaims { get { return Context.Set<RoleClaim>(); } }
    }
}
