using Microsoft.AspNetCore.Mvc;

namespace HomeQI.ADream.Identity.Web.Areas.Default.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Area("Default")]
    public class HomeController : Controller
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return Content("test");
        }
    }
}