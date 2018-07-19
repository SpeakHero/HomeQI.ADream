using System.Threading.Tasks;

namespace HomeQI.ADream.Services
{
    public interface ISmsSender
    {/// <summary>
     /// 
     /// </summary>
     /// <param name="number"></param>
     /// <param name="message"></param>
     /// <param name="SendTime"></param>
     /// <param name="Cell"></param>
     /// <returns></returns>
        Task<string> SendSmsAsync(string number, string message, string SendTime = "", string Cell = "");
    }
}