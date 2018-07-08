using System;
using System.Net.Http;

namespace System
{
    public static class IpHelper
    {
        public static string GetAddress(this string ip)
        {
            var url = "http://ip.taobao.com/service/getIpInfo.php?ip=" + ip;
            try
            {
                using (var client = new HttpClient())
                {
                    var html = client.GetAsync(url).Result.Content.ReadAsStringAsync().Result;
                    return Serializer.FromJson<IpEntity>(html).GetAddress();
                }
            }
            catch
            {
                return string.Empty;
            }
        }
    }

    internal class IpEntity
    {
        public string Code { get; set; }
        public IpEntityValue Data { get; set; }

        public string GetAddress()
        {
            if (this.Data == null)
            {
                return string.Empty;
            }
            return this.Data.Country + this.Data.City;
        }
    }

    internal class IpEntityValue
    {
        public string Country { get; set; }
        public string City { get; set; }
    }
}
