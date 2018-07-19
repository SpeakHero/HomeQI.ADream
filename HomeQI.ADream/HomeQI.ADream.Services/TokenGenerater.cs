using Microsoft.AspNetCore.Http;
using System;
using System.Security.Encrypt;
using System.Text;
using System.Threading.Tasks;
namespace HomeQI.ADream.Services
{
    /// <summary>
    /// 令牌生成服务
    /// </summary>
    public class TokenGenerater
    {
        private readonly HttpContext context;
        private readonly ISession session;

        public TokenGenerater(IHttpContextAccessor contextAccessor)
        {
            if (contextAccessor == null)
            {
                throw new ArgumentNullEx(nameof(contextAccessor));
            }
            context = contextAccessor.HttpContext ?? throw new ArgumentNullEx(nameof(contextAccessor.HttpContext));
            session = contextAccessor.HttpContext.Session ?? throw new ArgumentNullEx(nameof(contextAccessor.HttpContext.Session)); ;
        }
        public bool VerifGenerateRandomCode(string code)
        {
            return session.GetGenerateRandomCode().Equals(code);
        }
        /// <summary>
        ///生成制定位数的随机码（数字）
        /// </summary>
        /// <param name="length">定位数</param>
        /// <returns></returns>
        public string GenerateRandomCode(int length = 6)
        {
            var result = new StringBuilder();
            for (var i = 0; i < length; i++)
            {
                var rnd = new Random(Guid.NewGuid().GetHashCode());
                result.Append(rnd.Next(0, 10));
            }
            var r = result.ToString();
            session.SetString(nameof(GenerateRandomCode), r);
            return r;
        }
        /// <summary>
        /// 生成制定位数的随机码（数字）
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public async Task<string> GenerateRandomCodeAsync(int length = 6)
        {
            return await Task.FromResult(GenerateRandomCode(length));
        }
        /// <summary>
        /// 生成邮箱验证码
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public async Task<string> GenerateEmailAsync(string email)
        {
            if (email.IsNullOrEmpty())
            {
                throw new ArgumentNullEx(nameof(email));
            }
            var tokenmodel = new TokenModel() { Sid = session.Id, Name = email, RequestIp = context.GetUserIp() }.ToJson();
            var len = tokenmodel.LengthReal();

            var token = await Task.FromResult(SecurityHelper.AES256Encrypt(tokenmodel));
            session.SetObjectAsJson(nameof(GenerateEmailAsync), tokenmodel);
            return token;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<bool> VerifByEmailAsync(string code)
        {
            return await Task.FromResult(VerifByEmail(code));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="email"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public bool VerifByEmail(string code)
        {
            var token = SecurityHelper.AES256DEncrypt(code)?.DeserializeObject<TokenModel>();
            TokenModel tokenmodel = session.GetObjectFromJson<TokenModel>(nameof(GenerateEmailAsync));
            if (token == null)
            {
                return false;
            }
            var uip = context.GetUserIp();
            if (uip.IsNotNullOrEmpty())
            {
                if (!token.RequestIp.Equals(uip))
                {
                    return false;
                }
            }
            return (tokenmodel.Name == token.Name && token.Sid == tokenmodel.Sid && DateTime.Now < token.ResponseTime.AddMinutes(token.ExpiryTime));
        }
    }
    public enum TokenGeneraterType
    {
        Email, Phone, Other
    }
    public class TokenModel
    {
        /// <summary>
        /// 过期时间 分钟为单位
        /// </summary>
        public int ExpiryTime { get; set; } = 10;
        public string RequestIp { get; set; }
        /// <summary>
        /// token生成时间
        /// </summary>
        public DateTime ResponseTime { get; set; } = DateTime.Now;
        public string Name { get; set; }
        public object Result { get; set; }
        /// <summary>
        /// 会话id
        /// </summary>
        public string Sid { get; set; }
        public string Code { get; set; } = Guid.NewGuid().ToString("N");
    }
}
