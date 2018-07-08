using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using System;

namespace HomeQI.ADream.EntityFrameworkCore
{
    public partial class ADreamDbContext : DbContext
    {
        public string ConnectionString { get; set; }
        public IConfiguration Configuration => ConfigHelper.GetJsonConfig();
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            var conn = Configuration.GetConnectionString("DefaultConnection");
            var value = Configuration.GetValue<string>("DataBase").ToLower();
            ConnectionString = ConnectionString ?? conn;
            if (!string.IsNullOrEmpty(ConnectionString))
            {
                optionsBuilder = value == "mysql" ? optionsBuilder.UseMySql(ConnectionString) : optionsBuilder.UseSqlServer(ConnectionString);
            }
            optionsBuilder.EnableSensitiveDataLogging();
            MemoryCache cache = new MemoryCache(new MemoryCacheOptions());
            optionsBuilder.UseMemoryCache(cache);
        }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            //            foreach (var item in GetReferencingAssemblies())
            //            {
            //                var method = SetGlobalQueryMethod.MakeGenericMethod(item);
            //#if Debug
            //                Debug.WriteLine("Adding global query for: " + typeof(item));
            //#endif 

            //                method.Invoke(this, new object[] { builder });
            //            }
            builder.EnableAutoHistory(null);
        }
    }
}
