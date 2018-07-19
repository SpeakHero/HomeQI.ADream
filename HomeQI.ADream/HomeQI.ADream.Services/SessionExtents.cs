using HomeQI.ADream.Services;

namespace Microsoft.AspNetCore.Http
{
    public static class SessionExtents
    {
        /// <summary>
        /// 获取随机验证码
        /// </summary>
        /// <param name="session"></param>
        /// <returns></returns>
        public static string GetGenerateRandomCode(this ISession session)
        {
            return session.GetString(nameof(TokenGenerater.GenerateRandomCode));
        }
        public static string GetGenerateEmailAsync(this ISession session)
        {
            return session.GetString(nameof(TokenGenerater.GenerateEmailAsync));
        }
    }
}
