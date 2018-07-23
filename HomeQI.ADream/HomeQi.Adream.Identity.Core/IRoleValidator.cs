// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Entities.Framework;
using System.Threading.Tasks;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// �ṩ������֤��ɫ�ĳ���
    /// </summary>
    /// <typeparam name="TRole">�����ͷ�װ��һ����ɫ��</typeparam>
    public interface IRoleValidator<TRole> where TRole : EntityBase<string>
    {
        /// <summary>
        /// Validates a role as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="RoleManager{TRole}"/> managing the role store.</param>
        /// <param name="role">The role to validate.</param>
        /// <returns>A <see cref="Task{TResult}"/> that represents the <see cref="IdentityResult"/> of the asynchronous validation.</returns>
        Task<IdentityResult> ValidateAsync(RoleManager<TRole> manager, TRole role);
    }
}