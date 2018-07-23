using AutoMapper;
using HomeQI.Adream.Identity;
using HomeQI.ADream.Identity.ViewModels.PermissionViewModel;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;

namespace HomeQI.ADream.Identity.Web.Controllers
{

    /// <summary>
    /// 系统control and  action
    /// </summary>
    //  [Produces("application/*+json")]
    [ApiController]
    public class PermissionManagerController : ApiControllerBase
    {

        private readonly PermissionManager _permissionManager;
        private readonly IMapper mapper;
        private readonly ILoggerFactory logger;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="permissionManager"></param>
        public PermissionManagerController(ILoggerFactory logger, PermissionManager permissionManager)
        {
            this.logger = logger;
            _permissionManager = permissionManager;
            mapper = _permissionManager.Mapper;
        }

        /// <summary>
        /// 获取模块列表
        /// </summary>
        /// <param name="bootTabPlist"></param>
        /// <returns></returns>
        [Description("获取模块列表")]
        [HttpPost]
        public async Task<IActionResult> GetPermissions(BootTabPageListPar bootTabPlist)
        {
            if (!string.IsNullOrEmpty(bootTabPlist.Search))
            {
                if (bootTabPlist?.Filters == null)
                {

                    QueryableFilter[] fiters = {new QueryableFilter()
                        {
                            Condition = Condition.Or,
                            Contract = Contract.Like,
                            Key = "Action",
                            Value = bootTabPlist.Search
                        },
                          new QueryableFilter()
                        {
                            Condition = Condition.Or,
                            Contract = Contract.Like,
                            Key = "Id",
                            Value = bootTabPlist.Search
                        },
                        new QueryableFilter()
                        {
                            Condition = Condition.Or,
                            Contract = Contract.Like,
                            Key = "Controller",
                            Value = bootTabPlist.Search
                        }
                            ,
                            new QueryableFilter()
                        {
                            Condition = Condition.Or,
                            Contract = Contract.Like,
                            Key = "AreaName",
                            Value = bootTabPlist.Search
                        }
                    };
                    bootTabPlist.Filters = fiters;

                }
            }
            var results = await _permissionManager.GetListAsync<EditPermissionDto>(bootTabPlist);
            return Json(new BootTabPageResult { Page = bootTabPlist.CurrentPage, Total = results.TotalItemCount, Rows = results });

        }
        /// <summary>
        ///  根据编号获取模块
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Description("根据编号获取模块")]
        public async Task<IActionResult> GetPermission([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            var key = nameof(IdentityPermission) + id;
            var editPermissionDto = await GetCacheAsync<EditPermissionDto>(key);
            if (editPermissionDto == null)
            {
                editPermissionDto = await _permissionManager.GetDtos<EditPermissionDto>().
                   FirstOrDefaultAsync(p => p.Id.Equals(id));
                if (editPermissionDto == null)
                {
                    await Cache.RemoveAsync(key);
                    return NotFound();
                }
            }
            return Ok(editPermissionDto);
        }
        /// <summary>
        /// 编辑模块
        /// </summary>
        /// <param name="id"></param>
        /// <param name="editPermission"></param>
        /// <returns></returns>
        // PUT: api/PermissionManager/5
        [HttpPut("{id}")]
        [Description("编辑模块")]
        public async Task<IActionResult> Edit([FromRoute] string id, [FromBody] EditPermissionDto editPermission)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }

            if (id != editPermission.Id)
            {
                return BadRequest();
            }
            IdentityPermission identityPermission = mapper.Map<IdentityPermission>(editPermission);
            var result = await _permissionManager.Store.UpdateAsync(identityPermission, default);
            LogerHelp.Info(identityPermission);
            return Ok(result);
        }
        /// <summary>
        /// 创建模块
        /// </summary>
        /// <param name="creatPermissionDto"></param>
        /// <returns></returns>
        [Description("创建模块")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePermissionDto creatPermissionDto)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            var identityPermission = mapper.Map<IdentityPermission>(creatPermissionDto);
            identityPermission.CretaedUser = User.GetUserId();
            var result = await _permissionManager.Store.CreateAsync(identityPermission, default);
            return Ok(result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // DELETE: api/PermissionManager/5
        [Description("删除模块")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] string id)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }

            var identityPermission = await _permissionManager.Store.FindByIdAsync(id, default);
            if (identityPermission == null)
            {
                return NotFound();
            }
            return Ok(await _permissionManager.Store.DeleteAsync(identityPermission, default));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetAllAction()
        {
            return Json(PermissionManager.GetAllActionByAssembly());
        }
        /// <summary>
        /// 修改模块是否需要登录后操作
        /// </summary>
        /// <param name="id">模块编号</param>
        /// <param name="editetime">时间戳</param>
        /// <param name="enable">默认是true</param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Description("修改模块是否需要登录后操作")]
        public async Task<IActionResult> EditAllowAnonymous([FromRoute][Required]string id, [Required]DateTime editetime, bool enable = true)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            IdentityPermission identityPermission = new IdentityPermission
            {
                Id = id,
                EditedTime = editetime
            };
            return Json(await _permissionManager.EditAllowAnonymousAsync(identityPermission, enable));
        }
        /// <summary>
        /// 修改模块是否启用或者禁用
        /// </summary>
        /// <param name="id">模块编号</param>
        /// <param name="editetime">时间戳</param>
        /// <param name="enable">默认是true</param>
        /// <returns></returns>
        [Description("修改模块是否启用或者禁用")]
        [HttpPut("{id}")]
        public async Task<IActionResult> EditEnable([FromRoute][Required]string id, [Required]DateTime editetime, bool enable = true)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            IdentityPermission identityPermission = new IdentityPermission
            {
                Id = id,
                EditedTime = editetime
            };
            return Json(await _permissionManager.EditEnable(identityPermission, enable));

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> DeleteAllActionAsync()
        {
            var p = _permissionManager.Store.EntitySet;
            _permissionManager.Store.AutoSaveChanges = false;
            foreach (var item in p)
            {
                await _permissionManager.Store.DeleteAsync(item, default);
            }
            _permissionManager.Store.AutoSaveChanges = true;
            var b = await _permissionManager.Store.SaveChangesAsync();
            return Json(b);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> CreateAllActionAsync()
        {
            AspNetRoleManager rolemger = GetService<AspNetRoleManager>();
            IdentityRole admin = await rolemger.FindAsync(s => new IdentityRole { Id = s.Id }, d => d.Name.Equals("管理员"));
            var id = User.GetUserId();
            var p = PermissionManager.GetAllActionByAssembly();
            _permissionManager.Store.AutoSaveChanges = false;
            rolemger.Store.AutoSaveChanges = false;
            var bb = p.Each(action => { if (id.IsNotNullOrEmpty()) { action.CretaedUser = id; } });

            foreach (var item in bb)
            {
                await rolemger.AddClaimAsync(admin, new Claim(item.Id, item.Id));
                await _permissionManager.Store.CreateAsync(item, default);
            }
            rolemger.Store.AutoSaveChanges = true;
            _permissionManager.Store.AutoSaveChanges = true;
            await rolemger.Store.SaveChangesAsync();
            var b = await _permissionManager.Store.SaveChangesAsync();
            return Json(b);
        }
    }
}