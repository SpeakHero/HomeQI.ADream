using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Core.Authorize
{
    /// <summary>
    /// you must services.AddSingleton<IAuthorizationHandler, LoggingAuthorizationHandler>();
    /// </summary>
    public class LoggingAuthorizationHandler : AuthorizationHandler<AuthorizationRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, AuthorizationRequirement requirement)
        {
            // 检查是否符合要求。
            var httpcontext = context.Resource as AuthorizationFilterContext;
            return Task.CompletedTask;
        }
        public LoggingAuthorizationHandler()
        {

        }

    }
}
