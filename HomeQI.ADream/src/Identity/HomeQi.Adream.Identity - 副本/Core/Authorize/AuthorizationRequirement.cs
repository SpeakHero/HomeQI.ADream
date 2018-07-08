using Microsoft.AspNetCore.Authorization;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeQI.ADream.Identity.Core.Authorize
{
    /// <summary>
    /// 权限集合
    /// </summary>
    public class AuthorizationRequirement : IAuthorizationRequirement
    {
        /// <summary>
        /// 请求参数的值
        /// </summary>
        public string ParameterValue { get; set; }

        /// <summary>
        /// 请求参数的名称
        /// </summary>
        public string ParameterName { get; set; }

        public string Actions { get; set; }

        public string Controller { get; set; }

        public ICollection<string> RolesId { get; set; }


    }
}
