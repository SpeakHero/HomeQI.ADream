// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using HomeQI.ADream.Entities.Framework;
using System;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace HomeQI.Adream.Identity
{
    /// <summary>
    /// ������֤����֤��
    /// </summary>
    public class AuthenticatorTokenProvider<TUser> : IUserTwoFactorTokenProvider<TUser> where TUser : EntityBase<string>
    {
        /// <summary>
        /// ����Ƿ����Ϊָ��������˫���������֤����<paramref name="user"/>.
        /// </summary>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> to retrieve the <paramref name="user"/> from.</param>
        /// <param name="user">The <typeparamref name="TUser"/> to c�������˫������֤���ƵĿ����ԡ�</param>
        /// <returns>True if the user has an authenticator key set, otherwise false.</returns>
        public async virtual Task<bool> CanGenerateTwoFactorTokenAsync(UserManager<TUser> manager, TUser user)
        {
            var key = await manager.GetAuthenticatorKeyAsync(user);
            return !string.IsNullOrWhiteSpace(key);
        }

        /// <summary>
        /// ���ؿ��ַ�������Ϊû�з�����֤�ߴ��롣
        /// </summary>
        /// <param name="purpose">Ignored.</param>
        /// <param name="manager">The <see cref="UserManager{TUser}"/> to retrieve the <paramref name="user"/> from.</param>
        /// <param name="user">The <typeparamref name="TUser"/>.</param>
        /// <returns>string.Empty.</returns>
        public virtual Task<string> GenerateAsync(string purpose, UserManager<TUser> manager, TUser user)
        {
            return Task.FromResult(string.Empty);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="purpose"></param>
        /// <param name="token"></param>
        /// <param name="manager"></param>
        /// <param name="user"></param>
        /// <returns></returns>
        public virtual async Task<bool> ValidateAsync(string purpose, string token, UserManager<TUser> manager, TUser user)
        {
            var key = await manager.GetAuthenticatorKeyAsync(user);
            if (!int.TryParse(token, out int code))
            {
                return false;
            }

            var hash = new HMACSHA1(Base32.FromBase32(key));
            var unixTimestamp = Convert.ToInt64(Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds));
            var timestep = Convert.ToInt64(unixTimestamp / 30);
            // ��������90�����ÿ���������ǿ���ʹ������ã���
            for (int i = -2; i <= 2; i++)
            {
                var expectedCode = Rfc6238AuthenticationService.ComputeTotp(hash, (ulong)(timestep + i), modifier: null);
                if (expectedCode == code)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
