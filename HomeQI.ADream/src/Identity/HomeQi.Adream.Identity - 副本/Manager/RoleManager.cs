// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Store;
using HomeQI.ADream.Identity.Validators;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.Extensions.Logging;

namespace HomeQI.ADream.Identity.Manager
{
    /// <summary>
    /// Provides the APIs for managing roles in a persistence store.
    /// </summary>
    public class RoleManager : ManagerBase, IRoleManager
    {
        private bool _disposed;

        /// <summary>
        /// The cancellation token used to cancel operations.
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;

        /// <summary>
        /// Constructs a new instance of <see cref="RoleManager "/>.
        /// </summary>
        /// <param name="store">The persistence store the manager will operate over.</param>
        /// <param name="roleValidators">A collection of validators for roles.</param>
        /// <param name="keyNormalizer">The normalizer to use when normalizing role names to keys.</param>
        /// <param name="errors">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
        /// <param name="logger">The logger used to log messages, warnings and errors.</param>
        public RoleManager(IRoleStore store,
            IEnumerable<IRoleValidator> roleValidators,
            ILookupNormalizer keyNormalizer,
            IdentityErrorDescriber errors,
            ILogger<RoleManager> logger)
        {
            Store = store ?? throw new ArgumentNullException(nameof(store));
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
        ///获取此实例运行的持久性存储。
        /// </summary>
        /// <value>The persistence store this instance operates over.</value>
        protected IRoleStore Store { get; private set; }

        /// <summary>
        /// Gets the <see cref="ILogger"/> used to log messages from the manager.
        /// </summary>
        /// <value>
        /// The <see cref="ILogger"/> used to log messages from the manager.
        /// </value>
        public virtual ILogger Logger { get; set; }

        /// <summary>
        /// 获取在持久性之前调用角色的验证器列表。
        /// </summary>
        /// <value>A list of validators for roles to call before persistence.</value>
        public IList<IRoleValidator> RoleValidators { get; } = new List<IRoleValidator>();

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages.
        /// </summary>
        /// <value>
        /// The <see cref="IdentityErrorDescriber"/> used to provider error messages.
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
        /// 如果持久存储是一个角色，则可获得一个可查询的角色集合 <see cref="IQueryableRoleStore"/>,
        /// otherwise throws a <see cref="NotSupportedException"/>.
        /// </summary>
        /// <value>An IQueryable collection of Roles if the persistence store is an <see cref="IQueryableRoleStore "/>.</value>
        /// <exception cref="NotSupportedException">Thrown if the persistence store is not an <see cref="IQueryableRoleStore "/>.</exception>
        /// <remarks>
        /// Callers to this property should use <see cref="SupportsQueryableRoles"/> to ensure the backing role store supports 
        /// returning an IQueryable list of roles.
        /// </remarks>
        public virtual IQueryable<Role> Roles
        {
            get
            {
                if (!(Store is IQueryableRoleStore queryableStore))
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
                return Store is IQueryableRoleStore;
            }
        }

        /// <summary>
        /// 取一个标志，该标志指示底层持久存储是否支持角色
        /// </summary>
        /// <value>
        /// true if the underlying persistence store supports <see cref="Claim"/>s for roles, otherwise false.
        /// </value>
        public virtual bool SupportsRoleClaims
        {
            get
            {
                ThrowIfDisposed();
                return Store is IRoleClaimStore;
            }
        }

        /// <summary>
        /// Creates the specified <paramref name="role"/> in the persistence store.
        /// </summary>
        /// <param name="role">The role to create.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        public virtual async Task<IdentityResult> CreateAsync(Role role)
        {
            ThrowIfDisposed();
            role.CheakArgument();
            var result = await ValidateRoleAsync(role);
            if (!result.Succeeded)
            {
                return result;
            }
            await UpdateRoleNameAsync(role);
            result = await Store.CreateAsync(role, CancellationToken);
            return result;
        }

        /// <summary>
        /// 更新指定的规范化名称<paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose normalized name needs to be updated.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation.
        /// </returns>
        public virtual async Task UpdateRoleNameAsync(Role role)
        {
            var name = await GetRoleNameAsync(role);
            await Store.SetRoleNameAsync(role, NormalizeKey(name), CancellationToken);
        }

        /// <summary>
        /// Updates the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to updated.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the update.
        /// </returns>
        public virtual Task<IdentityResult> UpdateAsync(Role role)
        {
            ThrowIfDisposed();
            role.CheakArgument();
            return UpdateRoleAsync(role);
        }

        /// <summary>
        /// Deletes the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role to delete.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> for the delete.
        /// </returns>
        public virtual Task<IdentityResult> DeleteAsync(Role role)
        {
            ThrowIfDisposed();
            role.CheakArgument();
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
            roleName.CheakArgument();
            return await FindByNameAsync(NormalizeKey(roleName)) != null;
        }

        /// <summary>
        /// Gets a normalized representation of the specified <paramref name="key"/>.
        /// </summary>
        /// <param name="key">The value to normalize.</param>
        /// <returns>A normalized representation of the specified <paramref name="key"/>.</returns>
        public virtual string NormalizeKey(string key)
        {
            return key;
            // return (KeyNormalizer == null) ? key : KeyNormalizer.Normalize(key);
        }

        /// <summary>
        /// Finds the role associated with the specified <paramref name="roleId"/> if any.
        /// </summary>
        /// <param name="roleId">The role ID whose role should be returned.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the role 
        /// associated with the specified <paramref name="roleId"/>
        /// </returns>
        public virtual Task<Role> FindByIdAsync(string roleId)
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
        public virtual Task<string> GetRoleNameAsync(Role role)
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
        public virtual async Task<IdentityResult> SetRoleNameAsync(Role role, string name)
        {
            ThrowIfDisposed();

            await Store.SetRoleNameAsync(role, name, CancellationToken);
            await UpdateRoleNameAsync(role);
            return IdentityResult.Success() ;
        }

        /// <summary>
        /// Gets the ID of the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose ID should be retrieved.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the ID of the 
        /// specified <paramref name="role"/>.
        /// </returns>
        public virtual Task<string> GetRoleIdAsync(Role role)
        {
            ThrowIfDisposed();
            return Store.GetRoleIdAsync(role, CancellationToken);
        }

        /// <summary>
        /// Finds the role associated with the specified <paramref name="roleName"/> if any.
        /// </summary>
        /// <param name="roleName">The name of the role to be returned.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the role 
        /// associated with the specified <paramref name="roleName"/>
        /// </returns>
        public virtual Task<Role> FindByNameAsync(string roleName)
        {
            ThrowIfDisposed();
            roleName.CheakArgument();
            return Store.FindByNameAsync(NormalizeKey(roleName), CancellationToken);
        }

        /// <summary>
        /// Adds a claim to a role.
        /// </summary>
        /// <param name="role">The role to add the claim to.</param>
        /// <param name="claim">The claim to add.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<IdentityResult> AddClaimAsync(Role role, Claim claim)
        {
            ThrowIfDisposed();
            var claimStore = GetClaimStore();
            claim.CheakArgument();
            role.CheakArgument();
            await claimStore.AddClaimAsync(role, claim, CancellationToken);
            return await UpdateRoleAsync(role);
        }

        /// <summary>
        /// Removes a claim from a role.
        /// </summary>
        /// <param name="role">The role to remove the claim from.</param>
        /// <param name="claim">The claim to remove.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/>
        /// of the operation.
        /// </returns>
        public virtual async Task<IdentityResult> RemoveClaimAsync(Role role, Claim claim)
        {
            ThrowIfDisposed();
            role.CheakArgument();
            var claimStore = GetClaimStore();
            await claimStore.RemoveClaimAsync(role, claim, CancellationToken);
            return await UpdateRoleAsync(role);
        }

        /// <summary>
        /// Gets a list of claims associated with the specified <paramref name="role"/>.
        /// </summary>
        /// <param name="role">The role whose claims should be returned.</param>
        /// <returns>
        /// The <see cref="Task"/> that represents the asynchronous operation, containing the list of <see cref="Claim"/>s
        /// associated with the specified <paramref name="role"/>.
        /// </returns>
        public virtual Task<IList<Claim>> GetClaimsAsync(Role role)
        {
            ThrowIfDisposed();
            role.CheakArgument();
            var claimStore = GetClaimStore();
            return claimStore.GetClaimsAsync(role, CancellationToken);
        }

      

        /// <summary>
        /// Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
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
        /// Should return <see cref="IdentityResult.Success"/> if validation is successful. This is
        /// called before saving the role via Create or Update.
        /// </summary>
        /// <param name="role">The role</param>
        /// <returns>A <see cref="IdentityResult"/> representing whether validation was successful.</returns>
        protected virtual async Task<IdentityResult> ValidateRoleAsync(Role role)
        {
            var errors = new List<IdentityError>();
            foreach (var v in RoleValidators)
            {
                var result = await v.ValidateAsync(this, role);
                if (!result.Succeeded)
                {
                    errors.AddRange(result.Errors as List<IdentityError>);
                }
            }
            if (errors.Count > 0)
            {
                Logger.LogWarning(0, "Role {roleId} validation failed: {errors}.", await GetRoleIdAsync(role), string.Join(";", errors.Select(e => e.Code)));
                return IdentityResult.Failed(errors.ToArray()) ;
            }
            return IdentityResult.Success() ;
        }

        /// <summary>
        /// Called to update the role after validating and updating the normalized role name.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns>Whether the operation was successful.</returns>
        protected virtual async Task<IdentityResult> UpdateRoleAsync(Role role)
        {
            var result = await ValidateRoleAsync(role);
            if (!result.Succeeded)
            {
                return result;
            }
            await UpdateRoleNameAsync(role);
            return await Store.UpdateAsync(role, CancellationToken);
        }

        // IRoleClaimStore methods
        private IRoleClaimStore GetClaimStore()
        {
            if (!(Store is IRoleClaimStore cast))
            {
                throw new NotSupportedException(Resources.StoreNotIRoleClaimStore);
            }
            return cast;
        }

    }
}
