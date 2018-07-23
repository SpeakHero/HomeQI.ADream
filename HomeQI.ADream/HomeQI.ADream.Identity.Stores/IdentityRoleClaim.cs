// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Entities.Framework;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace HomeQI.Adream.Identity
{
    public class IdentityRoleClaim : IdentityRoleClaim<string>
    {
        public IdentityRoleClaim()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
    /// <summary>
    /// 表示授予角色内所有用户的声明。
    /// </summary>
    /// <typeparam name="TKey">The type of the primary key of the role associated with this claim.</typeparam>
    public class IdentityRoleClaim<TKey> : EntityBase<TKey> where TKey : IEquatable<TKey>
    {
        [Required]
        public virtual TKey RoleId { get; set; }

        /// <summary>
        /// Gets or sets the claim type for this claim.
        /// </summary>
        [Required]
        public virtual string ClaimType { get; set; }

        /// <summary>
        /// Gets or sets the claim value for this claim.
        /// </summary>
        [Required]
        public virtual string ClaimValue { get; set; }

        /// <summary>
        /// Constructs a new claim with the type and value.
        /// </summary>
        /// <returns></returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }

        /// <summary>
        /// 通过复制另一个声明中的CalimType和CalimalValk初始化。
        /// </summary>
        /// <param name="other">The claim to initialize from.</param>
        public virtual void InitializeFromClaim(Claim other)
        {
            ClaimType = other?.Type;
            ClaimValue = other?.Value;
        }
    }
}