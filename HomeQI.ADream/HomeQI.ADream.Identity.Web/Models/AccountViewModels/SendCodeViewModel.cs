using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HomeQI.ADream.Identity.AccountViewModels
{
    /// <summary>
    /// 发送验证码模型
    /// </summary>
    public class SendCodeViewModel
    {
        /// <summary>
        /// 选择手机号码还是邮箱 Phone ,Email
        /// </summary>
        public string SelectedProvider { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public ICollection<SelectListItem> Providers { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ReturnUrl { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public bool RememberMe { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string User { get; set; }
    }
}
