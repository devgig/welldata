using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using WellData.Ui;

namespace WellData.Bootstrap.Assemblies
{
    public class WellDataUiAssemblies
    {
        public static IEnumerable<Assembly> GetUiAssemblies()
        {
            return new[]
            {
                typeof( WellDataUiRegistry ).Assembly
            };
        }
    }
}
