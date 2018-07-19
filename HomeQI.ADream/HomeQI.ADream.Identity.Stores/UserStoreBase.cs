// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// 表示指定用户类型的持久性存储的新实例。
    /// </summary>

    public abstract class UserStoreBase<TDbcontext, TUser, TRole, TUserClaim, TUserLogin, TUserRole, TUserToken> : EntityStore<TUser, IdentityResult, TDbcontext, IdentityError>,
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
        IUserTwoFactorRecoveryCodeStore<TUser>
        where TDbcontext : ADreamDbContext where TUser : IdentityUser<string> where TRole : IdentityRole<string>, new() where TUserClaim : IdentityUserClaim<string>, new() where TUserLogin : IdentityUserLogin<string>, new() where TUserRole : IdentityUserRole<string>, new() where TUserToken : IdentityUserToken<string>, new()
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/> used to describe store errors.</param>
        public UserStoreBase(TDbcontext dbcontext, IdentityErrorDescriber describer, ILoggerFactory loggerFactory) : base(dbcontext, describer, loggerFactory)
        {
            ErrorDescriber = describer ?? throw new InvalidOperationEx(nameof(describer));
        }

        protected virtual TUserClaim CreateUserClaim(TUser user, Claim claim)
        {
            var userClaim = new TUserClaim { UserId = user.Id };
            userClaim.InitializeFromClaim(claim);
            return userClaim;
        }

        /// <summary>
        /// Called to create a new instance of a <see cref="TUserLogin{string}"/>.
        /// </summary>
        /// <param name="user">The associated user.</param>
        /// <param name="login">The sasociated login.</param>
        /// <returns></returns>
        protected virtual TUserLogin CreateUserLogin(TUser user, UserLoginInfo login)
        {
            return new TUserLogin
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName
            };
        }

        protected virtual TUserToken CreateUserToken(TUser user, string loginProvider, string name, string value)
        {
            return new TUserToken
            {
                UserId = user.Id,
                LoginProvider = loginProvider,
                Name = name,
                Value = value
            };
        }

        public virtual Task<string> GetUserIdAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.Id);
        }

        public virtual Task<string> GetUserNameAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.UserName);
        }

        public virtual Task SetUserNameAsync(TUser user, string userName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetNormalizedUserNameAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.NormalizedUserName);
        }

        public virtual Task SetNormalizedUserNameAsync(TUser user, string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }
        public abstract Task<TUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default);

        /// <summary>
        /// A navigation property for the users the store contains.
        /// </summary>
        public abstract IQueryable<TUser> Users
        {
            get;
        }

        public virtual Task SetPasswordHashAsync(TUser user, string passwordHash, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetPasswordHashAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.PasswordHash);
        }

        public virtual Task<bool> HasPasswordAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PasswordHash != null);
        }

        protected abstract Task<TUser> FindUserAsync(string userId, CancellationToken cancellationToken);

        protected abstract Task<TUserLogin> FindUserLoginAsync(string userId, string loginProvider, string providerKey, CancellationToken cancellationToken);

        protected abstract Task<TUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);

        public abstract Task<IList<Claim>> GetClaimsAsync(TUser user, CancellationToken cancellationToken = default);

        public abstract Task AddClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);

        public abstract Task ReplaceClaimAsync(TUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default);

        public abstract Task RemoveClaimsAsync(TUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);

        public abstract Task AddLoginAsync(TUser user, UserLoginInfo login, CancellationToken cancellationToken = default);

        public abstract Task RemoveLoginAsync(TUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default);

        public abstract Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user, CancellationToken cancellationToken = default);

        public async virtual Task<TUser> FindByLoginAsync(string loginProvider, string providerKey,
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

        public virtual Task<bool> GetEmailConfirmedAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.EmailConfirmed);
        }

        public virtual Task SetEmailConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }


        public virtual Task SetEmailAsync(TUser user, string email, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.Email = email;
            return Task.CompletedTask;
        }
        public virtual Task<string> GetEmailAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.Email);
        }

        public virtual Task<string> GetNormalizedEmailAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.Email);
        }

        public virtual Task SetNormalizedEmailAsync(TUser user, string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.Email = normalizedEmail;
            return Task.CompletedTask;
        }

        public abstract Task<TUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);

        public virtual Task<DateTimeOffset?> GetLockoutEndDateAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.LockoutEnd);
        }

        public virtual Task SetLockoutEndDateAsync(TUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }

        public virtual Task<int> IncrementAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task ResetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public virtual Task<int> GetAccessFailedCountAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task<bool> GetLockoutEnabledAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.LockoutEnabled);
        }

        public virtual Task SetLockoutEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }
        public virtual Task SetPhoneNumberAsync(TUser user, string phoneNumber, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetPhoneNumberAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.PhoneNumber);
        }

        public virtual Task<bool> GetPhoneNumberConfirmedAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public virtual Task SetPhoneNumberConfirmedAsync(TUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public virtual Task SetSecurityStampAsync(TUser user, string stamp, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            stamp.CheakArgument();
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetSecurityStampAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.SecurityStamp);
        }

        public virtual Task SetTwoFactorEnabledAsync(TUser user, bool enabled, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public virtual Task<bool> GetTwoFactorEnabledAsync(TUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public abstract Task<IList<TUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default);

        protected abstract Task<TUserToken> FindTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken);

        protected abstract Task AddUserTokenAsync(TUserToken token);

        protected abstract Task RemoveUserTokenAsync(TUserToken token);

        public virtual async Task SetTokenAsync(TUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            var token = await FindTokenAsync(user, loginProvider, name, cancellationToken);
            if (token == null)
            {
                await AddUserTokenAsync(CreateUserToken(user, loginProvider, name, value));
            }
            else
            {
                token.Value = value;
            }
        }

        public virtual async Task RemoveTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            var entry = await FindTokenAsync(user, loginProvider, name, cancellationToken);
            if (entry != null)
            {
                await RemoveUserTokenAsync(entry);
            }
        }

        public virtual async Task<string> GetTokenAsync(TUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            var entry = await FindTokenAsync(user, loginProvider, name, cancellationToken);
            return entry?.Value;
        }

        private const string InternalLoginProvider = "[UserStore]";
        private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
        private const string RecoveryCodeTokenName = "RecoveryCodes";


        public virtual Task SetAuthenticatorKeyAsync(TUser user, string key, CancellationToken cancellationToken)
            => SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);

        public virtual Task<string> GetAuthenticatorKeyAsync(TUser user, CancellationToken cancellationToken)
            => GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);

        public virtual async Task<int> CountCodesAsync(TUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
            if (mergedCodes.Length > 0)
            {
                return mergedCodes.Split(';').Length;
            }
            return 0;
        }

        public virtual Task ReplaceCodesAsync(TUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            var mergedCodes = string.Join(";", recoveryCodes);
            return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
        }

        public virtual async Task<bool> RedeemCodeAsync(TUser user, string code, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            code.CheakArgument();
            var mergedCodes = await GetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, cancellationToken) ?? "";
            var splitCodes = mergedCodes.Split(';');
            if (splitCodes.Contains(code))
            {
                var updatedCodes = new List<string>(splitCodes.Where(s => s != code));
                await ReplaceCodesAsync(user, updatedCodes, cancellationToken);
                return true;
            }
            return false;
        }

        protected virtual TUserRole CreateUserRole(TUser user, TRole role)
        {
            return new TUserRole()
            {
                UserId = user.Id,
                RoleId = role.Id
            };
        }
        public abstract Task<IList<TUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default);
        public abstract Task AddToRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default);

        public abstract Task RemoveFromRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default);

        public abstract Task<IList<string>> GetRolesAsync(TUser user, CancellationToken cancellationToken = default);

        public abstract Task<bool> IsInRoleAsync(TUser user, string normalizedRoleName, CancellationToken cancellationToken = default);
        protected abstract Task<TRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken);

        protected abstract Task<TUserRole> FindUserRoleAsync(string userId, string roleId, CancellationToken cancellationToken);
    }
}
