
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    ///  //扩展方法必须是静态的
    /// </summary>
    public static class StringHelper
    {
        /// <summary>
        /// IsEmail
        /// </summary>
        /// <param name="_input"></param>
        /// <returns></returns>
        public static bool IsEmail(this string _input)
        {
            if (string.IsNullOrEmpty(_input))
            {
                return false;
            }
            string _emailExpression = @"^((([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+(\.([a-z]|\d|[!#\$%&'\*\+\-\/=\?\^_`{\|}~]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+)*)|((\x22)((((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(([\x01-\x08\x0b\x0c\x0e-\x1f\x7f]|\x21|[\x23-\x5b]|[\x5d-\x7e]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(\\([\x01-\x09\x0b\x0c\x0d-\x7f]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF]))))*(((\x20|\x09)*(\x0d\x0a))?(\x20|\x09)+)?(\x22)))@((([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])|(([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])([a-z]|\d|-||_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])*([a-z]|\d|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))\.)+(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+|(([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])+([a-z]+|\d|-|\.{0,1}|_|~|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])?([a-z]|[\u00A0-\uD7FF\uF900-\uFDCF\uFDF0-\uFFEF])))$";
            return Regex.IsMatch(_input, _emailExpression);
        }
        /// <summary>
        ///   带多个参数的扩展方法
        //在原始字符串前后加上指定的字符
        /// </summary>
        /// <param name="_input"></param>
        /// <param name="_quot"></param>
        /// <returns></returns>
        public static string Quot(this string _input, string _quot)
        {
            return _quot + _input + _quot;
        }

        /// <summary>
        /// 是否为Guid
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsGuid(this string input)
        {
            return Guid.TryParse(input, out Guid g);
        }

        #region 数据转换

        #region 转Int
        /// <summary>
        /// 转Int,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static int ToInt(this string t)
        {
            int n;
            if (!int.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转Int,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static int ToInt(this string t, int pReturn)
        {
            int n;
            if (!int.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是Int true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsInt(this string t)
        {
            int n;
            return int.TryParse(t, out n);
        }
        #endregion

        #region 转Int16
        /// <summary>
        /// 转Int,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static Int16 ToInt16(this string t)
        {
            Int16 n;
            if (!Int16.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转Int,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static Int16 ToInt16(this string t, Int16 pReturn)
        {
            Int16 n;
            if (!Int16.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是Int true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsInt16(this string t)
        {
            Int16 n;
            return Int16.TryParse(t, out n);
        }
        #endregion

        #region 转byte
        /// <summary>
        /// 转byte,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static byte Tobyte(this string t)
        {
            byte n;
            if (!byte.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转byte,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static byte Tobyte(this string t, byte pReturn)
        {
            byte n;
            if (!byte.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是byte true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool Isbyte(this string t)
        {
            byte n;
            return byte.TryParse(t, out n);
        }
        #endregion

        #region 转Long
        /// <summary>
        /// 转Long,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static long ToLong(this string t)
        {

            long n;
            if (!long.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转Long,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static long ToLong(this string t, long pReturn)
        {
            long n;
            if (!long.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是Long true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsLong(this string t)
        {
            long n;
            return long.TryParse(t, out n);
        }
        #endregion

        #region 转Double
        /// <summary>
        /// 转Int,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static double ToDouble(this string t)
        {
            double n;
            if (!double.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转Double,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static double ToDouble(this string t, double pReturn)
        {
            double n;
            if (!double.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是Double true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsDouble(this string t)
        {
            double n;
            return double.TryParse(t, out n);
        }
        #endregion

        #region 转Decimal
        /// <summary>
        /// 转Decimal,失败返回0
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static decimal ToDecimal(this string t)
        {
            decimal n;
            if (!decimal.TryParse(t, out n))
                return 0;
            return n;
        }

        /// <summary>
        /// 转Decimal,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static decimal ToDecimal(this string t, decimal pReturn)
        {
            decimal n;
            if (!decimal.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 是否是Decimal true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsDecimal(this string t)
        {
            decimal n;
            return decimal.TryParse(t, out n);
        }
        #endregion

        #region 转DateTime
        /// <summary>
        /// 转DateTime,失败返回当前时间
        /// </summary>
        /// <param name="e"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string t)
        {
            DateTime n;
            if (!DateTime.TryParse(t, out n))
                return DateTime.Now;
            return n;
        }

        /// <summary>
        /// 转DateTime,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string t, DateTime pReturn)
        {
            DateTime n;
            if (!DateTime.TryParse(t, out n))
                return pReturn;
            return n;
        }

        /// <summary>
        /// 转DateTime,失败返回pReturn
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static string ToDateTime(this string t, string pFormat, string pReturn)
        {
            DateTime n;
            if (!DateTime.TryParse(t, out n))
                return pReturn;
            return n.ToString(pFormat);
        }

        /// <summary>
        /// 转DateTime,失败返回空
        /// </summary>
        /// <param name="e"></param>
        /// <param name="pReturn">失败返回的值</param>
        /// <returns></returns>
        public static string ToDateTime(this string t, string pFormat)
        {
            return t.ToDateTime(pFormat, string.Empty);
        }

        public static string ToShortDateTime(this string t)
        {
            return t.ToDateTime("yyyy-MM-dd", string.Empty);
        }

        /// <summary>
        /// 是否是DateTime true:是 false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsDateTime(this string t)
        {
            DateTime n;
            return DateTime.TryParse(t, out n);
        }
        #endregion

        #region 与int[]相关
        /// <summary>
        /// 转int[],字符串以逗号(,)隔开,请确保字符串内容都合法,否则会出错
        /// </summary>
        /// <param name="pStr"></param>
        /// <returns></returns>
        public static int[] ToIntArr(this string t)
        {
            return t.ToIntArr(new char[] { ',' });
        }

        /// <summary>
        /// 转int[],字符串以逗号(,)隔开,请确保字符串内容都合法,否则会出错
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pSplit">隔开的</param>
        /// <returns></returns>
        public static int[] ToIntArr(this string t, char[] pSplit)
        {
            if (t.Length == 0)
            {
                return new int[] { };
            }

            string[] ArrStr = t.Split(pSplit, StringSplitOptions.None);
            int[] iStr = new int[ArrStr.Length];

            for (int i = 0; i < ArrStr.Length; i++)
                iStr[i] = int.Parse(ArrStr[i]);

            return iStr;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToUpperInvariant(this string s)
        {
            return s.ToUpperInvariant();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ToLowerInvariant(this string s)
        {
            return s.ToLowerInvariant();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="telephone"></param>
        /// <returns></returns>
        public static bool IsTelephone(this string telephone)
        {
            return Regex.IsMatch(telephone, @"^(\d{3,4}-)?\d{6,8}$");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="phone"></param>
        /// <returns></returns>
        public static bool IsPhone(this string phone)
        {
            return Regex.IsMatch(phone, @"^[1]+[3,4,5,7,8]+\d{9}");
        }
        /// <summary>
        /// 身份证验证
        /// </summary>
        /// <param name="str_idcard"></param>
        /// <returns></returns>
        public static bool IsIDcard(this string str_idcard)
        {
            return Regex.IsMatch(str_idcard, @"(^\d{18}$)|(^\d{15}$)");
        }

        /// <summary>
        /// 是否为邮政编码
        /// </summary>
        /// <param name="str_postalcode"></param>
        /// <returns></returns>
        public static bool IsPostalcode(this string str_postalcode)
        {
            return Regex.IsMatch(str_postalcode, @"^\d{6}$");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsChinese(this string str)
        {
            return Regex.IsMatch(str, @"[\u4e00-\u9fbb]");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return String.IsNullOrEmpty(str);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !String.IsNullOrEmpty(str);
        }
        #endregion

        #region 过滤字符串的非int,重新组合成字符串
        /// <summary>
        /// 过滤字符串的非int,重新组合成字符串
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pSplit">分隔符</param>
        /// <returns></returns>
        public static string ClearNoInt(this string t, char pSplit)
        {
            string sStr = string.Empty;
            string[] ArrStr = t.Split(pSplit);

            for (int i = 0; i < ArrStr.Length; i++)
            {
                string lsStr = ArrStr[i];

                if (lsStr.IsInt())
                    sStr += lsStr + pSplit;
                else
                    continue;
            }

            if (sStr.Length > 0)
                sStr = sStr.TrimEnd(pSplit);

            return sStr;
        }

        /// <summary>
        /// 过滤字符串的非int,重新组合成字符串
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ClearNoInt(this string t)
        {
            return t.ClearNoInt(',');
        }
        #endregion

        #region 是否可以转换成int[]
        /// <summary>
        /// 是否可以转换成int[],true:是,false:否
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pSplit">分隔符</param>
        /// <returns></returns>
        public static bool IsIntArr(this string t, char pSplit)
        {
            string[] ArrStr = t.Split(pSplit);
            bool b = true;

            for (int i = 0; i < ArrStr.Length; i++)
            {
                if (!ArrStr[i].IsInt())
                {
                    b = false;
                    break;
                }
            }

            return b;
        }
        /// <summary>
        /// 把字符串转为同名的类型
        /// </summary>
        /// <param name="classname"></param>
        /// <param name="input"></param>
        /// <returns>T</returns>
        public static T CustomedConvert<T>(this string input, T classname) where T : class
        {
            if (classname.GetType() == typeof(string))
            {
                return input as T;
            }

            object result = null;
            result = System.ComponentModel.TypeDescriptor.GetConverter(classname.GetType()).ConvertFrom(input);
            return result as T;
        }
        /// <summary>
        /// 是否可以转换成int[],true:是,false:否
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static bool IsIntArr(this string t)
        {
            return t.IsIntArr(',');
        }
        #endregion

        #endregion

        #region 载取左字符
        /// <summary>
        /// 载取左字符
        /// </summary>
        /// <param name="t"></param>
        /// <param name="pLen">字符个数</param>
        /// <param name="pReturn">超出时后边要加的返回的内容</param>
        /// <returns></returns>
        public static string Left(this string t, int pLen, string pReturn)
        {
            if (t == null || t.Length == 0)
                return string.Empty;
            pLen *= 2;
            int i = 0, j = 0;
            foreach (char c in t)
            {
                if (c > 127)
                {
                    i += 2;
                }
                else
                {
                    i++;
                }

                if (i > pLen)
                {
                    return t.Substring(0, j) + pReturn;
                }

                j++;
            }

            return t;
        }

        public static string Left(this string t, int pLen)
        {
            return Left(t, pLen, string.Empty);
        }

        public static string StrLeft(this string t, int pLen)
        {
            if (t == null)
            {
                return "";
            }
            if (t.Length > pLen)
            {
                return t.Substring(0, pLen);
            }
            return t;
        }
        #endregion

        #region 删除文件名或路径的特殊字符

        private class ClearPathUnsafeList
        {
            public static readonly string[] unSafeStr = { "/", "\\", ":", "*", "?", "\"", "<", ">", "|" };
        }

        /// <summary>
        /// 删除文件名或路径的特殊字符
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string ClearPathUnsafe(this string t)
        {
            foreach (string s in ClearPathUnsafeList.unSafeStr)
            {
                t = t.Replace(s, "");
            }

            return t;
        }
        #endregion

        #region 字符串真实长度 如:一个汉字为两个字节
        /// <summary>
        /// 字符串真实长度 如:一个汉字为两个字节
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static int LengthReal(this string s)
        {
            return Encoding.Unicode.GetBytes(s).Length;
        }
        #endregion

        #region 去除小数位最后为0的
        /// <summary>
        /// 去除小数位最后为0的
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static decimal ClearDecimal0(this string t)
        {
            decimal d;
            if (decimal.TryParse(t, out d))
            {
                return decimal.Parse(double.Parse(d.ToString("g")).ToString());
            }
            return 0;
        }
        #endregion

        #region 进制转换
        /// <summary>
        /// 16进制转二进制
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static string Change16To2(this string t)
        {
            String BinOne = string.Empty;
            String BinAll = string.Empty;
            char[] nums = t.ToCharArray();
            for (int i = 0; i < nums.Length; i++)
            {
                string number = nums[i].ToString();
                int num = Int32.Parse(number, System.Globalization.NumberStyles.HexNumber);

                BinOne = Convert.ToString(num, 2).PadLeft(4, '0');
                BinAll = BinAll + BinOne;
            }
            return BinAll;
        }

        /// <summary>
        /// 二进制转十进制
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static Int64 Change2To10(this string t)
        {
            char[] arrc = t.ToCharArray();
            Int64 all = 0, indexC = 1;
            for (int i = arrc.Length - 1; i >= 0; i--)
            {
                if (arrc[i] == '1')
                {
                    all += indexC;
                }
                indexC = indexC * 2;
            }

            return all;
        }

        /// <summary>
        /// 二进制转换byte[]数组
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static byte[] Change2ToBytes(this string t)
        {
            List<byte> list = new List<byte>();

            char[] arrc = t.ToCharArray();
            byte n = 0;
            char c;
            int j = 0;
            //倒序获取位
            for (int i = arrc.Length - 1; i >= 0; i--)
            {
                c = arrc[i];

                if (c == '1')
                {
                    n += Convert.ToByte(Math.Pow(2, j));
                }
                j++;

                if (j % 8 == 0)
                {
                    list.Add(n);
                    j = 0;
                    n = 0;
                }
            }

            //剩余最高位
            if (n > 0)
                list.Add(n);

            byte[] arrb = new byte[list.Count];

            int j1 = 0;
            //倒序
            for (int i = list.Count - 1; i >= 0; i--)
            {
                arrb[j1] = list[i];
                j1++;
            }
            return arrb;
        }

        /// <summary>
        /// 二进制转化为索引id数据,从右到左
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        public static int[] Change2ToIndex(this string t)
        {
            List<int> list = new List<int>();
            char[] arrc = t.ToCharArray();
            char c;
            int j = 0;

            //倒序获取位
            for (int i = arrc.Length - 1; i >= 0; i--)
            {
                j++;
                c = arrc[i];

                if (c == '1')
                {
                    list.Add(j);
                }
            }

            return list.ToArray();
        }
        #endregion

        /// <summary>
        /// 检查该字符串是否是可用的Ip地址
        /// </summary>
        /// <param name="ipAddress">IP地址验证</param>
        /// <returns>如果字符串是有效的IP地址，则为true，否则为false</returns>
        public static bool IsIpAddress(this string ipAddress)
        {
            return IPAddress.TryParse(ipAddress, out IPAddress _);
        }
        /// <summary>
        /// 映射虚拟路径到物理路径
        /// </summary>
        /// <param name="path">The path to map. E.g. "~/bin"</param>
        ///  <param name="hostingEnvironment">The path to map. E.g. "~/bin"</param>
        /// <returns>The physical path. E.g. "c:\inetpub\wwwroot\bin"</returns>
        public static string MapPath(this string path, IHostingEnvironment hostingEnvironment)
        {
            path = path.Replace("~/", "").TrimStart('/').Replace('/', '\\');
            var baseDirectory = hostingEnvironment.ContentRootPath;
            return Path.Combine(baseDirectory ?? string.Empty, path);
        }
        /// <summary>
        /// 获取两个时间之间相差的年份
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <returns></returns>
        public static int GetDifferenceInYears(this DateTime startDate, DateTime endDate)
        {
            var age = endDate.Year - startDate.Year;
            if (startDate > endDate.AddYears(-age))
                age--;
            return age;
        }
        /// <summary>
        ///  深度优先的递归删除
        /// </summary>
        /// <param name="path">Directory path</param>
        public static void DeleteDirectory(this string path)
        {
            if (string.IsNullOrEmpty(path))
                throw new ArgumentNullEx(path);
            foreach (var directory in Directory.GetDirectories(path))
            {
                DeleteDirectory(directory);
            }

            try
            {
                Directory.Delete(path, true);
            }
            catch (IOException)
            {
                Directory.Delete(path, true);
            }
            catch (UnauthorizedAccessException)
            {
                Directory.Delete(path, true);
            }
        }

        /// <summary>  
        /// 转换值的类型  
        /// </summary>  
        /// <param name="value"></param>  
        /// <param name="p"></param>  
        /// <returns></returns>  
        public static object GetObject(this string value, PropertyInfo p)
        {
            switch (p.PropertyType.Name.ToString().ToLower())
            {
                case "int16":
                    return Convert.ToInt16(value);
                case "int32":
                    return Convert.ToInt32(value);
                case "int64":
                    return Convert.ToInt64(value);
                case "string":
                    return Convert.ToString(value);
                case "datetime":
                    return Convert.ToDateTime(value);
                case "boolean":
                    return Convert.ToBoolean(value);
                case "char":
                    return Convert.ToChar(value);
                case "double":
                    return Convert.ToDouble(value);
                default:
                    return value;
            }
        }
        /// <summary>
        /// json字符串转为类
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="str"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(this string str)
        {
            return JsonConvert.DeserializeObject<T>(str);
        }
        /// <summary>  
        /// 将反射得到字符串转换为对象  
        /// </summary>  
        /// <param name="str">反射得到的字符串</param>  
        /// <returns>实体类</returns>  
        public static T ToClass<T>(this string str)
        {
            string[] array = str.Split(',');
            string[] temp = null;
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            foreach (string s in array)
            {
                temp = s.Split(':');
                dictionary.Add(temp[0], temp[1]);
            }
            Assembly assembly = Assembly.GetAssembly(typeof(T));
            T entry = (T)assembly.CreateInstance(typeof(T).FullName);
            Type type = entry.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                foreach (string key in dictionary.Keys)
                {
                    if (propertyInfos[i].Name == key.ToString())
                    {
                        propertyInfos[i].SetValue(entry, GetObject(p: propertyInfos[i], value: dictionary[key]), null);
                        break;
                    }
                }
            }
            return entry;
        }

        /// <summary>  
        /// 将实体类通过反射组装成字符串  
        /// </summary>  
        /// <param name="t">实体类</param>  
        /// <returns>组装的字符串</returns>  
        public static string ToString<T>(this T t)
        {
            StringBuilder sb = new StringBuilder();
            Type type = t.GetType();
            PropertyInfo[] propertyInfos = type.GetProperties();
            for (int i = 0; i < propertyInfos.Length; i++)
            {
                sb.Append(propertyInfos[i].Name + ":" + propertyInfos[i].GetValue(t, null) + ",");
            }
            return sb.ToString().TrimEnd(new char[] { ',' });
        }
    }
}
