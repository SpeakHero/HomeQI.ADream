
using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Manager;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Validators
{

    public interface IUserValidator
    {
        Task<IdentityResult> ValidateAsync(IUserManager manager, User user);
        Task<IdentityResult> ValidatePhoneNumberAsync(IUserManager manager, User user, ICollection<IdentityError> errors);
        Task<IdentityResult> ValidateUserName(IUserManager manager, User user, ICollection<IdentityError> errors);
        Task<IdentityResult> ValidateEmail(IUserManager manager, User user, List<IdentityError> errors);
    }
}