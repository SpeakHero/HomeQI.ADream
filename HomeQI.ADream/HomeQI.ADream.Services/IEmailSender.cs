using System.Threading.Tasks;

namespace HomeQI.ADream.Services
{
    public interface IEmailSender
    {
        /// <summary>
        /// 异步发送邮件
        /// </summary>
        /// <param name="email">地址</param>
        /// <param name="subject">标题</param>
        /// <param name="message">新消息</param>
        /// <returns></returns>
        Task SendEmailAsync(string email, string subject, string message);
    }
}