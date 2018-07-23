using HomeQI.ADream.Entities.Framework;
using System;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.Adream.Identity
{
    public class IdentityPermission : IdentityPermission<string>
    {
        public IdentityPermission()
        {
            Id = Guid.NewGuid().ToString();
        }
    }

    public class IdentityPermission<TKey> : EntityBase<TKey> where TKey : IEquatable<TKey>
    {
        /// <summary>
        /// 控制器名
        /// </summary>

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "控制器名")]
        public string Controller { get; set; }

        /// <summary>
        /// 方法名
        /// </summary>

        [Required(AllowEmptyStrings = false)]
        [Display(Name = "方法名")]
        public string Action { get; set; }

        /// <summary>
        /// 参数字符串
        /// </summary>
        [Required]
        [Display(Name = "参数字符串")]
        public string Params { get; set; } = string.Empty;
        /// <summary>
        /// 是否允许匿名登录
        /// </summary>
        [Display(Name = "是否允许匿名访问")]
        public bool IsAllowAnonymous
        {
            get; set;
        } = false;
        /// <summary>
        /// 是否启用
        /// </summary>
        [Display(Name = "是否启用")]
        public bool IsEnable { get; set; } = true;
        [Required]
        [Display(Name = "区域名称")]
        public string AreaName { get; set; } = string.Empty;

        /// <summary>
        /// 显示顺序
        /// </summary>
        [Display(Name = "显示顺序")]

        public int ShowSort { get; set; } = 0;

        /// <summary>
        /// 级别 0为page 1为button
        /// </summary>
        public int Level { get; set; } = 0;
    }

    enum Levels
    {
        Page, Button
    }
}
