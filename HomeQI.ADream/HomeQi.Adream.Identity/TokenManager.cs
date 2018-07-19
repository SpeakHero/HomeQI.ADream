using HomeQI.Adream.Identity;
using HomeQI.ADream.Identity;
using IdentityModel;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HomeQi.Adream.Identity
{
    public class TokenManager
    {
        private readonly JwtSettings _settings;
        private readonly UserManager userManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly SignInManager signInManager;
        public TokenManager(IOptions<JwtSettings> settings, IHttpContextAccessor httpContextAccessor, SignInManager signInManager, UserManager aspNetUserManager)
        {
            if (settings == null)
            {
                throw new ArgumentNullEx(nameof(settings));
            }

            _settings = settings.Value;
            this.httpContextAccessor = httpContextAccessor;
            this.signInManager = signInManager;
            userManager = aspNetUserManager;
        }

        public async Task<SignInResult> PasswordSignInAsync(IdentityUser user, string password)
        {
            var result = await signInManager.PasswordSignInAsync(user, password, true, true);
            if (result.Succeeded)
            {
                return await SignInResultAsync(user);
            }
            return SignInResult.Failed("登录失败");
        }
        public async Task<SignInResult> SignInResultAsync(IdentityUser user)
        {
            var authTime = DateTime.UtcNow;
            var expiresAt = authTime.AddDays(7);
            var token = await GetTokenAsync(user, authTime, expiresAt);
            var result = new
            {
                access_token = token,
                token_type = "Bearer ",
                profile = new
                {
                    sid = user.Id,
                    name = user.UserName,
                    auth_time = new DateTimeOffset(authTime).ToUnixTimeSeconds(),
                    expires_at = new DateTimeOffset(expiresAt).ToUnixTimeSeconds(),
                }
            };
            return SignInResult.Success(result);
        }
        public async Task<string> GetTokenAsync(IdentityUser user, DateTime authTime, DateTime expiresAt)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_settings.Secret);
            var claims = new Claim[] {
                            new Claim(JwtClaimTypes.Audience,_settings.Aud),
                            new Claim(JwtClaimTypes.Issuer,_settings.Iss),
                            new Claim(JwtClaimTypes.Id, user.Id),
                            new Claim( ClaimTypes.Sid, httpContextAccessor.HttpContext.Session.Id),
                            new Claim(JwtClaimTypes.Name, user.UserName),
                              new Claim( ClaimTypes.Name, user.UserName),
                        };
            var identity = new ClaimsIdentity(claims);
            var role = await userManager.GetRolesAsync(user);
            if (role != null)
            {
                string sub = "";
                foreach (var item in role)
                {
                    sub += item + ",";
                }
                if (user.Email.IsNotNullOrEmpty())
                {
                    claims.Append(new Claim(JwtClaimTypes.Email, user.Email));
                    claims.Append(new Claim(ClaimTypes.Email, user.Email));
                }
                if (user.PhoneNumber.IsNotNullOrEmpty())
                {
                    claims.Append(new Claim(JwtClaimTypes.PhoneNumber, user.PhoneNumber));
                    claims.Append(new Claim(ClaimTypes.HomePhone, user.PhoneNumber));
                }
                if (sub.IsNullOrEmpty())
                {
                    claims.Append(new Claim(JwtClaimTypes.Role, sub));
                    claims.Append(new Claim(ClaimTypes.Role, sub));
                }
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = expiresAt,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public async Task<IdentityUser> GetUserAsync(bool isPhone, string email, string phone)
        {
            Expression<Func<IdentityUser, bool>> predicate = null;
            if (!isPhone)
            {
                predicate = d => d.Email.Equals(email);
            }
            else
            {
                predicate = d => d.PhoneNumber.Equals(phone);
            }
            return await userManager.FindAsync(s => new IdentityUser { Id = s.Id, UserName = s.UserName, PhoneNumber = s.PhoneNumber, Email = s.Email, LockoutEnabled = s.LockoutEnabled, LockoutEnd = s.LockoutEnd, EmailConfirmed = s.EmailConfirmed, PhoneNumberConfirmed = s.PhoneNumberConfirmed, PasswordHash = s.PasswordHash, TwoFactorEnabled = s.TwoFactorEnabled, SecurityStamp = s.SecurityStamp, ConcurrencyStamp = s.ConcurrencyStamp }, predicate);
        }
    }
}
