using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.ManageViewModels
{
    /// <summary>
    ///绑定手机号
    /// </summary>
    public class AddPhoneNumberViewModel
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [Phone(ErrorMessage = "你输入的{0}不正确")]
        [Display(Name = "手机号码", AutoGenerateField = true, Description = "手机号码", Prompt = "手机号码")]
        public string PhoneNumber { get; set; }
    }

    /// <summary>
    /// 绑定电子邮箱
    /// </summary>
    public class AdEmailAddressViewModel
    {
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required(ErrorMessage = "你必须填写{0}")]
        [EmailAddress(ErrorMessage = "你输入的{0}不正确")]
        [Display(Name = "邮箱地址", AutoGenerateField = true, Description = "邮箱地址", Prompt = "邮箱地址")]
        public string EmailAddress { get; set; }
    }
}
