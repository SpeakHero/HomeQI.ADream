using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static string GetUserId(this HttpRequest request)
        {
            return request.HttpContext.Session.GetString("UserId");
        }
        /// <summary>
        /// 是否是ajax请求
        /// </summary>
        public static bool IsAjax(this HttpRequest request)
        {
            return !string.IsNullOrEmpty(request.Headers["X-Requested-With"]);

        }
        /// <summary>
        /// 是否为手机端来源
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool IsFormPhone(this HttpRequest request)
        {
            var headersDictionary = request.Headers;
            var userAgent = headersDictionary[HeaderNames.UserAgent].ToString();
            string browser = "AppleWebKit.*Mobile|MIDP|SymbianOS|NOKIA|SAMSUNG|LG|NEC|TCL|Alcatel|BIRD|DBTEL|Dopod|PHILIPS|HAIER|LENOVO|MOT-|Nokia|SonyEricsson|SIE-|Amoi|ZTE/";
            string uAgent = request.Headers["User-Agent"];
            Regex reg = new Regex(browser);
            return reg.IsMatch(uAgent);

        }

        public static Uri ToUri(this HttpRequest request)
        {
            var hostComponents = request.Host.ToUriComponent().Split(':');

            var builder = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = hostComponents[0],
                Path = request.Path,
                Query = request.QueryString.ToUriComponent()
            };

            if (hostComponents.Length == 2)
            {
                builder.Port = Convert.ToInt32(hostComponents[1]);
            }

            return builder.Uri;
        }

        /// <summary>
        /// 获取Referer
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static string GetReferer(this HttpRequest request)
        {
            var headersDictionary = request.Headers;
            return headersDictionary[HeaderNames.Referer].ToString();
        }
        public static string GetCookieValue(this HttpRequest request, string key)
        {
            string value = "";
            var result = request.Cookies.TryGetValue(key, out value);
            return value;
        }


        public static string GetTheme(this HttpRequest request)
        {
            return GetCookieValue(request, "theme");
        }
        public static void SetTheme(this HttpResponse response, string themevalue)
        {
            try
            {
                response.Cookies.Delete("theme");
            }
            catch
            {

            }
            response.Cookies.Append("theme", themevalue);
        }
        public static void Write(this HttpResponse response, string data)
        {
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(data);
            response.Write(bytes); ;
        }


        public static void Write(this HttpResponse response, byte[] data)
        {
            response.Body.Write(data, 0, data.Length); ;
        }

        public static void ContentTypeOfJson(this HttpResponse response)
        {
            response.ContentType = "application/json;charset:utf-8";
        }

        public static void ToExcel(this HttpResponse response, string filename)
        {
            response.Headers.Add("Content-Disposition", "attachment;filename=" + filename);
            response.ContentType = "application/ms-excel";
        }
    }
}