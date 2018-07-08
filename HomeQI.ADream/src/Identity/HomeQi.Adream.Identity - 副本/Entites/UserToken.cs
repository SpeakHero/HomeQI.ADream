using HomeQI.ADream.Entities.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeQI.ADream.Identity.Entites
{

    /// <summary>
    /// 表示用户的身份验证令牌。
    /// </summary>
    public class UserToken : EntityBase
    {
        /// <summary>
        /// 获取或设置令牌所属的用户的主键。
        /// </summary>
        [Required]
        public virtual string UserId
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置loginprovider这个令牌是从。
        /// </summary>
        [Required]
        public virtual string LoginProvider
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置令牌的名称。
        /// </summary>
        [Required]
        public virtual string Name
        {
            get;
            set;
        }

        /// <summary>
        ///获取或设置令牌值。
        /// </summary>
        [Required]
        public virtual string Value
        {
            get;
            set;
        }
        public User User { get; set; }
    }
}
