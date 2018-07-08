using NLog;
using System;
using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class LogerHelp
    {


        public static Logger NLoger => LogManager.GetCurrentClassLogger();
        public static void Error(string msg)
        {
            NLoger.Log(LogLevel.Error, msg);
        }
        public static void Error(object ex)
        {
            NLoger.Log(LogLevel.Error, ex);
        }
        public static void Error(Exception ex)
        {
            NLoger.Log(LogLevel.Error, ex);
        }

        /////////////////////
        public static void Info(string msg)
        {
            NLoger.Log(LogLevel.Info, msg);
        }
        public static void Info(object ex)
        {
            NLoger.Log(LogLevel.Info, ex);
        }
        public static void Info(Exception ex)
        {
            NLoger.Log(LogLevel.Info, ex);
        }
        //////////////////
        public static void Warn(string msg)
        {
            NLoger.Log(LogLevel.Warn, msg);
        }
        public static void Warn(object ex)
        {
            NLoger.Log(LogLevel.Info, ex);
        }
        public static void Warn(Exception ex)
        {
            NLoger.Log(LogLevel.Warn, ex);
        }
    }
}
