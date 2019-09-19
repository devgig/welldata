using Microsoft.EntityFrameworkCore;
using WellData.Core.Data.Entities;

namespace WellData.Core.Data
{
    public class WellDataDbContext : DbContext
    {
        public WellDataDbContext(DbContextOptions<WellDataDbContext> options) : base(options)
        {
        }
        public DbSet<Well> Wells { get; set; }
        public DbSet<Tank> Tanks { get; set; }
    }
}
