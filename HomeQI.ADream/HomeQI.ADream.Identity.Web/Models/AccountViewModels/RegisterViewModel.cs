using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.AccountViewModels
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "你必须输入{0}")]
        [EmailAddress(ErrorMessage = "你输入{0}的不正确")]
        [Display(Name = "邮箱地址", AutoGenerateField = true, Description = "邮箱地址", Prompt = "邮箱地址")]

        public string Email { get; set; }
        [Phone(ErrorMessage = "你输入{0}的不正确")]
        [DataType(DataType.PhoneNumber,ErrorMessage ="你必须输入你的手机号码")]
        [Display(Name = "手机号码", AutoGenerateField = true, Description = "手机号码", Prompt = "手机号码")]
        public string Phone { get; set; }
        [Required(ErrorMessage = "你必须输入{0}")]
        [StringLength(100, ErrorMessage = " {0}必须至少为{2}，并在最大{1}字符长.", MinimumLength = 6)]

        [DataType(DataType.Password)]
        [Display(Name = "密码", AutoGenerateField = true, Description = "密码", Prompt = "密码")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "密码确认", AutoGenerateField = true, Description = "密码确认", Prompt = "密码确认")]

        [Compare("Password", ErrorMessage = "新密码和确认密码不匹配.")]
        public string ConfirmPassword { get; set; }
    }
}
