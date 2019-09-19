using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using WellData.Core.Common;

namespace WellData.Ui.Common
{
    /// <summary>
    ///   Monitors the PropertyChanged event of an object that implements INotifyPropertyChanged,
    ///   and executes callback methods (i.e. handlers) registered for properties of that object.
    /// </summary>
    /// <typeparam name = "TPropertySource">The type of object to monitor for property changes.</typeparam>
    public class PropertyObserver<TPropertySource> : IWeakEventListener
        where TPropertySource : INotifyPropertyChanged
    {
        private readonly HashtableList<string, Action<TPropertySource>> _propertyNameToHandlerMap;
        private readonly TPropertySource _propertySource;

        /// <summary>
        ///   Initializes a new instance of PropertyObserver, which
        ///   observes the 'propertySource' object for property changes.
        /// </summary>
        /// <param name = "propertySource">The object to monitor for property changes.</param>
        public PropertyObserver(TPropertySource propertySource)
        {
            if (propertySource == null)
                throw new ArgumentNullException("propertySource");

            _propertySource = propertySource;
            _propertyNameToHandlerMap = new HashtableList<string, Action<TPropertySource>>();
        }

        /// <summary>
        ///   Registers a callback to be invoked when the PropertyChanged event has been raised for the specified property.
        /// </summary>
        /// <param name = "propertyName">The name of the property to monitor.</param>
        /// <param name = "handler">The callback to invoke when the property has changed.</param>
        /// <returns>The object on which this method was invoked, to allow for multiple invocations chained together.</returns>
        public PropertyObserver<TPropertySource> RegisterHandler(string propertyName, Action<TPropertySource> handler)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentException("'propertyName' cannot be null or empty.");

            VerifyPropertyName(propertyName);

            if (handler == null)
                throw new ArgumentNullException("handler");

            _propertyNameToHandlerMap.Add(propertyName, handler);
            PropertyChangedEventManager.AddListener(_propertySource, this, propertyName);

            return this;
        }

        public IAfterWhenChangePart OnChangeOf(Expression<Func<TPropertySource, object>> propExpr, params Expression<Func<TPropertySource, object>>[] propExprs)
        {
            var array = propExprs.Prepend(propExpr).Select(x => new Reg { PropertyName = Member.Of(x).ToString() }).ToArray();
            return new AfterWhenChangePart(this, array);
        }

        /// <summary>
        ///   Removes the callback associated with the specified property.
        /// </summary>
        /// <param name = "propertyName">The name of the property for which to remove the handler.</param>
        /// <returns>The object on which this method was invoked, to allow for multiple invocations chained together.</returns>
        public PropertyObserver<TPropertySource> UnregisterHandler(string propertyName)
        {
            if (String.IsNullOrEmpty(propertyName))
                throw new ArgumentException("'propertyName' cannot be null or empty.");

            if (_propertyNameToHandlerMap.ContainsKey(propertyName))
            {
                _propertyNameToHandlerMap.Remove(propertyName);
                PropertyChangedEventManager.RemoveListener(_propertySource, this, propertyName);
            }

            return this;
        }

        public PropertyObserver<TPropertySource> UnregisterHandler(Expression<Func<TPropertySource, object>> propExpr, params Expression<Func<TPropertySource, object>>[] propExprs)
        {
            var array = propExprs.Prepend(propExpr).Select(x => new Reg { PropertyName = Member.Of(x).ToString() }).ToArray();
            foreach (var reg in array)
            {
                UnregisterHandler(reg.PropertyName);
            }

            return this;
        }

        bool IWeakEventListener.ReceiveWeakEvent(Type managerType, object sender, EventArgs e)
        {
            if (managerType != typeof(PropertyChangedEventManager))
                return false;

            var propertyName = ((PropertyChangedEventArgs)e).PropertyName;

            IEnumerable<Action<TPropertySource>> handlers;

            if (!_propertyNameToHandlerMap.TryGetValue(propertyName, out handlers))
                return false;

            foreach (var handler in handlers)
                handler(_propertySource);

            return true;
        }

        [Conditional("DEBUG")]
        private void VerifyPropertyName(string propertyName)
        {
            bool exists = TypeDescriptor.GetProperties(_propertySource)[propertyName] != null;
            if (!exists)
            {
                string msg = String.Format(
                    "Object of type {0} does not have a public property named '{1}'.",
                    typeof(TPropertySource).FullName,
                    propertyName);

                Debug.Fail(msg);
            }
        }

        public interface IAfterWhenChangePart
        {
            IAfterWhenChangePart Do(Action<TPropertySource> action);
        }

        private class AfterWhenChangePart: IAfterWhenChangePart
        {
            private readonly PropertyObserver<TPropertySource> _observer;
            private readonly Reg[] _regs;

            public AfterWhenChangePart(PropertyObserver<TPropertySource> observer, Reg[] regs)
            {
                _observer = observer;
                _regs = regs;
            }

            public IAfterWhenChangePart Do(Action<TPropertySource> action)
            {
                foreach (var reg in _regs)
                {
                    _observer.RegisterHandler(reg.PropertyName, action);
                }
                return this;
            }
        }

        private class Reg
        {
            public string PropertyName { get; set; }
        }
    }

  
}
