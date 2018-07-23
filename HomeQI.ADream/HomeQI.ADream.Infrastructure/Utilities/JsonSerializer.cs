using Newtonsoft.Json;
using System;

namespace HomeQI.ADream.Infrastructure.HomeQI.ADreamities
{
    /// <summary>
    /// Json 序列化/反序列化。
    /// </summary>
    public class JsonSerializer : ISerializer
    {
        #region ISerializer 成员

        /// <summary>
        /// 获取序列化类型。
        /// </summary>
        public SerializeType SerializeType { get; } = SerializeType.Json;

        /// <summary>
        /// 将一个对象序列化成一个字符串。
        /// </summary>
        /// <param name="obj">要序列化的对象。</param>
        /// <returns>序列化后的字符串。</returns>
        public string Serialize(object obj)
        {
            #region  参数校验

            if (obj == null)
                throw new ArgumentNullEx(nameof(obj));

            #endregion
            return JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将一个字符串反序列化为一个对象。
        /// </summary>
        /// <param name="objType">要反序序列化的对象类型。</param>
        /// <param name="str">要反序列化的字符串。</param>
        /// <returns>反序列化得到的对象。</returns>
        public object Deserialize(Type objType, string str)
        {
            #region 参数校验

            if (objType == null)
                throw new ArgumentNullEx(nameof(objType));

            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullEx(nameof(str));

            #endregion
            return JsonConvert.DeserializeObject(str,objType);
        }

        /// <summary>
        /// 将一个字符串反序列化为一个对象。
        /// </summary>
        /// <param name="str">要反序列化的字符串。</param>
        /// <returns>反序列化得到的对象。</returns>
        public T Deserialize<T>(string str)
        {
            #region 参数校验

            if (string.IsNullOrEmpty(str))
                throw new ArgumentNullEx(nameof(str));

            #endregion

            return (T)Deserialize(typeof(T), str);
        }

        #endregion
    }
}

