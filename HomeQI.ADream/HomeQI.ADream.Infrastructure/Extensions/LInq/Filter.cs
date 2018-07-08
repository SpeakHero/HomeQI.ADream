namespace System.Linq.Expressions
{
    /// <summary>
    /// //var filters = new Filter[]
    //{
    //           new Filter {Key = "UserName", Value = "yubaolee", Contract = "like"},
    //           new Filter {Key = "UserType", Value = "administrator", Contract = "="}
    //};
    /// </summary>
    public class QueryableFilter
    {
        /// <summary>
        /// //过滤的关键字  
        /// </summary>
        public string Key { get; set; }
        /// <summary>
        ///  //过滤的值  
        /// </summary>
        public object Value { get; set; }
        /// <summary>
        /// 过滤的约束 比如：‘<‘ ‘<=‘ ‘>‘ ‘>=‘ ‘like‘等  
        /// </summary>
        public Contract Contract { get; set; } = Contract.Equal;

        /// <summary>
        /// 下一个比较条件有  or and
        /// andalso
        /// </summary>
        public Condition Condition { get; set; } = Condition.And;
    }
    /// <summary>
    /// 
    /// </summary>
    public enum Condition
    {
        /// <summary>
    /// 
    /// </summary>
        And,
        /// <summary>
        /// 
        /// </summary>
        Or,
        /// <summary>
        /// 
        /// </summary>
        AndAlso
    }
    /// <summary>
    /// 
    /// </summary>
    public enum Contract
    {
        /// <summary>
        /// 
        /// </summary>
        GreaterThan,
        /// <summary>
        /// 
        /// </summary>
        LessThan,
        /// <summary>
        /// 
        /// </summary>
        GreaterThanOrEqual,
        /// <summary>
        /// 
        /// </summary>
        LessThanOrEqual,
        /// <summary>
        /// 
        /// </summary>
        Equal,
        /// <summary>
        /// 
        /// </summary>
        NotEqual,
        /// <summary>
        /// 
        /// </summary>
        Like,
        /// <summary>
        /// 
        /// </summary>
        Or
    }
}
