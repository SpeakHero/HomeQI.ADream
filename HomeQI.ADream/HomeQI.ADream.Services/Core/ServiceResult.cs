using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Linq.Dynamic;
using System;

namespace HomeQI.ADream.Services.Core
{
    public class ServiceResult
    {
        public bool Succeeded { get; protected set; }
        public object Result { get; protected set; }
        public IEnumerable<ServiceError> Errors => new List<ServiceError>();
        public static ServiceResult Success(object result) => new ServiceResult { Succeeded = true, Result = result };
        public static ServiceResult Success() => Success(null);
        public static ServiceResult Failed(params Exception[] errors)
        {
            ServiceError[] error = new ServiceError[errors.Length - 1];
            for (int i = 0; i < errors.Length; i++)
            {
                error[i].Code = errors[i].Source;
                error[i].Description = errors[i].Message;
            }
            return Failed(error);
        }
        public static ServiceResult Failed(params string[] errors)
        {
            ServiceError[] error = new ServiceError[errors.Length - 1];
            for (int i = 0; i < errors.Length; i++)
            {
                error[i].Description = errors[i];
            }
            return Failed(error);
        }
        public static ServiceResult Failed(params ServiceError[] errors)
        {
            var result = new ServiceResult { Succeeded = false };
            if (errors != null)
            {
                foreach (var item in errors)
                {
                    result.Errors.Append(item);
                }
            }
            return result;
        }
    }
}
