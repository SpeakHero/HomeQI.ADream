using HomeQI.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using System.Text;

namespace HomeQI.ADream.Identity.Entites
{
    public class UserClaim : EntityBase
    {

        [Required(ErrorMessage = "你必须输入{0}")]
        [Display(Name = "用户的主键")]
        /// <summary>
        /// 获取或设置与此请求相关联的用户的主键。
        /// </summary>
        public virtual string UserId
        {
            get;
            set;
        }
        [Required(ErrorMessage = "你必须输入{0}")]
        [Display(Name = "控制器id")]
        public virtual string PermissionId
        {
            get;
            set;
        }
        /// <summary>
        ///获取或设置此声明的声明类型。
        /// </summary>
        [Required(ErrorMessage = "你必须输入{0}")]
        [Display(Name = "获取或设置此声明的声明类型")]
        public virtual string ClaimType
        {
            get;
            set;
        }

        /// <summary>
        ///获取或设置此声明的声明值。 区域名称、控制器名称、方法名称组成
        /// </summary>
        [Required(ErrorMessage = "你必须输入{0}")]
        [Display(Name = "获取或设置此声明的声明值。 区域名称、控制器名称、方法名称组成")]
        public virtual string ClaimValue
        {
            get;
            set;
        }

        /// <summary>
        /// 将实体转换为声明实例。
        /// </summary>
        /// <returns></returns>
        public virtual Claim ToClaim()
        {
            return new Claim(ClaimType, ClaimValue);
        }

        /// <summary>
        /// Reads the type and value from the Claim.
        /// </summary>
        /// <param name="claim"></param>
        public virtual void InitializeFromClaim(Claim claim)
        {
            ClaimType = claim.Type;
            ClaimValue = claim.Value;
        }

        [Display(Name = "参数正则表达式")]
        public virtual string Regular { get; set; }
        [Display(Name = "是否允许")]
        public bool IsEnable { get; set; } = true;
        public ICollection<User> User { get; set; }
        public UserClaim()
        {
            ClaimValue = ClaimValue ?? PermissionId;
        }
    }
}
