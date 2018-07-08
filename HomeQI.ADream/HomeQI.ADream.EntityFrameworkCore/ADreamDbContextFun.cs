using HomeQI.ADream.Entities.Framework;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace HomeQI.ADream.EntityFrameworkCore
{
    public partial class ADreamDbContext : DbContext
    {
        private static readonly MethodInfo SetGlobalQueryMethod = typeof(ADreamDbContext).GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                                       .Single(t => t.IsGenericMethod && t.Name == "SetGlobalQuery");


        public T GetService<T>() => Database.GetService<T>();
        public void SetGlobalQuery<T, TKey>(ModelBuilder builder) where T : EntityBase<TKey> where TKey : IEquatable<TKey>
        {
            builder.Entity<T>().HasQueryFilter(f => !f.IsDeleted);
        }
        /// <summary>
        /// 检查迁移
        /// </summary>
        public async Task CheckMigrationsAsync()
        {
            //判断是否有待迁移
            if ((await Database.GetAppliedMigrationsAsync()).Any())
            {
                Console.WriteLine("迁徙中...");
                //执行迁移
                await Database.MigrateAsync();
                Console.WriteLine("迁徙完成");
            }
            Console.WriteLine("Check Migrations Coomplete!");
        }
        public async Task<IDataReader> ExecuteReaderAsync(RawSqlString sql, params object[] parameters)
        {
            using (await Database.GetService<IConcurrencyDetector>().EnterCriticalSectionAsync(default))
            {
                RawSqlCommand rawSqlCommand = Database.GetService<IRawSqlCommandBuilder>().Build(sql.Format, parameters);
                return (await rawSqlCommand.RelationalCommand.ExecuteReaderAsync(Database.GetService<IRelationalConnection>(), rawSqlCommand.ParameterValues)).DbDataReader;
            }
        }
        //  FormattableString
        public Task<IDataReader> ExecuteReaderAsync(FormattableString sql)
        {
            return ExecuteReaderAsync(sql.Format, sql.GetArguments());
        }
        public Task<object> ExecuteScalarAsync(FormattableString sql)
        {
            return ExecuteScalarAsync(sql.Format, sql.GetArguments());
        }
        public async Task<object> ExecuteScalarAsync(RawSqlString sql, params object[] parameters)
        {
            using (await Database.GetService<IConcurrencyDetector>().EnterCriticalSectionAsync(default))
            {
                RawSqlCommand rawSqlCommand = Database.GetService<IRawSqlCommandBuilder>().Build(sql.Format, parameters);
                return await rawSqlCommand.RelationalCommand.ExecuteScalarAsync(Database.GetService<IRelationalConnection>(), rawSqlCommand.ParameterValues);
            }
        }

        public IQueryable<TResult> ExecuteSqlCommand<TSource, TResult>(string sql, Expression<Func<TSource, TResult>> selector) where TResult : class where TSource : EntityBase
        {
            return Set<TSource>().Select(selector).FromSql(sql);
        }
        public Task<int> ExecuteSqlCommand(FormattableString sql)
        {
            return Database.ExecuteSqlCommandAsync(sql);
        }
        public Task<int> ExecuteSqlCommand(RawSqlString sql)
        {
            return Database.ExecuteSqlCommandAsync(sql);
        }
        private static IList<Type> _entityTypeCache;
        public static IEnumerable<Type> GetReferencingAssemblies()
        {
            if (_entityTypeCache == null)
            {
                var s = Assembly.Load("HomeQI.ADream.Entites");
                foreach (var item in s.GetTypes())
                {
                    if (item.BaseType != typeof(EntityBase))
                    {
                        continue;
                    }
                    if (_entityTypeCache == null)
                    {
                        _entityTypeCache = new List<Type>();
                    }
                    _entityTypeCache.Add(item);
                }
            }
            return _entityTypeCache;
        }
        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {

                    case EntityState.Added: entry.CurrentValues["IsDeleted"] = false; break;
                    case EntityState.Deleted: entry.State = EntityState.Modified; entry.CurrentValues["IsDeleted"] = true; entry.Property("IsDeleted").IsModified = true; break;
                }
            }
        }

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {

            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override int SaveChanges()
        {
            OnBeforeSaving();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(cancellationToken);
        }
    }
}
