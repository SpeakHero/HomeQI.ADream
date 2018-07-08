using HomeQI.ADream.Entities.Framework;

namespace HomeQI.ADream.Identity.Entites
{

    /// <summary>
    /// 表示用户的登录及其关联提供程序。
    /// </summary>
    public class UserLogin : EntityBase
    {
        /// <summary>
        /// 获取或设置登录的登录提供程序（例如脸谱网、谷歌）。
        /// </summary>
        public virtual string LoginProvider
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置此登录的唯一提供者标识符。
        /// </summary>
        public virtual string ProviderKey
        {
            get;
            set;
        }

        /// <summary>
        ///获取或设置此登录的UI中使用的友好名称。
        /// </summary>
        public virtual string ProviderDisplayName
        {
            get;
            set;
        }

        /// <summary>
        /// 获取或设置与此登录相关联的用户的主键。
        /// </summary>
        public virtual string UserId
        {
            get;
            set;
        }
        public User User { get; set; }
    }
}
