// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Entites;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Store
{
    /// <summary>
    /// 提供可用于实现帐户锁定的存储信息的抽象，
    //包括访问失败和锁定状态
    /// </summary>
    /// <typeparam name="User">The type that represents a user.</typeparam>
    public interface IUserLockoutStore: IUserStore
    {
        /// <summary>
        /// Gets the last <see cref="DateTimeOffset"/> a user's last lockout expired, if any.
        /// Any time in the past should be indicates a user is not locked out.
        /// </summary>
        /// <param name="user">The user whose lockout date should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the result of the asynchronous query, a <see cref="DateTimeOffset"/> containing the last time
        /// a user's lockout expired, if any.
        /// </returns>
        Task<DateTimeOffset?> GetLockoutEndDateAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Locks out a user until the specified end date has passed. Setting a end date in the past immediately unlocks a user.
        /// </summary>
        /// <param name="user">The user whose lockout date should be set.</param>
        /// <param name="lockoutEnd">The <see cref="DateTimeOffset"/> after which the <paramref name="user"/>'s lockout should end.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetLockoutEndDateAsync(User user, DateTimeOffset? lockoutEnd, CancellationToken cancellationToken);

        /// <summary>
        /// 记录发生了失败的访问，增加了失败的访问计数。
        /// </summary>
        /// <param name="user">The user whose cancellation count should be incremented.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the incremented failed access count.</returns>
        Task<int> IncrementAccessFailedCountAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// 重置用户的失败访问计数。
        /// </summary>
        /// <param name="user">The user whose failed access count should be reset.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        /// <remarks>This is typically called after the account is successfully accessed.</remarks>
        Task ResetAccessFailedCountAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// 检索指定的当前失败访问计数。<paramref name="user"/>.
        /// </summary>
        /// <param name="user">The user whose failed access count should be retrieved.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the failed access count.</returns>
        Task<int> GetAccessFailedCountAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        ///检索指示用户锁定是否可以为指定用户启用的标志。
        /// </summary>
        /// <param name="user">The user whose ability to be locked out should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, true if a user can be locked out, otherwise false.
        /// </returns>
        Task<bool> GetLockoutEnabledAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// 设置指示是否指定的标志<paramref name="user"/> can be locked out.
        /// </summary>
        /// <param name="user">The user whose ability to be locked out should be set.</param>
        /// <param name="enabled">A flag indicating if lock out can be enabled for the specified <paramref name="user"/>.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task SetLockoutEnabledAsync(User user, bool enabled, CancellationToken cancellationToken);
    }
}