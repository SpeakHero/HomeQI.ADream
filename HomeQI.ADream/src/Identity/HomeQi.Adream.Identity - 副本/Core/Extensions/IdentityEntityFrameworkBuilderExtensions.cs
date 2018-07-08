using HomeQI.ADream.Identity.EntityFrameworkCore;
using HomeQI.ADream.Identity.Extensions;
using HomeQI.ADream.Identity.Store;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class IdentityEntityFrameworkBuilderExtensions
    {
        public static IdentityBuilder AddEntityFrameworkStores(this IdentityBuilder builder)
        {
            AddStores(builder.Services);
            return builder;
        }

        private static void AddStores(IServiceCollection services)
        {
            services.TryAddScoped<IUserStore, UserStore>();
            services.TryAddScoped<IRoleClaimStore, RoleClaimStore>();
            services.TryAddScoped<IRoleStore, RoleStore>();
        }
        /// <summary>
        /// 找到通用的基类型
        /// </summary>
        /// <param name="currentType">当前类型</param>
        /// <param name="genericBaseType">泛型基本类型</param>
        /// <returns></returns>
        private static TypeInfo FindGenericBaseType(Type currentType, Type genericBaseType)
        {
            var type = currentType;
            while (type != null)
            {
                var typeInfo = type.GetTypeInfo();
                var genericType = type.IsGenericType ? type.GetGenericTypeDefinition() : null;
                if (genericType != null && genericType == genericBaseType)
                {
                    return typeInfo;
                }
                type = type.BaseType;
            }
            return null;
        }
    }
}