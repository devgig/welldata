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

        /// <summary>
        /// Returns all Well data
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<WellModel>> GetAll()
        {
            //mocking async call
            return await Task.FromResult(_wellDbContext.Wells.Select(x => ToModel(x)).ToArray());
        }

        private WellModel ToModel(Well well)
        {
            var model = new WellModel();
            using (model.SuppressNotifyPropertyChange())
            {
                model.Id = well.Id;
                model.Latitude = well.Latitude;
                model.Longitude = well.Longitude;
                model.LeaseOrWellName = well.LeaseOrWellName;
                model.Owner = well.Owner;
                model.Property = well.Property;
               return model;
            };


        }
    }
}
