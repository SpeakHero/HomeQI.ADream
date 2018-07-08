using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.AccountViewModels
{
    public class LoginViewModel
    {
        [Required(ErrorMessage = "你必须填写{0}")]
        [RegularExpression(@"^[1]+[3,4,5,7,8,9]+\d{9}", ErrorMessage = "你输入{0}的不正确")]
        [Display(Name = "手机号码", AutoGenerateField = true, Description = "手机号码", Prompt = "请输入手机号码")]

        public string Phone { get; set; }

        [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "密码", AutoGenerateField = true, Description = "密码", Prompt = "密码")]
        [DataType(DataType.Password)]
        public string Password
        {
            get; set;
        }

        [Display(Name = "记住我", AutoGenerateField = true, Description = "记住我", Prompt = "记住我")]
        public bool RememberMe { get; set; }
        [EmailAddress(ErrorMessage = "你输入{0}的不正确")]
        // [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "电子邮箱", AutoGenerateField = true, Description = "电子邮箱", Prompt = "请输入电子邮箱")]
        public string Email { get; set; }

        [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "验证码", AutoGenerateField = true, Description = "验证码", Prompt = "请输入验证码")]
        //[Remote("VerifyCodes", "Account", HttpMethod = "post", AdditionalFields = "Provider,__RequestVerificationToken,Email,Phone", ErrorMessage = "{0}错误")]
        public string Code { get; set; }
        [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "用户名", AutoGenerateField = true, Description = "用户名", Prompt = "请输入用户名")]
        public string Name
        {
            get; set;
        }
    }
}
