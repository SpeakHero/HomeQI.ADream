using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace HomeQI.ADream.Infrastructure.Core
{
    public abstract class DisposeBase
    {
        /// <summary>
        /// 取消令牌用于取消操作。
        /// </summary>
        protected virtual CancellationToken CancellationToken => CancellationToken.None;
        protected bool _disposed;
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// 释放角色管理器所使用的非托管资源，并可选地释放托管资源。
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {

            }
            _disposed = true;
        }
    }
}
