using System;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Options
{
    /// <summary>
    /// Options for <see cref="ISecurityStampValidator"/>.
    /// </summary>
    public class SecurityStampValidatorOptions
    {
        /// <summary>
        /// Gets or sets the <see cref="TimeSpan"/> after which security stamps are re-validated. Defaults to 30 minutes.
        /// </summary>
        /// <value>
        /// The <see cref="TimeSpan"/> after which security stamps are re-validated.
        /// </value>
        public TimeSpan ValidationInterval { get; set; } = TimeSpan.FromMinutes(30);

        /// <summary>
        /// 当默认安全戳验证器替换cookie中的用户CalimsEngor时调用。
        /// </summary>
        public Func<SecurityStampRefreshingPrincipalContext, Task> OnRefreshingPrincipal { get; set; }
    }
}