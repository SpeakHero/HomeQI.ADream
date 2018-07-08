using HomeQI.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.Entites
{
    public class User : EntityBase
    {
        [Required(ErrorMessage = "你必须输入{0}")]
        [StringLength(50)]
        [Display(Name = "用户名")]
        /// <summary>
        /// 获取或设置此用户的用户名。
        /// </summary>
        public virtual string UserName
        {
            get;
            set;
        }
        [StringLength(50)]
        [Display(Name = "标准化用户名")]
        /// <summary>
        /// 获取或设置此用户的标准化用户名。
        /// </summary>
        public virtual string NormalizedUserName
        {
            get;
            set;
        }
        [StringLength(50)]
        [Display(Name = "电子邮件地址")]
        /// <summary>
        /// 获取或设置此用户的电子邮件地址。
        /// </summary>
        public virtual string Email
        {
            get;
            set;
        } = string.Empty;
        [RegularExpression(@"[0-2]")]
        public int Sex { get; set; } = 0;
        [Display(Name = "是否激活电子邮件")]
        /// <summary>
        /// 确认电子邮件是否激活
        /// </summary>
        /// <value>如果电子邮件地址已确认，则为true，否则为false。</value>
        public virtual bool EmailConfirmed
        {
            get;
            set;
        } = false;

        /// <summary>
        /// 获取或设置一个腌哈希表示此用户的密码。
        /// </summary>
        public virtual string PasswordHash
        {
            get;
            set;
        } = "qwe123456";

        /// <summary>
        /// 每当用户凭据发生变化（密码更改、登录删除）时必须更改的随机值。
        /// </summary>
        public virtual string SecurityStamp
        {
            get;
            set;
        } = Guid.NewGuid().ToString();
        /// <summary>
        /// 获取或设置用户的电话号码。
        /// </summary>
        [Phone]
        public virtual string PhoneNumber
        {
            get;
            set;
        } = string.Empty;

        /// <summary>
        /// 获取或设置表示用户是否已确认其电话地址的标志。
        /// </summary>
        /// <value>如果电话号码已确认，则值为true，否则为false。</value>
        public virtual bool PhoneNumberConfirmed
        {
            get;
            set;
        } = false;

        /// <summary>
        /// 获取或设置一个标志，指示是否为该用户启用了双因素身份验证。
        /// </summary>
        /// <value>如果启用了2FA真，否则为假。</value>
        public virtual bool TwoFactorEnabled
        {
            get;
            set;
        } = false;

        /// <summary>
        /// 获取或设置UTC在任何用户锁定结束时的日期和时间。
        /// </summary>
        /// <remarks>
        /// 过去的值意味着用户没有被锁定。
        /// </remarks>
        public virtual DateTimeOffset? LockoutEnd
        {
            get;
            set;
        }

        /// <summary>
        ///获取或设置一个标志，指示用户是否可以被锁定。
        /// </summary>
        /// <value>如果用户可以被锁定，则为true，否则为false。</value>
        public virtual bool LockoutEnabled
        {
            get;
            set;
        } = false;

        /// <summary>
        /// 获取或设置当前用户失败的登录尝试次数。
        /// </summary>
        public virtual int AccessFailedCount
        {
            get;
            set;
        } = 0;
        /// <summary>
        /// 安全审计信息
        /// </summary>
        public virtual string SecurityAudit { get; set; } = string.Empty;
        /// <summary>
        /// Initializes a new instance of <see cref="T:HomeQI.ADream.Identity.Validators.IdentityUser`1" />.
        /// </summary>
        public User()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:HomeQI.ADream.Identity.Validators.IdentityUser`1" />.
        /// </summary>
        /// <param name="userName">The user name.</param>
        public User(string userName)
            : this()
        {
            UserName = userName;
        }
        public ICollection<Permission> Permissions { get; set; }
        public ICollection<Role> Roles { get; set; }
        /// <summary>
        /// 返回此用户的用户名。
        /// </summary>
        public override string ToString()
        {
            return UserName;
        }
    }
    public class SecurityAudit
    {
        public DateTime DateTime { get; set; } = DateTime.Now;
        public string ClientInfo { get; set; } = "web client";
        public string Remarks { get; set; } = "登录与。。。";
    }
}
