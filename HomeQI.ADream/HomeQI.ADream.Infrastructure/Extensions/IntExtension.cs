using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Lake.ADream.Infrastructure.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IntExtension
    {
        /// <summary>
        /// 产生一个随机数
        /// </summary>
        /// <param name="min">最小数</param>
        /// <param name="max">最大数</param>
        /// <returns>Result</returns>
        public static int GenerateRandomInteger(this int min, int max = int.MaxValue)
        {
            var randomNumberBuffer = new byte[10];
            new RNGCryptoServiceProvider().GetBytes(randomNumberBuffer);
            return new Random(BitConverter.ToInt32(randomNumberBuffer, 0)).Next(min, max);
        }
        /// <summary>
        /// 产生一个指定长度的随机数据字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns>字符串</returns>
        public static string GenerateRandomDigitCode(this int length)
        {
            var random = new Random();
            var str = string.Empty;
            for (var i = 0; i < length; i++)
                str = string.Concat(str, random.Next(10).ToString());
            return str;
        }
    }
}
