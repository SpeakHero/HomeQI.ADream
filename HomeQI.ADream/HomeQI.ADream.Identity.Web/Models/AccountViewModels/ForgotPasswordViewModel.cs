using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.AccountViewModels
{
    public class ForgotPasswordViewModel
    {
        [Required(ErrorMessage = "你必须输入{0}")]
        [EmailAddress(ErrorMessage = "你输入{0}的不正确")]
        [Display(Name = "邮箱地址", AutoGenerateField = true, Description = "邮箱地址", Prompt = "邮箱地址")]

        public string Email { get; set; }
    }
}
