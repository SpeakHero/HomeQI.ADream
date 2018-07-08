using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQI.ADream.Services
{
    public class AppIdentityErrorDescriber: IdentityErrorDescriber
    {

        /// <summary>
        /// Returns the default <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" />.
        /// </summary>
        /// <returns>The default <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" />.</returns>
        public override IdentityError DefaultError()
        {
            return new IdentityError
            {
                Code = "DefaultError",
                Description = "发生了未知的故障。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a concurrency failure.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a concurrency failure.</returns>
        public override IdentityError ConcurrencyFailure()
        {
            return new IdentityError
            {
                Code = "ConcurrencyFailure",
                Description = "乐观并发失败，对象已被修改。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password mismatch.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password mismatch.</returns>
        public override IdentityError PasswordMismatch()
        {
            return new IdentityError
            {
                Code = "PasswordMismatch",
                Description = "密码不正确。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating an invalid token.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating an invalid token.</returns>
        public override IdentityError InvalidToken()
        {
            return new IdentityError
            {
                Code = "InvalidToken",
                Description = "无效令牌。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a recovery code was not redeemed.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a recovery code was not redeemed.</returns>
        public override IdentityError RecoveryCodeRedemptionFailed()
        {
            return new IdentityError
            {
                Code = "RecoveryCodeRedemptionFailed",
                Description = "验证码验证失败。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating an external login is already associated with an account.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating an external login is already associated with an account.</returns>
        public override IdentityError LoginAlreadyAssociated()
        {
            return new IdentityError
            {
                Code = "LoginAlreadyAssociated",
                Description = "具有此登录名的用户已经存在。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified user <paramref name="userName" /> is invalid.
        /// </summary>
        /// <param name="userName">The user name that is invalid.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified user <paramref name="userName" /> is invalid.</returns>
        public override IdentityError InvalidUserName(string userName)
        {
            return new IdentityError
            {
                Code = "InvalidUserName",
                Description = $"用户名“{userName}”无效，只能包含字母或数字。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is invalid.
        /// </summary>
        /// <param name="email">The email that is invalid.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is invalid.</returns>
        public override IdentityError InvalidEmail(string email)
        {
            return new IdentityError
            {
                Code = "InvalidEmail",
                Description = $"电子邮件*{email}无效。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="userName" /> already exists.
        /// </summary>
        /// <param name="userName">The user name that already exists.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="userName" /> already exists.</returns>
        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError
            {
                Code = "DuplicateUserName",
                Description = $"用户名{userName}已经存在"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is already associated with an account.
        /// </summary>
        /// <param name="email">The email that is already associated with an account.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="email" /> is already associated with an account.</returns>
        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError
            {
                Code = "DuplicateEmail",
                Description = $"电子邮箱{email}已经存在"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="role" /> name is invalid.
        /// </summary>
        /// <param name="role">The invalid role.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specific role <paramref name="role" /> name is invalid.</returns>
        public override IdentityError InvalidRoleName(string role)
        {
            return new IdentityError
            {
                Code = "InvalidRoleName",
                Description = $"无效角色名称{role}"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specified <paramref name="role" /> name already exists.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating the specific role <paramref name="role" /> name already exists.</returns>
        public override IdentityError DuplicateRoleName(string role)
        {
            return new IdentityError
            {
                Code = "DuplicateRoleName",
                Description = $"角色名称{role}已经存在"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user already has a password.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user already has a password.</returns>
        public override IdentityError UserAlreadyHasPassword()
        {
            return new IdentityError
            {
                Code = "UserAlreadyHasPassword",
                Description = "用户已经设置了密码"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating user lockout is not enabled.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating user lockout is not enabled.</returns>
        public override IdentityError UserLockoutNotEnabled()
        {
            return new IdentityError
            {
                Code = "UserLockoutNotEnabled",
                Description = "用户锁定未启用"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user is already in the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user is already in the specified <paramref name="role" />.</returns>
        public override IdentityError UserAlreadyInRole(string role)
        {
            return new IdentityError
            {
                Code = "UserAlreadyInRole",
                Description = $"用户已经在角色“{role}”中。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user is not in the specified <paramref name="role" />.
        /// </summary>
        /// <param name="role">The duplicate role.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a user is not in the specified <paramref name="role" />.</returns>
        public override IdentityError UserNotInRole(string role)
        {
            return new IdentityError
            {
                Code = "UserNotInRole",
                Description = $"用户不在角色“{role}”中。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password of the specified <paramref name="length" /> does not meet the minimum length requirements.
        /// </summary>
        /// <param name="length">The length that is not long enough.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password of the specified <paramref name="length" /> does not meet the minimum length requirements.</returns>
        public override IdentityError PasswordTooShort(int length)
        {
            return new IdentityError
            {
                Code = "PasswordTooShort",
                Description = "密码太短"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password does not meet the minimum number <paramref name="uniqueChars" /> of unique chars.
        /// </summary>
        /// <param name="uniqueChars">The number of different chars that must be used.</param>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password does not meet the minimum number <paramref name="uniqueChars" /> of unique chars.</returns>
        public override IdentityError PasswordRequiresUniqueChars(int uniqueChars)
        {
            return new IdentityError
            {
                Code = "PasswordRequiresUniqueChars",
                Description = $"密码必须至少使用{uniqueChars}不同的字符。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a non-alphanumeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a non-alphanumeric character.</returns>
        public override IdentityError PasswordRequiresNonAlphanumeric()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresNonAlphanumeric",
                Description = "密码必须至少有一个非字母数字字符。"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a numeric character, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a numeric character.</returns>
        public override IdentityError PasswordRequiresDigit()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresDigit",
                Description ="密码至少需要一个数字"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a lower case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain a lower case letter.</returns>
        public override IdentityError PasswordRequiresLower()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresLower",
                Description = "密码至少需要一个小写字符"
            };
        }

        /// <summary>
        /// Returns an <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain an upper case letter, which is required by the password policy.
        /// </summary>
        /// <returns>An <see cref="T:Microsoft.AspNetCore.Identity.IdentityError" /> indicating a password entered does not contain an upper case letter.</returns>
        public override IdentityError PasswordRequiresUpper()
        {
            return new IdentityError
            {
                Code = "PasswordRequiresUpper",
                Description = "密码至少需要一个大写字符"
            };
        }
    }
}
