// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace HomeQI.ADream.Identity
{
    /// <summary>
    /// 指定密码验证的结果。
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
        /// 指示密码验证是成功的，但是密码是使用不推荐的算法进行编码的。
        /// 并且应该重新修改和更新。
        /// </summary>
        SuccessRehashNeeded = 2
    }
}