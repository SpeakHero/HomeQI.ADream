// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.Adream.Identity.Core;
using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.EntityFrameworkCore;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// 提供用于管理持久存储中的角色的API。
    /// </summary>
    /// <typeparam name="TRole">该类型封装了一个角色。</typeparam>
    public class RoleManager<TRole> : ManagerBase<TRole, IRoleStore<TRole>, IdentityResult, IdentityError> where TRole : EntityBase<string>
    {
        /// <summary>
        /// Constructs a new instance of <see cref="RoleManager{TRole}"/>.
        /// </summary>
        /// <param name="store">The persistence store the manager will operate over.</param>
        /// <param name="roleValidators">A collection of validators for roles.</param>
        /// <param name="keyNormalizer">The normalizer to use when normalizing role names to keys.</param>
        /// <param name="errors">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
        /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        public RoleManager(IRoleStore<TRole> store,
            IEnumerable<IRoleValidator<TRole>> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager<TRole>> logger) : base(store)
        {
            Store = store ?? throw new ArgumentNullEx(nameof(store));
            KeyNormalizer = keyNormalizer;
            ErrorDescriber = errors;
            Logger = logger;

            if (roleValidators != null)
            {
                foreach (var v in roleValidators)
                {
                    RoleValidators.Add(v);
                }
            }
        }
        /// <summary>
        /// Gets the <see cref="ILogger"/> used to log messages from the manager.
        /// </summary>
        /// <value>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </value>
        public virtual ILogger Logger { get; set; }

        /// <summary>
        /// Gets a list of validators for roles to call before persistence.
        /// </summary>
        /// <value>A list of validators for roles to call before persistence.</value>
        public IList<IRoleValidator<TRole>> RoleValidators { get; } = new List<IRoleValidator<TRole>>();

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> 用于提供错误消息。
        /// </summary>
        /// <value>
        /// The <see cref="IdentityErrorDescriber"/> 用于提供错误消息。
        /// </value>
        public IdentityErrorDescriber ErrorDescriber { get; set; }

        /// <summary>
        /// 获取在将角色名规范化到键时使用的规范化器。
        /// </summary>
        /// <value>
        /// 将角色名规范化为键时使用的规范化器。
        /// </value>
        public ILookupNormalizer KeyNormalizer { get; set; }

        /// <summary>
        /// Gets an IQueryable collection of Roles if the persistence store is an <see cref="IQueryableRoleStore{TRole}"/>,
        /// otherwise throws a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <value>An IQueryable collection of Roles if the persistence store is an <see cref="IQueryableRoleStore{TRole}"/>.</value>
        /// <exception cref="NotSupportedException">Thrown if the persistence store is not an <see cref="IQueryableRoleStore{TRole}"/>.</exception>
        /// <remarks>
        /// Callers to this property should use <see cref="SupportsQueryableRoles"/> to ensure the backing role store supports 
        /// returning an IQueryable list of roles.
        /// </remarks>
        public virtual IQueryable<TRole> Roles
        {
            get
            {
                if (!(Store is IQueryableRoleStore<TRole> queryableStore))
                {
                    throw new NotSupportedException(Resources.StoreNotIQueryableRoleStore);
                }
                return queryableStore.Roles;
            }
        }
        /// <summary>
        /// 获取指示基础持久存储是否支持返回的标志。 <see cref="IQueryable"/> collection of roles.
        /// </summary>
        /// <value>
        /// true if the underlying persistence store supports returning an <see cref="IQueryable"/> collection of roles, otherwise false.
        /// </value>
        public virtual bool SupportsQueryableRoles
        {
            get
            {
                ThrowIfDisposed();
                return Store is IQueryableRoleStore<TRole>;
            }
        }

        /// <summary>
        /// Gets a flag indicating whether the underlying persistence store supports <see cref="Claim"/>s for roles.
        /// </summary>
        /// <value>
        /// true if the underlying persistence store supports <see cref="Claim"/>s for roles, otherwise false.
        /// </value>
        public virtual bool SupportsRoleClaims
        {
            get
            {
                ThrowIfDisposed();
                return Store is IRoleClaimStore<TRole>;
            }
        }

        /// <summary>
        /// Creates the specified <paramref name="role"/> in the persistence store.
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        public virtual async Task<IdentityResult> CreateAsync(TRole role)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullEx(nameof(role));
            }
            var result = await ValidateRoleAsync(role);
            if (!result.Succeeded)
            {
                return result;
            }
            await UpdateNormalizedRoleNameAsync(role);
            result = await Store.CreateAsync(role, CancellationToken);
            return result;
        }

        /// <summary>
        /// Updates the normalized name for the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose normalized name needs to be updated.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        public virtual async Task UpdateNormalizedRoleNameAsync(TRole role)
        {
            var name = await GetRoleNameAsync(role);
            await Store.SetNormalizedRoleNameAsync(role, NormalizeKey(name), CancellationToken);
        }

        /// <summary>
        /// Updates the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to updated.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the update.
        /// </returns>
        public virtual Task<IdentityResult> UpdateAsync(TRole role, params string[] paramss)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullEx(nameof(role));
            }

            return UpdateRoleAsync(role, paramss);
        }

        /// <summary>
        /// Deletes the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to delete.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the delete.
        /// </returns>
        public virtual Task<IdentityResult> DeleteAsync(TRole role)
        {
            ThrowIfDisposed();
            if (role == null)
            {
                throw new ArgumentNullEx(nameof(role));
            }

            return Store.DeleteAsync(role, CancellationToken);
        }

        /// <summary>
        /// Gets a flag indicating whether the specified <paramref name="roleName"/> exists.
        /// </summary>
        /// <param name="roleName">The role name whose existence should be checked.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing true if the role name exists, otherwise false.
        /// </returns>
        public virtual async Task<bool> RoleExistsAsync(string roleName)
        {
            ThrowIfDisposed();
            if (roleName == null)
            {
                throw new ArgumentNullEx(nameof(roleName));
            }

            return await FindByNameAsync(NormalizeKey(roleName)) != null;
        }

        /// <summary>
        /// Gets a normalized representation of the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The value to normalize.</param>
        /// <returns>A normalized representation of the specified <paramref name="key"/>.</returns>
        public virtual string NormalizeKey(string key)
        {
            return (KeyNormalizer == null) ? key : KeyNormalizer.Normalize(key);
        }

        /// <summary>
        /// Finds the role associated with the specified <paramref name="roleId"/> if any.
        /// </summary>
        /// <param name="roleId">The role ID whose role should be returned.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the role 
        /// associated with the specified <paramref name="roleId"/>
        /// </returns>
        public virtual Task<TRole> FindByIdAsync(string roleId)
        {
            ThrowIfDisposed();
            return Store.FindByIdAsync(roleId, CancellationToken);
        }

        /// <summary>
        /// Gets the name of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose name should be retrieved.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the name of the 
        /// specified <paramref name="role"/>.
        /// </returns>
        public virtual Task<string> GetRoleNameAsync(TRole role)
        {
            ThrowIfDisposed();
            return Store.GetRoleNameAsync(role, CancellationToken);
        }

        /// <summary>
        /// Sets the name of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose name should be set.</param>
        /// <param name="name">The name to set.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<IdentityResult> SetRoleNameAsync(TRole role, string name)
        {
            ThrowIfDisposed();

            await Store.SetRoleNameAsync(role, name, CancellationToken);
            await UpdateNormalizedRoleNameAsync(role);
            return IdentityResult.Success();
        }

        /// <summary>
        /// Gets the ID of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose ID should be retrieved.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the ID of the 
        /// specified <paramref name="role"/>.
        /// </returns>
        public virtual Task<string> GetRoleIdAsync(TRole role)
        {
            ThrowIfDisposed();
            return Store.GetRoleIdAsync(role, CancellationToken);
        }

        /// <summary>
        /// 查找与指定的关联的角色。 <paramref name="roleName"/> if any.
        /// </summary>
        /// <param name="roleName">The name of the role to be returned.</param>
        /// <returns>
        /// The <see cref="Task"/> 表示异步操作，包含角色。
        //与指定的关联<paramref name="roleName"/>
        /// </returns>
        public virtual Task<TRole> FindByNameAsync(string roleName)
        {
            ThrowIfDisposed();
            if (roleName == null)
            {
                throw new ArgumentNullEx(nameof(roleName));
            }

            return Store.FindByNameAsync(NormalizeKey(roleName), CancellationToken);
        }

        /// <summary>
        ///向角色添加声明。
        /// </summary>
        /// <param name="role">添加索赔的角色。</param>
        /// <param name="claim">The claim to add.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<IdentityResult> AddClaimAsync(TRole role, Claim claim)
        {
            ThrowIfDisposed();
            var claimStore = GetClaimStore();
            if (claim == null)
            {
                throw new ArgumentNullEx(nameof(claim));
            }
            if (role == null)
            {
                throw new ArgumentNullEx(nameof(role));
            }
            return await claimStore.AddClaimAsync(role, claim, CancellationToken);
        }

        /// <summary>
        /// 删除角色的声明。
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<IdentityResult> RemoveClaimAsync(TRole role, Claim claim)
        {
            ThrowIfDisposed();
            var claimStore = GetClaimStore();
            if (role == null)
            {
                throw new ArgumentNullEx(nameof(role));
            }
            return await claimStore.RemoveClaimAsync(role, claim, CancellationToken);
        }

        /// <summary>
        /// 获取与指定的关联的声明的列表。 <paramref name="role"/>.
        /// </summary>
        /// <param name="role">应该归还索赔的角色。</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the list of <see cref="Claim"/>s
        /// associated with the specified <paramref name="role"/>.
        /// </returns>
        public virtual Task<IList<Claim>> GetClaimsAsync(TRole role)
        {
            ThrowIfDisposed();
            var claimStore = GetClaimStore();
            if (role == null)
            {
                throw new ArgumentNullEx(nameof(role));
            }
            return claimStore.GetClaimsAsync(role, CancellationToken);
        }



        /// <summary>
        /// 释放角色管理器所使用的非托管资源，并可选地释放托管资源。
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                Store.Dispose();
            }
            _disposed = true;
        }

        /// <summary>
        /// Should return <see cref="IdentityResult.Success()"/> if validation is successful. This is
        /// called before saving the role via Create or Update.
        /// </summary>
        /// <param name="role">The role</param>
        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        protected virtual async Task<IdentityResult> ValidateRoleAsync(TRole role)
        {
            var errors = new List<IdentityError>();
            foreach (var v in RoleValidators)
            {
                var result = await v.ValidateAsync(this, role);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors);
                }
            }
            if (errors.Count > 0)
            {
                Logger.LogWarning(0, "Role {roleId} validation failed: {errors}.", await GetRoleIdAsync(role), string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success();
        }

        /// <summary>
        /// 在验证和更新归一化角色名之后调用该函数来更新角色。
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>操作是否已成功完成.</returns>
        protected virtual async Task<IdentityResult> UpdateRoleAsync(TRole role, params string[] paramss)
        {
            var result = await ValidateRoleAsync(role);
            if (!result.Succeeded)
            {
                return result;
            }
            await UpdateNormalizedRoleNameAsync(role);
            return await Store.UpdateAsync(role, CancellationToken, paramss);
        }

        // IRoleClaimStore 存储方法
        protected IRoleClaimStore<TRole> GetClaimStore()
        {
            if (!(Store is IRoleClaimStore<TRole> cast))
            {
                throw new NotSupportedException(Resources.StoreNotIRoleClaimStore);
            }
            return cast;
        }


    }
}
