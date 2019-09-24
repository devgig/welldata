using Autofac;
using System;

namespace WellData.Core.Common
{
    public interface IFactory<T>
    {
        T Create();
        T Create(object context);
        T Create<C>(C type);


    }

    [ExcludeFromScan]
    public class Factory<T> : IFactory<T>
    {
        private readonly ILifetimeScope _scope;

        public Factory(ILifetimeScope scope)
        {
            _scope = scope;
        }

        public T Create()
        {
            return _scope.Resolve<T>();
        }

        public T Create(object context)
        {
            var item = _scope.Resolve<Func<object, T>>();
            return item(context);
        }

        public T Create<C>(C type)
        {
            var item = _scope.Resolve<Func<C, T>>();
            return item(type);
        }
    }

    public class Sample
    {
        public delegate Sample Factory(string context);
    }

}
