using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.ManageViewModels
{
    public class ConfigureTwoFactorViewModel
    {
        [Display(Name = "验证方式", AutoGenerateField = true, Description = "验证方式", Prompt = "验证方式")]
        [Required(ErrorMessage = "你必须填写{0}")]
        public string SelectedProvider { get; set; }

        public ICollection<SelectListItem> Providers { get; set; }
    }
}
