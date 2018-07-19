using HomeQI.Adream.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Stores
{
    public class UserValidator : UserValidator<IdentityUser>
    {
        public UserValidator(IdentityErrorDescriber errors = null) : base(errors)
        {
        }
        public override Task<IdentityResult> ValidateAsync(UserManager<IdentityUser> manager, IdentityUser user)
        {
            if (user.UserName == "" && user.PhoneNumber == "" && user.Email == "")
            {
                return Task.FromResult(IdentityResult.Failed("没有用户名"));
            }
            return base.ValidateAsync(manager, user);
        }
        protected override async Task ValidateEmailAsync(UserManager<IdentityUser> manager, IdentityUser user, List<IdentityError> errors)
        {
            if (manager == null)
            {
                throw new ArgumentNullEx(nameof(manager));
            }
            if (user == null)
            {
                throw new ArgumentNullEx(nameof(user));
            }
            if (user.Email.IsNotNullOrEmpty())
            {
                var owner = await manager.GetFirstOrDefaultAsync(s => new IdentityUser { Email = s.Email, Id = s.Id }, p => p.Email.Equals(user.Email));
                if (owner == null)
                {
                    return;
                }
                var email = owner.Email;
                if (string.IsNullOrWhiteSpace(email))
                {
                    errors.Add(Describer.InvalidEmail(email));
                    return;
                }
                if (!new EmailAddressAttribute().IsValid(email))
                {
                    errors.Add(Describer.InvalidEmail(email));
                    return;
                }
                if (owner != null &&
                    !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                {
                    errors.Add(Describer.DuplicateEmail(email));
                }
            }
        }
        protected override async Task ValidatePhoneAsync(UserManager<IdentityUser> manager, IdentityUser user, List<IdentityError> errors)
        {
            if (manager == null)
            {
                throw new ArgumentNullEx(nameof(manager));
            }
            if (user == null)
            {
                throw new ArgumentNullEx(nameof(user));
            }
            if (user.PhoneNumber.IsNotNullOrEmpty())
            {
                var owner = await manager.GetFirstOrDefaultAsync(s => new IdentityUser { PhoneNumber = s.PhoneNumber, Id = s.Id }, p => p.PhoneNumber.Equals(user.PhoneNumber));
                if (owner == null)
                {
                    return;
                }
                var phoneNumber = owner.PhoneNumber;
                if (!new PhoneAttribute().IsValid(phoneNumber))
                {
                    errors.Add(Describer.InvalidPhone());
                    return;
                }
                if (string.IsNullOrWhiteSpace(phoneNumber))
                {
                    errors.Add(Describer.InvalidPhone());
                    return;
                }
                else
                {
                    if (owner != null &&
                        !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                    {
                        errors.Add(Describer.DuplicatePhone(phoneNumber));
                    }
                }
            }

        }
        protected override async Task ValidateUserNameAsync(UserManager<IdentityUser> manager, IdentityUser user, ICollection<IdentityError> errors)
        {
            if (manager == null)
            {
                throw new ArgumentNullEx(nameof(manager));
            }
            if (user == null)
            {
                throw new ArgumentNullEx(nameof(user));
            }
            if (user.UserName.IsNotNullOrEmpty())
            {
                var owner = await manager.GetFirstOrDefaultAsync(s => new IdentityUser { UserName = s.UserName, Id = s.Id }, p => p.UserName.Equals(user.UserName));
                if (owner == null)
                {
                    return;
                }
                var userName = owner.UserName;
                if (string.IsNullOrWhiteSpace(userName))
                {
                    errors.Add(Describer.InvalidUserName(userName));
                }
                else if (!string.IsNullOrEmpty(manager.Options.User.AllowedUserNameCharacters) &&
                    userName.Any(c => !manager.Options.User.AllowedUserNameCharacters.Contains(c)))
                {
                    errors.Add(Describer.InvalidUserName(userName));
                }
                else
                {
                    if (owner != null &&
                        !string.Equals(await manager.GetUserIdAsync(owner), await manager.GetUserIdAsync(user)))
                    {
                        errors.Add(Describer.DuplicateUserName(userName));
                    }
                }
            }
        }
    }
}
