using HomeQI.Adream.Identity;
using HomeQI.Adream.Identity.EntityFrameworkCore;
using HomeQI.ADream.EntityFrameworkCore;
using HomeQI.ADream.IdentityService.Models;

namespace HomeQI.ADream.IdentityService.Repositories
{
    public interface ILoginUserRepository : IRepository<IdentityUser, IdentityDbContext>
    {
        IdentityUser Authenticate(string _userName, string _userPassword);
    }
}
