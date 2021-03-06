// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Manager;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Store
{
    /// <summary>
    /// 为两个因子令牌生成器提供抽象。
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface IUserTwoFactorTokenProvider
    {
        Task<string> GenerateAsync(string purpose, IUserManager manager, User user);
       
        Task<bool> ValidateAsync(string purpose, string token, IUserManager manager, User user);

        /// <summary>
        /// 返回一个标志，该标记指示令牌提供程序是否可以生成适合于双因素身份验证令牌的令牌。
        /// the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user a token could be generated for.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the a flag indicating if a two
        /// factor token could be generated by this provider for the specified <paramref name="user"/>.
        /// The task will return true if a two factor authentication token could be generated, otherwise false.
        /// </returns>
        Task<bool> CanGenerateTwoFactorTokenAsync(IUserManager manager, User user);
    }
}
