using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.ViewModels
{
    public class BaseVIewModel
    {
        /// <summary>
        /// 编号
        /// </summary>
        public string Id { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        public byte[] EditedTime { get; set; }
        /// <summary>
        /// 备注说明"
        /// </summary>
        [Display(Name = "备注说明", AutoGenerateField = true, Description = "备注说明", Prompt = "备注说明")]
        public string Description { get; set; }
    }
}
