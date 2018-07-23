using AutoMapper;
using AutoMapper.QueryableExtensions;
using HomeQI.ADream.Entities.Framework;
using HomeQI.ADream.Infrastructure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace HomeQI.ADream.EntityFrameworkCore
{
    public class ManagerBase<TEntity, IStore, IResult, TError> : DisposeBase, IDisposable
        where IStore : IEntityStore<TEntity, IResult, TError>
        where TEntity : EntityBase<string>
         where IResult : IBaseResult<TError> where TError : IBaseError
    {
        public virtual IStore Store { get; protected set; }
        public virtual IHttpContextAccessor HttpContextAccessor { get; protected set; }
        public IMapper Mapper { get; }

        public ManagerBase(IStore store)
        {
            Store = store;
            Mapper = Store.GetServices<IMapper>();
            HttpContextAccessor = Store.GetServices<IHttpContextAccessor>();
        }

        public IQueryable<TDto> GetDtos<TDto>() => Store.EntitySet.
                AsNoTracking().ProjectTo<TDto>(Mapper.ConfigurationProvider);
        public virtual async Task<IPagedList<TResult>> GetListAsync<TResult>(PageListPar pageListPar, Expression<Func<TEntity, TResult>> selector)
        {
            return await Store.GetListAsync(pageListPar, selector);
        }
        public virtual async Task<IPagedList<TEntity>> GetListAsync(PageListPar pageListPar)
        {
            return await Store.GetListAsync(pageListPar);
        }
        public virtual async Task<IPagedList<TResult>> GetListAsync<TResult>(PageListPar pageListPar)
        {
            return await Store.GetListAsync<TResult>(pageListPar);
        }
        public virtual async Task<TResult> FindAsync<TResult>(Expression<Func<TEntity, TResult>> selector, Expression<Func<TEntity, bool>> predicate)
        {
            var entity = Store.EntitySet;
            IQueryable<TResult> q = predicate == null ?
                entity.Select(selector) :
                entity.Where(predicate).Select(selector);
            return await q.FirstOrDefaultAsync();
        }
        public virtual async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = predicate == null ?
              Store.EntitySet :
               Store.EntitySet.Where(predicate);
            return await entity.FirstOrDefaultAsync();
        }
    }
}
