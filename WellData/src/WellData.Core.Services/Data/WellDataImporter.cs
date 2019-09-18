using System.Collections.Generic;
using System.Threading.Tasks;
using WellData.Core.Data;
using WellData.Core.Data.Extensions;
using WellData.Core.Data.Models;
using WellData.Core.Services.Common;

namespace WellData.Core.Services.Data
{
    public interface IWellDataImporter
    {
        Task<bool> Upload(string uploadFile);
    }
    public class WellDataImporter : IWellDataImporter
    {
        private readonly ICsvFileFactory _csvFileFactory;
        private readonly WellDbContext _wellDbContext;
        
        public WellDataImporter(
            ICsvFileFactory csvFileFactory,
            WellDbContext wellDbContext)
        {
            _csvFileFactory = csvFileFactory;
            _wellDbContext = wellDbContext;
        }

        public async Task<bool> Upload(string uploadFile)
        {
            var wells = new Dictionary<int, Well>();
            var csv = _csvFileFactory.NewCsvFile(uploadFile);

            foreach (var line in csv.LazyRead())
            {
                //assumed to be the key for a well
                var api = line[WellColumnConstants.API].ToNumber();

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
            return await Task.FromResult(true);
        }

        private Tank PopulateTank(IDictionary<string, string> line)
        {
            return new Tank
            {
                BbblsPerInch = line[TankColumnConstants.BbblsPerInch].ToDecimal(),
                County = line[TankColumnConstants.County],
                MID = line[TankColumnConstants.MID].ToNumber(),
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
                API = line[WellColumnConstants.API].ToNumber(),
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
