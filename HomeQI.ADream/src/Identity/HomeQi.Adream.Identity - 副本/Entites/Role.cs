using HomeQI.ADream.Entities.Framework;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.Entites
{
    public class Role: EntityBase
    {
        [Required(ErrorMessage = "你必须输入{0}")]
        [Display(Name = "角色的名称")]
        /// <summary>
        /// 获取或设置此角色的名称。
        /// </summary>
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:HomeQI.ADream.Identity.Validators.IdentityRole`1" />.
        /// </summary>
        public Role()
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="T:HomeQI.ADream.Identity.Validators.IdentityRole`1" />.
        /// </summary>
        /// <param name="roleName">The role name.</param>
        public Role(string roleName)
            : this()
        {
            Name = roleName;
        }

        /// <summary>
        /// 返回角色的名称。
        /// </summary>
        /// <returns>The name of the role.</returns>
        public override string ToString()
        {
            return Name;
        }
        public ICollection<RoleClaim> Permissions { get; set; }
        public string NormalizedName { get; set; }
    }
}
