using HomeQI.ADream.Identity.Entites;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Core.Authorize
{

    /// <summary>
    /// <![CDATA[
    /// @using Microsoft.AspNetCore.Authorization;
    /// @inject IAuthorizationService AuthorizationService
    /// @if (await AuthorizationService.AuthorizeAsync(User, "Over21"))
    /// ]]>
    /// </summary>
    public class CustomAuthorizationService : IAuthorizationService
    {
        private readonly Logger<CustomAuthorizationService> logger;

        public CustomAuthorizationService(Logger<CustomAuthorizationService> logger)
        {
            this.logger = logger;
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, IEnumerable<IAuthorizationRequirement> requirements)
        {
            var httpcontext = resource as AuthorizationFilterContext;
            if (TokenValidate(httpcontext.HttpContext).Succeeded)
            {
                var rotedata = httpcontext.RouteData;
                var _controllerName = rotedata.Values["controller"]?.ToString();
                var _actionName = rotedata.Values["action"]?.ToString();
                var _areaName = rotedata.Values["area"]?.ToString();
            }
            return Task.FromResult(AuthorizationResult.Failed());
        }

        public Task<AuthorizationResult> AuthorizeAsync(ClaimsPrincipal user, object resource, string policyName)
        {
            return Task.FromResult(AuthorizationResult.Success());
        }

        private AuthorizationResult TokenValidate(HttpContext httpContext)
        {
            var headers = httpContext.Request.Headers;
            //检测是否包含'Authorization'请求头，如果不包含返回context进行下一个中间件，用于访问不需要认证的API
            if (headers.ContainsKey("Authorization"))
            {
                try
                {
                    var tokenStr = headers["Authorization"];
                    string jwtStr = tokenStr.ToString().Substring("Bearer ".Length).Trim();
                    //验证缓存中是否存在该jwt字符串
                    var jwt = httpContext.Session.GetString(jwtStr);
                    if (jwt.IsNullOrEmpty())
                    {
                        httpContext.Response.StatusCode = 400;
                        httpContext.Response.WriteAsync("非法请求");
                        logger.LogError("非法请求", httpContext);
                        return AuthorizationResult.Failed();
                    }
                    Token tm = jwt.DeserializeObject<Token>();
                    //提取tokenModel中的Sub属性进行authorize认证
                    List<Claim> lc = new List<Claim>();
                    Claim c = new Claim(tm.Sub + "Type", tm.Sub);
                    lc.Add(c);
                    ClaimsIdentity identity = new ClaimsIdentity(lc);
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                    httpContext.User = principal;
                }
                catch (Exception ex)
                {
                    logger.LogError("token验证异常", ex);
                    httpContext.Response.StatusCode = 500;
                    httpContext.Response.WriteAsync("token验证异常");
                    return AuthorizationResult.Failed();
                }
            }
            return AuthorizationResult.Success();
        }
    }
}
