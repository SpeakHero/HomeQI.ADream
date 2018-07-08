using System;
using System.Collections.Generic;
using System.Text;

namespace HomeQI.ADream.Infrastructure.Core
{
    public interface IBaseResult<TError> where TError : IBaseError
    {
        IEnumerable<TError> Errors { get; }
        bool Succeeded { get; }
        object Result { get; }
    }

    public interface IBaseError
    {
        string Code { get; set; }
        string Description { get; set; }
    }
}
