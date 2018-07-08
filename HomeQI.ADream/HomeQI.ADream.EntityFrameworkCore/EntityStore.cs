using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;
namespace HomeQI.ADream.EntityFrameworkCore
{
    public class EntityStore<TEntity, IResult, TDbContext, TError> : IEntityStore<TEntity, IResult, TError> where TEntity : EntityBase<string> where IResult : BaseResult<IResult, TError>, new() where TDbContext : ADreamDbContext where TError : BaseError, new()
    {
        public IQueryable<TEntity> EntitySet => DbEntitySet;
        protected virtual TDbContext Context { get; private set; }
        protected virtual DbSet<TEntity> DbEntitySet => Context.Set<TEntity>();
        public virtual BaseErrorDescriber ErrorDescriber { get; set; }
        private bool _disposed;

        protected virtual ILogger Logger { get; set; }
        public EntityStore(TDbContext context, BaseErrorDescriber errorDescriber, ILoggerFactory loggerFactory)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            Context = context ?? throw new ArgumentNullEx(nameof(context));
            ErrorDescriber = errorDescriber ?? throw new ArgumentNullEx(nameof(errorDescriber));
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing && !_disposed)
            {
                _disposed = true;
            }
        }
        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                Dispose();
                var ex = new ObjectDisposedException(GetType().Name);
                Logger.LogError(new EventId(1, "ThrowIfDisposed()"), ex, ex.Message, ex);
                throw ex;
            }
        }
        public virtual bool AutoSaveChanges { get; set; } = true;
        protected virtual async Task SaveChanges(CancellationToken cancellationToken)
        {
            if (AutoSaveChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
        }
        protected virtual async Task<IResult> SaveChangesAsync(CancellationToken cancellationToken)
        {
            TError baseError = new TError
            {
                Code = "updata",
                Description = "保存失败"
            };
            IResult result = new IResult();
            try
            {
                if (AutoSaveChanges)
                {
                    var row = await Context.SaveChangesAsync(cancellationToken);
                    if (row > 0)
                    {
                        result = BaseResult<IResult, TError>.Success(row);
                    }
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                baseError.Code = ex.Source;
                baseError.Description = ex.Message;
                result = BaseResult<IResult, TError>.Failed(baseError);
                Logger.Log(LogLevel.Error, ex, "error", result);
            }
            return result;
        }

        public virtual Task<TEntity> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            return DbEntitySet.FindAsync(new object[1]
    {
            id
    });
        }
        public virtual Task<IResult> CreateAsync<IEntity>(IEntity entity, CancellationToken cancellationToken) where IEntity : EntityBase<string>
        {
            Context.AddAsync(entity, cancellationToken);
            Logger.LogTrace(nameof(CreateAsync), entity);
            return SaveChangesAsync(cancellationToken);
        }
        public virtual Task<IResult> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return CreateAsync<TEntity>(entity, cancellationToken);
        }
        public virtual Task<IResult> DeleteAsync<IEntity>(IEntity entity, CancellationToken cancellationToken) where IEntity : EntityBase<string>
        {
            // DbEntitySet.Remove(entity);
            entity.IsDeleted = true;
            Context.Entry(entity).Property(p => p.IsDeleted).IsModified = true;
            Logger.LogTrace(nameof(DeleteAsync), entity);
            return SaveChangesAsync(cancellationToken);
        }
        public virtual Task<IResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return DeleteAsync<TEntity>(entity, cancellationToken);
        }
        public virtual Task<IResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return UpdateAsync<TEntity>(entity, cancellationToken, null);
        }
        public virtual Task<IResult> UpdateAsync<Entitys>(Entitys entity, CancellationToken cancellationToken, params string[] propertys) where Entitys : EntityBase<string>
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            entity.CheakArgument();
            if (propertys == null)
            {
                Context.Update(entity);
            }
            else
            {
                var entry = Context.Entry(entity);
                //entry.State = EntityState.Unchanged;
                foreach (var property in propertys)
                {
                    entry.Property(property).IsModified = true;
                }
                entity.EditedTime = DateTime.Now;
                entry.Property(d => d.EditedTime).IsModified = true;
            }
            Logger.LogTrace(nameof(UpdateAsync), entity);
            return SaveChangesAsync(cancellationToken);
        }
        public virtual Task<IResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken, params string[] propertys)
        {
            return UpdateAsync<TEntity>(entity, cancellationToken, propertys);
        }
        public IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                int pageIndex = 0,
                                                int pageSize = 20,
                                                bool disableTracking = true)
        {
            IQueryable<TEntity> query = EntitySet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null ? orderBy(query).ToPagedList(pageIndex, pageSize) : query.ToPagedList(pageIndex, pageSize);
        }


        public Task<IPagedList<TEntity>> GetPagedListAsync(Expression<Func<TEntity, bool>> predicate = null,
                                                           Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                           Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                           int pageIndex = 0,
                                                           int pageSize = 20,
                                                           bool disableTracking = true,
                                                           CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IQueryable<TEntity> query = EntitySet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null ? orderBy(query).ToPagedListAsync(pageIndex, pageSize) : query.ToPagedListAsync(pageIndex, pageSize);
        }

        public IPagedList<TResult> GetPagedList<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                         Expression<Func<TEntity, bool>> predicate = null,
                                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                         int pageIndex = 0,
                                                         int pageSize = 20,
                                                         bool disableTracking = true)
            where TResult : class
        {
            IQueryable<TEntity> query = EntitySet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null
                ? orderBy(query).Select(selector).ToPagedList(pageIndex, pageSize)
                : query.Select(selector).ToPagedList(pageIndex, pageSize);
        }


        public Task<IPagedList<TResult>> GetPagedListAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                                    Expression<Func<TEntity, bool>> predicate = null,
                                                                    Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                                    Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                                    int pageIndex = 0,
                                                                    int pageSize = 20,
                                                                    bool disableTracking = true,
                                                                    CancellationToken cancellationToken = default)
            where TResult : class
        {
            cancellationToken.ThrowIfCancellationRequested();
            IQueryable<TEntity> query = EntitySet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null
                ? orderBy(query).Select(selector).ToPagedListAsync(pageIndex, pageSize)
                : query.Select(selector).ToPagedListAsync(pageIndex, pageSize);
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                         bool disableTracking = true)
        {
            IQueryable<TEntity> query = EntitySet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            if (orderBy != null)
            {
                return orderBy(query).FirstOrDefault();
            }
            else
            {
                return query.FirstOrDefault();
            }
        }


        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>> predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = EntitySet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null
                ? await orderBy(query).FirstOrDefaultAsync(cancellationToken)
                : await query.FirstOrDefaultAsync(cancellationToken);
        }

        public TResult GetFirstOrDefault<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                  bool disableTracking = true)
        {
            IQueryable<TEntity> query = EntitySet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null ? orderBy(query).Select(selector).FirstOrDefault() : query.Select(selector).FirstOrDefault();
        }

        public async Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                  bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = EntitySet;
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }

            if (include != null)
            {
                query = include(query);
            }

            if (predicate != null)
            {
                query = query.Where(predicate);
            }

            return orderBy != null
                ? await orderBy(query).Select(selector).FirstOrDefaultAsync(cancellationToken)
                : await query.Select(selector).FirstOrDefaultAsync(cancellationToken);
        }

        public IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {
            return EntitySet.FromSql(sql, parameters);
        }

        public IQueryable<TEntity> GetAll()
        {
            return EntitySet;
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null, CancellationToken cancellationToken = default) => predicate == null ? EntitySet.CountAsync(cancellationToken) : EntitySet.CountAsync(predicate, cancellationToken);

        public Task<string> GetEntityIdAsync(TEntity entity)
        {
            return Task.FromResult(entity.Id);
        }
    }
}
