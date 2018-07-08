using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace System
{
    public class InvalidOperationEx : InvalidOperationException
    {
        public InvalidOperationEx()
        {
            LogerHelp.NLoger.Error("errror");
        }

        public InvalidOperationEx(string message) : base(message)
        {
            LogerHelp.NLoger.Error(message);
        }

        public InvalidOperationEx(string message, Exception innerException) : base(message, innerException)
        {
            LogerHelp.NLoger.Error(innerException, message);
        }

        protected InvalidOperationEx(SerializationInfo info, StreamingContext context) : base(info, context)
        {
            LogerHelp.NLoger.Error(context);
            LogerHelp.NLoger.Error(info);
        }
    }
}
