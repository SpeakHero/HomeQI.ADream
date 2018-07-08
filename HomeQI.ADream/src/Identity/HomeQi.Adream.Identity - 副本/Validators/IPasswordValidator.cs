// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Manager;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Validators
{
    /// <summary>
    /// Provides an abstraction for validating passwords.
    /// </summary>
    public interface IPasswordValidator 
    {
        /// <summary>
        /// Validates a password as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager "/> to retrieve the 
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password supplied for validation</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IdentityResult> ValidateAsync(IUserManager  manager,  User user, string password);
    }
}