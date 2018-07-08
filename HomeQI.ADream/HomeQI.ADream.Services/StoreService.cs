using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.EntityFrameworkCore;
using HomeQI.ADream.Services.Core;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.Infrastructure.Core
{
    public class StoreService<TEntity> : ServiceBase, IStoreService<TEntity> where TEntity : EntityBase
    {
        public StoreService(ADreamDbContext aDreamDb, IServiceProvider serviceProvider, ILoggerFactory logger) : base(serviceProvider, logger) => Context = aDreamDb;
        protected ADreamDbContext Context { get; private set; }
        protected virtual DbSet<TEntity> DbEntitySet => Context.Set<TEntity>();
        public virtual bool AutoSaveChanges { get; set; } = true;
        protected virtual async Task<ServiceResult> SaveChangesAsync(CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                if (AutoSaveChanges)
                {
                    var row = await Context.SaveChangesAsync(cancellationToken);
                    if (row > 0)
                    {
                        return ServiceResult.Success(row);
                    }
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                Logger.LogError(ex.Message, ex);
                return ServiceResult.Failed(ex);
            }
            var exr = ServiceResult.Failed(new ServiceError { Code = "updata", Description = "更新失败" });
            Logger.LogError(exr.Errors.ToJson(), exr);
            return exr;
        }
        public virtual Task<TEntity> FindByIdAsync(string id, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            return DbEntitySet.FindAsync(new object[1]
    {
            id
    });
        }
        public virtual async Task<ServiceResult> CreateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            await DbEntitySet.AddAsync(entity);
            return await SaveChangesAsync(cancellationToken);
        }

        public virtual async Task<ServiceResult> DeleteAsync(TEntity entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            DbEntitySet.Remove(entity);
            return await SaveChangesAsync(cancellationToken);
        }
        public virtual async Task<ServiceResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            try
            {
                Context.Attach(entity);
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            entity.EditedTime = DateTime.Now;
            if (entity != null)
            {
                Context.Entry(entity).Property(p => p.EditedTime).IsModified = true;
                Context.Entry(entity).State = EntityState.Modified;
            }
            return await SaveChangesAsync(cancellationToken);
        }
        public virtual async Task<ServiceResult> UpdateAsync<Entity>(Entity entity, CancellationToken cancellationToken, params string[] propertys) where Entity : EntityBase
        {
            cancellationToken.ThrowIfCancellationRequested();
            ThrowIfDisposed();
            entity.CheakArgument();
            var entry = Context.Entry(entity);
            entry.State = EntityState.Unchanged;
            foreach (var property in propertys)
            {
                entry.Property(property).IsModified = true;
            }
            entry.Property(d => d.EditedTime).IsModified = true;
            return await SaveChangesAsync(cancellationToken);
        }
        public Task<ServiceResult> UpdateAsync(TEntity entity, CancellationToken cancellationToken, params string[] propertys)
        {
            return UpdateAsync<TEntity>(entity, cancellationToken, propertys);
        }
        public IPagedList<TEntity> GetPagedList(Expression<Func<TEntity, bool>> predicate = null,
                                                Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                int pageIndex = 0,
                                                int pageSize = 20,
                                                bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IQueryable<TEntity> query = DbEntitySet;
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
                return orderBy(query).ToPagedList(pageIndex, pageSize);
            }
            else
            {
                return query.ToPagedList(pageIndex, pageSize);
            }
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
            IQueryable<TEntity> query = DbEntitySet;
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
                return orderBy(query).ToPagedListAsync(pageIndex, pageSize);
            }
            else
            {
                return query.ToPagedListAsync(pageIndex, pageSize);
            }
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
            IQueryable<TEntity> query = DbEntitySet;
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
                return orderBy(query).Select(selector).ToPagedListAsync(pageIndex, pageSize);
            }
            else
            {
                return query.Select(selector).ToPagedListAsync(pageIndex, pageSize);
            }
        }

        public TEntity GetFirstOrDefault(Expression<Func<TEntity, bool>> predicate = null,
                                         Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                         Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                         bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IQueryable<TEntity> query = DbEntitySet;
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
            cancellationToken.ThrowIfCancellationRequested();
            IQueryable<TEntity> query = DbEntitySet;
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
                return await orderBy(query).FirstOrDefaultAsync();
            }
            else
            {
                return await query.FirstOrDefaultAsync();
            }
        }

      
        public  Task<TResult> GetFirstOrDefaultAsync<TResult>(Expression<Func<TEntity, TResult>> selector,
                                                  Expression<Func<TEntity, bool>> predicate = null,
                                                  Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
                                                  Func<IQueryable<TEntity>, IIncludableQueryable<TEntity, object>> include = null,
                                                  bool disableTracking = true, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();
            IQueryable<TEntity> query = DbEntitySet;
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
                return  orderBy(query).Select(selector).FirstOrDefaultAsync();
            }
            else
            {
                return  query.Select(selector).FirstOrDefaultAsync();
            }
        }

        public IQueryable<TEntity> FromSql(string sql, params object[] parameters)
        {
            return DbEntitySet.FromSql(sql, parameters);
        }
        public IQueryable<TEntity> GetAll()
        {
            return DbEntitySet;
        }

        public Task<int> CountAsync(Expression<Func<TEntity, bool>> predicate = null) => predicate == null ? DbEntitySet.CountAsync() : DbEntitySet.CountAsync(predicate);
    }
}
