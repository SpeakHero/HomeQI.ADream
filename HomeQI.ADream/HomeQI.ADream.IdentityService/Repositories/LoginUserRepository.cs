
using HomeQI.Adream.Identity;
using HomeQI.Adream.Identity.EntityFrameworkCore;
using HomeQI.ADream.EntityFrameworkCore;
using HomeQI.ADream.IdentityService.Models;
using System.Linq;

namespace HomeQI.ADream.IdentityService.Repositories
{
    public class LoginUserRepository : RepositoryBase<IdentityUser, IdentityDbContext>, ILoginUserRepository
    {
        public LoginUserRepository(IdentityDbContext dbContext) : base(dbContext)
        {
        }

        public IdentityUser Authenticate(string _userName, string _userPassword)
        {
            var entity = DbContext.Users.FirstOrDefault(p => p.UserName == _userName &&
                p.PasswordHash == _userPassword);
            return entity;
        }
    }
}
