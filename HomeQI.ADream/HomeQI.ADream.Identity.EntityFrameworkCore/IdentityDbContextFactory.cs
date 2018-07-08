﻿using HomeQI.ADream.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;

namespace HomeQI.Adream.Identity.EntityFrameworkCore
{
    public class IdentityDbContextFactory : IDesignTimeDbContextFactory<IdentityDbContext>
    {
        public IdentityDbContext CreateDbContext(string[] args)
        {
            DbContextOptionsBuilder<IdentityDbContext> builder = new DbContextOptionsBuilder<IdentityDbContext>();
            var adreamdb = new IdentityDbContext(builder.Options);
            var conn = adreamdb.ConnectionString ?? ConfigHelper.GetJsonConfig().GetConnectionString("DefaultConnection");
            ADreamDbContextConfigurer<IdentityDbContext>.Configure(builder, conn);
            // adreamdb.CheckMigrationsAsync().Start();
            return adreamdb; ;
        }
    }
}
