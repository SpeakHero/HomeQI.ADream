using System;
using System.Collections.Generic;
using System.Text;
using HomeQI.Adream.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
