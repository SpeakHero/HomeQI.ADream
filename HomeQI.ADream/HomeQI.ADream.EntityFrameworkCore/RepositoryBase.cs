using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace HomeQI.ADream.EntityFrameworkCore
{
    public class RepositoryBase<T, Context> : IRepository<T, Context>
          where T : class
          where Context : ADreamDbContext
    {
        public RepositoryBase(Context _dbContext)
        {
            DbContext = _dbContext;
        }

        public Context DbContext
        {
            get;
        }

        public DbSet<T> Entities
        {
            get
            {
                return DbContext.Set<T>();
            }
        }

        public IQueryable<T> Table
        {
            get
            {
                return Entities;
            }
        }


        public void Delete(T entity, bool isNeedSave = true)
        {
            Entities.Remove(entity);
            if (isNeedSave)
            {
                DbContext.SaveChanges();
            }
        }

        public T GetById(object id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public void Insert(T entity, bool isNeedSave = true)
        {
            Entities.Add(entity);
            if (isNeedSave)
            {
                DbContext.SaveChanges();
            }
        }

        public void Update(T entity, bool isNeedSave = true)
        {
            if (isNeedSave)
            {
                DbContext.SaveChanges();
            }
        }
    }

}
