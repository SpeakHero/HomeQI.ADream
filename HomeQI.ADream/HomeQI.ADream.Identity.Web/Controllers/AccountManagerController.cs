using HomeQI.Adream.Identity;
using HomeQI.ADream.Identity.ViewModels.ManageViewModels;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace HomeQI.ADream.Identity.Web.Controllers
{
    /// <summary>
    /// 身份管理
    /// </summary>
    public partial class ManagerController : ApiControllerBase
    {
        // GET: AccountManager
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 获取用户列表
        /// </summary>
        /// <param name="par"></param>
        /// <returns></returns>
        [Description("获取用户列表")]
        [HttpPost]
        public async Task<IActionResult> GetUsers([FromBody]BootTabPageListPar par)
        {

            if (par.Search.IsNotNullOrEmpty())
            {
                par.Filters = new QueryableFilter[] { new QueryableFilter
                { Key = "Name", Contract = Contract.Like, Value = par.Search } };
            }
            var results = await _userManager.GetListAsync<UserDto>(par);
            return Json(new BootTabPageResult
            {
                Page = par.CurrentPage,
                Total = results.TotalItemCount,
                Rows = results
            });
        }
        /// <summary>
        /// 创建用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <returns></returns>
        // POST: AccountManager/Create
        [HttpPost]
        [Description("创建用户")]
        public async Task<IActionResult> CreateAsync([FromBody]CreateUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            return Json(await _userManager.CreateAsync
                (_userManager.Mapper.Map<IdentityUser>
                (userDto)));
        }

        /// <summary>
        /// 锁定用户
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="enabled">sd</param>
        /// <returns></returns>
        [HttpPut("{enabled}")]
        [Description("锁定用户")]
        public async Task<IActionResult> SetLockoutEnabledAsync(
        [FromBody]EditeUserDto userDto, [FromRoute] bool enabled)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            return Json(await _userManager.SetLockoutEnabledAsync
               (_userManager.Mapper.Map<IdentityUser>
               (userDto), enabled));
        }
        /// <summary>
        /// 锁定用户结束时间
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="endtime">时间（秒）</param>
        /// <returns></returns>
        [HttpPut("{endtime}")]
        [Description("锁定用户结束时间")]
        public async Task<IActionResult> SetLockoutEndDateAsync(
        [FromBody]EditeUserDto userDto, [FromRoute] int endtime)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            var result = _userManager.Mapper.Map<IdentityUser>
               (userDto);
            result.LockoutEnabled = true;
            return Json(await _userManager.SetLockoutEndDateAsync
               (result, DateTime.Now.AddSeconds(endtime)));
        }
        /// <summary>
        /// 设置用户名
        /// </summary>
        /// <param name="userDto"></param>
        /// <param name="username"></param>
        /// <returns></returns>
        [HttpPut("{username}")]
        [Description("设置用户名")]
        public async Task<IActionResult> SetUserNameAsync(
 [FromBody]EditeUserDto userDto, [FromRoute]string username)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            if (username.IsNullOrEmpty())
            {
                return BadRequest();
            }
            return Json(await _userManager.SetUserNameAsync
               (_userManager.Mapper.Map<IdentityUser>
               (userDto), username));
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        // POST: AccountManager/Delete/5
        [HttpDelete("{id}")]
        [Description("删除用户")]
        public async Task<IActionResult> DeleteAsync([FromRoute]string id,
            [FromBody]UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            if (id != userDto.Id)
            {
                return BadRequest();
            }
            return Json(await _userManager.DeleteAsync
               (_userManager.Mapper.Map<IdentityUser>
               (userDto)));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Description("删除用户")]
        public async Task<IActionResult> SetUserClaimAsync([FromRoute]string id)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }
            await _userManager.Store.EntitySet.Where(d => d.IsDeleted).
                ForEachAsync(action => { action.EmailConfirmed = true; });
            return Json(_userManager.Store.SaveChangesAsync());
        }
    }
}