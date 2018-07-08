// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Entites;
using System.Linq;

namespace HomeQI.ADream.Identity.Store
{
    public interface IQueryableUserStore : IUserStore
    {
        IQueryable<User> Users { get; }
    }
}