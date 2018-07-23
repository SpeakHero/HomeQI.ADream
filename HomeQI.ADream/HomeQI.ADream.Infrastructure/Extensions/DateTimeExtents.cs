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

        /// <summary>
        /// 日期转换成unix时间戳
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static long DateTimeToUnixTimestamp(this DateTime dateTime)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, dateTime.Kind);
            return Convert.ToInt64((dateTime - start).TotalSeconds);
        }

        /// <summary>
        /// unix时间戳转换成日期
        /// </summary>
        /// <param name="unixTimeStamp">时间戳（秒）</param>
        /// <returns></returns>
        public static DateTime UnixTimestampToDateTime(this DateTime target, long timestamp)
        {
            var start = new DateTime(1970, 1, 1, 0, 0, 0, target.Kind);
            return start.AddSeconds(timestamp);
        }
    }
}
