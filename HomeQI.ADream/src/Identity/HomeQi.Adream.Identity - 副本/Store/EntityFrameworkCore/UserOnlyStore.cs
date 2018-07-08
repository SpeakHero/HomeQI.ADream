using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Store;
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

    public class UserOnlyStore : UserStoreBase,
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
        public UserOnlyStore(IdentityDbContext context, IdentityErrorDescriber errorDescriber, ILoggerFactory loggerFactory) : base(context, errorDescriber, loggerFactory)
        {
        }



        /// <summary>
        /// DbSet of users.
        /// </summary>
        protected virtual DbSet<User> UsersSet { get { return Context.Set<User>(); } }

        /// <summary>
        /// DbSet of user claims.
        /// </summary>
        protected DbSet<UserClaim> UserClaims { get { return Context.Set<UserClaim>(); } }

        /// <summary>
        /// DbSet of user logins.
        /// </summary>
        protected virtual DbSet<UserLogin> UserLogins { get { return Context.Set<UserLogin>(); } }

        /// <summary>
        /// DbSet of user tokens.
        /// </summary>
        protected virtual DbSet<UserToken> UserTokens { get { return Context.Set<UserToken>(); } }

        protected virtual DbSet<Role> Roles { get { return Context.Set<Role>(); } }
        protected virtual DbSet<UserRole> UserRoles { get { return Context.Set<UserRole>(); } }

        public override Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return base.CreateAsync(user, cancellationToken);


        }


        public override async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            user.SecurityStamp = Guid.NewGuid().ToString();
            return await base.UpdateAsync(user, cancellationToken);
        }

        public override Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            return base.DeleteAsync(user, cancellationToken);
        }


        public override Task<User> FindByIdAsync(string userId, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            var id = userId;
            return UsersSet.FindAsync(new object[] { id }, cancellationToken);
        }

        public override Task<User> FindByNameAsync(string UserName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            return Users.FirstOrDefaultAsync(u => u.UserName == UserName, cancellationToken);
        }
        public override IQueryable<User> Users => UsersSet;

        protected override Task<User> FindUserAsync(string userId, CancellationToken cancellationToken)
        {
            return Users.SingleOrDefaultAsync(u => u.Id.Equals(userId), cancellationToken);
        }


        protected override Task<UserLogin> FindUserLoginAsync(string userId, string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return UserLogins.SingleOrDefaultAsync(userLogin => userLogin.UserId.Equals(userId) && userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);
        }
        protected override Task<UserLogin> FindUserLoginAsync(string loginProvider, string providerKey, CancellationToken cancellationToken)
        {
            return UserLogins.SingleOrDefaultAsync(userLogin => userLogin.LoginProvider == loginProvider && userLogin.ProviderKey == providerKey, cancellationToken);
        }

        public override async Task<IList<Claim>> GetClaimsAsync(User user, CancellationToken cancellationToken = default)
        {
            ThrowIfDisposed();
            user.CheakArgument();
            return await UserClaims.Where(uc => uc.UserId.Equals(user.Id)).Select(c => c.ToClaim()).ToListAsync();
        }

        public override Task AddClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
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

        public override async Task ReplaceClaimAsync(User user, Claim claim, Claim newClaim, CancellationToken cancellationToken = default)
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
        public override async Task RemoveClaimsAsync(User user, IEnumerable<Claim> claims, CancellationToken cancellationToken = default)
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

        public override Task AddLoginAsync(User user, UserLoginInfo login,
            CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            login.CheakArgument();
            UserLogins.Add(CreateUserLogin(user, login));
            return Task.FromResult(false);
        }

        public override async Task RemoveLoginAsync(User user, string loginProvider, string providerKey,
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

        public async override Task<IList<UserLoginInfo>> GetLoginsAsync(User user, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            var userId = user.Id;
            return await UserLogins.Where(l => l.UserId.Equals(userId))
                .Select(l => new UserLoginInfo(l.LoginProvider, l.ProviderKey, l.ProviderDisplayName)).ToListAsync();
        }

        public override async Task<User> FindByLoginAsync(string loginProvider, string providerKey,
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
        public override Task<User> FindByPhoneAsync(string phoneNumber, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            phoneNumber.CheakArgument();
            return Users.FirstOrDefaultAsync(u => u.PhoneNumber == phoneNumber, cancellationToken);
        }

        public override Task<User> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            email.CheakArgument();
            return Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
        }

        public override async Task<IList<User>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken = default)
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

        protected override Task<UserToken> FindTokenAsync(User user, string loginProvider, string name, CancellationToken cancellationToken)
            => UserTokens.FindAsync(new object[] { user.Id, loginProvider, name }, cancellationToken);

        protected override Task AddUserTokenAsync(UserToken token)
        {
            UserTokens.Add(token);
            return Task.CompletedTask;
        }
        protected override Task RemoveUserTokenAsync(UserToken token)
        {
            UserTokens.Remove(token);
            return Task.CompletedTask;
        }

        public override async Task<IList<User>> GetUsersInRoleAsync(string RoleName, CancellationToken cancellationToken = default)
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
            return new List<User>();
        }

        public override async Task AddToRoleAsync(User user, string RoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                throw new ArgumentException(nameof(RoleName));
            }
            var roleEntity = await FindRoleAsync(RoleName, cancellationToken);
            if (roleEntity == null)
            {
                throw new InvalidOperationException("RoleNotFound");
            }
            UserRoles.Add(CreateUserRole(user, roleEntity));
        }

        public override async Task RemoveFromRoleAsync(User user, string RoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                throw new ArgumentException(nameof(RoleName));
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

        public override async Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken = default)
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

        public override async Task<bool> IsInRoleAsync(User user, string RoleName, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            user.CheakArgument();
            if (string.IsNullOrWhiteSpace(RoleName))
            {
                throw new ArgumentException(nameof(RoleName));
            }
            var role = await FindRoleAsync(RoleName, cancellationToken);
            if (role != null)
            {
                var userRole = await FindUserRoleAsync(user.Id, role.Id, cancellationToken);
                return userRole != null;
            }
            return false;
        }

        protected override Task<Role> FindRoleAsync(string RoleName, CancellationToken cancellationToken)
        {
            return Roles.SingleOrDefaultAsync(r => r.Name == RoleName, cancellationToken);
        }

        protected override Task<UserRole> FindUserRoleAsync(string userId, string roleId, CancellationToken cancellationToken)
        {
            return UserRoles.FindAsync(new object[] { userId, roleId }, cancellationToken);
        }
    }
}
