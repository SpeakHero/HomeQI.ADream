// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// 表示指定用户类型的持久性存储的新实例。
    /// </summary>

    public abstract class UserStoreBase<TDbcontext> : EntityStore<IdentityUser, IdentityResult, TDbcontext, IdentityError>,
        IUserLoginStore<IdentityUser>,
        IUserClaimStore<IdentityUser>,
        IUserPasswordStore<IdentityUser>,
        IUserSecurityStampStore<IdentityUser>,
        IUserEmailStore<IdentityUser>,
        IUserLockoutStore<IdentityUser>,
        IUserPhoneNumberStore<IdentityUser>,
        IQueryableUserStore<IdentityUser>,
        IUserTwoFactorStore<IdentityUser>,
        IUserAuthenticationTokenStore<IdentityUser>,
        IUserAuthenticatorKeyStore<IdentityUser>,
        IUserTwoFactorRecoveryCodeStore<IdentityUser>
        where TDbcontext : ADreamDbContext
    {
        /// <summary>
        /// Creates a new instance.
        /// </summary>
        /// <param name="describer">The <see cref="IdentityErrorDescriber"/> used to describe store errors.</param>
        public UserStoreBase(TDbcontext dbcontext, IdentityErrorDescriber describer, ILoggerFactory loggerFactory) : base(dbcontext, describer, loggerFactory)
        {
            ErrorDescriber = describer ?? throw new InvalidOperationEx(nameof(describer));
        }

        protected virtual IdentityUserClaim CreateUserClaim(IdentityUser user, Claim claim)
        {
            var userClaim = new IdentityUserClaim { UserId = user.Id };
            userClaim.InitializeFromClaim(claim);
            return userClaim;
        }

        /// <summary>
        /// Called to create a new instance of a <see cref="IdentityUserLogin{string}"/>.
        /// </summary>
        /// <param name="user">The associated user.</param>
        /// <param name="login">The sasociated login.</param>
        /// <returns></returns>
        protected virtual IdentityUserLogin CreateUserLogin(IdentityUser user, UserLoginInfo login)
        {
            return new IdentityUserLogin
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName
            };
        }

        protected virtual IdentityUserToken CreateUserToken(IdentityUser user, string loginProvider, string name, string value)
        {
            return new IdentityUserToken
            {
                UserId = user.Id,
                LoginProvider = loginProvider,
                Name = name,
                Value = value
            };
        }

        public virtual Task<string> GetUserIdAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.Id);
        }

        public virtual Task<string> GetUserNameAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.UserName);
        }

        public virtual Task SetUserNameAsync(IdentityUser user, string userName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetNormalizedUserNameAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.NormalizedUserName);
        }

        public virtual Task SetNormalizedUserNameAsync(IdentityUser user, string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }
        public abstract Task<IdentityUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default);

        /// <summary>
        /// A navigation property for the users the store contains.
        /// </summary>
        public abstract IQueryable<IdentityUser> Users
        {
            get;
        }

        public virtual Task SetPasswordHashAsync(IdentityUser user, string passwordHash, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetPasswordHashAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.PasswordHash);
        }

        public virtual Task<bool> HasPasswordAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PasswordHash != null);
        }

        protected abstract Task<IdentityUser> FindUserAsync(string userId, CancellationToken cancellationToken);

        protected abstract Task<IdentityUserLogin> FindUserLoginAsync(string userId, string loginProvider, string providerKey, CancellationToken cancellationToken);

        protected abstract Task<IdentityUserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);

        public abstract Task<IList<Claim>> GetClaimsAsync(IdentityUser user, CancellationToken cancellationToken = default);

        public abstract Task AddClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);

        public abstract Task ReplaceClaimAsync(IdentityUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default);

        public abstract Task RemoveClaimsAsync(IdentityUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);

        public abstract Task AddLoginAsync(IdentityUser user, UserLoginInfo login, CancellationToken cancellationToken = default);

        public abstract Task RemoveLoginAsync(IdentityUser user, string loginProvider, string providerKey, CancellationToken cancellationToken = default);

        public abstract Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user, CancellationToken cancellationToken = default);

        public async virtual Task<IdentityUser> FindByLoginAsync(string loginProvider, string providerKey,
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

        public virtual Task<bool> GetEmailConfirmedAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.EmailConfirmed);
        }

        public virtual Task SetEmailConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }


        public virtual Task SetEmailAsync(IdentityUser user, string email, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.Email = email;
            return Task.CompletedTask;
        }
        public virtual Task<string> GetEmailAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.Email);
        }

        public virtual Task<string> GetNormalizedEmailAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.Email);
        }

        public virtual Task SetNormalizedEmailAsync(IdentityUser user, string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.Email = normalizedEmail;
            return Task.CompletedTask;
        }

        public abstract Task<IdentityUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);

        public virtual Task<DateTimeOffset?> GetLockoutEndDateAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.LockoutEnd);
        }

        public virtual Task SetLockoutEndDateAsync(IdentityUser user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }

        public virtual Task<int> IncrementAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task ResetAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public virtual Task<int> GetAccessFailedCountAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task<bool> GetLockoutEnabledAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.LockoutEnabled);
        }

        public virtual Task SetLockoutEnabledAsync(IdentityUser user, bool enabled, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }
        public virtual Task SetPhoneNumberAsync(IdentityUser user, string phoneNumber, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetPhoneNumberAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.PhoneNumber);
        }

        public virtual Task<bool> GetPhoneNumberConfirmedAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public virtual Task SetPhoneNumberConfirmedAsync(IdentityUser user, bool confirmed, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public virtual Task SetSecurityStampAsync(IdentityUser user, string stamp, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            stamp.CheakArgument();
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetSecurityStampAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.SecurityStamp);
        }

        public virtual Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public virtual Task<bool> GetTwoFactorEnabledAsync(IdentityUser user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public abstract Task<IList<IdentityUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default);

        protected abstract Task<IdentityUserToken> FindTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken);

        protected abstract Task AddUserTokenAsync(IdentityUserToken token);

        protected abstract Task RemoveUserTokenAsync(IdentityUserToken token);

        public virtual async Task SetTokenAsync(IdentityUser user, string loginProvider, string name, string value, CancellationToken cancellationToken)
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

        public virtual async Task RemoveTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
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

        public virtual async Task<string> GetTokenAsync(IdentityUser user, string loginProvider, string name, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            var entry = await FindTokenAsync(user, loginProvider, name, cancellationToken);
            return entry?.Value;
        }

        private const string InternalLoginProvider = "[AspNeUserStore]";
        private const string AuthenticatorKeyTokenName = "AuthenticatorKey";
        private const string RecoveryCodeTokenName = "RecoveryCodes";


        public virtual Task SetAuthenticatorKeyAsync(IdentityUser user, string key, CancellationToken cancellationToken)
            => SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);

        public virtual Task<string> GetAuthenticatorKeyAsync(IdentityUser user, CancellationToken cancellationToken)
            => GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);

        public virtual async Task<int> CountCodesAsync(IdentityUser user, CancellationToken cancellationToken)
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

        public virtual Task ReplaceCodesAsync(IdentityUser user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            var mergedCodes = string.Join(";", recoveryCodes);
            return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
        }

        public virtual async Task<bool> RedeemCodeAsync(IdentityUser user, string code, CancellationToken cancellationToken)
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

        protected virtual IdentityUserRole CreateUserRole(IdentityUser user, IdentityRole role)
        {
            return new IdentityUserRole()
            {
                UserId = user.Id,
                RoleId = role.Id
            };
        }
        public abstract Task<IList<IdentityUser>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default);
        public abstract Task AddToRoleAsync(IdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default);

        public abstract Task RemoveFromRoleAsync(IdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default);

        public abstract Task<IList<string>> GetRolesAsync(IdentityUser user, CancellationToken cancellationToken = default);

        public abstract Task<bool> IsInRoleAsync(IdentityUser user, string normalizedRoleName, CancellationToken cancellationToken = default);
        protected abstract Task<IdentityRole> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken);

        protected abstract Task<IdentityUserRole> FindUserRoleAsync(string userId, string roleId, CancellationToken cancellationToken);
    }
}
