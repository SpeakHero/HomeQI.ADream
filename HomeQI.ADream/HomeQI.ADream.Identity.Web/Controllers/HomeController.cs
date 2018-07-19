using HomeQI.ADream.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
namespace HomeQI.ADream.Identity.Web.Controllers
{
    /// <summary>
    ///  
    /// </summary>
    public class HomeController : MvcController
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return RedirectToPage("~/swagger/index.html");
        }

    }
}
