using System;
using System.Threading.Tasks;
using TestSupport.EfHelpers;
using WellData.Core.Data;
using WellData.Core.Services.Data;
using Xunit;
using Xunit.Ioc.Autofac;

namespace WellData.Core.Services.Tests
{
    [UseAutofacTestFramework]
    [TestCaseOrderer("WellData.Core.Services.Tests.Database", "WellData.Core.Services.Tests")]
    public class UploadTests  : IClassFixture<DbContextFixture>
    {
        private readonly IWellDataImporter _wellDataImporter;
        private readonly DbContextFixture _dbContextFixture;

        public UploadTests() { }
        public UploadTests(IWellDataImporter wellDataImporter, DbContextFixture dbContextFixture)
        {
            _wellDataImporter = wellDataImporter;
            _dbContextFixture = dbContextFixture;
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


    }
}
