using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WellData.Core.Data.Entities;

namespace WellData.Core.Services.ImportStrategies
{
    public interface IWellDataImportStrategy
    {
        bool CanProcess(string file);

        IEnumerable<Well> Import(string file);
    }
}
