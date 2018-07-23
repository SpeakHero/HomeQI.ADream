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
        /// ��ʼ��һ����ʵ�� <see cref="TokenProviderDescriptor"/> class.
        /// </summary>
        /// <param name="type">�������ṩ����ľ������͡�</param>
        public TokenProviderDescriptor(Type type)
        {
            ProviderType = type;
        }

        /// <summary>
        ///�����ڴ������ṩ��������͡�
        /// </summary>
        public Type ProviderType { get; }

        /// <summary>
        /// ���ָ���������������ṩ�����ʵ����
        /// </summary>
        public object ProviderInstance { get; set; }
    }
}