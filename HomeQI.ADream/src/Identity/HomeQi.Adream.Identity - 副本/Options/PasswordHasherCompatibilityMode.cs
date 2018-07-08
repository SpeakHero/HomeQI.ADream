// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace HomeQI.ADream.Identity.Options
{
    /// <summary>
    /// 指定用于散列密码的格式。
    /// </summary>
    public enum PasswordHasherCompatibilityMode
    {
        /// <summary>
        /// 以与ASP.NET身份版本1和2兼容的方式指示散列密码。
        /// </summary>
        IdentityV2,

        /// <summary>
        /// 以与ASP.NET身份验证版本3兼容的方式指示散列密码。
        /// </summary>
        IdentityV3
    }
}