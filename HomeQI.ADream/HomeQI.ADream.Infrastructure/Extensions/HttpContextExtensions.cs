using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpContextExtensions
    {
        public static string GetUserIp(this HttpContext context)
        {
            var ip = context.Request.Headers["X-Forwarded-For"].FirstOrDefault();
            if (string.IsNullOrEmpty(ip))
            {
                ip = context.Connection.RemoteIpAddress.ToString();
            }
            return ip;
        }
        //// <summary>
        /// 判断是否是IP地址格式 0.0.0.0
        /// </summary>
        /// <param name="str1">待判断的IP地址</param>
        /// <returns>true or false</returns>
        public static bool IsIPAddress(this string str1)
        {
            if (str1 == null || str1 == string.Empty || str1.Length < 7 || str1.Length > 15) return false;
            string regformat = @"^\d{1,3}[\.]\d{1,3}[\.]\d{1,3}[\.]\d{1,3}$";
            Regex regex = new Regex(regformat, RegexOptions.IgnoreCase);
            return regex.IsMatch(str1);
        }
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