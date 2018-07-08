// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Entites;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Store
{
    /// <summary>
    /// 为存储用户的恢复代码的商店提供抽象。
    /// </summary>
    public interface IUserTwoFactorRecoveryCodeStore : IUserStore
    {
        /// <summary>
        /// Updates the recovery codes for the user while invalidating any previous recovery codes.
        /// </summary>
        /// <param name="user">The user to store new recovery codes for.</param>
        /// <param name="recoveryCodes">The new recovery codes for the user.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The new recovery codes for the user.</returns>
        Task ReplaceCodesAsync(User user, IEnumerable<string> recoveryCodes, CancellationToken cancellationToken);

        /// <summary>
        /// Returns whether a recovery code is valid for a user. Note: recovery codes are only valid
        /// once, and will be invalid after use.
        /// </summary>
        /// <param name="user">The user who owns the recovery code.</param>
        /// <param name="code">The recovery code to use.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>True if the recovery code was found for the user.</returns>
        Task<bool> RedeemCodeAsync(User user, string code, CancellationToken cancellationToken);

        /// <summary>
        /// Returns how many recovery code are still valid for a user.
        /// </summary>
        /// <param name="user">The user who owns the recovery code.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The number of valid recovery codes for the user..</returns>
        Task<int> CountCodesAsync(User user, CancellationToken cancellationToken);
    }
}