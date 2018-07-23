using System.Threading;
using System.Threading.Tasks;
using HomeQI.ADream.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace HomeQI.Adream.Identity.EntityFrameworkCore
{
    /// <summary>
    /// 
    /// </summary>
    public class PermissionStore : EntityStore<IdentityPermission, IdentityResult, IdentityDbContext, IdentityError>, IPermissionStore<IdentityPermission>
    {
        public PermissionStore(IdentityDbContext context,
            IdentityErrorDescriber errorDescriber,
            ILoggerFactory loggerFactory, IConfiguration configuration) : base(context, errorDescriber,
                loggerFactory, configuration)
        {
        }

        public override async Task<IdentityResult> CreateAsync(IdentityPermission entity, CancellationToken cancellationToken)
        {
            bool flag = await DbEntitySet.AnyAsync(a => a.Controller.Equals(entity.Controller) && a.Action.Equals(entity.Action) && a.AreaName.Equals(entity.AreaName));
            return flag ? IdentityResult.Failed("已经有该记录") : await base.CreateAsync(entity, cancellationToken);
        }

        public override async Task<IdentityResult> UpdateAsync(IdentityPermission entity, CancellationToken cancellationToken, params string[] propertys)
        {
            bool flag = await DbEntitySet.AnyAsync(a => !a.Id.Equals(entity.Id) && a.Controller.Equals(entity.Controller) && a.Action.Equals(entity.Action) && a.AreaName.Equals(entity.AreaName));

            return flag ? IdentityResult.Failed("已经有该记录") : await base.UpdateAsync(entity, cancellationToken, propertys);
        }

    }
}
