using System;
using Microsoft.Extensions.Logging;

namespace HomeQI.ADream.Infrastructure.Core
{
    public interface IServiceBase
    {
        ILogger Logger { get; }
        IServiceProvider ServiceProvider { get; }
        T GetService<T>(bool required = false);
    }
}