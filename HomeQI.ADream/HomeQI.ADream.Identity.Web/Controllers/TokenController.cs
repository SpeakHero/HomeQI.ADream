using HomeQi.Adream.Identity;
using HomeQI.Adream.Identity;
using HomeQI.ADream.Identity.AccountViewModels;
using HomeQI.ADream.Identity.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.ComponentModel;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Web.Controllers
{
    public class TokenController : BaseController
    {
        private readonly IOptions<Audience> _settings;
        private readonly IConfiguration _configuration;
        private readonly UserManager userManager;
        private readonly IOptions<Audience> options;
        private readonly SignInManager signInManager;

        public TokenController(IConfiguration configuration, IOptions<Audience> settings, SignInManager signInManager, UserManager aspNetUserManager, IOptions<Audience> options)
        {
            _settings = settings;
            this.signInManager = signInManager;
            _configuration = configuration;
            userManager = aspNetUserManager;
            this.options = options;
        }

        [AllowAnonymous]
        [Description("Jwt Token发放")]
        [HttpPost]
        public async Task<IActionResult> Auth([FromBody] LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                //if (CheakCode == null)
                //{
                //    return Json(new ResponseData
                //    {
                //        Code = "903",
                //        Message = "验证码为空",
                //        Data = model
                //    });
                //}
                //if (CheakCode?.ToUpper() != model.Code.ToUpper())
                //{
                //    return Json(new ResponseData
                //    {
                //        Code = "902",
                //        Message = "验证码错误",
                //        Data = model
                //    });
                //}
                var user = new IdentityUser { PhoneNumber = model.Phone, UserName = model.Name, Email = model.Email, PasswordHash = model.Password };
                var signin = await signInManager.PasswordSignInAsync(user, model.Password, true, true);
                if (signin.Succeeded)
                {
                    var uid = await userManager.GetUserAsync(User);
                    if (uid != null)
                    {


                        var claims = new Claim[] {
                            new Claim(ClaimTypes.Name, uid.UserName),
                            new  Claim(JwtRegisteredClaimNames.NameId, uid.Id)
                        };
                        var role = await userManager.GetRolesAsync(uid);
                        if (role != null)
                        {
                            string sub = "";
                            foreach (var item in role)
                            {
                                sub += item + ",";
                            }
                            claims.Append(new Claim(ClaimTypes.Role, sub));
                            if (uid.Email.IsNotNullOrEmpty())
                            {
                                claims.Append(new Claim(JwtRegisteredClaimNames.Email, uid.Email));

                            }
                            if (uid.PhoneNumber.IsNotNullOrEmpty())
                            {
                                claims.Append(new Claim(ClaimTypes.MobilePhone, uid.PhoneNumber));

                            }
                            claims.Append(new Claim(JwtRegisteredClaimNames.Sub, sub));
                        }
                        var tokenHandler = new JwtSecurityTokenHandler();
                        var key = Encoding.UTF8.GetBytes(options.Value.Secret);
                        var authTime = DateTime.UtcNow;
                        var expiresAt = authTime.AddDays(7);
                        var tokenDescriptor = new SecurityTokenDescriptor
                        {
                            Subject = new ClaimsIdentity(claims),
                            Expires = expiresAt,
                            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                        };
                        var token = tokenHandler.CreateToken(tokenDescriptor);
                        var tokenString = tokenHandler.WriteToken(token);
                        return Ok(new
                        {
                            access_token = tokenString,
                            token_type = "Bearer",
                            profile = new
                            {
                                sid = user.Id,
                                name = user.UserName,
                                auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                                expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
                            }
                        });
                    }
                }
            }
            return Unauthorized();
        }
    }
}
