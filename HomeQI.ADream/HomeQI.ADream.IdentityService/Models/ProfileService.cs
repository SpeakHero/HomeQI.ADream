using IdentityServer4.Models;
using IdentityServer4.Services;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQI.ADream.IdentityService.Models
{
    public class ProfileService : IProfileService
    {
        public Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var claims = context.Subject.Claims.ToList();
            context.IssuedClaims = claims.ToList();
            return Task.CompletedTask;
        }

        public Task IsActiveAsync(IsActiveContext context)
        {
            context.IsActive = true;
            return Task.CompletedTask;
        }
    }
}
