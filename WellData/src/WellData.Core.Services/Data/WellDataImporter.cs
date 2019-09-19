using System;
using System.Threading.Tasks;
using WellData.Core.Data;
using WellData.Core.Services.ImportStrategies;

namespace WellData.Core.Services.Data
{
    public interface IWellDataImporter
    {
        Task<int> Upload(string uploadFile);
    }
    public class WellDataImporter : IWellDataImporter
    {
        private readonly Func<string, IWellDataImportStrategy> _importStrategy;
        private readonly WellDataDbContext _wellDbContext;
        
        public WellDataImporter(Func<string, IWellDataImportStrategy> importStrategy,
            WellDataDbContext wellDbContext)
        {
            _importStrategy = importStrategy;
            _wellDbContext = wellDbContext;
        }

        public async Task<int> Upload(string uploadFile)
        {
            if (uploadFile == null) return await Task.FromResult(0);

            //clear previous items for this app.
            foreach (var item in _wellDbContext.Wells)
                _wellDbContext.Wells.Remove(item);

            await _wellDbContext.SaveChangesAsync();

            //pick appropriate import strategy based on file extension
            var strategy = _importStrategy(uploadFile);
            if (strategy == null) return await Task.FromResult(0);

            var wells = strategy.Import(uploadFile);
                        
            //save well data
            await _wellDbContext.Wells.AddRangeAsync(wells);
            return await _wellDbContext.SaveChangesAsync();
        }

    }
}
