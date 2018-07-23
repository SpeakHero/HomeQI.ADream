using HomeQI.Adream.Identity;
using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.EntityFrameworkCore;
using System;

namespace HomeQI.Adream.Identity
{
    public interface IPermissionStore<TPermission> : IEntityStore<TPermission, IdentityResult, IdentityError>, IDisposable where TPermission : EntityBase<string>
    {
    }
}
