using HomeQI.ADream.Identity.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.AccountViewModels
{
    /// <summary>
    /// 通过邮箱密码重置模型
    /// </summary>
    public class ResetPasswordViewModel : WithPhone, IClientModelValidator
    {

        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [StringLength(100, ErrorMessage = " {0}必须至少为{2}，并在最大{1}字符长.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码", AutoGenerateField = true, Description = "新密码", Prompt = "新密码")]

        public string Password { get; set; }
        /// <summary>
        /// 密码确认
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "密码确认", AutoGenerateField = true, Description = "密码确认", Prompt = "密码确认")]

        [Compare("Password", ErrorMessage = "两次确认密码不正确.")]
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [EmailAddress(ErrorMessage = "你输入的{0}不正确")]
        [Display(Name = "验证码", AutoGenerateField = true, Description = "验证码", Prompt = "验证码")]
        public string Code { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void AddValidation(ClientModelValidationContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullEx(nameof(context));
            }
        }
    }
}
