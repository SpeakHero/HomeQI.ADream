using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace HomeQI.ADream.Identity.EntityFrameworkCore
{

    public class UserStore : UserOnlyStore
    {
        public UserStore(IdentityDbContext context, IdentityErrorDescriber errorDescriber, ILoggerFactory loggerFactory) : base(context, errorDescriber, loggerFactory)
        {
        }
    }
}
