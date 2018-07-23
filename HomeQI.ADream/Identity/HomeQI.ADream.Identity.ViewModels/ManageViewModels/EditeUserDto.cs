using System;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.ViewModels.ManageViewModels
{
    public class EditeUserDto
    {
        [Timestamp]
        [Display(Name = "最后修改时间")]
        public virtual DateTime EditedTime { get; set; } = DateTime.Now;

        [Required(ErrorMessage = "你必须填写{0}")]
        [Display(Name = "编号")]
        public string Id { get; set; }
        /// <summary>
        /// 每当用户坚持到商店时，必须改变的随机值。
        /// </summary>
        [Required]
        public virtual string ConcurrencyStamp { get; set; }
    }
}
