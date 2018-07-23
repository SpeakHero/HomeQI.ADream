﻿//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.42000
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

namespace HomeQIAdream.Identity.Core
{
    using System;


    /// <summary>
    ///   一个强类型的资源类，用于查找本地化的字符串等。
    /// </summary>
    // 此类是由 StronglyTypedResourceBuilder
    // 类通过类似于 ResGen 或 Visual Studio 的工具自动生成的。
    // 若要添加或移除成员，请编辑 .ResX 文件，然后重新运行 ResGen
    // (以 /str 作为命令选项)，或重新生成 VS 项目。
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "15.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources
    {

        private static global::System.Resources.ResourceManager resourceMan;

        private static global::System.Globalization.CultureInfo resourceCulture;

        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources()
        {
        }
        private static string GetString(string name, params string[] formatterNames)
        {
            var value = resourceMan.GetString(name);

            System.Diagnostics.Debug.Assert(value != null);

            if (formatterNames != null)
            {
                for (var i = 0; i < formatterNames.Length; i++)
                {
                    value = value.Replace("{" + formatterNames[i] + "}", "{" + i + "}");
                }
            }

            return value;
        }
        /// <summary>
        ///   返回此类使用的缓存的 ResourceManager 实例。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager
        {
            get
            {
                if (object.ReferenceEquals(resourceMan, null))
                {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("HomeQIAdream.Identity.Core.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }

        /// <summary>
        ///   使用此强类型资源类，为所有资源查找
        ///   重写当前线程的 CurrentUICulture 属性。
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture
        {
            get
            {
                return resourceCulture;
            }
            set
            {
                resourceCulture = value;
            }
        }

        /// <summary>
        ///   查找类似 乐观并发失败，对象已被修改。 的本地化字符串。
        /// </summary>
        internal static string ConcurrencyFailure
        {
            get
            {
                return ResourceManager.GetString("ConcurrencyFailure", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 发生了未知的故障。 的本地化字符串。
        /// </summary>
        internal static string DefaultError
        {
            get
            {
                return ResourceManager.GetString("DefaultError", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 电子邮件“{0}”已经被使用。 的本地化字符串。
        /// </summary>
        internal static string DuplicateEmail
        {
            get
            {
                return ResourceManager.GetString("DuplicateEmail", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 手机号码“{0}”已经存在 的本地化字符串。
        /// </summary>
        internal static string DuplicatePhone
        {
            get
            {
                return ResourceManager.GetString("DuplicatePhone", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 角色名称&quot;{0}&quot;已经被使用 的本地化字符串。
        /// </summary>
        internal static string DuplicateRoleName
        {
            get
            {
                return ResourceManager.GetString("DuplicateRoleName", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 用户名称&quot;{0}&quot;已经被使用 的本地化字符串。
        /// </summary>
        internal static string DuplicateUserName
        {
            get
            {
                return ResourceManager.GetString("DuplicateUserName", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 电子邮件“{0}”是无效的。 的本地化字符串。
        /// </summary>
        internal static string InvalidEmail
        {
            get
            {
                return ResourceManager.GetString("InvalidEmail", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 类型{0}必须从{1} &lt;{2}&gt;派生。 的本地化字符串。
        /// </summary>
        internal static string InvalidManagerType
        {
            get
            {
                return ResourceManager.GetString("InvalidManagerType", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 提供的PasswordHasherCompatibilityMode无效。 的本地化字符串。
        /// </summary>
        internal static string InvalidPasswordHasherCompatibilityMode
        {
            get
            {
                return ResourceManager.GetString("InvalidPasswordHasherCompatibilityMode", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 迭代计数必须是正整数。 的本地化字符串。
        /// </summary>
        internal static string InvalidPasswordHasherIterationCount
        {
            get
            {
                return ResourceManager.GetString("InvalidPasswordHasherIterationCount", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 手机号码格式错误 的本地化字符串。
        /// </summary>
        internal static string InvalidPhone
        {
            get
            {
                return ResourceManager.GetString("InvalidPhone", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 角色名称“{0}”无效 的本地化字符串。
        /// </summary>
        internal static string InvalidRoleName
        {
            get
            {
                return ResourceManager.GetString("InvalidRoleName", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 无效令牌。 的本地化字符串。
        /// </summary>
        internal static string InvalidToken
        {
            get
            {
                return ResourceManager.GetString("InvalidToken", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 用户名“{0}”无效，只能包含字母或数字。 的本地化字符串。
        /// </summary>
        internal static string InvalidUserName
        {
            get
            {
                return ResourceManager.GetString("InvalidUserName", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 具有此登录名的用户已经存在。 的本地化字符串。
        /// </summary>
        internal static string LoginAlreadyAssociated
        {
            get
            {
                return ResourceManager.GetString("LoginAlreadyAssociated", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 必须在服务集合上调用AddIdentity 实体。 的本地化字符串。
        /// </summary>
        internal static string MustCallAddIdentity
        {
            get
            {
                return ResourceManager.GetString("MustCallAddIdentity", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 没有注册IMAPURATATATEPROCTIONER服务，这是在PurtPosialDATABASE=真时需要的。 的本地化字符串。
        /// </summary>
        internal static string NoPersonalDataProtector
        {
            get
            {
                return ResourceManager.GetString("NoPersonalDataProtector", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 没有指定RoleType，尝试AddRoles &lt;ROLE&gt;(）。 的本地化字符串。
        /// </summary>
        internal static string NoRoleType
        {
            get
            {
                return ResourceManager.GetString("NoRoleType", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 没有注册名为“{1}”的IUsReToFraceTokPotoServices &lt;{0}&gt;。 的本地化字符串。
        /// </summary>
        internal static string NoTokenProvider
        {
            get
            {
                return ResourceManager.GetString("NoTokenProvider", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 用户安全标记不能为空。 的本地化字符串。
        /// </summary>
        internal static string NullSecurityStamp
        {
            get
            {
                return ResourceManager.GetString("NullSecurityStamp", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 密码不正确。 的本地化字符串。
        /// </summary>
        internal static string PasswordMismatch
        {
            get
            {
                return ResourceManager.GetString("PasswordMismatch", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 密码必须至少有一个数字。 的本地化字符串。
        /// </summary>
        internal static string PasswordRequiresDigit
        {
            get
            {
                return ResourceManager.GetString("PasswordRequiresDigit", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 密码必须至少有一个小写字母。 的本地化字符串。
        /// </summary>
        internal static string PasswordRequiresLower
        {
            get
            {
                return ResourceManager.GetString("PasswordRequiresLower", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 密码必须至少有一个非字母数字字符。 的本地化字符串。
        /// </summary>
        internal static string PasswordRequiresNonAlphanumeric
        {
            get
            {
                return ResourceManager.GetString("PasswordRequiresNonAlphanumeric", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 密码必须至少使用{0}不同的字符。 的本地化字符串。
        /// </summary>
        internal static string PasswordRequiresUniqueChars
        {
            get
            {
                return ResourceManager.GetString("PasswordRequiresUniqueChars", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 密码必须至少有一个大写字母（‘a’-z’）。 的本地化字符串。
        /// </summary>
        internal static string PasswordRequiresUpper
        {
            get
            {
                return ResourceManager.GetString("PasswordRequiresUpper", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 密码必须至少为需要{0}个字符。 的本地化字符串。
        /// </summary>
        internal static string PasswordTooShort
        {
            get
            {
                return ResourceManager.GetString("PasswordTooShort", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 验证码验证失败。 的本地化字符串。
        /// </summary>
        internal static string RecoveryCodeRedemptionFailed
        {
            get
            {
                return ResourceManager.GetString("RecoveryCodeRedemptionFailed", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 角色{0}不存在 的本地化字符串。
        /// </summary>
        internal static string RoleNotFound
        {
            get
            {
                return ResourceManager.GetString("RoleNotFound", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现 IProtectedUserStore&lt;TUser&gt; 当ProtectPersonalData 真时需要。 的本地化字符串。
        /// </summary>
        internal static string StoreNotIProtectedUserStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIProtectedUserStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现 IQueryableRoleStore&lt;TRole&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIQueryableRoleStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIQueryableRoleStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现IQueryableUserStore&lt;TUser&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIQueryableUserStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIQueryableUserStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现IRoleClaimStore&lt;TRole&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIRoleClaimStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIRoleClaimStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现 IUserAuthenticationTokenStore&lt;User&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserAuthenticationTokenStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserAuthenticationTokenStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现IUserAuthenticatorKeyStore&lt;User&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserAuthenticatorKeyStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserAuthenticatorKeyStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现 IUserClaimStore&lt;TUser&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserClaimStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserClaimStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现 IUserConfirmationStore&lt;TUser&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserConfirmationStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserConfirmationStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现 IUserEmailStore&lt;TUser&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserEmailStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserEmailStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现 IUserLockoutStore&lt;TUser&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserLockoutStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserLockoutStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现 IUserLoginStore&lt;TUser&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserLoginStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserLoginStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现IUserPasswordStore&lt;TUser&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserPasswordStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserPasswordStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现 IUserPhoneNumberStore&lt;TUser&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserPhoneNumberStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserPhoneNumberStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现IUserRoleStore&lt;TUser&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserRoleStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserRoleStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现 IUserSecurityStampStore&lt;TUser&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserSecurityStampStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserSecurityStampStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现IUserTwoFactorRecoveryCodeStore&lt;User&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserTwoFactorRecoveryCodeStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserTwoFactorRecoveryCodeStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 存储不实现 IUserTwoFactorStore&lt;TUser&gt;. 的本地化字符串。
        /// </summary>
        internal static string StoreNotIUserTwoFactorStore
        {
            get
            {
                return ResourceManager.GetString("StoreNotIUserTwoFactorStore", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 用户已经设置了密码。 的本地化字符串。
        /// </summary>
        internal static string UserAlreadyHasPassword
        {
            get
            {
                return ResourceManager.GetString("UserAlreadyHasPassword", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 用户已经在角色“{0}”中。 的本地化字符串。
        /// </summary>
        internal static string UserAlreadyInRole
        {
            get
            {
                return ResourceManager.GetString("UserAlreadyInRole", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 用户被锁定。 的本地化字符串。
        /// </summary>
        internal static string UserLockedOut
        {
            get
            {
                return ResourceManager.GetString("UserLockedOut", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 此用户未启用。 的本地化字符串。
        /// </summary>
        internal static string UserLockoutNotEnabled
        {
            get
            {
                return ResourceManager.GetString("UserLockoutNotEnabled", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 用户{0}不存在 的本地化字符串。
        /// </summary>
        internal static string UserNameNotFound
        {
            get
            {
                return ResourceManager.GetString("UserNameNotFound", resourceCulture);
            }
        }

        /// <summary>
        ///   查找类似 用户不存在角色“{0}”中。 的本地化字符串。
        /// </summary>
        internal static string UserNotInRole
        {
            get
            {
                return ResourceManager.GetString("UserNotInRole", resourceCulture);
            }
        }
    }
}
