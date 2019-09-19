using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellData.Core.Data;
using WellData.Core.Data.Entities;
using WellData.Core.Data.Extensions;
using WellData.Core.Data.Models;
using WellData.Core.Services.Common;

namespace WellData.Core.Services.Data
{
    public interface IWellDataImporter
    {
        Task<int> Upload(string uploadFile);
    }
    public class WellDataImporter : IWellDataImporter
    {
        private readonly ICsvFileFactory _csvFileFactory;
        private readonly WellDataDbContext _wellDbContext;
        
        public WellDataImporter(
            ICsvFileFactory csvFileFactory,
            WellDataDbContext wellDbContext)
        {
            _csvFileFactory = csvFileFactory;
            _wellDbContext = wellDbContext;
        }

        public async Task<int> Upload(string uploadFile)
        {
            
            var wells = new Dictionary<string, Well>();
            var csv = _csvFileFactory.NewCsvFile(uploadFile);

            foreach (var line in csv.LazyRead())
            {
                //assumed to be the key for a well
                var api = line[WellColumnConstants.API];

                if(wells.ContainsKey(api))
                {
                    wells[api].Tanks.Add(PopulateTank(line));
                }
                else
                {
                    var well = PopulateWell(line);
                    well.Tanks.Add(PopulateTank(line));
                    wells.Add(api, well);
                }
            }

            //clear previous items for this app.
            foreach (var item in _wellDbContext.Wells)
                _wellDbContext.Wells.Remove(item);

            await _wellDbContext.SaveChangesAsync();

            var items = wells.Select(x => x.Value).ToArray();
            if (!items.Any())
                return await Task.FromResult(0);

            await _wellDbContext.Wells.AddRangeAsync(items);
            return await _wellDbContext.SaveChangesAsync();
        }

        private Tank PopulateTank(IDictionary<string, string> line)
        {
            return new Tank
            {
                //assuming the MID is the Tank Id
                Id = line[TankColumnConstants.MID].ToNumber(),
                BbblsPerInch = line[TankColumnConstants.BbblsPerInch].ToDecimal(),
                County = line[TankColumnConstants.County],
                Name = line[TankColumnConstants.Name],
                Number = line[TankColumnConstants.Number].ToNumber(),
                RNG = line[TankColumnConstants.RNG],
                SEC = line[TankColumnConstants.SEC].ToNumber(),
                Size = line[TankColumnConstants.Size].ToNumber(),
                TWP = line[TankColumnConstants.TWP]
            };

        }
        private Well PopulateWell(IDictionary<string, string> line)
        {
            return new Well
            {
                //assuming the API # is the Well Id
                Id = line[WellColumnConstants.API].ToDouble(),
                Latitude = line[WellColumnConstants.Latitude].ToDecimal(),
                Longitude = line[WellColumnConstants.Longitude].ToDecimal(),
                Owner = line[WellColumnConstants.Owner],
                Property = line[WellColumnConstants.Property].ToNumber(),
                LeaseOrWellName = line[WellColumnConstants.Name],
                Tanks = new List<Tank>()
            };

        }


    }
}
