using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System.Reflection;

namespace System.Linq.Expressions
{
    /// <summary>
    /// 
    /// </summary>
    public static class IQueryableExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">tyof</param>
        /// <param name="sortExpression">字段</param>
        /// <param name="sortDirection">desc</param>
        /// <returns></returns>
        public static IQueryable<T> DataSorting<T>(this IQueryable<T> source, string sortExpression, string sortDirection)
        {
            string sortingDir = string.Empty;
            if (sortDirection.ToUpper().Trim() == "ASC")
                sortingDir = "OrderBy";
            else
                sortingDir = "OrderByDescending";
            ParameterExpression param = Expression.Parameter(typeof(T), sortExpression);
            BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance; //忽略大小写
            PropertyInfo pi = typeof(T).GetProperty(sortExpression, flag);
            if (pi == null)
            {
                pi = typeof(T).GetProperty("Id", flag);
                if (pi == null)
                {
                    return source;
                }
            }
            Type[] types = new Type[2];
            types[0] = typeof(T);
            types[1] = pi.PropertyType;
            Expression expr = Expression.Call(typeof(Queryable), sortingDir, types, source.Expression, Expression.Lambda(Expression.Property(param, sortExpression), param));
            IQueryable<T> query = source.AsQueryable().Provider.CreateQuery<T>(expr);
            return query;
        }

        private static readonly TypeInfo QueryCompilerTypeInfo = typeof(QueryCompiler).GetTypeInfo();

        private static readonly FieldInfo QueryCompilerField = typeof(EntityQueryProvider).GetTypeInfo().DeclaredFields.First(x => x.Name == "_queryCompiler");

        private static readonly PropertyInfo NodeTypeProviderField = QueryCompilerTypeInfo.DeclaredProperties.Single(x => x.Name == "NodeTypeProvider");

        private static readonly MethodInfo CreateQueryParserMethod = QueryCompilerTypeInfo.DeclaredMethods.First(x => x.Name == "CreateQueryParser");

        private static readonly FieldInfo DataBaseField = QueryCompilerTypeInfo.DeclaredFields.Single(x => x.Name == "_database");

        private static readonly FieldInfo QueryCompilationContextFactoryField = typeof(Database).GetTypeInfo().DeclaredFields.Single(x => x.Name == "_queryCompilationContextFactory");

    }
}
