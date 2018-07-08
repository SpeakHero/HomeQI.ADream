using System;

namespace HomeQI.ADream.Infrastructure.Core
{
    public  class ManagerBase
    {
        private bool _disposed;
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }


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
