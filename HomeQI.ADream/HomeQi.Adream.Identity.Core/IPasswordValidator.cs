// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// �ṩ������֤����
    /// </summary>
    /// <typeparam name="TUser">The type that represents a user.</typeparam>
    public interface IPasswordValidator<TUser> where TUser : ADream.Entities.Framework.EntityBase<string>
    {
        /// <summary>
        /// Validates a password as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> to retrieve the <paramref name="user"/> properties from.</param>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password supplied for validation</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user, string password);
    }
}