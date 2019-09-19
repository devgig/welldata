using Autofac;
using Caliburn.Micro;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using WellData.Bootstrap.Assemblies;
using WellData.Core.Data;
using IContainer = Autofac.IContainer;

namespace WellData
{
    public class WellDataBootstrapper
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

            foreach (var assembly in WellDataUiAssemblies.GetUiAssemblies())
                RegisterMvvm(builder, assembly);


            //  register the single window manager for this container
            builder.RegisterType<WindowManager>().As<IWindowManager>().SingleInstance();

            //  register the single event aggregator for this container
            //builder.Register<IEventAggregator>(c => new EventAggregator()).InstancePerLifetimeScope();


            return builder.Build();
        }


        private void RegisterMvvm(ContainerBuilder builder, Assembly assembly)
        {
           
            //  register view models
            builder.RegisterAssemblyTypes(assembly)
              //  must be a type that ends with ViewModel
              .Where(type => type.Name.EndsWith("ViewModel"))
              .Where(type => type.GetInterface(typeof(INotifyPropertyChanged).Name) != null)
              .AsSelf()
              //  always create a new one
              .InstancePerDependency();

            //  register views
            builder.RegisterAssemblyTypes(assembly)
              //  must be a type that ends with View
              .Where(type => type.Name.EndsWith("View"))
              .AsSelf()
              //  always create a new one
              .InstancePerDependency();

        }
    }
}
