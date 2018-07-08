using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using NLog;
namespace System
{
    /// <summary>
    /// 
    /// </summary>
    public static class CheakArguments
    {
        /// <summary>
        /// 参数不能null检查
        /// </summary>
        public static void CheakArgument(this object obj)
        {
            if (obj == null)
            {
                var ex = new ArgumentException(nameof(obj));
                LogManager.GetCurrentClassLogger().Log(LogLevel.Error, ex.Message, ex);
                throw ex;
            }
        }
    }
}
