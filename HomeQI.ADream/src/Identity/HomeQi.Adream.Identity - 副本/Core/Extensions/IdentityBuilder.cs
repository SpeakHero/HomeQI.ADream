using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Manager;
using HomeQI.ADream.Identity.Options;
using HomeQI.ADream.Identity.Store;
using HomeQI.ADream.Identity.Validators;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Reflection;

namespace HomeQI.ADream.Identity.Extensions
{
    public class IdentityBuilder
    {
        /// <summary>
        /// Creates a new instance of <see cref="IdentityBuilder"/>.
        /// </summary>
        /// <param name="user">The <see cref="Type"/> to use for the users.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to attach to.</param>
        public IdentityBuilder(Type user, IServiceCollection services)
        {
            UserType = user;
            Services = services;
        }

        /// <summary>
        /// Creates a new instance of <see cref="IdentityBuilder"/>.
        /// </summary>
        /// <param name="user">The <see cref="Type"/> to use for the users.</param>
        /// <param name="role">The <see cref="Type"/> to use for the roles.</param>
        /// <param name="services">The <see cref="IServiceCollection"/> to attach to.</param>
        public IdentityBuilder(Type user, Type role, IServiceCollection services) : this(user, services)
            => RoleType = role;

        /// <summary>
        /// Gets the <see cref="Type"/> used for users.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> used for users.
        /// </value>
        public Type UserType { get; private set; }


        /// <summary>
        /// Gets the <see cref="Type"/> used for roles.
        /// </summary>
        /// <value>
        /// The <see cref="Type"/> used for roles.
        /// </value>
        public Type RoleType { get; private set; }

        /// <summary>
        /// Gets the <see cref="IServiceCollection"/> services are attached to.
        /// </summary>
        /// <value>
        /// The <see cref="IServiceCollection"/> services are attached to.
        /// </value>
        public IServiceCollection Services { get; private set; }

        private IdentityBuilder AddScoped(Type serviceType, Type concreteType)
        {
            Services.AddScoped(serviceType, concreteType);
            return this;
        }

        /// <summary>
        /// Adds an <see cref="IUserValidator"/> for the <seealso cref="UserType"/>.
        /// </summary>
        /// <typeparam name="TValidator">The user validator type.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public virtual IdentityBuilder AddUserValidator<TValidator>() where TValidator : class
            => AddScoped(typeof(IUserValidator).MakeGenericType(UserType), typeof(TValidator));

        /// <summary>
        /// Adds an <see cref="IUserClaimsPrincipalFactory"/> for the <seealso cref="UserType"/>.
        /// </summary>
        /// <typeparam name="TFactory">The type of the claims principal factory.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public virtual IdentityBuilder AddClaimsPrincipalFactory<TFactory>() where TFactory : class
            => AddScoped(typeof(IUserClaimsPrincipalFactory).MakeGenericType(UserType), typeof(TFactory));

        /// <summary>
        /// Adds an <see cref="IdentityErrorDescriber"/>.
        /// </summary>
        /// <typeparam name="TDescriber">The type of the error describer.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public virtual IdentityBuilder AddErrorDescriber<TDescriber>() where TDescriber : IdentityErrorDescriber
        {
            Services.AddScoped<IdentityErrorDescriber, TDescriber>();
            return this;
        }

        /// <summary>
        /// Adds an <see cref="IPasswordValidator"/> for the <seealso cref="UserType"/>.
        /// </summary>
        /// <typeparam name="TValidator">The validator type used to validate passwords.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public virtual IdentityBuilder AddPasswordValidator<TValidator>() where TValidator : class
            => AddScoped(typeof(IPasswordValidator).MakeGenericType(UserType), typeof(TValidator));

        /// <summary>
        /// Adds an <see cref="IUserStore"/> for the <seealso cref="UserType"/>.
        /// </summary>
        /// <typeparam name="TStore">The user store type.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public virtual IdentityBuilder AddUserStore<TStore>() where TStore : class
            => AddScoped(typeof(IUserStore).MakeGenericType(UserType), typeof(TStore));

        /// <summary>
        /// Adds a token provider.
        /// </summary>
        /// <typeparam name="TProvider">The type of the token provider to add.</typeparam>
        /// <param name="providerName">The name of the provider to add.</param>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public virtual IdentityBuilder AddTokenProvider<TProvider>(string providerName) where TProvider : class
            => AddTokenProvider(providerName, typeof(TProvider));

        /// <summary>
        /// Adds a token provider for the <seealso cref="UserType"/>.
        /// </summary>
        /// <param name="providerName">The name of the provider to add.</param>
        /// <param name="provider">The type of the <see cref="IUserTwoFactorTokenProvider"/> to add.</param>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public virtual IdentityBuilder AddTokenProvider(string providerName, Type provider)
        {
            if (!typeof(IUserTwoFactorTokenProvider).MakeGenericType(UserType).GetTypeInfo().IsAssignableFrom(provider.GetTypeInfo()))
            {
                throw new InvalidOperationException(Resources.FormatInvalidManagerType(provider.Name, "IUserTokenProvider", UserType.Name));
            }
            Services.Configure<IdentityOptions>(options =>
            {
                options.Tokens.ProviderMap[providerName] = new TokenProviderDescriptor(provider);
            });
            Services.AddTransient(provider);
            return this;
        }

        /// <summary>
        /// Adds a <see cref="UserManager"/> for the <seealso cref="UserType"/>.
        /// </summary>
        /// <typeparam name="TUserManager">The type of the user manager to add.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public virtual IdentityBuilder AddUserManager<TUserManager>() where TUserManager : class
        {
            var userManagerType = typeof(IUserManager).MakeGenericType(UserType);
            var customType = typeof(TUserManager);
            if (!userManagerType.GetTypeInfo().IsAssignableFrom(customType.GetTypeInfo()))
            {
                throw new InvalidOperationException(Resources.FormatInvalidManagerType(customType.Name, "UserManager", UserType.Name));
            }
            if (userManagerType != customType)
            {
                Services.AddScoped(customType, services => services.GetRequiredService(userManagerType));
            }
            return AddScoped(userManagerType, customType);
        }

       

        /// <summary>
        /// Adds an <see cref="IRoleValidator"/> for the <seealso cref="RoleType"/>.
        /// </summary>
        /// <typeparam name="TRole">The role validator type.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public virtual IdentityBuilder AddRoleValidator<TRole>() where TRole : class
        {
            if (RoleType == null)
            {
                throw new InvalidOperationException(Resources.NoRoleType);
            }
            return AddScoped(typeof(IRoleValidator).MakeGenericType(RoleType), typeof(TRole));
        }


        /// <summary>
        /// Adds a <see cref="IRoleStore"/> for the <seealso cref="RoleType"/>.
        /// </summary>
        /// <typeparam name="TStore">The role store.</typeparam>
        /// <returns>The current <see cref="IdentityBuilder"/> instance.</returns>
        public virtual IdentityBuilder AddRoleStore<TStore>() where TStore : class
        {
            if (RoleType == null)
            {
                throw new InvalidOperationException(Resources.NoRoleType);
            }
            return AddScoped(typeof(IRoleStore).MakeGenericType(RoleType), typeof(TStore));
        }

    }
}