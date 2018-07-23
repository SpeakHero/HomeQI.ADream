﻿using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.JsonWebTokens;

namespace HomeQI.Adream.Identity
{
    public static class IHttpContextAccessorExtension
    {
        public static int CurrentUser(this IHttpContextAccessor httpContextAccessor)
        {
            var stringId = httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            int.TryParse(stringId ?? "0", out int userId);

            return userId;
        }
    }
}
