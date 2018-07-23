using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace HomeQI.ADream.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class ADreamDbContextFactory<Context> : IDesignTimeDbContextFactory<Context> where Context : ADreamDbContext, new()
    {
        public Context CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<Context>();
            var adreamdb = new Context();
            var conn = adreamdb.ConnectionString ?? adreamdb.Configuration.GetConnectionString("DefaultConnection");
            ADreamDbContextConfigurer<Context>.Configure(builder, conn);
            //adreamdb.CheckMigrationsAsync();
            return adreamdb; ;
        }
    }
}
