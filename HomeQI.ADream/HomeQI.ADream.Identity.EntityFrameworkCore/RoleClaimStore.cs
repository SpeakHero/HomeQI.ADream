using HomeQI.Adream.Identity;
using HomeQI.Adream.Identity.EntityFrameworkCore;
using HomeQI.ADream.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.EntityFrameworkCore
{
    public class RoleClaimStore : EntityStore<IdentityRole,
        IdentityResult, IdentityDbContext, IdentityError>,
        IRoleClaimStore<IdentityRole>
    {
        public RoleClaimStore(IdentityDbContext context,
            IdentityErrorDescriber errorDescriber,
            ILoggerFactory loggerFactory, IConfiguration configuration) :
            base(context, errorDescriber, loggerFactory, configuration)
        {
        }
        protected virtual IdentityRoleClaim CreateRoleClaim(IdentityRole role, Claim claim)
      => new IdentityRoleClaim { RoleId = role.Id, ClaimType = claim.Type, ClaimValue = claim.Value };
        public virtual async Task<IdentityResult> AddClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            role.CheakArgument();
            claim.CheakArgument();
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

        public async virtual Task<IdentityResult> RemoveClaimAsync(IdentityRole role, Claim claim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            role.CheakArgument();
            claim.CheakArgument();
            var rolecasims = RoleClaims.Where(rc =>
  rc.RoleId.Equals(role.Id) &&
  rc.ClaimValue == claim.Value &&
  rc.ClaimType == claim.Type).Select(s => new IdentityRole() { Id = s.Id, EditedTime = s.EditedTime });
            AutoSaveChanges = false;
            foreach (var item in rolecasims)
            {
                item.IsDeleted = true;
                var delte = await DeleteAsync(item, default);
            }
            AutoSaveChanges = true;
            return await SaveChangesAsync(cancellationToken);
        }
        protected virtual DbSet<IdentityRoleClaim> RoleClaims { get { return Context.Set<IdentityRoleClaim>(); } }
    }
}
