using Autofac;
using WellData.Bootstrap.Assemblies;

namespace WellData.Tests
{
    public class TestBootstrapper
    {
        public IContainer Configure(ContainerBuilder builder)
        {
            //register all interfaces to concretes by name convention
            foreach (var assembly in WellDataAssemblies.GetAllAssemblies())
                builder.RegisterAssemblyTypes(assembly).AsImplementedInterfaces();

            return builder.Build();
        }

    }
}
