// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using HomeQI.ADream.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace HomeQI.Adream.Identity.EntityFrameworkCore
{
    /// <summary>
    /// 用于身份的实体框架数据库上下文的基类。
    /// </summary>
    /// <typeparam name="TUser">用户对象的类型。</typeparam>
    public class IdentityUserContext<TUser> : IdentityUserContext<TUser, string> where TUser : IdentityUser
    {
        /// <summary>
        /// Initializes a new instance of <see cref="IdentityUserContext{TUser}"/>.
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public IdentityUserContext(DbContextOptions options) : base(options) { }

    }

    /// <summary>
    /// 用于身份的实体框架数据库上下文的基类。
    /// </summary>
    /// <typeparam name="TUser">The type of user objects.</typeparam>
    /// <typeparam name="TKey">The type of the primary key for users and roles.</typeparam>
    public class IdentityUserContext<TUser, TKey> : IdentityUserContext<TUser, TKey, IdentityUserClaim<TKey>, IdentityUserLogin<TKey>, IdentityUserToken<TKey>, IdentityPermission<TKey>>
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 初始化DB上下文的新实例。
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public IdentityUserContext(DbContextOptions options) : base(options) { }

    }

    /// <summary>
    /// 用于身份的实体框架数据库上下文的基类。
    /// </summary>
    /// <typeparam name="TUser">用户对象的类型。</typeparam>
    /// <typeparam name="TKey">用户和角色的主键类型。</typeparam>
    /// <typeparam name="TUserClaim">用户声明对象的类型.</typeparam>
    /// <typeparam name="TUserLogin">用户登录对象的类型。</typeparam>
    /// <typeparam name="TUserToken">用户令牌对象的类型。</typeparam>
    public abstract class IdentityUserContext<TUser, TKey, TUserClaim, TUserLogin, TUserToken, TPermission> : ADreamDbContext
        where TUser : IdentityUser<TKey>
        where TKey : IEquatable<TKey>
        where TUserClaim : IdentityUserClaim<TKey>
        where TUserLogin : IdentityUserLogin<TKey>
        where TUserToken : IdentityUserToken<TKey>
        where TPermission : IdentityPermission<TKey>
    {
        /// <summary>
        ///初始化类的新实例。
        /// </summary>
        /// <param name="options">The options to be used by a <see cref="DbContext"/>.</param>
        public IdentityUserContext(DbContextOptions options) : base(options) { }
        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of Users.
        /// </summary>
        public DbSet<TUser> Users { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User claims.
        /// </summary>
        public DbSet<TUserClaim> UserClaims { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User logins.
        /// </summary>
        public DbSet<TUserLogin> UserLogins { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DbSet{TEntity}"/> of User tokens.
        /// </summary>
        public DbSet<TUserToken> UserTokens { get; set; }

        private StoreOptions GetStoreOptions() => GetService<IDbContextOptions>()
                            .Extensions.OfType<CoreOptionsExtension>()
                            .FirstOrDefault()?.ApplicationServiceProvider
                            ?.GetService<IOptions<IdentityOptions>>()
                            ?.Value?.Stores;
        public DbSet<TPermission> Permissions { get; set; }

        private class PersonalDataConverter : ValueConverter<string, string>
        {
            public PersonalDataConverter(IPersonalDataProtector protector) : base(s => protector.Protect(s), s => protector.Unprotect(s), default)
            { }
        }

        /// <summary>
        /// 配置标识框架所需的架构。
        /// </summary>
        /// <param name="builder">
        /// 构造器用于构建该上下文的模型。
        /// </param>
        protected override void OnModelCreating(ModelBuilder builder)
        {
            var storeOptions = GetStoreOptions();
            var maxKeyLength = storeOptions?.MaxLengthForKeys ?? 0;
            var encryptPersonalData = storeOptions?.ProtectPersonalData ?? false;
            PersonalDataConverter converter = null;
            builder.Entity<TPermission>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.HasIndex(u => u.Params).HasName("ParamsIndex");
                b.HasIndex(u => u.Controller).HasName("ControllerIndex");
                b.HasIndex(u => u.Action).HasName("ActionIndex");
                b.HasIndex(u => u.AreaName).HasName("AreaNameIndex");

            });
            builder.Entity<TUser>(b =>
        {
            b.HasKey(u => u.Id);
            //b.HasIndex(u => u.NormalizedUserName).HasName("UserNameIndex").IsUnique();
            b.HasIndex(u => u.NormalizedEmail).HasName("NormalizedEmailIndex");
            b.HasIndex(u => u.Email).HasName("EmailIndex");
            b.HasIndex(u => u.PhoneNumber).HasName("PhoneNumberIndex");
            b.ToTable("Users");
            b.Property(u => u.ConcurrencyStamp).IsConcurrencyToken();
            b.Property(u => u.UserName).HasMaxLength(256);
            b.Property(u => u.NormalizedUserName).HasMaxLength(256);
            b.Property(u => u.Email).HasMaxLength(256);
            b.Property(u => u.NormalizedEmail).HasMaxLength(256);

            if (encryptPersonalData)
            {
                converter = new PersonalDataConverter(this.GetService<IPersonalDataProtector>());
                var personalDataProps = typeof(TUser).GetProperties().Where(
                                prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                foreach (var p in personalDataProps)
                {
                    if (p.PropertyType != typeof(string))
                    {
                        throw new InvalidOperationException(Resources.CanOnlyProtectStrings);
                    }
                    b.Property(typeof(string), p.Name).HasConversion(converter);
                }
            }

            b.HasMany<TUserClaim>().WithOne().HasForeignKey(uc => uc.UserId).IsRequired();
            b.HasMany<TUserLogin>().WithOne().HasForeignKey(ul => ul.UserId).IsRequired();
            b.HasMany<TUserToken>().WithOne().HasForeignKey(ut => ut.UserId).IsRequired();
        });
            builder.Entity<TUserClaim>(b =>
            {
                b.HasKey(uc => uc.Id);
                b.HasIndex(u => u.ClaimType).HasName("ClaimTypeIndex");
                b.HasIndex(u => u.ClaimValue).HasName("ClaimValueIndex");
                b.ToTable("UserClaims");
            });

            builder.Entity<TUserLogin>(b =>
            {
                b.HasKey(l => new { l.LoginProvider, l.ProviderKey });

                if (maxKeyLength > 0)
                {
                    b.Property(l => l.LoginProvider).HasMaxLength(maxKeyLength);
                    b.Property(l => l.ProviderKey).HasMaxLength(maxKeyLength);
                }

                b.ToTable("UserLogins");
            });

            builder.Entity<TUserToken>(b =>
            {
                b.HasKey(t => new { t.UserId, t.LoginProvider, t.Name });

                if (maxKeyLength > 0)
                {
                    b.Property(t => t.LoginProvider).HasMaxLength(maxKeyLength);
                    b.Property(t => t.Name).HasMaxLength(maxKeyLength);
                }

                if (encryptPersonalData)
                {
                    var tokenProps = typeof(TUserToken).GetProperties().Where(
                                    prop => Attribute.IsDefined(prop, typeof(ProtectedPersonalDataAttribute)));
                    foreach (var p in tokenProps)
                    {
                        if (p.PropertyType != typeof(string))
                        {
                            throw new InvalidOperationException(Resources.CanOnlyProtectStrings);
                        }
                        b.Property(typeof(string), p.Name).HasConversion(converter);
                    }
                }

                b.ToTable("UserTokens");
            });
        }
    }
}