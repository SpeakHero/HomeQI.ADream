using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.AccountViewModels
{
    public class ResetPasswordViewModel
    {
        [Required(ErrorMessage = "你必须填写{0}")]
        [EmailAddress(ErrorMessage = "你输入的{0}不正确")]
        [Display(Name = "邮箱地址", AutoGenerateField = true, Description = "邮箱地址", Prompt = "邮箱地址")]

        public string Email { get; set; }

        [Required(ErrorMessage = "你必须填写{0}")]
        [StringLength(100, ErrorMessage = " {0}必须至少为{2}，并在最大{1}字符长.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码", AutoGenerateField = true, Description = "新密码", Prompt = "新密码")]

        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码确认", AutoGenerateField = true, Description = "密码确认", Prompt = "密码确认")]

        [Compare("Password", ErrorMessage = "两次确认密码不正确.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }
}
