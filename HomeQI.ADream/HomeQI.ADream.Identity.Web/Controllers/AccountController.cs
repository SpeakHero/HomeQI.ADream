using HomeQi.Adream.Identity;
using HomeQI.Adream.Identity;
using HomeQI.ADream.Identity.AccountViewModels;
using HomeQI.ADream.Infrastructure.Core;
using HomeQI.ADream.Infrastructure.Utilities;
using HomeQI.ADream.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Web.Controllers
{
    /// <summary>
    /// 账户管理
    /// </summary>
    [Description("账户管理")]
    public class AccountController : ApiControllerBase
    {
        private readonly SignInManager _signInManager;
        private readonly AspNetUserManager _userManager;
        private readonly IEmailSender _emailSender;
        private readonly ISmsSender _smsSender;
        private readonly TokenGenerater tokenGenerater;
        private readonly TokenManager tokenManager;

        /// <summary>
        /// 账户管理
        /// </summary>
        /// <param name="signInManager"></param>
        /// <param name="aspNetUserManager"></param>
        /// <param name="emailSender"></param>
        /// <param name="smsSender"></param>
        /// <param name="tokenGenerater"></param>
        /// <param name="tokenManager"></param>
        public AccountController(SignInManager signInManager, AspNetUserManager aspNetUserManager, IEmailSender emailSender,
            ISmsSender smsSender, TokenGenerater tokenGenerater, TokenManager tokenManager)
        {
            _signInManager = signInManager;
            _userManager = aspNetUserManager;
            _emailSender = emailSender;
            _smsSender = smsSender;
            this.tokenGenerater = tokenGenerater;
            this.tokenManager = tokenManager;
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Description("用户登录")]
        [HttpPost]
        public IActionResult Login([FromBody]LoginViewModel model, string returnUrl = null)
        {
            return RedirectToAction(nameof(TokenController.Auth), nameof(TokenController).Replace("Controller", ""), model);
        }

        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Description("获取验证码图片")]
        [AllowAnonymous]
        public IActionResult GetCheakCode()
        {
            CheakCode cheakcode = new CheakCode();
            var img = cheakcode.GetImgWithValidateCode(out string code);
            Session.SetString(nameof(CheakCode), code);
            return File(img.ToArray(), "image/Gif");
        }

        /// <summary>
        /// 短信验证码直接注册并登录系统
        /// </summary>
        /// <param name="phone">电话号码</param>
        /// <param name="code">图像验证码</param>
        /// <param name="xcode">短信验证码</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Description("短信验证码直接注册")]
        public async Task<IActionResult> RegisterAsync([RegularExpression(@"^[1]+[3,4,5,7,8]+\d{9}", ErrorMessage = "你输入的电话号码不正确")]string phone, string code, string xcode)
        {
            ResponseResult responseResult = VCheakCode(code);
            if (!responseResult.Succeeded)
            {
                return Json(responseResult);
            }
            if (xcode.Equals(Session.GetGenerateRandomCode()))
            {
                IdentityUser user = new IdentityUser { UserName = phone, PhoneNumber = phone, PhoneNumberConfirmed = true };
                var result = await _userManager.CreateAsync(user, "Qwe@#23");
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, true);
                    return Json(await tokenManager.SignInResultAsync(user));
                }
                else
                {
                    return Json(ResponseResult.Failed("注册失败，可能密码不符合要求", "注册失败,可能已经注册账号", "已注册，请直接登录"));
                }
            }
            return Json(ResponseResult.Failed("注册失败,短信验证码不正确"));
        }
        /// <summary>
        /// POST:注册用户
        /// </summary>
        /// <param name="model">注册用户模型</param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Description("用户注册")]
        public async Task<IActionResult> Register([FromBody]RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                string callbackUrl = string.Empty;
                ResponseResult responseResult = VCheakCode(model.Code);
                if (!responseResult.Succeeded)
                {
                    return Json(responseResult);
                }
                IdentityUser user = model.ByPhone ? new IdentityUser { UserName = model.Phone } : new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    Logger.LogInformation(3, "注册了一个新的用户.");
                    if (!model.ByPhone)
                    {
                        var codeEmail = await tokenGenerater.GenerateEmailAsync(model.Email);
                        callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = codeEmail }, protocol: HttpContext.Request.Scheme);
                        await _emailSender.SendEmailAsync(model.Email, user.UserName + ":请完成注册验证",
                            $"请点击该链接完成账号注册: <a href='{callbackUrl}'>点我完成注册！</a>有效时间为：10分钟，请你尽快填写。");
                    }
                    else
                    {
                        if (!model.XCode.Equals(Session.GetGenerateRandomCode()))
                        {
                            return Json(ResponseResult.Failed("注册失败,短信验证码不正确"));
                        }
                        else
                        {
                            var p = await _userManager.UpdateConfirmPhone(user);
                            if (!p.Succeeded)
                            {
                                return Json(p);
                            }
                            else
                            {
                                await _signInManager.SignInAsync(user, true);
                                return Json(await tokenManager.SignInResultAsync(user));
                            }
                        }
                    }
                }
                return Json(result);
            }
            return Failed();
        }
        /// <summary>
        /// /用户退出登录/LogOff
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [Description("用户退出登录")]
        public async Task<IActionResult> LogOff()
        {
            await _signInManager.SignOutAsync();
            Logger.LogInformation(4, "User logged out.");
            return Ok();
        }

        /// <summary>
        /// 使用第三方账户登录
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [Description("使用第三方账户登录")]
        public IActionResult ExternalLogin(string provider, string returnUrl = null)
        {
            // 请求重定向到外部登录提供程序。
            var redirectUrl = Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        /// <summary>
        /// 使用第三方账户登录
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="remoteError"></param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Description("使用第三方账户登录")]

        public async Task<IActionResult> ExternalLoginCallback(string returnUrl = null, string remoteError = null)
        {
            if (remoteError != null)
            {
                return BadRequest(remoteError);
            }
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return BadRequest("无效外部登录");
            }

            // 如果用户已经登录，则使用此外部登录提供程序在用户中签名。
            var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent: false);
            return Ok(result);
        }

        /// <summary>
        /// POST: 使用第三方账户登录
        /// </summary>
        /// <param name="model"></param>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Description("使用第三方账户登录")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // 从外部登录提供程序获取有关用户的信息
                var info = await _signInManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return BadRequest("无效外部登录");
                }
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                var result = await _userManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await _userManager.AddLoginAsync(user, info);
                    if (result.Succeeded)
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        Logger.LogInformation(6, "User created an account using {Name} provider.", info.LoginProvider);
                        return Ok(result);
                    }
                }
            }
            return BadRequest(model);
        }
        /// <summary>
        /// 确认邮箱
        /// </summary>
        /// <param name="userId">用户编号</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [Description("确认邮箱")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return Json(ResponseResult.Failed("输入不正确！"));
            }
            var user = await _userManager.FindAsync(s => new IdentityUser { SecurityStamp = s.SecurityStamp, EditedTime = s.EditedTime, ConcurrencyStamp = s.ConcurrencyStamp, Id = s.Id }, p => p.Id.Equals(userId));
            if (user == null)
            {
                return Json(ResponseResult.Failed("输入不正确！"));
            }
            if (await tokenGenerater.VerifByEmailAsync(code))
            {
                var e = await _userManager.UpdateConfirmEmail(user);
                if (e.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return Json(await tokenManager.SignInResultAsync(user));
                }
            }
            return Json(ResponseResult.Failed("输入不正确！"));
        }
        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="email">邮箱号码</param>
        /// <param name="msg">消息{0}占位符</param>
        /// <param name="title">标题开头</param>
        /// <returns>email</returns>
        [AllowAnonymous]
        [HttpPost]
        [Description("发送邮箱验证码")]
        public async Task<IActionResult> SendEmilToken(string email, string msg = "你正在进行验证{0}", string title = "验证码")
        {
            if (email.IsNullOrEmpty())
            {
                return BadRequest();
            }
            if (!email.IsEmail())
            {
                return BadRequest();
            }
            var codePhone = await tokenGenerater.GenerateEmailAsync(email);
            await _emailSender.SendEmailAsync(email, title, msg.Replace("{0}", codePhone));
            return Ok(email);
        }

        /// <summary>
        /// 发送手机验证码
        /// </summary>
        /// <param name="phone">移动电话号码</param>
        /// <param name="title">标题开头</param>
        /// <returns>phone</returns>
        [AllowAnonymous]
        [HttpPost]
        [Description("发送手机验证码")]
        public async Task<IActionResult> SendSmsToken(string phone, string title = "你正在进行手机验证")
        {
            if (phone.IsNullOrEmpty())
            {
                return BadRequest();
            }
            if (!phone.IsPhone())
            {
                return BadRequest();
            }
            var codePhone = await tokenGenerater.GenerateRandomCodeAsync();
            var msg = await _smsSender.SendSmsAsync(phone, $"{title}，验证码为: {codePhone}有效时间为10分钟，请你尽快完成确认，谢谢。");
            return Ok(msg);
        }
        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="model">model</param>
        /// <returns>model</returns>
        [AllowAnonymous]
        [HttpPost]
        [Description("重置密码")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                Expression<Func<IdentityUser, bool>> predicate = null;
                if (model.ByPhone)
                {
                    predicate = a => a.PhoneNumber.Equals(model.Phone);
                }
                else
                {
                    predicate = a => a.Email.Equals(model.Email);
                }
                var user = await _userManager.FindAsync(s => new IdentityUser { SecurityStamp = s.SecurityStamp, Id = s.Id, EditedTime = s.EditedTime }, predicate);
                if (user == null)
                {
                    return BadRequest(model);
                }
                else
                {
                    bool c = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (c)
                    {
                        bool ok = model.ByPhone ? tokenGenerater.VerifGenerateRandomCode(model.Code) :
                            await tokenGenerater.VerifByEmailAsync(model.Code);
                        if (ok)
                        {
                            return Ok(await _userManager.UpdatePasswordHash(user, model.Password, true));
                        }
                    }
                }
            }
            return BadRequest(model);
        }
    }
}
