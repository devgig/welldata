using System.Collections.Generic;
using System.Reflection;
using WellData.Core.Services;

namespace WellData.Bootstrap.Assemblies
{
    public class WellDataBackendAssemblies
    {
        public static IEnumerable<Assembly> GetBackendAssemblies()
        {
            return new[]
            {
                typeof( WellDataCoreServicesRegistry ).Assembly
            };
        }
    }
}
