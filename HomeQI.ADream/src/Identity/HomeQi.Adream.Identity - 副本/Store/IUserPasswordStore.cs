// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Entites;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Store
{
    /// <summary>
    /// 为包含用户密码散列的商店提供抽象。
    /// </summary>
    /// <typeparam name="User">The type encapsulating a user.</typeparam>
    public interface IUserPasswordStore : IUserStore
    {
        /// <summary>
        /// Sets the password hash for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose password hash to set.</param>
        /// <param name="passwordHash">The password hash to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetPasswordHashAsync(User user, string passwordHash, CancellationToken cancellationToken);

        /// <summary>
        /// Gets the password hash for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose password hash to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, returning the password hash for the specified <paramref name="user"/>.</returns>
        Task<string> GetPasswordHashAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a flag indicating whether the specified <paramref name="user"/> has a password.
        /// </summary>
        /// <param name="user">The user to return a flag for, indicating whether they have a password or not.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, returning true if the specified <paramref name="user"/> has a password
        /// otherwise false.
        /// </returns>
        Task<bool> HasPasswordAsync(User user, CancellationToken cancellationToken);
    }
}