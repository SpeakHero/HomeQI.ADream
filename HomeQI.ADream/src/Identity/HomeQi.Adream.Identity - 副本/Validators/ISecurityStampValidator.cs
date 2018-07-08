
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace HomeQI.ADream.Identity.Validators
{
    /// <summary>
    ///提供用于验证传入标识的安全戳的摘要，并对其进行再生或拒绝
    /// 基于验证结果的身份。
    /// </summary>
    public interface ISecurityStampValidator
    {
        /// <summary>
        /// 验证身份的安全标记作为异步操作，如果验证成功，则重新构建身份，否则拒绝。
        /// the identity.
        /// </summary>
        /// <param name="context">The context containing the <see cref="System.Security.Claims.ClaimsPrincipal"/>
        /// and <see cref="Http.Authentication.AuthenticationProperties"/> to validate.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous validation operation.</returns>
        Task ValidateAsync(CookieValidatePrincipalContext context);
    }
}

