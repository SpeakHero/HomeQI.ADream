using HomeQI.ADream.IdentityService.Models;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace HomeQI.ADream.IdentityService.Controllers
{
    [Produces("application/json-patch+json")]
    [Route("api/[controller]/[action]")]
    public class AccoutController : ApiControllerBase
    {
        private readonly IConfiguration configuration;
        public AccoutController(IConfiguration _configuration)
        {
            configuration = _configuration;
        }

        [HttpPost]
        public async Task<ActionResult> RequestToken([FromBody]LoginRequestParam model)
        {
            Dictionary<string, string> dict = new Dictionary<string, string>
            {
                ["client_id"] = model.ClientId,
                ["client_secret"] = configuration[$"IdentityClients:{model.ClientId}:ClientSecret"],
                ["grant_type"] = configuration[$"IdentityClients:{model.ClientId}:GrantType"],
                ["username"] = model.UserName,
                ["password"] = model.Password
            };

            using (HttpClient http = new HttpClient())
            using (var content = new FormUrlEncodedContent(dict))
            {
                var msg = await http.PostAsync(configuration["IdentityService:TokenUri"], content);
                if (!msg.IsSuccessStatusCode)
                {
                    return StatusCode(Convert.ToInt32(msg.StatusCode));
                }

                string result = await msg.Content.ReadAsStringAsync();
                return Json(result);
            }
        }
    }
}