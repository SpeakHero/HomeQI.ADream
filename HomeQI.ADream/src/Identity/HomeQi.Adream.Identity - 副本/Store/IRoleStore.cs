// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.EntityFrameworkCore;
using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Store
{
    /// <summary>
    /// 提供对角色的存储和管理的抽象。
    /// </summary>
    public interface IRoleStore:IEntityStore<Role,IdentityResult,IdentityError>
    {
        Task<string> GetRoleIdAsync(Role role, CancellationToken cancellationToken);
        Task<string> GetRoleNameAsync(Role role, CancellationToken cancellationToken);
        Task SetRoleNameAsync(Role role, string roleName, CancellationToken cancellationToken);
        Task<Role> FindByNameAsync(string normalizedRoleName, CancellationToken cancellationToken);
    }
}