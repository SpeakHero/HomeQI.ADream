using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.ViewModels.RoleManagerViewModels
{
    public class AddRoleClaimDto
    {
        [Display(Name = "模块编号", Prompt = "模块编号", Description = "模块编号")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "你必须输入{0}")]
        public string Id { get; set; }
        [Display(Name = "角色编号", Prompt = "角色编号", Description = "角色编号")]
        [Required(AllowEmptyStrings = false, ErrorMessage = "你必须输入{0}")]
        public string RoleId { get; set; }
    }
}
