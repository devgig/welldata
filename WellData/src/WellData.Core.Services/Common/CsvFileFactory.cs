using WellData.Core.Services.Data;

namespace WellData.Core.Services.Common
{
    public interface ICsvFileFactory
    {
        ICsvFile NewCsvFile(string filePath);
    }

    public class CsvFileFactory : ICsvFileFactory
    {
        public ICsvFile NewCsvFile(string filePath)
        {
            return new CsvFile(filePath);
        }
    }
}
