using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Validators;
using Microsoft.Extensions.Logging;

namespace HomeQI.ADream.Identity.Manager
{
    public interface IRoleManager: IDisposable
    {
        IdentityErrorDescriber ErrorDescriber { get; set; }
        ILookupNormalizer KeyNormalizer { get; set; }
        ILogger Logger { get; set; }
        IQueryable<Role> Roles { get; }
        IList<IRoleValidator> RoleValidators { get; }
        bool SupportsQueryableRoles { get; }
        bool SupportsRoleClaims { get; }

        Task<IdentityResult> AddClaimAsync(Role role, Claim claim);
        Task<IdentityResult> CreateAsync(Role role);
        Task<IdentityResult> DeleteAsync(Role role);
        Task<Role> FindByIdAsync(string roleId);
        Task<Role> FindByNameAsync(string roleName);
        Task<IList<Claim>> GetClaimsAsync(Role role);
        Task<string> GetRoleIdAsync(Role role);
        Task<string> GetRoleNameAsync(Role role);
        string NormalizeKey(string key);
        Task<IdentityResult> RemoveClaimAsync(Role role, Claim claim);
        Task<bool> RoleExistsAsync(string roleName);
        Task<IdentityResult> SetRoleNameAsync(Role role, string name);
        Task<IdentityResult> UpdateAsync(Role role);
        Task UpdateRoleNameAsync(Role role);
    }
}