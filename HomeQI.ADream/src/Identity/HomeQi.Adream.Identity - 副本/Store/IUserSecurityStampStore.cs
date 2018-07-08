// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Entites;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Store
{
    /// <summary>
    /// 为存储用户安全戳的商店提供抽象。
    /// </summary>
    public interface IUserSecurityStampStore  : IUserStore
    {
        /// <summary>
        /// Sets the provided security <paramref name="stamp"/> for the specified <paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose security stamp should be set.</param>
        /// <param name="stamp">The security stamp to set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetSecurityStampAsync(User user, string stamp, CancellationToken cancellationToken);

        /// <summary>
        /// Get the security stamp for the specified <paramref name="user" />.
        /// </summary>
        /// <param name="user">The user whose security stamp should be set.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the security stamp for the specified <paramref name="user"/>.</returns>
        Task<string> GetSecurityStampAsync(User user, CancellationToken cancellationToken);
    }
}