using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


namespace HomeQI.ADream.Services
{
    public class MessageSender : IEmailSender, ISmsSender
    {
        private readonly MailConfig mailConfig;
        private readonly ILogger logger;
        private readonly IOptions<SmsConfig> smsoptions;
        private readonly IHttpClientFactory httpClientFactory;

        public MessageSender(IOptions<MailConfig> emailoptions, IOptions<SmsConfig> smsoptions, IHttpClientFactory httpClientFactory, ILoggerFactory loggerFactory)
        {
            if (emailoptions == null)
            {
                throw new ArgumentNullEx(nameof(emailoptions));
            }

            logger = loggerFactory.CreateLogger(GetType());
            mailConfig = emailoptions.Value;
            this.smsoptions = smsoptions ?? throw new ArgumentNullException(nameof(smsoptions));
            this.httpClientFactory = httpClientFactory ?? throw new ArgumentNullException(nameof(httpClientFactory));

        }

        // public EmailMessageSenderOptions Options { get; private set; }
        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (!email.IsEmail())
            {
                return;
            }
            if (mailConfig.RequireValid)
            {
                MimeMessage mailmessage = new MimeMessage();
                mailmessage.From.Add(new MailboxAddress(mailConfig.Title, mailConfig.Uid));
                mailmessage.To.Add(new MailboxAddress(subject, email));
                mailmessage.Subject = subject;

                var plain = new TextPart("plain")
                {
                    Text = message
                };
                var html = new TextPart("html")
                {
                    Text = message
                };

                var alternative = new Multipart("alternative")
                {
                    plain,
                    html
                };

                var multipart = new Multipart("mixed")
                {
                    alternative
                };
                mailmessage.Body = multipart;

                using (var client = new SmtpClient())
                {
                    client.Connect(mailConfig.Server, mailConfig.Port, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");

                    client.Authenticate(mailConfig.Uid, mailConfig.Pwd);
                    try
                    {
                        await client.SendAsync(mailmessage);
                        await client.DisconnectAsync(true);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex.Message, ex);
#if Debug
                          throw ;
#endif

                    }


                }
            }

        }
        /// <summary>
        /// 短信发送
        /// </summary>
        /// <param name="number">电话号码多个号码用逗号隔开</param>
        /// <param name="message">发送内容</param>
        /// <param name="SendTime">定时时间</param>
        /// <param name="Cell">抄送</param>
        /// <returns></returns>
        public async Task<string> SendSmsAsync(string number, string message, string SendTime = "", string Cell = "")
        {

            //  return await Task.FromResult(message);
            var flag = await SmsFirewall(number, message);
            if (!flag)
            {
                return "-50";
            }

            var opt = smsoptions.Value;
            string url = opt.Url;
            string CorpID = opt.CorpID;
            string Pwd = opt.Pwd;
            using (var client = httpClientFactory.CreateClient())
            {
                client.MaxResponseContentBufferSize = 256000;
                client.DefaultRequestHeaders.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/54.0.2840.71 Safari/537.36");
                client.DefaultRequestHeaders.Add("Referer", "http://www.365fenqi.cn/");
                List<KeyValuePair<String, String>> paramList = new List<KeyValuePair<String, String>>
                {
                    new KeyValuePair<string, string>("Content-Type", "application/x-www-form-urlencoded"),
                    new KeyValuePair<string, string>("CorpID", CorpID),
                    new KeyValuePair<string, string>("Pwd", Pwd),
                    new KeyValuePair<string, string>("Mobile", number),
                    new KeyValuePair<string, string>("Content", message),
                    new KeyValuePair<string, string>("Cell", Cell),
                    new KeyValuePair<string, string>("SendTime", SendTime)
                };
                var response = await client.PostAsync(new Uri(url), new FormUrlEncodedContent(paramList));
                return await response.Content.ReadAsStringAsync();
            }

        }

        public async Task<bool> SmsFirewall(string number, string message)
        {

            return await Task.FromResult(true);
        }
    }

    public class MailConfig
    {
        [JsonProperty("EnablePwdCheck")]
        public bool EnablePwdCheck { get; set; }
        [JsonProperty("EnableSSL")]
        public bool EnableSSL { get; set; }
        [JsonProperty("Port")]
        public int Port { get; set; }
        [JsonProperty("Pwd")]
        public string Pwd { get; set; }
        [JsonProperty("RequireValid")]
        public bool RequireValid { get; set; }
        [JsonProperty("Server")]
        public string Server { get; set; }
        [JsonProperty("Title")]
        public string Title { get; set; }
        [JsonProperty("Uid")]
        public string Uid { get; set; }

    }
    public class SmsConfig
    {
        /// <summary>
        /// dd
        /// </summary>
        public string CorpID { get; set; }
        /// <summary>
        /// ff
        /// </summary>
        public string Pwd { get; set; }
        /// <summary>
        /// https://mb345.com/utf8/BatchSend2.aspx
        /// </summary>
        public string Url { get; set; }
    }
}