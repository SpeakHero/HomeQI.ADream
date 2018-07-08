using System;
using System.Collections.Generic;
using System.Text;

namespace System.Net.Http
{
    public static class HttpContextEx
    {
        public static object User(this HttpContent httpContent)
        {
            return httpContent;
        }
    }
}
