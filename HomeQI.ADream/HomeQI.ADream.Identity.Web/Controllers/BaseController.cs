using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace HomeQI.ADream.Identity.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public abstract class BaseController : Controller
    {
        public ISession Session => HttpContext.Session;

        protected string CheakCode => Session.GetString(nameof(CheakCode));
        public string ControllerName { get; set; }
        public string ActionName { get; set; }

        public string AreaName { get; set; }
        /// <summary>
        /// 如果是ajax返回json
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        protected IActionResult AjaxOrView(object obj)
        {
            if (Request.IsAjax())
            {
                return Json(obj);
            }
            return View(obj);
        }

        protected async Task<IActionResult> UploadFiles(string dir)
        {
            Logger.LogInformation(nameof(UploadFiles), dir);

            try
            {
                var files = await UploadFile(dir);
                Logger.LogInformation(nameof(UploadFiles), files);
                return Json(new
                {
                    error = 0,
                    url = Url.Content(files[0]),
                    message = "上传成功"
                });
            }
            catch (Exception ex)
            {
                Logger.LogError(nameof(UploadFiles), ex);
                return Json(new { error = 1, message = ex.Message });
            }

        }

        protected async Task<IList<string>> UploadFile(string dir = "image")
        {
            Logger.LogInformation(nameof(UploadFile), dir);

            string url = string.Empty;
            IList<string> urls = new List<string>();
            var files = Request.Form.Files;
            string str = $"{ Env.WebRootPath}Upload/";
            //long size = files.Sum(f => f.Length);
            foreach (var file in files)
            {
                if (file.Length <= 0)
                {
                    break;
                }
                var fileName = file.FileName;
                var extension = Path.GetExtension(fileName);

                fileName = DateTime.Now.ToFileTime().ToString() + extension;
                if (UploadExtension(extension))
                {
                    if (dir.Contains("image"))
                    {
                        str += "img/";
                    }
                    else
                    {
                        str += "file/";
                    }
                    string str2 = DateTime.Now.ToString("yyyyMMddhh");
                    string text2 = str + str2 + "/";
                    if (!Directory.Exists(text2))
                    {
                        Directory.CreateDirectory(text2);
                    }
                    text2 += fileName;
                    var newfilename = text2;
                    using (FileStream fs = System.IO.File.Create(newfilename))
                    {
                        await file.CopyToAsync(fs);
                        fs.Flush();
                    }
                    url += $"~{ newfilename.Replace(Env.WebRootPath, "")}";
                    urls.Add(url);
                }
            }
            Logger.LogInformation(nameof(UploadFile), urls);
            return urls;
        }

        private bool UploadExtension(string extension)
        {
            var extensions = System.IO.File.ReadAllText($@"{ Env.ContentRootPath}/Config/allowedExtensions.dat").Replace(" ", "").Split(',');
            return extensions.Contains(extension);
        }

        public ILogger Logger => LoggerFactory.CreateLogger(GetType());
        public override void OnActionExecuting(ActionExecutingContext context)
        {

            Logger.LogInformation(nameof(OnActionExecuting), context);

            ControllerName = context.RouteData.Values["controller"]?.ToString();
            ActionName = context.RouteData.Values["action"]?.ToString();
            AreaName = context.RouteData.Values["area"]?.ToString();
            //StreamWriter writer = new StreamWriter(Response.Body);
            //var buffer = new MemoryStream();
            //Response.Body.CopyTo(buffer); ;
            //buffer.Position = 0;
            //buffer.CopyTo(writer.BaseStream);
            //buffer.Position = 0;
            //byte[] bytes = new byte[buffer.Length];
            //buffer.Read(bytes, 0, Convert.ToInt32(buffer.Length));
            //var s = Encoding.UTF8.GetString(bytes);
            //writer.Write(Compress(s));
            //buffer.CopyTo(Response.Body);
            base.OnActionExecuting(context);
        }


        public override void OnActionExecuted(ActionExecutedContext context)
        {
            Logger.LogInformation(nameof(OnActionExecuted), context);
            base.OnActionExecuted(context);
        }

        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Logger.LogInformation(nameof(OnActionExecutionAsync), context);
            return base.OnActionExecutionAsync(context, next);
        }
        /// <summary>  
        /// 压缩html代码  
        /// </summary>  
        /// <param name="text">html代码</param>  
        /// <returns></returns>  
        private static string Compress(string text)
        {
            Regex reg = new Regex(@"\s*(</?[^\s/>]+[^>]*>)\s+(</?[^\s/>]+[^>]*>)\s*");
            text = reg.Replace(text, m => m.Groups[1].Value + m.Groups[2].Value);

            reg = new Regex(@"(?<=>)\s|\n|\t(?=<)");
            text = reg.Replace(text, string.Empty);

            return text;
        }
        public ILoggerFactory LoggerFactory => GetRequiredService<ILoggerFactory>();
        public T GetService<T>() => HttpContext.RequestServices.GetService<T>();
        public T GetRequiredService<T>() => HttpContext.RequestServices.GetRequiredService<T>();
        protected IHostingEnvironment Env => GetRequiredService<IHostingEnvironment>();
    }
}
