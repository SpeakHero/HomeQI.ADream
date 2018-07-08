
namespace System.Linq. Expressions
{
    /// <summary>
    /// 
    /// </summary>
    public static class DynamicExtention
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="query"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IQueryable<T> Where<T>(this IQueryable<T> query, params QueryableFilter[] filters)
        {
            if (filters == null)
            {
                return query;
            }
            var param = DynamicLinq.CreateLambdaParam<T>("c");
            Expression body = param.GenerateBody<T>(filters[0]); //初始默认一个
            if (filters.Length > 1)
            {
                for (int i = 1; i < filters.Length; i++)
                {
                    var filter = filters[i];
                    switch (filter.Condition)
                    {
                        case Condition.Or:
                            body = body.Or(param.GenerateBody<T>(filter));
                            break;

                        case Condition.AndAlso:
                            body = body.AndAlso(param.GenerateBody<T>(filter));
                            break;
                        case Condition.And:
                        default:
                            body = body.And(param.GenerateBody<T>(filter));
                            break;
                    }
                }
            }

            var lambda = param.GenerateTypeLambda<T>(body); //最终组成lambda  
            return query.Where(lambda);
        }

    }
}
