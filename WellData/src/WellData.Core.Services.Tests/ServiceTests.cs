using Xunit;
using Xunit.Ioc.Autofac;
using System.Threading.Tasks;
using System.Linq;
using WellData.Core.Services.Models;
using WellData.Core.Data;
using TestSupport.EfHelpers;

namespace WellData.Tests
{
    [UseAutofacTestFramework]
    [TestCaseOrderer("WellData.Core.Services.Tests.Database", "WellData.Core.Services.Tests")]
    public class ServiceTests
    {
        private readonly IWellProvider _wellProvider;
        private readonly WellDataDbContext _dbContext;

        public ServiceTests() { }
        public ServiceTests(IWellProvider wellProvider, WellDataDbContext dbContext)
        {
            _wellProvider = wellProvider;
            _dbContext = dbContext;
        }

        [Fact]
        public async Task should_return_0_wells()
        {
            _dbContext.CreateEmptyViaDelete();
            var wells = await _wellProvider.GetAll();
            Assert.Empty(wells);

        }
    }
}
