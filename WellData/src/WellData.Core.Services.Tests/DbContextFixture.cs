using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellData.Core.Data;

namespace WellData.Core.Services.Tests
{
    public class DbContextFixture : IDisposable
    {
        public DbContextFixture(WellDataDbContext dbContext)
        {
            DbContext = dbContext;   
        }

        public void Dispose()
        {
            // ... clean up test data from the database ...
        }

        public WellDataDbContext DbContext { get; private set; }
    }
}
