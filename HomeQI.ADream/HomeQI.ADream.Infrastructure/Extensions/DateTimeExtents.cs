using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class DateTimeExtents
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        public static DateTime ToDateTime(this byte[] bytes, int offset)
        {
            if (bytes != null)
            {
                long ticks = BitConverter.ToInt64(bytes, offset);
                if (ticks < DateTime.MaxValue.Ticks && ticks > DateTime.MinValue.Ticks)
                {
                    DateTime dt = new DateTime(ticks);
                    return dt;
                }
            }
            return new DateTime();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="dt"></param>
        /// <returns></returns>

        public static byte[] ToBytes(this DateTime dt)
        {
            return BitConverter.GetBytes(dt.Ticks);
        }
    }
}
