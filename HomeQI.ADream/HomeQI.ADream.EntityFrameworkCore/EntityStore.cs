using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
namespace HomeQI.ADream.EntityFrameworkCore
{
    public class EntityStore<TEntity, IResult, TDbContext, TError> : IEntityStore<TEntity, IResult, TError> where TEntity : EntityBase<string> where IResult : BaseResult<IResult, TError>, new() where TDbContext : ADreamDbContext where TError : BaseError, new()
    {
        public virtual IQueryable<TEntity> EntitySet => DbEntitySet;
        protected virtual TDbContext Context { get; private set; }
        private readonly IDistributedCache cache;
        protected virtual DbSet<TEntity> DbEntitySet => Context.Set<TEntity>();
        public virtual BaseErrorDescriber ErrorDescriber { get; set; }
        private bool _disposed;
        protected readonly IHttpContextAccessor httpContextAccessor;
        protected string Uid { get; set; }
        public virtual IQueryable<TResult> GetResults<TResult>()
        {
            return EntitySet.ProjectTo<TResult>(Mapper.ConfigurationProvider);
        }
        protected virtual ILogger Logger { get; set; }
        public EntityStore(TDbContext context, BaseErrorDescriber errorDescriber,
            ILoggerFactory loggerFactory, IConfiguration configuration)
        {
            Logger = loggerFactory.CreateLogger(GetType());
            Context = context ?? throw new ArgumentNullEx(nameof(context));
            Context.Configuration = configuration;
            Mapper = Context.Database.GetService<IMapper>();
            cache = GetServices<IDistributedCache>();
            ErrorDescriber = errorDescriber ??
                throw new ArgumentNullEx(nameof(errorDescriber));
            httpContextAccessor = GetServices<IHttpContextAccessor>();
            Uid = httpContextAccessor?.HttpContext?.User?.FindFirstValue("id");
            if (Uid.IsNullOrEmpty())
            {
                Uid = string.Empty;
            }
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
                Context.Dispose();
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
        public IMapper Mapper { get; }
        protected virtual async Task SaveChanges(CancellationToken cancellationToken = default)
        {
            if (AutoSaveChanges)
            {
                await Context.SaveChangesAsync(cancellationToken);
            }
        }
        public virtual async Task<IResult> SaveChangesAsync(CancellationToken cancellationToken = default)
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
        protected async Task<TEntity> GetCacheAsync(string key, CancellationToken cancellationToken = default)
        {
            return await GetCacheAsync<TEntity>(key, cancellationToken);
        }

        protected async Task<T> GetCacheAsync<T>(string key, CancellationToken cancellationToken = default)
        {
            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentNullEx(nameof(key), "key Is Null Or Empty");
            }
            var cacheValue = await cache.GetStringAsync(key);
            if (!string.IsNullOrEmpty(cacheValue))
            {
                try
                {
                    return JsonConvert.DeserializeObject<T>(cacheValue);
                }
                catch (Exception ex)
                {
                    LogerHelp.Error(ex);
                    //出错时删除key
                    cache.Remove(key);
                }
            }
            return default;
        }
        protected async Task SetCacheAsync(string key, TEntity objects, CancellationToken cancellationToken = default)
        {
            await SetCacheAsync(key, objects.ToJson(), cancellationToken);
        }
        protected async Task SetCacheAsync<T>(string key, T objects, CancellationToken cancellationToken = default) where T : class
        {
            await cache.SetStringAsync(key, objects.ToJson(), cancellationToken);
        }
        public virtual async Task<TEntity> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            var entity = (await GetCacheAsync(typeof(TEntity).Name + id, cancellationToken));
            if (entity == null)
            {
                entity = await DbEntitySet.FindAsync(new object[1] { id });
                await SetCacheAsync(typeof(TEntity).Name + id, entity, cancellationToken);
            }
            return entity;
        }
        public virtual async Task<IResult> CreateAsync<IEntity>(IEntity entity, CancellationToken cancellationToken) where IEntity : EntityBase<string>
        {
            await Context.AddAsync(entity, cancellationToken);

            Logger.LogTrace(nameof(CreateAsync), entity);
            LogerHelp.NLoger.Info(nameof(CreateAsync) + entity.ToJson());
            var save = await SaveChangesAsync(cancellationToken);
            if (save.Succeeded)
            {
                await SetCacheAsync(typeof(TEntity).Name + entity.Id, entity, cancellationToken);
            }
            return save;
        }
        public virtual Task<IResult> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            entity.CretaedUser = Uid;
            return CreateAsync<TEntity>(entity, cancellationToken);
        }
        public virtual async Task<IResult> DeleteAsync<IEntity>(IEntity entity, CancellationToken cancellationToken) where IEntity : EntityBase<string>
        {
            // DbEntitySet.Remove(entity);
            // Context.Entry(entity).State = EntityState.Unchanged;
            entity.IsDeleted = true;
            Context.Entry(entity).Property(p => p.IsDeleted).IsModified = true;
            Logger.LogTrace(nameof(DeleteAsync), entity);
            LogerHelp.NLoger.Info(nameof(DeleteAsync) + entity.ToJson());
            var save = await SaveChangesAsync(cancellationToken);
            if (save.Succeeded)
            {
                await cache.RemoveAsync(entity.Id, cancellationToken);
            }
            return save;
        }
        public virtual Task<IResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return DeleteAsync<TEntity>(entity, cancellationToken);
        }
        public virtual Task<IResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            return UpdateAsync<TEntity>(entity, cancellationToken, null);
        }
        public virtual async Task<IResult> UpdateAsync<Entitys>(Entitys entity, CancellationToken cancellationToken, params string[] propertys)
            where Entitys : EntityBase<string>
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            entity.CheakArgument();
            var entry = Context.Entry(entity);

            if (propertys == null)
            {
                entry.State = EntityState.Modified;
            }
            else
            {
                // entry.State = EntityState.Detached;
                foreach (var property in propertys)
                {
                    entry.Property(property).IsModified = true;
                }
            }

            entity.EditeUser = Uid;
            entity.EditedTime = DateTime.Now;
            entry.Property(d => d.EditedTime).IsModified = true;
            entry.Property(d => d.EditeUser).IsModified = true;
            entry.Property(p => p.CreatedTime).IsModified = false;
            entry.Property(p => p.CretaedUser).IsModified = false;
            Logger.LogTrace(nameof(UpdateAsync), entity);
            LogerHelp.NLoger.Info(nameof(UpdateAsync) + entity.ToJson());
            var save = await SaveChangesAsync(cancellationToken);
            if (save.Succeeded)
            {
                await cache.RemoveAsync(typeof(Entitys).Name + entity.Id, cancellationToken);
                await SetCacheAsync(typeof(Entitys).Name + entity.Id, entity, cancellationToken);
            }
            return save;
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
            IQueryable<TEntity> query = GetIQueryable(EntitySet,
                predicate, disableTracking);
            if (include != null)
            {
                query = include(query);
            }
            if (include != null)
            {
                query = include(query);
            }
            return query.ToPagedList(pageIndex, pageSize);
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
            IQueryable<TEntity> query = GetIQueryable(EntitySet,
                predicate, disableTracking);
            if (include != null)
            {
                query = include(query);
            }
            return ProjectToPagedListAsync<TEntity, TEntity>(query, orderBy,
             pageIndex, pageSize);
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
            IQueryable<TEntity> query = GetIQueryable(EntitySet,
                predicate, disableTracking);
            if (include != null)
            {
                query = include(query);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }
            return query.Select(selector).ToPagedList(pageIndex, pageSize);
        }
        public IPagedList<TResult> GetPagedList<TResult>(
          Expression<Func<TEntity, bool>> predicate = null,
          Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
          int pageIndex = 0,
          int pageSize = 20,
          bool disableTracking = true)
            where TResult : class
        {
            IQueryable<TEntity> query = GetIQueryable(EntitySet,
               predicate, disableTracking);
            return ProjectToPagedList<TResult, TEntity>(query, orderBy,
                pageIndex, pageSize);
        }
        public IPagedList<TResult> ProjectToPagedList<TResult, IQueryable>(IQueryable<IQueryable> queryable,
                 Func<IQueryable<IQueryable>, IOrderedQueryable<IQueryable>> orderBy = null,
            int pageIndex = 0, int pageSize = 20) where IQueryable : EntityBase<string>
        {
            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }
            return queryable.ProjectTo<TResult>(Mapper.ConfigurationProvider).
                  ToPagedList(pageIndex, pageSize);
        }
        public async Task<IPagedList<TResult>> ProjectToPagedListAsync<TResult, IQueryable>(IQueryable<IQueryable> queryable,
                 Func<IQueryable<IQueryable>, IOrderedQueryable<IQueryable>> orderBy = null,
          int pageIndex = 0,
          int pageSize = 20) where IQueryable : EntityBase<string>
        {
            if (orderBy != null)
            {
                queryable = orderBy(queryable);
            }
            return await queryable.ProjectTo<TResult>(Mapper.ConfigurationProvider).
                  ToPagedListAsync(pageIndex, pageSize);
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
            IQueryable<TEntity> query = GetIQueryable(EntitySet,
              predicate, disableTracking);
            if (include != null)
            {
                query = include(query);
            }
            if (orderBy != null)
            {
                query = orderBy(query);
            }

            return query.Select(selector).ToPagedListAsync(pageIndex, pageSize);
        }
        public virtual async Task<IPagedList<TResult>> GetListAsync<TResult>(PageListPar pageListPar, Expression<Func<TEntity, TResult>> selector)
        {
            var dbset = EntitySet.AsNoTracking();
            var results = await dbset.Where(pageListPar.Filters).DataSorting(pageListPar.Sort, pageListPar.Order).Select(selector).ToPagedListAsync(pageListPar.CurrentPage, pageListPar.Limit);
            return results;
        }
        public virtual async Task<IPagedList<TResult>> GetListAsync<TResult>(PageListPar pageListPar)
        {
            var dbset = EntitySet.AsNoTracking();
            var results = await dbset.Where(pageListPar.Filters).
                DataSorting(pageListPar.Sort, pageListPar.Order).
                ProjectTo<TResult>(Mapper.ConfigurationProvider).
                ToPagedListAsync(pageListPar.CurrentPage, pageListPar.Limit);
            return results;
        }
        public virtual async Task<IPagedList<TEntity>> GetListAsync(PageListPar pageListPar)
        {
            var dbset = EntitySet.AsNoTracking();
            var results = await dbset.Where(pageListPar.Filters).
                DataSorting(pageListPar.Sort,
                pageListPar.Order).ToPagedListAsync(pageListPar.CurrentPage,
                pageListPar.Limit);
            return results;
        }
        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                         bool disableTracking = true)
        {
            IQueryable<TEntity> query = GetIQueryable(EntitySet,
            predicate, disableTracking);
            if (include != null)
            {
                query = include(query);
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


        public async Task<TEntity> GetFirstOrDefaultAsync(Expression<Func<TEntity, bool>>
            predicate = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
            bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = GetIQueryable(EntitySet,
            predicate, disableTracking);
            if (include != null)
            {
                query = include(query);
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
            IQueryable<TEntity> query = GetIQueryable(EntitySet,
              predicate, disableTracking);
            if (include != null)
            {
                query = include(query);
            }
            return orderBy != null ? orderBy(query).Select(selector).FirstOrDefault() : query.Select(selector).FirstOrDefault();
        }
        public async Task<TResult> GetFirstOrDefaultAsync<TResult>(
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  bool disableTracking = true, CancellationToken cancellationToken = default)

        {
            return await ProjectTo<TResult, TEntity>(EntitySet, predicate,
                disableTracking).FirstOrDefaultAsync();
        }
        public async Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                  bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            IQueryable<TEntity> query = GetIQueryable(EntitySet,
            predicate, disableTracking);
            return orderBy != null
                ? await orderBy(query).Select(selector).FirstOrDefaultAsync(cancellationToken)
                : await query.Select(selector).FirstOrDefaultAsync(cancellationToken);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <typeparam name="TEntitys"></typeparam>
        /// <param name="query"></param>
        /// <param name="predicate"></param>
        /// <param name="disableTracking"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        public IQueryable<TResult> ProjectTo<TResult, TEntitys>(IQueryable<TEntitys> query,
                                                  Expression<Func<TEntitys, bool>> predicate = null,
                                                  bool disableTracking = true
                                                 ) where TEntitys : EntityBase<string>
        {
            return GetIQueryable(query, predicate, disableTracking).ProjectTo<TResult>(Mapper.ConfigurationProvider);
        }
        public IQueryable<TEntitys> GetIQueryable<TEntitys>(IQueryable<TEntitys> query,
                                                 Expression<Func<TEntitys, bool>> predicate = null,
                                                 bool disableTracking = true
                                                ) where TEntitys : EntityBase<string>
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }
            if (disableTracking)
            {
                query = query.AsNoTracking();
            }
            if (predicate != null)
            {
                query = query.Where(predicate);
            }
            return query;
        }
        public IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {
            return EntitySet.FromSql(sql, parameters);
        }
        public T GetServices<T>()
        {
            return Context.Database.GetService<T>();
        }
        [Obsolete("不推荐使用")]
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
