
namespace HomeQI.ADream.Identity.Store
{
    /// <summary>
    /// 标记接口，用于指示存储支持 <see cref="StoreOptions.ProtectPersonalData"/> flag.
    public interface IProtectedUserStore : IUserStore
    { }
}