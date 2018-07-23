using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace HomeQI.Adream.Identity
{
    public class UserManager : UserManager<IdentityUser>
    {
        public UserManager(IUserStore<IdentityUser> store,
            IOptions<IdentityOptions> optionsAccessor,
            IPasswordHasher<IdentityUser> passwordHasher, IUserValidator<IdentityUser>
            userValidators, IPasswordValidator<IdentityUser> passwordValidators,
            ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors,
            IServiceProvider services, ILogger<UserManager<IdentityUser>> logger) :
            base(store, optionsAccessor, passwordHasher,
                userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}
