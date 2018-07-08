using HomeQI.Adream.Identity;

namespace HomeQI.ADream.IdentityService.Services
{
    public interface ILoginUserService
    {
        bool Authenticate(string _userName, string _userPassword, out IdentityUser loginUser);
    }
}
