using HomeQI.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeQI.ADream.Identity.Entites
{
    public  class UserRole: EntityBase
    {
        [Required(ErrorMessage = "你必须输入{0}")]
        [Display(Name = "用户的主键")]
        /// <summary>
        /// 获取或设置链接到角色的用户的主键。
        /// </summary>
        public virtual string UserId
        {
            get;
            set;
        }
        [Required(ErrorMessage = "你必须输入{0}")]
        [Display(Name = "角色的主键")]
        /// <summary>
        ///获取或设置链接到用户的角色的主键。
        /// </summary>
        public virtual string RoleId
        {
            get;
            set;
        }
        public virtual User User { get; set; }
        public virtual ICollection<Role> Roles { get; set; }    
    }
}
