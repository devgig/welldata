using Autofac;
using System.Reflection;
using Xunit;
using Xunit.Abstractions;
using Xunit.Ioc.Autofac;
using Xunit.Sdk;

[assembly: TestFramework("WellData.Core.Services.Tests.ConfigureTestFramework", "WellData.Core.Services.Tests")]

namespace WellData.Core.Services.Tests
{
    public class ConfigureTestFramework : AutofacTestFramework
    {
        private const string TestSuffixConvention = "Tests";
        public ConfigureTestFramework(IMessageSink diagnosticMessageSink) : base(diagnosticMessageSink)
        {
            var builder = new ContainerBuilder();

            builder.RegisterAssemblyTypes(Assembly.GetExecutingAssembly())
               .Where(t => t.Name.EndsWith(TestSuffixConvention));

            builder.Register(context => new TestOutputHelper())
                .AsSelf()
                .As<ITestOutputHelper>()
                .InstancePerLifetimeScope();


            var bootstrap = new TestBootstrapper();
            Container = bootstrap.Configure(builder);

           
        }
    }
}
