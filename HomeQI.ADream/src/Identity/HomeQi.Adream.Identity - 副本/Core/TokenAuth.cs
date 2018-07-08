using HomeQI.ADream.Identity.Entites;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Core
{
    /// <summary>
    /// Token验证授权中间件
    /// </summary>
    public class TokenAuth
    {
        /// <summary>
        /// http委托
        /// </summary>
        private readonly RequestDelegate _next;

        public TokenAuth(RequestDelegate next, ILogger<TokenAuth> logger)
        {
            logger.CheakArgument();
            Logger = logger;
            _next = next;
        }
        public ILogger<TokenAuth> Logger { get; }
        /// <summary>
        /// 验证授权
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        public Task Invoke(HttpContext httpContext)
        {
            var headers = httpContext.Request.Headers;
            httpContext.Response.Headers.Append("x-powered-by", "SpeakHero");
            //检测是否包含'Authorization'请求头，如果不包含返回context进行下一个中间件，用于访问不需要认证的API
            if (!headers.ContainsKey("Authorization"))
            {
                return _next(httpContext);
            }
            var tokenStr = headers["Authorization"];
            try
            {

                string jwtStr = tokenStr.ToString().Substring("Bearer ".Length).Trim();
                //验证缓存中是否存在该jwt字符串
                var jwt = httpContext.Session.GetString("Authorization");
                if (jwt.IsNotNullOrEmpty())
                {
                    if (jwt.Equals(jwtStr))
                    {
                        Token tm = jwt.DeserializeObject<Token>();
                        //提取tokenModel中的Sub属性进行authorize认证
                        List<Claim> lc = new List<Claim>();
                        Claim c = new Claim(tm.Sub + "Type", tm.Sub);
                        lc.Add(c);
                        ClaimsIdentity identity = new ClaimsIdentity(lc);
                        ClaimsPrincipal principal = new ClaimsPrincipal(identity);
                        httpContext.User = principal;
                        return _next(httpContext);
                    }
                    else
                    {
                        httpContext.Response.StatusCode = 400;
                        Logger.LogError("非法请求", httpContext);
                        return httpContext.Response.WriteAsync("Unlawful request");
                    }
                }
                httpContext.Response.StatusCode = 403;
                Logger.LogError("Unauthorized", httpContext);
                return httpContext.Response.WriteAsync("Unauthorized");
            }
            catch (Exception ex)
            {
                Logger.LogError("token验证异常", ex);
                httpContext.Response.StatusCode = 500;
                return httpContext.Response.WriteAsync("token验证异常");
            }
        }
    }
}
