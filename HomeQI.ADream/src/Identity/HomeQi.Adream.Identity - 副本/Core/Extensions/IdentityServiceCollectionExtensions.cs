using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Core.Authorize;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Manager;
using HomeQI.ADream.Identity.Options;
using HomeQI.ADream.Identity.Validators;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;

namespace HomeQI.ADream.Identity.Extensions
{
    public static class IdentityServiceCollectionExtensions
    {
        public static IdentityBuilder AddIdentity(
               this IServiceCollection services) => AddIdentity(services, null);

        public static IdentityBuilder AddIdentity(
            this IServiceCollection services,
            Action<IdentityOptions> setupAction)

        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies 
                // is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            // 身份使用的服务
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddCookie(IdentityConstants.ApplicationScheme, o =>
            {
                o.LoginPath = new PathString("/Account/Login");
                o.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = SecurityStampValidator.ValidatePrincipalAsync
                };
            })
            .AddCookie(IdentityConstants.ExternalScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.ExternalScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            })
            .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
                o.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = SecurityStampValidator.ValidateAsync<ITwoFactorSecurityStampValidator>
                };
            })
            .AddCookie(IdentityConstants.TwoFactorUserIdScheme, o =>
            {
                o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5);
            });
            services.AddAuthorization();
            // Hosting doesn't add IHttpContextAccessor by default
            services.AddHttpContextAccessor();
            // Identity services
            services.TryAddScoped<IUserValidator, UserValidator>();
            services.TryAddScoped<IPasswordValidator, PasswordValidator>();
            services.TryAddScoped<IPasswordHasher, PasswordHasher>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator, RoleValidator>();
            // 没有错误描述器的接口，所以我们可以在不修改接口的情况下添加错误。
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<User>>();
            services.TryAddScoped<ITwoFactorSecurityStampValidator, TwoFactorSecurityStampValidator>();
            services.TryAddScoped<IUserClaimsPrincipalFactory, UserClaimsPrincipalFactory>();
            services.TryAddScoped<IUserManager, UserManager>();
            services.TryAddScoped<ISignInManager, SignInManager>();
            services.TryAddScoped<IRoleManager, RoleManager>();
            services.TryAddScoped<TokenManager>();
            services.TryAddScoped<TokenAuth>();
            services.TryAddScoped<IAuthorizationHandler, LoggingAuthorizationHandler>();
            // services.AddAuthorization(options => options.AddPolicy("foraction", policy => policy.Requirements.Add(new AuthorizationRequirement())));
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }

            return new IdentityBuilder(typeof(User), typeof(Role), services);
        }

        /// <summary>
        /// Configures the application cookie.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configure">An action to configure the <see cref="CookieAuthenticationOptions"/>.</param>
        /// <returns>The services.</returns>
        public static IServiceCollection ConfigureApplicationCookie(this IServiceCollection services, Action<CookieAuthenticationOptions> configure)
            => services.Configure(IdentityConstants.ApplicationScheme, configure);

        /// <summary>
        /// Configure the external cookie.
        /// </summary>
        /// <param name="services">The services available in the application.</param>
        /// <param name="configure">An action to configure the <see cref="CookieAuthenticationOptions"/>.</param>
        /// <returns>The services.</returns>
        public static IServiceCollection ConfigureExternalCookie(this IServiceCollection services, Action<CookieAuthenticationOptions> configure)
            => services.Configure(IdentityConstants.ExternalScheme, configure);

    }
}
