using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.ManageViewModels
{
    /// <summary>
    /// 修改密码
    /// </summary>
    public class ChangePasswordViewModel
    {
        /// <summary>
        /// 旧密码
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [DataType(DataType.Password)]
        [Display(Name = "旧密码", AutoGenerateField = true, Description = "旧密码", Prompt = "旧密码")]
        public string OldPassword { get; set; }
        /// <summary>
        /// 新密码
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [StringLength(100, ErrorMessage = " {0}必须至少为{2}，并在最大{1}字符长.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "新密码", AutoGenerateField = true, Description = "新密码", Prompt = "新密码")]
        public string NewPassword { get; set; }
        /// <summary>
        /// 确认密码
        /// </summary>
        [DataType(DataType.Password)]
        [Display(Name = "确认密码", AutoGenerateField = true, Description = "确认密码", Prompt = "确认密码")]
        [Compare("NewPassword", ErrorMessage = "新密码和确认密码不匹配.")]
        public string ConfirmPassword { get; set; }
    }
}
