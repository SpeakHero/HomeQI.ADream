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
    /// 提供用于管理持久存储中的角色的API。
    /// </summary>
    /// <typeparam name="TRole">该类型封装了一个角色。</typeparam>
    public class AspNetRoleManager<TRole> : RoleManager<TRole>, IDisposable where TRole : class
    {
        private readonly CancellationToken _cancel;

        /// <summary>
        /// Constructs a new instance of <see cref="RoleManager{TRole}"/>.
        /// </summary>
        /// <param name="store">管理器的持久存储将运行。</param>
        /// <param name="roleValidators">角色的验证器集合。</param>
        /// <param name="keyNormalizer">将角色名规范化为键时使用的规范化器。</param>
        /// <param name="errors">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
        /// <param name="logger">记录器用于记录消息、警告和错误。</param>
        /// <param name="contextAccessor">访问的访问器<see cref="HttpContext"/>.</param>
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
