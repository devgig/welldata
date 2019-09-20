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
        Task<int> Save(IEnumerable<TankModel> tanks);
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

        public async Task<int> Save(IEnumerable<TankModel> tanks)
        {
            var ids = tanks.Select(x => x.Id);
            var entities = _wellDbContext.Tanks.Where(x => ids.Contains(x.Id)).ToArray();

            var entityDict = entities.ToDictionary(x => x.Id);
            foreach(var tank in tanks)
            {
                if(entityDict.ContainsKey(tank.Id))
                CopyToEntity(entityDict[tank.Id], tank);
            }

            _wellDbContext.Tanks.UpdateRange(entityDict.Select(x => x.Value).ToArray());

            return await _wellDbContext.SaveChangesAsync();
        }

        private void CopyToEntity(Tank entity, TankModel tank)
        {
            entity.BbblsPerInch = tank.BbblsPerInch;
            entity.County = tank.County;
            entity.Name = tank.Name;
            entity.Number = tank.Number;
            entity.RNG = tank.RNG;
            entity.SEC = tank.SEC;
            entity.Size = tank.Size;
            entity.TWP = tank.TWP;
            
        }

        private TankModel ToModel(Tank tank)
        {
            var model = new TankModel
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
            model.Clean();
            return model;

        }
    }
}
