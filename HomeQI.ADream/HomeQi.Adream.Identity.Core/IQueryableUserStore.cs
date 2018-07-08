// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Linq;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// Provides an abstraction for querying users in a User store.
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public interface IQueryableUserStore<TUser> : IUserStore<TUser> where TUser : ADream.Entities.Framework.EntityBase<string>
    {
        /// <summary>
        /// Returns an <see cref="IQueryable{T}"/> collection of users.
        /// </summary>
        /// <value>An <see cref="IQueryable{T}"/> collection of users.</value>
        IQueryable<TUser> Users { get; }
    }
}