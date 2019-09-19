using System.Collections.Generic;
using System.IO;
using System.Linq;
using WellData.Core.Data.Entities;
using WellData.Core.Data.Extensions;
using WellData.Core.Services.Common;
using WellData.Core.Services.Data;

namespace WellData.Core.Services.ImportStrategies
{
    public class CsvWellDataImporter : IWellDataImportStrategy
    {
        private readonly ICsvFileFactory _csvFileFactory;

        public CsvWellDataImporter(ICsvFileFactory csvFileFactory)
        {
            _csvFileFactory = csvFileFactory;
        }

        public bool CanProcess(string file)
        {
            if (file == null) return false;
            return Path.GetExtension(file) == ".csv";
        }

        public IEnumerable<Well> Import(string file)
        {
            if (file == null) return Enumerable.Empty<Well>();

            var csv = _csvFileFactory.NewCsvFile(file);

            var wells = new Dictionary<string, Well>();

            foreach (var line in csv.LazyRead())
            {
                //assumed to be the key for a well
                var api = line[WellColumnConstants.API];

                if (wells.ContainsKey(api))
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
            var items = wells.Select(x => x.Value).ToArray();
            if (!items.Any())
                return Enumerable.Empty<Well>();

            return items;
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
