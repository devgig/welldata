using ExcelDataReader;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using WellData.Core.Data.Entities;
using WellData.Core.Extensions;
using WellData.Core.Services.Data;

namespace WellData.Core.Services.ImportStrategies
{
    public class XlsxWellDataImporter : IWellDataImportStrategy
    {
        public bool CanProcess(string file)
        {
            if (file == null) return false;
            return Path.GetExtension(file) == ".xlsx";
        }

        public IEnumerable<Well> Import(string file)
        {

            var wells = new Dictionary<double, Well>();
            var list = new List<WellData>();
            DataSet ds;

            using (var stream = File.Open(file, FileMode.Open, FileAccess.Read))
            {
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    ds = reader.AsDataSet(new ExcelDataSetConfiguration()
                    {
                        ConfigureDataTable = (tableReader) => new ExcelDataTableConfiguration()
                        {
                            UseHeaderRow = true
                        }
                    });
                }
            }

            foreach (DataRow row in ds.Tables[0].Rows)
            {
                var data = new WellData
                {
                    API = row[WellColumnConstants.API].ToDouble(),
                    Latitude = row[WellColumnConstants.Latitude].ToDecimal(),
                    Longitude = row[WellColumnConstants.Longitude].ToDecimal(),
                    Owner = row[WellColumnConstants.Owner]?.ToString(),
                    Property = row[WellColumnConstants.Property].ToNumber(),
                    LeaseOrWellName = row[WellColumnConstants.Name]?.ToString(),
                    TankMID = row[TankColumnConstants.MID].ToNumber(),
                    TankName = row[TankColumnConstants.Name]?.ToString(),
                    TankSize = row[TankColumnConstants.Size].ToDecimal(),
                    TWP = row[TankColumnConstants.TWP]?.ToString(),
                    SEC = row[TankColumnConstants.SEC].ToNumber(),
                    RNG = row[TankColumnConstants.RNG]?.ToString(),
                    TankNumber = row[TankColumnConstants.Number].ToNumber(),
                    County = row[TankColumnConstants.County]?.ToString(),
                    BbblsPerInch = row[TankColumnConstants.BbblsPerInch].ToDecimal()
                };

                list.Add(data);
            }

            foreach (var line in list)
            {
                //assumed to be the key for a well
                if (wells.ContainsKey(line.API))
                {
                    wells[line.API].Tanks.Add(PopulateTank(line));
                }
                else
                {
                    var well = PopulateWell(line);
                    well.Tanks.Add(PopulateTank(line));
                    wells.Add(line.API, well);
                }
            }
            var items = wells.Select(x => x.Value).ToArray();
            if (!items.Any())
                return Enumerable.Empty<Well>();

            return items;

        }

        private Tank PopulateTank(WellData line)
        {
            return new Tank
            {
                //assuming the MID is the Tank Id
                Id = line.TankMID,
                BbblsPerInch = line.BbblsPerInch,
                County = line.County,
                Name = line.TankName,
                Number = line.TankNumber,
                RNG = line.RNG,
                SEC = line.SEC,
                Size = line.TankSize,
                TWP = line.TWP
            };

        }
        private Well PopulateWell(WellData line)
        {
            return new Well
            {
                //assuming the API # is the Well Id
                Id = line.API,
                Latitude = line.Latitude,
                Longitude = line.Longitude,
                Owner = line.Owner,
                Property = line.Property,
                LeaseOrWellName = line.LeaseOrWellName,
                Tanks = new List<Tank>()
            };

        }

    }

    public class WellData
    {
        public string Owner { get; set; }


        public double API { get; set; }

        public decimal Longitude { get; set; }

        public decimal Latitude { get; set; }

        public int Property { get; set; }

        public string LeaseOrWellName { get; set; }

        public int TankMID { get; set; }

        public string TankName { get; set; }

        public int TankNumber { get; set; }

        public decimal TankSize { get; set; }

        public decimal BbblsPerInch { get; set; }

        public int SEC { get; set; }

        public string TWP { get; set; }

        public string RNG { get; set; }

        public string County { get; set; }

    }

}
