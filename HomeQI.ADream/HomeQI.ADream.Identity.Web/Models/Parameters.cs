using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Web.Models
{
    public class Parameters
    {
        public string Grant_type { get; set; }
        public string Refresh_token { get; set; }
        public string Client_id { get; set; }
        public string Client_secret { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
