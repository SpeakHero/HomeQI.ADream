using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using HomeQI.Adream.Identity;
using HomeQI.Adream.Identity;
using HomeQI.ADream.Identity.ManageViewModels;
using HomeQI.ADream.Infrastructure.Core;
using HomeQI.ADream.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace HomeQI.ADream.Identity.Web.Controllers
{
    /// <summary>
    /// 身份管理
    /// </summary>
    public partial class ManagerController : ApiControllerBase
    {
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ISmsSender _smsSender;
        private readonly AspNetUserManager _userManager;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        /// <param name="userManager"></param>
        /// <param name="signInManager"></param>
        /// <param name="emailSender"></param>
        /// <param name="smsSender"></param>
        public ManagerController(ILoggerFactory logger, AspNetUserManager userManager,
            SignInManager signInManager,
            IEmailSender emailSender,
            ISmsSender smsSender)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
        }


        /// <summary>
        /// 移除外部登录
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("移除外部登录")]

        public async Task<IActionResult> RemoveLogin([FromBody]RemoveLoginViewModel account)
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.RemoveLoginAsync(user, account.LoginProvider, account.ProviderKey);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Json(result);
                }
            }
            return Json(IdentityResult.Failed());
        }
        /// <summary>
        /// 添加手机号码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]

        [Description(" 添加手机号码")]

        public async Task<IActionResult> AddPhoneNumber([FromBody]AddPhoneNumberViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            //生成令牌并发送它
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest();
            }
            var code = await _userManager.GenerateChangePhoneNumberTokenAsync(user, model.PhoneNumber);
            await _smsSender.SendSmsAsync(model.PhoneNumber, "你正在进行手机验证，验证码为: " + code + "有效时间为10分钟，请你尽快完成确认，谢谢。");
            return Ok();
        }

        /// <summary>
        /// 添加电子邮件
        /// </summary>
        /// <param name="model"></param>
        /// <param name="authorization"></param>
        /// <returns></returns>
        [HttpPost]

        [Description(" 添加电子邮件")]
        public async Task<IActionResult> AddEmailAddress([FromBody]AdEmailAddressViewModel model, [FromHeader]string authorization)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("not IsValid");
            }
            // 生成令牌并发送它
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest("Error");
            }

            var code = await _userManager.GenerateChangeEmailTokenAsync(user, model.EmailAddress);
            var callbackUrl = Url.Action(nameof(VerifyEmailaddress), "Manager", new { Emailaddress = model.EmailAddress, code }, protocol: HttpContext.Request.Scheme);
            await _emailSender.SendEmailAsync(model.EmailAddress, user.UserName + ":邮箱绑定验证",
                        $"验证码为:<br/> {code}  <br/>有效时间为：10分钟，请你尽快填写。");
            return Ok();
        }

        /// <summary>
        /// 开启登录二次验证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("开启登录二次验证")]

        public async Task<IActionResult> EnableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, true);
                await _signInManager.SignInAsync(user, isPersistent: false);
                Logger.LogInformation(1, "User enabled two-factor authentication.");
                return Ok();
            }
            return BadRequest();
        }

        /// <summary>
        /// 禁用登录二次验证
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("禁用登录二次验证")]

        public async Task<IActionResult> DisableTwoFactorAuthentication()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                await _userManager.SetTwoFactorEnabledAsync(user, false);
                await _signInManager.SignInAsync(user, isPersistent: false);
                Logger.LogInformation(2, "User disabled two-factor authentication.");
                return Ok();
            }
            return BadRequest();
        }
        /// <summary>
        /// 验证邮箱地址
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("验证邮箱地址")]
        public async Task<IActionResult> VerifyEmailaddress([FromBody]VerifyEmailaddressViewModel model)
        {
            if (ModelState.IsValid)
            {


                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var result = await _userManager.ChangeEmailAsync(user, model.Emailaddress, model.Code);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    return Json(result);
                }
            }
            ModelState.AddModelError(string.Empty, "验证失败");
            return Failed();
        }

        /// <summary>
        /// 验证手机号码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("验证手机号码")]

        public async Task<IActionResult> VerifyPhoneNumber([FromBody]VerifyPhoneNumberViewModel model)
        {
            if (ModelState.IsValid)
            {

                var user = await GetCurrentUserAsync();
                if (user != null)
                {
                    var result = await _userManager.ChangePhoneNumberAsync(user, model.PhoneNumber, model.Code);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                    }
                    return Json(result);
                }
            }
            ModelState.AddModelError(string.Empty, "无法验证电话号码");
            return Failed();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("解除手机号码绑定")]

        public async Task<IActionResult> RemovePhoneNumber()
        {
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.SetPhoneNumberAsync(user, null);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                return Json(result);
            }
            return BadRequest();
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("修改密码")]
        public async Task<IActionResult> ChangePassword([FromBody]ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    Logger.LogInformation(3, "User changed their password successfully.");
                }
                return Json(result);
            }
            return BadRequest();
        }

        /// <summary>
        ///  设置密码
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("设置密码")]

        public async Task<IActionResult> SetPassword([FromBody]SetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Failed();
            }

            var user = await GetCurrentUserAsync();
            if (user != null)
            {
                var result = await _userManager.AddPasswordAsync(user, model.NewPassword);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                }
                return Json(result);
            }
            return BadRequest();
        }
        /// <summary>
        ///外部登录列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> ManageLogins()
        {

            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest("Error");
            }
            var userLogins = await _userManager.GetLoginsAsync(user);
            var schemes = await _signInManager.GetExternalAuthenticationSchemesAsync();
            var otherLogins = schemes.Where(auth => userLogins.All(ul => auth.Name != ul.LoginProvider)).ToList();
            return Json(new
            {
                CurrentLogins = userLogins,
                OtherLogins = otherLogins
            });
        }

        /// <summary>
        /// 向外部登录提供程序请求重定向到当前用户的登录链接      
        /// </summary>
        /// <param name="provider"></param>
        /// <returns></returns>
        [HttpPost]
        [Description("向外部登录提供程序请求重定向到当前用户的登录链接")]

        public IActionResult LinkLogin(string provider)
        {
            // 向外部登录提供程序请求重定向到当前用户的登录链接
            var redirectUrl = Url.Action("LinkLoginCallback", "Manage");
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl, _userManager.GetUserId(User));
            return Challenge(properties, provider);
        }

        /// <summary>
        /// 向外部登录提供程序请求重定向到当前用户的登录链接回调
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("向外部登录提供程序请求重定向到当前用户的登录链接回调")]
        public async Task<ActionResult> LinkLoginCallback()
        {
            var user = await GetCurrentUserAsync();
            if (user == null)
            {
                return BadRequest("Error");
            }
            var info = await _signInManager.GetExternalLoginInfoAsync(await _userManager.GetUserIdAsync(user));
            return Json(info);
        }

        #region Helpers

        /// <summary>
        /// 
        /// </summary>
        public enum ManageMessageId
        {
            /// <summary>
            /// 增加电话成功
            /// </summary>
            AddPhoneSuccess,
            /// <summary>
            /// 添加登录成功
            /// </summary>
            AddLoginSuccess,
            /// <summary>
            /// 更改密码成功
            /// </summary>
            ChangePasswordSuccess,
            /// <summary>
            /// 双因素成功
            /// </summary>
            SetTwoFactorSuccess,
            /// <summary>
            /// 设置密码成功
            /// </summary>
            SetPasswordSuccess,
            /// <summary>
            /// 删除登录成功
            /// </summary>
            RemoveLoginSuccess,
            /// <summary>
            /// 删除电话成功
            /// </summary>
            RemovePhoneSuccess,
            /// <summary>
            /// 错误
            /// </summary>
            Error,
            /// <summary>
            /// 添加电子邮件地址成功
            /// </summary>
            AddEmailaddressSuccess
        }

        private Task<IdentityUser> GetCurrentUserAsync()
        {
            return _userManager.GetUserAsync(User);
            //  return Task.FromResult(HttpContext.User.FindFirst(nameof(IdentityUser)).Value.FromJson<IdentityUser>());
        }

        #endregion
    }
}