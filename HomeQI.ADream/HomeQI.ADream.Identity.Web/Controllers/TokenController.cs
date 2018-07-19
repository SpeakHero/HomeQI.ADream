using HomeQi.Adream.Identity;
using HomeQI.ADream.Identity.AccountViewModels;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.Threading.Tasks;
using SignInResult = HomeQI.Adream.Identity.SignInResult;
namespace HomeQI.ADream.Identity.Web.Controllers
{
    /// <summary>
    ///  Token管理
    /// </summary>
    public class TokenController : ApiControllerBase
    {
        private readonly TokenManager tokenManager;
        /// <summary>
        /// Token管理
        /// </summary>
        /// <param name="tokenManager"></param>
        public TokenController(TokenManager tokenManager)
        {
            this.tokenManager = tokenManager;
        }
        /// <summary>
        /// Jwt Token发放
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [Description("Jwt Token发放")]
        [HttpPost]
        public async Task<IActionResult> Auth([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //ResponseResult responseResult = VCheakCode(model.Code);
                //if (!responseResult.Succeeded)
                //{
                //    return Json(responseResult);
                //}
                var user = await tokenManager.GetUserAsync(model.ByPhone, model.Email, model.Phone);
                if (user != null)
                {
                    if (model.ByPhone)
                    {
                        if (!user.PhoneNumberConfirmed)
                        {
                            return Json(SignInResult.PhoneNumberConfirmed);
                        }
                    }
                    else
                    {
                        if (!user.EmailConfirmed)
                        {
                            return Json(SignInResult.EmailConfirmed);
                        }
                    }
                    var result = await tokenManager.PasswordSignInAsync(user, model.Password);
                    return Json(result);
                }
                else
                {
                    return Json(SignInResult.Failed("用户名或者密码错误!"));
                }
            }
            return Failed();
        }

    }
}
