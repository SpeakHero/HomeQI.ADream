using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace HomeQI.ADream.Identity.Entites
{
    public class UserLoginInfo
    {
        /// <summary>
        /// Creates a new instance of <see cref="UserLoginInfo"/>
        /// </summary>
        /// <param name="loginProvider">The provider associated with this login information.</param>
        /// <param name="providerKey">The unique identifier for this user provided by the login provider.</param>
        /// <param name="displayName">The display name for this user provided by the login provider.</param>
        public UserLoginInfo(string loginProvider, string providerKey, string displayName)
        {
            LoginProvider = loginProvider;
            ProviderKey = providerKey;
            ProviderDisplayName = displayName;
        }
        [Required]
        public string LoginProvider { get; set; }

        [Required]
        public string ProviderKey { get; set; }
        [Required]
        public string ProviderDisplayName { get; set; }
    }
}
