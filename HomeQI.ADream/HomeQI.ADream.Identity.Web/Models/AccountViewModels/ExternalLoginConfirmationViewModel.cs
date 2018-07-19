using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.AccountViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class ExternalLoginConfirmationViewModel
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required(ErrorMessage = "你必须输入{0}")]
        [Display(Name = "邮箱地址", AutoGenerateField = true, Description = "邮箱地址", Prompt = "邮箱地址")]
        [EmailAddress(ErrorMessage = "你输入{0}的不正确")]
        public string Email { get; set; }
    }
}
