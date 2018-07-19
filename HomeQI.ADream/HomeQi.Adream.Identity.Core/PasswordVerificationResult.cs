// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// Specifies the results for password verification.
    /// </summary>
    public enum PasswordVerificationResult
    {
        /// <summary>
        /// 指示密码验证失败。
        /// </summary>
        Failed = 0,

        /// <summary>
        /// 指示密码验证成功。
        /// </summary>
        Success = 1,

        /// <summary>
        ///指示密码验证是成功的，但是密码是使用不推荐的算法进行编码的。
        /// and should be rehashed and updated.
        /// </summary>
        SuccessRehashNeeded = 2
    }
}