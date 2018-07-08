using HomeQI.ADream.Models.Entities.Identity;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeQI.ADream.Services.Identity.Extensions
{
    public static class IdentityServicesExtensions
    {
        
        public static IdentityBuilder AddIdentityCore(this IServiceCollection services)
            => services.AddIdentityCore(o => { });

        public static IdentityBuilder AddIdentityCore(this IServiceCollection services, Action<IdentityOptions> setupAction)

        {
            // Services identity depends on
            services.AddOptions().AddLogging();

            // Services used by identity
            services.TryAddScoped<IUserValidator<User>, UserValidator>();
            services.TryAddScoped<IPasswordValidator<User>, PasswordValidator<User>>();
            services.TryAddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IdentityErrorDescriber, AppIdentityErrorDescriber>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory>();
            services.TryAddScoped<UserManager<User>, UserServices>();
            if (setupAction != null)
            {
                services.Configure(setupAction);
            }
            return new IdentityBuilder(typeof(User), services);
        }

        public static IdentityBuilder AddIdentity(this IServiceCollection services)
        {
            return services.AddIdentity(null);
        }

        public static IServiceCollection ConfigureApplicationCookie(this IServiceCollection services, Action<CookieAuthenticationOptions> configure)
        {
            return OptionsServiceCollectionExtensions.Configure(services, IdentityConstants.ApplicationScheme, configure);
        }

        public static IServiceCollection ConfigureExternalCookie(this IServiceCollection services, Action<CookieAuthenticationOptions> configure)
        {
            return OptionsServiceCollectionExtensions.Configure(services, IdentityConstants.ExternalScheme, configure);
        }


        public static IdentityBuilder AddIdentity(this IServiceCollection services, Action<IdentityOptions> setupAction)
        {
            services.AddAuthentication(delegate (AuthenticationOptions options)
            {
                options.DefaultAuthenticateScheme = IdentityConstants.ApplicationScheme;
                options.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, delegate (CookieAuthenticationOptions o)
            {
                o.LoginPath = new PathString("/Account/Login");
                o.Events = new CookieAuthenticationEvents
                {
                    OnValidatePrincipal = new Func<CookieValidatePrincipalContext, Task>(SecurityStampValidator.ValidatePrincipalAsync)
                };
            }).AddCookie(IdentityConstants.ExternalScheme, delegate (CookieAuthenticationOptions o)
            {
                o.Cookie.Name = IdentityConstants.ExternalScheme;
                o.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
            })
                .AddCookie(IdentityConstants.TwoFactorRememberMeScheme, delegate (CookieAuthenticationOptions o)
                {
                    o.Cookie.Name = IdentityConstants.TwoFactorRememberMeScheme;
                })
                .AddCookie(IdentityConstants.TwoFactorUserIdScheme, delegate (CookieAuthenticationOptions o)
                {
                    o.Cookie.Name = IdentityConstants.TwoFactorUserIdScheme;
                    o.ExpireTimeSpan = TimeSpan.FromMinutes(5.0);
                });
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.TryAddScoped<IUserValidator<User>, UserValidator>();
            services.TryAddScoped<IPasswordValidator<User>, PasswordValidator<User>>();
            services.TryAddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            services.TryAddScoped<ILookupNormalizer, UpperInvariantLookupNormalizer>();
            services.TryAddScoped<IRoleValidator<Role>, RoleValidator<Role>>();
            services.TryAddScoped<IdentityErrorDescriber>();
            services.TryAddScoped<ISecurityStampValidator, SecurityStampValidator<User>>();
            services.TryAddScoped<IUserClaimsPrincipalFactory<User>, UserClaimsPrincipalFactory<User, Role>>();
            services.TryAddScoped<UserManager<User>, UserServices>();
            services.TryAddScoped<SignInManager<User>, SignInManager<User>>();
            services.TryAddScoped<RoleManager<Role>, AspNetRoleManager<Role>>();
            if (setupAction != null)
            {
                OptionsServiceCollectionExtensions.Configure(services, setupAction);
            }
            return new IdentityBuilder(typeof(User), typeof(Role), services);
        }
    }
}
