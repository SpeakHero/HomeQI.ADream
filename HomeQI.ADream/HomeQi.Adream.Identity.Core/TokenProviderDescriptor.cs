// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// Used to represents a token provider in <see cref="TokenOptions"/>'s TokenMap.
    /// </summary>
    public class TokenProviderDescriptor
    {
        /// <summary>
        /// 初始化一个新实例 <see cref="TokenProviderDescriptor"/> class.
        /// </summary>
        /// <param name="type">此令牌提供程序的具体类型。</param>
        public TokenProviderDescriptor(Type type)
        {
            ProviderType = type;
        }

        /// <summary>
        ///将用于此令牌提供程序的类型。
        /// </summary>
        public Type ProviderType { get; }

        /// <summary>
        /// 如果指定，则将用于令牌提供程序的实例。
        /// </summary>
        public object ProviderInstance { get; set; }
    }
}