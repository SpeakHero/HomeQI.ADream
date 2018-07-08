// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Entities.Framework;
using System;

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
    /// ��ʾ�û��ĵ�¼����������ṩ����
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key of the user associated with this login.</typeparam>
    public class IdentityUserLogin<TKey> : EntityBase<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// Gets or sets the login provider for the login (e.g. facebook, google)
        /// </summary>
        public virtual string LoginProvider { get; set; }

        /// <summary>
        /// Gets or sets the unique provider identifier for this login.
        /// </summary>
        public virtual string ProviderKey { get; set; }

        /// <summary>
        /// Gets or sets the friendly name used in a UI for this login.
        /// </summary>
        public virtual string ProviderDisplayName { get; set; }

        /// <summary>
        /// Gets or sets the of the primary key of the user associated with this login.
        /// </summary>
        public virtual TKey UserId { get; set; }
    }
}