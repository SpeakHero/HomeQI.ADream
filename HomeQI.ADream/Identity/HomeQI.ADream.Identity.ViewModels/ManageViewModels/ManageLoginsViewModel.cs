using HomeQI.Adream.Identity;
using Microsoft.AspNetCore.Http.Authentication;
using System.Collections.Generic;

namespace HomeQI.ADream.Identity.ManageViewModels
{
    public class ManageLoginsViewModel
    {
        public IList<UserLoginInfo> CurrentLogins { get; set; }

        public IList<AuthenticationDescription> OtherLogins { get; set; }
    }
}
