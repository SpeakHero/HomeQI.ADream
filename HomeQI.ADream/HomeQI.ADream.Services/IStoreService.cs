using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.Services.Core;
using Microsoft.EntityFrameworkCore.Query;

namespace HomeQI.ADream.Infrastructure.Core
{
    public interface IStoreService<TEntity> where TEntity : EntityBase
    {
        bool AutoSaveChanges { get; set; }

        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null);
        Task<ServiceResult> CreateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<ServiceResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity> FindByIdAsync(string id, CancellationToken cancellationToken);
        IQueryable<TEntity> FromSql(string sql, params object[] parameters);
        IQueryable<TEntity> GetAll();
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, CancellationToken cancellationToken = default);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, CancellationToken cancellationToken = default);
        Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, CancellationToken cancellationToken = default);
        IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default);
        Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default);
        Task<IPagedList<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default) where TResult : class;
        Task<ServiceResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<ServiceResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken, params string[] propertys);
        Task<ServiceResult> UpdateAsync<Entity>(Entity entity, CancellationToken cancellationToken, params string[] propertys) where Entity : EntityBase;
    }
}