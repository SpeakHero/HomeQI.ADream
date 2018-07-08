// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace HomeQI.Adream.Identity
{

    public class AspNetRoleManager : AspNetRoleManager<IdentityRole>
    {
        public AspNetRoleManager(IRoleStore<IdentityRole> store, IEnumerable<IRoleValidator<IdentityRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<IdentityRole>> logger, IHttpContextAccessor contextAccessor) : base(store, roleValidators, keyNormalizer, errors, logger, contextAccessor)
        {
        }
    }
    /// <summary>
    /// �ṩ���ڹ���־ô洢�еĽ�ɫ��API��
    /// </summary>
    /// <typeparam name="TRole">�����ͷ�װ��һ����ɫ��</typeparam>
    public class AspNetRoleManager<TRole> : RoleManager<TRole>, IDisposable where TRole : class
    {
        private readonly CancellationToken _cancel;

        /// <summary>
        /// Constructs a new instance of <see cref="RoleManager{TRole}"/>.
        /// </summary>
        /// <param name="store">�������ĳ־ô洢�����С�</param>
        /// <param name="roleValidators">��ɫ����֤�����ϡ�</param>
        /// <param name="keyNormalizer">����ɫ���淶��Ϊ��ʱʹ�õĹ淶������</param>
        /// <param name="errors">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
        /// <param name="logger">��¼�����ڼ�¼��Ϣ������ʹ���</param>
        /// <param name="contextAccessor">���ʵķ�����<see cref="HttpContext"/>.</param>
        public AspNetRoleManager(IRoleStore<TRole> store,
            IEnumerable<IRoleValidator<TRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<TRole>> logger,
            IHttpContextAccessor contextAccessor)
            : base(store, roleValidators, keyNormalizer, errors, logger)
        {
            _cancel = contextAccessor?.HttpContext?.RequestAborted ?? CancellationToken.None;
        }

        /// <summary>
        /// The cancellation token associated with the current HttpContext.RequestAborted or CancellationToken.None if unavailable.
        /// </summary>
        protected override CancellationToken CancellationToken => _cancel;
    }
}
