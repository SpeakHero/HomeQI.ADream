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
        public virtual bool Succeeded { get; protected set; }
        public virtual IEnumerable<TError> Errors { get { return _errors; } protected set { _errors = value.ToList(); } }
        public virtual dynamic Result { get; set; }
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
        public static TResult Failed()
        {

            var res = new TResult
            {
                Succeeded = false
            };
            return res;
        }
        public static TResult Failed(params TError[] errors)
        {

            var res = new TResult
            {
                Succeeded = false
            };
            res._errors.AddRange(errors);
            return res;
        }
        public static TResult Failed(params string[] errors)
        {
            List<TError> terrors = new List<TError>();
            foreach (var item in errors)
            {
                terrors.Add(new TError
                {
                    Description = item
                });
            }
            return Failed(terrors.ToArray());
        }
        //private static TResult RMapper(params TError[] errors)
        //{
        //    Mapper.Initialize(x =>
        //    {
        //        x.CreateMap<BaseResult<TResult, TError>, TResult>();
        //    });
        //    var bres = new BaseResult<TResult, TError>();
        //    var res = Mapper.Map<TResult>(bres);
        //    return res;
        //}
    }
}
