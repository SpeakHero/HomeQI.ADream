//using System;
//using System.Collections.Generic;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Security.Claims;
//using System.Text;
//using System.Threading.Tasks;
//using HomeQI.ADream.Infrastructure.Core;
//using IdentityModel;
//using Microsoft.AspNetCore.Mvc;
//using Microsoft.IdentityModel.Tokens;

//namespace HomeQI.ADream.IdentityService.Controllers
//{
//    public class OAuthController : ApiControllerBase
//    {
//        private UserStore _store;

//        public OAuthController(UserStore store)
//        {
//            _store = store;
//        }


//        [HttpPost("auth")]
//        public IActionResult Authenticate([FromBody]UserDto userDto)
//        {
//            var user = _store.FindUser(userDto.UserName, userDto.Password);
//            if (user == null) return Unauthorized();
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.UTF8.GetBytes(Consts.Secret);
//            var authTime = DateTime.UtcNow;
//            var expiresAt = authTime.AddDays(7);
//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(new Claim[]
//                {
//                    new Claim(JwtClaimTypes.Audience,"api"),
//                    new Claim(JwtClaimTypes.Issuer,"http://localhost:5200"),
//                    new Claim(JwtClaimTypes.Id, user.Id.ToString()),
//                    new Claim(JwtClaimTypes.Name, user.Name),
//                    new Claim(JwtClaimTypes.Email, user.Email),
//                    new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber)
//                }),
//                Expires = expiresAt,
//                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
//            };
//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            var tokenString = tokenHandler.WriteToken(token);
//            return Ok(new
//            {
//                access_token = tokenString,
//                token_type = "Bearer",
//                profile = new
//                {
//                    sid = user.Id,
//                    name = user.Name,
//                    auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
//                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds()
//                }
//            });
//        }


//        public class UserDto
//        {
//            public string UserName { get; set; }

//            public string Password { get; set; }
//        }
//    }
//}