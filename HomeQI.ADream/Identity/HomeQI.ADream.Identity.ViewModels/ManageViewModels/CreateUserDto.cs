using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.ViewModels.ManageViewModels
{
    public class CreateUserDto
    {

        [Display(Name = "备注说明")]
        public virtual string Description { get; set; } = string.Empty;
        /// <summary>
        /// Gets or sets the user name for this user.
        /// </summary>
        [Display(Name = "用户名")]
        [Required(ErrorMessage = "你必须填写{0}")]
        public virtual string UserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the normalized user name for this user.
        /// </summary>
        [Display(Name = "用户名标准名称")]
        [Required(ErrorMessage = "你必须填写{0}")]
        public virtual string NormalizedUserName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email address for this user.
        /// </summary>
        [Display(Name = "邮箱地址")]
        [EmailAddress(ErrorMessage = "{0}不正确")]
        [Required(ErrorMessage = "你必须填写{0}")]
        public virtual string Email { get; set; } = string.Empty;

        /// <summary>
        /// 获取或设置此用户的规范化电子邮件地址。
        /// </summary>
        [Display(Name = "邮箱地址标准名称")]
        [EmailAddress(ErrorMessage = "{0}不正确")]
        [Required(ErrorMessage = "你必须填写{0}")]
        public virtual string NormalizedEmail { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their email address.
        /// </summary>
        /// <value>True if the email address has been confirmed, otherwise false.</value>
        public virtual bool EmailConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a telephone number for the user.
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [Phone(ErrorMessage = "你输入的{0}不正确")]
        [Display(Name = "手机号码", AutoGenerateField = true,
            Description = "手机号码", Prompt = "手机号码")]
        public virtual string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a flag indicating if a user has confirmed their telephone address.
        /// </summary>
        /// <value>True if the telephone number has been confirmed, otherwise false.</value>
        public virtual bool PhoneNumberConfirmed { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if two factor authentication is enabled for this user.
        /// </summary>
        /// <value>True if 2fa is enabled, otherwise false.</value>
        public virtual bool TwoFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets a flag indicating if the user could be locked out.
        /// </summary>
        /// <value>True if the user could be locked out, otherwise false.</value>
        public virtual bool LockoutEnabled { get; set; }
    }
}
