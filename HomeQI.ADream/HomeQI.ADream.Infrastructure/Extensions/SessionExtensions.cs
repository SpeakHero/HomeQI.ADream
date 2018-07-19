
using Newtonsoft.Json;
using System;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// 
    /// </summary>
    public static class SessionExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void SetObjectAsJson(this ISession session, string key, object value)
        {
            session.SetString(key, value.ToJson());
        }
        public static string GetCheakCode(this ISession session)
        {
            return session.GetString("CheakCode");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="session"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static T GetObjectFromJson<T>(this ISession session, string key)
        {
            var value = session.GetString(key);

            T t = JsonConvert.DeserializeObject<T>(value);
            return value == null ? default : t;
        }
    }
}
