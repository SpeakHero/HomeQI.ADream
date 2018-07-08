using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Manager
{
    public class TokenManager
    {

        public TokenManager(ISignInManager signInManager, IConfiguration configuration)
        {
            signInManager.CheakArgument();
            Session = signInManager.Context.Session;
            SignInManager = signInManager;
            Configuration = configuration;
        }

        public ISession Session { get; }
        public ISignInManager SignInManager { get; }
        public IConfiguration Configuration { get; }

        private async Task<User> GetUserAsync(string userName)
        {
            if (userName.IsEmail())
            {
                return await SignInManager.UserManager.FindByEmailAsync(userName);
            }
            if (userName.IsPhone())
            {
                return await SignInManager.UserManager.FindByPhoneAsync(userName);
            }
            return await SignInManager.UserManager.FindByNameAsync(userName);
        }
        private string GetUserName(User user)
        {
            if (user.Email.IsNotNullOrEmpty())
            {
                return user.Email;
            }
            if (user.PhoneNumber.IsNotNullOrEmpty())
            {
                return user.PhoneNumber;
            }
            return user.UserName;
        }
        /// <summary>
        /// 获取JWT字符串并存入缓存
        /// </summary>
        public string IssueJWTAsync(User user)
        {

            Claim[] claims = new Claim[]
            {
                new Claim(JwtRegisteredClaimNames.NameId,user.Id),//Issued At，JWT颁发的时间，采用标准unix时间，用于验证过期
                new Claim(JwtRegisteredClaimNames.UniqueName,GetUserName(user), ClaimValueTypes.Integer64),//Issued At，JWT颁发的时间，采用标准unix时间，用于验证过期
                new Claim(JwtRegisteredClaimNames.Sub,user.Roles.ToJson()),//Subject,
                new Claim(JwtRegisteredClaimNames.Jti, user.SecurityStamp),//JWT ID,JWT的唯一标识
                new Claim(JwtRegisteredClaimNames.Iat,user.EditedTime.ToString()),//Issued At，JWT颁发的时间，采用标准unix时间，用于验证过期
            };
            var audienceConfig = Configuration.GetSection("Audience");
            JwtSecurityToken jwt = new JwtSecurityToken(
            issuer: audienceConfig["Issuer"],//jwt签发者,非必须
            audience: audienceConfig["Audience"],//jwt的接收该方，非必须
            claims: claims,//声明集合
            expires: DateTime.UtcNow.AddHours(12),//指定token的生命周期，unix时间戳格式,非必须
            signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(audienceConfig["Secret"])), SecurityAlgorithms.HmacSha256));//使用私钥进行签名加密

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);//生成最后的JWT字符串
            Session.SetString("Authorization ", encodedJwt);
            return encodedJwt;
        }
    }

}
