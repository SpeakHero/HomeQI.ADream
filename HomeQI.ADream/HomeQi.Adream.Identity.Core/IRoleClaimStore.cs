// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Entities.Framework;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// Provides an abstraction for a store of role specific claims.
    /// </summary>
    /// <typeparam name="TRole">该类型封装了一个角色。</typeparam>
    public interface IRoleClaimStore<TRole> : IRoleStore<TRole> where TRole : EntityBase<string>
    {
        /// <summary>
        ///  Gets a list of <see cref="Claim"/>s to be belonging to the specified <paramref name="role"/> as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role whose claims to retrieve.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that represents the result of the asynchronous query, a list of <see cref="Claim"/>s.
        /// </returns>
        Task<IList<Claim>> GetClaimsAsync(TRole role, CancellationToken cancellationToken = default);

        /// <summary>
        /// Add a new claim to a role as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to add a claim to.</param>
        /// <param name="claim">The <see cref="Claim"/> to add.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IdentityResult> AddClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default);

        /// <summary>
        /// Remove a claim from a role as an asynchronous operation.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The <see cref="Claim"/> to remove.</param>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> used to propagate notifications that the operation should be canceled.</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        Task<IdentityResult> RemoveClaimAsync(TRole role, Claim claim, CancellationToken cancellationToken = default);
    }
}