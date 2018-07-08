// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Entites;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Store
{
    /// <summary>
    ///为将用户映射到角色的存储提供抽象。
    /// </summary>
    public interface IUserRoleStore  : IUserStore
    {
        /// <summary>
        /// Add the specified <paramref name="user"/> to the named role.
        /// </summary>
        /// <param name="user">The user to add to the named role.</param>
        /// <param name="roleName">The name of the role to add the user to.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task AddToRoleAsync(User user, string roleName, CancellationToken cancellationToken);

        /// <summary>
        /// Remove the specified <paramref name="user"/> from the named role.
        /// </summary>
        /// <param name="user">The user to remove the named role from.</param>
        /// <param name="roleName">The name of the role to remove.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation.</returns>
        Task RemoveFromRoleAsync(User user, string roleName, CancellationToken cancellationToken);

        /// <summary>
        /// Gets a list of role names the specified <paramref name="user"/> belongs to.
        /// </summary>
        /// <param name="user">The user whose role names to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing a list of role names.</returns>
        Task<IList<string>> GetRolesAsync(User user, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a flag indicating whether the specified <paramref name="user"/> is a member of the given named role.
        /// </summary>
        /// <param name="user">The user whose role membership should be checked.</param>
        /// <param name="roleName">The name of the role to be checked.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a flag indicating whether the specified <paramref name="user"/> is
        /// a member of the named role.
        /// </returns>
        Task<bool> IsInRoleAsync(User user, string roleName, CancellationToken cancellationToken);

        /// <summary>
        /// Returns a list of Users who are members of the named role.
        /// </summary>
        /// <param name="roleName">The name of the role whose membership should be returned.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing a list of users who are in the named role.
        /// </returns>
        Task<IList<User>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken);
    }
}
