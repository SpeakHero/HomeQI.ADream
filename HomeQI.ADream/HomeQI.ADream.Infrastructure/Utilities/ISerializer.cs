using System;

namespace HomeQI.ADream.Infrastructure.HomeQI.ADreamities
{
    /// <summary>
    /// 
    /// </summary>
    public interface ISerializer
    {
        /// <summary>
        /// 
        /// </summary>
        SerializeType SerializeType { get; }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="objType"></param>
        /// <param name="str"></param>
        /// <returns></returns>
        object Deserialize(Type objType, string str);
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        T Deserialize<T>(string str);
        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        string Serialize(object obj);
    }
}