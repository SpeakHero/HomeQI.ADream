using HomeQi.Adream.Identity;
using HomeQI.Adream.Identity;
using HomeQI.ADream.Identity.AccountViewModels;
using HomeQI.ADream.Infrastructure.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Web.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Description("账户管理")]
    public class AccountController : BaseController
    {
        public SignInManager SignInManager { get; }
        public AspNetUserManager UserManager { get; }
        public AccountController(SignInManager signInManager, AspNetUserManager aspNetUserManager)
        {
            signInManager.CheakArgument();
            SignInManager = signInManager;
            UserManager = aspNetUserManager;
        }
        //[HttpPost("authenticate")]
        //[Description("token发放")]
        //public IActionResult Authenticate([FromBody]UserDto userDto)
        //{
        //    var user = _store.FindUser(userDto.UserName, userDto.Password);
        //    if (user == null) return Unauthorized();
        //    var tokenHandler = new JwtSecurityTokenHandler();
        //    var key = Encoding.ASCII.GetBytes(Consts.Secret);
        //    var authTime = DateTime.UtcNow;
        //    var expiresAt = authTime.AddDays(7);
        //    var tokenDescriptor = new SecurityTokenDescriptor
        //    {
        //        Subject = new ClaimsIdentity(new Claim[]
        //        {
        //    new Claim(JwtClaimTypes.Audience,"api"),
        //    new Claim(JwtClaimTypes.Issuer,"http://localhost:5200"),
        //    new Claim(JwtClaimTypes.Id, user.Id.ToString()),
        //    new Claim(JwtClaimTypes.Name, user.Name),
        //    new Claim(JwtClaimTypes.Email, user.Email),
        //    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber)
        //        }),
        //        Expires = expiresAt,
        //        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
        //    };
        //    var token = tokenHandler.CreateToken(tokenDescriptor);
        //    var tokenString = tokenHandler.WriteToken(token);
        //    return Ok(new
        //    {
        //        access_token = tokenString,
        //        token_type = "Bearer",
        //        profile = new
        //        {
        //            sid = user.Id,
        //            name = user.Name,
        //            auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
        //            expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
        //        }
        //    });
        //}
        [AllowAnonymous]
        [Description("用户登录")]
        [HttpPost]
        public async Task<IdentityResult> Login([FromBody]LoginViewModel model, string returnUrl = null)
        {
            IdentityError identityError = null;
            if (ModelState.IsValid)
            {
                if (CheakCode.ToUpper() != model.Code.ToUpper())
                {
                    identityError = new IdentityError
                    {
                        Code = nameof(model.Code),
                        Description = "验证码错误！"
                    };
                }
                else
                {
                    bool user;
                    user = await UserManager.Users.AnyAsync(a => a.PhoneNumber.Equals(model.Phone) || a.UserName.Equals(model.Name) || a.Email.Equals(model.Email));
                    if (!user)
                    {
                        identityError = new IdentityError
                        {
                            Code = nameof(Login),
                            Description = "登陆验证失败！密码或者用户名错误。"
                        };
                    }
                }
            }
            else
            {
                identityError = new IdentityError
                {
                    Code = nameof(Login),
                    Description = "登陆验证失败！请检查输入。"
                };
            }
            return IdentityResult.Failed(identityError);
        }

        /// <summary>
        /// 获取验证码图片
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetCheakCode()
        {
            CheakCode cheakcode = new CheakCode();
            Session.SetString(nameof(CheakCode), cheakcode.GetValidateCode);
            return File(cheakcode.GetImgWithValidateCode().ToArray(), "image/Gif");
        }
        /// <summary>
        /// api/Account
        /// </summary>
        /// <returns>IHostingEnvironment</returns>
            // GET: api/Account
        [HttpGet]
        public IHostingEnvironment Get()
        {
            return Env;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        // GET: api/Account/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            return "value";
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="value"></param>
        // POST: api/Account
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }
        /// <summary>
        /// /
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        // PUT: api/Account/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
