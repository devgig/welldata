using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace WellData.Bootstrap.Assemblies
{
    public class WellDataAssemblies
    {
        public static IEnumerable<Assembly> GetAllAssemblies()
        {
            return WellDataBackendAssemblies.GetBackendAssemblies().Union(WellDataUiAssemblies.GetUiAssemblies());
        }
    }
}
