using SimpleExcelImport;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WellData.Core.Data.Entities;
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
            try
            {
                var data = File.ReadAllBytes(file);
                var import = new ImportFromExcel();
                import.LoadXlsx(data);
                //first parameter it's the sheet number in the excel workbook
                //second parameter it's the number of rows to skip at the start(we have an header in the file)
                List<WellData> output = import.ExcelToList<WellData>(0, 1);

                var wells = new Dictionary<double, Well>();

                foreach (var line in output)
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
            catch(Exception ex)
            {
                return Enumerable.Empty<Well>();
            }
           
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
        [ExcelImport(WellColumnConstants.Owner, order = 1)]
        public string Owner { get; set; }


        [ExcelImport(WellColumnConstants.API, order = 2)]
        public double API {get; set; }

        [ExcelImport(WellColumnConstants.Longitude, order = 3)]
        public decimal Longitude { get; set; }

        [ExcelImport(WellColumnConstants.Latitude, order = 4)]
        public decimal Latitude { get; set; }

        [ExcelImport(WellColumnConstants.Property, order = 5)]
        public int Property { get; set; }

        [ExcelImport(WellColumnConstants.Name, order = 6)]
        public string LeaseOrWellName { get; set; }

        [ExcelImport(TankColumnConstants.MID, order = 7)]
        public int TankMID { get; set; }

        [ExcelImport(TankColumnConstants.Name, order = 8)]
        public string TankName { get; set; }

        [ExcelImport(TankColumnConstants.Number, order = 9)]
        public int TankNumber { get; set; }

        [ExcelImport(TankColumnConstants.Size, order = 10)]
        public decimal TankSize { get; set; }

        [ExcelImport(TankColumnConstants.BbblsPerInch, order = 11)]
        public decimal BbblsPerInch { get; set; }

        [ExcelImport(TankColumnConstants.SEC, order = 12)]
        public int SEC { get; set; }

        [ExcelImport(TankColumnConstants.TWP, order = 13)]
        public string TWP { get; set; }

        [ExcelImport(TankColumnConstants.RNG, order = 14)]
        public string RNG { get; set; }

        [ExcelImport(TankColumnConstants.County, order = 15)]
        public string County { get; set; }


    }

}
