// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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
namespace HomeQI.Adream.Identity.EntityFrameworkCore
{
    public class UserOnlyStore : UserOnlyStore<IdentityUser, IdentityRole, IdentityUserClaim, IdentityUserLogin, IdentityUserRole, IdentityUserToken>
    {
        public UserOnlyStore(IdentityDbContext context, IdentityErrorDescriber errorDescriber, ILoggerFactory loggerFactory, IConfiguration configuration)
            : base(context, errorDescriber, loggerFactory, configuration)
        {
        }
    }
    public class UserOnlyStore<TUser, TRole, TUserClaim, TUserLogin, TUserRole, TUserToken> : UserStoreBase<IdentityDbContext, TUser, TRole, TUserClaim, TUserLogin, TUserRole, TUserToken>,
        IUserLoginStore<TUser>,
        IUserClaimStore<TUser>,
        IUserPasswordStore<TUser>,
        IUserSecurityStampStore<TUser>,
        IUserEmailStore<TUser>,
        IUserLockoutStore<TUser>,
        IUserPhoneNumberStore<TUser>,
        IQueryableUserStore<TUser>,
        IUserTwoFactorStore<TUser>,
        IUserAuthenticationTokenStore<TUser>,
        IUserAuthenticatorKeyStore<TUser>,
        IUserTwoFactorRecoveryCodeStore<TUser>,
        IUserRoleStore<TUser>
        where TUser : IdentityUser<string>, new() where TRole : IdentityRole<string>, new() where TUserClaim : IdentityUserClaim<string>, new() where TUserLogin : IdentityUserLogin<string>, new() where TUserRole : IdentityUserRole<string>, new() where TUserToken : IdentityUserToken<string>, new()
    {
        public UserOnlyStore(IdentityDbContext context, IdentityErrorDescriber errorDescriber, ILoggerFactory loggerFactory, IConfiguration configuration) : base(context, errorDescriber, loggerFactory, configuration)
        {
        }



        /// <summary>
        /// DbSet of users.
        /// </summary>
        protected virtual DbSet<TUser> UsersSet { get { return Context.Set<TUser>(); } }

        /// <summary>
        /// DbSet of user claims.
        /// </summary>
        protected virtual DbSet<TUserClaim> UserClaims
        {
            get
            {
                return Context.Set<TUserClaim>();
            }
            set { }
        }

        /// <summary>
        /// DbSet of user logins.
        /// </summary>
        protected virtual DbSet<TUserLogin> UserLogins { get { return Context.Set<TUserLogin>(); } }

        /// <summary>
        /// DbSet of user tokens.
        /// </summary>
        protected virtual DbSet<TUserToken> UserTokens { get { return Context.Set<TUserToken>(); } }

        protected virtual DbSet<TRole> Roles { get { return Context.Set<TRole>(); } }
        protected virtual DbSet<TUserRole> UserRoles { get { return Context.Set<TUserRole>(); } }

        public override Task<IdentityResult> CreateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return base.CreateAsync(user, cancellationToken);


        }


        public override async Task<IdentityResult> UpdateAsync(TUser user, CancellationToken cancellationToken = default, params string[] propertys)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.SecurityStamp = Guid.NewGuid().ToString();
            return await base.UpdateAsync(user, cancellationToken, propertys);
        }

        public override Task<IdentityResult> DeleteAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return base.DeleteAsync(user, cancellationToken);
        }
        public override Task<TUser> FindByNameAsync(string UserName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Users.FirstOrDefaultAsync(u => u.UserName == UserName, cancellationToken);
        }
        public override IQueryable<TUser> Users => UsersSet;

        protected override Task<TUser> FindUserAsync(string userId, CancellationToken cancellationToken)
        {
            return Users.SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);
        }


        protected override Task<TUserLogin> FindUserLoginAsync(string userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return UserLogins.SingleOrDefaultAsync(userLogin => userLogin.UserId.Equals(userId) && userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);
        }
        protected override Task<TUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return UserLogins.SingleOrDefaultAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);
        }

        public override async Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            user.CheakArgument();
            return await UserClaims.Where(uc => uc.UserId.Equals(user.Id)).Select(c => c.ToClaim()).ToListAsync();
        }

        public override Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            user.CheakArgument();
            claims.CheakArgument();
            foreach (var claim in claims)
            {
                UserClaims.Add(CreateUserClaim(user, claim));
            }
            return Task.CompletedTask;
        }

        public override async Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            user.CheakArgument();
            claim.CheakArgument();
            newClaim.CheakArgument();
            var matchedClaims = await UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync();
            foreach (var matchedClaim in matchedClaims)
            {
                matchedClaim.ClaimValue = newClaim.Value;
                matchedClaim.ClaimType = newClaim.Type;
            }
        }
        public override async Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            user.CheakArgument();
            claims.CheakArgument();
            foreach (var claim in claims)
            {
                var matchedClaims = await UserClaims.Where(uc => uc.UserId.Equals(user.Id) && uc.ClaimValue == claim.Value && uc.ClaimType == claim.Type).ToListAsync();
                foreach (var c in matchedClaims)
                {
                    UserClaims.Remove(c);
                }
            }
        }

        public override Task AddLoginAsync(TUser user, UserLoginInfo login,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            login.CheakArgument();
            UserLogins.Add(CreateUserLogin(user, login));
            return Task.FromResult(false);
        }

        public override async Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            var entry = await FindUserLoginAsync(user.Id, loginProvider, providerKey, cancellationToken);
            if (entry != null)
            {
                UserLogins.Remove(entry);
            }
        }

        public async override Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            var userId = user.Id;
            return await UserLogins.Where(l => l.UserId.Equals(userId))
                .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToListAsync();
        }

        public override async Task<TUser> FindByLoginAsync(string loginProvider, string providerKey,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var userLogin = await FindUserLoginAsync(loginProvider, providerKey, cancellationToken);
            if (userLogin != null)
            {
                return await FindUserAsync(userLogin.UserId, cancellationToken);
            }
            return null;
        }

        public override Task<TUser> FindByEmailAsync(string Email, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Users.FirstOrDefaultAsync(u => u.Email == Email, cancellationToken);
        }

        public override async Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            claim.CheakArgument();
            return await (from userclaims in UserClaims
                          join user in Users on userclaims.UserId equals user.Id
                          where userclaims.ClaimValue == claim.Value
                          && userclaims.ClaimType == claim.Type
                          select user).ToListAsync();

        }

        protected override Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
            => UserTokens.FindAsync(new object[] { user.Id, loginProvider, name }, cancellationToken);

        protected override Task AddUserTokenAsync(TUserToken token)
        {
            UserTokens.Add(token);
            return Task.CompletedTask;
        }
        protected override Task RemoveUserTokenAsync(TUserToken token)
        {
            UserTokens.Remove(token);
            return Task.CompletedTask;
        }

        public override async Task<IList<TUser>> GetUsersInRoleAsync(string RoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (string.IsNullOrEmpty(RoleName))
            {
                throw new ArgumentNullException(nameof(RoleName));
            }
            var role = await FindRoleAsync(RoleName, cancellationToken);

            if (role != null)
            {
                return await (from userrole in UserRoles
                              join user in Users on userrole.UserId equals user.Id
                              where userrole.RoleId.Equals(role.Id)
                              select user).ToListAsync();
            }
            return new List<TUser>();
        }

        public override async Task AddToRoleAsync(TUser user, string RoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                throw new ArgumentNullEx(nameof(RoleName));
            }
            var roleEntity = await FindRoleAsync(RoleName, cancellationToken);
            if (roleEntity == null)
            {
                throw new InvalidOperationEx("RoleNotFound");
            }
            UserRoles.Add(CreateUserRole(user, roleEntity));
        }

        public override async Task RemoveFromRoleAsync(TUser user, string RoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                throw new ArgumentNullEx(nameof(RoleName));
            }
            var roleEntity = await FindRoleAsync(RoleName, cancellationToken);
            if (roleEntity != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, roleEntity.Id, cancellationToken);
                if (userRole != null)
                {
                    UserRoles.Remove(userRole);
                }
            }
        }

        public override async Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            var userId = user.Id;
            return await (from userRole in UserRoles
                          join role in Roles on userRole.RoleId equals role.Id
                          where userRole.UserId.Equals(userId)
                          select role.Name).ToListAsync();
        }

        public override async Task<bool> IsInRoleAsync(TUser user, string RoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                throw new ArgumentNullEx(nameof(RoleName));
            }
            var role = await FindRoleAsync(RoleName, cancellationToken);
            if (role != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
                return userRole != null;
            }
            return false;
        }

        protected override Task<TRole> FindRoleAsync(string RoleName, CancellationToken cancellationToken)
        {
            return Roles.SingleOrDefaultAsync(r => r.Name == RoleName, cancellationToken);
        }

        protected override Task<TUserRole> FindUserRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            return UserRoles.FindAsync(new object[] { userId, roleId }, cancellationToken);
        }
    }
}
