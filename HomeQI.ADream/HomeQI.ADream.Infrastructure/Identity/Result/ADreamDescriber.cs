// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.


namespace HomeQI.ADream.Infrastructure.Identity
{
    /// <summary>
    /// Service to enable localization for application facing identity errors.
    /// </summary>
    /// <remarks>
    /// These errors are returned to controllers and are generally used as display messages to end users.
    /// </remarks>
    public class ADreamDescriber
    {
        /// <summary>
        /// Returns the default <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" />.
        /// </summary>
        /// <returns>The default <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" />.</returns>
        public  ADreamError DefaultError()
        {
            return new ADreamError
            {
                Code = "DefaultError",
                Description = "发生了未知的故障。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a concurrency failure.
        /// </summary>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a concurrency failure.</returns>
        public  ADreamError ConcurrencyFailure()
        {
            return new ADreamError
            {
                Code = "ConcurrencyFailure",
                Description = "乐观并发失败，对象已被修改。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password mismatch.
        /// </summary>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password mismatch.</returns>
        public  ADreamError PasswordMismatch()
        {
            return new ADreamError
            {
                Code = "PasswordMismatch",
                Description = "密码不正确。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating an invalid token.
        /// </summary>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating an invalid token.</returns>
        public  ADreamError InvalidToken()
        {
            return new ADreamError
            {
                Code = "InvalidToken",
                Description = "无效令牌。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a recovery code was not redeemed.
        /// </summary>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a recovery code was not redeemed.</returns>
        public  ADreamError RecoveryCodeRedemptionFailed()
        {
            return new ADreamError
            {
                Code = "RecoveryCodeRedemptionFailed",
                Description = "验证码验证失败。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating an external login is already associated with an account.
        /// </summary>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating an external login is already associated with an account.</returns>
        public  ADreamError LoginAlreadyAssociated()
        {
            return new ADreamError
            {
                Code = "LoginAlreadyAssociated",
                Description = "具有此登录名的用户已经存在。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specified user <paramref name="userName" /> is invalid.
        /// </summary>
        /// <param name="userName">The user name that is invalid.</param>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specified user <paramref name="userName" /> is invalid.</returns>
        public  ADreamError InvalidUserName(string userName)
        {
            return new ADreamError
            {
                Code = "InvalidUserName",
                Description = $"用户名“{userName}”无效，只能包含字母或数字。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is invalid.
        /// </summary>
        /// <param name="email">The email that is invalid.</param>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is invalid.</returns>
        public  ADreamError InvalidEmail(string email)
        {
            return new ADreamError
            {
                Code = "InvalidEmail",
                Description = $"电子邮件*{email}无效。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specified <paramref name="userName" /> already exists.
        /// </summary>
        /// <param name="userName">The user name that already exists.</param>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specified <paramref name="userName" /> already exists.</returns>
        public  ADreamError DuplicateUserName(string userName)
        {
            return new ADreamError
            {
                Code = "DuplicateUserName",
                Description = $"用户名{userName}已经存在"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is already associated with an account.
        /// </summary>
        /// <param name="email">The email that is already associated with an account.</param>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is already associated with an account.</returns>
        public  ADreamError DuplicateEmail(string email)
        {
            return new ADreamError
            {
                Code = "DuplicateEmail",
                Description = $"电子邮箱{email}已经存在"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specified <paramref name="role" /> name is invalid.
        /// </summary>
        /// <param name="role">The invalid role.</param>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specific role <paramref name="role" /> name is invalid.</returns>
        public  ADreamError InvalidRoleName(string role)
        {
            return new ADreamError
            {
                Code = "InvalidRoleName",
                Description = $"无效角色名称{role}"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specified <paramref name="role" /> name already exists.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating the specific role <paramref name="role" /> name already exists.</returns>
        public  ADreamError DuplicateRoleName(string role)
        {
            return new ADreamError
            {
                Code = "DuplicateRoleName",
                Description = $"角色名称{role}已经存在"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a user already has a password.
        /// </summary>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a user already has a password.</returns>
        public  ADreamError UserAlreadyHasPassword()
        {
            return new ADreamError
            {
                Code = "UserAlreadyHasPassword",
                Description = "用户已经设置了密码"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating user lockout is not enabled.
        /// </summary>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating user lockout is not enabled.</returns>
        public  ADreamError UserLockoutNotEnabled()
        {
            return new ADreamError
            {
                Code = "UserLockoutNotEnabled",
                Description = "用户锁定未启用"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a user is already in the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a user is already in the specified <paramref name="role" />.</returns>
        public  ADreamError UserAlreadyInRole(string role)
        {
            return new ADreamError
            {
                Code = "UserAlreadyInRole",
                Description = $"用户已经在角色“{role}”中。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a user is not in the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a user is not in the specified <paramref name="role" />.</returns>
        public  ADreamError UserNotInRole(string role)
        {
            return new ADreamError
            {
                Code = "UserNotInRole",
                Description = $"用户不在角色“{role}”中。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password of the specified <paramref name="length" /> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is not long enough.</param>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password of the specified <paramref name="length" /> does not meet the minimum length requirements.</returns>
        public  ADreamError PasswordTooShort(int length)
        {
            return new ADreamError
            {
                Code = "PasswordTooShort",
                Description = "密码太短"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password does not meet the minimum number <paramref name="uniqueChars" /> of unique chars.
        /// </summary>
        /// <param name="uniqueChars">The number of different chars that must be used.</param>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password does not meet the minimum number <paramref name="uniqueChars" /> of unique chars.</returns>
        public  ADreamError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new ADreamError
            {
                Code = "PasswordRequiresUniqueChars",
                Description = $"密码必须至少使用{uniqueChars}不同的字符。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password entered does not contain a non-alphanumeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password entered does not contain a non-alphanumeric character.</returns>
        public  ADreamError PasswordRequiresNonAlphanumeric()
        {
            return new ADreamError
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "密码必须至少有一个非字母数字字符。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password entered does not contain a numeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password entered does not contain a numeric character.</returns>
        public  ADreamError PasswordRequiresDigit()
        {
            return new ADreamError
            {
                Code = "PasswordRequiresDigit",
                Description = "密码至少需要一个数字"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password entered does not contain a lower case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password entered does not contain a lower case letter.</returns>
        public  ADreamError PasswordRequiresLower()
        {
            return new ADreamError
            {
                Code = "PasswordRequiresLower",
                Description = "密码至少需要一个小写字符"
            };
        }

        /// <summary>
        /// Returns an <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password entered does not contain an upper case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T: HomeQI.ADream.Infrastructure.Identity.IdentityError" /> indicating a password entered does not contain an upper case letter.</returns>
        public  ADreamError PasswordRequiresUpper()
        {
            return new ADreamError
            {
                Code = "PasswordRequiresUpper",
                Description = "密码至少需要一个大写字符"
            };
        }
    }
}