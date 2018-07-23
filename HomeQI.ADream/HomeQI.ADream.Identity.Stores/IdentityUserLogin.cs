// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Entities.Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.Adream.Identity
{

    public class IdentityUserLogin : IdentityUserLogin<string>
    {
        public IdentityUserLogin()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
    /// <summary>
    /// 表示用户的登录及其关联的提供程序。
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key of the user associated with this login.</typeparam>
    public class IdentityUserLogin<TKey> : EntityBase<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the login provider for the login (e.g. facebook, google)
        /// </summary>
        [Required]
        public virtual string LoginProvider { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique provider identifier for this login.
        /// </summary>
        [Required]
        public virtual string ProviderKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the friendly name used in a UI for this login.
        /// </summary>
        [Required]
        public virtual string ProviderDisplayName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the of the primary key of the user associated with this login.
        /// </summary>
        [Required]
        public virtual TKey UserId { get; set; }
    }
}