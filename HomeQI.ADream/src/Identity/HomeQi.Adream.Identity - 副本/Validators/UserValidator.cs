using HomeQI.ADream.Identity.Core;
using HomeQI.ADream.Identity.Entites;
using HomeQI.ADream.Identity.Manager;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace HomeQI.ADream.Identity.Validators
{
    public class UserValidator : IUserValidator
    {
        public UserValidator(IdentityErrorDescriber errors = null)
        {
            Describer = errors ?? new IdentityErrorDescriber();
        }
        public IdentityErrorDescriber Describer { get; private set; }

        public virtual async Task<IdentityResult> ValidateAsync(IUserManager manager, User user)
        {
            manager.CheakArgument();
            user.CheakArgument();
            var errors = new List<IdentityError>();
            await ValidateUserName(manager, user, errors);
            await ValidatePhoneNumberAsync(manager, user, errors);
            if (manager.Options.User.RequireUniqueEmail)
            {
                await ValidateEmail(manager, user, errors);
            }
            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success();
        }

        public async Task<IdentityResult> ValidatePhoneNumberAsync(IUserManager manager, User user, ICollection<IdentityError> errors)
        {
            var phoneNumber = user.PhoneNumber;
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                errors.Add(new IdentityError { Code = phoneNumber, Description = $"电话号码不能为空" });
                return IdentityResult.Failed(errors.ToArray());
            }
            if (!new PhoneAttribute().IsValid(phoneNumber))
            {
                errors.Add(new IdentityError { Code = phoneNumber, Description = $"电话号码：{phoneNumber}格式不正确" });
                return IdentityResult.Failed(errors.ToArray());
            }
            if (await manager.Users.AnyAsync(d => d.PhoneNumber.Equals(phoneNumber)))
            {
                errors.Add(new IdentityError { Code = phoneNumber, Description = $"电话号码：{phoneNumber}已经存在" });
                return IdentityResult.Failed(errors.ToArray());
            }
            return IdentityResult.Success();
        }
        public async Task<IdentityResult> ValidateUserName(IUserManager manager, User user, ICollection<IdentityError> errors)
        {
            var userName = user.UserName;
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add(Describer.DuplicateUserName(userName));
                return IdentityResult.Failed(errors.ToArray());
            }
            else if (!string.IsNullOrEmpty(manager.Options.User.AllowedUserNameCharacters) &&
                userName.Any(c => !manager.Options.User.AllowedUserNameCharacters.Contains(c)))
            {
                errors.Add(Describer.InvalidUserName(userName));
                return IdentityResult.Failed(errors.ToArray());
            }
            else
            {
                if (await manager.Users.AnyAsync(d => d.UserName.Equals(userName)))
                {
                    errors.Add(new IdentityError { Code = userName, Description = $"用户名称：{userName}已经存在" });
                    return IdentityResult.Failed(errors.ToArray());
                }
            }
            return IdentityResult.Success();
        }

        // 确保电子邮件不是空的、有效的、唯一的
        public async Task<IdentityResult> ValidateEmail(IUserManager manager, User user, List<IdentityError> errors)
        {
            var email = user.Email;
            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add(Describer.InvalidEmail(email));
                return IdentityResult.Failed(errors.ToArray());
            }
            if (!new EmailAddressAttribute().IsValid(email))
            {
                errors.Add(Describer.InvalidEmail(email));
                return IdentityResult.Failed(errors.ToArray());

            }
            if (await manager.Users.AnyAsync(d => d.Email.Equals(email)))
            {
                errors.Add(Describer.DuplicateEmail(email));
                return IdentityResult.Failed(Describer.DuplicateEmail(email));
            }
            return IdentityResult.Success();
        }
    }
}
