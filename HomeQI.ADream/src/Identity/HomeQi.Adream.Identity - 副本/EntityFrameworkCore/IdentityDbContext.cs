using HomeQI.ADream.EntityFrameworkCore;
using HomeQI.ADream.Identity.Entites;
using Microsoft.EntityFrameworkCore;

namespace HomeQI.ADream.Identity.EntityFrameworkCore
{
    public class IdentityDbContext : ADreamDbContext
    {
        public IdentityDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }

        public DbSet<UserClaim> UserClaims { get; set; }

        public DbSet<UserLogin> UserLogins { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<RoleClaim> RoleClaims { get; set; }
        public DbSet<UserOrg> UserOrgs { get; set; }
        public DbSet<Organization> Organizations { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            SetGlobalQuery<User>(builder);
            SetGlobalQuery<UserClaim>(builder);
            SetGlobalQuery<RoleClaim>(builder);
            SetGlobalQuery<UserLogin>(builder);
            SetGlobalQuery<UserRole>(builder);
            SetGlobalQuery<UserToken>(builder);
            SetGlobalQuery<Role>(builder);
            SetGlobalQuery<Organization>(builder);
            SetGlobalQuery<UserOrg>(builder);
        }
    }
}
