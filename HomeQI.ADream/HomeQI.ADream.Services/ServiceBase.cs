using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
namespace HomeQI.ADream.Services
{
    public class ServiceBase : IDisposable
    {
        /// <summary>
        ///取消标记,用来取消操作。
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;
        #region IDisposable Support
        protected bool _disposed = false; // 要检测冗余调用

        public IServiceProvider ServiceProvider { get; }
        public ILogger Logger { get; }

        public ServiceBase(IServiceProvider serviceProvider, ILoggerFactory logger)
        {
            serviceProvider.CheakArgument();
            logger.CheakArgument();
            ServiceProvider = serviceProvider;
            Logger = logger.CreateLogger(GetType());
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: 释放托管状态(托管对象)。
                }

                // TODO: 释放未托管的资源(未托管的对象)并在以下内容中替代终结器。
                // TODO: 将大型字段设置为 null。

                _disposed = true;
            }
        }

        // TODO: 仅当以上 Dispose(bool disposing) 拥有用于释放未托管资源的代码时才替代终结器。
        // ~ServiceBase() {
        //   // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
        //   Dispose(false);
        // }

        // 添加此代码以正确实现可处置模式。
        public void Dispose()
        {
            // 请勿更改此代码。将清理代码放入以上 Dispose(bool disposing) 中。
            Dispose(true);
            // TODO: 如果在以上内容中替代了终结器，则取消注释以下行。
            GC.SuppressFinalize(this);
        }
        /// <summary>
        /// Throws if this class has been disposed.
        /// </summary>
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        #endregion

        public T GetService<T>(bool required = false)
        {
            return !required ? ServiceProvider.GetService<T>() : ServiceProvider.GetRequiredService<T>();
        }

    }
}
