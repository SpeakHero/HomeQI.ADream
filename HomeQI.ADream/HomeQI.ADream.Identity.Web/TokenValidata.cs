namespace HomeQI.ADream.Identity.Web
{
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using System.Security.Claims;
    /// <summary>
    /// 
    /// </summary>
    public class MyTokenValidata : ISecurityTokenValidator
    {
        /// <summary>
        /// 
        /// </summary>
        //判断当前token是否有值
        public bool CanValidateToken => true;
        /// <summary>
        /// 
        /// </summary>
        public int MaximumTokenSizeInBytes { get; set; }//顾名思义是验证token的最大bytes
        /// <summary>
        /// 
        /// </summary>
        /// <param name="securityToken"></param>
        /// <returns></returns>
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
