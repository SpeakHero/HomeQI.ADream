
using HomeQI.Adream.Identity;
using HomeQI.ADream.IdentityService.Models;
using HomeQI.ADream.IdentityService.Repositories;

namespace HomeQI.ADream.IdentityService.Services
{
    public class LoginUserService : ILoginUserService
    {
        private ILoginUserRepository loginUserRepository;

        public LoginUserService(ILoginUserRepository _loginUserRepository)
        {
            loginUserRepository = _loginUserRepository;
        }

        public bool Authenticate(string _userName, string _userPassword, out IdentityUser loginUser)
        {
            //这里的一些商业逻辑代码 ...
            // eg.Security check & MD5 & 3DES ...
            loginUser = loginUserRepository.Authenticate(_userName, _userPassword);
            return loginUser == null ? false : true;
        }
    }
}
