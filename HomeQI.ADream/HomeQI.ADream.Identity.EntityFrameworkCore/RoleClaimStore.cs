using HomeQI.Adream.Identity;
using HomeQI.Adream.Identity.EntityFrameworkCore;
using HomeQI.ADream.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.EntityFrameworkCore
{
    public class RoleClaimStore : EntityStore<IdentityRole, IdentityResult, IdentityDbContext, IdentityError>, IRoleClaimStore<IdentityRole>
    {
        public RoleClaimStore(IdentityDbContext context, IdentityErrorDescriber errorDescriber, ILoggerFactory loggerFactory) : base(context, errorDescriber, loggerFactory)
        {
        }
        protected virtual IdentityRoleClaim CreateRoleClaim(IdentityRole role, Claim claim)
      => new IdentityRoleClaim { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
        public virtual Task AddClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            role.CheakArgument();
            claim.CheakArgument();
            Context.RoleClaims.Add(CreateRoleClaim(role, claim));
            return Task.CompletedTask;
        }
        public virtual Task<string> GetRoleIdAsync(IdentityRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.CheakArgument();
            return Task.FromResult(role.Id);
        }

        public virtual Task<string> GetRoleNameAsync(IdentityRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.CheakArgument();
            return Task.FromResult(role.Name);
        }

        public virtual Task SetRoleNameAsync(IdentityRole role, string roleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.CheakArgument();
            role.Name = roleName;
            return Task.CompletedTask;
        }


        public virtual Task<IdentityRole> FindByNameAsync(string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return DbEntitySet.FirstOrDefaultAsync(r => r.NormalizedName == normalizedName, cancellationToken);
        }

        public virtual Task<string> GetNormalizedRoleNameAsync(IdentityRole role, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.CheakArgument();
            return Task.FromResult(role.NormalizedName);
        }


        public virtual Task SetNormalizedRoleNameAsync(IdentityRole role, string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            role.CheakArgument();
            role.NormalizedName = normalizedName;
            return Task.CompletedTask;
        }
        public async virtual Task<IList<Claim>> GetClaimsAsync(IdentityRole role, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            role.CheakArgument();
            return await RoleClaims.Where(rc => rc.RoleId.Equals(role.Id)).Select(c => new Claim(c.ClaimType, c.ClaimValue)).ToListAsync(cancellationToken);
        }

        public async virtual Task RemoveClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
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
        protected virtual DbSet<IdentityRoleClaim> RoleClaims { get { return Context.Set<IdentityRoleClaim>(); } }
    }
}
