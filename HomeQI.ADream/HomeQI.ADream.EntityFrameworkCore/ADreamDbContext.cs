using Microsoft.EntityFrameworkCore;

namespace HomeQI.ADream.EntityFrameworkCore
{
    public partial class ADreamDbContext : DbContext
    {
        public ADreamDbContext( DbContextOptions options) : base(options)
        {
        }
    }
}
