using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.EntityFrameworkCore
{

    public interface IEntityStore<TEntity, IResult, TError> : IDisposable
        where TEntity : EntityBase<string>
        where IResult : IBaseResult<TError> where TError : IBaseError
    {
        T GetServices<T>();
        Task<IResult> SaveChangesAsync(CancellationToken cancellationToken = default);
        bool AutoSaveChanges { get; set; }
        IQueryable<TEntity> EntitySet { get; }
        BaseErrorDescriber ErrorDescriber { get; set; }
        Task<IPagedList<TEntity>> GetListAsync(PageListPar pageListPar);
        Task<IPagedList<TResult>> GetListAsync<TResult>(PageListPar pageListPar, Expression<Func<TEntity, TResult>> selector);
        Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default);
        Task<IResult> CreateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<IResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken);
        Task<TEntity> FindByIdAsync(string id, CancellationToken cancellationToken);
        IQueryable<TEntity> FromSql(string sql, params object[] parameters);
        [Obsolete("不推荐使用")]
        IQueryable<TEntity> GetAll();
        Task<string> GetEntityIdAsync(TEntity entity);
        TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true);
        TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true);
        Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, CancellationToken cancellationToken = default);
        Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, bool disableTracking = true, CancellationToken cancellationToken = default);
        IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true);
        IPagedList<TResult> GetPagedList<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true) where TResult : class;
        Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default);
        Task<IPagedList<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null, int pageIndex = 0, int pageSize = 20, bool disableTracking = true, CancellationToken cancellationToken = default) where TResult : class;
        Task<IResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken);
        Task<IResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken, params string[] propertys);
        Task<IResult> UpdateAsync<Entity>(Entity entity, CancellationToken cancellationToken, params string[] propertys) where Entity : EntityBase<string>;
        Task<IResult> CreateAsync<IEntity>(IEntity entity, CancellationToken cancellationToken) where IEntity : EntityBase<string>;
        Task<IResult> DeleteAsync<IEntity>(IEntity entity, CancellationToken cancellationToken) where IEntity : EntityBase<string>;
        Task<IPagedList<TResult>> GetListAsync<TResult>(PageListPar pageListPar);
    }
}