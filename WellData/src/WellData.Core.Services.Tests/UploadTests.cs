using System.Threading.Tasks;
using WellData.Core.Services.Data;
using WellData.Core.Services.Models;
using Xunit;
using Xunit.Ioc.Autofac;
using System.Linq;
using Newtonsoft.Json;

namespace WellData.Core.Services.Tests
{
    [UseAutofacTestFramework]
    [TestCaseOrderer("WellData.Core.Services.Tests.Database", "WellData.Core.Services.Tests")]
    public class UploadTests  : IClassFixture<DbContextFixture>
    {
        private readonly IWellDataImporter _wellDataImporter;
        private readonly DbContextFixture _dbContextFixture;
        private readonly IWellProvider _wellProvider;
        private readonly ITankProvider _tankProvider;

        public UploadTests() { }
        public UploadTests(IWellDataImporter wellDataImporter, 
            DbContextFixture dbContextFixture,
            IWellProvider wellProvider,
            ITankProvider tankProvider)
        {
            _wellDataImporter = wellDataImporter;
            _dbContextFixture = dbContextFixture;
            _wellProvider = wellProvider;
            _tankProvider = tankProvider;
        }


        [Fact]
        public async Task should_upload_csv_file()
        {
            var filename = "./Resources/WellDataTest.csv";
            var result = await _wellDataImporter.Upload(filename);
            Assert.Equal(3, result);
        }

        [Fact]
        public async Task should_upload_xlsx_file()
        {
            var filename = "./Resources/WellDataTest.xlsx";
            var result = await _wellDataImporter.Upload(filename);
            Assert.Equal(6, result);
        }

        [Fact]
        public async Task should_upload_csv_file_correctly()
        {
            var filename = "./Resources/WellDataTest.csv";
            await _wellDataImporter.Upload(filename);

            await Verify();
          
        }
      
        [Fact]
        public async Task should_upload_xlsx_file_correctly()
        {
            var filename = "./Resources/WellDataTest.xlsx";
            await _wellDataImporter.Upload(filename);

            await Verify();
        }

        private async Task Verify()
        {
            //verify well was uploaded correctly
            var wells = await _wellProvider.GetAll();
            var well = wells.FirstOrDefault(x => x.Id == "2508321270");
            var owell = ModelConstants.GetWell();

            var swell = JsonConvert.SerializeObject(well);
            var sowell = JsonConvert.SerializeObject(owell);
            Assert.Equal(swell, sowell);

            //verify tank was uploaded correctly
            var tanks = await _tankProvider.GetByWellId(well.Id);
            var tank = tanks.FirstOrDefault(x => x.Id == 1065);
            var otank = ModelConstants.GetTank();

            var stank = JsonConvert.SerializeObject(tank);
            var sotank = JsonConvert.SerializeObject(otank);
            Assert.Equal(stank, sotank);

        }

    }
}
