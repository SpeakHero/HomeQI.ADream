using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Infrastructure.Core
{
    //  [Produces("application/*+json")]
    [Route("api/[controller]/[action]")]
    //[ApiController]
    [Authorize]
    public abstract class ApiControllerBase : BaseController
    {
    }

    public abstract class MvcController : BaseController
    {

    }

    public abstract class BaseController : Controller
    {
        protected ResponseResult VCheakCode(string code)
        {
            if (code.IsNullOrEmpty())
            {
                return ResponseResult.Failed("图形验证码不能为空 ");
            }
            if (Session == null)
            {
                return ResponseResult.Failed("请刷新浏览器 ");
            }
            if (CheakCode == null)
            {
                return ResponseResult.Failed("图形验证码不正确");
            }
            if (!CheakCode.Equals(code))
            {
                return ResponseResult.Failed("图形验证码不正确");
            }
            Session.SetString(nameof(CheakCode), string.Empty);
            return ResponseResult.Success();
        }
        protected ISession Session => HttpContext.Session;
        protected IDistributedCache Cache => GetService<IDistributedCache>();
        protected string CheakCode => Session.GetString(nameof(CheakCode));
        protected string ControllerName { get; set; }
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
        [HttpPost]
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
        protected ILoggerFactory LoggerFactory => GetRequiredService<ILoggerFactory>();
        public T GetService<T>() => HttpContext.RequestServices.GetService<T>();
        public T GetRequiredService<T>() => HttpContext.RequestServices.GetRequiredService<T>();
        public IHostingEnvironment Env => GetRequiredService<IHostingEnvironment>();

        protected string GetModelErrs()
        {
            StringBuilder sb = new StringBuilder();
            var errors = ModelState.Values;
            foreach (var item in errors)
            {
                foreach (var item2 in item.Errors)
                {
                    if (item2.ErrorMessage.IsNotNullOrEmpty())
                    {
                        sb.AppendLine(item2.ErrorMessage + "<br />");
                    }
                }
            }
            return sb.ToString();
        }
        protected async Task SetCacheAsync<T>(string key, T objects, CancellationToken cancellationToken = default) where T : class
        {
            await Cache.SetStringAsync(key, objects.ToJson(), cancellationToken);
        }
        protected async Task<T> GetCacheAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullEx(nameof(key), "key Is Null Or Empty");
            }
            var cacheValue = await Cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cacheValue))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(cacheValue);
                }
                catch (Exception ex)
                {
                    LogerHelp.Error(ex);
                    //出错时删除key
                    await Cache.RefreshAsync(key);
                }
            }
            return default;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected IActionResult Failed()
        {
            List<ResponseError> responseErrors = new List<ResponseError>();
            var keys = ModelState.Keys.ToArray();
            var vales = ModelState.Values.ToArray();
            for (int i = 0; i <= ModelState.ErrorCount - 1; i++)
            {
                var dies = string.Empty;
                if (vales != null)
                {
                    string key = string.Empty;
                    if (keys.Length > i)
                    {
                        key = keys[i];
                        foreach (var item in ModelState[key].Errors)
                        {
                            if (item.ErrorMessage.IsNotNullOrEmpty())
                            {
                                dies += item.ErrorMessage + ";";
                            }
                        }
                    }
                    else
                    {
                        foreach (var item in ModelState.Values)
                        {
                            foreach (var item2 in item.Errors)
                            {
                                dies += item2.ErrorMessage + ";";
                            }
                        }
                    }
                    responseErrors.Add(new ResponseError
                    {
                        Code = key,
                        Description = dies
                    });

                }
            }
            return Json(ResponseResult.Failed(responseErrors.ToArray()));
        }
    }
}
