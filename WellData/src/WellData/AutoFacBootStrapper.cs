using System;
using System.Collections.Generic;
using System.Windows;
using Caliburn.Micro;
using Autofac;
using WellData.Ui.Screens;
using System.Reflection;
using WellData.Bootstrap.Assemblies;
using WellData.Ui.AutoUpdate;

namespace WellData
{
    public class AutoFacBootStrapper : BootstrapperBase
    {
        private IContainer _container;

        public AutoFacBootStrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = ConfigureContainer();
            _container.Resolve<IAutoUpdater>().HandleUpdateEvents();

        }

        private static IContainer ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            var bootstrap = new WellDataBootstrapper();
            return bootstrap.Configure(builder);

        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
            base.OnStartup(sender, e);
        }

        protected override object GetInstance(Type service, string key)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                if (_container.IsRegistered(service))
                    return _container.Resolve(service);
            }
            else
            {
                if (_container.IsRegisteredWithKey(key, service))
                    return _container.ResolveKeyed(key, service);

            }
            throw new Exception(string.Format("Could not locate any instances of contract {0}.", key ?? service.Name));
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.Resolve(typeof(IEnumerable<>).MakeGenericType(service)) as IEnumerable<object>;
        }

        protected override void BuildUp(object instance)
        {
            _container.InjectProperties(instance);
        }

        protected override IEnumerable<Assembly> SelectAssemblies()
        {
            return WellDataUiAssemblies.GetUiAssemblies();
        }



    }
}
