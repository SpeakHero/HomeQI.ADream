using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.EntityFrameworkCore;
using HomeQI.ADream.Models.Entities.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HomeQI.ADream.Services
{
    public class UserServices : UserManager<User>
    {
        public UserServices(IUserStore<User> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<User> passwordHasher, IEnumerable<IUserValidator<User>> userValidators, IEnumerable<IPasswordValidator<User>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<User>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
        protected IEntityStore<TEntity> EntityStore<TEntity>() where TEntity : EntityBase
        {
            if (!(Store is IEntityStore<TEntity> cast))
            {
                throw new NotSupportedException(nameof(IEntityStore<TEntity>));
            }
            return cast;
        }
        private static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
       

        public IEntityStore<User> EntityStore() => EntityStore<User>();

        public virtual Task<IdentityResult> UpdateAsync(User user, params string[] propertys)
        {
            ThrowIfDisposed();
            user.CheakArgument();
            return EntityStore().UpdateAsync(user, CancellationToken, propertys);
        }
    }
}
