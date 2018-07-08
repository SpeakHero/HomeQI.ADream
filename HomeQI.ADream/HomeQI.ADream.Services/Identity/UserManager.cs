//using HomeQI.ADream.Models.Entities.Identity;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.Extensions.DependencyInjection;
//using Microsoft.Extensions.Logging;
//using Microsoft.Extensions.Options;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Security.Claims;
//using System.Security.Cryptography;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;

//namespace HomeQI.ADream.Services.Identity
//{
//    public class UserManager : IDisposable
//    {
//        /// <summary>
//        /// The data protection purpose used for the reset password related methods.
//        /// </summary>
//        public const string ResetPasswordTokenPurpose = "ResetPassword";

//        /// <summary>
//        /// The data protection purpose used for the change phone number methods.
//        /// </summary>
//        public const string ChangePhoneNumberTokenPurpose = "ChangePhoneNumber";

//        /// <summary>
//        /// The data protection purpose used for the email confirmation related methods.
//        /// </summary>
//        public const string ConfirmEmailTokenPurpose = "EmailConfirmation";

//        private readonly Dictionary<string, IUserTwoFactorTokenProvider<User>> _tokenProviders =
//            new Dictionary<string, IUserTwoFactorTokenProvider<User>>();

//        private TimeSpan _defaultLockout = TimeSpan.Zero;
//        private bool _disposed;
//        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
//        private IServiceProvider _services;

//        /// <summary>
//        /// The cancellation token used to cancel operations.
//        /// </summary>
//        protected virtual CancellationToken CancellationToken => CancellationToken.None;
//        public UserManager(IUserStore<User> store,
//            IOptions<IdentityOptions> optionsAccessor,
//            IPasswordHasher<User> passwordHasher,
//            IEnumerable<IUserValidator<User>> userValidators,
//            IEnumerable<IPasswordValidator<User>> passwordValidators,
//            ILookupNormalizer keyNormalizer,
//            IdentityErrorDescriber errors,
//            IServiceProvider services,
//            ILogger<UserManager<User>> logger)
//        {
//            Store = store ?? throw new ArgumentNullException(nameof(store));
//            Options = optionsAccessor?.Value ?? new IdentityOptions();
//            PasswordHasher = passwordHasher;
//            KeyNormalizer = keyNormalizer;
//            ErrorDescriber = errors;
//            Logger = logger;

//            if (userValidators != null)
//            {
//                foreach (var v in userValidators)
//                {
//                    UserValidators.Add(v);
//                }
//            }
//            if (passwordValidators != null)
//            {
//                foreach (var v in passwordValidators)
//                {
//                    PasswordValidators.Add(v);
//                }
//            }
//            _services = services;
//            if (services != null)
//            {
//                foreach (var providerName in Options.Tokens.ProviderMap.Keys)
//                {
//                    var description = Options.Tokens.ProviderMap[providerName];

//                    if ((description.ProviderInstance ?? services.GetRequiredService(description.ProviderType)) is IUserTwoFactorTokenProvider<User> provider)
//                    {
//                        RegisterTokenProvider(providerName, provider);
//                    }
//                }
//            }
//        }
//        protected internal IUserStore<User> Store { get; set; }

//        /// <summary>
//        /// The <see cref="ILogger"/> used to log messages from the manager.
//        /// </summary>
//        /// <value>
//        /// The <see cref="ILogger"/> used to log messages from the manager.
//        /// </value>
//        public virtual ILogger Logger { get; set; }
//        public IPasswordHasher<User> PasswordHasher { get; set; }

//        public IList<IUserValidator<User>> UserValidators { get; } = new List<IUserValidator<User>>();
//        public IList<IPasswordValidator<User>> PasswordValidators { get; } = new List<IPasswordValidator<User>>();

//        public ILookupNormalizer KeyNormalizer { get; set; }

//        public IdentityErrorDescriber ErrorDescriber { get; set; }

//        public IdentityOptions Options { get; set; }

//        public virtual bool SupportsUserAuthenticationTokens
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserAuthenticationTokenStore<User>;
//            }
//        }

//        public virtual bool SupportsUserAuthenticatorKey
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserAuthenticatorKeyStore<User>;
//            }
//        }
//        public virtual bool SupportsUserTwoFactorRecoveryCodes
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserTwoFactorRecoveryCodeStore<User>;
//            }
//        }
//        public virtual bool SupportsUserTwoFactor
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserTwoFactorStore<User>;
//            }
//        }
//        /// </value>
//        public virtual bool SupportsUserPassword
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserPasswordStore<User>;
//            }
//        }

//        public virtual bool SupportsUserSecurityStamp
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserSecurityStampStore<User>;
//            }
//        }
//        public virtual bool SupportsUserRole
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserRoleStore<User>;
//            }
//        }

//        public virtual bool SupportsUserLogin
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserLoginStore<User>;
//            }
//        }

//        public virtual bool SupportsUserEmail
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserEmailStore<User>;
//            }
//        }
//        public virtual bool SupportsUserPhoneNumber
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserPhoneNumberStore<User>;
//            }
//        }

//        public virtual bool SupportsUserClaim
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserClaimStore<User>;
//            }
//        }
//        public virtual bool SupportsUserLockout
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IUserLockoutStore<User>;
//            }
//        }

//        public virtual bool SupportsQueryableUsers
//        {
//            get
//            {
//                ThrowIfDisposed();
//                return Store is IQueryableUserStore<User>;
//            }
//        }

//        public virtual IQueryable<User> Users
//        {
//            get
//            {
//                var queryableStore = Store as IQueryableUserStore<User>;
//                if (queryableStore == null)
//                {
//                    throw new NotSupportedException("Resources.StoreNotIQueryableUserStore");
//                }
//                return queryableStore.Users;
//            }
//        }

//        public void Dispose()
//        {
//            Dispose(true);
//            GC.SuppressFinalize(this);
//        }

//        public virtual string GetUserName(ClaimsPrincipal principal)
//        {
//            principal.CheakArgument();
//            return principal.FindFirstValue(Options.ClaimsIdentity.UserNameClaimType);
//        }
//        public virtual string GetUserId(ClaimsPrincipal principal)
//        {
//            principal.CheakArgument();
//            return principal.FindFirstValue(Options.ClaimsIdentity.UserIdClaimType);
//        }

//        public virtual Task<User> GetUserAsync(ClaimsPrincipal principal)
//        {
//            principal.CheakArgument();
//            var id = GetUserId(principal);
//            return id == null ? Task.FromResult<User>(null) : FindByIdAsync(id);
//        }

//        public virtual Task<string> GenerateConcurrencyStampAsync(User user)
//        {
//            return Task.FromResult(Guid.NewGuid().ToString());
//        }

//        public virtual async Task<IdentityResult> CreateAsync(User user)
//        {
//            ThrowIfDisposed();
//            await UpdateSecurityStampInternal(user);
//            var result = await ValidateUserAsync(user);
//            if (!result.Succeeded)
//            {
//                return result;
//            }
//            if (Options.Lockout.AllowedForNewUsers && SupportsUserLockout)
//            {
//                await GetUserLockoutStore().SetLockoutEnabledAsync(user, true, CancellationToken);
//            }
//            await UpdateNormalizedUserNameAsync(user);
//            await UpdateNormalizedEmailAsync(user);

//            return await Store.CreateAsync(user, CancellationToken);
//        }
//        public virtual Task<IdentityResult> UpdateAsync(User user)
//        {
//            ThrowIfDisposed();
//            user.CheakArgument();
//            return UpdateUserAsync(user);
//        }
//        public virtual Task<IdentityResult> DeleteAsync(User user)
//        {
//            ThrowIfDisposed();
//            user.CheakArgument();
//            return Store.DeleteAsync(user, CancellationToken);
//        }
//        public virtual Task<User> FindByIdAsync(string userId)
//        {
//            ThrowIfDisposed();
//            return Store.FindByIdAsync(userId, CancellationToken);
//        }

//        public virtual async Task<User> FindByNameAsync(string userName)
//        {
//            ThrowIfDisposed();
//            userName.CheakArgument();
//            userName = NormalizeKey(userName);

//            var user = await Store.FindByNameAsync(userName, CancellationToken);
//            return user;
//        }
//        public virtual async Task<IdentityResult> CreateAsync(User user, string password)
//        {
//            ThrowIfDisposed();
//            var passwordStore = GetPasswordStore();
//            user.CheakArgument();
//            password.CheakArgument();
//              var result = await UpdatePasswordHash(passwordStore, user, password);
//            if (!result.Succeeded)
//            {
//                return result;
//            }
//            return await CreateAsync(user);
//        }
//        public virtual string NormalizeKey(string key)
//        {
//            return (KeyNormalizer == null) ? key : KeyNormalizer.Normalize(key);
//        }

//        private string ProtectPersonalData(string data)
//        {
//            return data;
//        }
//        public virtual async Task UpdateNormalizedUserNameAsync(User user)
//        {
//            var normalizedName = NormalizeKey(await GetUserNameAsync(user));
//            normalizedName = ProtectPersonalData(normalizedName);
//            await Store.SetNormalizedUserNameAsync(user, normalizedName, CancellationToken);
//        }
//        public virtual async Task<string> GetUserNameAsync(User user)
//        {
//            ThrowIfDisposed();
//            user.CheakArgument();
//            return await Store.GetUserNameAsync(user, CancellationToken);
//        }
//        public virtual async Task<IdentityResult> SetUserNameAsync(User user, string userName)
//        {
//            ThrowIfDisposed();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            await Store.SetUserNameAsync(user, userName, CancellationToken);
//            await UpdateSecurityStampInternal(user);
//            return await UpdateUserAsync(user);
//        }

//        public virtual async Task<string> GetUserIdAsync(User user)
//        {
//            ThrowIfDisposed();
//            return await Store.GetUserIdAsync(user, CancellationToken);
//        }

//        public virtual async Task<bool> CheckPasswordAsync(User user, string password)
//        {
//            ThrowIfDisposed();
//            var passwordStore = GetPasswordStore();
//            user.CheakArgument();
//               var result = await VerifyPasswordAsync(passwordStore, user, password);
//            if (result == PasswordVerificationResult.SuccessRehashNeeded)
//            {
//                await UpdatePasswordHash(passwordStore, user, password, validatePassword: false);
//                await UpdateUserAsync(user);
//            }

//            var success = result != PasswordVerificationResult.Failed;
//            if (!success)
//            {
//                Logger.LogWarning(0, "Invalid password for user {userId}.", await GetUserIdAsync(user));
//            }
//            return success;
//        }
//        public virtual Task<bool> HasPasswordAsync(User user)
//        {
//            ThrowIfDisposed();
//            var passwordStore = GetPasswordStore();
//            user.CheakArgument();
//            return passwordStore.HasPasswordAsync(user, CancellationToken);
//        }
//        public virtual async Task<IdentityResult> AddPasswordAsync(User user, string password)
//        {
//            ThrowIfDisposed();
//            var passwordStore = GetPasswordStore();
//            user.CheakArgument();
//            var hash = await passwordStore.GetPasswordHashAsync(user, CancellationToken);
//            if (hash != null)
//            {
//                Logger.LogWarning(1, "User {userId} already has a password.", await GetUserIdAsync(user));
//                return IdentityResult.Failed(ErrorDescriber.UserAlreadyHasPassword());
//            }
//            var result = await UpdatePasswordHash(passwordStore, user, password);
//            if (!result.Succeeded)
//            {
//                return result;
//            }
//            return await UpdateUserAsync(user);
//        }
//        public virtual async Task<IdentityResult> ChangePasswordAsync(User user, string currentPassword, string newPassword)
//        {
//            ThrowIfDisposed();
//            var passwordStore = GetPasswordStore();
//            user.CheakArgument();
//            if (await VerifyPasswordAsync(passwordStore, user, currentPassword) != PasswordVerificationResult.Failed)
//            {
//                var result = await UpdatePasswordHash(passwordStore, user, newPassword);
//                if (!result.Succeeded)
//                {
//                    return result;
//                }
//                return await UpdateUserAsync(user);
//            }
//            Logger.LogWarning(2, "Change password failed for user {userId}.", await GetUserIdAsync(user));
//            return IdentityResult.Failed(ErrorDescriber.PasswordMismatch());
//        }

//        public virtual async Task<IdentityResult> RemovePasswordAsync(User user)
//        {
//            ThrowIfDisposed();
//            var passwordStore = GetPasswordStore();
//            user.CheakArgument();
//            await UpdatePasswordHash(passwordStore, user, null, validatePassword: false);
//            return await UpdateUserAsync(user);
//        }

//        protected virtual async Task<PasswordVerificationResult> VerifyPasswordAsync(IUserPasswordStore<User> store, User user, string password)
//        {
//            var hash = await store.GetPasswordHashAsync(user, CancellationToken);
//            if (hash == null)
//            {
//                return PasswordVerificationResult.Failed;
//            }
//            return PasswordHasher.VerifyHashedPassword(user, hash, password);
//        }

//        public virtual async Task<string> GetSecurityStampAsync(User user)
//        {
//            ThrowIfDisposed();
//            var securityStore = GetSecurityStore();
//            user.CheakArgument();
//            return await securityStore.GetSecurityStampAsync(user, CancellationToken);
//        }

//        public virtual async Task<IdentityResult> UpdateSecurityStampAsync(User user)
//        {
//            ThrowIfDisposed();
//            GetSecurityStore();
//            user.CheakArgument();
//            await UpdateSecurityStampInternal(user);
//            return await UpdateUserAsync(user);
//        }

//        public virtual Task<string> GeneratePasswordResetTokenAsync(User user)
//        {
//            ThrowIfDisposed();
//            return GenerateUserTokenAsync(user, Options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose);
//        }

//        public virtual async Task<IdentityResult> ResetPasswordAsync(User user, string token, string newPassword)
//        {
//            ThrowIfDisposed();
//            user.CheakArgument();

//            // 确保令牌有效和邮票匹配
//            if (!await VerifyUserTokenAsync(user, Options.Tokens.PasswordResetTokenProvider, ResetPasswordTokenPurpose, token))
//            {
//                return IdentityResult.Failed(ErrorDescriber.InvalidToken());
//            }
//            var result = await UpdatePasswordHash(user, newPassword, validatePassword: true);
//            if (!result.Succeeded)
//            {
//                return result;
//            }
//            return await UpdateUserAsync(user);
//        }

//        public virtual Task<User> FindByLoginAsync(string loginProvider, string providerKey)
//        {
//            ThrowIfDisposed();
//            var loginStore = GetLoginStore();
//            loginProvider.CheakArgument();
//            providerKey.CheakArgument();
//            return loginStore.FindByLoginAsync(loginProvider, providerKey, CancellationToken);
//        }

//        public virtual async Task<IdentityResult> RemoveLoginAsync(User user, string loginProvider, string providerKey)
//        {
//            ThrowIfDisposed();
//            var loginStore = GetLoginStore();
//            loginProvider.CheakArgument();
//            providerKey.CheakArgument();
//            user.CheakArgument();
//            await loginStore.RemoveLoginAsync(user, loginProvider, providerKey, CancellationToken);
//            await UpdateSecurityStampInternal(user);
//            return await UpdateUserAsync(user);
//        }

//        public virtual async Task<IdentityResult> AddLoginAsync(User user, UserLoginInfo login)
//        {
//            ThrowIfDisposed();
//            var loginStore = GetLoginStore();
//            login.CheakArgument();
//            user.CheakArgument();
//            var existingUser = await FindByLoginAsync(login.LoginProvider, login.ProviderKey);
//            if (existingUser != null)
//            {
//                Logger.LogWarning(4, "AddLogin for user {userId} failed because it was already associated with another user.", await GetUserIdAsync(user));
//                return IdentityResult.Failed(ErrorDescriber.LoginAlreadyAssociated());
//            }
//            await loginStore.AddLoginAsync(user, login, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        /// </returns>
//        public virtual async Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
//        {
//            ThrowIfDisposed();
//            var loginStore = GetLoginStore();
//            user.CheakArgument();
//            return await loginStore.GetLoginsAsync(user, CancellationToken);
//        }

//        public virtual Task<IdentityResult> AddClaimAsync(User user, Claim claim)
//        {
//            ThrowIfDisposed();
//            var claimStore = GetClaimStore();
//            user.CheakArgument();
//            claim.CheakArgument();
//            return AddClaimsAsync(user, new Claim[] { claim });
//        }

//        public virtual async Task<IdentityResult> AddClaimsAsync(User user, IEnumerable<Claim> claims)
//        {
//            ThrowIfDisposed();
//            var claimStore = GetClaimStore();
//            user.CheakArgument();
//            claims.CheakArgument();
//            await claimStore.AddClaimsAsync(user, claims, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        public virtual async Task<IdentityResult> ReplaceClaimAsync(User user, Claim claim, Claim newClaim)
//        {
//            ThrowIfDisposed();
//            var claimStore = GetClaimStore();
//            claim.CheakArgument();
//            newClaim.CheakArgument();
//            user.CheakArgument();
//            await claimStore.ReplaceClaimAsync(user, claim, newClaim, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        public virtual Task<IdentityResult> RemoveClaimAsync(User user, Claim claim)
//        {
//            ThrowIfDisposed();
//            var claimStore = GetClaimStore();
//            user.CheakArgument();
//            claim.CheakArgument();
//            return RemoveClaimsAsync(user, new Claim[] { claim });
//        }

//        public virtual async Task<IdentityResult> RemoveClaimsAsync(User user, IEnumerable<Claim> claims)
//        {
//            ThrowIfDisposed();
//            var claimStore = GetClaimStore();
//            user.CheakArgument();
//            claims.CheakArgument();
//            await claimStore.RemoveClaimsAsync(user, claims, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        public virtual async Task<IList<Claim>> GetClaimsAsync(User user)
//        {
//            ThrowIfDisposed();
//            var claimStore = GetClaimStore();
//            user.CheakArgument();
//            return await claimStore.GetClaimsAsync(user, CancellationToken);
//        }
//        public virtual async Task<IdentityResult> AddToRoleAsync(User user, string role)
//        {
//            ThrowIfDisposed();
//            var userRoleStore = GetUserRoleStore();
//            user.CheakArgument();
//            var normalizedRole = NormalizeKey(role);
//            if (await userRoleStore.IsInRoleAsync(user, normalizedRole, CancellationToken))
//            {
//                return await UserAlreadyInRoleError(user, role);
//            }
//            await userRoleStore.AddToRoleAsync(user, normalizedRole, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        public virtual async Task<IdentityResult> AddToRolesAsync(User user, IEnumerable<string> roles)
//        {
//            ThrowIfDisposed();
//            var userRoleStore = GetUserRoleStore();
//            user.CheakArgument();
//            roles.CheakArgument();
//            foreach (var role in roles.Distinct())
//            {
//                var normalizedRole = NormalizeKey(role);
//                if (await userRoleStore.IsInRoleAsync(user, normalizedRole, CancellationToken))
//                {
//                    return await UserAlreadyInRoleError(user, role);
//                }
//                await userRoleStore.AddToRoleAsync(user, normalizedRole, CancellationToken);
//            }
//            return await UpdateUserAsync(user);
//        }

//        public virtual async Task<IdentityResult> RemoveFromRoleAsync(User user, string role)
//        {
//            ThrowIfDisposed();
//            var userRoleStore = GetUserRoleStore();
//            user.CheakArgument();
//            var normalizedRole = NormalizeKey(role);
//            if (!await userRoleStore.IsInRoleAsync(user, normalizedRole, CancellationToken))
//            {
//                return await UserNotInRoleError(user, role);
//            }
//            await userRoleStore.RemoveFromRoleAsync(user, normalizedRole, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        private async Task<IdentityResult> UserAlreadyInRoleError(User user, string role)
//        {
//            Logger.LogWarning(5, "User {userId} is already in role {role}.", await GetUserIdAsync(user), role);
//            return IdentityResult.Failed(ErrorDescriber.UserAlreadyInRole(role));
//        }

//        private async Task<IdentityResult> UserNotInRoleError(User user, string role)
//        {
//            Logger.LogWarning(6, "User {userId} is not in role {role}.", await GetUserIdAsync(user), role);
//            return IdentityResult.Failed(ErrorDescriber.UserNotInRole(role));
//        }

//        public virtual async Task<IdentityResult> RemoveFromRolesAsync(User user, IEnumerable<string> roles)
//        {
//            ThrowIfDisposed();
//            var userRoleStore = GetUserRoleStore();
//            user.CheakArgument();
//            roles.CheakArgument();
//            foreach (var role in roles)
//            {
//                var normalizedRole = NormalizeKey(role);
//                if (!await userRoleStore.IsInRoleAsync(user, normalizedRole, CancellationToken))
//                {
//                    return await UserNotInRoleError(user, role);
//                }
//                await userRoleStore.RemoveFromRoleAsync(user, normalizedRole, CancellationToken);
//            }
//            return await UpdateUserAsync(user);
//        }

//        public virtual async Task<IList<string>> GetRolesAsync(User user)
//        {
//            ThrowIfDisposed();
//            var userRoleStore = GetUserRoleStore();
//            user.CheakArgument();
//            return await userRoleStore.GetRolesAsync(user, CancellationToken);
//        }

//        public virtual async Task<bool> IsInRoleAsync(User user, string role)
//        {
//            ThrowIfDisposed();
//            var userRoleStore = GetUserRoleStore();
//            user.CheakArgument();
//            return await userRoleStore.IsInRoleAsync(user, NormalizeKey(role), CancellationToken);
//        }

//        public virtual async Task<string> GetEmailAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetEmailStore();
//            user.CheakArgument();
//            return await store.GetEmailAsync(user, CancellationToken);
//        }
//        public virtual async Task<IdentityResult> SetEmailAsync(User user, string email)
//        {
//            ThrowIfDisposed();
//            var store = GetEmailStore();
//            user.CheakArgument();
//            await store.SetEmailAsync(user, email, CancellationToken);
//            await store.SetEmailConfirmedAsync(user, false, CancellationToken);
//            await UpdateSecurityStampInternal(user);
//            return await UpdateUserAsync(user);
//        }

//        public virtual async Task<User> FindByEmailAsync(string email)
//        {
//            ThrowIfDisposed();
//            var store = GetEmailStore();
//            email.CheakArgument();
//             email = NormalizeKey(email);
//            var user = await store.FindByEmailAsync(email, CancellationToken);
//            return user;
//        }

//        /// <summary>
//        /// Updates the normalized email for the specified <paramref name="user"/>.
//        /// </summary>
//        /// <param name="user">The user whose email address should be normalized and updated.</param>
//        /// <returns>The task object representing the asynchronous operation.</returns>
//        public virtual async Task UpdateNormalizedEmailAsync(User user)
//        {
//            var store = GetEmailStore(throwOnFail: false);
//            if (store != null)
//            {
//                var email = await GetEmailAsync(user);
//                await store.SetNormalizedEmailAsync(user, ProtectPersonalData(NormalizeKey(email)), CancellationToken);
//            }
//        }

//        /// <summary>
//        /// Generates an email confirmation token for the specified user.
//        /// </summary>
//        /// <param name="user">The user to generate an email confirmation token for.</param>
//        /// <returns>
//        /// The <see cref="Task"/> that represents the asynchronous operation, an email confirmation token.
//        /// </returns>
//        public virtual Task<string> GenerateEmailConfirmationTokenAsync(User user)
//        {
//            ThrowIfDisposed();
//            return GenerateUserTokenAsync(user, Options.Tokens.EmailConfirmationTokenProvider, ConfirmEmailTokenPurpose);
//        }

//        public virtual async Task<IdentityResult> ConfirmEmailAsync(User user, string token)
//        {
//            ThrowIfDisposed();
//            var store = GetEmailStore();
//            user.CheakArgument();
//            if (!await VerifyUserTokenAsync(user, Options.Tokens.EmailConfirmationTokenProvider, ConfirmEmailTokenPurpose, token))
//            {
//                return IdentityResult.Failed(ErrorDescriber.InvalidToken());
//            }
//            await store.SetEmailConfirmedAsync(user, true, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        public virtual async Task<bool> IsEmailConfirmedAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetEmailStore();
//            user.CheakArgument();
//            return await store.GetEmailConfirmedAsync(user, CancellationToken);
//        }
//        public virtual Task<string> GenerateChangeEmailTokenAsync(User user, string newEmail)
//        {
//            ThrowIfDisposed();
//            return GenerateUserTokenAsync(user, Options.Tokens.ChangeEmailTokenProvider, GetChangeEmailTokenPurpose(newEmail));
//        }

//        public virtual async Task<IdentityResult> ChangeEmailAsync(User user, string newEmail, string token)
//        {
//            ThrowIfDisposed();
//            user.CheakArgument();
//            // Make sure the token is valid and the stamp matches
//            if (!await VerifyUserTokenAsync(user, Options.Tokens.ChangeEmailTokenProvider, GetChangeEmailTokenPurpose(newEmail), token))
//            {
//                return IdentityResult.Failed(ErrorDescriber.InvalidToken());
//            }
//            var store = GetEmailStore();
//            await store.SetEmailAsync(user, newEmail, CancellationToken);
//            await store.SetEmailConfirmedAsync(user, true, CancellationToken);
//            await UpdateSecurityStampInternal(user);
//            return await UpdateUserAsync(user);
//        }

//        public virtual async Task<string> GetPhoneNumberAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetPhoneNumberStore();
//            user.CheakArgument();
//            return await store.GetPhoneNumberAsync(user, CancellationToken);
//        }

//        public virtual async Task<IdentityResult> SetPhoneNumberAsync(User user, string phoneNumber)
//        {
//            ThrowIfDisposed();
//            var store = GetPhoneNumberStore();
//            user.CheakArgument();
//            await store.SetPhoneNumberAsync(user, phoneNumber, CancellationToken);
//            await store.SetPhoneNumberConfirmedAsync(user, false, CancellationToken);
//            await UpdateSecurityStampInternal(user);
//            return await UpdateUserAsync(user);
//        }

//        public virtual async Task<IdentityResult> ChangePhoneNumberAsync(User user, string phoneNumber, string token)
//        {
//            ThrowIfDisposed();
//            var store = GetPhoneNumberStore();
//            user.CheakArgument();
//            if (!await VerifyChangePhoneNumberTokenAsync(user, token, phoneNumber))
//            {
//                Logger.LogWarning(7, "Change phone number for user {userId} failed with invalid token.", await GetUserIdAsync(user));
//                return IdentityResult.Failed(ErrorDescriber.InvalidToken());
//            }
//            await store.SetPhoneNumberAsync(user, phoneNumber, CancellationToken);
//            await store.SetPhoneNumberConfirmedAsync(user, true, CancellationToken);
//            await UpdateSecurityStampInternal(user);
//            return await UpdateUserAsync(user);
//        }

//        public virtual Task<bool> IsPhoneNumberConfirmedAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetPhoneNumberStore();
//            user.CheakArgument();
//            return store.GetPhoneNumberConfirmedAsync(user, CancellationToken);
//        }

//        public virtual Task<string> GenerateChangePhoneNumberTokenAsync(User user, string phoneNumber)
//        {
//            ThrowIfDisposed();
//            return GenerateUserTokenAsync(user, Options.Tokens.ChangePhoneNumberTokenProvider, ChangePhoneNumberTokenPurpose + ":" + phoneNumber);
//        }

//        public virtual Task<bool> VerifyChangePhoneNumberTokenAsync(User user, string token, string phoneNumber)
//        {
//            ThrowIfDisposed();
//            user.CheakArgument();
//            // Make sure the token is valid and the stamp matches
//            return VerifyUserTokenAsync(user, Options.Tokens.ChangePhoneNumberTokenProvider, ChangePhoneNumberTokenPurpose + ":" + phoneNumber, token);
//        }

//        public virtual async Task<bool> VerifyUserTokenAsync(User user, string tokenProvider, string purpose, string token)
//        {
//            ThrowIfDisposed();
//            user.CheakArgument();
//            tokenProvider.CheakArgument();
//            if (!_tokenProviders.ContainsKey(tokenProvider))
//            {
//                throw new NotSupportedException("Resources.FormatNoTokenProvider(nameof(User), tokenProvider)");
//            }
//            // Make sure the token is valid
//            var result = await _tokenProviders[tokenProvider].ValidateAsync(purpose, token, this, user);

//            if (!result)
//            {
//                Logger.LogWarning(9, "VerifyUserTokenAsync() failed with purpose: {purpose} for user {userId}.", purpose, await GetUserIdAsync(user));
//            }
//            return result;
//        }

//        /// <summary>
//        /// Generates a token for the given <paramref name="user"/> and <paramref name="purpose"/>.
//        /// </summary>
//        /// <param name="purpose">The purpose the token will be for.</param>
//        /// <param name="user">The user the token will be for.</param>
//        /// <param name="tokenProvider">The provider which will generate the token.</param>
//        /// <returns>
//        /// The <see cref="Task"/> that represents result of the asynchronous operation, a token for
//        /// the given user and purpose.
//        /// </returns>
//        public virtual Task<string> GenerateUserTokenAsync(User user, string tokenProvider, string purpose)
//        {
//            ThrowIfDisposed();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            if (tokenProvider == null)
//            {
//                throw new ArgumentNullException(nameof(tokenProvider));
//            }
//            if (!_tokenProviders.ContainsKey(tokenProvider))
//            {
//                throw new NotSupportedException("Resources.FormatNoTokenProvider(nameof(User), tokenProvider)");
//            }

//            return _tokenProviders[tokenProvider].GenerateAsync(purpose, this, user);
//        }

//        /// <summary>
//        /// Registers a token provider.
//        /// </summary>
//        /// <param name="providerName">The name of the provider to register.</param>
//        /// <param name="provider">The provider to register.</param>
//        public virtual void RegisterTokenProvider(string providerName, IUserTwoFactorTokenProvider<User> provider)
//        {
//            ThrowIfDisposed();
//            if (provider == null)
//            {
//                throw new ArgumentNullException(nameof(provider));
//            }
//            _tokenProviders[providerName] = provider;
//        }

//        /// <summary>
//        /// Gets a list of valid two factor token providers for the specified <paramref name="user"/>,
//        /// as an asynchronous operation.
//        /// </summary>
//        /// <param name="user">The user the whose two factor authentication providers will be returned.</param>
//        /// <returns>
//        /// The <see cref="Task"/> that represents result of the asynchronous operation, a list of two
//        /// factor authentication providers for the specified user.
//        /// </returns>
//        public virtual async Task<IList<string>> GetValidTwoFactorProvidersAsync(User user)
//        {
//            ThrowIfDisposed();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            var results = new List<string>();
//            foreach (var f in _tokenProviders)
//            {
//                if (await f.Value.CanGenerateTwoFactorTokenAsync(this, user))
//                {
//                    results.Add(f.Key);
//                }
//            }
//            return results;
//        }

//        /// <summary>
//        /// Verifies the specified two factor authentication <paramref name="token" /> against the <paramref name="user"/>.
//        /// </summary>
//        /// <param name="user">The user the token is supposed to be for.</param>
//        /// <param name="tokenProvider">The provider which will verify the token.</param>
//        /// <param name="token">The token to verify.</param>
//        /// <returns>
//        /// The <see cref="Task"/> that represents result of the asynchronous operation, true if the token is valid,
//        /// otherwise false.
//        /// </returns>
//        public virtual async Task<bool> VerifyTwoFactorTokenAsync(User user, string tokenProvider, string token)
//        {
//            ThrowIfDisposed();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            if (!_tokenProviders.ContainsKey(tokenProvider))
//            {
//                throw new NotSupportedException("Resources.FormatNoTokenProvider(nameof(User), tokenProvider)");
//            }

//            // Make sure the token is valid
//            var result = await _tokenProviders[tokenProvider].ValidateAsync("TwoFactor", token, this, user);
//            if (!result)
//            {
//                Logger.LogWarning(10, $"{nameof(VerifyTwoFactorTokenAsync)}() failed for user {await GetUserIdAsync(user)}.");
//            }
//            return result;
//        }

//        /// <summary>
//        /// Gets a two factor authentication token for the specified <paramref name="user"/>.
//        /// </summary>
//        /// <param name="user">The user the token is for.</param>
//        /// <param name="tokenProvider">The provider which will generate the token.</param>
//        /// <returns>
//        /// The <see cref="Task"/> that represents result of the asynchronous operation, a two factor authentication token
//        /// for the user.
//        /// </returns>
//        public virtual Task<string> GenerateTwoFactorTokenAsync(User user, string tokenProvider)
//        {
//            ThrowIfDisposed();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            if (!_tokenProviders.ContainsKey(tokenProvider))
//            {
//                throw new NotSupportedException("Resources.FormatNoTokenProvider(nameof(User), tokenProvider)");
//            }

//            return _tokenProviders[tokenProvider].GenerateAsync("TwoFactor", this, user);
//        }

//        /// <summary>
//        /// Returns a flag indicating whether the specified <paramref name="user"/> has two factor authentication enabled or not,
//        /// as an asynchronous operation.
//        /// </summary>
//        /// <param name="user">The user whose two factor authentication enabled status should be retrieved.</param>
//        /// <returns>
//        /// The <see cref="Task"/> that represents the asynchronous operation, true if the specified <paramref name="user "/>
//        /// has two factor authentication enabled, otherwise false.
//        /// </returns>
//        public virtual async Task<bool> GetTwoFactorEnabledAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetUserTwoFactorStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            return await store.GetTwoFactorEnabledAsync(user, CancellationToken);
//        }

//        /// <summary>
//        /// Sets a flag indicating whether the specified <paramref name="user"/> has two factor authentication enabled or not,
//        /// as an asynchronous operation.
//        /// </summary>
//        /// <param name="user">The user whose two factor authentication enabled status should be set.</param>
//        /// <param name="enabled">A flag indicating whether the specified <paramref name="user"/> has two factor authentication enabled.</param>
//        /// <returns>
//        /// The <see cref="Task"/> that represents the asynchronous operation, the <see cref="IdentityResult"/> of the operation
//        /// </returns>
//        public virtual async Task<IdentityResult> SetTwoFactorEnabledAsync(User user, bool enabled)
//        {
//            ThrowIfDisposed();
//            var store = GetUserTwoFactorStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            await store.SetTwoFactorEnabledAsync(user, enabled, CancellationToken);
//            await UpdateSecurityStampInternal(user);
//            return await UpdateUserAsync(user);
//        }

//        /// <summary>
//        /// Returns a flag indicating whether the specified <paramref name="user"/> his locked out,
//        /// as an asynchronous operation.
//        /// </summary>
//        /// <param name="user">The user whose locked out status should be retrieved.</param>
//        /// <returns>
//        /// The <see cref="Task"/> that represents the asynchronous operation, true if the specified <paramref name="user "/>
//        /// is locked out, otherwise false.
//        /// </returns>
//        public virtual async Task<bool> IsLockedOutAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetUserLockoutStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            if (!await store.GetLockoutEnabledAsync(user, CancellationToken))
//            {
//                return false;
//            }
//            var lockoutTime = await store.GetLockoutEndDateAsync(user, CancellationToken);
//            return lockoutTime >= DateTimeOffset.UtcNow;
//        }

//        /// <summary>
//        /// Sets a flag indicating whether the specified <paramref name="user"/> is locked out,
//        /// as an asynchronous operation.
//        /// </summary>
//        /// <param name="user">The user whose locked out status should be set.</param>
//        /// <param name="enabled">Flag indicating whether the user is locked out or not.</param>
//        /// <returns>
//        /// The <see cref="Task"/> that represents the asynchronous operation, the <see cref="IdentityResult"/> of the operation
//        /// </returns>
//        public virtual async Task<IdentityResult> SetLockoutEnabledAsync(User user, bool enabled)
//        {
//            ThrowIfDisposed();
//            var store = GetUserLockoutStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            await store.SetLockoutEnabledAsync(user, enabled, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        /// <summary>
//        /// Retrieves a flag indicating whether user lockout can enabled for the specified user.
//        /// </summary>
//        /// <param name="user">The user whose ability to be locked out should be returned.</param>
//        /// <returns>
//        /// The <see cref="Task"/> that represents the asynchronous operation, true if a user can be locked out, otherwise false.
//        /// </returns>
//        public virtual async Task<bool> GetLockoutEnabledAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetUserLockoutStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            return await store.GetLockoutEnabledAsync(user, CancellationToken);
//        }

//        /// <summary>
//        /// Gets the last <see cref="DateTimeOffset"/> a user's last lockout expired, if any.
//        /// Any time in the past should be indicates a user is not locked out.
//        /// </summary>
//        /// <param name="user">The user whose lockout date should be retrieved.</param>
//        /// <returns>
//        /// A <see cref="Task{TResult}"/> that represents the lookup, a <see cref="DateTimeOffset"/> containing the last time a user's lockout expired, if any.
//        /// </returns>
//        public virtual async Task<DateTimeOffset?> GetLockoutEndDateAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetUserLockoutStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            return await store.GetLockoutEndDateAsync(user, CancellationToken);
//        }

//        /// <summary>
//        /// Locks out a user until the specified end date has passed. Setting a end date in the past immediately unlocks a user.
//        /// </summary>
//        /// <param name="user">The user whose lockout date should be set.</param>
//        /// <param name="lockoutEnd">The <see cref="DateTimeOffset"/> after which the <paramref name="user"/>'s lockout should end.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
//        public virtual async Task<IdentityResult> SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd)
//        {
//            ThrowIfDisposed();
//            var store = GetUserLockoutStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            if (!await store.GetLockoutEnabledAsync(user, CancellationToken))
//            {
//                Logger.LogWarning(11, "Lockout for user {userId} failed because lockout is not enabled for this user.", await GetUserIdAsync(user));
//                return IdentityResult.Failed(ErrorDescriber.UserLockoutNotEnabled());
//            }
//            await store.SetLockoutEndDateAsync(user, lockoutEnd, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        /// <summary>
//        /// Increments the access failed count for the user as an asynchronous operation.
//        /// If the failed access account is greater than or equal to the configured maximum number of attempts,
//        /// the user will be locked out for the configured lockout time span.
//        /// </summary>
//        /// <param name="user">The user whose failed access count to increment.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
//        public virtual async Task<IdentityResult> AccessFailedAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetUserLockoutStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            // If this puts the user over the threshold for lockout, lock them out and reset the access failed count
//            var count = await store.IncrementAccessFailedCountAsync(user, CancellationToken);
//            if (count < Options.Lockout.MaxFailedAccessAttempts)
//            {
//                return await UpdateUserAsync(user);
//            }
//            Logger.LogWarning(12, "User {userId} is locked out.", await GetUserIdAsync(user));
//            await store.SetLockoutEndDateAsync(user, DateTimeOffset.UtcNow.Add(Options.Lockout.DefaultLockoutTimeSpan),
//                CancellationToken);
//            await store.ResetAccessFailedCountAsync(user, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        /// <summary>
//        /// Resets the access failed count for the specified <paramref name="user"/>.
//        /// </summary>
//        /// <param name="user">The user whose failed access count should be reset.</param>
//        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the operation.</returns>
//        public virtual async Task<IdentityResult> ResetAccessFailedCountAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetUserLockoutStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            if (await GetAccessFailedCountAsync(user) == 0)
//            {
//                return IdentityResult.Success;
//            }
//            await store.ResetAccessFailedCountAsync(user, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        /// <summary>
//        /// Retrieves the current number of failed accesses for the given <paramref name="user"/>.
//        /// </summary>
//        /// <param name="user">The user whose access failed count should be retrieved for.</param>
//        /// <returns>The <see cref="Task"/> that contains the result the asynchronous operation, the current failed access count
//        /// for the user.</returns>
//        public virtual async Task<int> GetAccessFailedCountAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetUserLockoutStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            return await store.GetAccessFailedCountAsync(user, CancellationToken);
//        }

//        /// <summary>
//        /// Returns a list of users from the user store who have the specified <paramref name="claim"/>.
//        /// </summary>
//        /// <param name="claim">The claim to look for.</param>
//        /// <returns>
//        /// A <see cref="Task{TResult}"/> that represents the result of the asynchronous query, a list of <typeparamref name="User"/>s who
//        /// have the specified claim.
//        /// </returns>
//        public virtual Task<IList<User>> GetUsersForClaimAsync(Claim claim)
//        {
//            ThrowIfDisposed();
//            var store = GetClaimStore();
//            if (claim == null)
//            {
//                throw new ArgumentNullException(nameof(claim));
//            }
//            return store.GetUsersForClaimAsync(claim, CancellationToken);
//        }

//        /// <summary>
//        /// Returns a list of users from the user store who are members of the specified <paramref name="roleName"/>.
//        /// </summary>
//        /// <param name="roleName">The name of the role whose users should be returned.</param>
//        /// <returns>
//        /// A <see cref="Task{TResult}"/> that represents the result of the asynchronous query, a list of <typeparamref name="User"/>s who
//        /// are members of the specified role.
//        /// </returns>
//        public virtual Task<IList<User>> GetUsersInRoleAsync(string roleName)
//        {
//            ThrowIfDisposed();
//            var store = GetUserRoleStore();
//            if (roleName == null)
//            {
//                throw new ArgumentNullException(nameof(roleName));
//            }

//            return store.GetUsersInRoleAsync(NormalizeKey(roleName), CancellationToken);
//        }

//        /// <summary>
//        /// Returns an authentication token for a user.
//        /// </summary>
//        /// <param name="user"></param>
//        /// <param name="loginProvider">The authentication scheme for the provider the token is associated with.</param>
//        /// <param name="tokenName">The name of the token.</param>
//        /// <returns>The authentication token for a user</returns>
//        public virtual Task<string> GetAuthenticationTokenAsync(User user, string loginProvider, string tokenName)
//        {
//            ThrowIfDisposed();
//            var store = GetAuthenticationTokenStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            if (loginProvider == null)
//            {
//                throw new ArgumentNullException(nameof(loginProvider));
//            }
//            if (tokenName == null)
//            {
//                throw new ArgumentNullException(nameof(tokenName));
//            }

//            return store.GetTokenAsync(user, loginProvider, tokenName, CancellationToken);
//        }

//        /// <summary>
//        /// Sets an authentication token for a user.
//        /// </summary>
//        /// <param name="user"></param>
//        /// <param name="loginProvider">The authentication scheme for the provider the token is associated with.</param>
//        /// <param name="tokenName">The name of the token.</param>
//        /// <param name="tokenValue">The value of the token.</param>
//        /// <returns>Whether the user was successfully updated.</returns>
//        public virtual async Task<IdentityResult> SetAuthenticationTokenAsync(User user, string loginProvider, string tokenName, string tokenValue)
//        {
//            ThrowIfDisposed();
//            var store = GetAuthenticationTokenStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            if (loginProvider == null)
//            {
//                throw new ArgumentNullException(nameof(loginProvider));
//            }
//            if (tokenName == null)
//            {
//                throw new ArgumentNullException(nameof(tokenName));
//            }

//            // REVIEW: should updating any tokens affect the security stamp?
//            await store.SetTokenAsync(user, loginProvider, tokenName, tokenValue, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        /// <summary>
//        /// Remove an authentication token for a user.
//        /// </summary>
//        /// <param name="user"></param>
//        /// <param name="loginProvider">The authentication scheme for the provider the token is associated with.</param>
//        /// <param name="tokenName">The name of the token.</param>
//        /// <returns>Whether a token was removed.</returns>
//        public virtual async Task<IdentityResult> RemoveAuthenticationTokenAsync(User user, string loginProvider, string tokenName)
//        {
//            ThrowIfDisposed();
//            var store = GetAuthenticationTokenStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            if (loginProvider == null)
//            {
//                throw new ArgumentNullException(nameof(loginProvider));
//            }
//            if (tokenName == null)
//            {
//                throw new ArgumentNullException(nameof(tokenName));
//            }

//            await store.RemoveTokenAsync(user, loginProvider, tokenName, CancellationToken);
//            return await UpdateUserAsync(user);
//        }

//        /// <summary>
//        /// Returns the authenticator key for the user.
//        /// </summary>
//        /// <param name="user">The user.</param>
//        /// <returns>The authenticator key</returns>
//        public virtual Task<string> GetAuthenticatorKeyAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetAuthenticatorKeyStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            return store.GetAuthenticatorKeyAsync(user, CancellationToken);
//        }

//        /// <summary>
//        /// Resets the authenticator key for the user.
//        /// </summary>
//        /// <param name="user">The user.</param>
//        /// <returns>Whether the user was successfully updated.</returns>
//        public virtual async Task<IdentityResult> ResetAuthenticatorKeyAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetAuthenticatorKeyStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }
//            await store.SetAuthenticatorKeyAsync(user, GenerateNewAuthenticatorKey(), CancellationToken);
//            await UpdateSecurityStampInternal(user);
//            return await UpdateAsync(user);
//        }

//        /// <summary>
//        /// Generates a new base32 encoded 160-bit security secret (size of SHA1 hash).
//        /// </summary>
//        /// <returns>The new security secret.</returns>
//        public virtual string GenerateNewAuthenticatorKey()
//            => NewSecurityStamp();

//        /// <summary>
//        /// Generates recovery codes for the user, this invalidates any previous recovery codes for the user.
//        /// </summary>
//        /// <param name="user">The user to generate recovery codes for.</param>
//        /// <param name="number">The number of codes to generate.</param>
//        /// <returns>The new recovery codes for the user.  Note: there may be less than number returned, as duplicates will be removed.</returns>
//        public virtual async Task<IEnumerable<string>> GenerateNewTwoFactorRecoveryCodesAsync(User user, int number)
//        {
//            ThrowIfDisposed();
//            var store = GetRecoveryCodeStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            var newCodes = new List<string>(number);
//            for (var i = 0; i < number; i++)
//            {
//                newCodes.Add(CreateTwoFactorRecoveryCode());
//            }

//            await store.ReplaceCodesAsync(user, newCodes.Distinct(), CancellationToken);
//            var update = await UpdateAsync(user);
//            if (update.Succeeded)
//            {
//                return newCodes;
//            }
//            return null;
//        }

//        /// <summary>
//        /// Generate a new recovery code.
//        /// </summary>
//        /// <returns></returns>
//        protected virtual string CreateTwoFactorRecoveryCode()
//            => Guid.NewGuid().ToString().Substring(0, 8);

//        /// <summary>
//        /// Returns whether a recovery code is valid for a user. Note: recovery codes are only valid
//        /// once, and will be invalid after use.
//        /// </summary>
//        /// <param name="user">The user who owns the recovery code.</param>
//        /// <param name="code">The recovery code to use.</param>
//        /// <returns>True if the recovery code was found for the user.</returns>
//        public virtual async Task<IdentityResult> RedeemTwoFactorRecoveryCodeAsync(User user, string code)
//        {
//            ThrowIfDisposed();
//            var store = GetRecoveryCodeStore();
//            if (user == null)
//            {
//                throw new ArgumentNullException(nameof(user));
//            }

//            var success = await store.RedeemCodeAsync(user, code, CancellationToken);
//            if (success)
//            {
//                return await UpdateAsync(user);
//            }
//            return IdentityResult.Failed(ErrorDescriber.RecoveryCodeRedemptionFailed());
//        }

//        /// <summary>
//        /// Returns how many recovery code are still valid for a user.
//        /// </summary>
//        /// <param name="user">The user.</param>
//        /// <returns>How many recovery code are still valid for a user.</returns>
//        public virtual Task<int> CountRecoveryCodesAsync(User user)
//        {
//            ThrowIfDisposed();
//            var store = GetRecoveryCodeStore();
//            user.CheakArgument();
//            return store.CountCodesAsync(user, CancellationToken);
//        }
//        protected virtual void Dispose(bool disposing)
//        {
//            if (disposing && !_disposed)
//            {
//                Store.Dispose();
//                _disposed = true;
//            }
//        }

//        private IUserTwoFactorStore<User> GetUserTwoFactorStore()
//        {
//            if (!(Store is IUserTwoFactorStore<User> cast))
//            {
//                throw new NotSupportedException("Resources.StoreNotIUserTwoFactorStore");
//            }
//            return cast;
//        }

//        private IUserLockoutStore<User> GetUserLockoutStore()
//        {
//            if (!(Store is IUserLockoutStore<User> cast))
//            {
//                throw new NotSupportedException("Resources.StoreNotIUserLockoutStore");
//            }
//            return cast;
//        }

//        private IUserEmailStore<User> GetEmailStore(bool throwOnFail = true)
//        {
//            if (throwOnFail && Store as IUserEmailStore<User> == null)
//            {
//                throw new NotSupportedException("Resources.StoreNotIUserEmailStore");
//            }
//            return Store as IUserEmailStore<User>;
//        }

//        private IUserPhoneNumberStore<User> GetPhoneNumberStore()
//        {
//            if (!(Store is IUserPhoneNumberStore<User> cast))
//            {
//                throw new NotSupportedException("Resources.StoreNotIUserPhoneNumberStore");
//            }
//            return cast;
//        }
//        public virtual async Task<byte[]> CreateSecurityTokenAsync(User user)
//        {
//            return Encoding.Unicode.GetBytes(await GetSecurityStampAsync(user));
//        }
//        private async Task UpdateSecurityStampInternal(User user)
//        {
//            if (SupportsUserSecurityStamp)
//            {
//                await GetSecurityStore().SetSecurityStampAsync(user, NewSecurityStamp(), CancellationToken);
//            }
//        }

//        protected virtual Task<IdentityResult> UpdatePasswordHash(User user, string newPassword, bool validatePassword)
//            => UpdatePasswordHash(GetPasswordStore(), user, newPassword, validatePassword);

//        private async Task<IdentityResult> UpdatePasswordHash(IUserPasswordStore<User> passwordStore,
//            User user, string newPassword, bool validatePassword = true)
//        {
//            if (validatePassword)
//            {
//                var validate = await ValidatePasswordAsync(user, newPassword);
//                if (!validate.Succeeded)
//                {
//                    return validate;
//                }
//            }
//            var hash = newPassword != null ? PasswordHasher.HashPassword(user, newPassword) : null;
//            await passwordStore.SetPasswordHashAsync(user, hash, CancellationToken);
//            await UpdateSecurityStampInternal(user);
//            return IdentityResult.Success;
//        }

//        private IUserRoleStore<User> GetUserRoleStore()
//        {
//            if (!(Store is IUserRoleStore<User> cast))
//            {
//                throw new NotSupportedException("Resources.StoreNotIUserRoleStore");
//            }
//            return cast;
//        }

//        private static string NewSecurityStamp()
//        {
//            byte[] bytes = new byte[20];
//            _rng.GetBytes(bytes);
//            return Base32.ToBase32(bytes);
//        }

//        // IUserLoginStore methods
//        private IUserLoginStore<User> GetLoginStore()
//        {
//            if (!(Store is IUserLoginStore<User> cast))
//            {
//                throw new NotSupportedException("Resources.StoreNotIUserLoginStore");
//            }
//            return cast;
//        }

//        private IUserSecurityStampStore<User> GetSecurityStore()
//        {
//            if (!(Store is IUserSecurityStampStore<User> cast))
//            {
//                throw new NotSupportedException("Resources.StoreNotIUserSecurityStampStore");
//            }
//            return cast;
//        }

//        private IUserClaimStore<User> GetClaimStore()
//        {
//            if (!(Store is IUserClaimStore<User> cast))
//            {
//                throw new NotSupportedException("Resources.StoreNotIUserClaimStore");
//            }
//            return cast;
//        }

//        protected static string GetChangeEmailTokenPurpose(string newEmail)
//        {
//            return "ChangeEmail:" + newEmail;
//        }

//        protected async Task<IdentityResult> ValidateUserAsync(User user)
//        {
//            if (SupportsUserSecurityStamp)
//            {
//                if (await GetSecurityStampAsync(user) == null)
//                {
//                    throw new InvalidOperationException("Resources.NullSecurityStamp");
//                }
//            }
//            var errors = new List<IdentityError>();
//            foreach (var v in UserValidators)
//            {
//                var result = await v.ValidateAsync(new UserManager<User>, user);
//                if (!result.Succeeded)
//                {
//                    errors.AddRange(result.Errors);
//                }
//            }
//            if (errors.Count > 0)
//            {
//                Logger.LogWarning(13, "User {userId} validation failed: {errors}.", await GetUserIdAsync(user), string.Join(";", errors.Select(e => e.Code)));
//                return IdentityResult.Failed(errors.ToArray());
//            }
//            return IdentityResult.Success;
//        }

//        /// <summary>
//        /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
//        /// called before updating the password hash.
//        /// </summary>
//        /// <param name="user">The user.</param>
//        /// <param name="password">The password.</param>
//        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
//        protected async Task<IdentityResult> ValidatePasswordAsync(User user, string password)
//        {
//            var errors = new List<IdentityError>();
//            foreach (var v in PasswordValidators)
//            {
//                var result = await v.ValidateAsync(this, user, password);
//                if (!result.Succeeded)
//                {
//                    errors.AddRange(result.Errors);
//                }
//            }
//            if (errors.Count > 0)
//            {
//                Logger.LogWarning(14, "User {userId} password validation failed: {errors}.", await GetUserIdAsync(user), string.Join(";", errors.Select(e => e.Code)));
//                return IdentityResult.Failed(errors.ToArray());
//            }
//            return IdentityResult.Success;
//        }

//        /// <summary>
//        /// Called to update the user after validating and updating the normalized email/user name.
//        /// </summary>
//        /// <param name="user">The user.</param>
//        /// <returns>Whether the operation was successful.</returns>
//        protected virtual async Task<IdentityResult> UpdateUserAsync(User user)
//        {
//            var result = await ValidateUserAsync(user);
//            if (!result.Succeeded)
//            {
//                return result;
//            }
//            await UpdateNormalizedUserNameAsync(user);
//            await UpdateNormalizedEmailAsync(user);
//            return await Store.UpdateAsync(user, CancellationToken);
//        }

//        private IUserAuthenticatorKeyStore<User> GetAuthenticatorKeyStore()
//        {
//            var cast = Store as IUserAuthenticatorKeyStore<User>;
//            if (cast == null)
//            {
//                throw new NotSupportedException(Resources.StoreNotIUserAuthenticatorKeyStore);
//            }
//            return cast;
//        }

//        private IUserTwoFactorRecoveryCodeStore<User> GetRecoveryCodeStore()
//        {
//            var cast = Store as IUserTwoFactorRecoveryCodeStore<User>;
//            if (cast == null)
//            {
//                throw new NotSupportedException(Resources.StoreNotIUserTwoFactorRecoveryCodeStore);
//            }
//            return cast;
//        }

//        private IUserAuthenticationTokenStore<User> GetAuthenticationTokenStore()
//        {
//            var cast = Store as IUserAuthenticationTokenStore<User>;
//            if (cast == null)
//            {
//                throw new NotSupportedException(Resources.StoreNotIUserAuthenticationTokenStore);
//            }
//            return cast;
//        }

//        private IUserPasswordStore<User> GetPasswordStore()
//        {
//            var cast = Store as IUserPasswordStore<User>;
//            if (cast == null)
//            {
//                throw new NotSupportedException(Resources.StoreNotIUserPasswordStore);
//            }
//            return cast;
//        }

//        /// <summary>
//        /// Throws if this class has been disposed.
//        /// </summary>
//        protected void ThrowIfDisposed()
//        {
//            if (_disposed)
//            {
//                throw new ObjectDisposedException(GetType().Name);
//            }
//        }

//    }
//}
