using System.Linq;
using System.Text;

namespace System.Net.Http
{
    /// <summary>
    /// 
    /// </summary>
     public static class HttpClientExtentx
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="client"></param>
        /// <param name="requestUri"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public static HttpResponseMessage PostAsyncSafe(this HttpClient client, string requestUri, string content)
        {
            var requestContent = new StringContent(content, Encoding.UTF8, "application/json-patch+json");
            return PerformActionSafe(() => (client.PostAsync(requestUri, requestContent)).Result);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public static HttpResponseMessage PerformActionSafe(Func<HttpResponseMessage> action)
        {
            try
            {
                return action();
            }
            catch (AggregateException aex)
            {
                Exception firstException = null;
                if (aex.InnerExceptions != null && aex.InnerExceptions.Any())
                {
                    firstException = aex.InnerExceptions.First();

                    if (firstException.InnerException != null)
                        firstException = firstException.InnerException;
                }

                var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
                {
                    Content =
                        new StringContent(firstException != null
                                            ? firstException.ToString()
                                            : "遇到一个总闸阀异常没有任何内部异常")
                };

                return response;
            }
        }
    }
}
