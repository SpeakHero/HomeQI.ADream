
namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public class KnownException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public KnownException(string message) : base(string.IsNullOrEmpty(message) ? "未知异常" : message)
        {

        }
    }

}
