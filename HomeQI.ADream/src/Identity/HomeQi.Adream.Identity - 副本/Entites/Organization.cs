using HomeQI.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeQI.ADream.Identity.Entites
{
    public class Organization : EntityBase
    {
        [Required(ErrorMessage = "你必须输入{0}")]
        [StringLength(50)]
        [Display(Name = "机构名称")]
        public string Name { get; set; }
        [Required(ErrorMessage = "你必须输入{0}")]
        [StringLength(50)]
        [Display(Name = "机构代码")]
        public string OrgCode { get; set; }
        [Required(ErrorMessage = "你必须输入{0}")]
        [StringLength(50)]
        [Display(Name = "机构地址")]
        public string Address { get; set; }
        [Required(ErrorMessage = "你必须输入{0}")]
        [StringLength(15)]
        [Display(Name = "邮政编码")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "你必须输入{0}")]
        [StringLength(100)]
        [Display(Name = "网络地址")]
        [Url]
        public string WebSite { get; set; } = "";
        [Required(ErrorMessage = "你必须输入{0}")]
        [StringLength(100)]
        [Display(Name = "联系人员")]
        public string Contacts { get; set; } = "";
        [Required(ErrorMessage = "你必须输入{0}")]
        [StringLength(100)]
        [Display(Name = "联系电话")]
        [Phone]
        public string Phone { get; set; } = "";
        [Required(ErrorMessage = "你必须输入{0}")]
        [StringLength(100)]
        [Display(Name = "联系电话")]
        [EmailAddress]
        public string Email { get; set; } = "";
    }
}
