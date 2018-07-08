using HomeQI.ADream.EntityFrameworkCore;
using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Store;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.EntityFrameworkCore
{
    public abstract class UserStoreBase : EntityStore<User, IdentityResult, IdentityDbContext, IdentityError>,
        IUserLoginStore,
        IUserClaimStore,
        IUserPasswordStore,
        IUserSecurityStampStore,
        IUserEmailStore,
        IUserLockoutStore,
        IUserPhoneNumberStore,
        IQueryableUserStore,
        IUserTwoFactorStore,
        IUserAuthenticationTokenStore,
        IUserAuthenticatorKeyStore,
        IUserTwoFactorRecoveryCodeStore
    {

        protected virtual UserClaim CreateUserClaim(User user, Claim claim)
        {
            var userClaim = new UserClaim { UserId = user.Id };
            userClaim.InitializeFromClaim(claim);
            return userClaim;
        }

        /// <summary>
        /// Called to create a new instance of a <see cref="IdentityUserLogin{string}"/>.
        /// </summary>
        /// <param name="user">The associated user.</param>
        /// <param name="login">The sasociated login.</param>
        /// <returns></returns>
        protected virtual UserLogin CreateUserLogin(User user, UserLoginInfo login)
        {
            return new UserLogin
            {
                UserId = user.Id,
                ProviderKey = login.ProviderKey,
                LoginProvider = login.LoginProvider,
                ProviderDisplayName = login.ProviderDisplayName
            };
        }

        protected virtual UserToken CreateUserToken(User user, string loginProvider, string name, string value)
        {
            return new UserToken
            {
                UserId = user.Id,
                LoginProvider = loginProvider,
                Name = name,
                Value = value
            };
        }

        public virtual Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.Id);
        }

        public virtual Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.UserName);
        }

        public virtual Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.NormalizedUserName);
        }

        public virtual Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }
        public abstract Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken = default);

        /// <summary>
        /// A navigation property for the users the store contains.
        /// </summary>
        public abstract IQueryable<User> Users
        {
            get;
        }

        public virtual Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.PasswordHash);
        }

        public virtual Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return Task.FromResult(user.PasswordHash != null);
        }

        protected abstract Task<User> FindUserAsync(string userId, CancellationToken cancellationToken);

        protected abstract Task<UserLogin> FindUserLoginAsync(string userId, string loginProvider, string providerKey, CancellationToken cancellationToken);

        protected abstract Task<UserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken);

        public abstract Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default);

        public abstract Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);

        public abstract Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default);

        public abstract Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default);

        public abstract Task AddLoginAsync(User user, UserLoginInfo login, CancellationToken cancellationToken = default);

        public abstract Task RemoveLoginAsync(User user, string loginProvider, string providerKey, CancellationToken cancellationToken = default);

        public abstract Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken = default);

        public async virtual Task<User> FindByLoginAsync(string loginProvider, string providerKey,
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

        public virtual Task<bool> GetEmailConfirmedAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.EmailConfirmed);
        }

        public virtual Task SetEmailConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.EmailConfirmed = confirmed;
            return Task.CompletedTask;
        }


        public virtual Task SetEmailAsync(User user, string email, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.Email = email;
            return Task.CompletedTask;
        }
        public virtual Task<string> GetEmailAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.Email);
        }

        public virtual Task<string> GetNormalizedEmailAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.Email);
        }

        public virtual Task SetNormalizedEmailAsync(User user, string normalizedEmail, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.Email = normalizedEmail;
            return Task.CompletedTask;
        }
        public abstract Task<User> FindByPhoneAsync(string normalizedEmail, CancellationToken cancellationToken = default);

        public abstract Task<User> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken = default);

        public virtual Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }
            return Task.FromResult(user.LockoutEnd);
        }

        public virtual Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.LockoutEnd = lockoutEnd;
            return Task.CompletedTask;
        }

        public virtual Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.AccessFailedCount++;
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.AccessFailedCount = 0;
            return Task.CompletedTask;
        }

        public virtual Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.AccessFailedCount);
        }

        public virtual Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.LockoutEnabled);
        }

        public virtual Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.LockoutEnabled = enabled;
            return Task.CompletedTask;
        }
        public virtual Task SetPhoneNumberAsync(User user, string phoneNumber, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.PhoneNumber = phoneNumber;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetPhoneNumberAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.PhoneNumber);
        }

        public virtual Task<bool> GetPhoneNumberConfirmedAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.PhoneNumberConfirmed);
        }

        public virtual Task SetPhoneNumberConfirmedAsync(User user, bool confirmed, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.PhoneNumberConfirmed = confirmed;
            return Task.CompletedTask;
        }

        public virtual Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            stamp.CheakArgument();
            user.SecurityStamp = stamp;
            return Task.CompletedTask;
        }

        public virtual Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.SecurityStamp);
        }

        public virtual Task SetTwoFactorEnabledAsync(User user, bool enabled, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.TwoFactorEnabled = enabled;
            return Task.CompletedTask;
        }

        public virtual Task<bool> GetTwoFactorEnabledAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return Task.FromResult(user.TwoFactorEnabled);
        }

        public abstract Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default);

        protected abstract Task<UserToken> FindTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken);

        protected abstract Task AddUserTokenAsync(UserToken token);

        protected abstract Task RemoveUserTokenAsync(UserToken token);

        public virtual async Task SetTokenAsync(User user, string loginProvider, string name, string value, CancellationToken cancellationToken)
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

        public virtual async Task RemoveTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
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

        public virtual async Task<string> GetTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
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

        public UserStoreBase(IdentityDbContext context, IdentityErrorDescriber errorDescriber, ILoggerFactory loggerFactory) : base(context, errorDescriber, loggerFactory)
        {
        }

        public virtual Task SetAuthenticatorKeyAsync(User user, string key, CancellationToken cancellationToken)
            => SetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, key, cancellationToken);

        public virtual Task<string> GetAuthenticatorKeyAsync(User user, CancellationToken cancellationToken)
            => GetTokenAsync(user, InternalLoginProvider, AuthenticatorKeyTokenName, cancellationToken);

        public virtual async Task<int> CountCodesAsync(User user, CancellationToken cancellationToken)
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

        public virtual Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken)
        {
            var mergedCodes = string.Join(";", recoveryCodes);
            return SetTokenAsync(user, InternalLoginProvider, RecoveryCodeTokenName, mergedCodes, cancellationToken);
        }

        public virtual async Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken)
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

        protected virtual UserRole CreateUserRole(User user, Role role)
        {
            return new UserRole()
            {
                UserId = user.Id,
                RoleId = role.Id
            };
        }
        public abstract Task<IList<User>> GetUsersInRoleAsync(string normalizedRoleName, CancellationToken cancellationToken = default);
        public abstract Task AddToRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken = default);

        public abstract Task RemoveFromRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken = default);

        public abstract Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default);

        public abstract Task<bool> IsInRoleAsync(User user, string normalizedRoleName, CancellationToken cancellationToken = default);
        protected abstract Task<Role> FindRoleAsync(string normalizedRoleName, CancellationToken cancellationToken);

        protected abstract Task<UserRole> FindUserRoleAsync(string userId, string roleId, CancellationToken cancellationToken);
    }
}
