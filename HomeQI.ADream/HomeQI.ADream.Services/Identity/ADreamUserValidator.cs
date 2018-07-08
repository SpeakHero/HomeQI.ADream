using HomeQI.ADream.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HomeQI.ADream.Services.Identity
{
    public class UserValidator : IUserValidator<User>
    {
        /// <summary>
        /// Creates a new instance of <see cref="UserValidator{User}"/>/
        /// </summary>
        /// <param name="errors">The <see cref="IdentityErrorDescriber"/> used to provider error messages.</param>
        public UserValidator(IdentityErrorDescriber errors = null)
        {
            Describer = errors ?? new IdentityErrorDescriber();
        }

        /// <summary>
        /// Gets the <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{User}"/>.
        /// </summary>
        /// <value>The <see cref="IdentityErrorDescriber"/> used to provider error messages for the current <see cref="UserValidator{User}"/>.</value>
        public IdentityErrorDescriber Describer { get; private set; }

        /// <summary>
        /// Validates the specified <paramref name="user"/> as an asynchronous operation.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{User}"/> that can be used to retrieve user properties.</param>
        /// <param name="user">The user to validate.</param>
        /// <returns>The <see cref="Task"/> that represents the asynchronous operation, containing the <see cref="IdentityResult"/> of the validation operation.</returns>
        public virtual async Task<IdentityResult> ValidateAsync(UserManager<User> manager, User user)
        {
            manager.CheakArgument();
            user.CheakArgument();
            var errors = new List<IdentityError>();
            await ValidateUserName(manager, user, errors);
            await ValidatePhoneNumberAsync(manager, user,errors);
            if (manager.Options.User.RequireUniqueEmail)
            {
                await ValidateEmail(manager, user, errors);
            }
            return errors.Count > 0 ? IdentityResult.Failed(errors.ToArray()) : IdentityResult.Success;
        }

        private async Task ValidatePhoneNumberAsync(UserManager<User> manager, User user, ICollection<IdentityError> errors)
        {
            var phoneNumber = user.PhoneNumber;
            if (string.IsNullOrWhiteSpace(phoneNumber))
            {
                errors.Add(new IdentityError { Code = phoneNumber, Description = $"电话号码不能为空" });
                return;
            }
            if (!new PhoneAttribute().IsValid(phoneNumber))
            {
                errors.Add(new IdentityError { Code = phoneNumber, Description = $"电话号码：{phoneNumber}格式不正确" });
                return;
            }
            if (await manager.Users.AnyAsync(d => d.PhoneNumber.Equals(phoneNumber)))
            {
                errors.Add(new IdentityError { Code = phoneNumber, Description = $"电话号码：{phoneNumber}已经存在" });
                return;
            }
        }
        private async Task ValidateUserName(UserManager<User> manager, User user, ICollection<IdentityError> errors)
        {
            var userName = user.UserName;
            if (string.IsNullOrWhiteSpace(userName))
            {
                errors.Add(new IdentityError { Code = userName, Description = $"用户名称不能为空" });
                return;
            }
            else if (!string.IsNullOrEmpty(manager.Options.User.AllowedUserNameCharacters) &&
                userName.Any(c => !manager.Options.User.AllowedUserNameCharacters.Contains(c)))
            {
                errors.Add(Describer.InvalidUserName(userName));
                return;
            }
            else
            {
                if (await manager.Users.AnyAsync(d => d.UserName.Equals(userName)))
                {
                    errors.Add(new IdentityError { Code = userName, Description = $"用户名称：{userName}已经存在" });
                    return;
                }
            }
        }

        // make sure email is not empty, valid, and unique
        private async Task ValidateEmail(UserManager<User> manager, User user, List<IdentityError> errors)
        {
            var email = user.Email;
            if (string.IsNullOrWhiteSpace(email))
            {
                errors.Add(new IdentityError { Code = email, Description = $"电子邮件不能为空" });
                return;
            }
            if (!new EmailAddressAttribute().IsValid(email))
            {
                errors.Add(new IdentityError { Code = email, Description = "电子邮件格式不正确" });
                return;
            }
            if (await manager.Users.AnyAsync(d => d.Email.Equals(email)))
            {
                errors.Add(new IdentityError { Code = email, Description = $"电子邮件：{email}已经存在" });
                return;
            }
        }
    }
}
