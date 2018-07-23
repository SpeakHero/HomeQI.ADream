using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.ManageViewModels
{
    /// <summary>
    /// 验证手机
    /// </summary>
    public class VerifyPhoneNumberViewModel
    {
        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "验证码", AutoGenerateField = true, Description = "验证码", Prompt = "验证码")]
        public string Code { get; set; }
        /// <summary>
        /// 手机号码        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [Phone(ErrorMessage = "你输入的{0}不正确")]
        [Display(Name = "手机号码", AutoGenerateField = true, Description = "手机号码", Prompt = "手机号码")]
        public string PhoneNumber { get; set; }
    }
    /// <summary>
    /// 验证邮箱
    /// </summary>
    public class VerifyEmailaddressViewModel
    {
        /// <summary>
        /// 验证码
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "验证码", AutoGenerateField = true, Description = "验证码", Prompt = "验证码")]
        public string Code { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [EmailAddress(ErrorMessage = "你输入的{0}不正确")]
        [Display(Name = "邮箱地址", AutoGenerateField = true, Description = "邮箱地址", Prompt = "邮箱地址")]
        public string Emailaddress { get; set; }
    }
}
