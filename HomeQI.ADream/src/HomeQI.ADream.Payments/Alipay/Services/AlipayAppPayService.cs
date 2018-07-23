using System.Threading.Tasks;
using HomeQI.ADream.Payments.Abstractions;
using HomeQI.ADream.Payments.Configs;
using HomeQI.ADream.Payments.Parameters;
using HomeQI.ADream.Payments.Parameters.Requests;
using HomeQI.ADream.Payments.Services.Base;
using Util.Biz.Payments.Core;

namespace HomeQI.ADream.Payments.Services {
    /// <summary>
    /// 支付宝App支付服务
    /// </summary>
    public class AlipayAppPayService : AlipayServiceBase, IAlipayAppPayService {
        /// <summary>
        /// 初始化支付宝App支付服务
        /// </summary>
        /// <param name="provider">支付宝配置提供器</param>
        public AlipayAppPayService( IAlipayConfigProvider provider ) : base( provider ) {
        }

        /// <summary>
        /// 支付
        /// </summary>
        /// <param name="request">支付参数</param>
        public async Task<string> PayAsync( AlipayAppPayRequest request ) {
            var result = await PayAsync( request.ToParam() );
            return result.Result;
        }

        /// <summary>
        /// 请求结果
        /// </summary>
        protected override Task<PayResult> RequstResult( AlipayConfig config, AlipayParameterBuilder builder ) {
            var result = builder.Result( true );
            WriteLog( config, builder, result );
            return Task.FromResult( new PayResult { Result = result } );
        }

        /// <summary>
        /// 获取请求方法
        /// </summary>
        protected override string GetMethod() {
            return "alipay.trade.app.pay";
        }

        /// <summary>
        /// 获取支付方式
        /// </summary>
        protected override PayWay GetPayWay() {
            return PayWay.AlipayAppPay;
        }
    }
}