using Microsoft.EntityFrameworkCore;
using WellData.Core.Data.Models;

namespace WellData.Core.Data
{
    public class WellDbContext : DbContext
    {
        public WellDbContext(DbContextOptions<WellDbContext> options) : base(options)
        {
        }
        public DbSet<Well> WellItems { get; set; }
    }
}
