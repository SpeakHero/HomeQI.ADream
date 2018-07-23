using HomeQI.Adream.Identity;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

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
            return RedirectPermanent("~/swagger/");
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public IActionResult JwtSettings([FromServices]IOptions<JwtSettings> options)
        {
            return Json(options);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public IActionResult IdentityOptions([FromServices]IOptions<IdentityOptions> options)
        {
            return Json(options);
        }
    }
}
