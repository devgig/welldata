using Autofac;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;
using WellData.Bootstrap.Assemblies;
using WellData.Core.Data;
using WellData.Core.Services.ImportStrategies;

namespace WellData.Core.Services.Tests
{
    public class TestBootstrapper
    {
        public IContainer Configure(ContainerBuilder builder)
        {
            var context = new DbContextOptionsBuilder<WellDataDbContext>();
            context.UseInMemoryDatabase("WellList");

            builder.RegisterType<WellDataDbContext>().WithParameter("options", context.Options);


            //register all interfaces to concretes by name convention except those excluded from scan
            foreach (var assembly in WellDataAssemblies.GetAllAssemblies())
                builder.RegisterAssemblyTypes(assembly)
                    .Where(t => t.CustomAttributes.All(s => !s.AttributeType.Name.Contains("ExcludeFromScan")))
                    .AsImplementedInterfaces();

            
           
            //register strategy pattern
            builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(IWellDataImportStrategy)))
                  .Where(t => typeof(IWellDataImportStrategy).IsAssignableFrom(t))
                  .AsSelf();

            builder.Register<Func<string, IWellDataImportStrategy>>(c =>
            {

                var types = c.ComponentRegistry.Registrations
                 .Where(r => typeof(IWellDataImportStrategy).IsAssignableFrom(r.Activator.LimitType))
                 .Select(r => r.Activator.LimitType);

                IWellDataImportStrategy[] lst = types.Select(t => c.Resolve(t) as IWellDataImportStrategy).ToArray();

                return (file) =>
                {
                    return lst.FirstOrDefault(x => x.CanProcess(file));
                };
            });

            return builder.Build();
        }

    }
}
