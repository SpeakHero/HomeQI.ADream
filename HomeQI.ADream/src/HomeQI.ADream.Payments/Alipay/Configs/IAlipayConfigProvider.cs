using System.Threading.Tasks;

namespace HomeQI.ADream.Payments.Configs {
    /// <summary>
    /// 支付宝配置提供器
    /// </summary>
    public interface IAlipayConfigProvider {
        /// <summary>
        /// 获取配置
        /// </summary>
        Task<AlipayConfig> GetConfigAsync();
    }
}