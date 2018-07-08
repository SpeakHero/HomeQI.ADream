// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Entites;
using System.Linq;

namespace HomeQI.ADream.Identity.Store
{
    /// <summary>
    /// 提供用于查询角色存储中的角色的抽象。
    /// </summary>

    public interface IQueryableRoleStore : IRoleStore   
    {
        IQueryable<Role> Roles { get; }
    }
}