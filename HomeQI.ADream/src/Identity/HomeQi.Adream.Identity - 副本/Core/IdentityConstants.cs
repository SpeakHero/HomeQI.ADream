
namespace HomeQI.ADream.Identity.Core
{
    /// <summary>
    /// 表示可以用来配置标识系统所使用的Cookie中间件的所有选项。
    /// </summary>
    public class IdentityConstants
    {
        private static readonly string CookiePrefix = "Identity";
        /// <summary>
        /// The scheme used to identify application authentication cookies.
        /// </summary>
        public static readonly string ApplicationScheme = CookiePrefix + ".Application";

        /// <summary>
        /// The scheme used to identify external authentication cookies.
        /// </summary>
        public static readonly string ExternalScheme = CookiePrefix + ".External";

        /// <summary>
        /// The scheme used to identify Two Factor authentication cookies for saving the Remember Me state.
        /// </summary>
        public static readonly string TwoFactorRememberMeScheme = CookiePrefix + ".TwoFactorRememberMe";

        /// <summary>
        /// The scheme used to identify Two Factor authentication cookies for round tripping user identities.
        /// </summary>
        public static readonly string TwoFactorUserIdScheme = CookiePrefix + ".TwoFactorUserId";
    }
}
