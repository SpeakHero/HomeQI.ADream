using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace HomeQI.ADream.Identity.Entites
{
    public class ExternalLoginInfo : UserLoginInfo
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExternalLoginInfo"/>
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> to associate with this login.</param>
        /// <param name="loginProvider">The provider associated with this login information.</param>
        /// <param name="providerKey">The unique identifier for this user provided by the login provider.</param>
        /// <param name="displayName">The display name for this user provided by the login provider.</param>
        public ExternalLoginInfo(ClaimsPrincipal principal, string loginProvider, string providerKey,
            string displayName) : base(loginProvider, providerKey, displayName)
        {
            Principal = principal;
        }

        /// <summary>
        /// Gets or sets the <see cref="ClaimsPrincipal"/> associated with this login.
        /// </summary>
        /// <value>The <see cref="ClaimsPrincipal"/> associated with this login.</value>
        public ClaimsPrincipal Principal { get; set; }

        /// <summary>
        /// The <see cref="AuthenticationToken"/>s associated with this login.
        /// </summary>
        public IEnumerable<AuthenticationToken> AuthenticationTokens { get; set; }
    }
}
