using HomeQI.ADream.Identity.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.AccountViewModels
{

    /// <summary>
    /// 登录模型
    /// </summary>
    public class LoginViewModel : WithPhone
    {

        /// <summary>
        /// 密码
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "密码", AutoGenerateField = true, Description = "密码", Prompt = "密码")]
        [DataType(DataType.Password)]
        public string Password
        {
            get; set;
        }
        /// <summary>
        /// 记住我
        /// </summary>
        [Display(Name = "记住我", AutoGenerateField = true, Description = "记住我", Prompt = "记住我")]
        public bool RememberMe { get; set; }

        /// <summary>
        /// 图形验证码
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "验证码", AutoGenerateField = true, Description = "验证码", Prompt = "请输入验证码")]
        public string Code { get; set; }

    }
}
