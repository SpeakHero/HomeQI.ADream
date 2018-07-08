using HomeQI.Adream.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;

namespace HomeQi.Adream.Identity
{
    public class UserManager : UserManager<IdentityUser>
    {
        public UserManager(IUserStore<IdentityUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<IdentityUser> passwordHasher, IEnumerable<IUserValidator<IdentityUser>> userValidators, IEnumerable<IPasswordValidator<IdentityUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<IdentityUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
