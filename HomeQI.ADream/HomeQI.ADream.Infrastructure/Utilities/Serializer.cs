using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.IO;
using System.Text;
using System.Xml.Serialization;

namespace System
{
    /// <summary>
    /// 序列化帮助类
    /// </summary>
    public static class Serializer
    {

      
        /// <summary>
        /// 对象转换为json字符串
        /// </summary>
        /// <param name="entity">对象</param>
        /// <returns></returns>
        public static string ToJson(this object entity)
        {
            var converter = new IsoDateTimeConverter();
            converter.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            var serializer = new JsonSerializer();
            serializer.Converters.Add(converter);

            var sb = new StringBuilder();
            serializer.Serialize(new JsonTextWriter(new StringWriter(sb)), entity);
            return sb.ToString();
        }
        /// <summary>
        /// 字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T FromJson<T>(this string json)
        {
            return JsonConvert.DeserializeObject<T>(json);
        }
        /// <summary>
        /// json字符串尝试转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T TryFromJson<T>(this string json) where T : class, new()
        {
            try
            {
                var value = JsonConvert.DeserializeObject<T>(json);
                return value ?? new T();
            }
            catch (Exception)
            {
                return new T();
            }
        }
        /// <summary>
        /// 对象转换为xml
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToXml<T>(this T obj)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                serializer.Serialize(stream, obj);
                stream.Position = 0;
                var reader = new StreamReader(stream);
                var xml = reader.ReadToEnd();
                reader.Dispose();
                return xml;
            }
        }
        /// <summary>
        /// 从xml字符串转换为对象
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="xml"></param>
        /// <returns></returns>
        public static T FromXml<T>(this string xml)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var stream = new MemoryStream())
            {
                var writer = new StreamWriter(stream);
                writer.Write(xml);
                writer.Flush();
                stream.Position = 0;
                T instance = (T)serializer.Deserialize(stream);
                writer.Dispose();
                return instance;
            }
        }

    }
}
