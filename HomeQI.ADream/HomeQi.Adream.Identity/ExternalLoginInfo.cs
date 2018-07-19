// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// ��ʾ�û���¼�ĵ�¼��Ϣ��Դ������ⲿԴ���롣
    /// </summary>
    public class ExternalLoginInfo : UserLoginInfo
    {
        /// <summary>
        /// Creates a new instance of <see cref="ExternalLoginInfo"/>
        /// </summary>
        /// <param name="principal">The <see cref="ClaimsPrincipal"/> to associate with this login.</param>
        /// <param name="loginProvider">��˵�¼��Ϣ��������ṩ����</param>
        /// <param name="providerKey">�ɵ�¼�ṩ�����ṩ�Ĵ��û���Ψһ��ʶ����</param>
        /// <param name="displayName">�ɵ�¼�ṩ�����ṩ���û�����ʾ���ơ�</param>
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