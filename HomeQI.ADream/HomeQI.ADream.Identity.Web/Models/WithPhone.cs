using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.Models
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class WithPhone : IValidatableObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        public string GetDisplayAttributeName(string propertyName)
        {
            var propertyInfo = GetType().GetProperty(propertyName);
            object[] attrs = propertyInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true);
            return (attrs[0] as DisplayNameAttribute).DisplayName;
        }
        /// <summary>
        /// 邮箱地址
        /// </summary>
        [EmailAddress(ErrorMessage = "你输入{0}的不正确")]
        [Display(Name = "邮箱地址", AutoGenerateField = true, Description = "邮箱地址", Prompt = "邮箱地址")]
        public virtual string Email { get; set; }
        /// <summary>
        /// 手机号码
        /// </summary>
        [Phone(ErrorMessage = "你输入{0}的不正确")]

        [DataType(DataType.PhoneNumber, ErrorMessage = "你必须输入你的手机号码")]
        [Display(Name = "手机号码", AutoGenerateField = true, Description = "手机号码", Prompt = "手机号码")]
        public virtual string Phone { get; set; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="validationContext"></param>
        /// <returns></returns>
        public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            List<ValidationResult> validationResults = new List<ValidationResult>();
            if (!ByPhone)
            {
                Name = Email;
                if (Email.IsNullOrEmpty())
                {
                    validationResults.Add(new ValidationResult($"你必须输入邮箱地址", new string[] { nameof(Email) }));
                }
                return validationResults;
            }
            Name = Phone;
            if (Phone.IsNullOrEmpty())
            {
                validationResults.Add(new ValidationResult($"你必须输入手机号码", new string[] { nameof(Phone) }));
            }
            //    [RegularExpression(@"^[1]+[3,4,5,7,8]+\d{9}", ErrorMessage = "你输入的{0}不正确")]
            if (Phone.IsNotNullOrEmpty())
            {
                if (!Phone.IsPhone())
                {
                    validationResults.Add(new ValidationResult($"你输入的手机号码不正确", new string[] { nameof(Phone) }));
                }
            }
            return validationResults;
        }

        /// <summary>
        /// 是否已手机号码
        /// </summary>
        public virtual bool ByPhone { get; set; } = false;
        /// <summary>
        /// 系统自动判断
        /// </summary>
        public virtual string Name { get; protected set; }
    }
}
