using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Web.Models
{
    public class ResponseData
    {
        public string Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}
