using System.Collections.Generic;
using System.Linq;
using WellData.Core.Data;
using WellData.Core.Data.Entities;

namespace WellData.Core.Services.Models
{
    public interface IWellProvider
    {
        IEnumerable<WellModel> GetAll();
    }
    public class WellProvider : IWellProvider
    {
        private readonly WellDataDbContext _wellDbContext;

        public WellProvider(WellDataDbContext wellDbContext)
        {
            _wellDbContext = wellDbContext;
        }
        public IEnumerable<WellModel> GetAll()
        {
            return _wellDbContext.Wells.Select(x => ToModel(x)).ToArray();
        }

        private WellModel ToModel(Well well)
        {
            return new WellModel
            {
                Id = well.Id,
                Latitude = well.Latitude,
                Longitude = well.Longitude,
                LeaseOrWellName = well.LeaseOrWellName,
                Owner = well.Owner,
                Property = well.Property,
            };
        }
    }
}
