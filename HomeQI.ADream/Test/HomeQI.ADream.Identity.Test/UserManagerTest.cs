using HomeQi.Adream.Identity;
using HomeQI.ADream.Infrastructure.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security;
using System.Threading.Tasks;

namespace HomeQI.Adream.Identity.Test
{
    [TestClass]
    public class UserManagerTest : TestBase
    {
        private readonly IdentityUser user = new IdentityUser();
        public UserManager UserManager;
        public UserManagerTest() : base()
        {
            UserManager = GetRequiredService<UserManager>();
        }

        [TestMethod]
        public void EncryptProviders()
        {
            var result = EncryptProvider.CreateDesKey();

            var a = EncryptProvider.DESEncrypt("qwe123", "TThvj8G5jb6GcTFZmxVwh5Cj");
            var b = EncryptProvider.DESDecrypt(a, "TThvj8G5jb6GcTFZmxVwh5Cj");
        }
        [TestMethod]
        public async Task AddUserAsync()
        {
            var result = await UserManager.CreateAsync(new IdentityUser { UserName = "SpeakHero", PhoneNumber = "18116699557", Email = "491960352@qq.com" }, "Qwe@#23");
            Assert.IsTrue(IdentityResult.Success().Succeeded);

        }

        [TestMethod]
        public async Task CheckPasswordAsync()
        {
            var user = UserManager.Users.FirstOrDefault(d => d.UserName != "");
            var result = await UserManager.CheckPasswordAsync(user, "Qwe@#23");
            Assert.IsTrue(result);
        }
        [TestMethod]
        public async Task DeleteUserAsync()
        {
            var user = UserManager.Users.FirstOrDefault(d => d.UserName != "");
            var result = await UserManager.DeleteAsync(user);
            Assert.IsTrue(IdentityResult.Success().Succeeded);
        }
        [TestMethod]
        public async Task EditUserAsync()
        {
            var user = UserManager.Users.FirstOrDefault(d => d.UserName != "");
            var result = await UserManager.UpdateAsync(user);
            Assert.IsTrue(IdentityResult.Success().Succeeded);
        }
        [TestMethod]
        public async Task SetPhoneNumberAsync()
        {
            var user = UserManager.Users.FirstOrDefault(d => d.UserName != "");
            var result = await UserManager.SetPhoneNumberAsync(user, "18116699557");
            if (result.Errors != null)
                foreach (var item in result.Errors)
                {
                    Debug.WriteLine(item);

                }
        }
        [TestMethod]
        public async Task HasPasswordAsync()
        {
            var user = UserManager.Users.FirstOrDefault(d => d.UserName != "");
            var result = await UserManager.HasPasswordAsync(user);
            Debug.Write(result);
        }

        [TestMethod]
        public async Task SetLockoutEndDateAsync()
        {
            var user = UserManager.Users.FirstOrDefault(d => d.UserName != "");
            var result = await UserManager.SetLockoutEndDateAsync(user, DateTime.Now.AddHours(1));
            if (result.Errors != null)
                foreach (var item in result.Errors)
                {
                    Debug.WriteLine(item);
                }
        }
        [TestMethod]
        public async Task SetLockoutEnabledAsync()
        {
            var user = UserManager.Users.FirstOrDefault(d => d.UserName != "");
            var result = await UserManager.SetLockoutEnabledAsync(user, true);
            if (result.Errors != null)
                foreach (var item in result.Errors)
                {
                    Debug.WriteLine(item);
                }
        }
        [TestMethod]
        public async Task RemovePasswordAsync()
        {
            var user = UserManager.Users.FirstOrDefault(d => d.UserName != "");
            var result = await UserManager.RemovePasswordAsync(user);
            if (result.Errors != null)
                foreach (var item in result.Errors)
                {
                    Debug.WriteLine(item);
                }
        }
        [TestMethod]
        public async Task SetEmailAsync()
        {

            var user = UserManager.Users.FirstOrDefault(d => d.UserName != "");
            var result = await UserManager.SetEmailAsync(user, "mynasme@qq.com");
            if (result.Errors != null)
                foreach (var item in result.Errors)
                {
                    Debug.WriteLine(item);
                }
        }
        [TestMethod]
        public void CheakCode()
        {
            for (int i = 0; i < 10; i++)
            {
                CheakCode cheakCode = new CheakCode();
                using (var validateImageData = cheakCode.GetImgWithValidateCode())
                {
                    using (Image image = Image.FromStream(validateImageData))
                    {
                        image.Save($@"d:\ss{i}.Jpeg", ImageFormat.Jpeg);
                    }
                }
            }
        }
    }
}
