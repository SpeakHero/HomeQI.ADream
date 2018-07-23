using HomeQI.Adream.Identity;
using HomeQI.Adream.Identity;
using HomeQI.ADream.Entities.Framework;
using System.Collections;

namespace System.Security.Principal
{
    public static class IdentityExtension
    {
        public static string Role(this IIdentity identity)
        {
            var claim = ((ADreamClaimIdentity)identity).FindFirst("Role");
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static string FullName(this IIdentity identity)
        {
            var claim = ((ADreamClaimIdentity)identity).FindFirst("FullName");
            return (claim != null) ? claim.Value : string.Empty;
        }

        public static ArrayList PermissionList(this IIdentity identity)
        {
            return ((ADreamClaimIdentity)identity).PermissionList;
        }
        public static IdentityUser IdentityUser(this IIdentity identity)
        {
            return ((ADreamClaimIdentity)identity).IdentityUser as IdentityUser;
        }
        public static T IdentityUser<T>(this IIdentity identity) where T : IdentityUser
        {
            return ((ADreamClaimIdentity)identity).IdentityUser as T;
        }
    }
}
