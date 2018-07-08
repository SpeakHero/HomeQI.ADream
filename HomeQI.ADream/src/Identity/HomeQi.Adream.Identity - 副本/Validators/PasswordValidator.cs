// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Manager;
using HomeQI.ADream.Infrastructure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Validators

{
    /// <summary>
    /// 为身份提供默认密码策略。
    /// </summary>
    /// <typeparam name="TUser">The type that represents a user.</typeparam>
    public class PasswordValidator : IPasswordValidator
    {
        /// <summary>
        /// Constructions a new instance of <see cref="PasswordValidator"/>.
        /// </summary>
        /// <param name="errors">The <see cref="IdentityErrorDescriber"/> to retrieve error text from.</param>
        public PasswordValidator(IdentityErrorDescriber errors = null)
        {
            Describer = errors ?? new IdentityErrorDescriber();
        }

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to supply error text.
        /// </summary>
        /// <value>The <see cref="IdentityErrorDescriber"/> used to supply error text.</value>
        public IdentityErrorDescriber Describer { get; private set; }

        /// <summary>
        /// Validates a password as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager"/> to retrieve the <paramref name="user"/> properties from.</param>
        /// <param name="user">The user whose password should be validated.</param>
        /// <param name="password">The password supplied for validation</param>
        /// <returns>The task object representing the asynchronous operation.</returns>
        public virtual Task<IdentityResult> ValidateAsync(IUserManager manager, User user, string password)
        {
            password.CheakArgument();
            manager.CheakArgument();
            var errors = new List<IdentityError>();
            var options = manager.Options.Password;
            if (string.IsNullOrWhiteSpace(password) || password.Length < options.RequiredLength)
            {
                errors.Add(Describer.PasswordTooShort(options.RequiredLength));
            }
            if (options.RequireNonAlphanumeric && password.All(IsLetterOrDigit))
            {
                errors.Add(Describer.PasswordRequiresNonAlphanumeric() );
            }
            if (options.RequireDigit && !password.Any(IsDigit))
            {
                errors.Add(Describer.PasswordRequiresDigit() );
            }
            if (options.RequireLowercase && !password.Any(IsLower))
            {
                errors.Add(Describer.PasswordRequiresLower() );
            }
            if (options.RequireUppercase && !password.Any(IsUpper))
            {
                errors.Add(Describer.PasswordRequiresUpper() );
            }
            if (options.RequiredUniqueChars >= 1 && password.Distinct().Count() < options.RequiredUniqueChars)
            {
                errors.Add(Describer.PasswordRequiresUniqueChars(options.RequiredUniqueChars) );
            }
            var   result = errors.Count == 0
                    ? IdentityResult.Success(errors.Count)
                    : IdentityResult.Failed(errors.ToArray());
            return Task.FromResult(result);
        }

        /// <summary>
        /// 返回一个标志，表示所提供的字符是否为数字。
        /// </summary>
        /// <param name="c">The character to check if it is a digit.</param>
        /// <returns>True if the character is a digit, otherwise false.</returns>
        public virtual bool IsDigit(char c)
        {
            return c >= '0' && c <= '9';
        }

        /// <summary>
        ///返回一个标志，该符号指示所提供的字符是否为小写字母ASCII字母。
        /// </summary>
        /// <param name="c">The character to check if it is a lower case ASCII letter.</param>
        /// <returns>True if the character is a lower case ASCII letter, otherwise false.</returns>
        public virtual bool IsLower(char c)
        {
            return c >= 'a' && c <= 'z';
        }

        /// <summary>
        /// 返回一个标志，该符号指示所提供的字符是否为大写字母ASCII字母。
        /// </summary>
        /// <param name="c">The character to check if it is an upper case ASCII letter.</param>
        /// <returns>True if the character is an upper case ASCII letter, otherwise false.</returns>
        public virtual bool IsUpper(char c)
        {
            return c >= 'A' && c <= 'Z';
        }

        /// <summary>
        /// 返回一个标志，表示所提供的字符是否为ASCII字母或数字。
        /// </summary>
        /// <param name="c">The character to check if it is an ASCII letter or digit.</param>
        /// <returns>True if the character is an ASCII letter or digit, otherwise false.</returns>
        public virtual bool IsLetterOrDigit(char c)
        {
            return IsUpper(c) || IsLower(c) || IsDigit(c);
        }
    }
}