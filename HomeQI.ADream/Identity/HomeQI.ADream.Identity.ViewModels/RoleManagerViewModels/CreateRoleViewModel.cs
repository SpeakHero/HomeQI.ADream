using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System;
namespace HomeQI.ADream.Identity.ViewModels.RoleManagerViewModels
{
    /// <summary>
    /// 创建角色
    /// </summary>
    public class CreateRoleDto
    {
        /// <summary>
        /// 角色名称
        /// </summary>
        [Display(Name = "角色名称", Prompt = "角色名称", Description = "角色名称")]
        [Remote("CheackNameExist", "RoleManager", areaName: "Admin", ErrorMessage = "{0}已存在")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "你必须输入{0}")]
        public string Name { get; set; }
        [Display(Name = "角色标准名称", Prompt = "角色标准名称", Description = "角色标准名称")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "你必须输入{0}")]
        public string NormalizedName { get; set; }
        /// <summary>
        /// 备注说明"
        /// </summary>
        [Display(Name = "备注说明", AutoGenerateField = true, Description = "备注说明", Prompt = "备注说明")]
        public string Description { get; set; }
    }

    public class EditRoleDto : CreateRoleDto
    {
        /// <summary>
        /// 并发印章
        /// </summary>
        [Display(Name = "并发印章", Prompt = "并发印章", Description = "并发印章")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "你必须输入{0}")]
        public string ConcurrencyStamp { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        /// </summary>
        [Display(Name = "编号", Prompt = "编号", Description = "编号")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "你必须输入{0}")]
        public string Id { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        [Display(Name = "时间戳", Prompt = "时间戳", Description = "时间戳")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "你必须输入{0}")]
        public DateTime EditedTime { get; set; }

    }

    public class DeleteRoleViewModel
    {
        /// <summary>
        /// 并发印章
        /// </summary>
        [Display(Name = "并发印章", Prompt = "并发印章", Description = "并发印章")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "你必须输入{0}")]
        public string ConcurrencyStamp { get; set; }
        /// <summary>
        /// 编号
        /// </summary>
        /// </summary>
        [Display(Name = "编号", Prompt = "编号", Description = "编号")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "你必须输入{0}")]
        public string Id { get; set; }
        /// <summary>
        /// 时间戳
        /// </summary>
        [Display(Name = "时间戳", Prompt = "时间戳", Description = "时间戳")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "你必须输入{0}")]
        public DateTime EditedTime { get; set; }
    }
}
