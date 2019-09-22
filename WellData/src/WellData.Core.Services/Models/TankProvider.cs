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
        Task<IEnumerable<TankModel>> GetByWellId(string wellId);
        Task<int> Save(IEnumerable<TankModel> tanks);
    }
    public class TankProvider : ITankProvider
    {
        private readonly WellDataDbContext _wellDbContext;

        public TankProvider(WellDataDbContext wellDbContext)
        {
            _wellDbContext = wellDbContext;
        }

        /// <summary>
        /// Returns tanks by well id
        /// </summary>
        /// <param name="wellId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TankModel>> GetByWellId(string wellId)
        {
            //mocking async calls
            var tanks = _wellDbContext.Tanks.Where(x => x.WellId == wellId).ToArray();

            if (tanks == null || !tanks.Any())
                return await Task.FromResult(Enumerable.Empty<TankModel>());

            return await Task.FromResult(tanks.Select(x => ToModel(x)).ToArray());
        }

        /// <summary>
        /// Save tanks
        /// </summary>
        /// <param name="tanks">Number saved</param>
        /// <returns></returns>
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
            var model = new TankModel();
            using(model.SuppressNotifyPropertyChange())
            {
                model.Id = tank.Id;
                model.BbblsPerInch = tank.BbblsPerInch;
                model.County = tank.County;
                model.Name = tank.Name;
                model.Number = tank.Number;
                model.RNG = tank.RNG;
                model.SEC = tank.SEC;
                model.Size = tank.Size;
                model.TWP = tank.TWP;
                model.WellId = tank.WellId;
                //sets dirty data tracking
                model.Clean();
                return model;
            }
        }
    }
}
