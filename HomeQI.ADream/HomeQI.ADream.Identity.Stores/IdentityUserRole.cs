// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.Adream.Identity
{
    public class IdentityUserRole : IdentityUserRole<string>
    {
        public IdentityUserRole()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
    /// <summary>
    ///表示用户和角色之间的链接。
    /// </summary>
    /// <typeparam name="TKey">用于用户和角色的主键的类型。</typeparam>
    public class IdentityUserRole<TKey> : EntityBase<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the primary key of the user that is linked to a role.
        /// </summary>
        [Required]
        public virtual TKey UserId { get; set; }

        /// <summary>
        /// Gets or sets the primary key of the role that is linked to the user.
        /// </summary>
        [Required]
        public virtual TKey RoleId { get; set; }

    }
}