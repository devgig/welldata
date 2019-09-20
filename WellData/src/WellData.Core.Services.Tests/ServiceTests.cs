using Xunit;
using Xunit.Ioc.Autofac;
using System.Threading.Tasks;
using System.Linq;
using WellData.Core.Services.Models;

namespace WellData.Tests
{
    [UseAutofacTestFramework]
    public class ServiceTests
    {
        private readonly IWellProvider _wellProvider;

        public ServiceTests() { }
        public ServiceTests(IWellProvider wellProvider)
        {
            _wellProvider = wellProvider;
        }

        [Fact]
        public async Task should_return_0_wells()
        {
            var wells = await _wellProvider.GetAll();
            Assert.Empty(wells);

        }
    }
}
