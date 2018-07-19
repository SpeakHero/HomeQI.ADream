using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Web
{
    using System.Security.Claims;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    public class MyTokenValidata : ISecurityTokenValidator
    {
        //判断当前token是否有值
        public bool CanValidateToken => true;

        public int MaximumTokenSizeInBytes { get; set; }//顾名思义是验证token的最大bytes

        public bool CanReadToken(string securityToken)
        {
            return true;
        }
        ///验证securityToken
        public ClaimsPrincipal ValidateToken(string securityToken, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            validatedToken = null;
            if (securityToken != "yourtoken")
            {
                return null;
            }
            var identity = new ClaimsIdentity(JwtBearerDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim("name", "cyao"));
            identity.AddClaim(new Claim(ClaimsIdentity.DefaultRoleClaimType, "admin"));
            identity.AddClaim(new Claim("SuperAdmin", "true"));//添加用户访问权限
            var principal = new ClaimsPrincipal(identity);
            return principal;
        }
    }
}
