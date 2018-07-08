using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Store;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace HomeQI.ADream.Identity.EntityFrameworkCore
{

    public class RoleStore : RoleClaimStore,
        IQueryableRoleStore,
        IRoleClaimStore
    {
        public RoleStore(IdentityDbContext context, IdentityErrorDescriber errorDescriber, ILoggerFactory loggerFactory) : base(context, errorDescriber, loggerFactory)
        {
        }
        public IQueryable<Role> Roles => DbEntitySet;
    }
}
