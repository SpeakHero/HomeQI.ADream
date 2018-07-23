using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.AccountViewModels
{
    public class VerifyCodeViewModel
    {
        [Required(ErrorMessage = "你输入的{0}不正确")]
        public string Provider { get; set; }
        [Display(Name = "验证码", AutoGenerateField = true, Description = "验证码", Prompt = "验证码")]

        [Required(ErrorMessage = "你必须输入{0}")]
        public string Code { get; set; }

        public string ReturnUrl { get; set; }

        [Display(Name = "下次自动登录?", AutoGenerateField = true, Description = "下次自动登录?", Prompt = "下次自动登录?")]

        public bool RememberBrowser { get; set; }
        [Display(Name = "记住我?", AutoGenerateField = true, Description = "记住我?", Prompt = "记住我?")]
        public bool RememberMe { get; set; }
        [RegularExpression(@"^[1]+[3,4,5,7,8]+\d{9}", ErrorMessage = "你输入{0}的不正确")]
        public string Phone { get; set; }
        [EmailAddress(ErrorMessage = "你输入{0}的不正确")]
        public string Email { get; set; }
    }
}
