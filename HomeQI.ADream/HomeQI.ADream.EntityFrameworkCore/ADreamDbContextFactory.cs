using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace HomeQI.ADream.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ADreamDbContextFactory<Context> : IDesignTimeDbContextFactory<Context> where Context : ADreamDbContext
    {
        public Context CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<Context>();
            var adreamdb = default(Context);
            var conn = adreamdb.ConnectionString ?? ConfigHelper.GetJsonConfig().GetConnectionString("DefaultConnection");
            ADreamDbContextConfigurer<Context>.Configure(builder, conn);
            //adreamdb.CheckMigrationsAsync();
            return adreamdb; ;
        }
    }
}
