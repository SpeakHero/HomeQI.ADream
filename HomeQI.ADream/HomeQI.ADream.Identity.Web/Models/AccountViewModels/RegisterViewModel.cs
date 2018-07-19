using HomeQI.ADream.Identity.Models;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;

namespace HomeQI.ADream.Identity.AccountViewModels
{
    /// <summary>
    /// 注册模型
    /// </summary>
    public class RegisterViewModel : WithPhone, IValidatableObject
    {

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "你必须输入{0}")]
        [StringLength(100, ErrorMessage = " {0}必须至少为{2}，并在最大{1}字符长.", MinimumLength = 6)]

        [DataType(DataType.Password)]
        [Display(Name = "密码", AutoGenerateField = true, Description = "密码", Prompt = "密码")]
        public string Password { get; set; }
        /// <summary>
        /// 密码确认
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "密码确认", AutoGenerateField = true, Description = "密码确认", Prompt = "密码确认")]

        [Compare("Password", ErrorMessage = "新密码和确认密码不匹配.")]
        public string ConfirmPassword { get; set; }
        /// <summary>
        /// 图像验证码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 随机验证码
        /// </summary>
        public string XCode { get; set; }

    }
}
