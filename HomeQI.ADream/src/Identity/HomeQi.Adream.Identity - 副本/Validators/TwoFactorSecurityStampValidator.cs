// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Manager;
using HomeQI.ADream.Identity.Options;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Validators
{

    public class TwoFactorSecurityStampValidator : SecurityStampValidator<User>, ITwoFactorSecurityStampValidator 
    {
        /// <summary>
        /// Creates a new instance of <see cref="SecurityStampValidator"/>.
        /// </summary>
        /// <param name="options">Used to access the <see cref="IdentityOptions"/>.</param>
        /// <param name="signInManager">The <see cref="SignInManager"/>.</param>
        /// <param name="clock">The system clock.</param>
        public TwoFactorSecurityStampValidator(IOptions<SecurityStampValidatorOptions> options, ISignInManager signInManager, ISystemClock clock) : base(options, signInManager, clock)
        { }

        /// <summary>
        /// Verifies the principal's security stamp, returns the matching user if successful
        /// </summary>
        /// <param name="principal">The principal to verify.</param>
        /// <returns>The verified user or null if verification fails.</returns>
        protected override Task<User> VerifySecurityStamp(ClaimsPrincipal principal)
            => SignInManager.ValidateTwoFactorSecurityStampAsync(principal);

        /// <summary>
        /// Called when the security stamp has been verified.
        /// </summary>
        /// <param name="user">The user who has been verified.</param>
        /// <param name="context">The <see cref="CookieValidatePrincipalContext"/>.</param>
        /// <returns>A task.</returns>
        protected override Task SecurityStampVerified(User user, CookieValidatePrincipalContext context)
            => Task.CompletedTask;
    }
}