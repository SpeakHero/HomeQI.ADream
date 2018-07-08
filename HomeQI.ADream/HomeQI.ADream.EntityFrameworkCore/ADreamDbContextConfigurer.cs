using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace HomeQI.ADream.EntityFrameworkCore
{
    public static class ADreamDbContextConfigurer<Context> where Context : ADreamDbContext
    {
        public static void Configure(DbContextOptionsBuilder<Context> builder, string connectionString) => builder.UseMySql(connectionString);

        public static void Configure(DbContextOptionsBuilder<Context> builder, DbConnection connection) => builder.UseMySql(connection);
    }
}
