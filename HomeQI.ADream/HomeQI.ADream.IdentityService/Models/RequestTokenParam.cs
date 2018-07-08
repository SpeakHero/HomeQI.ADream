using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQI.ADream.IdentityService.Models
{
    public class LoginRequestParam
    {
        public string UserName { get; set; }
        public string Password { get; set; }

        public string UId { get; set; }
        public string Sub { get; set; }
        public DateTime RequstTime { get; set; }
        public string ClientId { get; set; }
    }
}
