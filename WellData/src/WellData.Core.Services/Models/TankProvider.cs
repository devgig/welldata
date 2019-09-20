using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellData.Core.Data;
using WellData.Core.Data.Entities;

namespace WellData.Core.Services.Models
{
    public interface ITankProvider
    {
        Task<IEnumerable<TankModel>> GetByWellId(double wellId);
    }
    public class TankProvider : ITankProvider
    {
        private readonly WellDataDbContext _wellDbContext;

        public TankProvider(WellDataDbContext wellDbContext)
        {
            _wellDbContext = wellDbContext;
        }
        public async Task<IEnumerable<TankModel>> GetByWellId(double wellId)
        {
            //mocking async calls
            var tanks = _wellDbContext.Tanks.Where(x => x.WellId == wellId).ToArray();

            if (tanks == null || !tanks.Any())
                return await Task.FromResult(Enumerable.Empty<TankModel>());

            return await Task.FromResult(tanks.Select(x => ToModel(x)).ToArray());
        }

        private TankModel ToModel(Tank tank)
        {
            return new TankModel
            {
                Id = tank.Id,
                BbblsPerInch = tank.BbblsPerInch,
                County = tank.County,
                Name = tank.Name,
                Number = tank.Number,
                RNG = tank.RNG,
                SEC = tank.SEC,
                Size = tank.Size,
                TWP = tank.TWP,
                WellId = tank.WellId
            };

        }
    }
}
