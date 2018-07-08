namespace  HomeQI.ADream.Identity.Options
{
    /// <summary>
    /// Used for store specific options
    /// </summary>
    public class StoreOptions
    {
        /// <summary>
        /// If set to a positive number, the default OnModelCreating will use this value as the max length for any 
        /// properties used as keys, i.e. UserId, LoginProvider, ProviderKey.
        /// </summary>
        public int MaxLengthForKeys { get; set; }

        /// <summary>
        /// 如果设置为true，则商店必须保护用户的所有个人识别数据。
        /// This will be enforced by requiring the store to implement <see cref="IProtectedUserStore"/>.
        /// </summary>
        public bool ProtectPersonalData { get; set; }
    }
}