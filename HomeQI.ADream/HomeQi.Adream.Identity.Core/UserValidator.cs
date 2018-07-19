// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    ///为用户类提供验证服务。
    /// </summary>
    /// <typeparam name="TUser">The type encapsulating a user.</typeparam>
    public class UserValidator<TUser> : IUserValidator<TUser> where TUser : EntityBase<string>
    {
        /// <summary>
        /// Creates a new instance of <see cref="UserValidator{TUser}"/>/
        /// </summary>
        /// <param name="errors">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
        public UserValidator(IdentityErrorDescriber errors = null)
        {
            Describer = errors ?? new IdentityErrorDescriber();
        }

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.
        /// </summary>
        /// <value>The <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{TUser}"/>.</value>
        public IdentityErrorDescriber Describer { get; private set; }

        /// <summary>
        /// Validates the specified <paramref name="user"/> as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user to validate.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the validation operation.</returns>
        public virtual async Task<IdentityResult> ValidateAsync(UserManager<TUser> manager, TUser user)
        {
            if (manager == null)
            {
                throw new ArgumentNullEx(nameof(manager));
            }
            if (user == null)
            {
                throw new ArgumentNullEx(nameof(user));
            }

            var errors = new List<IdentityError>();
            await ValidateUserNameAsync(manager, user, errors);
            await ValidateEmailAsync(manager, user, errors);
            await ValidatePhoneAsync(manager, user, errors);

            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success();
        }

        protected virtual Task ValidateUserNameAsync(UserManager<TUser> manager, TUser user, ICollection<IdentityError> errors)
        {
            return Task.CompletedTask;
        }
        protected virtual Task ValidatePhoneAsync(UserManager<TUser> manager, TUser user, List<IdentityError> errors)
        {
            return Task.CompletedTask;
        }
        // make sure email is not empty, valid, and unique
        protected virtual Task ValidateEmailAsync(UserManager<TUser> manager, TUser user, List<IdentityError> errors)
        {
            return Task.CompletedTask;
        }
    }
}
