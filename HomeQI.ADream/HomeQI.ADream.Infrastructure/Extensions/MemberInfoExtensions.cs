﻿using System.Linq;

namespace System.Reflection
{
    internal static class MemberInfoExtensions
    {
        public static Type GetMemberType(this MemberInfo memberInfo)
            => (memberInfo as PropertyInfo)?.PropertyType ?? ((FieldInfo)memberInfo)?.FieldType;

        public static bool IsSameAs(this MemberInfo propertyInfo, MemberInfo otherPropertyInfo)
        {
            if (propertyInfo == null)
            {
                return otherPropertyInfo == null;
            }

            if (otherPropertyInfo == null)
            {
                return false;
            }

            return Equals(propertyInfo, otherPropertyInfo)
                   || (propertyInfo.Name == otherPropertyInfo.Name
                       && (propertyInfo.DeclaringType == otherPropertyInfo.DeclaringType
                           || propertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(otherPropertyInfo.DeclaringType)
                           || otherPropertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(propertyInfo.DeclaringType)
                           || propertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains(otherPropertyInfo.DeclaringType)
                           || otherPropertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains(propertyInfo.DeclaringType)));
        }
    }
}