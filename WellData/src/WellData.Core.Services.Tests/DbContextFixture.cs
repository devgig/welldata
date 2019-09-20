using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using WellData.Core.Data;

namespace WellData.Core.Services.Tests
{
    public class DbContextFixture : IDisposable
    {
        private readonly WellDataDbContext _dbContext;

        public DbContextFixture(WellDataDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Dispose()
        {
            _dbContext.CreateEmptyViaDelete();
        }

    }
}
