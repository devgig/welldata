using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WellData.Core.Data;
using WellData.Core.Data.Entities;

namespace WellData.Core.Services.Models
{
    public interface IWellProvider
    {
        Task<IEnumerable<WellModel>> GetAll();
    }
    public class WellProvider : IWellProvider
    {
        private readonly WellDataDbContext _wellDbContext;

        public WellProvider(WellDataDbContext wellDbContext)
        {
            _wellDbContext = wellDbContext;
        }
        public async Task<IEnumerable<WellModel>> GetAll()
        {
            //mocking async call
            return await Task.FromResult(_wellDbContext.Wells.Select(x => ToModel(x)).ToArray());
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
