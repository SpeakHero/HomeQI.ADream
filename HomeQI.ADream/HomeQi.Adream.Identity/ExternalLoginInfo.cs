// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// 表示用户记录的登录信息、源代码和外部源代码。
    /// </summary>
    public class ExternalLoginInfo : UserLoginInfo
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExternalLoginInfo"/>
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> to associate with this login.</param>
        /// <param name="loginProvider">与此登录信息相关联的提供程序。</param>
        /// <param name="providerKey">由登录提供程序提供的此用户的唯一标识符。</param>
        /// <param name="displayName">由登录提供程序提供的用户的显示名称。</param>
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