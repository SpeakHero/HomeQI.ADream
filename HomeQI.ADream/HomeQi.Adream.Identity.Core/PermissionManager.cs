using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.EntityFrameworkCore;
using System;

namespace HomeQI.Adream.Identity
{
    public class PermissionManager<TPermission> : ManagerBase
        <TPermission, IPermissionStore<TPermission>, IdentityResult, IdentityError>
        where TPermission : EntityBase<string>
    {
        public PermissionManager(IPermissionStore<TPermission> store) : base(store)
        {
            Store = store ?? throw new ArgumentNullEx(nameof(store));
        }
    }
}
