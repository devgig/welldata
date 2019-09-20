using Xunit;
using Xunit.Ioc.Autofac;
using System.Threading.Tasks;
using System.Linq;
using WellData.Core.Services.Models;
using WellData.Core.Data;
using TestSupport.EfHelpers;
using WellData.Core.Services.Tests;

namespace WellData.Tests
{
    [UseAutofacTestFramework]
    [TestCaseOrderer("WellData.Core.Services.Tests.Database", "WellData.Core.Services.Tests")]
    public class ServiceTests : IClassFixture<DbContextFixture>
    {
        private readonly IWellProvider _wellProvider;
        private readonly DbContextFixture _dbContextFixture;

        public ServiceTests() { }
        public ServiceTests(IWellProvider wellProvider, DbContextFixture dbContextFixture)
        {
            _wellProvider = wellProvider;
            _dbContextFixture = dbContextFixture;
        }

        [Fact]
        public async Task should_return_0_wells()
        {
            var wells = await _wellProvider.GetAll();
            Assert.Empty(wells);

        }
    }
}
