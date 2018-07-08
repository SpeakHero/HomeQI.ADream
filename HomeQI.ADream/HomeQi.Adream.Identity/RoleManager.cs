using HomeQI.Adream.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeQi.Adream.Identity
{
    public class RoleManager : RoleManager<IdentityRole>
    {
        public RoleManager(IRoleStore<IdentityRole> store, IEnumerable<IRoleValidator<IdentityRole>> roleValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, ILogger<RoleManager<IdentityRole>> logger) : base(store, roleValidators, keyNormalizer, errors, logger)
        {
        }
    }
}
