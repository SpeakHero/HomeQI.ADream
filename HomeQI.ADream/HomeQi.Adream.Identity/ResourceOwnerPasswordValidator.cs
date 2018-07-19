//using IdentityServer4.Models;
//using IdentityServer4.Validation;
//using System.Security.Claims;
//using System.Threading.Tasks;
//using HomeQI.Adream.Identity;
//using System.Linq.Expressions;
//using System;

//namespace HomeQi.Adream.Identity
//{
//    public class ResourceOwnerPasswordValidator : IResourceOwnerPasswordValidator
//    {
//        private readonly SignInManager _signInManager;
//        private readonly UserManager<IdentityUser> userManager;
//        public ResourceOwnerPasswordValidator(SignInManager signInManager)
//        {
//            _signInManager = signInManager;
//            userManager = signInManager.UserManager;
//        }

//        public async Task ValidateAsync(ResourceOwnerPasswordValidationContext context)
//        {

//            var uname = context.UserName;
//            Expression<Func<IdentityUser, bool>> predicate = null;
//            if (uname.IsEmail())
//            {
//                predicate = d => d.EmailConfirmed && d.Email.Equals(uname);
//            }
//            else
//            {
//                if (uname.IsPhone())
//                {
//                    predicate = d => d.PhoneNumberConfirmed && d.PhoneNumber.Equals(uname);
//                }
//                else
//                {
//                    predicate = d => d.UserName.Equals(uname);
//                }
//            }
//            var user = await userManager.FindAsync(s => new IdentityUser { UserName = s.UserName, PhoneNumber = s.PhoneNumber, Email = s.Email, LockoutEnabled = s.LockoutEnabled, LockoutEnd = s.LockoutEnd, EmailConfirmed = s.EmailConfirmed, PhoneNumberConfirmed = s.PhoneNumberConfirmed, PasswordHash = s.PasswordHash, TwoFactorEnabled = s.TwoFactorEnabled }, predicate);
//            var isAuthenticated = await _signInManager.PasswordSignInAsync(user, context.Password, true, true);
//            if (!isAuthenticated.Succeeded)
//            {
//                context.Result = new GrantValidationResult(TokenRequestErrors.InvalidGrant, "无效客户凭证");
//            }
//            else
//            {
//                var roles = await userManager.GetRolesAsync(user);
//                context.Result = new GrantValidationResult(
//                    subject: context.UserName,
//                    authenticationMethod: "custom",
//                    claims: new Claim[] {
//                        new Claim("Name", user.UserName),
//                        new Claim("Id", user.Id),
//                        new Claim("roles", roles.Join()),
//                        new Claim("Email", user.Email),
//                        new Claim("Phone", user.Email),
//                        new Claim("SecurityStamp", user.SecurityStamp)
//                    }
//                );
//            }
//        }
//    }
//}
