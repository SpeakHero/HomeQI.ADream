using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.AccountViewModels
{
    /// <summary>
    /// 找回密码
    /// </summary>
    public class ForgotPasswordViewModel : IValidatableObject
    {
        /// <summary>
        /// 电话号码
        /// </summary>
        [Phone(ErrorMessage = "你输入{0}的不正确")]
        [Display(Name = "电话号码", AutoGenerateField = true, Description = "电话号码", Prompt = "电话号码")]

        public string Phone { get; set; }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [EmailAddress(ErrorMessage = "你输入{0}的不正确")]
        [Display(Name = "邮箱地址", AutoGenerateField = true, Description = "邮箱地址", Prompt = "邮箱地址")]

        public string Email { get; set; }
        /// <summary>
        /// 是否是手机模式
        /// </summary>
        public bool ByPhone { get; set; } = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!ByPhone)
            {
                yield return new ValidationResult("你必须输入{0}", new string[] { nameof(Email) });
            }
            yield return new ValidationResult("你必须输入{0}", new string[] { nameof(Phone) });
        }
    }
}
