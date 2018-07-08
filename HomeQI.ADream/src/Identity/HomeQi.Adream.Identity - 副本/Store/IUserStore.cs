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
    /// 为管理用户帐户的商店提供抽象。
    /// </summary>
    public interface IUserStore : IEntityStore<User, IdentityResult, IdentityError>
    {
        Task<string> GetUserNameAsync(User user, CancellationToken cancellationToken);

        Task SetUserNameAsync(User user, string userName, CancellationToken cancellationToken);

        Task<string> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken);

        Task SetNormalizedUserNameAsync(User user, string normalizedName, CancellationToken cancellationToken);
        Task<User> FindByPhoneAsync(string phone, CancellationToken cancellationToken);

        Task<User> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken);
    }
}
