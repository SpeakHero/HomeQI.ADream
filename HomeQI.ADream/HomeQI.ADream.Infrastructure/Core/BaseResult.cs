using AutoMapper;
using System.Collections.Generic;
using System.Linq;

namespace HomeQI.ADream.Infrastructure.Core
{
    public class BaseResult<TResult, TError> : IBaseResult<TError> where TResult : BaseResult<TResult, TError>, new() where TError : IBaseError, new()
    {
        protected List<TError> _errors = new List<TError>();
        public BaseResult()
        {

        }
        public bool Succeeded { get; protected set; }
        public IEnumerable<TError> Errors => _errors;
        public dynamic Result { get; protected set; }
        public override string ToString()
        {
            return Succeeded ?
                   "Succeeded" :
                   string.Format("{0} : {1}", "Failed", string.Join(",", Errors.Select(x => x.Code).ToList()));
        }

        public static TResult Success(object result = null)
        {
            return new TResult
            {
                Succeeded = true,
                _errors = null,
                Result = result
            };
        }
        public static TResult Failed(params TError[] errors)
        {

            var res = new TResult
            {
                Succeeded = false,
            };
            res._errors.AddRange(errors);
            return res;
        }
        private static TResult RMapper(params TError[] errors)
        {
            Mapper.Initialize(x =>
            {
                x.CreateMap<BaseResult<TResult, TError>, TResult>();
            });
            var bres = new BaseResult<TResult, TError>();
            var res = Mapper.Map<TResult>(bres);
            return res;

        }
    }
}
