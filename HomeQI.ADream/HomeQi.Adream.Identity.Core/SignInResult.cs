// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using HomeQI.ADream.Infrastructure.Core;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// Represents the result of a sign-in operation.
    /// </summary>
    public class SignInResult : BaseResult<SignInResult, IdentityError>
    {

        /// <summary>
        /// 返回一个标志指示是否试图登录的用户被锁定。
        /// </summary>
        /// <value>True if the user attempting to sign-in is locked out, otherwise false.</value>
        public virtual bool IsLockedOut { get; protected set; }

        /// <summary>
        /// 返回一个标志指示是否试图登录的用户不允许登录。
        /// </summary>
        /// <value>True if the user attempting to sign-in is not allowed to sign-in, otherwise false.</value>
        public virtual bool IsNotAllowed { get; protected set; }

        /// <summary>
        /// 返回一个标志指示用户尝试登录是否需要双因素身份验证。
        /// </summary>
        /// <value>True if the user attempting to sign-in requires two factor authentication, otherwise false.</value>
        public virtual bool RequiresTwoFactor { get; protected set; }
        /// <summary>
        /// Returns a <see cref="SignInResult"/> that represents a sign-in attempt that failed because 
        /// the user was logged out.
        /// </summary>
        /// <returns>A <see cref="SignInResult"/> that represents sign-in attempt that failed due to the
        /// user being locked out.</returns>
        public static SignInResult LockedOut { get; } = new SignInResult { IsLockedOut = true };
        public static SignInResult PhoneNumberConfirmed { get; } = new SignInResult { IsPhoneNumberConfirmed = true };
        public static SignInResult EmailConfirmed { get; } = new SignInResult { IsEmailConfirmed = true };


        /// <summary>
        /// Returns a <see cref="SignInResult"/> that represents a sign-in attempt that failed because 
        /// the user is not allowed to sign-in.
        /// </summary>
        /// <returns>A <see cref="SignInResult"/> that represents sign-in attempt that failed due to the
        /// user is not allowed to sign-in.</returns>
        public static SignInResult NotAllowed { get; } = new SignInResult { IsNotAllowed = true };

        /// <summary>
        /// Returns a <see cref="SignInResult"/> that represents a sign-in attempt that needs two-factor 
        /// authentication.
        /// </summary>
        /// <returns>A <see cref="SignInResult"/> that represents sign-in attempt that needs two-factor
        /// authentication.</returns>
        public static SignInResult TwoFactorRequired { get; } = new SignInResult { RequiresTwoFactor = true };
        public bool IsPhoneNumberConfirmed { get; private set; }
        public bool IsEmailConfirmed { get; private set; }

        /// <summary>
        /// Converts the value of the current <see cref="SignInResult"/> object to its equivalent string representation.
        /// </summary>
        /// <returns>A string representation of value of the current <see cref="SignInResult"/> object.</returns>
        public override string ToString()
        {
            return IsLockedOut ? "被锁定" :
                      IsNotAllowed ? "不允许" :
                      IsEmailConfirmed ? "邮箱需要确认" :
                      IsPhoneNumberConfirmed ? "电话号码需要确认" :
                   RequiresTwoFactor ? "需要二次认证" :
                   Succeeded ? "成功" : "失败";
        }
    }
}