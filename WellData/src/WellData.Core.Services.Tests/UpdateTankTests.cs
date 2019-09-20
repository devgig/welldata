using System.Linq;
using System.Threading.Tasks;
using WellData.Core.Services.Data;
using WellData.Core.Services.Models;
using Xunit;
using Xunit.Ioc.Autofac;
using Newtonsoft.Json;

namespace WellData.Core.Services.Tests
{
    [UseAutofacTestFramework]
    [TestCaseOrderer("WellData.Core.Services.Tests.Database", "WellData.Core.Services.Tests")]
    public class UpdateTankTests : IClassFixture<DbContextFixture>
    {
        private readonly DbContextFixture _dbContextFixture;
        private readonly IWellDataImporter _wellDataImporter;
        private readonly IWellProvider _wellProvider;
        private readonly ITankProvider _tankProvider;

        public UpdateTankTests() { }

        public UpdateTankTests(
            DbContextFixture dbContextFixture, 
            IWellDataImporter wellDataImporter,
            IWellProvider wellProvider,
            ITankProvider tankProvider)
        {
            _dbContextFixture = dbContextFixture;
            _wellDataImporter = wellDataImporter;
            _wellProvider = wellProvider;
            _tankProvider = tankProvider;
        }

        [Fact]
        public async Task should_modify_a_tank_and_save_tank_that_is_dirty()
        {
            //load data with one well
            var filename = "./Resources/WellDataTest.csv";
            await _wellDataImporter.Upload(filename);
            
            //verify there is one well
            var wells = await _wellProvider.GetAll();
            var well = wells.FirstOrDefault();
            Assert.NotNull(well);

            //get tanks for the one well
            var tanks = await _tankProvider.GetByWellId(well.Id);

            //verify expected first state
            var tank1 = tanks.FirstOrDefault(x => x.Id == 1065);
            Assert.Equal(1, tank1.Number);

            //verity expected second state
            var tank2 = tanks.FirstOrDefault(x => x.Id == 1066);
            Assert.Equal(2, tank2.Number);

            //change the number
            tank1.Number = 5;
            tank2.Number = 6;

            //both tanks should be dirty
            var dirty = tanks.Where(x => x.IsDirty()).ToArray();
            Assert.Equal(2, dirty.Count());

            //save dirty tanks
            await _tankProvider.Save(dirty);

            //get tanks again by well
            var ntanks = _tankProvider.GetByWellId(well.Id);

            //verify expected first change
            var ntank1 = tanks.FirstOrDefault(x => x.Id == 1065);
            Assert.Equal(5, ntank1.Number);

            //verity expected second change
            var ntank2 = tanks.FirstOrDefault(x => x.Id == 1066);
            Assert.Equal(6, ntank2.Number);

        }

        [Fact]
        public async Task should_save_every_update_of_a_tank()
        {
            //for comparing tanks original state
            var cotank1 = ModelConstants.GetTank();
            
            //for comparing tanks new state
            var cntank1 = new TankModel
            {
                Id = 1065,
                Name = "Sorenson",
                Number = 5,
                Size = 450.56m,
                BbblsPerInch = 2.575867769m,
                SEC = 7,
                TWP = "022M",
                RNG = "059F",
                County = "GREENLAND",
                WellId = "2508321270"
            };


            //load data with one well
            var filename = "./Resources/WellDataTest.csv";
            await _wellDataImporter.Upload(filename);

            //verify there is one well
            var wells = await _wellProvider.GetAll();
            var well = wells.FirstOrDefault();
            Assert.NotNull(well);

            //get tanks for the one well
            var tanks = await _tankProvider.GetByWellId(well.Id);

            //verify expected first state
            var tank1 = tanks.FirstOrDefault(x => x.Id == 1065);

            var stank1 = JsonConvert.SerializeObject(tank1);
            var scotank1 = JsonConvert.SerializeObject(cotank1);
            Assert.Equal(stank1, scotank1);

            //change all of tank one
            tank1.Name = "Sorenson";
            tank1.Number = 5;
            tank1.Size = 450.56m;
            tank1.BbblsPerInch = 2.575867769m;
            tank1.SEC = 7;
            tank1.TWP = "022M";
            tank1.RNG = "059F";
            tank1.County = "GREENLAND";


            //save dirty tank
            var dirty = tanks.Where(x => x.IsDirty()).ToArray();
            await _tankProvider.Save(dirty);

            //get tanks again by well
            var ntanks = _tankProvider.GetByWellId(well.Id);

            //verify expected first change
            var ntank1 = tanks.FirstOrDefault(x => x.Id == 1065);

            var sntank1 = JsonConvert.SerializeObject(ntank1);
            var sncotank1 = JsonConvert.SerializeObject(cntank1);
            Assert.Equal(sntank1, sncotank1);

        }

    }
}
