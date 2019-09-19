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
        IEnumerable<TankModel> GetByWellId(double wellId);
    }
    public class TankProvider : ITankProvider
    {
        private readonly WellDataDbContext _wellDbContext;

        public TankProvider(WellDataDbContext wellDbContext)
        {
            _wellDbContext = wellDbContext;
        }
        public IEnumerable<TankModel> GetByWellId(double wellId)
        {
            var tanks = _wellDbContext.Tanks.Where(x => x.WellId == wellId).ToArray();

            if (tanks == null || !tanks.Any())
                return Enumerable.Empty<TankModel>();

            return tanks.Select(x => ToModel(x)).ToArray();
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
