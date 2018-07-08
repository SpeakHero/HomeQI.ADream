// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Entites;

namespace HomeQI.ADream.Identity
{
    /// <summary>
    /// 提供散列密码的抽象。
    /// </summary>
    public interface IPasswordHasher
    {
        string HashPassword(User user, string password);
        PasswordVerificationResult VerifyHashedPassword(User user, string hashedPassword, string providedPassword);
    }
}