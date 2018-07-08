using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.EntityFrameworkCore;
using HomeQI.ADream.IServcies;
using HomeQI.ADream.IServcies.Repository;
using NLog;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading;

namespace HomeQI.ADream.Services
{
    public class ServiceFactory : IServiceFactory
    {
        internal ILogger _logger;

        public ServiceFactory(IUnitOfWork unitOfWork)
        {
            UnitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _logger = LogManager.GetCurrentClassLogger();
        }

        public virtual IUnitOfWork UnitOfWork { get; }
    }
    public class ServiceFactory<TEntity>:IDisposable where TEntity : EntityBase
    {
        protected ILogger _logger;
        public ServiceFactory(
            IServiceProvider services)
        {
            _services = services;
            _logger = LogManager.GetCurrentClassLogger();
        }

        protected bool _disposed;
        protected static readonly RandomNumberGenerator _rng = RandomNumberGenerator.Create();
        protected IServiceProvider _services;

      
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
        /// <summary>
        /// 取消令牌用于取消操作.
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;
        /// <summary>
        /// Releases all resources used by the role manager.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the role manager and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
       
            _disposed = true;
        }
    }
}
