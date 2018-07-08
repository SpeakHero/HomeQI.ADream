// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Entities.Framework;
using System;

namespace HomeQI.Adream.Identity
{
    public class IdentityUserToken : IdentityUserToken<string>
    {
        public IdentityUserToken()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
    /// <summary>
    ///表示用户的身份验证令牌
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key used for users.</typeparam>
    public class IdentityUserToken<TKey> : EntityBase<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key of the user that the token belongs to.
        /// </summary>
        public virtual TKey UserId { get; set; }

        /// <summary>
        /// Gets or sets the LoginProvider this token is from.
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the name of the token.
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the token value.
        /// </summary>
        [ProtectedPersonalData]
        public virtual string Value { get; set; }
    }
}