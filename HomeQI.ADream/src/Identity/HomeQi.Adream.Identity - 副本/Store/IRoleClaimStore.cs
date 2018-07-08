// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Entites;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Store
{
    /// <summary>
    /// 为角色特定声明的存储提供抽象。
    /// </summary>

    public interface IRoleClaimStore : IRoleStore
    {
        Task<IList<Claim>> GetClaimsAsync(Role role,  CancellationToken cancellationToken = default);

        Task AddClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default);

        Task RemoveClaimAsync(Role role, Claim claim, CancellationToken cancellationToken = default);
    }
}