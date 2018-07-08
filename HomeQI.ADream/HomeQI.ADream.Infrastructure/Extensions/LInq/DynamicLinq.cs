using System.Linq.Expressions;
using System.Reflection;

namespace System.Linq. Expressions
{
    public static class DynamicLinq
    {
        /// <summary>  
        /// 创建lambda中的参数,即c=>c.xxx==xx 中的c  
        /// </summary>  
        public static ParameterExpression CreateLambdaParam<T>(string name)
        {
            return Expression.Parameter(typeof(T), name);
        }

        /// <summary>  
        /// 创建linq表达示的body部分,即c=>c.xxx==xx 中的c.xxx==xx  
        /// </summary>  
        public static Expression GenerateBody<T>(this ParameterExpression param, QueryableFilter filterObj)
        {
            BindingFlags flag = BindingFlags.Public | BindingFlags.IgnoreCase | BindingFlags.Instance; //忽略大小写

            PropertyInfo property = typeof(T).GetProperty(filterObj.Key, flag);

            //组装左边  
            Expression left = Expression.Property(param, property);
            //组装右边  
            Expression right = null;

            //todo: 下面根据需要，扩展自己的类型  
            if (property.PropertyType == typeof(int))
            {
                right = Expression.Constant(Convert.ToInt32(filterObj.Value));
            }
            else if (property.PropertyType == typeof(DateTime))
            {
                right = Expression.Constant(Convert.ToDateTime(filterObj.Value));
            }
            else if (property.PropertyType == typeof(string))
            {
                right = Expression.Constant((filterObj.Value));
            }
            else if (property.PropertyType == typeof(decimal))
            {
                right = Expression.Constant(Convert.ToDecimal(filterObj.Value));
            }
            else if (property.PropertyType == typeof(Guid))
            {
                right = Expression.Constant(Guid.Parse(Convert.ToString(filterObj.Value)));
            }
            else if (property.PropertyType == typeof(bool))
            {
                right = Expression.Constant(Convert.ToBoolean(filterObj.Value));
            }
            else if (property.PropertyType == typeof(byte[]))
            {
                right = Expression.Constant(filterObj.Value as byte[]);
            }
            else
            {
                throw new Exception("暂不能解析该Key的类型");
            }
        
            //todo: 下面根据需要扩展自己的比较  
            Expression filter = Expression.Equal(left, right);
            switch (filterObj.Contract)
            {
                case Contract.LessThanOrEqual:
                    filter = Expression.LessThanOrEqual(left, right);
                    break;

                case Contract.LessThan:
                    filter = Expression.LessThan(left, right);
                    break;

                case Contract.GreaterThan:
                    filter = Expression.GreaterThan(left, right);
                    break;

                case Contract.GreaterThanOrEqual:
                    filter = Expression.GreaterThanOrEqual(left, right);
                    break;

                case Contract.Like:
                    filter = Expression.Call(left, typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                                 Expression.Constant(filterObj.Value));
                    break;
                case Contract.Equal:
                default:
                    filter = Expression.Equal(left, right);
                    break;
                case Contract.Or:
                    filter = Expression.Call(right, typeof(string).GetMethod("Contains", new[] { typeof(string) }),
                            Expression.Constant(left));
                    break;
                
                case Contract.NotEqual:
                    filter = Expression.NotEqual(left, right);
                    break;
            }

            return filter;
        }

        /// <summary>  
        /// 创建完整的lambda,即c=>c.xxx==xx  
        /// </summary>  
        public static LambdaExpression GenerateLambda(this ParameterExpression param, Expression body)
        {
            return Expression.Lambda(body, param);
        }

        /// <summary>  
        /// 创建完整的lambda，为了兼容EF中的where语句  
        /// </summary>  
        public static Expression<Func<T, bool>> GenerateTypeLambda<T>(this ParameterExpression param, Expression body)
        {
            return (Expression<Func<T, bool>>)(param.GenerateLambda(body));
        }

        public static Expression AndAlso(this Expression expression, Expression expressionRight)
        {
            return Expression.AndAlso(expression, expressionRight);
        }

        public static Expression Or(this Expression expression, Expression expressionRight)
        {
            return Expression.Or(expression, expressionRight);
        }

        public static Expression And(this Expression expression, Expression expressionRight)
        {
            return Expression.And(expression, expressionRight);
        }
    }
}
