using HomeQI.Adream.Identity;
using HomeQI.ADream.Entities.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace HomeQI.Adream.Identity
{
    public class ADreamClaimIdentity : ClaimsIdentity
    {
        public ADreamClaimIdentity()
        {
        }

        public ADreamClaimIdentity(IEnumerable<Claim> claims) : base(claims)
        {
        }

        public ADreamClaimIdentity(BinaryReader reader) : base(reader)
        {
        }

        public ADreamClaimIdentity(IIdentity identity) : base(identity)
        {
        }

        public ADreamClaimIdentity(string authenticationType) : base(authenticationType)
        {
        }



        public ADreamClaimIdentity(IEnumerable<Claim> claims, string authenticationType) : base(claims, authenticationType)
        {
        }

        public ADreamClaimIdentity(IIdentity identity, IEnumerable<Claim> claims) : base(identity, claims)
        {
        }

        public ADreamClaimIdentity(string authenticationType, string nameType, string roleType) : base(authenticationType, nameType, roleType)
        {
        }

        public ADreamClaimIdentity(IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) : base(claims, authenticationType, nameType, roleType)
        {
        }

        public ADreamClaimIdentity(IIdentity identity, IEnumerable<Claim> claims, string authenticationType, string nameType, string roleType) : base(identity, claims, authenticationType, nameType, roleType)
        {
        }

        protected ADreamClaimIdentity(ClaimsIdentity other) : base(other)
        {
        }

        protected ADreamClaimIdentity(SerializationInfo info) : base(info)
        {
        }

        protected ADreamClaimIdentity(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }

        public IEntityBase<string> IdentityUser { get; set; }
        /// <summary>
        /// 用户权限列表
        /// </summary>

        public ArrayList PermissionList { get; set; }
    }
}
