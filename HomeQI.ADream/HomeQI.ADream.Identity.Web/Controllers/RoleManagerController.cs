using AutoMapper;
using HomeQI.Adream.Identity;
using HomeQI.ADream.Identity.ViewModels.RoleManagerViewModels;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Web.Controllers
{
    /// <summary>
    /// 角色管理
    /// </summary>
    public class RoleManagerController : ApiControllerBase
    {
        private readonly IMapper mapper;
        private readonly AspNetUserManager userManager;
        private readonly AspNetRoleManager roleManager;
        private readonly ILoggerFactory logger;

        /// <summary>
        /// 角色管理
        /// </summary>
        /// <param name="userManager"></param>
        /// <param name="roleManager"></param>
        /// <param name="logger"></param>
        public RoleManagerController(AspNetUserManager userManager, AspNetRoleManager roleManager, ILoggerFactory logger)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.logger = logger;
            mapper = roleManager.Mapper;
        }
        /// <summary>
        /// 角色列表
        /// </summary>
        /// <param name="bootTabPlist"></param>
        /// <returns></returns>
        [Description("角色列表")]
        [HttpPost]
        public async Task<IActionResult> GetRoles([FromBody]BootTabPageListPar bootTabPlist)
        {
            if (!string.IsNullOrEmpty(bootTabPlist.Search))
            {
                if (bootTabPlist?.Filters == null)
                {
                    QueryableFilter[] fiters =
                        {
                            new QueryableFilter()
                        {
                            Condition = Condition.Or,
                            Contract = Contract.Like,
                            Key = "Name",
                            Value = bootTabPlist.Search
                        }
                         };
                    bootTabPlist.Filters = fiters;

                }

            }
            var results = await roleManager.GetListAsync<EditRoleDto>(bootTabPlist);
            return Json(new BootTabPageResult { Page = bootTabPlist.CurrentPage, Total = results.TotalItemCount, Rows = results });
        }
        /// <summary>
        /// 新建角色
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Description("新建角色")]
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Name,Description,NormalizedName")][FromBody]CreateRoleDto model)
        {
            if (ModelState.IsValid)
            {
                var role = mapper.Map<IdentityRole>(model);
                role.CretaedUser = User.GetUserId();
                var result = await roleManager.CreateAsync(role);
                return Json(result);
            }
            ModelState.AddModelError("", "操作失败");
            return Failed();
        }
        /// <summary>
        /// 编辑角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Description("编辑角色")]
        public async Task<IActionResult> Edit([FromRoute] string id, [Bind("Id,EditedTime,Name,Description,NormalizedName,ConcurrencyStamp")][FromBody]EditRoleDto model)
        {
            if (ModelState.IsValid)
            {
                //var result = await roleManager.FindByIdAsync(model.Id);
                //if (result == null)
                //{
                //    return NotFound();
                //}
                //if (result.EditedTime != model.EditedTime || result.ConcurrencyStamp != model.ConcurrencyStamp)
                //{
                //    return Json(ResponseResult.Failed("修改失败，数据已经被修改，请重新获取数据"));
                //}
                if (id != model.Id)
                {
                    return BadRequest();
                }
                var result = mapper.Map<IdentityRole>(model);
                result.EditeUser = User.GetUserId();
                var sult = await roleManager.UpdateAsync(result, nameof(IdentityRole.Description), nameof(IdentityRole.Name), nameof(IdentityRole.NormalizedName), nameof(IdentityRole.ConcurrencyStamp));
                return Json(sult);
            }
            ModelState.AddModelError("", "验证失败");
            return Failed();
        }
        /// <summary>
        /// 检查角色是否存在
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Description("检查角色是否存在")]
        public async Task<IActionResult> CheackNameExist(string name)
        {
            return Json(!await roleManager.RoleExistsAsync(name));
        }
        /// <summary>
        /// 移除角色
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Description("移除角色")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id, [Bind("Id,EditedTime,ConcurrencyStamp")][FromBody]DeleteRoleViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }

            if (model.Id.IsNullOrEmpty())
            {
                return NotFound();
            }

            if (id != model.Id)
            {
                return BadRequest();
            }
            //var role = await roleManager.FindByIdAsync(models.Id);
            //if (role.EditedTime != models.EditedTime)
            //{
            //    return Json(ResponseResult.Failed("并发失败，数据已经被修改"));
            //}
            var role = mapper.Map<IdentityRole>(model);
            role.EditeUser = User.GetUserId();
            var result = await roleManager.DeleteAsync(role);
            return Json(result);
        }
        /// <summary>
        /// 添加角色权限
        /// </summary>
        /// <param name="id">权限编号</param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Description("添加角色权限")]
        [HttpPut("{id}")]
        public async Task<IActionResult> CreateRoleClaimAsync([FromRoute] string id,
            [Bind("Id,RoleId")]
        [FromBody]AddRoleClaimDto model)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            if (id != model.RoleId)
            {
                return BadRequest();
            }
            var result = await roleManager.AddClaimAsync(new
                IdentityRole
            { Id = model.RoleId }, new Claim(model.Id, model.Id));
            return Json(result);
        }
        /// <summary>
        /// 删除角色权限
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [Description("删除角色权限")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoleClaimAsync([FromRoute] string id,
        [Bind("Id,RoleId")]
        [FromBody]AddRoleClaimDto model)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            if (id != model.RoleId)
            {
                return BadRequest();
            }
            var result = await roleManager.RemoveClaimAsync(new
                IdentityRole
            { Id = model.RoleId }, new Claim(model.Id, model.Id));
            return Json(result);
        }
    }
}